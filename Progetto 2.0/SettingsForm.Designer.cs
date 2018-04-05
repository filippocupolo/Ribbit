namespace Progetto_2._0
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.UserName = new System.Windows.Forms.TextBox();
            this.LabelName = new System.Windows.Forms.Label();
            this.AutomaticReception = new System.Windows.Forms.CheckBox();
            this.PrivateMode = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.DestPath = new System.Windows.Forms.Label();
            this.CP_Button = new System.Windows.Forms.Button();
            this.AutomaticReceptionP = new System.Windows.Forms.Panel();
            this.PrivateModeP = new System.Windows.Forms.Panel();
            this.SaveFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.AutomaticReceptionP.SuspendLayout();
            this.PrivateModeP.SuspendLayout();
            this.SuspendLayout();
            // 
            // UserName
            // 
            this.UserName.Location = new System.Drawing.Point(40, 58);
            this.UserName.Margin = new System.Windows.Forms.Padding(4);
            this.UserName.Name = "UserName";
            this.UserName.Size = new System.Drawing.Size(232, 31);
            this.UserName.TabIndex = 0;
            this.UserName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UserName_KeyDown);
            // 
            // LabelName
            // 
            this.LabelName.AutoSize = true;
            this.LabelName.Location = new System.Drawing.Point(304, 58);
            this.LabelName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LabelName.Name = "LabelName";
            this.LabelName.Size = new System.Drawing.Size(57, 25);
            this.LabelName.TabIndex = 1;
            this.LabelName.Text = "User";
            // 
            // AutomaticReception
            // 
            this.AutomaticReception.AutoSize = true;
            this.AutomaticReception.Location = new System.Drawing.Point(16, 15);
            this.AutomaticReception.Margin = new System.Windows.Forms.Padding(4);
            this.AutomaticReception.Name = "AutomaticReception";
            this.AutomaticReception.Size = new System.Drawing.Size(28, 27);
            this.AutomaticReception.TabIndex = 2;
            this.AutomaticReception.UseVisualStyleBackColor = true;
            this.AutomaticReception.CheckedChanged += new System.EventHandler(this.AutomaticReception_CheckedChanged);
            // 
            // PrivateMode
            // 
            this.PrivateMode.AutoSize = true;
            this.PrivateMode.Location = new System.Drawing.Point(16, 17);
            this.PrivateMode.Margin = new System.Windows.Forms.Padding(4);
            this.PrivateMode.Name = "PrivateMode";
            this.PrivateMode.Size = new System.Drawing.Size(28, 27);
            this.PrivateMode.TabIndex = 3;
            this.PrivateMode.UseVisualStyleBackColor = true;
            this.PrivateMode.CheckedChanged += new System.EventHandler(this.PrivateMode_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(304, 119);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(210, 25);
            this.label2.TabIndex = 4;
            this.label2.Text = "Automatic Reception";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(304, 198);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(139, 25);
            this.label3.TabIndex = 5;
            this.label3.Text = "Private Mode";
            // 
            // DestPath
            // 
            this.DestPath.AutoEllipsis = true;
            this.DestPath.Location = new System.Drawing.Point(36, 285);
            this.DestPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.DestPath.Name = "DestPath";
            this.DestPath.Size = new System.Drawing.Size(236, 48);
            this.DestPath.TabIndex = 6;
            this.DestPath.Text = "label4";
            // 
            // CP_Button
            // 
            this.CP_Button.Location = new System.Drawing.Point(296, 277);
            this.CP_Button.Margin = new System.Windows.Forms.Padding(4);
            this.CP_Button.Name = "CP_Button";
            this.CP_Button.Size = new System.Drawing.Size(106, 40);
            this.CP_Button.TabIndex = 7;
            this.CP_Button.Text = "Cambia";
            this.CP_Button.UseVisualStyleBackColor = true;
            this.CP_Button.Click += new System.EventHandler(this.CP_Button_Click);
            // 
            // AutomaticReceptionP
            // 
            this.AutomaticReceptionP.Controls.Add(this.AutomaticReception);
            this.AutomaticReceptionP.Location = new System.Drawing.Point(40, 104);
            this.AutomaticReceptionP.Margin = new System.Windows.Forms.Padding(4);
            this.AutomaticReceptionP.Name = "AutomaticReceptionP";
            this.AutomaticReceptionP.Size = new System.Drawing.Size(56, 56);
            this.AutomaticReceptionP.TabIndex = 8;
            // 
            // PrivateModeP
            // 
            this.PrivateModeP.Controls.Add(this.PrivateMode);
            this.PrivateModeP.Location = new System.Drawing.Point(40, 181);
            this.PrivateModeP.Margin = new System.Windows.Forms.Padding(4);
            this.PrivateModeP.Name = "PrivateModeP";
            this.PrivateModeP.Size = new System.Drawing.Size(56, 62);
            this.PrivateModeP.TabIndex = 9;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(536, 417);
            this.Controls.Add(this.PrivateModeP);
            this.Controls.Add(this.AutomaticReceptionP);
            this.Controls.Add(this.CP_Button);
            this.Controls.Add(this.DestPath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.LabelName);
            this.Controls.Add(this.UserName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SettingsForm";
            this.Text = "SettingsForm";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.Shown += new System.EventHandler(this.SettingsForm_Shown);
            this.AutomaticReceptionP.ResumeLayout(false);
            this.AutomaticReceptionP.PerformLayout();
            this.PrivateModeP.ResumeLayout(false);
            this.PrivateModeP.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox UserName;
        private System.Windows.Forms.Label LabelName;
        private System.Windows.Forms.CheckBox AutomaticReception;
        private System.Windows.Forms.CheckBox PrivateMode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label DestPath;
        private System.Windows.Forms.Button CP_Button;
        private System.Windows.Forms.Panel AutomaticReceptionP;
        private System.Windows.Forms.Panel PrivateModeP;
        private System.Windows.Forms.FolderBrowserDialog SaveFolder;
    }
}