namespace OmniMIDIConfigurator
{
    partial class RivaTunerSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RivaTunerSettings));
            this.BlackListDef = new System.Windows.Forms.Label();
            this.ProgramsList = new System.Windows.Forms.ListBox();
            this.AddAllowedProgram = new System.Windows.Forms.OpenFileDialog();
            this.RivaOSDContext = new System.Windows.Forms.ContextMenu();
            this.AE = new System.Windows.Forms.MenuItem();
            this.AEr = new System.Windows.Forms.MenuItem();
            this.RE = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.CBLi = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // BlackListDef
            // 
            this.BlackListDef.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BlackListDef.Location = new System.Drawing.Point(12, 9);
            this.BlackListDef.Name = "BlackListDef";
            this.BlackListDef.Size = new System.Drawing.Size(519, 67);
            this.BlackListDef.TabIndex = 35;
            this.BlackListDef.Text = resources.GetString("BlackListDef.Text");
            this.BlackListDef.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ProgramsList
            // 
            this.ProgramsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgramsList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ProgramsList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProgramsList.FormattingEnabled = true;
            this.ProgramsList.HorizontalScrollbar = true;
            this.ProgramsList.IntegralHeight = false;
            this.ProgramsList.Location = new System.Drawing.Point(9, 86);
            this.ProgramsList.Margin = new System.Windows.Forms.Padding(0);
            this.ProgramsList.Name = "ProgramsList";
            this.ProgramsList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.ProgramsList.Size = new System.Drawing.Size(522, 290);
            this.ProgramsList.TabIndex = 34;
            // 
            // AddAllowedProgram
            // 
            this.AddAllowedProgram.FileName = "openFileDialog1";
            this.AddAllowedProgram.Filter = "PE/Executable files|*.exe;";
            this.AddAllowedProgram.Multiselect = true;
            this.AddAllowedProgram.ReadOnlyChecked = true;
            this.AddAllowedProgram.Title = "Add a program to the blacklist...";
            // 
            // RivaOSDContext
            // 
            this.RivaOSDContext.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
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
            this.AE.Click += new System.EventHandler(this.AE_Click);
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
            this.RE.Click += new System.EventHandler(this.RE_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 3;
            this.menuItem3.Text = "-";
            // 
            // CBLi
            // 
            this.CBLi.Index = 4;
            this.CBLi.Text = "Clear list";
            this.CBLi.Click += new System.EventHandler(this.CBLi_Click);
            // 
            // RivaTunerSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 385);
            this.Controls.Add(this.BlackListDef);
            this.Controls.Add(this.ProgramsList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RivaTunerSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Apps allowed to use RivaTuner\'s OSD";
            this.Load += new System.EventHandler(this.RivaTunerSettings_Load);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Label BlackListDef;
        internal System.Windows.Forms.ListBox ProgramsList;
        private System.Windows.Forms.OpenFileDialog AddAllowedProgram;
        private System.Windows.Forms.ContextMenu RivaOSDContext;
        private System.Windows.Forms.MenuItem AE;
        private System.Windows.Forms.MenuItem AEr;
        private System.Windows.Forms.MenuItem RE;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem CBLi;
    }
}