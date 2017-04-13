namespace KeppySynthConfigurator
{
    partial class InfoDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InfoDialog));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.VerLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.GitHubLink = new System.Windows.Forms.LinkLabel();
            this.LicenseFile = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.WinVer = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.WinName = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CompiledOn = new System.Windows.Forms.Label();
            this.BASSMIDIVer = new System.Windows.Forms.Label();
            this.BASSVer = new System.Windows.Forms.Label();
            this.DriverVer = new System.Windows.Forms.Label();
            this.OKClose = new System.Windows.Forms.Button();
            this.CTC = new System.Windows.Forms.Button();
            this.CFU = new System.Windows.Forms.Button();
            this.DonateBtn = new System.Windows.Forms.Button();
            this.CurBranch = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::KeppySynthConfigurator.Properties.Resources.KSynthLogo;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(79, 80);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // VerLabel
            // 
            this.VerLabel.AutoSize = true;
            this.VerLabel.Location = new System.Drawing.Point(97, 12);
            this.VerLabel.Name = "VerLabel";
            this.VerLabel.Size = new System.Drawing.Size(172, 52);
            this.VerLabel.TabIndex = 1;
            this.VerLabel.Text = "Keppy\'s Synthesizer VERSION\r\n\r\nCopyright Ⓒ 2011\r\nKaleidonKep99, Kode54 && Mudlord" +
    "";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(358, 42);
            this.label2.TabIndex = 2;
            this.label2.Text = "This software is open-source.\r\nRedistribution and use of this code or any derivat" +
    "ive works are permitted provided that the following conditions are met:";
            // 
            // GitHubLink
            // 
            this.GitHubLink.AutoSize = true;
            this.GitHubLink.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.GitHubLink.Location = new System.Drawing.Point(97, 77);
            this.GitHubLink.Name = "GitHubLink";
            this.GitHubLink.Size = new System.Drawing.Size(277, 13);
            this.GitHubLink.TabIndex = 3;
            this.GitHubLink.TabStop = true;
            this.GitHubLink.Text = "https://github.com/KaleidonKep99/Keppy-s-Synthesizer/";
            this.GitHubLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.GitHubLink_LinkClicked);
            // 
            // LicenseFile
            // 
            this.LicenseFile.AutoSize = true;
            this.LicenseFile.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.LicenseFile.Location = new System.Drawing.Point(234, 127);
            this.LicenseFile.Name = "LicenseFile";
            this.LicenseFile.Size = new System.Drawing.Size(85, 13);
            this.LicenseFile.TabIndex = 4;
            this.LicenseFile.TabStop = true;
            this.LicenseFile.Text = "Open license file";
            this.LicenseFile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LicenseFile_LinkClicked);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Driver version:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "BASS version:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "BASSMIDI version:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 77);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Compiled on:";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.WinVer);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.WinName);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Location = new System.Drawing.Point(10, 274);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(362, 59);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Windows installation info";
            // 
            // WinVer
            // 
            this.WinVer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.WinVer.AutoSize = true;
            this.WinVer.Location = new System.Drawing.Point(120, 39);
            this.WinVer.Name = "WinVer";
            this.WinVer.Size = new System.Drawing.Size(61, 13);
            this.WinVer.TabIndex = 9;
            this.WinVer.Text = "WIN VERS";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 39);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Version:";
            // 
            // WinName
            // 
            this.WinName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.WinName.AutoSize = true;
            this.WinName.Location = new System.Drawing.Point(120, 20);
            this.WinName.Name = "WinName";
            this.WinName.Size = new System.Drawing.Size(63, 13);
            this.WinName.TabIndex = 8;
            this.WinName.Text = "WIN NAME";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 20);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(38, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "Name:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.CurBranch);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.CompiledOn);
            this.groupBox1.Controls.Add(this.BASSMIDIVer);
            this.groupBox1.Controls.Add(this.BASSVer);
            this.groupBox1.Controls.Add(this.DriverVer);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(10, 151);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(362, 117);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Driver info";
            // 
            // CompiledOn
            // 
            this.CompiledOn.AutoSize = true;
            this.CompiledOn.Location = new System.Drawing.Point(120, 77);
            this.CompiledOn.Name = "CompiledOn";
            this.CompiledOn.Size = new System.Drawing.Size(63, 13);
            this.CompiledOn.TabIndex = 7;
            this.CompiledOn.Text = "COMP DAT";
            // 
            // BASSMIDIVer
            // 
            this.BASSMIDIVer.AutoSize = true;
            this.BASSMIDIVer.Location = new System.Drawing.Point(120, 58);
            this.BASSMIDIVer.Name = "BASSMIDIVer";
            this.BASSMIDIVer.Size = new System.Drawing.Size(54, 13);
            this.BASSMIDIVer.TabIndex = 6;
            this.BASSMIDIVer.Text = "LIB VER2";
            // 
            // BASSVer
            // 
            this.BASSVer.AutoSize = true;
            this.BASSVer.Location = new System.Drawing.Point(120, 39);
            this.BASSVer.Name = "BASSVer";
            this.BASSVer.Size = new System.Drawing.Size(54, 13);
            this.BASSVer.TabIndex = 5;
            this.BASSVer.Text = "LIB VER1";
            // 
            // DriverVer
            // 
            this.DriverVer.AutoSize = true;
            this.DriverVer.Location = new System.Drawing.Point(120, 20);
            this.DriverVer.Name = "DriverVer";
            this.DriverVer.Size = new System.Drawing.Size(62, 13);
            this.DriverVer.TabIndex = 4;
            this.DriverVer.Text = "DRV VERS";
            // 
            // OKClose
            // 
            this.OKClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKClose.Location = new System.Drawing.Point(286, 339);
            this.OKClose.Name = "OKClose";
            this.OKClose.Size = new System.Drawing.Size(87, 23);
            this.OKClose.TabIndex = 7;
            this.OKClose.Text = "OK";
            this.OKClose.UseVisualStyleBackColor = true;
            this.OKClose.Click += new System.EventHandler(this.OKClose_Click);
            // 
            // CTC
            // 
            this.CTC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CTC.Location = new System.Drawing.Point(9, 339);
            this.CTC.Name = "CTC";
            this.CTC.Size = new System.Drawing.Size(105, 23);
            this.CTC.TabIndex = 8;
            this.CTC.Text = "Copy to clipboard";
            this.CTC.UseVisualStyleBackColor = true;
            this.CTC.Click += new System.EventHandler(this.CTC_Click);
            // 
            // CFU
            // 
            this.CFU.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CFU.Location = new System.Drawing.Point(120, 339);
            this.CFU.Name = "CFU";
            this.CFU.Size = new System.Drawing.Size(105, 23);
            this.CFU.TabIndex = 9;
            this.CFU.Text = "Check for updates";
            this.CFU.UseVisualStyleBackColor = true;
            this.CFU.Click += new System.EventHandler(this.CFU_Click);
            // 
            // DonateBtn
            // 
            this.DonateBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DonateBtn.Image = global::KeppySynthConfigurator.Properties.Resources.ppdonate;
            this.DonateBtn.Location = new System.Drawing.Point(285, 12);
            this.DonateBtn.Name = "DonateBtn";
            this.DonateBtn.Size = new System.Drawing.Size(87, 23);
            this.DonateBtn.TabIndex = 11;
            this.DonateBtn.Text = "Donate";
            this.DonateBtn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.DonateBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.DonateBtn.UseVisualStyleBackColor = true;
            this.DonateBtn.Click += new System.EventHandler(this.DonateBtn_Click);
            // 
            // CurBranch
            // 
            this.CurBranch.AutoSize = true;
            this.CurBranch.Location = new System.Drawing.Point(120, 96);
            this.CurBranch.Name = "CurBranch";
            this.CurBranch.Size = new System.Drawing.Size(52, 13);
            this.CurBranch.TabIndex = 9;
            this.CurBranch.Text = "BRANCH";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 96);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(81, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Update branch:";
            // 
            // InfoDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(382, 371);
            this.Controls.Add(this.DonateBtn);
            this.Controls.Add(this.CFU);
            this.Controls.Add(this.CTC);
            this.Controls.Add(this.OKClose);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.LicenseFile);
            this.Controls.Add(this.GitHubLink);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.VerLabel);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InfoDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Information";
            this.Load += new System.EventHandler(this.InfoDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label VerLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel GitHubLink;
        private System.Windows.Forms.LinkLabel LicenseFile;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label WinVer;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label WinName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label CompiledOn;
        private System.Windows.Forms.Label BASSMIDIVer;
        private System.Windows.Forms.Label BASSVer;
        private System.Windows.Forms.Label DriverVer;
        private System.Windows.Forms.Button OKClose;
        private System.Windows.Forms.Button CTC;
        private System.Windows.Forms.Button CFU;
        private System.Windows.Forms.Button DonateBtn;
        private System.Windows.Forms.Label CurBranch;
        private System.Windows.Forms.Label label9;
    }
}