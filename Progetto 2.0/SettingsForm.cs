using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Net.Sockets;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO.Pipes;
using System.Net.NetworkInformation;

namespace Progetto_2._0
{
    public partial class SettingsForm : Form
    {
        //title bar
        int mouseX = 0, mouseY = 0;
        bool mouseDown;
        Label ProgramName = new Label();
        Panel TitleBar = new Panel();
        Button CloseButton = new Button();

        NotifyIcon NoIcon = new NotifyIcon();
        private System.Windows.Forms.ContextMenu IconMenu;
        private System.Windows.Forms.MenuItem QuitIcon;
        private System.Windows.Forms.MenuItem SettingsIcon;
        private System.ComponentModel.IContainer Components;
        private String path = null;
        private Options options;
        private int portTCP;
        private List<User> userList;
        private Dictionary<Thread,SharingForm> threadFormList;
        private ConcurrentDictionary<SharingForm,String> sharingFormList;
        private Boolean realClose = false;

        private ClientUDP clientUDP;
        private Thread UDPClientThread;

        private ServerTCP serverTCP;
        private Thread TCPServerThread;

        private ServerUDP serverUDP;
        private Thread UDPServerThread;

        private NamedPipeServerStream pipe;
        
        //set delegates
        public delegate void CloseThread(Thread thread, String labol);
        public delegate void DownloadState(String message, Boolean error);

        public CloseThread CloseThreadDelegate;
        public DownloadState DownloadStateDelegate;
        public SettingsForm(String path)
        {
            this.options = new Options();
            userList = new List<User>();
            threadFormList = new Dictionary<Thread, SharingForm>();
            sharingFormList = new ConcurrentDictionary<SharingForm, String>();

            InitializeComponent();
            EditTitleBar();
            SetNoIcon();

            UserName.Text = options.Name;
            DestPath.Text = options.DestPath;

            AutomaticReceptionP.Size = new Size(60, 25);
            System.Windows.Forms.ToolTip ToolTip1 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(this.AutomaticReception, "autoRec Info");
            
            AutomaticReceptionP.BackColor = Color.FromArgb(220, 220, 220);
            PrivateModeP.BackColor = Color.FromArgb(220, 220, 220);
            
            AutomaticReception.Size = new Size(45, 45);
            AutomaticReception.Appearance = Appearance.Button;
            AutomaticReception.FlatStyle = FlatStyle.Flat;
            AutomaticReception.FlatAppearance.BorderSize = 0;
            AutomaticReception.Checked = options.RicMode;

            PrivateModeP.Size = new Size(60, 25);
            System.Windows.Forms.ToolTip ToolTip2 = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(this.PrivateMode, "Private mode Info");

            PrivateMode.Size = new Size(45, 45);
            PrivateMode.Appearance = Appearance.Button;

            PrivateMode.FlatStyle = FlatStyle.Flat;
            
            PrivateMode.FlatAppearance.BorderSize = 0;
            PrivateMode.Checked = options.PrivateMode;

            CheckBoxApparence(PrivateMode);
            CheckBoxApparence(AutomaticReception);

            //set changeNetwork callback
            NetworkChange.NetworkAddressChanged += new NetworkAddressChangedEventHandler(AddressChangedCallback);

            //set delegates
            CloseThreadDelegate = new CloseThread(this.CloseThreadMethod);
            DownloadStateDelegate = new DownloadState(this.notifyFile);

            DestPath.Text = options.DestPath;
            DestPath.AutoEllipsis = true;

            System.Windows.Forms.ToolTip ToolTip = new System.Windows.Forms.ToolTip();
            ToolTip1.SetToolTip(this.CP_Button, "Change Path of the receiving folder");

            if (path != null)
            {
                //hide settingsForm and launch SharingForm

                this.path = path;
            }
        }

