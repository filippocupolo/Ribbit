namespace Progetto_2._0
{
    partial class FirstRegistrationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FirstRegistrationForm));
            this.setName = new System.Windows.Forms.TextBox();
            this.ButtonOk = new System.Windows.Forms.Button();
            this.Description = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // setName
            // 
            this.setName.Location = new System.Drawing.Point(93, 98);
            this.setName.Name = "setName";
            this.setName.Size = new System.Drawing.Size(261, 31);
            this.setName.TabIndex = 0;
            // 
            // ButtonOk
            // 
            this.ButtonOk.Location = new System.Drawing.Point(400, 93);
            this.ButtonOk.Name = "ButtonOk";
            this.ButtonOk.Size = new System.Drawing.Size(128, 40);
            this.ButtonOk.TabIndex = 1;
            this.ButtonOk.Text = "Ok";
            this.ButtonOk.UseVisualStyleBackColor = true;
            this.ButtonOk.Click += new System.EventHandler(this.ButtonOk_Click);
            // 
            // Description
            // 
            this.Description.AutoSize = true;
            this.Description.Location = new System.Drawing.Point(88, 52);
            this.Description.Name = "Description";
            this.Description.Size = new System.Drawing.Size(178, 25);
            this.Description.TabIndex = 2;
            this.Description.Text = "Insert User Name";
            // 
            // FirstRegistrationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 177);
            this.Controls.Add(this.Description);
            this.Controls.Add(this.ButtonOk);
            this.Controls.Add(this.setName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FirstRegistrationForm";
            this.Text = "firstRegistration";
            this.Load += new System.EventHandler(this.firstRegistration_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox setName;
        private System.Windows.Forms.Button ButtonOk;
        private System.Windows.Forms.Label Description;
    }
}