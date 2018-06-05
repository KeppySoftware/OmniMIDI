using System.Windows.Forms;

namespace OmniMIDIConfigurator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KeppySynthBlacklistSystem));
            this.BlackListDef = new System.Windows.Forms.Label();
            this.ProgramsBlackList = new System.Windows.Forms.ListBox();
            this.AddBlacklistedProgram = new System.Windows.Forms.OpenFileDialog();
            this.BlacklistContext = new System.Windows.Forms.ContextMenu();
            this.AE = new System.Windows.Forms.MenuItem();
            this.AEr = new System.Windows.Forms.MenuItem();
            this.RE = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.CBLi = new System.Windows.Forms.MenuItem();
            this.EDBLi = new System.Windows.Forms.Button();
            this.UDBLi = new System.Windows.Forms.Button();
            this.NoBlockMessage = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // BlackListDef
            // 
            this.BlackListDef.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BlackListDef.Location = new System.Drawing.Point(12, 12);
            this.BlackListDef.Name = "BlackListDef";
            this.BlackListDef.Size = new System.Drawing.Size(401, 67);
            this.BlackListDef.TabIndex = 26;
            this.BlackListDef.Text = resources.GetString("BlackListDef.Text");
            this.BlackListDef.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ProgramsBlackList
            // 
            this.ProgramsBlackList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgramsBlackList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ProgramsBlackList.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ProgramsBlackList.FormattingEnabled = true;
            this.ProgramsBlackList.HorizontalScrollbar = true;
            this.ProgramsBlackList.ItemHeight = 15;
            this.ProgramsBlackList.Location = new System.Drawing.Point(5, 91);
            this.ProgramsBlackList.Margin = new System.Windows.Forms.Padding(0);
            this.ProgramsBlackList.Name = "ProgramsBlackList";
            this.ProgramsBlackList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.ProgramsBlackList.Size = new System.Drawing.Size(685, 287);
            this.ProgramsBlackList.TabIndex = 21;
            this.ProgramsBlackList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ProgramsBlackList_KeyDown);
            // 
            // AddBlacklistedProgram
            // 
            this.AddBlacklistedProgram.FileName = "openFileDialog1";
            this.AddBlacklistedProgram.Filter = "PE/Executable files|*.exe;";
            this.AddBlacklistedProgram.Multiselect = true;
            this.AddBlacklistedProgram.ReadOnlyChecked = true;
            this.AddBlacklistedProgram.Title = "Add a program to the blacklist...";
            // 
            // BlacklistContext
            // 
            this.BlacklistContext.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.AE,
            this.AEr,
            this.RE,
            this.menuItem3,
            this.CBLi});
            // 
            // AE
            // 
            this.AE.Index = 0;
            this.AE.Text = "Add executable(s) from file";
            this.AE.Click += new System.EventHandler(this.AddBlackList_Click);
            // 
            // AEr
            // 
            this.AEr.Index = 1;
            this.AEr.Text = "Add executable(s) from running processes";
            this.AEr.Click += new System.EventHandler(this.AEr_Click);
            // 
            // RE
            // 
            this.RE.Index = 2;
            this.RE.Text = "Remove executable(s)";
            this.RE.Click += new System.EventHandler(this.RemoveBlackList_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 3;
            this.menuItem3.Text = "-";
            // 
            // CBLi
            // 
            this.CBLi.Index = 4;
            this.CBLi.Text = "Clear blacklist";
            this.CBLi.Click += new System.EventHandler(this.ClearBlacklist_Click);
            // 
            // EDBLi
            // 
            this.EDBLi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.EDBLi.BackColor = System.Drawing.Color.Transparent;
            this.EDBLi.Location = new System.Drawing.Point(556, 34);
            this.EDBLi.Name = "EDBLi";
            this.EDBLi.Size = new System.Drawing.Size(134, 23);
            this.EDBLi.TabIndex = 32;
            this.EDBLi.Text = "Edit default blacklist";
            this.EDBLi.UseVisualStyleBackColor = false;
            this.EDBLi.Click += new System.EventHandler(this.EDBLi_Click);
            // 
            // UDBLi
            // 
            this.UDBLi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.UDBLi.BackColor = System.Drawing.Color.Transparent;
            this.UDBLi.Location = new System.Drawing.Point(556, 56);
            this.UDBLi.Name = "UDBLi";
            this.UDBLi.Size = new System.Drawing.Size(134, 23);
            this.UDBLi.TabIndex = 33;
            this.UDBLi.Text = "Update default blacklist";
            this.UDBLi.UseVisualStyleBackColor = false;
            this.UDBLi.Click += new System.EventHandler(this.UDBLi_Click);
            // 
            // NoBlockMessage
            // 
            this.NoBlockMessage.AutoSize = true;
            this.NoBlockMessage.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.NoBlockMessage.Location = new System.Drawing.Point(464, 12);
            this.NoBlockMessage.Name = "NoBlockMessage";
            this.NoBlockMessage.Size = new System.Drawing.Size(226, 17);
            this.NoBlockMessage.TabIndex = 34;
            this.NoBlockMessage.Text = "Don\'t show message on blocked programs";
            this.NoBlockMessage.UseVisualStyleBackColor = true;
            this.NoBlockMessage.CheckedChanged += new System.EventHandler(this.NoBlockMessage_CheckedChanged);
            // 
            // KeppySynthBlacklistSystem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(695, 385);
            this.Controls.Add(this.NoBlockMessage);
            this.Controls.Add(this.UDBLi);
            this.Controls.Add(this.EDBLi);
            this.Controls.Add(this.BlackListDef);
            this.Controls.Add(this.ProgramsBlackList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "OmniMIDIBlacklistSystem";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Blacklist manager";
            this.Load += new System.EventHandler(this.KeppyDriverBlacklistSystem_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal System.Windows.Forms.Label BlackListDef;
        internal System.Windows.Forms.ListBox ProgramsBlackList;
        private System.Windows.Forms.OpenFileDialog AddBlacklistedProgram;
        private ContextMenu BlacklistContext;
        private MenuItem AE;
        private MenuItem RE;
        private MenuItem menuItem3;
        private MenuItem CBLi;
        private Button EDBLi;
        private Button UDBLi;
        private CheckBox NoBlockMessage;
        private MenuItem AEr;
    }
}