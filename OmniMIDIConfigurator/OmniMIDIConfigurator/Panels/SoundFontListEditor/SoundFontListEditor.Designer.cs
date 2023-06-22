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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] {
            "Mama mia",
            "127",
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
            this.LisCM = new System.Windows.Forms.ContextMenu();
            this.OSF = new System.Windows.Forms.MenuItem();
            this.OSFd = new System.Windows.Forms.MenuItem();
            this.EBPV = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.CSFs = new System.Windows.Forms.MenuItem();
            this.PSFs = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.ReLSFl = new System.Windows.Forms.MenuItem();
            this.MSu = new System.Windows.Forms.MenuItem();
            this.MSd = new System.Windows.Forms.MenuItem();
            this.ESF = new System.Windows.Forms.MenuItem();
            this.DSF = new System.Windows.Forms.MenuItem();
            this.Preload = new System.Windows.Forms.Button();
            this.Lis = new OmniMIDIConfigurator.ListViewEx();
            this.SoundFont = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SrcBank = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SrcPres = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DesBank = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DesPres = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DesBankLSB = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.XGDrums = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PreloadSF = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SFFormat = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SFSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ButtonToolTip = new System.Windows.Forms.ToolTip(this.components);
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
            this.SFlg.Location = new System.Drawing.Point(723, 6);
            this.SFlg.Name = "SFlg";
            this.SFlg.Size = new System.Drawing.Size(28, 28);
            this.SFlg.TabIndex = 4;
            this.SFlg.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.ButtonToolTip.SetToolTip(this.SFlg, "How does this panel work?");
            this.SFlg.UseVisualStyleBackColor = false;
            this.SFlg.Click += new System.EventHandler(this.SFlg_Click);
            this.SFlg.Paint += new System.Windows.Forms.PaintEventHandler(this.SoundFontListGuideButton);
            // 
            // Separator
            // 
            this.Separator.AutoSize = true;
            this.Separator.Enabled = false;
            this.Separator.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Separator.Location = new System.Drawing.Point(198, 5);
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
            this.EL.Location = new System.Drawing.Point(757, 403);
            this.EL.Name = "EL";
            this.EL.Size = new System.Drawing.Size(28, 35);
            this.EL.TabIndex = 14;
            this.EL.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.ButtonToolTip.SetToolTip(this.EL, "Export SoundFont list to a file...");
            this.EL.UseVisualStyleBackColor = false;
            this.EL.Click += new System.EventHandler(this.EL_Click);
            this.EL.Paint += new System.Windows.Forms.PaintEventHandler(this.ImportListButton);
            // 
            // LoadToApp
            // 
            this.LoadToApp.AccessibleDescription = "Load SoundFonts list to all OmniMIDI instances currently running";
            this.LoadToApp.AccessibleName = "Load SoundFonts list to app";
            this.LoadToApp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LoadToApp.BackColor = System.Drawing.Color.Transparent;
            this.LoadToApp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.LoadToApp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LoadToApp.ForeColor = System.Drawing.Color.Transparent;
            this.LoadToApp.Location = new System.Drawing.Point(757, 108);
            this.LoadToApp.Name = "LoadToApp";
            this.LoadToApp.Size = new System.Drawing.Size(28, 28);
            this.LoadToApp.TabIndex = 10;
            this.LoadToApp.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.ButtonToolTip.SetToolTip(this.LoadToApp, "Force all apps that are using OM to load the SoundFont list");
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
            this.IEL.Location = new System.Drawing.Point(757, 369);
            this.IEL.Name = "IEL";
            this.IEL.Size = new System.Drawing.Size(28, 35);
            this.IEL.TabIndex = 13;
            this.IEL.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.ButtonToolTip.SetToolTip(this.IEL, "Import SoundFont list from a file...");
            this.IEL.UseVisualStyleBackColor = false;
            this.IEL.Click += new System.EventHandler(this.IEL_Click);
            this.IEL.Paint += new System.Windows.Forms.PaintEventHandler(this.ImportListButton);
            // 
            // BankPresetOverride
            // 
            this.BankPresetOverride.AutoSize = true;
            this.BankPresetOverride.Location = new System.Drawing.Point(215, 10);
            this.BankPresetOverride.Name = "BankPresetOverride";
            this.BankPresetOverride.Size = new System.Drawing.Size(401, 19);
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
            this.SelectedListBox.Location = new System.Drawing.Point(111, 7);
            this.SelectedListBox.Name = "SelectedListBox";
            this.SelectedListBox.Size = new System.Drawing.Size(86, 23);
            this.SelectedListBox.TabIndex = 1;
            this.SelectedListBox.SelectedIndexChanged += new System.EventHandler(this.SelectedListBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 15);
            this.label1.TabIndex = 17;
            this.label1.Text = "Select list to edit:";
            // 
            // ListOverride
            // 
            this.ListOverride.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ListOverride.AutoSize = true;
            this.ListOverride.BackColor = System.Drawing.Color.Transparent;
            this.ListOverride.Enabled = false;
            this.ListOverride.Location = new System.Drawing.Point(1, 507);
            this.ListOverride.Name = "ListOverride";
            this.ListOverride.Size = new System.Drawing.Size(471, 15);
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
            this.CLi.Location = new System.Drawing.Point(757, 6);
            this.CLi.Name = "CLi";
            this.CLi.Size = new System.Drawing.Size(28, 28);
            this.CLi.TabIndex = 5;
            this.CLi.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.ButtonToolTip.SetToolTip(this.CLi, "Clear SoundFont list");
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
            this.MvD.Location = new System.Drawing.Point(757, 205);
            this.MvD.Name = "MvD";
            this.MvD.Size = new System.Drawing.Size(28, 28);
            this.MvD.TabIndex = 9;
            this.MvD.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.ButtonToolTip.SetToolTip(this.MvD, "Move SoundFont down in the list");
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
            this.MvU.Location = new System.Drawing.Point(757, 179);
            this.MvU.Name = "MvU";
            this.MvU.Size = new System.Drawing.Size(28, 28);
            this.MvU.TabIndex = 8;
            this.MvU.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.ButtonToolTip.SetToolTip(this.MvU, "Move SoundFont up in the list");
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
            this.RmvSF.Location = new System.Drawing.Point(757, 66);
            this.RmvSF.Name = "RmvSF";
            this.RmvSF.Size = new System.Drawing.Size(28, 28);
            this.RmvSF.TabIndex = 7;
            this.RmvSF.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.ButtonToolTip.SetToolTip(this.RmvSF, "Remove a SoundFont from the list");
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
            this.AddSF.Location = new System.Drawing.Point(757, 39);
            this.AddSF.Name = "AddSF";
            this.AddSF.Size = new System.Drawing.Size(28, 28);
            this.AddSF.TabIndex = 6;
            this.AddSF.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.ButtonToolTip.SetToolTip(this.AddSF, "Add a SoundFont to the list");
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
            // LisCM
            // 
            this.LisCM.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.OSF,
            this.OSFd,
            this.EBPV,
            this.menuItem2,
            this.CSFs,
            this.PSFs,
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
            // EBPV
            // 
            this.EBPV.Index = 2;
            this.EBPV.Text = "Edit bank and preset values";
            this.EBPV.Click += new System.EventHandler(this.EBPV_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 3;
            this.menuItem2.Text = "-";
            // 
            // CSFs
            // 
            this.CSFs.Index = 4;
            this.CSFs.Text = "Copy SoundFont(s)";
            this.CSFs.Click += new System.EventHandler(this.CSFs_Click);
            // 
            // PSFs
            // 
            this.PSFs.Index = 5;
            this.PSFs.Text = "Paste SoundFont(s)";
            this.PSFs.Click += new System.EventHandler(this.PSFs_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 6;
            this.menuItem3.Text = "-";
            // 
            // ReLSFl
            // 
            this.ReLSFl.Index = 7;
            this.ReLSFl.Text = "(Re)Load SoundFont list";
            this.ReLSFl.Click += new System.EventHandler(this.LoadToApp_Click);
            // 
            // MSu
            // 
            this.MSu.Index = 8;
            this.MSu.Text = "Move SoundFont up";
            this.MSu.Click += new System.EventHandler(this.MvU_Click);
            // 
            // MSd
            // 
            this.MSd.Index = 9;
            this.MSd.Text = "Move SoundFont down";
            this.MSd.Click += new System.EventHandler(this.MvD_Click);
            // 
            // ESF
            // 
            this.ESF.Index = 10;
            this.ESF.Text = "Enable SoundFont";
            // 
            // DSF
            // 
            this.DSF.Index = 11;
            this.DSF.Text = "Disable SoundFont";
            // 
            // Preload
            // 
            this.Preload.AccessibleDescription = "Toggles the preload of the samples for the selected SoundFonts";
            this.Preload.AccessibleName = "Toggle preload for selected SoundFonts";
            this.Preload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Preload.BackColor = System.Drawing.Color.Transparent;
            this.Preload.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Preload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Preload.ForeColor = System.Drawing.Color.Transparent;
            this.Preload.Location = new System.Drawing.Point(757, 135);
            this.Preload.Name = "Preload";
            this.Preload.Size = new System.Drawing.Size(28, 28);
            this.Preload.TabIndex = 32;
            this.Preload.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.ButtonToolTip.SetToolTip(this.Preload, "Toggle preloading for the selected SoundFont in the list");
            this.Preload.UseVisualStyleBackColor = false;
            this.Preload.Click += new System.EventHandler(this.Preload_Click);
            this.Preload.Paint += new System.Windows.Forms.PaintEventHandler(this.ButtonLoad);
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
            this.Lis.CheckBoxes = true;
            this.Lis.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.SoundFont,
            this.SrcBank,
            this.SrcPres,
            this.DesBank,
            this.DesPres,
            this.DesBankLSB,
            this.XGDrums,
            this.PreloadSF,
            this.SFFormat,
            this.SFSize});
            this.Lis.FullRowSelect = true;
            this.Lis.GridLines = true;
            this.Lis.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.Lis.HideSelection = false;
            listViewItem3.StateImageIndex = 0;
            this.Lis.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem3});
            this.Lis.LabelWrap = false;
            this.Lis.LineAfter = -1;
            this.Lis.LineBefore = -1;
            this.Lis.Location = new System.Drawing.Point(6, 39);
            this.Lis.Name = "Lis";
            this.Lis.ShowGroups = false;
            this.Lis.Size = new System.Drawing.Size(745, 462);
            this.Lis.TabIndex = 3;
            this.Lis.UseCompatibleStateImageBehavior = false;
            this.Lis.View = System.Windows.Forms.View.Details;
            this.Lis.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.Lis_ColumnWidthChanged);
            this.Lis.DragDrop += new System.Windows.Forms.DragEventHandler(this.Lis_DragDrop);
            this.Lis.DragEnter += new System.Windows.Forms.DragEventHandler(this.Lis_DragEnter);
            this.Lis.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Lis_KeyDown);
            this.Lis.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Lis_MouseDown);
            // 
            // SoundFont
            // 
            this.SoundFont.Tag = "SF";
            this.SoundFont.Text = "SoundFont";
            this.SoundFont.Width = 425;
            // 
            // SrcBank
            // 
            this.SrcBank.Tag = "SrcB";
            this.SrcBank.Text = "SB";
            this.SrcBank.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SrcBank.Width = 30;
            // 
            // SrcPres
            // 
            this.SrcPres.Tag = "SrcP";
            this.SrcPres.Text = "SP";
            this.SrcPres.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SrcPres.Width = 30;
            // 
            // DesBank
            // 
            this.DesBank.Tag = "DesB";
            this.DesBank.Text = "DB";
            this.DesBank.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.DesBank.Width = 30;
            // 
            // DesPres
            // 
            this.DesPres.Tag = "DesP";
            this.DesPres.Text = "DP";
            this.DesPres.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.DesPres.Width = 30;
            // 
            // DesBankLSB
            // 
            this.DesBankLSB.Tag = "DesBLSB";
            this.DesBankLSB.Text = "LSB";
            this.DesBankLSB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.DesBankLSB.Width = 32;
            // 
            // XGDrums
            // 
            this.XGDrums.Tag = "XG";
            this.XGDrums.Text = "XG";
            this.XGDrums.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.XGDrums.Width = 31;
            // 
            // PreloadSF
            // 
            this.PreloadSF.Tag = "PSF";
            this.PreloadSF.Text = "Preload";
            this.PreloadSF.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // SFFormat
            // 
            this.SFFormat.Tag = "SFF";
            this.SFFormat.Text = "Format";
            this.SFFormat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SFFormat.Width = 53;
            // 
            // SFSize
            // 
            this.SFSize.Tag = "SFS";
            this.SFSize.Text = "Size";
            this.SFSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SFSize.Width = 62;
            // 
            // ButtonToolTip
            // 
            this.ButtonToolTip.IsBalloon = true;
            this.ButtonToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.ButtonToolTip.ToolTipTitle = "Information";
            // 
            // SoundFontListEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Preload);
            this.Controls.Add(this.Lis);
            this.Controls.Add(this.SFlg);
            this.Controls.Add(this.Separator);
            this.Controls.Add(this.EL);
            this.Controls.Add(this.LoadToApp);
            this.Controls.Add(this.IEL);
            this.Controls.Add(this.BankPresetOverride);
            this.Controls.Add(this.SelectedListBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ListOverride);
            this.Controls.Add(this.CLi);
            this.Controls.Add(this.MvD);
            this.Controls.Add(this.MvU);
            this.Controls.Add(this.RmvSF);
            this.Controls.Add(this.AddSF);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SoundFontListEditor";
            this.Size = new System.Drawing.Size(791, 524);
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
        private System.Windows.Forms.ContextMenu LisCM;
        private System.Windows.Forms.MenuItem OSF;
        private System.Windows.Forms.MenuItem OSFd;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem ReLSFl;
        private System.Windows.Forms.MenuItem MSu;
        private System.Windows.Forms.MenuItem MSd;
        private System.Windows.Forms.MenuItem ESF;
        private System.Windows.Forms.MenuItem DSF;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem CSFs;
        private System.Windows.Forms.MenuItem PSFs;
        private System.Windows.Forms.ColumnHeader PreloadSF;
        public System.Windows.Forms.Button Preload;
        public System.Windows.Forms.ColumnHeader DesBankLSB;
        private System.Windows.Forms.MenuItem EBPV;
        private System.Windows.Forms.ToolTip ButtonToolTip;
    }
}
