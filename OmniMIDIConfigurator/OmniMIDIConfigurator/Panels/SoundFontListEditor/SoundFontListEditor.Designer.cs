namespace OmniMIDIConfigurator
{
    partial class SoundFontListEditor
    {
        /// <summary> 
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione componenti

        /// <summary> 
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare 
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] {
            "Mama mia",
            "127",
            "127",
            "127",
            "127",
            "Yes",
            "0",
            "0"}, -1);
            this.SFlg = new System.Windows.Forms.Button();
            this.Separator = new System.Windows.Forms.Label();
            this.EL = new System.Windows.Forms.Button();
            this.LoadToApp = new System.Windows.Forms.Button();
            this.IEL = new System.Windows.Forms.Button();
            this.BankPresetOverride = new System.Windows.Forms.CheckBox();
            this.SelectedListBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.DisableSF = new System.Windows.Forms.Button();
            this.EnableSF = new System.Windows.Forms.Button();
            this.ListOverride = new System.Windows.Forms.Label();
            this.CLi = new System.Windows.Forms.Button();
            this.MvD = new System.Windows.Forms.Button();
            this.MvU = new System.Windows.Forms.Button();
            this.RmvSF = new System.Windows.Forms.Button();
            this.AddSF = new System.Windows.Forms.Button();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SoundFontImport = new System.Windows.Forms.OpenFileDialog();
            this.ExternalListImport = new System.Windows.Forms.OpenFileDialog();
            this.ExternalListExport = new System.Windows.Forms.SaveFileDialog();
            this.LisCM = new System.Windows.Forms.ContextMenu();
            this.OSF = new System.Windows.Forms.MenuItem();
            this.OSFd = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.MSu = new System.Windows.Forms.MenuItem();
            this.MSd = new System.Windows.Forms.MenuItem();
            this.ESF = new System.Windows.Forms.MenuItem();
            this.DSF = new System.Windows.Forms.MenuItem();
            this.ReLSFl = new System.Windows.Forms.MenuItem();
            this.Lis = new OmniMIDIConfigurator.ListViewEx();
            this.SoundFont = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SrcBank = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SrcPres = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DesBank = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DesPres = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.XGDrums = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SFFormat = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SFSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // SFlg
            // 
            this.SFlg.AccessibleDescription = "Shows the guide about the various preset, bank and XG settings etcetera";
            this.SFlg.AccessibleName = "SoundFont list guide";
            this.SFlg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SFlg.BackColor = System.Drawing.Color.Transparent;
            this.SFlg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SFlg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SFlg.ForeColor = System.Drawing.Color.Transparent;
            this.SFlg.Location = new System.Drawing.Point(620, 5);
            this.SFlg.Name = "SFlg";
            this.SFlg.Size = new System.Drawing.Size(24, 24);
            this.SFlg.TabIndex = 4;
            this.SFlg.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.SFlg.UseVisualStyleBackColor = false;
            this.SFlg.Click += new System.EventHandler(this.SFlg_Click);
            this.SFlg.Paint += new System.Windows.Forms.PaintEventHandler(this.SoundFontListGuideButton);
            // 
            // Separator
            // 
            this.Separator.AutoSize = true;
            this.Separator.Enabled = false;
            this.Separator.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Separator.Location = new System.Drawing.Point(170, 4);
            this.Separator.Name = "Separator";
            this.Separator.Size = new System.Drawing.Size(14, 20);
            this.Separator.TabIndex = 31;
            this.Separator.Text = "|";
            // 
            // EL
            // 
            this.EL.AccessibleDescription = "Export a SoundFonts list to an external file";
            this.EL.AccessibleName = "Export SoundFonts list";
            this.EL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.EL.BackColor = System.Drawing.Color.Transparent;
            this.EL.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.EL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.EL.ForeColor = System.Drawing.Color.Transparent;
            this.EL.Location = new System.Drawing.Point(649, 356);
            this.EL.Name = "EL";
            this.EL.Size = new System.Drawing.Size(24, 30);
            this.EL.TabIndex = 14;
            this.EL.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.EL.UseVisualStyleBackColor = false;
            this.EL.Click += new System.EventHandler(this.EL_Click);
            this.EL.Paint += new System.Windows.Forms.PaintEventHandler(this.ImportListButton);
            // 
            // LoadToApp
            // 
            this.LoadToApp.AccessibleDescription = "Load SoundFonts list to app";
            this.LoadToApp.AccessibleName = "Load SoundFonts list to all OmniMIDI instances currently running";
            this.LoadToApp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LoadToApp.BackColor = System.Drawing.Color.Transparent;
            this.LoadToApp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.LoadToApp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LoadToApp.ForeColor = System.Drawing.Color.Transparent;
            this.LoadToApp.Location = new System.Drawing.Point(649, 154);
            this.LoadToApp.Name = "LoadToApp";
            this.LoadToApp.Size = new System.Drawing.Size(24, 24);
            this.LoadToApp.TabIndex = 10;
            this.LoadToApp.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.LoadToApp.UseVisualStyleBackColor = false;
            this.LoadToApp.Click += new System.EventHandler(this.LoadToApp_Click);
            this.LoadToApp.Paint += new System.Windows.Forms.PaintEventHandler(this.ButtonLoad);
            // 
            // IEL
            // 
            this.IEL.AccessibleDescription = "Import a SoundFonts list from an external file";
            this.IEL.AccessibleName = "Import SoundFonts list";
            this.IEL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.IEL.BackColor = System.Drawing.Color.Transparent;
            this.IEL.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.IEL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.IEL.ForeColor = System.Drawing.Color.Transparent;
            this.IEL.Location = new System.Drawing.Point(649, 327);
            this.IEL.Name = "IEL";
            this.IEL.Size = new System.Drawing.Size(24, 30);
            this.IEL.TabIndex = 13;
            this.IEL.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.IEL.UseVisualStyleBackColor = false;
            this.IEL.Click += new System.EventHandler(this.IEL_Click);
            this.IEL.Paint += new System.Windows.Forms.PaintEventHandler(this.ImportListButton);
            // 
            // BankPresetOverride
            // 
            this.BankPresetOverride.AutoSize = true;
            this.BankPresetOverride.Location = new System.Drawing.Point(184, 9);
            this.BankPresetOverride.Name = "BankPresetOverride";
            this.BankPresetOverride.Size = new System.Drawing.Size(355, 17);
            this.BankPresetOverride.TabIndex = 2;
            this.BankPresetOverride.Text = "Import specific bank/preset from SoundFont file and assign it manually";
            this.BankPresetOverride.UseVisualStyleBackColor = true;
            // 
            // SelectedListBox
            // 
            this.SelectedListBox.AccessibleDescription = "Switch between the 8 available lists";
            this.SelectedListBox.AccessibleName = "SoundFonts list switcher";
            this.SelectedListBox.BackColor = System.Drawing.Color.White;
            this.SelectedListBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SelectedListBox.ForeColor = System.Drawing.Color.Black;
            this.SelectedListBox.FormattingEnabled = true;
            this.SelectedListBox.Items.AddRange(new object[] {
            "Shared list",
            "List 2",
            "List 3",
            "List 4",
            "List 5",
            "List 6",
            "List 7",
            "List 8"});
            this.SelectedListBox.Location = new System.Drawing.Point(95, 6);
            this.SelectedListBox.Name = "SelectedListBox";
            this.SelectedListBox.Size = new System.Drawing.Size(74, 21);
            this.SelectedListBox.TabIndex = 1;
            this.SelectedListBox.SelectedIndexChanged += new System.EventHandler(this.SelectedListBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Select list to edit:";
            // 
            // DisableSF
            // 
            this.DisableSF.AccessibleDescription = "Disable selected SoundFonts in the SoundFonts list";
            this.DisableSF.AccessibleName = "Disable selected SoundFonts";
            this.DisableSF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DisableSF.BackColor = System.Drawing.Color.Transparent;
            this.DisableSF.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.DisableSF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DisableSF.ForeColor = System.Drawing.Color.Transparent;
            this.DisableSF.Location = new System.Drawing.Point(649, 214);
            this.DisableSF.Name = "DisableSF";
            this.DisableSF.Size = new System.Drawing.Size(24, 24);
            this.DisableSF.TabIndex = 12;
            this.DisableSF.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.DisableSF.UseVisualStyleBackColor = false;
            this.DisableSF.Click += new System.EventHandler(this.DisableSF_Click);
            this.DisableSF.Paint += new System.Windows.Forms.PaintEventHandler(this.ButtonDisable);
            // 
            // EnableSF
            // 
            this.EnableSF.AccessibleDescription = "Enable selected SoundFonts in the SoundFonts list";
            this.EnableSF.AccessibleName = "Enable selected SoundFonts";
            this.EnableSF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.EnableSF.BackColor = System.Drawing.Color.Transparent;
            this.EnableSF.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.EnableSF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.EnableSF.ForeColor = System.Drawing.Color.Transparent;
            this.EnableSF.Location = new System.Drawing.Point(649, 191);
            this.EnableSF.Name = "EnableSF";
            this.EnableSF.Size = new System.Drawing.Size(24, 24);
            this.EnableSF.TabIndex = 11;
            this.EnableSF.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.EnableSF.UseVisualStyleBackColor = false;
            this.EnableSF.Click += new System.EventHandler(this.EnableSF_Click);
            this.EnableSF.Paint += new System.Windows.Forms.PaintEventHandler(this.ButtonEnable);
            // 
            // ListOverride
            // 
            this.ListOverride.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ListOverride.AutoSize = true;
            this.ListOverride.BackColor = System.Drawing.Color.Transparent;
            this.ListOverride.Enabled = false;
            this.ListOverride.Location = new System.Drawing.Point(1, 422);
            this.ListOverride.Name = "ListOverride";
            this.ListOverride.Size = new System.Drawing.Size(414, 13);
            this.ListOverride.TabIndex = 30;
            this.ListOverride.Text = "The last SoundFont will override the ones above it (loading order is from top to " +
    "bottom).";
            // 
            // CLi
            // 
            this.CLi.AccessibleDescription = "Clears the list that is currently selected";
            this.CLi.AccessibleName = "Clear SoundFont list";
            this.CLi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CLi.BackColor = System.Drawing.Color.Transparent;
            this.CLi.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CLi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CLi.ForeColor = System.Drawing.Color.Transparent;
            this.CLi.Location = new System.Drawing.Point(649, 5);
            this.CLi.Name = "CLi";
            this.CLi.Size = new System.Drawing.Size(24, 24);
            this.CLi.TabIndex = 5;
            this.CLi.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.CLi.UseVisualStyleBackColor = false;
            this.CLi.Click += new System.EventHandler(this.CLi_Click);
            this.CLi.Paint += new System.Windows.Forms.PaintEventHandler(this.ClearListButton);
            // 
            // MvD
            // 
            this.MvD.AccessibleDescription = "Move selected SoundFont down in the SoundFonts list";
            this.MvD.AccessibleName = "Move SoundFont down";
            this.MvD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MvD.BackColor = System.Drawing.Color.Transparent;
            this.MvD.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.MvD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MvD.ForeColor = System.Drawing.Color.Transparent;
            this.MvD.Location = new System.Drawing.Point(649, 117);
            this.MvD.Name = "MvD";
            this.MvD.Size = new System.Drawing.Size(24, 24);
            this.MvD.TabIndex = 9;
            this.MvD.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.MvD.UseVisualStyleBackColor = false;
            this.MvD.Click += new System.EventHandler(this.MvD_Click);
            this.MvD.Paint += new System.Windows.Forms.PaintEventHandler(this.ButtonUpDown);
            // 
            // MvU
            // 
            this.MvU.AccessibleDescription = "Move selected SoundFont up in the SoundFonts list";
            this.MvU.AccessibleName = "Move SoundFont up";
            this.MvU.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MvU.BackColor = System.Drawing.Color.Transparent;
            this.MvU.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.MvU.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MvU.ForeColor = System.Drawing.Color.Transparent;
            this.MvU.Location = new System.Drawing.Point(649, 94);
            this.MvU.Name = "MvU";
            this.MvU.Size = new System.Drawing.Size(24, 24);
            this.MvU.TabIndex = 8;
            this.MvU.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.MvU.UseVisualStyleBackColor = false;
            this.MvU.Click += new System.EventHandler(this.MvU_Click);
            this.MvU.Paint += new System.Windows.Forms.PaintEventHandler(this.ButtonUpDown);
            // 
            // RmvSF
            // 
            this.RmvSF.AccessibleDescription = "Remove SoundFonts to the SoundFonts list";
            this.RmvSF.AccessibleName = "Remove SoundFonts";
            this.RmvSF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RmvSF.BackColor = System.Drawing.Color.Transparent;
            this.RmvSF.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.RmvSF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RmvSF.ForeColor = System.Drawing.Color.Transparent;
            this.RmvSF.Location = new System.Drawing.Point(649, 57);
            this.RmvSF.Name = "RmvSF";
            this.RmvSF.Size = new System.Drawing.Size(24, 24);
            this.RmvSF.TabIndex = 7;
            this.RmvSF.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.RmvSF.UseVisualStyleBackColor = false;
            this.RmvSF.Click += new System.EventHandler(this.RmvSF_Click);
            this.RmvSF.Paint += new System.Windows.Forms.PaintEventHandler(this.ButtonAddRemove);
            // 
            // AddSF
            // 
            this.AddSF.AccessibleDescription = "Add SoundFonts to the SoundFonts list";
            this.AddSF.AccessibleName = "Add SoundFonts";
            this.AddSF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AddSF.BackColor = System.Drawing.Color.Transparent;
            this.AddSF.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.AddSF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddSF.ForeColor = System.Drawing.Color.Transparent;
            this.AddSF.Location = new System.Drawing.Point(649, 34);
            this.AddSF.Name = "AddSF";
            this.AddSF.Size = new System.Drawing.Size(24, 24);
            this.AddSF.TabIndex = 6;
            this.AddSF.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.AddSF.UseVisualStyleBackColor = false;
            this.AddSF.Click += new System.EventHandler(this.AddSF_Click);
            this.AddSF.Paint += new System.Windows.Forms.PaintEventHandler(this.ButtonAddRemove);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "SoundFont";
            this.columnHeader1.Width = 425;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "SB";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 30;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "SP";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 30;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "DB";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader4.Width = 30;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "DP";
            this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader5.Width = 30;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "XG";
            this.columnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader6.Width = 31;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Format";
            this.columnHeader7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader7.Width = 53;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Size";
            this.columnHeader8.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader8.Width = 62;
            // 
            // SoundFontImport
            // 
            this.SoundFontImport.FileName = "Add soundfonts to the list...";
            this.SoundFontImport.Filter = "Soundfont files|*.sf2;*.sfz;*.sfpack;";
            this.SoundFontImport.Multiselect = true;
            this.SoundFontImport.RestoreDirectory = true;
            this.SoundFontImport.SupportMultiDottedExtensions = true;
            // 
            // ExternalListImport
            // 
            this.ExternalListImport.Filter = "Soundfont lists|*.sflist;*.omlist;*.txt;";
            this.ExternalListImport.Multiselect = true;
            this.ExternalListImport.RestoreDirectory = true;
            this.ExternalListImport.SupportMultiDottedExtensions = true;
            // 
            // ExternalListExport
            // 
            this.ExternalListExport.Filter = "Soundfont list|*.omlist;";
            this.ExternalListExport.RestoreDirectory = true;
            // 
            // LisCM
            // 
            this.LisCM.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.OSF,
            this.OSFd,
            this.menuItem3,
            this.ReLSFl,
            this.MSu,
            this.MSd,
            this.ESF,
            this.DSF});
            // 
            // OSF
            // 
            this.OSF.Index = 0;
            this.OSF.Text = "Open SoundFont";
            this.OSF.Click += new System.EventHandler(this.OSF_Click);
            // 
            // OSFd
            // 
            this.OSFd.Index = 1;
            this.OSFd.Text = "Open SoundFont\'s directory";
            this.OSFd.Click += new System.EventHandler(this.OSFd_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 2;
            this.menuItem3.Text = "-";
            // 
            // MSu
            // 
            this.MSu.Index = 4;
            this.MSu.Text = "Move SoundFont up";
            this.MSu.Click += new System.EventHandler(this.MvU_Click);
            // 
            // MSd
            // 
            this.MSd.Index = 5;
            this.MSd.Text = "Move SoundFont down";
            this.MSd.Click += new System.EventHandler(this.MvD_Click);
            // 
            // ESF
            // 
            this.ESF.Index = 6;
            this.ESF.Text = "Enable SoundFont";
            this.ESF.Click += new System.EventHandler(this.EnableSF_Click);
            // 
            // DSF
            // 
            this.DSF.Index = 7;
            this.DSF.Text = "Disable SoundFont";
            this.DSF.Click += new System.EventHandler(this.DisableSF_Click);
            // 
            // ReLSFl
            // 
            this.ReLSFl.Index = 3;
            this.ReLSFl.Text = "(Re)Load SoundFont list";
            this.ReLSFl.Click += new System.EventHandler(this.LoadToApp_Click);
            // 
            // Lis
            // 
            this.Lis.AccessibleDescription = "The SoundFonts list";
            this.Lis.AccessibleName = "SoundFonts list";
            this.Lis.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.Lis.AllowDrop = true;
            this.Lis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Lis.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Lis.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Lis.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.SoundFont,
            this.SrcBank,
            this.SrcPres,
            this.DesBank,
            this.DesPres,
            this.XGDrums,
            this.SFFormat,
            this.SFSize});
            this.Lis.FullRowSelect = true;
            this.Lis.GridLines = true;
            this.Lis.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.Lis.HideSelection = false;
            this.Lis.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem3});
            this.Lis.LabelWrap = false;
            this.Lis.LineAfter = -1;
            this.Lis.LineBefore = -1;
            this.Lis.Location = new System.Drawing.Point(5, 34);
            this.Lis.Name = "Lis";
            this.Lis.ShowGroups = false;
            this.Lis.Size = new System.Drawing.Size(639, 384);
            this.Lis.TabIndex = 3;
            this.Lis.UseCompatibleStateImageBehavior = false;
            this.Lis.View = System.Windows.Forms.View.Details;
            this.Lis.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.Lis_ColumnWidthChanged);
            this.Lis.DragDrop += new System.Windows.Forms.DragEventHandler(this.Lis_DragDrop);
            this.Lis.DragEnter += new System.Windows.Forms.DragEventHandler(this.Lis_DragEnter);
            // 
            // SoundFont
            // 
            this.SoundFont.Text = "SoundFont";
            this.SoundFont.Width = 425;
            // 
            // SrcBank
            // 
            this.SrcBank.Text = "SB";
            this.SrcBank.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SrcBank.Width = 30;
            // 
            // SrcPres
            // 
            this.SrcPres.Text = "SP";
            this.SrcPres.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SrcPres.Width = 30;
            // 
            // DesBank
            // 
            this.DesBank.Text = "DB";
            this.DesBank.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.DesBank.Width = 30;
            // 
            // DesPres
            // 
            this.DesPres.Text = "DP";
            this.DesPres.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.DesPres.Width = 30;
            // 
            // XGDrums
            // 
            this.XGDrums.Text = "XG";
            this.XGDrums.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.XGDrums.Width = 31;
            // 
            // SFFormat
            // 
            this.SFFormat.Text = "Format";
            this.SFFormat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SFFormat.Width = 53;
            // 
            // SFSize
            // 
            this.SFSize.Text = "Size";
            this.SFSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SFSize.Width = 62;
            // 
            // SoundFontListEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Lis);
            this.Controls.Add(this.SFlg);
            this.Controls.Add(this.Separator);
            this.Controls.Add(this.EL);
            this.Controls.Add(this.LoadToApp);
            this.Controls.Add(this.IEL);
            this.Controls.Add(this.BankPresetOverride);
            this.Controls.Add(this.SelectedListBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DisableSF);
            this.Controls.Add(this.EnableSF);
            this.Controls.Add(this.ListOverride);
            this.Controls.Add(this.CLi);
            this.Controls.Add(this.MvD);
            this.Controls.Add(this.MvU);
            this.Controls.Add(this.RmvSF);
            this.Controls.Add(this.AddSF);
            this.Name = "SoundFontListEditor";
            this.Size = new System.Drawing.Size(678, 437);
            this.Load += new System.EventHandler(this.SoundFontListEditor_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.CheckBox BankPresetOverride;
        public System.Windows.Forms.ComboBox SelectedListBox;
        public System.Windows.Forms.Button SFlg;
        public System.Windows.Forms.Label Separator;
        public System.Windows.Forms.ColumnHeader SoundFont;
        public System.Windows.Forms.ColumnHeader SrcBank;
        public System.Windows.Forms.ColumnHeader SrcPres;
        public System.Windows.Forms.ColumnHeader DesBank;
        public System.Windows.Forms.ColumnHeader DesPres;
        public System.Windows.Forms.ColumnHeader XGDrums;
        public System.Windows.Forms.ColumnHeader SFFormat;
        public System.Windows.Forms.ColumnHeader SFSize;
        public System.Windows.Forms.Button EL;
        public System.Windows.Forms.Button LoadToApp;
        public System.Windows.Forms.Button IEL;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Button DisableSF;
        public System.Windows.Forms.Button EnableSF;
        public System.Windows.Forms.Label ListOverride;
        public System.Windows.Forms.Button CLi;
        public System.Windows.Forms.Button MvD;
        public System.Windows.Forms.Button MvU;
        public System.Windows.Forms.Button RmvSF;
        public System.Windows.Forms.Button AddSF;
        public System.Windows.Forms.ColumnHeader columnHeader1;
        public System.Windows.Forms.ColumnHeader columnHeader2;
        public System.Windows.Forms.ColumnHeader columnHeader3;
        public System.Windows.Forms.ColumnHeader columnHeader4;
        public System.Windows.Forms.ColumnHeader columnHeader5;
        public System.Windows.Forms.ColumnHeader columnHeader6;
        public System.Windows.Forms.ColumnHeader columnHeader7;
        public System.Windows.Forms.ColumnHeader columnHeader8;
        public ListViewEx Lis;
        private System.Windows.Forms.OpenFileDialog SoundFontImport;
        public System.Windows.Forms.OpenFileDialog ExternalListImport;
        public System.Windows.Forms.SaveFileDialog ExternalListExport;
        private System.Windows.Forms.ContextMenu LisCM;
        private System.Windows.Forms.MenuItem OSF;
        private System.Windows.Forms.MenuItem OSFd;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem ReLSFl;
        private System.Windows.Forms.MenuItem MSu;
        private System.Windows.Forms.MenuItem MSd;
        private System.Windows.Forms.MenuItem ESF;
        private System.Windows.Forms.MenuItem DSF;
    }
}
