namespace KeppySynthConfigurator
{
    partial class BecomeAPatron
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BecomeAPatron));
            this.BecomeAPatronNow = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.DontShowAnymore = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BecomeAPatronNow
            // 
            this.BecomeAPatronNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BecomeAPatronNow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BecomeAPatronNow.Image = global::KeppySynthConfigurator.Properties.Resources.patronbtn;
            this.BecomeAPatronNow.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BecomeAPatronNow.Location = new System.Drawing.Point(196, 196);
            this.BecomeAPatronNow.Name = "BecomeAPatronNow";
            this.BecomeAPatronNow.Size = new System.Drawing.Size(136, 33);
            this.BecomeAPatronNow.TabIndex = 8;
            this.BecomeAPatronNow.Text = "Become a patron";
            this.BecomeAPatronNow.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BecomeAPatronNow.UseVisualStyleBackColor = true;
            this.BecomeAPatronNow.Click += new System.EventHandler(this.BecomeAPatronNow_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(320, 175);
            this.label1.TabIndex = 7;
            this.label1.Text = resources.GetString("label1.Text");
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DontShowAnymore
            // 
            this.DontShowAnymore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DontShowAnymore.Image = global::KeppySynthConfigurator.Properties.Resources.clocklater;
            this.DontShowAnymore.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.DontShowAnymore.Location = new System.Drawing.Point(12, 196);
            this.DontShowAnymore.Name = "DontShowAnymore";
            this.DontShowAnymore.Size = new System.Drawing.Size(136, 33);
            this.DontShowAnymore.TabIndex = 5;
            this.DontShowAnymore.Text = "Maybe later";
            this.DontShowAnymore.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.DontShowAnymore.UseVisualStyleBackColor = true;
            this.DontShowAnymore.Click += new System.EventHandler(this.DontShowAnymore_Click);
            // 
            // BecomeAPatron
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 241);
            this.ControlBox = false;
            this.Controls.Add(this.BecomeAPatronNow);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DontShowAnymore);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BecomeAPatron";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Become a patron to keep the project alive";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BecomeAPatronNow;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button DontShowAnymore;
    }
}