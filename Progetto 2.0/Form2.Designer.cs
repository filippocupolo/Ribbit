namespace Progetto_2._0
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.fileName = new System.Windows.Forms.Label();
            this.Download = new System.Windows.Forms.Label();
            this.Annulla = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(80, 28);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(214, 28);
            this.progressBar1.TabIndex = 0;
            // 
            // fileName
            // 
            this.fileName.AutoSize = true;
            this.fileName.Location = new System.Drawing.Point(78, 74);
            this.fileName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.fileName.Name = "fileName";
            this.fileName.Size = new System.Drawing.Size(48, 13);
            this.fileName.TabIndex = 1;
            this.fileName.Text = "fileName";
            // 
            // Download
            // 
            this.Download.AutoSize = true;
            this.Download.Location = new System.Drawing.Point(196, 74);
            this.Download.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Download.Name = "Download";
            this.Download.Size = new System.Drawing.Size(74, 13);
            this.Download.TabIndex = 2;
            this.Download.Text = "download_prc";
            // 
            // Annulla
            // 
            this.Annulla.Location = new System.Drawing.Point(282, 113);
            this.Annulla.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Annulla.Name = "Annulla";
            this.Annulla.Size = new System.Drawing.Size(64, 26);
            this.Annulla.TabIndex = 3;
            this.Annulla.Text = "Annulla";
            this.Annulla.UseVisualStyleBackColor = true;
            this.Annulla.Click += new System.EventHandler(this.Annulla_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 152);
            this.Controls.Add(this.Annulla);
            this.Controls.Add(this.Download);
            this.Controls.Add(this.fileName);
            this.Controls.Add(this.progressBar1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form2";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label fileName;
        private System.Windows.Forms.Label Download;
        private System.Windows.Forms.Button Annulla;
    }
}