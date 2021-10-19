using System.Windows.Forms;

namespace OmniMIDIConfigurator
{
    partial class BlacklistSystem
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BlacklistSystem));
            this.BlackListDef = new System.Windows.Forms.Label();
            this.ProgramsBlackList = new System.Windows.Forms.ListBox();
            this.AddBlacklistedProgram = new System.Windows.Forms.OpenFileDialog();
            this.BlacklistContext = new System.Windows.Forms.ContextMenu();
            this.EDBLi = new System.Windows.Forms.Button();
            this.UDBLi = new System.Windows.Forms.Button();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // BlackListDef
            // 
            this.BlackListDef.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BlackListDef.Location = new System.Drawing.Point(12, 6);
            this.BlackListDef.Name = "BlackListDef";
            this.BlackListDef.Size = new System.Drawing.Size(514, 81);
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
            this.ProgramsBlackList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProgramsBlackList.FormattingEnabled = true;
            this.ProgramsBlackList.HorizontalScrollbar = true;
            this.ProgramsBlackList.IntegralHeight = false;
            this.ProgramsBlackList.Location = new System.Drawing.Point(9, 95);
            this.ProgramsBlackList.Margin = new System.Windows.Forms.Padding(0);
            this.ProgramsBlackList.Name = "ProgramsBlackList";
            this.ProgramsBlackList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.ProgramsBlackList.Size = new System.Drawing.Size(677, 332);
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
            // EDBLi
            // 
            this.EDBLi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.EDBLi.BackColor = System.Drawing.Color.Transparent;
            this.EDBLi.Location = new System.Drawing.Point(550, 29);
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
            this.UDBLi.Location = new System.Drawing.Point(550, 51);
            this.UDBLi.Name = "UDBLi";
            this.UDBLi.Size = new System.Drawing.Size(134, 23);
            this.UDBLi.TabIndex = 33;
            this.UDBLi.Text = "Update default blacklist";
            this.UDBLi.UseVisualStyleBackColor = false;
            this.UDBLi.Click += new System.EventHandler(this.UDBLi_Click);
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem2,
            this.menuItem3,
            this.menuItem4,
            this.menuItem5,
            this.menuItem6});
            this.menuItem1.Text = "File";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 0;
            this.menuItem2.Text = "Add executable(s) from file";
            this.menuItem2.Click += new System.EventHandler(this.AddBlackList_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 1;
            this.menuItem3.Text = "Add executable(s) from running processes";
            this.menuItem3.Click += new System.EventHandler(this.AEr_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            this.menuItem4.Text = "Remove executable(s)";
            this.menuItem4.Click += new System.EventHandler(this.RemoveBlackList_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 3;
            this.menuItem5.Text = "-";
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 4;
            this.menuItem6.Text = "Clear blacklist";
            this.menuItem6.Click += new System.EventHandler(this.ClearBlacklist_Click);
            // 
            // BlacklistSystem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(695, 437);
            this.Controls.Add(this.UDBLi);
            this.Controls.Add(this.EDBLi);
            this.Controls.Add(this.BlackListDef);
            this.Controls.Add(this.ProgramsBlackList);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Menu = this.mainMenu1;
            this.Name = "BlacklistSystem";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Blacklist manager";
            this.Load += new System.EventHandler(this.KeppyDriverBlacklistSystem_Load);
            this.ResumeLayout(false);

        }

        #endregion
        internal System.Windows.Forms.Label BlackListDef;
        internal System.Windows.Forms.ListBox ProgramsBlackList;
        private System.Windows.Forms.OpenFileDialog AddBlacklistedProgram;
        private ContextMenu BlacklistContext;
        private Button EDBLi;
        private Button UDBLi;
        private MainMenu mainMenu1;
        private MenuItem menuItem1;
        private MenuItem menuItem2;
        private MenuItem menuItem3;
        private MenuItem menuItem4;
        private MenuItem menuItem5;
        private MenuItem menuItem6;
    }
}