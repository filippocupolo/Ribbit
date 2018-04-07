using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Progetto_2._0
{
    public partial class FirstRegistrationForm : Form
    {
        int mouseX = 0, mouseY = 0;
        bool mouseDown;
        
        Label ProgramName = new Label();
        Panel TitleBar = new Panel();
        Button CloseButton = new Button();
        //devo aggiungere l'icona
        //new Icon Icon = new Icon("../../ img / share.ico");

        private Options options;

        public FirstRegistrationForm(Options options)
        {
            this.options = options;

            InitializeComponent();
            EditTitleBar();
        }

        private void firstRegistration_Load(object sender, EventArgs e)
        {

        }

        private void EditTitleBar() {
            
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
            CloseButton.Size = new Size(25,25);
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

        private void CloseButton_MouseHover(object sender,EventArgs e) {
            CloseButton.BackColor = Color.Red;
        }

        private void CloseButton_Click(object sender,EventArgs e) {
            this.Close();
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

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            if (setName.Text == "DefaultName" || setName.Text == "")
            {
                MessageBox.Show("Invalid Name!");
            }
            else
            {
                options.Name = setName.Text;
                this.Close();
            }
        }
        
    }
}
