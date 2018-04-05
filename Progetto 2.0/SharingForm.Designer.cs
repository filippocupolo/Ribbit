namespace Progetto_2._0
{
    partial class SharingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SharingForm));
            this.SharingList = new System.Windows.Forms.ListBox();
            this.ShareButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SharingList
            // 
            this.SharingList.FormattingEnabled = true;
            this.SharingList.ItemHeight = 25;
            this.SharingList.Location = new System.Drawing.Point(12, 80);
            this.SharingList.Name = "SharingList";
            this.SharingList.Size = new System.Drawing.Size(543, 304);
            this.SharingList.TabIndex = 0;
            // 
            // ShareButton
            // 
            this.ShareButton.Location = new System.Drawing.Point(12, 403);
            this.ShareButton.Name = "ShareButton";
            this.ShareButton.Size = new System.Drawing.Size(257, 84);
            this.ShareButton.TabIndex = 1;
            this.ShareButton.Text = "Share";
            this.ShareButton.UseVisualStyleBackColor = true;
            this.ShareButton.Click += new System.EventHandler(this.ShareButton_Click);
            // 
            // SharingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 499);
            this.Controls.Add(this.ShareButton);
            this.Controls.Add(this.SharingList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SharingForm";
            this.Text = "SharingForm";
            this.Load += new System.EventHandler(this.SharingForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox SharingList;
        private System.Windows.Forms.Button ShareButton;
    }
}