        private void SettingsForm_Load(object sender, EventArgs e){

            
            

            //set Pipe
            pipe = new NamedPipeServerStream("RibbitPipe", PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
            pipe.BeginWaitForConnection(PipeCallBack, pipe);

            //Initialize serverUDP
            serverUDP = new ServerUDP(userList, sharingFormList, this);
            UDPServerThread = new Thread(serverUDP.Execute);
            UDPServerThread.Start();



            //if not private mode initialize serverTCP and clientUDP
            if (!options.PrivateMode)
            {
                //get a free port
                if (!Utilities.FindPort(ref portTCP))
                {
                    //there are no free ports so close the program
                    RealClose();
                }

                //Initialize serverTCP
                serverTCP = new ServerTCP(portTCP, options.DestPath, options.RicMode, this);
                TCPServerThread = new Thread(serverTCP.Execute);
                TCPServerThread.Start();

                //Initialize clientUDP
                clientUDP = new ClientUDP(options.Name, portTCP, this);
                UDPClientThread = new Thread(clientUDP.Execute);
                UDPClientThread.Start();
            }

            if (this.path != null)
            {
                this.LaunchSharingFormMethod(path);
            }
            
        }

        void EditTitleBar()
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            TitleBar.Name = "titleBar";
            TitleBar.Dock = DockStyle.Top;
            TitleBar.Height = 25;
            TitleBar.BackColor = Color.Gray;
            TitleBar.MouseMove += new MouseEventHandler(TitleBar_MouseMove);
            TitleBar.MouseUp += new MouseEventHandler(this.TitleBar_MouseUp);
            TitleBar.MouseDown += new MouseEventHandler(this.TitleBar_MouseDown);

            CloseButton.Name = "closeButton";
            CloseButton.Text = "X";
            CloseButton.ForeColor = Color.WhiteSmoke;
            CloseButton.Dock = DockStyle.Right;
            CloseButton.FlatStyle = FlatStyle.Flat;


            CloseButton.Size = new Size(25, 25);
            CloseButton.BackColor = Color.IndianRed;
            CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            CloseButton.MouseHover += new System.EventHandler(this.CloseButton_MouseHover);

            ProgramName.Name = "programName";
            ProgramName.Text = "Ribbit";
            ProgramName.ForeColor = Color.WhiteSmoke;
            ProgramName.AutoSize = true;
            ProgramName.Location = new Point(25, 8);
            
            TitleBar.Controls.Add(CloseButton);
            TitleBar.Controls.Add(ProgramName);
            this.Controls.Add(TitleBar);
        }

        private void CloseButton_MouseHover(object sender, EventArgs e)
        {
            CloseButton.BackColor = Color.Red;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Hide();
          
        }

        private void TitleBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                mouseX = MousePosition.X - 20;
                mouseY = MousePosition.Y - 20;
                this.SetDesktopLocation(mouseX, mouseY);
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

        void SetNoIcon() {

            this.Components = new System.ComponentModel.Container();
            this.IconMenu = new System.Windows.Forms.ContextMenu();
            this.QuitIcon = new System.Windows.Forms.MenuItem();
            this.SettingsIcon = new System.Windows.Forms.MenuItem();

            // Initialize IconMenu
            this.IconMenu.MenuItems.AddRange(
            new System.Windows.Forms.MenuItem[] { this.QuitIcon, this.SettingsIcon });

            //initialize SettingsIcon
            this.SettingsIcon.Index = 0;
            this.SettingsIcon.Text = "S&ettings";
            this.SettingsIcon.Click += new System.EventHandler(this.SettingsIcon_Click);

            //initialize QuitIcon
            this.QuitIcon.Index = 1;
            this.QuitIcon.Text = "Q&uit";
            this.QuitIcon.Click += new System.EventHandler(this.QuitIcon_Click);

            NoIcon = new System.Windows.Forms.NotifyIcon(this.Components);

            NoIcon.Icon = new Icon(AppDomain.CurrentDomain.BaseDirectory + "/../../img/share2.ico");
            NoIcon.ContextMenu = this.IconMenu;
            NoIcon.Text = "Ribbit";
            NoIcon.DoubleClick += new System.EventHandler(this.NoIcon_DoubleClick);
            NoIcon.Visible = true;
        }
        private void QuitIcon_Click(object sender, EventArgs e) {
            
                NoIcon.Visible = false;
                this.RealClose();
        }

        private void SettingsIcon_Click(object sender, EventArgs e)
        {
            Rectangle workingArea = Screen.GetWorkingArea(this);
            this.Location = new Point(workingArea.Right - Size.Width, workingArea.Bottom - Size.Height);
            this.Show();
        }

