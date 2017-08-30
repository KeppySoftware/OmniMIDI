namespace KeppySynthConfigurator
{
    partial class Telemetry
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
            this.NicknameVal = new System.Windows.Forms.TextBox();
            this.EmailVal = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CountryVal = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.AgeVal = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.SoundCards = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.InstGPUVal = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.OSVal = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.InstCPUVal = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.InstRAMVal = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.OkBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.LetUserEditSpecs = new System.Windows.Forms.CheckBox();
            this.DiscLabel = new System.Windows.Forms.LinkLabel();
            this.HelpProvider = new System.Windows.Forms.HelpProvider();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.AdditionalFeed = new System.Windows.Forms.RichTextBox();
            this.BugReport = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "(Nick)Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "E-mail:";
            // 
            // NicknameVal
            // 
            this.NicknameVal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HelpProvider.SetHelpString(this.NicknameVal, "This field is mandatory. I need it to be able to identify you.");
            this.NicknameVal.Location = new System.Drawing.Point(89, 17);
            this.NicknameVal.Name = "NicknameVal";
            this.HelpProvider.SetShowHelp(this.NicknameVal, true);
            this.NicknameVal.Size = new System.Drawing.Size(273, 20);
            this.NicknameVal.TabIndex = 2;
            // 
            // EmailVal
            // 
            this.EmailVal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EmailVal.Location = new System.Drawing.Point(89, 43);
            this.EmailVal.Name = "EmailVal";
            this.EmailVal.Size = new System.Drawing.Size(273, 20);
            this.EmailVal.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.CountryVal);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.AgeVal);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.NicknameVal);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.EmailVal);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(372, 125);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Personal information";
            // 
            // CountryVal
            // 
            this.CountryVal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CountryVal.Location = new System.Drawing.Point(89, 95);
            this.CountryVal.Name = "CountryVal";
            this.CountryVal.Size = new System.Drawing.Size(273, 20);
            this.CountryVal.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Country:";
            // 
            // AgeVal
            // 
            this.AgeVal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AgeVal.Location = new System.Drawing.Point(89, 69);
            this.AgeVal.Name = "AgeVal";
            this.AgeVal.Size = new System.Drawing.Size(273, 20);
            this.AgeVal.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Age";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.SoundCards);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.InstGPUVal);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.OSVal);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.InstCPUVal);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.InstRAMVal);
            this.groupBox2.Controls.Add(this.label7);
            this.HelpProvider.SetHelpString(this.groupBox2, "All the fields here are mandatory.");
            this.groupBox2.Location = new System.Drawing.Point(12, 143);
            this.groupBox2.Name = "groupBox2";
            this.HelpProvider.SetShowHelp(this.groupBox2, true);
            this.groupBox2.Size = new System.Drawing.Size(372, 153);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "PC specifications";
            // 
            // SoundCards
            // 
            this.SoundCards.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SoundCards.FormattingEnabled = true;
            this.SoundCards.Items.AddRange(new object[] {
            "No device selected"});
            this.SoundCards.Location = new System.Drawing.Point(166, 121);
            this.SoundCards.Name = "SoundCards";
            this.SoundCards.Size = new System.Drawing.Size(196, 21);
            this.SoundCards.TabIndex = 10;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(6, 124);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(154, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Select your default sound card:";
            // 
            // InstGPUVal
            // 
            this.InstGPUVal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InstGPUVal.Enabled = false;
            this.InstGPUVal.Location = new System.Drawing.Point(130, 95);
            this.InstGPUVal.Name = "InstGPUVal";
            this.InstGPUVal.Size = new System.Drawing.Size(232, 20);
            this.InstGPUVal.TabIndex = 7;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(6, 98);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(90, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "Graphics card:";
            // 
            // OSVal
            // 
            this.OSVal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OSVal.Enabled = false;
            this.OSVal.Location = new System.Drawing.Point(130, 69);
            this.OSVal.Name = "OSVal";
            this.OSVal.Size = new System.Drawing.Size(232, 20);
            this.OSVal.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(6, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(108, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Operating system:";
            // 
            // InstCPUVal
            // 
            this.InstCPUVal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InstCPUVal.Enabled = false;
            this.InstCPUVal.Location = new System.Drawing.Point(130, 17);
            this.InstCPUVal.Name = "InstCPUVal";
            this.InstCPUVal.Size = new System.Drawing.Size(232, 20);
            this.InstCPUVal.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Processor:";
            // 
            // InstRAMVal
            // 
            this.InstRAMVal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InstRAMVal.Enabled = false;
            this.InstRAMVal.Location = new System.Drawing.Point(130, 43);
            this.InstRAMVal.Name = "InstRAMVal";
            this.InstRAMVal.Size = new System.Drawing.Size(232, 20);
            this.InstRAMVal.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(6, 46);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(90, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Installed RAM:";
            // 
            // OkBtn
            // 
            this.OkBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OkBtn.Location = new System.Drawing.Point(534, 307);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(75, 23);
            this.OkBtn.TabIndex = 7;
            this.OkBtn.Text = "Send";
            this.OkBtn.UseVisualStyleBackColor = true;
            this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelBtn.Location = new System.Drawing.Point(453, 307);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 8;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // LetUserEditSpecs
            // 
            this.LetUserEditSpecs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LetUserEditSpecs.AutoSize = true;
            this.LetUserEditSpecs.Location = new System.Drawing.Point(12, 311);
            this.LetUserEditSpecs.Name = "LetUserEditSpecs";
            this.LetUserEditSpecs.Size = new System.Drawing.Size(128, 17);
            this.LetUserEditSpecs.TabIndex = 9;
            this.LetUserEditSpecs.Text = "Edit PC specifications";
            this.LetUserEditSpecs.UseVisualStyleBackColor = true;
            this.LetUserEditSpecs.CheckedChanged += new System.EventHandler(this.LetUserEditSpecs_CheckedChanged);
            // 
            // DiscLabel
            // 
            this.DiscLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DiscLabel.AutoSize = true;
            this.DiscLabel.Location = new System.Drawing.Point(392, 312);
            this.DiscLabel.Name = "DiscLabel";
            this.DiscLabel.Size = new System.Drawing.Size(55, 13);
            this.DiscLabel.TabIndex = 10;
            this.DiscLabel.TabStop = true;
            this.DiscLabel.Text = "Disclaimer";
            this.DiscLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.DiscLabel_LinkClicked);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.AdditionalFeed);
            this.groupBox3.Location = new System.Drawing.Point(390, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(219, 284);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Additional feedback";
            // 
            // AdditionalFeed
            // 
            this.AdditionalFeed.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AdditionalFeed.Location = new System.Drawing.Point(6, 15);
            this.AdditionalFeed.Name = "AdditionalFeed";
            this.AdditionalFeed.Size = new System.Drawing.Size(207, 263);
            this.AdditionalFeed.TabIndex = 0;
            this.AdditionalFeed.Text = "";
            // 
            // BugReport
            // 
            this.BugReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BugReport.Location = new System.Drawing.Point(426, 260);
            this.BugReport.Name = "BugReport";
            this.BugReport.Size = new System.Drawing.Size(154, 36);
            this.BugReport.TabIndex = 11;
            this.BugReport.Text = "Check if this is a bug report";
            this.BugReport.UseVisualStyleBackColor = true;
            this.BugReport.Visible = false;
            // 
            // Telemetry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(621, 342);
            this.Controls.Add(this.BugReport);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.DiscLabel);
            this.Controls.Add(this.LetUserEditSpecs);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OkBtn);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Telemetry";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Telemetry";
            this.Load += new System.EventHandler(this.Telemetry_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox NicknameVal;
        private System.Windows.Forms.TextBox EmailVal;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox CountryVal;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox AgeVal;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox InstGPUVal;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox OSVal;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox InstCPUVal;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox InstRAMVal;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button OkBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.CheckBox LetUserEditSpecs;
        private System.Windows.Forms.LinkLabel DiscLabel;
        private System.Windows.Forms.HelpProvider HelpProvider;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RichTextBox AdditionalFeed;
        private System.Windows.Forms.ComboBox SoundCards;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox BugReport;
    }
}