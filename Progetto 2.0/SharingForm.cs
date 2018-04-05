﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Progetto_2._0
{
    public partial class SharingForm : Form
    {
        //title bar
        int mouseX = 0, mouseY = 0;
        bool mouseDown;
        Label ProgramName = new Label();
        Panel TitleBar = new Panel();
        Button CloseButton = new Button();

        private ObservableCollection<User> userCollection;
        private HashSet<Thread> threadList;
        private string sourcePath;
        private Options options;
        private SettingsForm settingsForm;
        private Flag isCreated;

        public delegate void CloseThread();
        public CloseThread closeThreadDelegate;

        public delegate void AddRemoveChangeItem(User user);
        public AddRemoveChangeItem addItemDelegate;
        public AddRemoveChangeItem removeItemDelegate;
        public AddRemoveChangeItem changeItemDelegate;

        private ConcurrentDictionary<SharingForm,String> sharingFormList;


        public SharingForm(List<User> userList, ConcurrentDictionary<SharingForm, String> sharingFormList, string sourcePath, Options options, SettingsForm settingsForm, Flag isCreated)
        {
            InitializeComponent();
            this.sourcePath = sourcePath;
            this.options = options;
            this.sharingFormList = sharingFormList;
            this.settingsForm = settingsForm;
            this.isCreated = isCreated;
            try
            {
                userCollection = new ObservableCollection<User>(userList);
            }
            catch(Exception ex) {
                //ArgumentNullException

            }
            
            SettingList();
            PrintList();
            EditTitleBar();

            //set delegate
            addItemDelegate = new AddRemoveChangeItem(this.AddItemMethod);
            removeItemDelegate = new AddRemoveChangeItem(this.RemoveItemMethod);
            changeItemDelegate = new AddRemoveChangeItem(this.ChangeItemMethod);
            closeThreadDelegate = new CloseThread(this.CloseForm);

            threadList = new HashSet<Thread>();
    }

        private void SharingForm_Load(object sender, EventArgs e){
            try
            {
                //add the sharingform to the list
                sharingFormList.GetOrAdd(this, "");

                //tell to settigs form that the form is been created
                lock (isCreated)
                {
                    isCreated.setTrue();
                    Monitor.PulseAll(isCreated);
                }
            }
            catch (Exception ex) {
                //ArgumentNullException
                //SynchronizationLockException
                //OverflowException
            }
        }

        private void SettingList(){
            try {
                SharingList.SelectionMode = SelectionMode.MultiExtended;
            } catch (Exception ex) {

                //InvalidEnumArgumentException
            }
            
        }

        private void PrintList()
        {
            try
            {
                SharingList.DataSource = new BindingSource(userCollection, null);
                SharingList.ValueMember = "IP.Address";
                SharingList.DisplayMember = "Name";
                this.Controls.Add(SharingList);
            }
            catch (Exception ex) {
                //ArgumentException
                //Exception
            }
        }

        private void AddItemMethod(User user)
        {
            try
            {
                userCollection.Add(user);
                SharingList.SelectionMode = SelectionMode.MultiExtended;
                SharingList.DataSource = userCollection;
                SharingList.SelectionMode = SelectionMode.One;
                SharingList.SelectionMode = SelectionMode.MultiExtended;
            }
            catch (Exception ex)
            {
                //ArgumentException
                //InvalidEnumArgumentException
                Console.WriteLine(ex.ToString());
            }
        }
        private void RemoveItemMethod(User user)
        {
            try
            {
                userCollection.Remove(user);
                SharingList.SelectionMode = SelectionMode.MultiExtended;
                SharingList.DataSource = null;
                SharingList.SelectionMode = SelectionMode.One;
                SharingList.SelectionMode = SelectionMode.MultiExtended;
                PrintList();
                
            }
            catch (Exception ex)
            {
                //ArgumentException
                //InvalidEnumArgumentException
                Console.WriteLine(ex.ToString());
            }
        }

        private void ChangeItemMethod(User user)
        {
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

                SharingList.SelectionMode = SelectionMode.MultiExtended;
                SharingList.DataSource = userCollection;
                SharingList.SelectionMode = SelectionMode.One;
                SharingList.SelectionMode = SelectionMode.MultiExtended;
            }
            catch (Exception ex)
            {
                //InvalidEnumArgumentException
                //ArgumentException
                Console.WriteLine(ex.ToString());
            }
        }

        private void EditTitleBar()
        {
            try
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
            catch (Exception ex) {
                //Exception
                //ArgumentException
                //InvalidEnumArgumentException
            }

        }
        private void CloseButton_MouseHover(object sender, EventArgs e)
        {
            CloseButton.BackColor = Color.Red;
        }
        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.CloseForm();
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

        private void ShareButton_Click(object sender, EventArgs e)
        {
            try
            {

                if (SharingList.SelectedIndex == -1)
                {
                    MessageBox.Show("select a user first!");
                }
                else
                {

                    int nElements = SharingList.SelectedIndices.Count;
                    int index;
                    bool IsFolder = CheckPath();


                    this.Hide();

                    for (int i = 0; i < nElements; i++)
                    {
                        index = SharingList.SelectedIndices[i];
                        Send(sourcePath, IsFolder, userCollection[index].IP);
                    }

                    foreach (Thread t in threadList)
                    {
                        t.Join();
                    }

                    this.CloseForm();
                }
            }
            catch (Exception ex)
            {
                //ArgumentException
                //ArgumentOutOfRangeException
                //ThreadStateException
                //ThreadInterruptedException
                Console.WriteLine(ex.ToString());
            }
        }

        private void CloseForm() {

            //set flagInvoke and close form
            try
            {
                String s;
                sharingFormList.TryRemove(this, out s);
                settingsForm.BeginInvoke(settingsForm.CloseThreadDelegate, new object[] { Thread.CurrentThread });
                this.Close();
            }
            catch (Exception ex) {
                //ArgumentNullException
                //InvalidOperationException
                //ObjectDisposedException
            }
        }

        private void Send(string sourcePath, bool IsFolder, IPEndPoint ip)
        {
            {
                try
                {
                    ClientTCP clientTCP = new ClientTCP(sourcePath, IsFolder, options.Name, ip, settingsForm);
                    Thread TCPClientThread = new Thread(clientTCP.Execute);
                    TCPClientThread.Start();
                    threadList.Add(TCPClientThread);
                }
                catch (Exception ex)
                {
                    //ArgumentNullException
                    //ThreadStateException
                    //OutOfMemoryException
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private bool CheckPath()
        {
            try
            {
                bool IsFolder = File.GetAttributes(sourcePath).HasFlag(FileAttributes.Directory);
                return IsFolder;
            }
            catch (Exception ex)
            {
                //ArgumentException
                //PathTooLongException
                //NotSupportedException
                //FileNotFoundException
                //DirectoryNotFoundException
                //IOException
                //UnauthorizedAccessException
                return false;

            }

        }
    }
}
