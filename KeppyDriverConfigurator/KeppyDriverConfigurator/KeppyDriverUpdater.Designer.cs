namespace KeppyDriverConfigurator
{
    partial class KeppyDriverUpdater
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
            this.label2 = new System.Windows.Forms.Label();
            this.LatestVersion = new System.Windows.Forms.Label();
            this.ThisVersion = new System.Windows.Forms.Label();
            this.UpdateCheck = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Trebuchet MS", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(301, 27);
            this.label2.TabIndex = 8;
            this.label2.Text = "Keppy\'s Driver Update Checker";
            // 
            // LatestVersion
            // 
            this.LatestVersion.AutoSize = true;
            this.LatestVersion.Location = new System.Drawing.Point(14, 67);
            this.LatestVersion.Name = "LatestVersion";
            this.LatestVersion.Size = new System.Drawing.Size(356, 13);
            this.LatestVersion.TabIndex = 7;
            this.LatestVersion.Text = "Click on \"Check for updates\" to check for the latest version of the driver.";
            // 
            // ThisVersion
            // 
            this.ThisVersion.AutoSize = true;
            this.ThisVersion.Location = new System.Drawing.Point(14, 51);
            this.ThisVersion.Name = "ThisVersion";
            this.ThisVersion.Size = new System.Drawing.Size(325, 13);
            this.ThisVersion.TabIndex = 6;
            this.ThisVersion.Text = "The current version of the driver, installed on your system, is: IDK";
            // 
            // UpdateCheck
            // 
            this.UpdateCheck.Location = new System.Drawing.Point(432, 11);
            this.UpdateCheck.Name = "UpdateCheck";
            this.UpdateCheck.Size = new System.Drawing.Size(106, 23);
            this.UpdateCheck.TabIndex = 5;
            this.UpdateCheck.Text = "Check for updates";
            this.UpdateCheck.UseVisualStyleBackColor = true;
            this.UpdateCheck.Click += new System.EventHandler(this.UpdateCheck_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::KeppyDriverConfigurator.Properties.Resources.updatebk;
            this.pictureBox1.Location = new System.Drawing.Point(243, 83);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(329, 248);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // KeppyDriverUpdater
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(549, 277);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.LatestVersion);
            this.Controls.Add(this.ThisVersion);
            this.Controls.Add(this.UpdateCheck);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KeppyDriverUpdater";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Keppy\'s Driver Update Checker";
            this.Load += new System.EventHandler(this.KeppyDriverUpdater_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label LatestVersion;
        private System.Windows.Forms.Label ThisVersion;
        private System.Windows.Forms.Button UpdateCheck;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}