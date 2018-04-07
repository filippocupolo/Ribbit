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

namespace Progetto_2._0
{
    public partial class ProgressBar : Form
    {
        private Flag cancel;
        private Object locker;
        private Flag isCreated;
        private object cancelLocker = new object();
        private string message;
        private System.Windows.Forms.Timer timer;
        public delegate void setPercentageDelegate(int i);
        public delegate void closeDelegate();
        public delegate void setTimeDelegate(String s);
        public setPercentageDelegate percentageDelegate;
        public closeDelegate closeFormDelegate;
        public setTimeDelegate timeDelegate;
        
        //for the title bar
        int mouseX = 0, mouseY = 0;
        bool mouseDown;
        Label ProgramName = new Label();
        Panel TitleBar = new Panel();
        Button IconButton = new Button();
        public ProgressBar(Flag isCreated, Object locker, Flag cancel, string message)
        {
            InitializeComponent();
            this.cancel = cancel;
            this.isCreated = isCreated;
            this.locker = locker;
            this.message = message;
            timer = new System.Windows.Forms.Timer();
            percentageDelegate = new setPercentageDelegate(this.Setpercentage);
            timeDelegate = new setTimeDelegate(SetTime);
            closeFormDelegate = new closeDelegate(this.CloseFormMethod);
            EditTitleBar();
        }

        private void Setpercentage(int i)
        {
            this.progressBar1.Value = i;   
        }

        private void SetTime(string s)
        {
            this.time.Text = s;
        }

        private void Annulla_Click(object sender, EventArgs e)
        {
            lock (locker)
            {
                cancel.setTrue();
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void Download_Click(object sender, EventArgs e)
        {

        }

        private void CloseFormMethod() {
            
                this.Setpercentage(100);
                this.SetTime("0 seconds");
                timer.Interval = 2000;
                timer.Tick += timer_Tick;
                timer.Start();
            
        }

        void timer_Tick(object sender, EventArgs e)
        {
            this.Close();
            
        }

        private void Form2_Shown(object sender, EventArgs e)
        {
            lock (locker)
            {
                isCreated.setTrue();
                Monitor.PulseAll(locker);

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


            IconButton.Name = "iconButton";
            IconButton.Text = "-";
            IconButton.ForeColor = Color.WhiteSmoke;
            
            IconButton.Dock = DockStyle.Right;
            IconButton.FlatStyle = FlatStyle.Flat;
            

            IconButton.Size = new Size(25, 25);
            IconButton.BackColor = Color.IndianRed;
            IconButton.Click += new System.EventHandler(this.lblMaximize_Click);
            IconButton.MouseHover += new System.EventHandler(this.IconButton_MouseHover);

            ProgramName.Name = "programName";
            ProgramName.Text = message;
            ProgramName.ForeColor = Color.WhiteSmoke;
            ProgramName.AutoSize = true;
            ProgramName.Location = new Point(25, 8);

            TitleBar.Controls.Add(IconButton);
            TitleBar.Controls.Add(ProgramName);
            this.Controls.Add(TitleBar);
            
        }

        private void IconButton_MouseHover(object sender, EventArgs e)
        {
            IconButton.BackColor = Color.Red;
        }
        private void lblMaximize_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Minimized;
            }
            else if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Maximized;
            }
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
    }
}


