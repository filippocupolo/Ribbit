using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.NetworkInformation;
using System.Collections.ObjectModel;

namespace Progetto_2._0

    
{
    public partial class Form1 : Form
    {
        //for the titlebar
        int mouseX = 0, mouseY=0;   
        bool mouseDown;
        bool settingsShow=false;
        OpenFileDialog ofd = new OpenFileDialog();
        


        Options options = new Options();

        private int portTCP;

        private ClientUDP clientUDP;
        private Thread UDPClientThread;

        private ServerTCP serverTCP;
        private Thread TCPServerThread;

        private ServerUDP serverUDP;
        private Thread UDPServerThread;

        private ObservableCollection<User> userCollection;
        private HashSet<Thread> threadList;

        public delegate void AddRemoveChangeItem(User user);
        public delegate void CloseThread(Thread thread);
        public CloseThread CloseThreadDelegate;
        public AddRemoveChangeItem addItemDelegate;
        public AddRemoveChangeItem removeItemDelegate;
        public AddRemoveChangeItem changeItemDelegate;

        //for the icon
        private System.Windows.Forms.NotifyIcon shareIcon;
        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.ComponentModel.IContainer componenti;

        public Form1()
        {
            try { 
                InitializeComponent();
                //Initialize_Settings
                settingsPanel.Visible = false;
                //testing
                options.RicMode = true;
                
                
                threadList = new HashSet<Thread>();
                
                //inizializzo serverUDP
                serverUDP = new ServerUDP(this);
                UDPServerThread = new Thread(serverUDP.Execute);
                
                if (!options.PrivateMode)
                {
                    //get a free port
                    if (!FindPort(ref portTCP))
                    {
                        //there are no free ports so close the program
                        RealClose();
                    }

                    //inizializzo clientUDP
                    clientUDP = new ClientUDP(options.Name, portTCP,this);
                    UDPClientThread = new Thread(clientUDP.Execute);
                   
                    //inizializzo serverTCP
                    serverTCP = new ServerTCP(portTCP, options.DestPath, options.RicMode,this);
                    TCPServerThread = new Thread(serverTCP.Execute);
                    
                }

                userCollection = new ObservableCollection<User>();
                addItemDelegate = new AddRemoveChangeItem(this.AddItemMethod);
                removeItemDelegate= new AddRemoveChangeItem(this.RemoveItemMethod);
                changeItemDelegate= new AddRemoveChangeItem(this.ChangeItemMethod);
                CloseThreadDelegate= new CloseThread (this.CloseThreadMethod);


                //Thread.Sleep(10000);
                
                nameUser.Name = "nameUser";
                nameUser.Text = options.Name;
                lName.Text = options.Name;

                //setto le due checkbox
                privateUserP.Size = new Size(60, 25);
                privateUserP.BackColor = Color.FromArgb(220, 220, 220);
                privateUser.Size = new Size(45, 45);
                privateUser.Name = "privateUser";
                privateUser.Appearance = Appearance.Button;
                privateUser.FlatStyle = FlatStyle.Flat;
                privateUser.FlatAppearance.BorderSize = 0;
                privateUser.Checked = options.PrivateMode;

                ricModeP.Size = new Size(60,25);
                ricModeP.BackColor = Color.FromArgb(220,220,220);
                ricMode.Size = new Size(45, 45);
                ricMode.Name = "lRicMode";
                ricMode.Appearance = Appearance.Button;
                ricMode.FlatStyle = FlatStyle.Flat;
                ricMode.FlatAppearance.BorderSize = 0;
                ricMode.Checked = options.RicMode;
                checkboxApparence(ricMode);
                checkboxApparence(privateUser);

                destPath.Name = "destPath";
                destPath.Text = options.DestPath;
                destPath.AutoSize = true;

                //icon
                this.componenti = new System.ComponentModel.Container();
                this.contextMenu1 = new System.Windows.Forms.ContextMenu();
                this.menuItem1 = new System.Windows.Forms.MenuItem();

                // Initialize contextMenu1
                this.contextMenu1.MenuItems.AddRange(
                new System.Windows.Forms.MenuItem[] { this.menuItem1 });

                // Initialize menuItem1
                this.menuItem1.Index = 0;
                this.menuItem1.Text = "E&sci";
                this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
                
                // Create the NotifyIcon.
                this.shareIcon = new System.Windows.Forms.NotifyIcon(this.componenti);
                shareIcon.Visible = false;
                shareIcon.Icon = new Icon("../../img/share.ico");

                shareIcon.ContextMenu = this.contextMenu1;
                shareIcon.Text = "Ribbit";
                shareIcon.DoubleClick += new System.EventHandler(this.shareIcon_DoubleClick);
                
                //test
                //IPAddress iptest = IPAddress.Parse("121.212.211.212");
                //userCollection.Add(new User("ciao", new IPEndPoint(iptest, 200)));
                //userCollection.Add(new User("culo", new IPEndPoint(iptest, 220)));
                //userCollection.Add(new User("ciccio", new IPEndPoint(iptest, 210)));
                Print();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

      
        private void changeName()
        {
            try
            {
                //check if is alive
                clientUDP.Name = nameUser.Text;
                lName.Text = nameUser.Text;
                options.Name = nameUser.Text;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = "";
                string filePath = "";
                if (userList.SelectedIndex == -1)
                {
                    MessageBox.Show("select a user first!");
                }
                else
                {

                    int  index = userList.SelectedIndex;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        //path+nome
                        filePath = ofd.FileName;

                        //path
                        fileName = ofd.SafeFileName;
                        Send(filePath, false, userCollection[index].IP);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                
             
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void RealClose() {
            shareIcon.Visible = false;
            try
            {
                //hide form
                Hide(); //oppure metterla in trasparenza senza che si possa premere più nulla

                //close all threads
                serverUDP.CloseThread();
                if (!options.PrivateMode)
                {
                    clientUDP.CloseThread();
                    serverTCP.CloseThread();
                }
                
                //join all living threads
                foreach (Thread thread in threadList)
                {
                    thread.Join();
                }

                //exit
                Application.Exit();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        private void Send(string filePath, bool IsFolder, IPEndPoint ip)
        {
            try
            {
                ClientTCP clientTCP = new ClientTCP(filePath, IsFolder, options.Name, ip, this);
                Thread TCPClientThread = new Thread(clientTCP.Execute);
                TCPClientThread.Start();
                threadList.Add(TCPClientThread);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void AddItemMethod(User user) {
            try
            {
                
                userCollection.Add(user);
                userList.SelectionMode = SelectionMode.MultiExtended;
                userList.DataSource = userCollection;
                userList.SelectionMode = SelectionMode.One;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        private void RemoveItemMethod(User user) {
            try
            {
                userCollection.Remove(user);
                userList.SelectionMode = SelectionMode.MultiExtended;
                userList.DataSource = null;
                Print();
                userList.SelectionMode = SelectionMode.One;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void ChangeItemMethod(User user) {
            try
            {
                bool ok = false;
                for (int i = 0; i < userCollection.Count; i++)
                {
                    if (userCollection[i].IP.Address == user.IP.Address)
                    {
                        userCollection[i] = user;
                        ok = true;
                        break;
                    }
                }

                if (!ok)
                {
                    //errore
                }

                userList.SelectionMode = SelectionMode.MultiExtended;
                userList.DataSource = userCollection;
                userList.SelectionMode = SelectionMode.One;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void Print() {

            userList.DataSource = new BindingSource(userCollection, null);
            userList.ValueMember = "IP.Address";
            userList.DisplayMember = "Name";
            this.Controls.Add(userList);
            
        }

        private void CloseThreadMethod(Thread thread)
        {
            try
            {
                thread.Join();
                if (!threadList.Remove(thread))
                {
                    Console.WriteLine("ERROR thread is not in threadList");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private bool CheckAvailableServerPort(int port)
        {
            bool isAvailable = true;

            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpListeners();
            IPEndPoint[] udpConnInfoArray = ipGlobalProperties.GetActiveUdpListeners();

            foreach (IPEndPoint endpoint in tcpConnInfoArray)
            {
                if (endpoint.Port == port)
                {
                    isAvailable = false;
                    break;
                }
            }

            foreach (IPEndPoint endpoint in udpConnInfoArray)
            {
                if (endpoint.Port == port)
                {
                    isAvailable = false;
                    break;
                }
            }

            return isAvailable;
        }

        private bool FindPort(ref int port)
        {
            bool found = false;
            for(port = 2000; port <= IPEndPoint.MaxPort; port++){
                if (CheckAvailableServerPort(port))
                {
                    found = true;
                    break;
                }
            }

            return found;
        }
        //-----------------------------------------Title Bar-----------------------------------
        private void TitleBar_Paint(object sender, PaintEventArgs e)
        {

        }

        private void TitleBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                mouseX = MousePosition.X-20;
                mouseY = MousePosition.Y-20;
                this.SetDesktopLocation(mouseX,mouseY);
            }
        }

        private void TitleBar_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void TitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
        }

      

        private void closeButton_Click(object sender, EventArgs e)
        {
            try
            {
               Hide();
               shareIcon.Visible = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

     //-------------------------settingsBar----------------------------------------
        private void settingsButton_Click(object sender, EventArgs e)
        {
            if (settingsShow == false)
            {
                settingsPanel.Visible = true;
                settingsShow = true;
            }
            else{
                settingsPanel.Visible = false;
                settingsShow = false;

            }
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
         
        }

        private void changePath_Click_1(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = saveFolder.ShowDialog();
                if (result == DialogResult.OK)
                {
                    options.DestPath = saveFolder.SelectedPath;
                    destPath.Text = options.DestPath;
                    //un po' di controlli
                    if (!options.PrivateMode)
                    {
                        serverTCP.PathDest = saveFolder.SelectedPath;
                    }

                }
             
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void checkboxApparence(CheckBox c)
        {
            if (c.Checked)
            {
                c.Text = "ON";
                c.BackColor = Color.FromArgb(150, 255, 0);
                c.Dock = DockStyle.Left;
            }
            else
            {
                c.Text = "OFF";
                c.BackColor = Color.Red;
                c.Dock = DockStyle.Right;
            }

        }

        private void ricMode_CheckedChanged(object sender, EventArgs e)
        {
            try { 
                options.RicMode = ricMode.Checked;
                serverTCP.AutomaticAnswer = ricMode.Checked;
                checkboxApparence(ricMode);
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        private void privateUser_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (privateUser.Checked)
                {
                    options.PrivateMode = true;

                    //close threads
                    clientUDP.CloseThread();
                    serverTCP.CloseThread();
                }
                else
                {
                    //get a free port
                    if (!FindPort(ref portTCP))
                    {
                        //there are no free ports so close the program
                        RealClose();
                    }

                    options.PrivateMode = false;

                    //inizializzo serverTCP
                    serverTCP = new ServerTCP(portTCP, options.DestPath, options.RicMode, this);
                    TCPServerThread = new Thread(serverTCP.Execute);
                    TCPServerThread.Start();
                    threadList.Add(TCPServerThread);

                    //inizializzo clientUDP
                    clientUDP = new ClientUDP(options.Name, portTCP, this);
                    UDPClientThread = new Thread(clientUDP.Execute);
                    UDPClientThread.Start();
                    threadList.Add(UDPClientThread);

                }
                checkboxApparence(privateUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        private void nameUser_TextChanged(object sender, EventArgs e)
        {

        }

        private void nameUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                changeName();
                lName.Focus();
            }
        }

        //------------------------------------icon stuff-----------------------
        private void shareIcon_DoubleClick(object Sender, EventArgs e) {
            try
            {
                this.Show();

            }catch(Exception ex)
            {Console.WriteLine(ex.ToString());}
            
        }

        private void userList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            /*This method is called after the form is created the first time.
             *Launch the thread to avoid that a invoke is sent before the for is created.
             */

            //start server UDP thread
            UDPServerThread.Start();
            threadList.Add(UDPServerThread);

            //if not private mode start server TCP and client UDP threads
            if (!options.PrivateMode)
            {
                UDPClientThread.Start();
                threadList.Add(UDPClientThread);

                TCPServerThread.Start();
                threadList.Add(TCPServerThread);
            }
        }

        private void menuItem1_Click(object Sender, EventArgs e) {
            RealClose();
        }


        
        
    }
}


