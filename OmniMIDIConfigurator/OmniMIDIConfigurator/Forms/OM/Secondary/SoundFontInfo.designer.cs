namespace OmniMIDIConfigurator
{
    partial class SoundFontInfo
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.FNBox = new System.Windows.Forms.TextBox();
            this.ISFBox = new System.Windows.Forms.TextBox();
            this.CIBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SofSFLab = new System.Windows.Forms.Label();
            this.SFfLab = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.CommentRich = new System.Windows.Forms.RichTextBox();
            this.CloseBtn = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.LELabel = new System.Windows.Forms.Label();
            this.PreviewThread = new System.ComponentModel.BackgroundWorker();
            this.CustomMIDI = new System.Windows.Forms.OpenFileDialog();
            this.RightClickMenu = new System.Windows.Forms.ContextMenu();
            this.LoopYesNo = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.StartNormalPrvw1 = new System.Windows.Forms.MenuItem();
            this.StartNormalPrvw2 = new System.Windows.Forms.MenuItem();
            this.StartNormalPrvw3 = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.StartCustomPrvw = new System.Windows.Forms.MenuItem();
            this.WhatDoesTheSFSay = new System.Windows.Forms.ToolTip(this.components);
            this.SizeWarning = new System.Windows.Forms.ToolTip(this.components);
            this.PrvwBtn = new System.Windows.Forms.Button();
            this.SamF = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Filename:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(144, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Internal SoundFont name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(129, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "Copyright information:";
            // 
            // FNBox
            // 
            this.FNBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FNBox.Location = new System.Drawing.Point(166, 7);
            this.FNBox.Name = "FNBox";
            this.FNBox.ReadOnly = true;
            this.FNBox.Size = new System.Drawing.Size(439, 23);
            this.FNBox.TabIndex = 3;
            // 
            // ISFBox
            // 
            this.ISFBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ISFBox.Location = new System.Drawing.Point(166, 37);
            this.ISFBox.Name = "ISFBox";
            this.ISFBox.ReadOnly = true;
            this.ISFBox.Size = new System.Drawing.Size(439, 23);
            this.ISFBox.TabIndex = 4;
            // 
            // CIBox
            // 
            this.CIBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CIBox.Location = new System.Drawing.Point(166, 67);
            this.CIBox.Name = "CIBox";
            this.CIBox.ReadOnly = true;
            this.CIBox.Size = new System.Drawing.Size(439, 23);
            this.CIBox.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "Size of the SoundFont:";
            // 
            // SofSFLab
            // 
            this.SofSFLab.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SofSFLab.Location = new System.Drawing.Point(166, 100);
            this.SofSFLab.Name = "SofSFLab";
            this.SofSFLab.Size = new System.Drawing.Size(440, 15);
            this.SofSFLab.TabIndex = 8;
            this.SofSFLab.Text = "label5";
            // 
            // SFfLab
            // 
            this.SFfLab.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SFfLab.Location = new System.Drawing.Point(166, 130);
            this.SFfLab.Name = "SFfLab";
            this.SFfLab.Size = new System.Drawing.Size(440, 15);
            this.SFfLab.TabIndex = 10;
            this.SFfLab.Text = "label6";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 130);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(107, 15);
            this.label7.TabIndex = 9;
            this.label7.Text = "SoundFont format:";
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 192);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(64, 15);
            this.label9.TabIndex = 12;
            this.label9.Text = "Comment:";
            // 
            // CommentRich
            // 
            this.CommentRich.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CommentRich.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CommentRich.Location = new System.Drawing.Point(166, 188);
            this.CommentRich.Name = "CommentRich";
            this.CommentRich.ReadOnly = true;
            this.CommentRich.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.CommentRich.Size = new System.Drawing.Size(439, 161);
            this.CommentRich.TabIndex = 13;
            this.CommentRich.Text = "";
            // 
            // CloseBtn
            // 
            this.CloseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseBtn.Location = new System.Drawing.Point(518, 357);
            this.CloseBtn.Name = "CloseBtn";
            this.CloseBtn.Size = new System.Drawing.Size(87, 27);
            this.CloseBtn.TabIndex = 14;
            this.CloseBtn.Text = "OK";
            this.CloseBtn.UseVisualStyleBackColor = true;
            this.CloseBtn.Click += new System.EventHandler(this.CloseBtn_Click);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 362);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 15);
            this.label5.TabIndex = 15;
            this.label5.Text = "Last edited on:";
            // 
            // LELabel
            // 
            this.LELabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LELabel.Location = new System.Drawing.Point(166, 362);
            this.LELabel.Name = "LELabel";
            this.LELabel.Size = new System.Drawing.Size(183, 15);
            this.LELabel.TabIndex = 16;
            this.LELabel.Text = "SAS";
            this.WhatDoesTheSFSay.SetToolTip(this.LELabel, "Do not trust this info, since it can be modified\r\nby sharing the SoundFont betwee" +
        "n computers!");
            // 
            // PreviewThread
            // 
            this.PreviewThread.DoWork += new System.ComponentModel.DoWorkEventHandler(this.PreviewThread_DoWork);
            // 
            // CustomMIDI
            // 
            this.CustomMIDI.Filter = "MIDI files|*.mid;*.midi;*.rmi;*.xm;*.it;*.s3m;*.mod;*.mtm;*.umx;";
            this.CustomMIDI.Title = "Select a MIDI...";
            // 
            // RightClickMenu
            // 
            this.RightClickMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.LoopYesNo,
            this.menuItem3,
            this.StartNormalPrvw1,
            this.StartNormalPrvw2,
            this.StartNormalPrvw3,
            this.menuItem1,
            this.StartCustomPrvw});
            // 
            // LoopYesNo
            // 
            this.LoopYesNo.Index = 0;
            this.LoopYesNo.Text = "Loop";
            this.LoopYesNo.Click += new System.EventHandler(this.LoopYesNo_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 1;
            this.menuItem3.Text = "-";
            // 
            // StartNormalPrvw1
            // 
            this.StartNormalPrvw1.Index = 2;
            this.StartNormalPrvw1.Text = "Preview with Town";
            this.StartNormalPrvw1.Click += new System.EventHandler(this.StartNormalPrvw_Click);
            // 
            // StartNormalPrvw2
            // 
            this.StartNormalPrvw2.Index = 3;
            this.StartNormalPrvw2.Text = "Preview with Onestop";
            this.StartNormalPrvw2.Click += new System.EventHandler(this.StartNormalPrvw2_Click);
            // 
            // StartNormalPrvw3
            // 
            this.StartNormalPrvw3.Index = 4;
            this.StartNormalPrvw3.Text = "Preview with Flourish";
            this.StartNormalPrvw3.Click += new System.EventHandler(this.StartNormalPrvw3_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 5;
            this.menuItem1.Text = "-";
            // 
            // StartCustomPrvw
            // 
            this.StartCustomPrvw.Index = 6;
            this.StartCustomPrvw.Text = "Preview with a custom MIDI...";
            this.StartCustomPrvw.Click += new System.EventHandler(this.StartCustomPrvw_Click);
            // 
            // WhatDoesTheSFSay
            // 
            this.WhatDoesTheSFSay.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Warning;
            this.WhatDoesTheSFSay.ToolTipTitle = "Date of last edit";
            // 
            // SizeWarning
            // 
            this.SizeWarning.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Warning;
            this.SizeWarning.ToolTipTitle = "SoundFont bigger than 2GB";
            // 
            // PrvwBtn
            // 
            this.PrvwBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.PrvwBtn.Location = new System.Drawing.Point(356, 357);
            this.PrvwBtn.Name = "PrvwBtn";
            this.PrvwBtn.Size = new System.Drawing.Size(155, 27);
            this.PrvwBtn.TabIndex = 17;
            this.PrvwBtn.Text = "Play SoundFont preview";
            this.PrvwBtn.UseVisualStyleBackColor = true;
            this.PrvwBtn.Click += new System.EventHandler(this.PrvwBtn_Click);
            // 
            // SamF
            // 
            this.SamF.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SamF.Location = new System.Drawing.Point(166, 162);
            this.SamF.Name = "SamF";
            this.SamF.Size = new System.Drawing.Size(440, 15);
            this.SamF.TabIndex = 19;
            this.SamF.Text = "label6";
            this.SamF.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 162);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(93, 15);
            this.label8.TabIndex = 18;
            this.label8.Text = "Samples format:";
            this.label8.Visible = false;
            // 
            // SoundFontInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(612, 391);
            this.ControlBox = false;
            this.Controls.Add(this.SamF);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.PrvwBtn);
            this.Controls.Add(this.LELabel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.CloseBtn);
            this.Controls.Add(this.CommentRich);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.SFfLab);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.SofSFLab);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.CIBox);
            this.Controls.Add(this.ISFBox);
            this.Controls.Add(this.FNBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SoundFontInfo";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "OmniMIDI - Information about the SoundFont";
            this.Load += new System.EventHandler(this.OmniMIDISoundfontInfo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox FNBox;
        private System.Windows.Forms.TextBox ISFBox;
        private System.Windows.Forms.TextBox CIBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label SofSFLab;
        private System.Windows.Forms.Label SFfLab;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.RichTextBox CommentRich;
        private System.Windows.Forms.Button CloseBtn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label LELabel;
        private System.ComponentModel.BackgroundWorker PreviewThread;
        private System.Windows.Forms.OpenFileDialog CustomMIDI;
        private System.Windows.Forms.ContextMenu RightClickMenu;
        private System.Windows.Forms.MenuItem StartCustomPrvw;
        private System.Windows.Forms.MenuItem StartNormalPrvw1;
        private System.Windows.Forms.MenuItem StartNormalPrvw2;
        private System.Windows.Forms.MenuItem StartNormalPrvw3;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem LoopYesNo;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.ToolTip WhatDoesTheSFSay;
        private System.Windows.Forms.ToolTip SizeWarning;
        private System.Windows.Forms.Button PrvwBtn;
        private System.Windows.Forms.Label SamF;
        private System.Windows.Forms.Label label8;
    }
}