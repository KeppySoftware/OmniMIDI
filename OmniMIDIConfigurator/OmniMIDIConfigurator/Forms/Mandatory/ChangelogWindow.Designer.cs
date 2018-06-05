namespace OmniMIDIConfigurator{
    partial class ChangelogWindow
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
            this.ChangelogBrowser = new System.Windows.Forms.WebBrowser();
            this.VersionLabel = new System.Windows.Forms.Label();
            this.OkBtn = new System.Windows.Forms.Button();
            this.UpdateBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ChangelogBrowser
            // 
            this.ChangelogBrowser.AllowNavigation = false;
            this.ChangelogBrowser.AllowWebBrowserDrop = false;
            this.ChangelogBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChangelogBrowser.IsWebBrowserContextMenuEnabled = false;
            this.ChangelogBrowser.Location = new System.Drawing.Point(0, 47);
            this.ChangelogBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.ChangelogBrowser.Name = "ChangelogBrowser";
            this.ChangelogBrowser.ScriptErrorsSuppressed = true;
            this.ChangelogBrowser.Size = new System.Drawing.Size(708, 391);
            this.ChangelogBrowser.TabIndex = 0;
            // 
            // VersionLabel
            // 
            this.VersionLabel.AutoSize = true;
            this.VersionLabel.Location = new System.Drawing.Point(18, 17);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(146, 13);
            this.VersionLabel.TabIndex = 1;
            this.VersionLabel.Text = "Changelog for version 0.0.0.0";
            this.VersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // OkBtn
            // 
            this.OkBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.OkBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.OkBtn.Location = new System.Drawing.Point(621, 12);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(75, 23);
            this.OkBtn.TabIndex = 2;
            this.OkBtn.Text = "OK";
            this.OkBtn.UseVisualStyleBackColor = true;
            // 
            // UpdateBtn
            // 
            this.UpdateBtn.Location = new System.Drawing.Point(509, 12);
            this.UpdateBtn.Name = "UpdateBtn";
            this.UpdateBtn.Size = new System.Drawing.Size(108, 23);
            this.UpdateBtn.TabIndex = 3;
            this.UpdateBtn.Text = "Check for updates";
            this.UpdateBtn.UseVisualStyleBackColor = true;
            this.UpdateBtn.Click += new System.EventHandler(this.UpdateBtn_Click);
            // 
            // ChangelogWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.OkBtn;
            this.ClientSize = new System.Drawing.Size(708, 438);
            this.Controls.Add(this.UpdateBtn);
            this.Controls.Add(this.OkBtn);
            this.Controls.Add(this.VersionLabel);
            this.Controls.Add(this.ChangelogBrowser);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChangelogWindow";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ChangelogWindow";
            this.Load += new System.EventHandler(this.ChangelogWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser ChangelogBrowser;
        private System.Windows.Forms.Label VersionLabel;
        private System.Windows.Forms.Button OkBtn;
        private System.Windows.Forms.Button UpdateBtn;
    }
}