        private void CP_Button_Click(object sender, EventArgs e)
        {
            DialogResult result = SaveFolder.ShowDialog();
            if (result == DialogResult.OK)
            {
                bool checkAccess = CheckFolderPermission(SaveFolder.SelectedPath);
                //check if folder has got security permission
                if (checkAccess)
                {
                    options.DestPath = SaveFolder.SelectedPath;
                    DestPath.Text = options.DestPath;

                    if (serverTCP != null) {
                        serverTCP.PathDest = SaveFolder.SelectedPath;
                    }

                }else{
                    MessageBox.Show("Permission Denied!");
                }
                
                //un po' di controlli
                //

            }
        }



        public bool CheckFolderPermission(string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath)) return false;

            try
            {
                DirectorySecurity ds = System.IO.Directory.GetAccessControl(folderPath);
                if (ds.AreAccessRulesProtected)
                {
                    return false;
                }

            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            return true;
        }

        private void NoIcon_DoubleClick(object sender, EventArgs e) {

            this.Show();
            Rectangle workingArea = Screen.GetWorkingArea(this);
            this.Location = new Point(workingArea.Right - Size.Width, workingArea.Bottom - Size.Height);
        }

        private void AutomaticReception_CheckedChanged(object sender, EventArgs e)
        {
            if (AutomaticReception.Checked)
            {
                options.RicMode = true;
                
            }else{
                options.RicMode = false;
            }
            
            if(serverTCP!= null){
                serverTCP.AutomaticAnswer = options.RicMode;
            }
            
            CheckBoxApparence(AutomaticReception);
        }

        private void PrivateMode_CheckedChanged(object sender, EventArgs e)
        {
            if (PrivateMode.Checked)
            {
                options.PrivateMode = true;

                //close threads
                if (UDPClientThread != null)
                {
                    if (UDPClientThread.IsAlive)
                    {
                        clientUDP.CloseThread(false);
                    }
                }
                if (TCPServerThread != null)
                {
                    if (TCPServerThread.IsAlive)
                    {
                        serverTCP.CloseThread(false);
                    }
                }
                
            }
            else
            {
                if (!Utilities.FindPort(ref portTCP))
                {
                    //there are no free ports so close the program
                    RealClose();
                }

                options.PrivateMode = false;

                serverTCP = new ServerTCP(portTCP, options.DestPath, options.RicMode, this);
                TCPServerThread = new Thread(serverTCP.Execute);
                TCPServerThread.Start();

                //inizializzo clientUDP
                clientUDP = new ClientUDP(options.Name, portTCP, this);
                UDPClientThread = new Thread(clientUDP.Execute);
                UDPClientThread.Start();

            }
            CheckBoxApparence(PrivateMode);
        }

