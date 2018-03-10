namespace Progetto_2._0
{
    partial class Form1
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.TitleBar = new System.Windows.Forms.Panel();
            this.lName = new System.Windows.Forms.Label();
            this.closeButton = new System.Windows.Forms.Button();
            this.saveFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.settingsButton = new System.Windows.Forms.Button();
            this.settingsBar = new System.Windows.Forms.Panel();
            this.settingsPanel = new System.Windows.Forms.Panel();
            this.NU = new System.Windows.Forms.Label();
            this.PU = new System.Windows.Forms.Label();
            this.RM = new System.Windows.Forms.Label();
            this.privateUserP = new System.Windows.Forms.Panel();
            this.privateUser = new System.Windows.Forms.CheckBox();
            this.ricModeP = new System.Windows.Forms.Panel();
            this.ricMode = new System.Windows.Forms.CheckBox();
            this.changePath = new System.Windows.Forms.Button();
            this.destPath = new System.Windows.Forms.Label();
            this.nameUser = new System.Windows.Forms.TextBox();
            this.userList = new System.Windows.Forms.ListBox();
            this.TitleBar.SuspendLayout();
            this.settingsBar.SuspendLayout();
            this.settingsPanel.SuspendLayout();
            this.privateUserP.SuspendLayout();
            this.ricModeP.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(18, 284);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(90, 36);
            this.button1.TabIndex = 0;
            this.button1.Text = "Condividi";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TitleBar
            // 
            this.TitleBar.BackColor = System.Drawing.SystemColors.ControlDark;
            this.TitleBar.Controls.Add(this.lName);
            this.TitleBar.Controls.Add(this.closeButton);
            this.TitleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.TitleBar.Location = new System.Drawing.Point(0, 0);
            this.TitleBar.Margin = new System.Windows.Forms.Padding(2);
            this.TitleBar.Name = "TitleBar";
            this.TitleBar.Size = new System.Drawing.Size(446, 24);
            this.TitleBar.TabIndex = 2;
            this.TitleBar.Paint += new System.Windows.Forms.PaintEventHandler(this.TitleBar_Paint);
            this.TitleBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TitleBar_MouseDown);
            this.TitleBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TitleBar_MouseMove);
            this.TitleBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TitleBar_MouseUp);
            // 
            // lName
            // 
            this.lName.AutoSize = true;
            this.lName.Location = new System.Drawing.Point(15, 6);
            this.lName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lName.Name = "lName";
            this.lName.Size = new System.Drawing.Size(35, 13);
            this.lName.TabIndex = 4;
            this.lName.Text = "label1";
            // 
            // closeButton
            // 
            this.closeButton.FlatAppearance.BorderSize = 0;
            this.closeButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.closeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeButton.Location = new System.Drawing.Point(420, 0);
            this.closeButton.Margin = new System.Windows.Forms.Padding(2);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(26, 24);
            this.closeButton.TabIndex = 3;
            this.closeButton.Text = "X";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // settingsButton
            // 
            this.settingsButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.settingsButton.BackColor = System.Drawing.SystemColors.Control;
            this.settingsButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("settingsButton.BackgroundImage")));
            this.settingsButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.settingsButton.FlatAppearance.BorderSize = 0;
            this.settingsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.settingsButton.Location = new System.Drawing.Point(420, 0);
            this.settingsButton.Margin = new System.Windows.Forms.Padding(2);
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(26, 23);
            this.settingsButton.TabIndex = 4;
            this.settingsButton.UseVisualStyleBackColor = false;
            this.settingsButton.Click += new System.EventHandler(this.settingsButton_Click);
            // 
            // settingsBar
            // 
            this.settingsBar.BackColor = System.Drawing.SystemColors.Control;
            this.settingsBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.settingsBar.Controls.Add(this.settingsButton);
            this.settingsBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.settingsBar.Location = new System.Drawing.Point(0, 334);
            this.settingsBar.Margin = new System.Windows.Forms.Padding(2);
            this.settingsBar.Name = "settingsBar";
            this.settingsBar.Size = new System.Drawing.Size(446, 26);
            this.settingsBar.TabIndex = 3;
            // 
            // settingsPanel
            // 
            this.settingsPanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.settingsPanel.Controls.Add(this.NU);
            this.settingsPanel.Controls.Add(this.PU);
            this.settingsPanel.Controls.Add(this.RM);
            this.settingsPanel.Controls.Add(this.privateUserP);
            this.settingsPanel.Controls.Add(this.ricModeP);
            this.settingsPanel.Controls.Add(this.changePath);
            this.settingsPanel.Controls.Add(this.destPath);
            this.settingsPanel.Controls.Add(this.nameUser);
            this.settingsPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.settingsPanel.Location = new System.Drawing.Point(223, 24);
            this.settingsPanel.Margin = new System.Windows.Forms.Padding(2);
            this.settingsPanel.Name = "settingsPanel";
            this.settingsPanel.Size = new System.Drawing.Size(223, 310);
            this.settingsPanel.TabIndex = 4;
            // 
            // NU
            // 
            this.NU.AutoSize = true;
            this.NU.Location = new System.Drawing.Point(148, 13);
            this.NU.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.NU.Name = "NU";
            this.NU.Size = new System.Drawing.Size(70, 13);
            this.NU.TabIndex = 11;
            this.NU.Text = "Nome Utente";
            // 
            // PU
            // 
            this.PU.AutoSize = true;
            this.PU.Location = new System.Drawing.Point(134, 72);
            this.PU.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.PU.Name = "PU";
            this.PU.Size = new System.Drawing.Size(83, 13);
            this.PU.TabIndex = 10;
            this.PU.Text = "Modalità Privata";
            // 
            // RM
            // 
            this.RM.AutoSize = true;
            this.RM.Location = new System.Drawing.Point(107, 42);
            this.RM.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.RM.Name = "RM";
            this.RM.Size = new System.Drawing.Size(110, 13);
            this.RM.TabIndex = 9;
            this.RM.Text = "Ricezione Automatica";
            // 
            // privateUserP
            // 
            this.privateUserP.Controls.Add(this.privateUser);
            this.privateUserP.Location = new System.Drawing.Point(2, 64);
            this.privateUserP.Margin = new System.Windows.Forms.Padding(2);
            this.privateUserP.Name = "privateUserP";
            this.privateUserP.Size = new System.Drawing.Size(25, 29);
            this.privateUserP.TabIndex = 8;
            // 
            // privateUser
            // 
            this.privateUser.AutoSize = true;
            this.privateUser.Location = new System.Drawing.Point(2, 8);
            this.privateUser.Margin = new System.Windows.Forms.Padding(2);
            this.privateUser.Name = "privateUser";
            this.privateUser.Size = new System.Drawing.Size(15, 14);
            this.privateUser.TabIndex = 1;
            this.privateUser.UseVisualStyleBackColor = true;
            this.privateUser.CheckedChanged += new System.EventHandler(this.privateUser_CheckedChanged);
            // 
            // ricModeP
            // 
            this.ricModeP.Controls.Add(this.ricMode);
            this.ricModeP.Location = new System.Drawing.Point(2, 35);
            this.ricModeP.Margin = new System.Windows.Forms.Padding(2);
            this.ricModeP.Name = "ricModeP";
            this.ricModeP.Size = new System.Drawing.Size(25, 26);
            this.ricModeP.TabIndex = 7;
            // 
            // ricMode
            // 
            this.ricMode.AutoSize = true;
            this.ricMode.Location = new System.Drawing.Point(2, 6);
            this.ricMode.Margin = new System.Windows.Forms.Padding(2);
            this.ricMode.Name = "ricMode";
            this.ricMode.Size = new System.Drawing.Size(15, 14);
            this.ricMode.TabIndex = 0;
            this.ricMode.UseVisualStyleBackColor = true;
            this.ricMode.CheckedChanged += new System.EventHandler(this.ricMode_CheckedChanged);
            // 
            // changePath
            // 
            this.changePath.Location = new System.Drawing.Point(166, 104);
            this.changePath.Margin = new System.Windows.Forms.Padding(2);
            this.changePath.Name = "changePath";
            this.changePath.Size = new System.Drawing.Size(56, 24);
            this.changePath.TabIndex = 5;
            this.changePath.Text = "Cambia";
            this.changePath.UseVisualStyleBackColor = true;
            this.changePath.Click += new System.EventHandler(this.changePath_Click_1);
            // 
            // destPath
            // 
            this.destPath.AutoEllipsis = true;
            this.destPath.Location = new System.Drawing.Point(8, 110);
            this.destPath.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.destPath.MaximumSize = new System.Drawing.Size(150, 18);
            this.destPath.MinimumSize = new System.Drawing.Size(150, 18);
            this.destPath.Name = "destPath";
            this.destPath.Size = new System.Drawing.Size(150, 18);
            this.destPath.TabIndex = 3;
            this.destPath.Text = "label1";
            // 
            // nameUser
            // 
            this.nameUser.Location = new System.Drawing.Point(2, 12);
            this.nameUser.Margin = new System.Windows.Forms.Padding(2);
            this.nameUser.Name = "nameUser";
            this.nameUser.Size = new System.Drawing.Size(135, 20);
            this.nameUser.TabIndex = 2;
            this.nameUser.TextChanged += new System.EventHandler(this.nameUser_TextChanged);
            this.nameUser.KeyDown += new System.Windows.Forms.KeyEventHandler(this.nameUser_KeyDown);
            // 
            // userList
            // 
            this.userList.FormattingEnabled = true;
            this.userList.Location = new System.Drawing.Point(15, 34);
            this.userList.Margin = new System.Windows.Forms.Padding(2);
            this.userList.Name = "userList";
            this.userList.Size = new System.Drawing.Size(195, 238);
            this.userList.TabIndex = 6;
            this.userList.SelectedIndexChanged += new System.EventHandler(this.userList_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(446, 360);
            this.Controls.Add(this.userList);
            this.Controls.Add(this.settingsPanel);
            this.Controls.Add(this.settingsBar);
            this.Controls.Add(this.TitleBar);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.Click += new System.EventHandler(this.Form1_Click);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.TitleBar.ResumeLayout(false);
            this.TitleBar.PerformLayout();
            this.settingsBar.ResumeLayout(false);
            this.settingsPanel.ResumeLayout(false);
            this.settingsPanel.PerformLayout();
            this.privateUserP.ResumeLayout(false);
            this.privateUserP.PerformLayout();
            this.ricModeP.ResumeLayout(false);
            this.ricModeP.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel TitleBar;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Label lName;
        private System.Windows.Forms.FolderBrowserDialog saveFolder;
        private System.Windows.Forms.Button settingsButton;
        private System.Windows.Forms.Panel settingsBar;
        private System.Windows.Forms.Panel settingsPanel;
        private System.Windows.Forms.Button changePath;
        private System.Windows.Forms.Label destPath;
        private System.Windows.Forms.TextBox nameUser;
        private System.Windows.Forms.CheckBox privateUser;
        private System.Windows.Forms.CheckBox ricMode;
        private System.Windows.Forms.Panel ricModeP;
        private System.Windows.Forms.Panel privateUserP;
        private System.Windows.Forms.Label PU;
        private System.Windows.Forms.Label RM;
        private System.Windows.Forms.Label NU;
        private System.Windows.Forms.ListBox userList;
    }
}

