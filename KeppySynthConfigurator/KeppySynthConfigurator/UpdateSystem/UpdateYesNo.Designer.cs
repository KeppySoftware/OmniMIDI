namespace KeppySynthConfigurator
{
    partial class UpdateYesNo
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateYesNo));
            this.panel1 = new System.Windows.Forms.Panel();
            this.ShowChangelog = new System.Windows.Forms.Button();
            this.NoBtn = new System.Windows.Forms.Button();
            this.YesBtn = new System.Windows.Forms.Button();
            this.CurrentIcon = new System.Windows.Forms.PictureBox();
            this.MessageText = new System.Windows.Forms.Label();
            this.ChangelogToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.ShowChangelog);
            this.panel1.Controls.Add(this.NoBtn);
            this.panel1.Controls.Add(this.YesBtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 86);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(394, 47);
            this.panel1.TabIndex = 0;
            // 
            // ShowChangelog
            // 
            this.ShowChangelog.Location = new System.Drawing.Point(12, 12);
            this.ShowChangelog.Name = "ShowChangelog";
            this.ShowChangelog.Size = new System.Drawing.Size(98, 23);
            this.ShowChangelog.TabIndex = 2;
            this.ShowChangelog.Text = "Show changelog";
            this.ShowChangelog.UseVisualStyleBackColor = true;
            this.ShowChangelog.Click += new System.EventHandler(this.ShowChangelog_Click);
            // 
            // NoBtn
            // 
            this.NoBtn.Location = new System.Drawing.Point(307, 12);
            this.NoBtn.Name = "NoBtn";
            this.NoBtn.Size = new System.Drawing.Size(75, 23);
            this.NoBtn.TabIndex = 1;
            this.NoBtn.Text = "No";
            this.NoBtn.UseVisualStyleBackColor = true;
            this.NoBtn.Click += new System.EventHandler(this.NoBtn_Click);
            // 
            // YesBtn
            // 
            this.YesBtn.Location = new System.Drawing.Point(226, 12);
            this.YesBtn.Name = "YesBtn";
            this.YesBtn.Size = new System.Drawing.Size(75, 23);
            this.YesBtn.TabIndex = 0;
            this.YesBtn.Text = "Yes";
            this.YesBtn.UseVisualStyleBackColor = true;
            this.YesBtn.Click += new System.EventHandler(this.YesBtn_Click);
            // 
            // CurrentIcon
            // 
            this.CurrentIcon.Image = ((System.Drawing.Image)(resources.GetObject("CurrentIcon.Image")));
            this.CurrentIcon.Location = new System.Drawing.Point(19, 17);
            this.CurrentIcon.Name = "CurrentIcon";
            this.CurrentIcon.Size = new System.Drawing.Size(50, 50);
            this.CurrentIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.CurrentIcon.TabIndex = 1;
            this.CurrentIcon.TabStop = false;
            // 
            // MessageText
            // 
            this.MessageText.Location = new System.Drawing.Point(85, 8);
            this.MessageText.Name = "MessageText";
            this.MessageText.Size = new System.Drawing.Size(302, 68);
            this.MessageText.TabIndex = 2;
            this.MessageText.Text = "Message here.";
            this.MessageText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ChangelogToolTip
            // 
            this.ChangelogToolTip.AutomaticDelay = 100;
            this.ChangelogToolTip.AutoPopDelay = 2147483647;
            this.ChangelogToolTip.InitialDelay = 100;
            this.ChangelogToolTip.ReshowDelay = 20;
            this.ChangelogToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.ChangelogToolTip.ToolTipTitle = "Show changelog";
            // 
            // UpdateYesNo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(394, 133);
            this.Controls.Add(this.MessageText);
            this.Controls.Add(this.CurrentIcon);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UpdateYesNo";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Keppy\'s Synthesizer - Event here";
            this.Load += new System.EventHandler(this.UpdateYesNo_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CurrentIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button NoBtn;
        private System.Windows.Forms.Button YesBtn;
        private System.Windows.Forms.PictureBox CurrentIcon;
        private System.Windows.Forms.Label MessageText;
        private System.Windows.Forms.ToolTip ChangelogToolTip;
        private System.Windows.Forms.Button ShowChangelog;
    }
}