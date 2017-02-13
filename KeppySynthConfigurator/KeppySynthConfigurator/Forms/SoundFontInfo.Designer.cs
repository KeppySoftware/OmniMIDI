namespace KeppySynthConfigurator
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
            this.PrvwBtn = new System.Windows.Forms.Button();
            this.PreviewThread = new System.ComponentModel.BackgroundWorker();
            this.CustomMIDI = new System.Windows.Forms.OpenFileDialog();
            this.RightClickMenu = new System.Windows.Forms.ContextMenu();
            this.StartNormalPrvw1 = new System.Windows.Forms.MenuItem();
            this.StartCustomPrvw = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.StartNormalPrvw2 = new System.Windows.Forms.MenuItem();
            this.StartNormalPrvw3 = new System.Windows.Forms.MenuItem();
            this.LoopYesNo = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Filename:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Internal SoundFont name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Copyright information:";
            // 
            // FNBox
            // 
            this.FNBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FNBox.Location = new System.Drawing.Point(142, 6);
            this.FNBox.Name = "FNBox";
            this.FNBox.ReadOnly = true;
            this.FNBox.Size = new System.Drawing.Size(377, 20);
            this.FNBox.TabIndex = 3;
            // 
            // ISFBox
            // 
            this.ISFBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ISFBox.Location = new System.Drawing.Point(142, 32);
            this.ISFBox.Name = "ISFBox";
            this.ISFBox.ReadOnly = true;
            this.ISFBox.Size = new System.Drawing.Size(377, 20);
            this.ISFBox.TabIndex = 4;
            // 
            // CIBox
            // 
            this.CIBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CIBox.Location = new System.Drawing.Point(142, 58);
            this.CIBox.Name = "CIBox";
            this.CIBox.ReadOnly = true;
            this.CIBox.Size = new System.Drawing.Size(377, 20);
            this.CIBox.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Size of the SoundFont:";
            // 
            // SofSFLab
            // 
            this.SofSFLab.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SofSFLab.Location = new System.Drawing.Point(142, 87);
            this.SofSFLab.Name = "SofSFLab";
            this.SofSFLab.Size = new System.Drawing.Size(377, 13);
            this.SofSFLab.TabIndex = 8;
            this.SofSFLab.Text = "label5";
            // 
            // SFfLab
            // 
            this.SFfLab.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SFfLab.Location = new System.Drawing.Point(142, 113);
            this.SFfLab.Name = "SFfLab";
            this.SFfLab.Size = new System.Drawing.Size(377, 13);
            this.SFfLab.TabIndex = 10;
            this.SFfLab.Text = "label6";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 113);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(94, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "SoundFont format:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 139);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(54, 13);
            this.label9.TabIndex = 12;
            this.label9.Text = "Comment:";
            // 
            // CommentRich
            // 
            this.CommentRich.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CommentRich.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CommentRich.Location = new System.Drawing.Point(142, 136);
            this.CommentRich.Name = "CommentRich";
            this.CommentRich.ReadOnly = true;
            this.CommentRich.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.CommentRich.Size = new System.Drawing.Size(377, 140);
            this.CommentRich.TabIndex = 13;
            this.CommentRich.Text = "";
            // 
            // CloseBtn
            // 
            this.CloseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseBtn.Location = new System.Drawing.Point(444, 282);
            this.CloseBtn.Name = "CloseBtn";
            this.CloseBtn.Size = new System.Drawing.Size(75, 23);
            this.CloseBtn.TabIndex = 14;
            this.CloseBtn.Text = "OK";
            this.CloseBtn.UseVisualStyleBackColor = true;
            this.CloseBtn.Click += new System.EventHandler(this.CloseBtn_Click);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 287);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Last edited on:";
            // 
            // LELabel
            // 
            this.LELabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LELabel.Location = new System.Drawing.Point(142, 287);
            this.LELabel.Name = "LELabel";
            this.LELabel.Size = new System.Drawing.Size(296, 13);
            this.LELabel.TabIndex = 16;
            this.LELabel.Text = "SAS";
            // 
            // PrvwBtn
            // 
            this.PrvwBtn.Location = new System.Drawing.Point(305, 282);
            this.PrvwBtn.Name = "PrvwBtn";
            this.PrvwBtn.Size = new System.Drawing.Size(133, 23);
            this.PrvwBtn.TabIndex = 17;
            this.PrvwBtn.Text = "Play SoundFont preview";
            this.PrvwBtn.UseVisualStyleBackColor = true;
            this.PrvwBtn.Click += new System.EventHandler(this.PrvwBtn_Click);
            // 
            // PreviewThread
            // 
            this.PreviewThread.DoWork += new System.ComponentModel.DoWorkEventHandler(this.PreviewThread_DoWork);
            // 
            // CustomMIDI
            // 
            this.CustomMIDI.Filter = "MIDI files|*.mid;*.midi;*.rmi";
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
            // StartNormalPrvw1
            // 
            this.StartNormalPrvw1.Index = 2;
            this.StartNormalPrvw1.Text = "Preview with Town";
            this.StartNormalPrvw1.Click += new System.EventHandler(this.StartNormalPrvw_Click);
            // 
            // StartCustomPrvw
            // 
            this.StartCustomPrvw.Index = 6;
            this.StartCustomPrvw.Text = "Preview with a custom MIDI...";
            this.StartCustomPrvw.Click += new System.EventHandler(this.StartCustomPrvw_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 5;
            this.menuItem1.Text = "-";
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
            // SoundFontInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(525, 312);
            this.ControlBox = false;
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
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SoundFontInfo";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Keppy\'s Synthesizer - Information about the SoundFont";
            this.Load += new System.EventHandler(this.KeppySynthSoundfontInfo_Load);
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
        private System.Windows.Forms.Button PrvwBtn;
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
    }
}