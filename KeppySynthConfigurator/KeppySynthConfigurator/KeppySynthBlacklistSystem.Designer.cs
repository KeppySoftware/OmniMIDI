namespace KeppySynthConfigurator
{
    partial class KeppySynthBlacklistSystem
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
            this.ClearBlacklist = new System.Windows.Forms.Button();
            this.Label13 = new System.Windows.Forms.Label();
            this.ManualBlackListLabel = new System.Windows.Forms.Label();
            this.ManualBlackList = new System.Windows.Forms.TextBox();
            this.BlackListDef = new System.Windows.Forms.Label();
            this.BlackListAdvancedMode = new System.Windows.Forms.CheckBox();
            this.RemoveBlackList = new System.Windows.Forms.Button();
            this.AddBlackList = new System.Windows.Forms.Button();
            this.ProgramsBlackList = new System.Windows.Forms.ListBox();
            this.AddBlacklistedProgram = new System.Windows.Forms.OpenFileDialog();
            this.DefBlackListEdit = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // ClearBlacklist
            // 
            this.ClearBlacklist.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ClearBlacklist.Location = new System.Drawing.Point(561, 61);
            this.ClearBlacklist.Name = "ClearBlacklist";
            this.ClearBlacklist.Size = new System.Drawing.Size(122, 23);
            this.ClearBlacklist.TabIndex = 30;
            this.ClearBlacklist.Text = "Clear the  list";
            this.ClearBlacklist.UseVisualStyleBackColor = true;
            this.ClearBlacklist.Click += new System.EventHandler(this.ClearBlacklist_Click);
            // 
            // Label13
            // 
            this.Label13.AutoSize = true;
            this.Label13.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Label13.Location = new System.Drawing.Point(381, 374);
            this.Label13.Name = "Label13";
            this.Label13.Size = new System.Drawing.Size(312, 13);
            this.Label13.TabIndex = 29;
            this.Label13.Text = "Tip: Names are NOT case sensitive, and spaces are recognized.";
            // 
            // ManualBlackListLabel
            // 
            this.ManualBlackListLabel.Enabled = false;
            this.ManualBlackListLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ManualBlackListLabel.Location = new System.Drawing.Point(12, 65);
            this.ManualBlackListLabel.Name = "ManualBlackListLabel";
            this.ManualBlackListLabel.Size = new System.Drawing.Size(221, 13);
            this.ManualBlackListLabel.TabIndex = 28;
            this.ManualBlackListLabel.Text = "Full path to the program (with .exe extension):";
            this.ManualBlackListLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ManualBlackList
            // 
            this.ManualBlackList.Enabled = false;
            this.ManualBlackList.Location = new System.Drawing.Point(234, 62);
            this.ManualBlackList.Name = "ManualBlackList";
            this.ManualBlackList.Size = new System.Drawing.Size(269, 20);
            this.ManualBlackList.TabIndex = 27;
            // 
            // BlackListDef
            // 
            this.BlackListDef.AutoSize = true;
            this.BlackListDef.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BlackListDef.Location = new System.Drawing.Point(12, 16);
            this.BlackListDef.Name = "BlackListDef";
            this.BlackListDef.Size = new System.Drawing.Size(239, 13);
            this.BlackListDef.TabIndex = 26;
            this.BlackListDef.Text = "Select a program by clicking \'\'Add executable(s)\'\'.";
            // 
            // BlackListAdvancedMode
            // 
            this.BlackListAdvancedMode.AutoSize = true;
            this.BlackListAdvancedMode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BlackListAdvancedMode.Location = new System.Drawing.Point(15, 45);
            this.BlackListAdvancedMode.Name = "BlackListAdvancedMode";
            this.BlackListAdvancedMode.Size = new System.Drawing.Size(229, 17);
            this.BlackListAdvancedMode.TabIndex = 25;
            this.BlackListAdvancedMode.Text = "I want to add the program\'s name by myself";
            this.BlackListAdvancedMode.UseVisualStyleBackColor = true;
            this.BlackListAdvancedMode.CheckedChanged += new System.EventHandler(this.BlackListAdvancedMode_CheckedChanged);
            // 
            // RemoveBlackList
            // 
            this.RemoveBlackList.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.RemoveBlackList.Location = new System.Drawing.Point(561, 36);
            this.RemoveBlackList.Name = "RemoveBlackList";
            this.RemoveBlackList.Size = new System.Drawing.Size(122, 23);
            this.RemoveBlackList.TabIndex = 23;
            this.RemoveBlackList.Text = "Remove executable(s)";
            this.RemoveBlackList.UseVisualStyleBackColor = true;
            this.RemoveBlackList.Click += new System.EventHandler(this.RemoveBlackList_Click);
            // 
            // AddBlackList
            // 
            this.AddBlackList.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.AddBlackList.Location = new System.Drawing.Point(561, 11);
            this.AddBlackList.Name = "AddBlackList";
            this.AddBlackList.Size = new System.Drawing.Size(122, 23);
            this.AddBlackList.TabIndex = 22;
            this.AddBlackList.Text = "Add executable(s)";
            this.AddBlackList.UseVisualStyleBackColor = true;
            this.AddBlackList.Click += new System.EventHandler(this.AddBlackList_Click);
            // 
            // ProgramsBlackList
            // 
            this.ProgramsBlackList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ProgramsBlackList.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ProgramsBlackList.FormattingEnabled = true;
            this.ProgramsBlackList.HorizontalScrollbar = true;
            this.ProgramsBlackList.ItemHeight = 15;
            this.ProgramsBlackList.Location = new System.Drawing.Point(5, 98);
            this.ProgramsBlackList.Margin = new System.Windows.Forms.Padding(0);
            this.ProgramsBlackList.Name = "ProgramsBlackList";
            this.ProgramsBlackList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.ProgramsBlackList.Size = new System.Drawing.Size(685, 272);
            this.ProgramsBlackList.TabIndex = 21;
            // 
            // AddBlacklistedProgram
            // 
            this.AddBlacklistedProgram.FileName = "openFileDialog1";
            this.AddBlacklistedProgram.Filter = "PE/Executable files|*.exe;";
            this.AddBlacklistedProgram.Multiselect = true;
            this.AddBlacklistedProgram.ReadOnlyChecked = true;
            this.AddBlacklistedProgram.Title = "Add a program to the blacklist...";
            // 
            // DefBlackListEdit
            // 
            this.DefBlackListEdit.AutoSize = true;
            this.DefBlackListEdit.Location = new System.Drawing.Point(6, 374);
            this.DefBlackListEdit.Name = "DefBlackListEdit";
            this.DefBlackListEdit.Size = new System.Drawing.Size(176, 13);
            this.DefBlackListEdit.TabIndex = 31;
            this.DefBlackListEdit.TabStop = true;
            this.DefBlackListEdit.Text = "How can I edit the default blacklist?";
            this.DefBlackListEdit.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.DefBlackListEdit_LinkClicked);
            // 
            // KeppyDriverBlacklistSystem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(695, 393);
            this.Controls.Add(this.DefBlackListEdit);
            this.Controls.Add(this.ClearBlacklist);
            this.Controls.Add(this.Label13);
            this.Controls.Add(this.ManualBlackListLabel);
            this.Controls.Add(this.ManualBlackList);
            this.Controls.Add(this.BlackListDef);
            this.Controls.Add(this.BlackListAdvancedMode);
            this.Controls.Add(this.RemoveBlackList);
            this.Controls.Add(this.AddBlackList);
            this.Controls.Add(this.ProgramsBlackList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "KeppyDriverBlacklistSystem";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Blacklist manager";
            this.Load += new System.EventHandler(this.KeppyDriverBlacklistSystem_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button ClearBlacklist;
        internal System.Windows.Forms.Label Label13;
        internal System.Windows.Forms.Label ManualBlackListLabel;
        internal System.Windows.Forms.TextBox ManualBlackList;
        internal System.Windows.Forms.Label BlackListDef;
        internal System.Windows.Forms.CheckBox BlackListAdvancedMode;
        internal System.Windows.Forms.Button RemoveBlackList;
        internal System.Windows.Forms.Button AddBlackList;
        internal System.Windows.Forms.ListBox ProgramsBlackList;
        private System.Windows.Forms.OpenFileDialog AddBlacklistedProgram;
        private System.Windows.Forms.LinkLabel DefBlackListEdit;
    }
}