        private void UserName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                changeName();
                this.ActiveControl = LabelName;
            }
        }

        private void changeName()
        {
            if (UserName.Text=="" || UserName.Text == "DefaultName")
            {
                MessageBox.Show("Invalid User Name!");
                UserName.Text = options.Name;

            }
            else
            {
                //check if is alive and set clientUDP
                if (clientUDP != null)
                {
                    clientUDP.Name = UserName.Text;
                }
                options.Name = UserName.Text;
            }
        }

        private void CheckBoxApparence(CheckBox c) {
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
        
        private void CloseThreadMethod(Thread thread, String lable)
        {
            if (thread.IsAlive)
            {

                thread.Join();

                //if the thread is contained in the threadlist remove it
                if (threadFormList.ContainsKey(thread))
                {
                    threadFormList.Remove(thread);
                }
            }

            if (lable == null) {
                return;
            }

            if (!realClose) { 
                if (lable.Equals(Utilities.ServerUDP)) {

                    //Initialize serverUDP
                    serverUDP = new ServerUDP(userList, sharingFormList, this);
                    UDPServerThread = new Thread(serverUDP.Execute);
                    UDPServerThread.Start();
                    
                }
                if (lable.Equals(Utilities.ServerTCP) && options.PrivateMode==false)
                {
                    //get a free port
                    if (!Utilities.FindPort(ref portTCP))
                    {
                        //there are no free ports so close the program
                        RealClose();
                    }

                    if (clientUDP != null)
                    {
                        clientUDP.PortTCP = portTCP;
                    }

                    //Initialize serverTCP
                    serverTCP = new ServerTCP(portTCP, options.DestPath, options.RicMode, this);
                    TCPServerThread = new Thread(serverTCP.Execute);
                    TCPServerThread.Start();
                    
                }
                if (lable.Equals(Utilities.CientUDP) && options.PrivateMode == false)
                {
                    //get a free port
                    if (!Utilities.FindPort(ref portTCP))
                    {
                        //there are no free ports so close the program
                        RealClose();
                    }

                    if (serverTCP != null)
                    {
                        serverTCP.PortTCP = portTCP;
                    }

                    //Initialize clientUDP
                    clientUDP = new ClientUDP(options.Name, portTCP, this);
                    UDPClientThread = new Thread(clientUDP.Execute);
                    UDPClientThread.Start();
                }
            }
        }

        private void SettingsForm_Shown(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void notifyFile(String message, Boolean error)
        {
            NoIcon.BalloonTipTitle = "Ribbit";

            if (error)
            {
                NoIcon.BalloonTipIcon = ToolTipIcon.Error;
            }
            else
            {
                NoIcon.BalloonTipIcon = ToolTipIcon.Info;
            }

            NoIcon.BalloonTipText = message;
            NoIcon.ShowBalloonTip(1000);
        }

        private void LaunchSharingFormMethod(String path) {

            //set senInvok to true so Server UDP can send invoke to sharingForm
            Flag isCreated = new Flag(false);
            SharingForm sharingForm = new SharingForm(new List<User>(userList), sharingFormList, path, options, this, isCreated);
            Thread thread = new Thread(() => { sharingForm.ShowDialog(); });
            thread.Start();
            threadFormList.Add(thread, sharingForm);

            //wait until form is created
            lock (isCreated)
            {
                while (isCreated.value() == false)
                {
                    Monitor.Wait(isCreated);
                }
            }
        }

        private void RealClose(){
            
            //hide form
            Hide();
            realClose = true;

            List<Thread> threadList = new List<Thread>();

            //close all threads
            if (TCPServerThread != null)
            {
                if (TCPServerThread.IsAlive)
                {
                    serverTCP.CloseThread(true);
                    threadList.Add(TCPServerThread);
                }

            }

            if (UDPClientThread != null)
            {
                if (UDPClientThread.IsAlive)
                {
                    clientUDP.CloseThread(true);
                    threadList.Add(TCPServerThread);
                }
            }

            if (UDPServerThread.IsAlive)
            {
                serverUDP.CloseThread();
                threadList.Add(TCPServerThread);
            }
            
            foreach (KeyValuePair<Thread, SharingForm> pair in threadFormList)
            {
                if (pair.Value.IsHandleCreated)
                {
                    pair.Value.Invoke(pair.Value.closeThreadDelegate);
                }
            }

            foreach (KeyValuePair<Thread, SharingForm> pair in threadFormList) {
                pair.Key.Join();
            }

            foreach (Thread t in threadList) {
                t.Join();
            }

            //exit
            Application.Exit();
        }
        private void AddressChangedCallback(object sender, EventArgs e)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                if (TCPServerThread != null)
                {
                    if (TCPServerThread.IsAlive)
                    {
                        serverTCP.CloseThread(false);
                    }
                }
            }
        }

        private void PipeCallBack(IAsyncResult result)
        {
            if (realClose) {
                MessageBox.Show("The program is closing");
                return;
            }

            try
            {
                pipe.EndWaitForConnection(result);
                pipe.WaitForPipeDrain();
                byte[] buff = new byte[4];
                pipe.Read(buff, 0, 4);
                int Buffersize = BitConverter.ToInt32(buff, 0);
                buff = new byte[Buffersize];
                pipe.Read(buff, 0, Buffersize);
                String arg = Encoding.UTF8.GetString(buff);          
                Array.Clear(buff, 0, buff.Length);
                pipe.Disconnect();
                pipe.BeginWaitForConnection(PipeCallBack, pipe);
                LaunchSharingFormMethod(arg);
            }
            catch (Exception ex) {
                Console.WriteLine(ex.ToString());
                if (pipe.IsConnected) {
                    pipe.Disconnect();
                }
                pipe.BeginWaitForConnection(PipeCallBack, pipe);
            }
        }

    }
}
