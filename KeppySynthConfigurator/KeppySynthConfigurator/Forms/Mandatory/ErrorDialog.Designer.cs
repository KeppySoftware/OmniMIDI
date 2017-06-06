namespace KeppySynthConfigurator
{
    partial class SecretDialog
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.OkBtn = new System.Windows.Forms.Button();
            this.CurrentIcon = new System.Windows.Forms.PictureBox();
            this.MessageText = new System.Windows.Forms.Label();
            this.ChangelogToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.DebugInfoText = new System.Windows.Forms.RichTextBox();
            this.ExGroup = new System.Windows.Forms.GroupBox();
            this.SaveLog = new System.Windows.Forms.Button();
            this.SaveLogDialog = new System.Windows.Forms.SaveFileDialog();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentIcon)).BeginInit();
            this.ExGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.SaveLog);
            this.panel1.Controls.Add(this.ExGroup);
            this.panel1.Controls.Add(this.OkBtn);
            this.panel1.Location = new System.Drawing.Point(0, 89);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(547, 231);
            this.panel1.TabIndex = 0;
            // 
            // OkBtn
            // 
            this.OkBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.OkBtn.Location = new System.Drawing.Point(460, 12);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(75, 23);
            this.OkBtn.TabIndex = 1;
            this.OkBtn.Text = "OK";
            this.OkBtn.UseVisualStyleBackColor = true;
            this.OkBtn.Click += new System.EventHandler(this.NoBtn_Click);
            // 
            // CurrentIcon
            // 
            this.CurrentIcon.Image = global::KeppySynthConfigurator.Properties.Resources.wi;
            this.CurrentIcon.Location = new System.Drawing.Point(19, 17);
            this.CurrentIcon.Name = "CurrentIcon";
            this.CurrentIcon.Size = new System.Drawing.Size(50, 50);
            this.CurrentIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.CurrentIcon.TabIndex = 1;
            this.CurrentIcon.TabStop = false;
            // 
            // MessageText
            // 
            this.MessageText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MessageText.Location = new System.Drawing.Point(88, 0);
            this.MessageText.Name = "MessageText";
            this.MessageText.Size = new System.Drawing.Size(441, 86);
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
            // DebugInfoText
            // 
            this.DebugInfoText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DebugInfoText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DebugInfoText.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.DebugInfoText.DetectUrls = false;
            this.DebugInfoText.Location = new System.Drawing.Point(8, 18);
            this.DebugInfoText.Name = "DebugInfoText";
            this.DebugInfoText.ReadOnly = true;
            this.DebugInfoText.Size = new System.Drawing.Size(419, 187);
            this.DebugInfoText.TabIndex = 0;
            this.DebugInfoText.Text = "";
            // 
            // ExGroup
            // 
            this.ExGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ExGroup.Controls.Add(this.DebugInfoText);
            this.ExGroup.Location = new System.Drawing.Point(12, 6);
            this.ExGroup.Name = "ExGroup";
            this.ExGroup.Size = new System.Drawing.Size(435, 213);
            this.ExGroup.TabIndex = 2;
            this.ExGroup.TabStop = false;
            this.ExGroup.Text = "Exception reason";
            // 
            // SaveLog
            // 
            this.SaveLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveLog.Location = new System.Drawing.Point(460, 41);
            this.SaveLog.Name = "SaveLog";
            this.SaveLog.Size = new System.Drawing.Size(75, 23);
            this.SaveLog.TabIndex = 3;
            this.SaveLog.Text = "Save...";
            this.SaveLog.UseVisualStyleBackColor = true;
            this.SaveLog.Click += new System.EventHandler(this.SaveLog_Click);
            // 
            // SaveLogDialog
            // 
            this.SaveLogDialog.DefaultExt = "*.log";
            this.SaveLogDialog.Filter = "Log files|*.log";
            // 
            // SecretDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(547, 320);
            this.Controls.Add(this.MessageText);
            this.Controls.Add(this.CurrentIcon);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SecretDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Keppy\'s Synthesizer - Event here";
            this.Load += new System.EventHandler(this.UpdateYesNo_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CurrentIcon)).EndInit();
            this.ExGroup.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button OkBtn;
        private System.Windows.Forms.PictureBox CurrentIcon;
        private System.Windows.Forms.Label MessageText;
        private System.Windows.Forms.ToolTip ChangelogToolTip;
        private System.Windows.Forms.RichTextBox DebugInfoText;
        private System.Windows.Forms.Button SaveLog;
        private System.Windows.Forms.GroupBox ExGroup;
        private System.Windows.Forms.SaveFileDialog SaveLogDialog;
    }
}