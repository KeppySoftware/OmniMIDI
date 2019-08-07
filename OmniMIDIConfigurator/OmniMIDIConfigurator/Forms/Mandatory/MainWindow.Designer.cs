namespace OmniMIDIConfigurator
{
    partial class OmniMIDIConfiguratorMain
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

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OmniMIDIConfiguratorMain));
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "Mama mia",
            "127",
            "127",
            "127",
            "127",
            "Yes",
            "0",
            "0"}, -1);
            this.SoundfontImport = new System.Windows.Forms.OpenFileDialog();
            this.ExternalListImport = new System.Windows.Forms.OpenFileDialog();
            this.ExternalListExport = new System.Windows.Forms.SaveFileDialog();
            this.RightClickMenu = new System.Windows.Forms.ContextMenu();
            this.EditSFSettings = new System.Windows.Forms.MenuItem();
            this.menuItem11 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItem35 = new System.Windows.Forms.MenuItem();
            this.menuItem38 = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.menuItem10 = new System.Windows.Forms.MenuItem();
            this.menuItem16 = new System.Windows.Forms.MenuItem();
            this.OpenSFDefaultApp = new System.Windows.Forms.MenuItem();
            this.OpenSFMainDirectory = new System.Windows.Forms.MenuItem();
            this.menuItem37 = new System.Windows.Forms.MenuItem();
            this.menuItem54 = new System.Windows.Forms.MenuItem();
            this.menuItem55 = new System.Windows.Forms.MenuItem();
            this.ThemeCheck = new System.ComponentModel.BackgroundWorker();
            this.ExportSettingsDialog = new System.Windows.Forms.SaveFileDialog();
            this.ImportSettingsDialog = new System.Windows.Forms.OpenFileDialog();
            this.Settings = new System.Windows.Forms.TabPage();
            this.ExportPres = new System.Windows.Forms.Button();
            this.SeparatorPres = new System.Windows.Forms.Label();
            this.ImportPres = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.VolPanel = new System.Windows.Forms.Panel();
            this.VolLabel = new System.Windows.Forms.Label();
            this.VolTrackBar = new KnobControl.KnobControl();
            this.VolSimView = new System.Windows.Forms.Label();
            this.MixerBox = new System.Windows.Forms.GroupBox();
            this.MixerContext = new System.Windows.Forms.ContextMenu();
            this.OpenMixer = new System.Windows.Forms.MenuItem();
            this.menuItem19 = new System.Windows.Forms.MenuItem();
            this.DisableOLM = new System.Windows.Forms.MenuItem();
            this.VolLevelS = new System.Windows.Forms.Label();
            this.SignalLabelS = new System.Windows.Forms.Label();
            this.LEDS = new System.Windows.Forms.Panel();
            this.RV22S = new System.Windows.Forms.Panel();
            this.RLab = new System.Windows.Forms.Label();
            this.LV22S = new System.Windows.Forms.Panel();
            this.LLab = new System.Windows.Forms.Label();
            this.RV18S = new System.Windows.Forms.Panel();
            this.LV1S = new System.Windows.Forms.Panel();
            this.LV18S = new System.Windows.Forms.Panel();
            this.LV2S = new System.Windows.Forms.Panel();
            this.RV21S = new System.Windows.Forms.Panel();
            this.RV1S = new System.Windows.Forms.Panel();
            this.LV21S = new System.Windows.Forms.Panel();
            this.LV3S = new System.Windows.Forms.Panel();
            this.RV13S = new System.Windows.Forms.Panel();
            this.RV2S = new System.Windows.Forms.Panel();
            this.LV13S = new System.Windows.Forms.Panel();
            this.LV4S = new System.Windows.Forms.Panel();
            this.RV20S = new System.Windows.Forms.Panel();
            this.RV3S = new System.Windows.Forms.Panel();
            this.LV20S = new System.Windows.Forms.Panel();
            this.LV9S = new System.Windows.Forms.Panel();
            this.RV19S = new System.Windows.Forms.Panel();
            this.RV4S = new System.Windows.Forms.Panel();
            this.LV19S = new System.Windows.Forms.Panel();
            this.LV5S = new System.Windows.Forms.Panel();
            this.RV17S = new System.Windows.Forms.Panel();
            this.RV9S = new System.Windows.Forms.Panel();
            this.LV17S = new System.Windows.Forms.Panel();
            this.LV10S = new System.Windows.Forms.Panel();
            this.RV8S = new System.Windows.Forms.Panel();
            this.RV5S = new System.Windows.Forms.Panel();
            this.LV8S = new System.Windows.Forms.Panel();
            this.LV6S = new System.Windows.Forms.Panel();
            this.RV16S = new System.Windows.Forms.Panel();
            this.RV10S = new System.Windows.Forms.Panel();
            this.LV16S = new System.Windows.Forms.Panel();
            this.RV6S = new System.Windows.Forms.Panel();
            this.RV12S = new System.Windows.Forms.Panel();
            this.LV11S = new System.Windows.Forms.Panel();
            this.LV12S = new System.Windows.Forms.Panel();
            this.RV11S = new System.Windows.Forms.Panel();
            this.RV15S = new System.Windows.Forms.Panel();
            this.LV7S = new System.Windows.Forms.Panel();
            this.LV15S = new System.Windows.Forms.Panel();
            this.RV7S = new System.Windows.Forms.Panel();
            this.RV14S = new System.Windows.Forms.Panel();
            this.LV14S = new System.Windows.Forms.Panel();
            this.EnginesBox = new System.Windows.Forms.GroupBox();
            this.WhatIsXAudio = new System.Windows.Forms.LinkLabelEx();
            this.WhatIsOutput = new System.Windows.Forms.LinkLabelEx();
            this.label2 = new System.Windows.Forms.Label();
            this.AudioEngBox = new System.Windows.Forms.ComboBox();
            this.AdditionalSettingsBox = new System.Windows.Forms.GroupBox();
            this.ChangeMask = new System.Windows.Forms.Button();
            this.AASButton = new System.Windows.Forms.Button();
            this.MEPSButton = new System.Windows.Forms.Button();
            this.resetToDefaultToolStripMenuItem = new System.Windows.Forms.Button();
            this.applySettingsToolStripMenuItem = new System.Windows.Forms.Button();
            this.OutputSettingsBox = new System.Windows.Forms.GroupBox();
            this.SincConv = new System.Windows.Forms.ComboBox();
            this.SincConvLab = new System.Windows.Forms.Label();
            this.DrvHzLabel = new System.Windows.Forms.Label();
            this.Frequency = new System.Windows.Forms.ComboBox();
            this.SincInter = new System.Windows.Forms.CheckBox();
            this.BufferText = new System.Windows.Forms.Label();
            this.bufsize = new System.Windows.Forms.NumericUpDown();
            this.EnableSFX = new System.Windows.Forms.CheckBox();
            this.SynthSettingsBox = new System.Windows.Forms.GroupBox();
            this.MaxCPU = new System.Windows.Forms.NumericUpDown();
            this.SysResetIgnore = new System.Windows.Forms.CheckBox();
            this.RenderingTimeLabel = new System.Windows.Forms.Label();
            this.VoiceLimitLabel = new System.Windows.Forms.Label();
            this.PolyphonyLimit = new System.Windows.Forms.NumericUpDown();
            this.Preload = new System.Windows.Forms.CheckBox();
            this.NoteOffCheck = new System.Windows.Forms.CheckBox();
            this.SettingsPresetsBtn = new OmniMIDIConfigurator.MenuButton();
            this.SoundFontTab = new System.Windows.Forms.TabPage();
            this.SFlg = new System.Windows.Forms.Button();
            this.Separator = new System.Windows.Forms.Label();
            this.Lis = new OmniMIDIConfigurator.ListViewEx();
            this.SoundFont = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SrcBank = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SrcPres = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DesBank = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DesPres = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.XGDrums = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SFFormat = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SFSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
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
            this.TabsForTheControls = new System.Windows.Forms.TabControl();
            this.TabImgs = new System.Windows.Forms.ImageList(this.components);
            this.SettingsPresets = new System.Windows.Forms.ContextMenu();
            this.MSGSWSEmu = new System.Windows.Forms.MenuItem();
            this.menuItem50 = new System.Windows.Forms.MenuItem();
            this.keppysSteinwayPianoRealismToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem41 = new System.Windows.Forms.MenuItem();
            this.lowLatencyPresetToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.SBLowLatToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.ProLowLatToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem36 = new System.Windows.Forms.MenuItem();
            this.blackMIDIsPresetToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem44 = new System.Windows.Forms.MenuItem();
            this.chiptunesRetrogamingToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.SavedLabel = new System.Windows.Forms.Timer(this.components);
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.Status = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusDoneOr = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.UpdateStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.VersionLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ButtonsDesc = new System.Windows.Forms.ToolTip(this.components);
            this.Requirements = new System.Windows.Forms.ToolTip(this.components);
            this.CheckUpdates = new System.ComponentModel.BackgroundWorker();
            this.ExportPresetDialog = new System.Windows.Forms.SaveFileDialog();
            this.ImportPresetDialog = new System.Windows.Forms.OpenFileDialog();
            this.openDebugWindowToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.openTheMixerToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.openTheBlacklistManagerToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.assignASoundfontListToASpecificAppToolStripMenuItemToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.OmniMapperCpl = new System.Windows.Forms.MenuItem();
            this.menuItem13 = new System.Windows.Forms.MenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.openTheRivaTunerOSDManagerToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.LiveChangesTrigger = new System.Windows.Forms.MenuItem();
            this.hotkeys = new System.Windows.Forms.MenuItem();
            this.AutoLoad = new System.Windows.Forms.MenuItem();
            this.ShowOutLevel = new System.Windows.Forms.MenuItem();
            this.menuItem17 = new System.Windows.Forms.MenuItem();
            this.manageFolderFavouritesToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.changeDirectoryOfTheOutputToWAVModeToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.DePrio = new System.Windows.Forms.MenuItem();
            this.menuItem34 = new System.Windows.Forms.MenuItem();
            this.RTPrio = new System.Windows.Forms.MenuItem();
            this.HiPrio = new System.Windows.Forms.MenuItem();
            this.HNPrio = new System.Windows.Forms.MenuItem();
            this.NoPrio = new System.Windows.Forms.MenuItem();
            this.LNPrio = new System.Windows.Forms.MenuItem();
            this.LoPrio = new System.Windows.Forms.MenuItem();
            this.menuItem31 = new System.Windows.Forms.MenuItem();
            this.menuItem21 = new System.Windows.Forms.MenuItem();
            this.SpatialSound = new System.Windows.Forms.MenuItem();
            this.MaskSynthesizerAsAnother = new System.Windows.Forms.MenuItem();
            this.enableextra8sf = new System.Windows.Forms.MenuItem();
            this.menuItem14 = new System.Windows.Forms.MenuItem();
            this.ImportSettings = new System.Windows.Forms.MenuItem();
            this.ExportSettings = new System.Windows.Forms.MenuItem();
            this.menuItem12 = new System.Windows.Forms.MenuItem();
            this.menuItem15 = new System.Windows.Forms.MenuItem();
            this.DebugModePls = new System.Windows.Forms.MenuItem();
            this.DebugModeOpenNotepad = new System.Windows.Forms.MenuItem();
            this.menuItem28 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.LoudMaxInstallMenu = new System.Windows.Forms.MenuItem();
            this.LoudMaxUninstallMenu = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.LMWarningARM64 = new System.Windows.Forms.MenuItem();
            this.menuItem18 = new System.Windows.Forms.MenuItem();
            this.DisableChime = new System.Windows.Forms.MenuItem();
            this.AMIDIMapInstallMenu = new System.Windows.Forms.MenuItem();
            this.AMIDIMapUninstallMenu = new System.Windows.Forms.MenuItem();
            this.menuItem40 = new System.Windows.Forms.MenuItem();
            this.SignatureCheck = new System.Windows.Forms.MenuItem();
            this.SelfSignedCertificate = new System.Windows.Forms.MenuItem();
            this.menuItem39 = new System.Windows.Forms.MenuItem();
            this.SetAssociationWithSFs = new System.Windows.Forms.MenuItem();
            this.menuItem46 = new System.Windows.Forms.MenuItem();
            this.DeleteUserData = new System.Windows.Forms.MenuItem();
            this.ResetToDefault = new System.Windows.Forms.MenuItem();
            this.menuItem25 = new System.Windows.Forms.MenuItem();
            this.WMMPatches = new System.Windows.Forms.MenuItem();
            this.menuItem60 = new System.Windows.Forms.MenuItem();
            this.MIDIInOutTest = new System.Windows.Forms.MenuItem();
            this.informationAboutTheDriverToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.openUpdaterToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem49 = new System.Windows.Forms.MenuItem();
            this.donateToSupportUsToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem29 = new System.Windows.Forms.MenuItem();
            this.SeeChangelog = new System.Windows.Forms.MenuItem();
            this.SeeLatestChangelog = new System.Windows.Forms.MenuItem();
            this.downloadTheSourceCodeToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem33 = new System.Windows.Forms.MenuItem();
            this.HAPLink = new System.Windows.Forms.MenuItem();
            this.BASSLink = new System.Windows.Forms.MenuItem();
            this.BASSNetLink = new System.Windows.Forms.MenuItem();
            this.FodyCredit = new System.Windows.Forms.MenuItem();
            this.menuItem45 = new System.Windows.Forms.MenuItem();
            this.OctokitDev = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.BugReport = new System.Windows.Forms.MenuItem();
            this.KSSD = new System.Windows.Forms.MenuItem();
            this.KDMAPIDoc = new System.Windows.Forms.MenuItem();
            this.SynthMenu = new System.Windows.Forms.MainMenu(this.components);
            this.VolTrackBarMenu = new System.Windows.Forms.ContextMenu();
            this.FineTuneKnobIt = new System.Windows.Forms.MenuItem();
            this.menuItem57 = new System.Windows.Forms.MenuItem();
            this.VolumeBoost = new System.Windows.Forms.MenuItem();
            this.VolumeCheck = new System.Windows.Forms.Timer(this.components);
            this.Settings.SuspendLayout();
            this.VolPanel.SuspendLayout();
            this.MixerBox.SuspendLayout();
            this.EnginesBox.SuspendLayout();
            this.AdditionalSettingsBox.SuspendLayout();
            this.OutputSettingsBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bufsize)).BeginInit();
            this.SynthSettingsBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MaxCPU)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PolyphonyLimit)).BeginInit();
            this.SoundFontTab.SuspendLayout();
            this.TabsForTheControls.SuspendLayout();
            this.StatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // SoundfontImport
            // 
            this.SoundfontImport.Filter = "Soundfont files|*.sf2;*.sfz;*.sfpack;";
            this.SoundfontImport.Multiselect = true;
            this.SoundfontImport.RestoreDirectory = true;
            this.SoundfontImport.SupportMultiDottedExtensions = true;
            this.SoundfontImport.Title = "Add soundfonts to the list...";
            // 
            // ExternalListImport
            // 
            this.ExternalListImport.Filter = "Soundfont lists|*.sflist;*.omlist;*.txt;";
            // 
            // ExternalListExport
            // 
            this.ExternalListExport.Filter = "Soundfont list|*.omlist;";
            // 
            // RightClickMenu
            // 
            this.RightClickMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.EditSFSettings,
            this.menuItem11,
            this.menuItem4,
            this.menuItem5,
            this.menuItem35,
            this.menuItem38,
            this.menuItem8,
            this.menuItem10,
            this.menuItem16,
            this.OpenSFDefaultApp,
            this.OpenSFMainDirectory,
            this.menuItem37,
            this.menuItem54,
            this.menuItem55});
            // 
            // EditSFSettings
            // 
            this.EditSFSettings.Index = 0;
            this.EditSFSettings.Text = "Edit settings for this SoundFont";
            this.EditSFSettings.Click += new System.EventHandler(this.EditSFSettings_Click);
            // 
            // menuItem11
            // 
            this.menuItem11.Index = 1;
            this.menuItem11.Text = "-";
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            this.menuItem4.Text = "Add SoundFont(s)";
            this.menuItem4.Click += new System.EventHandler(this.AddSF_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 3;
            this.menuItem5.Text = "Remove SoundFont(s)";
            this.menuItem5.Click += new System.EventHandler(this.RmvSF_Click);
            // 
            // menuItem35
            // 
            this.menuItem35.Index = 4;
            this.menuItem35.Text = "Enable SoundFont(s)";
            this.menuItem35.Click += new System.EventHandler(this.menuItem35_Click);
            // 
            // menuItem38
            // 
            this.menuItem38.Index = 5;
            this.menuItem38.Text = "Disable SoundFont(s)";
            this.menuItem38.Click += new System.EventHandler(this.menuItem38_Click);
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 6;
            this.menuItem8.Text = "Move SoundFont up";
            this.menuItem8.Click += new System.EventHandler(this.MvU_Click);
            // 
            // menuItem10
            // 
            this.menuItem10.Index = 7;
            this.menuItem10.Text = "Move SoundFont down";
            this.menuItem10.Click += new System.EventHandler(this.MvD_Click);
            // 
            // menuItem16
            // 
            this.menuItem16.Index = 8;
            this.menuItem16.Text = "-";
            // 
            // OpenSFDefaultApp
            // 
            this.OpenSFDefaultApp.Index = 9;
            this.OpenSFDefaultApp.Text = "Open SoundFont with default app";
            this.OpenSFDefaultApp.Click += new System.EventHandler(this.OpenSFDefaultApp_Click);
            // 
            // OpenSFMainDirectory
            // 
            this.OpenSFMainDirectory.Index = 10;
            this.OpenSFMainDirectory.Text = "Open SoundFont parent directory";
            this.OpenSFMainDirectory.Click += new System.EventHandler(this.OpenSFMainDirectory_Click);
            // 
            // menuItem37
            // 
            this.menuItem37.Index = 11;
            this.menuItem37.Text = "-";
            // 
            // menuItem54
            // 
            this.menuItem54.Index = 12;
            this.menuItem54.Text = "Copy selected SoundFont items to clipboard";
            this.menuItem54.Click += new System.EventHandler(this.menuItem54_Click);
            // 
            // menuItem55
            // 
            this.menuItem55.Index = 13;
            this.menuItem55.Text = "Paste SoundFont items from clipboard";
            this.menuItem55.Click += new System.EventHandler(this.menuItem55_Click);
            // 
            // ThemeCheck
            // 
            this.ThemeCheck.DoWork += new System.ComponentModel.DoWorkEventHandler(this.ThemeCheck_DoWork);
            // 
            // ExportSettingsDialog
            // 
            this.ExportSettingsDialog.Filter = "Registry files|*.reg";
            // 
            // ImportSettingsDialog
            // 
            this.ImportSettingsDialog.Filter = "Registry files|*.reg";
            // 
            // Settings
            // 
            this.Settings.BackColor = System.Drawing.Color.Transparent;
            this.Settings.Controls.Add(this.ExportPres);
            this.Settings.Controls.Add(this.SeparatorPres);
            this.Settings.Controls.Add(this.ImportPres);
            this.Settings.Controls.Add(this.label3);
            this.Settings.Controls.Add(this.VolPanel);
            this.Settings.Controls.Add(this.MixerBox);
            this.Settings.Controls.Add(this.EnginesBox);
            this.Settings.Controls.Add(this.AdditionalSettingsBox);
            this.Settings.Controls.Add(this.resetToDefaultToolStripMenuItem);
            this.Settings.Controls.Add(this.applySettingsToolStripMenuItem);
            this.Settings.Controls.Add(this.OutputSettingsBox);
            this.Settings.Controls.Add(this.SynthSettingsBox);
            this.Settings.Controls.Add(this.SettingsPresetsBtn);
            this.Settings.Location = new System.Drawing.Point(4, 23);
            this.Settings.Name = "Settings";
            this.Settings.Size = new System.Drawing.Size(641, 394);
            this.Settings.TabIndex = 4;
            this.Settings.Text = "Settings";
            // 
            // ExportPres
            // 
            this.ExportPres.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ExportPres.Location = new System.Drawing.Point(187, 362);
            this.ExportPres.Name = "ExportPres";
            this.ExportPres.Size = new System.Drawing.Size(45, 23);
            this.ExportPres.TabIndex = 20;
            this.ExportPres.Text = "Export";
            this.ExportPres.UseVisualStyleBackColor = true;
            this.ExportPres.Click += new System.EventHandler(this.ExportPres_Click);
            // 
            // SeparatorPres
            // 
            this.SeparatorPres.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SeparatorPres.Enabled = false;
            this.SeparatorPres.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SeparatorPres.Location = new System.Drawing.Point(127, 362);
            this.SeparatorPres.Name = "SeparatorPres";
            this.SeparatorPres.Size = new System.Drawing.Size(10, 23);
            this.SeparatorPres.TabIndex = 49;
            this.SeparatorPres.Text = "|";
            // 
            // ImportPres
            // 
            this.ImportPres.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ImportPres.Location = new System.Drawing.Point(140, 362);
            this.ImportPres.Name = "ImportPres";
            this.ImportPres.Size = new System.Drawing.Size(45, 23);
            this.ImportPres.TabIndex = 19;
            this.ImportPres.Text = "Import";
            this.ImportPres.UseVisualStyleBackColor = true;
            this.ImportPres.Click += new System.EventHandler(this.ImportPres_Click);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 367);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 47;
            this.label3.Text = "Presets:";
            // 
            // VolPanel
            // 
            this.VolPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.VolPanel.BackColor = System.Drawing.Color.Transparent;
            this.VolPanel.Controls.Add(this.VolLabel);
            this.VolPanel.Controls.Add(this.VolTrackBar);
            this.VolPanel.Controls.Add(this.VolSimView);
            this.VolPanel.Location = new System.Drawing.Point(543, 3);
            this.VolPanel.Name = "VolPanel";
            this.VolPanel.Size = new System.Drawing.Size(94, 104);
            this.VolPanel.TabIndex = 15;
            // 
            // VolLabel
            // 
            this.VolLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.VolLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VolLabel.Location = new System.Drawing.Point(2, 88);
            this.VolLabel.Name = "VolLabel";
            this.VolLabel.Size = new System.Drawing.Size(54, 14);
            this.VolLabel.TabIndex = 3;
            this.VolLabel.Text = "VOLUME:";
            this.VolLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // VolTrackBar
            // 
            this.VolTrackBar.BackColor = System.Drawing.SystemColors.Control;
            this.VolTrackBar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.VolTrackBar.EndAngle = 405F;
            this.VolTrackBar.ImeMode = System.Windows.Forms.ImeMode.On;
            this.VolTrackBar.knobBackColor = System.Drawing.Color.White;
            this.VolTrackBar.KnobPointerStyle = KnobControl.KnobControl.knobPointerStyle.line;
            this.VolTrackBar.LargeChange = 1000;
            this.VolTrackBar.Location = new System.Drawing.Point(8, 3);
            this.VolTrackBar.Maximum = 10000;
            this.VolTrackBar.Minimum = 0;
            this.VolTrackBar.Name = "VolTrackBar";
            this.VolTrackBar.PointerColor = System.Drawing.Color.White;
            this.VolTrackBar.ScaleColor = System.Drawing.Color.Black;
            this.VolTrackBar.ScaleDivisions = 10;
            this.VolTrackBar.ScaleSubDivisions = 10;
            this.VolTrackBar.ShowLargeScale = false;
            this.VolTrackBar.ShowSmallScale = false;
            this.VolTrackBar.Size = new System.Drawing.Size(79, 79);
            this.VolTrackBar.SmallChange = 500;
            this.VolTrackBar.StartAngle = 135F;
            this.VolTrackBar.TabIndex = 5;
            this.ButtonsDesc.SetToolTip(this.VolTrackBar, "Right-click the knob to fine tune it");
            this.VolTrackBar.Value = 10000;
            this.VolTrackBar.ValueChanged += new KnobControl.ValueChangedEventHandler(this.VolTrackBar_Scroll);
            // 
            // VolSimView
            // 
            this.VolSimView.BackColor = System.Drawing.Color.Transparent;
            this.VolSimView.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.VolSimView.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VolSimView.ForeColor = System.Drawing.Color.MediumBlue;
            this.VolSimView.Location = new System.Drawing.Point(0, 82);
            this.VolSimView.Name = "VolSimView";
            this.VolSimView.Size = new System.Drawing.Size(94, 22);
            this.VolSimView.TabIndex = 4;
            this.VolSimView.Text = "100";
            this.VolSimView.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.VolSimView.UseMnemonic = false;
            // 
            // MixerBox
            // 
            this.MixerBox.ContextMenu = this.MixerContext;
            this.MixerBox.Controls.Add(this.VolLevelS);
            this.MixerBox.Controls.Add(this.SignalLabelS);
            this.MixerBox.Controls.Add(this.LEDS);
            this.MixerBox.Controls.Add(this.RV22S);
            this.MixerBox.Controls.Add(this.RLab);
            this.MixerBox.Controls.Add(this.LV22S);
            this.MixerBox.Controls.Add(this.LLab);
            this.MixerBox.Controls.Add(this.RV18S);
            this.MixerBox.Controls.Add(this.LV1S);
            this.MixerBox.Controls.Add(this.LV18S);
            this.MixerBox.Controls.Add(this.LV2S);
            this.MixerBox.Controls.Add(this.RV21S);
            this.MixerBox.Controls.Add(this.RV1S);
            this.MixerBox.Controls.Add(this.LV21S);
            this.MixerBox.Controls.Add(this.LV3S);
            this.MixerBox.Controls.Add(this.RV13S);
            this.MixerBox.Controls.Add(this.RV2S);
            this.MixerBox.Controls.Add(this.LV13S);
            this.MixerBox.Controls.Add(this.LV4S);
            this.MixerBox.Controls.Add(this.RV20S);
            this.MixerBox.Controls.Add(this.RV3S);
            this.MixerBox.Controls.Add(this.LV20S);
            this.MixerBox.Controls.Add(this.LV9S);
            this.MixerBox.Controls.Add(this.RV19S);
            this.MixerBox.Controls.Add(this.RV4S);
            this.MixerBox.Controls.Add(this.LV19S);
            this.MixerBox.Controls.Add(this.LV5S);
            this.MixerBox.Controls.Add(this.RV17S);
            this.MixerBox.Controls.Add(this.RV9S);
            this.MixerBox.Controls.Add(this.LV17S);
            this.MixerBox.Controls.Add(this.LV10S);
            this.MixerBox.Controls.Add(this.RV8S);
            this.MixerBox.Controls.Add(this.RV5S);
            this.MixerBox.Controls.Add(this.LV8S);
            this.MixerBox.Controls.Add(this.LV6S);
            this.MixerBox.Controls.Add(this.RV16S);
            this.MixerBox.Controls.Add(this.RV10S);
            this.MixerBox.Controls.Add(this.LV16S);
            this.MixerBox.Controls.Add(this.RV6S);
            this.MixerBox.Controls.Add(this.RV12S);
            this.MixerBox.Controls.Add(this.LV11S);
            this.MixerBox.Controls.Add(this.LV12S);
            this.MixerBox.Controls.Add(this.RV11S);
            this.MixerBox.Controls.Add(this.RV15S);
            this.MixerBox.Controls.Add(this.LV7S);
            this.MixerBox.Controls.Add(this.LV15S);
            this.MixerBox.Controls.Add(this.RV7S);
            this.MixerBox.Controls.Add(this.RV14S);
            this.MixerBox.Controls.Add(this.LV14S);
            this.MixerBox.Location = new System.Drawing.Point(358, 3);
            this.MixerBox.Name = "MixerBox";
            this.MixerBox.Size = new System.Drawing.Size(183, 95);
            this.MixerBox.TabIndex = 46;
            this.MixerBox.TabStop = false;
            this.MixerBox.Text = "Output level meter";
            // 
            // MixerContext
            // 
            this.MixerContext.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.OpenMixer,
            this.menuItem19,
            this.DisableOLM});
            // 
            // OpenMixer
            // 
            this.OpenMixer.Index = 0;
            this.OpenMixer.Text = "Open the mixer";
            this.OpenMixer.Click += new System.EventHandler(this.openTheMixerToolStripMenuItem_Click);
            // 
            // menuItem19
            // 
            this.menuItem19.Index = 1;
            this.menuItem19.Text = "-";
            // 
            // DisableOLM
            // 
            this.DisableOLM.Index = 2;
            this.DisableOLM.Text = "Disable the output level meter";
            this.DisableOLM.Click += new System.EventHandler(this.DisableOLM_Click);
            // 
            // VolLevelS
            // 
            this.VolLevelS.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.VolLevelS.Font = new System.Drawing.Font("Microsoft Sans Serif", 5.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VolLevelS.ForeColor = System.Drawing.Color.Black;
            this.VolLevelS.Location = new System.Drawing.Point(19, 75);
            this.VolLevelS.Name = "VolLevelS";
            this.VolLevelS.Size = new System.Drawing.Size(23, 10);
            this.VolLevelS.TabIndex = 95;
            this.VolLevelS.Text = "0%";
            this.VolLevelS.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SignalLabelS
            // 
            this.SignalLabelS.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.SignalLabelS.AutoSize = true;
            this.SignalLabelS.Font = new System.Drawing.Font("Microsoft Sans Serif", 5.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SignalLabelS.ForeColor = System.Drawing.Color.Black;
            this.SignalLabelS.Location = new System.Drawing.Point(135, 77);
            this.SignalLabelS.Name = "SignalLabelS";
            this.SignalLabelS.Size = new System.Drawing.Size(30, 7);
            this.SignalLabelS.TabIndex = 74;
            this.SignalLabelS.Text = "SIGNAL";
            // 
            // LEDS
            // 
            this.LEDS.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.LEDS.BackColor = System.Drawing.Color.Black;
            this.LEDS.Location = new System.Drawing.Point(165, 76);
            this.LEDS.Name = "LEDS";
            this.LEDS.Size = new System.Drawing.Size(8, 8);
            this.LEDS.TabIndex = 75;
            this.LEDS.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // RV22S
            // 
            this.RV22S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RV22S.BackColor = System.Drawing.Color.DeepPink;
            this.RV22S.Location = new System.Drawing.Point(168, 46);
            this.RV22S.Name = "RV22S";
            this.RV22S.Size = new System.Drawing.Size(5, 28);
            this.RV22S.TabIndex = 93;
            this.RV22S.Visible = false;
            this.RV22S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // RLab
            // 
            this.RLab.AutoSize = true;
            this.RLab.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RLab.ForeColor = System.Drawing.Color.Black;
            this.RLab.Location = new System.Drawing.Point(5, 54);
            this.RLab.Name = "RLab";
            this.RLab.Size = new System.Drawing.Size(15, 13);
            this.RLab.TabIndex = 49;
            this.RLab.Text = "R";
            this.RLab.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LV22S
            // 
            this.LV22S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LV22S.BackColor = System.Drawing.Color.DeepPink;
            this.LV22S.Location = new System.Drawing.Point(168, 17);
            this.LV22S.Name = "LV22S";
            this.LV22S.Size = new System.Drawing.Size(5, 28);
            this.LV22S.TabIndex = 71;
            this.LV22S.Visible = false;
            this.LV22S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // LLab
            // 
            this.LLab.AutoSize = true;
            this.LLab.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LLab.ForeColor = System.Drawing.Color.Black;
            this.LLab.Location = new System.Drawing.Point(6, 24);
            this.LLab.Name = "LLab";
            this.LLab.Size = new System.Drawing.Size(13, 13);
            this.LLab.TabIndex = 48;
            this.LLab.Text = "L";
            this.LLab.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // RV18S
            // 
            this.RV18S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RV18S.BackColor = System.Drawing.Color.DarkOrange;
            this.RV18S.Location = new System.Drawing.Point(140, 46);
            this.RV18S.Name = "RV18S";
            this.RV18S.Size = new System.Drawing.Size(5, 28);
            this.RV18S.TabIndex = 89;
            this.RV18S.Visible = false;
            this.RV18S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // LV1S
            // 
            this.LV1S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LV1S.BackColor = System.Drawing.Color.DarkSlateGray;
            this.LV1S.Location = new System.Drawing.Point(21, 17);
            this.LV1S.Name = "LV1S";
            this.LV1S.Size = new System.Drawing.Size(5, 28);
            this.LV1S.TabIndex = 50;
            this.LV1S.Visible = false;
            this.LV1S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // LV18S
            // 
            this.LV18S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LV18S.BackColor = System.Drawing.Color.DarkOrange;
            this.LV18S.Location = new System.Drawing.Point(140, 17);
            this.LV18S.Name = "LV18S";
            this.LV18S.Size = new System.Drawing.Size(5, 28);
            this.LV18S.TabIndex = 67;
            this.LV18S.Visible = false;
            this.LV18S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // LV2S
            // 
            this.LV2S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LV2S.BackColor = System.Drawing.Color.DarkGreen;
            this.LV2S.Location = new System.Drawing.Point(28, 17);
            this.LV2S.Name = "LV2S";
            this.LV2S.Size = new System.Drawing.Size(5, 28);
            this.LV2S.TabIndex = 51;
            this.LV2S.Visible = false;
            this.LV2S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // RV21S
            // 
            this.RV21S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RV21S.BackColor = System.Drawing.Color.Crimson;
            this.RV21S.Location = new System.Drawing.Point(161, 46);
            this.RV21S.Name = "RV21S";
            this.RV21S.Size = new System.Drawing.Size(5, 28);
            this.RV21S.TabIndex = 92;
            this.RV21S.Visible = false;
            this.RV21S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // RV1S
            // 
            this.RV1S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RV1S.BackColor = System.Drawing.Color.DarkSlateGray;
            this.RV1S.Location = new System.Drawing.Point(21, 46);
            this.RV1S.Name = "RV1S";
            this.RV1S.Size = new System.Drawing.Size(5, 28);
            this.RV1S.TabIndex = 72;
            this.RV1S.Visible = false;
            this.RV1S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // LV21S
            // 
            this.LV21S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LV21S.BackColor = System.Drawing.Color.Crimson;
            this.LV21S.Location = new System.Drawing.Point(161, 17);
            this.LV21S.Name = "LV21S";
            this.LV21S.Size = new System.Drawing.Size(5, 28);
            this.LV21S.TabIndex = 70;
            this.LV21S.Visible = false;
            this.LV21S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // LV3S
            // 
            this.LV3S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LV3S.BackColor = System.Drawing.Color.ForestGreen;
            this.LV3S.Location = new System.Drawing.Point(35, 17);
            this.LV3S.Name = "LV3S";
            this.LV3S.Size = new System.Drawing.Size(5, 28);
            this.LV3S.TabIndex = 52;
            this.LV3S.Visible = false;
            this.LV3S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // RV13S
            // 
            this.RV13S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RV13S.BackColor = System.Drawing.Color.GreenYellow;
            this.RV13S.Location = new System.Drawing.Point(105, 46);
            this.RV13S.Name = "RV13S";
            this.RV13S.Size = new System.Drawing.Size(5, 28);
            this.RV13S.TabIndex = 84;
            this.RV13S.Visible = false;
            this.RV13S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // RV2S
            // 
            this.RV2S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RV2S.BackColor = System.Drawing.Color.DarkGreen;
            this.RV2S.Location = new System.Drawing.Point(28, 46);
            this.RV2S.Name = "RV2S";
            this.RV2S.Size = new System.Drawing.Size(5, 28);
            this.RV2S.TabIndex = 73;
            this.RV2S.Visible = false;
            this.RV2S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // LV13S
            // 
            this.LV13S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LV13S.BackColor = System.Drawing.Color.GreenYellow;
            this.LV13S.Location = new System.Drawing.Point(105, 17);
            this.LV13S.Name = "LV13S";
            this.LV13S.Size = new System.Drawing.Size(5, 28);
            this.LV13S.TabIndex = 62;
            this.LV13S.Visible = false;
            this.LV13S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // LV4S
            // 
            this.LV4S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LV4S.BackColor = System.Drawing.Color.Green;
            this.LV4S.Location = new System.Drawing.Point(42, 17);
            this.LV4S.Name = "LV4S";
            this.LV4S.Size = new System.Drawing.Size(5, 28);
            this.LV4S.TabIndex = 53;
            this.LV4S.Visible = false;
            this.LV4S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // RV20S
            // 
            this.RV20S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RV20S.BackColor = System.Drawing.Color.Red;
            this.RV20S.Location = new System.Drawing.Point(154, 46);
            this.RV20S.Name = "RV20S";
            this.RV20S.Size = new System.Drawing.Size(5, 28);
            this.RV20S.TabIndex = 91;
            this.RV20S.Visible = false;
            this.RV20S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // RV3S
            // 
            this.RV3S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RV3S.BackColor = System.Drawing.Color.ForestGreen;
            this.RV3S.Location = new System.Drawing.Point(35, 46);
            this.RV3S.Name = "RV3S";
            this.RV3S.Size = new System.Drawing.Size(5, 28);
            this.RV3S.TabIndex = 74;
            this.RV3S.Visible = false;
            this.RV3S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // LV20S
            // 
            this.LV20S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LV20S.BackColor = System.Drawing.Color.Red;
            this.LV20S.Location = new System.Drawing.Point(154, 17);
            this.LV20S.Name = "LV20S";
            this.LV20S.Size = new System.Drawing.Size(5, 28);
            this.LV20S.TabIndex = 69;
            this.LV20S.Visible = false;
            this.LV20S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // LV9S
            // 
            this.LV9S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LV9S.BackColor = System.Drawing.Color.Lime;
            this.LV9S.Location = new System.Drawing.Point(77, 17);
            this.LV9S.Name = "LV9S";
            this.LV9S.Size = new System.Drawing.Size(5, 28);
            this.LV9S.TabIndex = 58;
            this.LV9S.Visible = false;
            this.LV9S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // RV19S
            // 
            this.RV19S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RV19S.BackColor = System.Drawing.Color.Red;
            this.RV19S.Location = new System.Drawing.Point(147, 46);
            this.RV19S.Name = "RV19S";
            this.RV19S.Size = new System.Drawing.Size(5, 28);
            this.RV19S.TabIndex = 90;
            this.RV19S.Visible = false;
            this.RV19S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // RV4S
            // 
            this.RV4S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RV4S.BackColor = System.Drawing.Color.Green;
            this.RV4S.Location = new System.Drawing.Point(42, 46);
            this.RV4S.Name = "RV4S";
            this.RV4S.Size = new System.Drawing.Size(5, 28);
            this.RV4S.TabIndex = 75;
            this.RV4S.Visible = false;
            this.RV4S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // LV19S
            // 
            this.LV19S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LV19S.BackColor = System.Drawing.Color.Red;
            this.LV19S.Location = new System.Drawing.Point(147, 17);
            this.LV19S.Name = "LV19S";
            this.LV19S.Size = new System.Drawing.Size(5, 28);
            this.LV19S.TabIndex = 68;
            this.LV19S.Visible = false;
            this.LV19S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // LV5S
            // 
            this.LV5S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LV5S.BackColor = System.Drawing.Color.LimeGreen;
            this.LV5S.Location = new System.Drawing.Point(49, 17);
            this.LV5S.Name = "LV5S";
            this.LV5S.Size = new System.Drawing.Size(5, 28);
            this.LV5S.TabIndex = 54;
            this.LV5S.Visible = false;
            this.LV5S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // RV17S
            // 
            this.RV17S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RV17S.BackColor = System.Drawing.Color.Gold;
            this.RV17S.Location = new System.Drawing.Point(133, 46);
            this.RV17S.Name = "RV17S";
            this.RV17S.Size = new System.Drawing.Size(5, 28);
            this.RV17S.TabIndex = 88;
            this.RV17S.Visible = false;
            this.RV17S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // RV9S
            // 
            this.RV9S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RV9S.BackColor = System.Drawing.Color.Lime;
            this.RV9S.Location = new System.Drawing.Point(77, 46);
            this.RV9S.Name = "RV9S";
            this.RV9S.Size = new System.Drawing.Size(5, 28);
            this.RV9S.TabIndex = 80;
            this.RV9S.Visible = false;
            this.RV9S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // LV17S
            // 
            this.LV17S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LV17S.BackColor = System.Drawing.Color.Gold;
            this.LV17S.Location = new System.Drawing.Point(133, 17);
            this.LV17S.Name = "LV17S";
            this.LV17S.Size = new System.Drawing.Size(5, 28);
            this.LV17S.TabIndex = 66;
            this.LV17S.Visible = false;
            this.LV17S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // LV10S
            // 
            this.LV10S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LV10S.BackColor = System.Drawing.Color.Lime;
            this.LV10S.Location = new System.Drawing.Point(84, 17);
            this.LV10S.Name = "LV10S";
            this.LV10S.Size = new System.Drawing.Size(5, 28);
            this.LV10S.TabIndex = 59;
            this.LV10S.Visible = false;
            this.LV10S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // RV8S
            // 
            this.RV8S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RV8S.BackColor = System.Drawing.Color.Lime;
            this.RV8S.Location = new System.Drawing.Point(70, 46);
            this.RV8S.Name = "RV8S";
            this.RV8S.Size = new System.Drawing.Size(5, 28);
            this.RV8S.TabIndex = 79;
            this.RV8S.Visible = false;
            this.RV8S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // RV5S
            // 
            this.RV5S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RV5S.BackColor = System.Drawing.Color.LimeGreen;
            this.RV5S.Location = new System.Drawing.Point(49, 46);
            this.RV5S.Name = "RV5S";
            this.RV5S.Size = new System.Drawing.Size(5, 28);
            this.RV5S.TabIndex = 76;
            this.RV5S.Visible = false;
            this.RV5S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // LV8S
            // 
            this.LV8S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LV8S.BackColor = System.Drawing.Color.Lime;
            this.LV8S.Location = new System.Drawing.Point(70, 17);
            this.LV8S.Name = "LV8S";
            this.LV8S.Size = new System.Drawing.Size(5, 28);
            this.LV8S.TabIndex = 57;
            this.LV8S.Visible = false;
            this.LV8S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // LV6S
            // 
            this.LV6S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LV6S.BackColor = System.Drawing.Color.Lime;
            this.LV6S.Location = new System.Drawing.Point(56, 17);
            this.LV6S.Name = "LV6S";
            this.LV6S.Size = new System.Drawing.Size(5, 28);
            this.LV6S.TabIndex = 55;
            this.LV6S.Visible = false;
            this.LV6S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // RV16S
            // 
            this.RV16S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RV16S.BackColor = System.Drawing.Color.Yellow;
            this.RV16S.Location = new System.Drawing.Point(126, 46);
            this.RV16S.Name = "RV16S";
            this.RV16S.Size = new System.Drawing.Size(5, 28);
            this.RV16S.TabIndex = 87;
            this.RV16S.Visible = false;
            this.RV16S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // RV10S
            // 
            this.RV10S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RV10S.BackColor = System.Drawing.Color.Lime;
            this.RV10S.Location = new System.Drawing.Point(84, 46);
            this.RV10S.Name = "RV10S";
            this.RV10S.Size = new System.Drawing.Size(5, 28);
            this.RV10S.TabIndex = 81;
            this.RV10S.Visible = false;
            this.RV10S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // LV16S
            // 
            this.LV16S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LV16S.BackColor = System.Drawing.Color.Yellow;
            this.LV16S.Location = new System.Drawing.Point(126, 17);
            this.LV16S.Name = "LV16S";
            this.LV16S.Size = new System.Drawing.Size(5, 28);
            this.LV16S.TabIndex = 65;
            this.LV16S.Visible = false;
            this.LV16S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // RV6S
            // 
            this.RV6S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RV6S.BackColor = System.Drawing.Color.Lime;
            this.RV6S.Location = new System.Drawing.Point(56, 46);
            this.RV6S.Name = "RV6S";
            this.RV6S.Size = new System.Drawing.Size(5, 28);
            this.RV6S.TabIndex = 77;
            this.RV6S.Visible = false;
            this.RV6S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // RV12S
            // 
            this.RV12S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RV12S.BackColor = System.Drawing.Color.Chartreuse;
            this.RV12S.Location = new System.Drawing.Point(98, 46);
            this.RV12S.Name = "RV12S";
            this.RV12S.Size = new System.Drawing.Size(5, 28);
            this.RV12S.TabIndex = 83;
            this.RV12S.Visible = false;
            this.RV12S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // LV11S
            // 
            this.LV11S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LV11S.BackColor = System.Drawing.Color.Lime;
            this.LV11S.Location = new System.Drawing.Point(91, 17);
            this.LV11S.Name = "LV11S";
            this.LV11S.Size = new System.Drawing.Size(5, 28);
            this.LV11S.TabIndex = 60;
            this.LV11S.Visible = false;
            this.LV11S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // LV12S
            // 
            this.LV12S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LV12S.BackColor = System.Drawing.Color.Chartreuse;
            this.LV12S.Location = new System.Drawing.Point(98, 17);
            this.LV12S.Name = "LV12S";
            this.LV12S.Size = new System.Drawing.Size(5, 28);
            this.LV12S.TabIndex = 61;
            this.LV12S.Visible = false;
            this.LV12S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // RV11S
            // 
            this.RV11S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RV11S.BackColor = System.Drawing.Color.Lime;
            this.RV11S.Location = new System.Drawing.Point(91, 46);
            this.RV11S.Name = "RV11S";
            this.RV11S.Size = new System.Drawing.Size(5, 28);
            this.RV11S.TabIndex = 82;
            this.RV11S.Visible = false;
            this.RV11S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // RV15S
            // 
            this.RV15S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RV15S.BackColor = System.Drawing.Color.Yellow;
            this.RV15S.Location = new System.Drawing.Point(119, 46);
            this.RV15S.Name = "RV15S";
            this.RV15S.Size = new System.Drawing.Size(5, 28);
            this.RV15S.TabIndex = 86;
            this.RV15S.Visible = false;
            this.RV15S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // LV7S
            // 
            this.LV7S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LV7S.BackColor = System.Drawing.Color.Lime;
            this.LV7S.Location = new System.Drawing.Point(63, 17);
            this.LV7S.Name = "LV7S";
            this.LV7S.Size = new System.Drawing.Size(5, 28);
            this.LV7S.TabIndex = 56;
            this.LV7S.Visible = false;
            this.LV7S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // LV15S
            // 
            this.LV15S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LV15S.BackColor = System.Drawing.Color.Yellow;
            this.LV15S.Location = new System.Drawing.Point(119, 17);
            this.LV15S.Name = "LV15S";
            this.LV15S.Size = new System.Drawing.Size(5, 28);
            this.LV15S.TabIndex = 64;
            this.LV15S.Visible = false;
            this.LV15S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // RV7S
            // 
            this.RV7S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RV7S.BackColor = System.Drawing.Color.Lime;
            this.RV7S.Location = new System.Drawing.Point(63, 46);
            this.RV7S.Name = "RV7S";
            this.RV7S.Size = new System.Drawing.Size(5, 28);
            this.RV7S.TabIndex = 78;
            this.RV7S.Visible = false;
            this.RV7S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // RV14S
            // 
            this.RV14S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RV14S.BackColor = System.Drawing.Color.Yellow;
            this.RV14S.Location = new System.Drawing.Point(112, 46);
            this.RV14S.Name = "RV14S";
            this.RV14S.Size = new System.Drawing.Size(5, 28);
            this.RV14S.TabIndex = 85;
            this.RV14S.Visible = false;
            this.RV14S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // LV14S
            // 
            this.LV14S.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LV14S.BackColor = System.Drawing.Color.Yellow;
            this.LV14S.Location = new System.Drawing.Point(112, 17);
            this.LV14S.Name = "LV14S";
            this.LV14S.Size = new System.Drawing.Size(5, 28);
            this.LV14S.TabIndex = 63;
            this.LV14S.Visible = false;
            this.LV14S.Paint += new System.Windows.Forms.PaintEventHandler(this.Level_Paint);
            // 
            // EnginesBox
            // 
            this.EnginesBox.Controls.Add(this.WhatIsXAudio);
            this.EnginesBox.Controls.Add(this.WhatIsOutput);
            this.EnginesBox.Controls.Add(this.label2);
            this.EnginesBox.Controls.Add(this.AudioEngBox);
            this.EnginesBox.Location = new System.Drawing.Point(6, 3);
            this.EnginesBox.Name = "EnginesBox";
            this.EnginesBox.Size = new System.Drawing.Size(181, 95);
            this.EnginesBox.TabIndex = 0;
            this.EnginesBox.TabStop = false;
            this.EnginesBox.Text = "Audio engine settings";
            // 
            // WhatIsXAudio
            // 
            this.WhatIsXAudio.AutoSize = true;
            this.WhatIsXAudio.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(0)))), ((int)(((byte)(119)))));
            this.WhatIsXAudio.Location = new System.Drawing.Point(8, 68);
            this.WhatIsXAudio.Name = "WhatIsXAudio";
            this.WhatIsXAudio.Size = new System.Drawing.Size(51, 13);
            this.WhatIsXAudio.TabIndex = 16;
            this.WhatIsXAudio.TabStop = true;
            this.WhatIsXAudio.Text = "Engines?";
            this.WhatIsXAudio.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.WhatIsXAudio_Click);
            // 
            // WhatIsOutput
            // 
            this.WhatIsOutput.AutoSize = true;
            this.WhatIsOutput.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(0)))), ((int)(((byte)(119)))));
            this.WhatIsOutput.Location = new System.Drawing.Point(71, 68);
            this.WhatIsOutput.Name = "WhatIsOutput";
            this.WhatIsOutput.Size = new System.Drawing.Size(103, 13);
            this.WhatIsOutput.TabIndex = 15;
            this.WhatIsOutput.TabStop = true;
            this.WhatIsOutput.Text = "What\'s WAV mode?";
            this.WhatIsOutput.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.WhatIsOutput_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Renderer:";
            this.Requirements.SetToolTip(this.label2, "To change this in real-time, enable the \"Enable live changes for all the settings" +
        "\" function.");
            // 
            // AudioEngBox
            // 
            this.AudioEngBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AudioEngBox.FormattingEnabled = true;
            this.AudioEngBox.Items.AddRange(new object[] {
            "Audio to .WAV mode",
            "Microsoft DirectX Audio",
            "Audio Stream Input/Output",
            "Windows Audio Session API"});
            this.AudioEngBox.Location = new System.Drawing.Point(11, 37);
            this.AudioEngBox.Name = "AudioEngBox";
            this.AudioEngBox.Size = new System.Drawing.Size(159, 21);
            this.AudioEngBox.TabIndex = 1;
            this.Requirements.SetToolTip(this.AudioEngBox, "Changing this setting requires a restart of the audio stream.");
            this.AudioEngBox.SelectedIndexChanged += new System.EventHandler(this.AudioEngBox_SelectedIndexChanged);
            // 
            // AdditionalSettingsBox
            // 
            this.AdditionalSettingsBox.Controls.Add(this.ChangeMask);
            this.AdditionalSettingsBox.Controls.Add(this.AASButton);
            this.AdditionalSettingsBox.Controls.Add(this.MEPSButton);
            this.AdditionalSettingsBox.Location = new System.Drawing.Point(193, 3);
            this.AdditionalSettingsBox.Name = "AdditionalSettingsBox";
            this.AdditionalSettingsBox.Size = new System.Drawing.Size(159, 95);
            this.AdditionalSettingsBox.TabIndex = 1;
            this.AdditionalSettingsBox.TabStop = false;
            this.AdditionalSettingsBox.Text = "Additional settings";
            // 
            // ChangeMask
            // 
            this.ChangeMask.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ChangeMask.Location = new System.Drawing.Point(7, 14);
            this.ChangeMask.Name = "ChangeMask";
            this.ChangeMask.Size = new System.Drawing.Size(145, 23);
            this.ChangeMask.TabIndex = 2;
            this.ChangeMask.Text = "Change synthesizer\'s mask";
            this.ChangeMask.UseVisualStyleBackColor = true;
            this.ChangeMask.Click += new System.EventHandler(this.MaskSynthesizerAsAnother_Click);
            // 
            // AASButton
            // 
            this.AASButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AASButton.Location = new System.Drawing.Point(7, 39);
            this.AASButton.Name = "AASButton";
            this.AASButton.Size = new System.Drawing.Size(145, 23);
            this.AASButton.TabIndex = 3;
            this.AASButton.Text = "Advanced audio settings";
            this.AASButton.UseVisualStyleBackColor = true;
            this.AASButton.Click += new System.EventHandler(this.AASMenu_Click);
            // 
            // MEPSButton
            // 
            this.MEPSButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.MEPSButton.Location = new System.Drawing.Point(7, 64);
            this.MEPSButton.Name = "MEPSButton";
            this.MEPSButton.Size = new System.Drawing.Size(145, 23);
            this.MEPSButton.TabIndex = 4;
            this.MEPSButton.Text = "MIDI events parser settings";
            this.MEPSButton.UseVisualStyleBackColor = true;
            this.MEPSButton.Click += new System.EventHandler(this.MEPSMenu_Click);
            // 
            // resetToDefaultToolStripMenuItem
            // 
            this.resetToDefaultToolStripMenuItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.resetToDefaultToolStripMenuItem.Location = new System.Drawing.Point(450, 362);
            this.resetToDefaultToolStripMenuItem.Name = "resetToDefaultToolStripMenuItem";
            this.resetToDefaultToolStripMenuItem.Size = new System.Drawing.Size(90, 23);
            this.resetToDefaultToolStripMenuItem.TabIndex = 17;
            this.resetToDefaultToolStripMenuItem.Text = "Reset to default";
            this.resetToDefaultToolStripMenuItem.UseVisualStyleBackColor = true;
            this.resetToDefaultToolStripMenuItem.Click += new System.EventHandler(this.resetToDefaultToolStripMenuItem_Click);
            // 
            // applySettingsToolStripMenuItem
            // 
            this.applySettingsToolStripMenuItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.applySettingsToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ControlText;
            this.applySettingsToolStripMenuItem.Location = new System.Drawing.Point(545, 362);
            this.applySettingsToolStripMenuItem.Name = "applySettingsToolStripMenuItem";
            this.applySettingsToolStripMenuItem.Size = new System.Drawing.Size(90, 23);
            this.applySettingsToolStripMenuItem.TabIndex = 16;
            this.applySettingsToolStripMenuItem.Text = "Apply settings";
            this.applySettingsToolStripMenuItem.UseVisualStyleBackColor = true;
            this.applySettingsToolStripMenuItem.Click += new System.EventHandler(this.applySettingsToolStripMenuItem_Click);
            this.applySettingsToolStripMenuItem.Paint += new System.Windows.Forms.PaintEventHandler(this.applySettingsToolStripMenuItem_Paint);
            // 
            // OutputSettingsBox
            // 
            this.OutputSettingsBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OutputSettingsBox.BackColor = System.Drawing.Color.Transparent;
            this.OutputSettingsBox.Controls.Add(this.SincConv);
            this.OutputSettingsBox.Controls.Add(this.SincConvLab);
            this.OutputSettingsBox.Controls.Add(this.DrvHzLabel);
            this.OutputSettingsBox.Controls.Add(this.Frequency);
            this.OutputSettingsBox.Controls.Add(this.SincInter);
            this.OutputSettingsBox.Controls.Add(this.BufferText);
            this.OutputSettingsBox.Controls.Add(this.bufsize);
            this.OutputSettingsBox.Controls.Add(this.EnableSFX);
            this.OutputSettingsBox.Location = new System.Drawing.Point(6, 245);
            this.OutputSettingsBox.Name = "OutputSettingsBox";
            this.OutputSettingsBox.Size = new System.Drawing.Size(629, 108);
            this.OutputSettingsBox.TabIndex = 3;
            this.OutputSettingsBox.TabStop = false;
            this.OutputSettingsBox.Text = "Audio output settings";
            // 
            // SincConv
            // 
            this.SincConv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SincConv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SincConv.Enabled = false;
            this.SincConv.FormattingEnabled = true;
            this.SincConv.Items.AddRange(new object[] {
            "Linear inter.",
            "8 point sinc",
            "16 point sinc",
            "32 point sinc"});
            this.SincConv.Location = new System.Drawing.Point(535, 16);
            this.SincConv.Name = "SincConv";
            this.SincConv.Size = new System.Drawing.Size(85, 21);
            this.SincConv.TabIndex = 12;
            this.Requirements.SetToolTip(this.SincConv, resources.GetString("SincConv.ToolTip"));
            // 
            // SincConvLab
            // 
            this.SincConvLab.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SincConvLab.AutoSize = true;
            this.SincConvLab.Enabled = false;
            this.SincConvLab.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.SincConvLab.Location = new System.Drawing.Point(439, 19);
            this.SincConvLab.Name = "SincConvLab";
            this.SincConvLab.Size = new System.Drawing.Size(96, 13);
            this.SincConvLab.TabIndex = 17;
            this.SincConvLab.Text = "Conversion quality:";
            this.Requirements.SetToolTip(this.SincConvLab, resources.GetString("SincConvLab.ToolTip"));
            // 
            // DrvHzLabel
            // 
            this.DrvHzLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.DrvHzLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.DrvHzLabel.Location = new System.Drawing.Point(6, 58);
            this.DrvHzLabel.Name = "DrvHzLabel";
            this.DrvHzLabel.Size = new System.Drawing.Size(544, 13);
            this.DrvHzLabel.TabIndex = 13;
            this.DrvHzLabel.Text = "Output sample rate (in Hertz)";
            this.Requirements.SetToolTip(this.DrvHzLabel, "To change this in real-time, enable the \"Enable live changes for all the settings" +
        "\" function.");
            // 
            // Frequency
            // 
            this.Frequency.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Frequency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Frequency.FormattingEnabled = true;
            this.Frequency.Items.AddRange(new object[] {
            "384000",
            "352800",
            "192000",
            "176400",
            "96000",
            "88200",
            "74750",
            "66150",
            "64000",
            "50400",
            "50000",
            "48000",
            "47250",
            "44100",
            "44056",
            "37800",
            "34750",
            "32000",
            "22050",
            "16000",
            "11025",
            "8000",
            "4000"});
            this.Frequency.Location = new System.Drawing.Point(556, 54);
            this.Frequency.Name = "Frequency";
            this.Frequency.Size = new System.Drawing.Size(64, 21);
            this.Frequency.TabIndex = 14;
            this.Requirements.SetToolTip(this.Frequency, "Changing this setting requires a restart of the audio stream.");
            this.Frequency.SelectedIndexChanged += new System.EventHandler(this.Frequency_SelectedIndexChanged);
            this.Frequency.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CheckIfUserPressesEnter);
            // 
            // SincInter
            // 
            this.SincInter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SincInter.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.SincInter.Location = new System.Drawing.Point(7, 17);
            this.SincInter.Name = "SincInter";
            this.SincInter.Size = new System.Drawing.Size(427, 17);
            this.SincInter.TabIndex = 11;
            this.SincInter.Text = "Enable sinc interpolation (improves audio quality, but increases rendering time)";
            this.ButtonsDesc.SetToolTip(this.SincInter, resources.GetString("SincInter.ToolTip"));
            this.SincInter.UseVisualStyleBackColor = true;
            this.SincInter.CheckedChanged += new System.EventHandler(this.SincInter_CheckedChanged);
            // 
            // BufferText
            // 
            this.BufferText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BufferText.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BufferText.Location = new System.Drawing.Point(6, 73);
            this.BufferText.Name = "BufferText";
            this.BufferText.Size = new System.Drawing.Size(529, 31);
            this.BufferText.TabIndex = 17;
            this.BufferText.Text = "Driver buffer length (in ms, from 1 to 1000)";
            this.BufferText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bufsize
            // 
            this.bufsize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bufsize.Location = new System.Drawing.Point(556, 79);
            this.bufsize.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.bufsize.Name = "bufsize";
            this.bufsize.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.bufsize.Size = new System.Drawing.Size(64, 20);
            this.bufsize.TabIndex = 15;
            this.bufsize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Requirements.SetToolTip(this.bufsize, "Changing this setting requires a restart of the audio stream.");
            this.bufsize.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.bufsize.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CheckIfUserPressesEnter);
            // 
            // EnableSFX
            // 
            this.EnableSFX.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.EnableSFX.AutoSize = true;
            this.EnableSFX.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.EnableSFX.Location = new System.Drawing.Point(7, 35);
            this.EnableSFX.Name = "EnableSFX";
            this.EnableSFX.Size = new System.Drawing.Size(430, 17);
            this.EnableSFX.TabIndex = 13;
            this.EnableSFX.Text = "Enable sound effects (i.e. reverb and chorus; disabling this can reduce rendering" +
    " time)";
            this.EnableSFX.UseVisualStyleBackColor = true;
            // 
            // SynthSettingsBox
            // 
            this.SynthSettingsBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SynthSettingsBox.BackColor = System.Drawing.Color.Transparent;
            this.SynthSettingsBox.Controls.Add(this.MaxCPU);
            this.SynthSettingsBox.Controls.Add(this.SysResetIgnore);
            this.SynthSettingsBox.Controls.Add(this.RenderingTimeLabel);
            this.SynthSettingsBox.Controls.Add(this.VoiceLimitLabel);
            this.SynthSettingsBox.Controls.Add(this.PolyphonyLimit);
            this.SynthSettingsBox.Controls.Add(this.Preload);
            this.SynthSettingsBox.Controls.Add(this.NoteOffCheck);
            this.SynthSettingsBox.Location = new System.Drawing.Point(6, 104);
            this.SynthSettingsBox.Name = "SynthSettingsBox";
            this.SynthSettingsBox.Size = new System.Drawing.Size(629, 135);
            this.SynthSettingsBox.TabIndex = 2;
            this.SynthSettingsBox.TabStop = false;
            this.SynthSettingsBox.Text = "Synthesizer\'s settings";
            // 
            // MaxCPU
            // 
            this.MaxCPU.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.MaxCPU.Location = new System.Drawing.Point(556, 104);
            this.MaxCPU.Name = "MaxCPU";
            this.MaxCPU.Size = new System.Drawing.Size(64, 20);
            this.MaxCPU.TabIndex = 10;
            this.MaxCPU.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ButtonsDesc.SetToolTip(this.MaxCPU, resources.GetString("MaxCPU.ToolTip"));
            this.MaxCPU.Value = new decimal(new int[] {
            75,
            0,
            0,
            0});
            this.MaxCPU.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CheckIfUserPressesEnter);
            // 
            // SysResetIgnore
            // 
            this.SysResetIgnore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SysResetIgnore.AutoSize = true;
            this.SysResetIgnore.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.SysResetIgnore.Location = new System.Drawing.Point(8, 37);
            this.SysResetIgnore.Name = "SysResetIgnore";
            this.SysResetIgnore.Size = new System.Drawing.Size(565, 17);
            this.SysResetIgnore.TabIndex = 7;
            this.SysResetIgnore.Text = "Ignore system reset events when the system mode is unchanged (might cause issues " +
    "with program change events)";
            this.SysResetIgnore.UseVisualStyleBackColor = true;
            // 
            // RenderingTimeLabel
            // 
            this.RenderingTimeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RenderingTimeLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.RenderingTimeLabel.Location = new System.Drawing.Point(6, 107);
            this.RenderingTimeLabel.Name = "RenderingTimeLabel";
            this.RenderingTimeLabel.Size = new System.Drawing.Size(544, 13);
            this.RenderingTimeLabel.TabIndex = 11;
            this.RenderingTimeLabel.Text = "Maximum rendering time (percentage, set to 0% to disable it)";
            this.ButtonsDesc.SetToolTip(this.RenderingTimeLabel, resources.GetString("RenderingTimeLabel.ToolTip"));
            // 
            // VoiceLimitLabel
            // 
            this.VoiceLimitLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.VoiceLimitLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.VoiceLimitLabel.Location = new System.Drawing.Point(6, 82);
            this.VoiceLimitLabel.Name = "VoiceLimitLabel";
            this.VoiceLimitLabel.Size = new System.Drawing.Size(544, 13);
            this.VoiceLimitLabel.TabIndex = 9;
            this.VoiceLimitLabel.Text = "Driver voice limit (1 to 100,000 voices)";
            this.ButtonsDesc.SetToolTip(this.VoiceLimitLabel, "If there are currently more voices active than the new limit, then some voices wi" +
        "ll be killed to meet the limit.");
            // 
            // PolyphonyLimit
            // 
            this.PolyphonyLimit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.PolyphonyLimit.Location = new System.Drawing.Point(556, 80);
            this.PolyphonyLimit.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.PolyphonyLimit.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.PolyphonyLimit.Name = "PolyphonyLimit";
            this.PolyphonyLimit.Size = new System.Drawing.Size(64, 20);
            this.PolyphonyLimit.TabIndex = 9;
            this.PolyphonyLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.PolyphonyLimit.ThousandsSeparator = true;
            this.Requirements.SetToolTip(this.PolyphonyLimit, "If there are currently more voices active than the new limit, then some voices wi" +
        "ll be killed to meet the limit.");
            this.PolyphonyLimit.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.PolyphonyLimit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CheckIfUserPressesEnter);
            // 
            // Preload
            // 
            this.Preload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Preload.AutoSize = true;
            this.Preload.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Preload.Location = new System.Drawing.Point(8, 17);
            this.Preload.Name = "Preload";
            this.Preload.Size = new System.Drawing.Size(461, 17);
            this.Preload.TabIndex = 6;
            this.Preload.Text = "Preload SoundFont in memory (might cause a delay depending on computer and SoundF" +
    "ont)";
            this.Preload.UseVisualStyleBackColor = true;
            // 
            // NoteOffCheck
            // 
            this.NoteOffCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.NoteOffCheck.AutoSize = true;
            this.NoteOffCheck.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.NoteOffCheck.Location = new System.Drawing.Point(8, 57);
            this.NoteOffCheck.Name = "NoteOffCheck";
            this.NoteOffCheck.Size = new System.Drawing.Size(464, 17);
            this.NoteOffCheck.TabIndex = 8;
            this.NoteOffCheck.Text = "Only release the oldest instance of a note upon note-off event (could increase re" +
    "ndering time)";
            this.NoteOffCheck.UseVisualStyleBackColor = true;
            // 
            // SettingsPresetsBtn
            // 
            this.SettingsPresetsBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SettingsPresetsBtn.Location = new System.Drawing.Point(55, 362);
            this.SettingsPresetsBtn.Name = "SettingsPresetsBtn";
            this.SettingsPresetsBtn.Size = new System.Drawing.Size(72, 23);
            this.SettingsPresetsBtn.TabIndex = 18;
            this.SettingsPresetsBtn.Text = "Pre-made";
            this.SettingsPresetsBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.SettingsPresetsBtn.UseVisualStyleBackColor = true;
            this.SettingsPresetsBtn.Click += new System.EventHandler(this.SettingsPresetsBtn_Click);
            // 
            // SoundFontTab
            // 
            this.SoundFontTab.BackColor = System.Drawing.Color.Transparent;
            this.SoundFontTab.Controls.Add(this.SFlg);
            this.SoundFontTab.Controls.Add(this.Separator);
            this.SoundFontTab.Controls.Add(this.Lis);
            this.SoundFontTab.Controls.Add(this.EL);
            this.SoundFontTab.Controls.Add(this.LoadToApp);
            this.SoundFontTab.Controls.Add(this.IEL);
            this.SoundFontTab.Controls.Add(this.BankPresetOverride);
            this.SoundFontTab.Controls.Add(this.SelectedListBox);
            this.SoundFontTab.Controls.Add(this.label1);
            this.SoundFontTab.Controls.Add(this.DisableSF);
            this.SoundFontTab.Controls.Add(this.EnableSF);
            this.SoundFontTab.Controls.Add(this.ListOverride);
            this.SoundFontTab.Controls.Add(this.CLi);
            this.SoundFontTab.Controls.Add(this.MvD);
            this.SoundFontTab.Controls.Add(this.MvU);
            this.SoundFontTab.Controls.Add(this.RmvSF);
            this.SoundFontTab.Controls.Add(this.AddSF);
            this.SoundFontTab.Location = new System.Drawing.Point(4, 23);
            this.SoundFontTab.Name = "SoundFontTab";
            this.SoundFontTab.Padding = new System.Windows.Forms.Padding(3);
            this.SoundFontTab.Size = new System.Drawing.Size(641, 394);
            this.SoundFontTab.TabIndex = 0;
            this.SoundFontTab.Text = "Lists editor";
            // 
            // SFlg
            // 
            this.SFlg.AccessibleDescription = "SoundFont list guide";
            this.SFlg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SFlg.BackColor = System.Drawing.Color.Transparent;
            this.SFlg.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SFlg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SFlg.ForeColor = System.Drawing.Color.Transparent;
            this.SFlg.Location = new System.Drawing.Point(581, 6);
            this.SFlg.Name = "SFlg";
            this.SFlg.Size = new System.Drawing.Size(24, 24);
            this.SFlg.TabIndex = 16;
            this.SFlg.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.ButtonsDesc.SetToolTip(this.SFlg, "SoundFont list guide");
            this.SFlg.UseVisualStyleBackColor = false;
            this.SFlg.Click += new System.EventHandler(this.SFlg_Click);
            this.SFlg.Paint += new System.Windows.Forms.PaintEventHandler(this.SoundFontListGuideButton);
            // 
            // Separator
            // 
            this.Separator.AutoSize = true;
            this.Separator.Enabled = false;
            this.Separator.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Separator.Location = new System.Drawing.Point(159, 6);
            this.Separator.Name = "Separator";
            this.Separator.Size = new System.Drawing.Size(14, 20);
            this.Separator.TabIndex = 15;
            this.Separator.Text = "|";
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
            listViewItem1});
            this.Lis.LabelWrap = false;
            this.Lis.LineAfter = -1;
            this.Lis.LineBefore = -1;
            this.Lis.Location = new System.Drawing.Point(5, 36);
            this.Lis.Name = "Lis";
            this.Lis.ShowGroups = false;
            this.Lis.Size = new System.Drawing.Size(600, 341);
            this.Lis.TabIndex = 3;
            this.Lis.UseCompatibleStateImageBehavior = false;
            this.Lis.View = System.Windows.Forms.View.Details;
            this.Lis.DragDrop += new System.Windows.Forms.DragEventHandler(this.Lis_DragDrop);
            this.Lis.DragEnter += new System.Windows.Forms.DragEventHandler(this.Lis_DragEnter);
            this.Lis.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Lis_KeyDown);
            this.Lis.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Lis_MouseDown);
            this.Lis.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Lis_MouseMove);
            this.Lis.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Lis_MouseUp);
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
            // EL
            // 
            this.EL.AccessibleDescription = "Export SoundFonts list";
            this.EL.AccessibleName = "Export SoundFonts list";
            this.EL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.EL.BackColor = System.Drawing.Color.Transparent;
            this.EL.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.EL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.EL.ForeColor = System.Drawing.Color.Transparent;
            this.EL.Location = new System.Drawing.Point(611, 347);
            this.EL.Name = "EL";
            this.EL.Size = new System.Drawing.Size(24, 30);
            this.EL.TabIndex = 13;
            this.EL.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.ButtonsDesc.SetToolTip(this.EL, "Export SoundFonts list");
            this.EL.UseVisualStyleBackColor = false;
            this.EL.Click += new System.EventHandler(this.EL_Click);
            this.EL.Paint += new System.Windows.Forms.PaintEventHandler(this.ImportListButton);
            // 
            // LoadToApp
            // 
            this.LoadToApp.AccessibleDescription = "Load SoundFonts list to app";
            this.LoadToApp.AccessibleName = "Load SoundFonts list to app";
            this.LoadToApp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LoadToApp.BackColor = System.Drawing.Color.Transparent;
            this.LoadToApp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.LoadToApp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LoadToApp.ForeColor = System.Drawing.Color.Transparent;
            this.LoadToApp.Location = new System.Drawing.Point(611, 156);
            this.LoadToApp.Name = "LoadToApp";
            this.LoadToApp.Size = new System.Drawing.Size(24, 24);
            this.LoadToApp.TabIndex = 9;
            this.LoadToApp.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.ButtonsDesc.SetToolTip(this.LoadToApp, "Load SoundFonts list to app");
            this.LoadToApp.UseVisualStyleBackColor = false;
            this.LoadToApp.Click += new System.EventHandler(this.LoadToApp_Click);
            this.LoadToApp.Paint += new System.Windows.Forms.PaintEventHandler(this.ButtonLoad);
            // 
            // IEL
            // 
            this.IEL.AccessibleDescription = "Import SoundFonts list";
            this.IEL.AccessibleName = "Import SoundFonts list";
            this.IEL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.IEL.BackColor = System.Drawing.Color.Transparent;
            this.IEL.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.IEL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.IEL.ForeColor = System.Drawing.Color.Transparent;
            this.IEL.Location = new System.Drawing.Point(611, 318);
            this.IEL.Name = "IEL";
            this.IEL.Size = new System.Drawing.Size(24, 30);
            this.IEL.TabIndex = 12;
            this.IEL.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.ButtonsDesc.SetToolTip(this.IEL, "Import SoundFonts list");
            this.IEL.UseVisualStyleBackColor = false;
            this.IEL.Click += new System.EventHandler(this.IEL_Click);
            this.IEL.Paint += new System.Windows.Forms.PaintEventHandler(this.ImportListButton);
            // 
            // BankPresetOverride
            // 
            this.BankPresetOverride.AutoSize = true;
            this.BankPresetOverride.Location = new System.Drawing.Point(173, 11);
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
            "List 1",
            "List 2",
            "List 3",
            "List 4",
            "List 5",
            "List 6",
            "List 7",
            "List 8"});
            this.SelectedListBox.Location = new System.Drawing.Point(100, 8);
            this.SelectedListBox.Name = "SelectedListBox";
            this.SelectedListBox.Size = new System.Drawing.Size(57, 21);
            this.SelectedListBox.TabIndex = 1;
            this.SelectedListBox.SelectedIndexChanged += new System.EventHandler(this.SelectedListBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 0;
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
            this.DisableSF.Location = new System.Drawing.Point(611, 216);
            this.DisableSF.Name = "DisableSF";
            this.DisableSF.Size = new System.Drawing.Size(24, 24);
            this.DisableSF.TabIndex = 11;
            this.DisableSF.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.ButtonsDesc.SetToolTip(this.DisableSF, "Disable SoundFont(s)");
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
            this.EnableSF.Location = new System.Drawing.Point(611, 193);
            this.EnableSF.Name = "EnableSF";
            this.EnableSF.Size = new System.Drawing.Size(24, 24);
            this.EnableSF.TabIndex = 10;
            this.EnableSF.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.ButtonsDesc.SetToolTip(this.EnableSF, "Enable SoundFont(s)");
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
            this.ListOverride.Location = new System.Drawing.Point(2, 380);
            this.ListOverride.Name = "ListOverride";
            this.ListOverride.Size = new System.Drawing.Size(414, 13);
            this.ListOverride.TabIndex = 14;
            this.ListOverride.Text = "The last SoundFont will override the ones above it (loading order is from top to " +
    "bottom).";
            // 
            // CLi
            // 
            this.CLi.AccessibleDescription = "Clear SoundFont list";
            this.CLi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CLi.BackColor = System.Drawing.Color.Transparent;
            this.CLi.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CLi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CLi.ForeColor = System.Drawing.Color.Transparent;
            this.CLi.Location = new System.Drawing.Point(611, 6);
            this.CLi.Name = "CLi";
            this.CLi.Size = new System.Drawing.Size(24, 24);
            this.CLi.TabIndex = 4;
            this.CLi.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.ButtonsDesc.SetToolTip(this.CLi, "Clear SoundFont list");
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
            this.MvD.Location = new System.Drawing.Point(611, 119);
            this.MvD.Name = "MvD";
            this.MvD.Size = new System.Drawing.Size(24, 24);
            this.MvD.TabIndex = 8;
            this.MvD.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.ButtonsDesc.SetToolTip(this.MvD, "Move SoundFont down");
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
            this.MvU.Location = new System.Drawing.Point(611, 96);
            this.MvU.Name = "MvU";
            this.MvU.Size = new System.Drawing.Size(24, 24);
            this.MvU.TabIndex = 7;
            this.MvU.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.ButtonsDesc.SetToolTip(this.MvU, "Move SoundFont up");
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
            this.RmvSF.Location = new System.Drawing.Point(611, 59);
            this.RmvSF.Name = "RmvSF";
            this.RmvSF.Size = new System.Drawing.Size(24, 24);
            this.RmvSF.TabIndex = 6;
            this.RmvSF.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.ButtonsDesc.SetToolTip(this.RmvSF, "Remove SoundFont(s)");
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
            this.AddSF.Location = new System.Drawing.Point(611, 36);
            this.AddSF.Name = "AddSF";
            this.AddSF.Size = new System.Drawing.Size(24, 24);
            this.AddSF.TabIndex = 5;
            this.AddSF.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.ButtonsDesc.SetToolTip(this.AddSF, "Add SoundFont(s)");
            this.AddSF.UseVisualStyleBackColor = false;
            this.AddSF.Click += new System.EventHandler(this.AddSF_Click);
            this.AddSF.Paint += new System.Windows.Forms.PaintEventHandler(this.ButtonAddRemove);
            // 
            // TabsForTheControls
            // 
            this.TabsForTheControls.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.TabsForTheControls.AllowDrop = true;
            this.TabsForTheControls.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TabsForTheControls.Controls.Add(this.SoundFontTab);
            this.TabsForTheControls.Controls.Add(this.Settings);
            this.TabsForTheControls.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.TabsForTheControls.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TabsForTheControls.ImageList = this.TabImgs;
            this.TabsForTheControls.Location = new System.Drawing.Point(0, 0);
            this.TabsForTheControls.MinimumSize = new System.Drawing.Size(629, 387);
            this.TabsForTheControls.Name = "TabsForTheControls";
            this.TabsForTheControls.SelectedIndex = 0;
            this.TabsForTheControls.Size = new System.Drawing.Size(649, 421);
            this.TabsForTheControls.TabIndex = 0;
            // 
            // TabImgs
            // 
            this.TabImgs.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("TabImgs.ImageStream")));
            this.TabImgs.TransparentColor = System.Drawing.Color.Transparent;
            this.TabImgs.Images.SetKeyName(0, "ListIcon");
            this.TabImgs.Images.SetKeyName(1, "SettingsIcon");
            // 
            // SettingsPresets
            // 
            this.SettingsPresets.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.MSGSWSEmu,
            this.menuItem50,
            this.keppysSteinwayPianoRealismToolStripMenuItem,
            this.menuItem41,
            this.lowLatencyPresetToolStripMenuItem,
            this.SBLowLatToolStripMenuItem,
            this.ProLowLatToolStripMenuItem,
            this.menuItem36,
            this.blackMIDIsPresetToolStripMenuItem,
            this.menuItem44,
            this.chiptunesRetrogamingToolStripMenuItem});
            // 
            // MSGSWSEmu
            // 
            this.MSGSWSEmu.Index = 0;
            this.MSGSWSEmu.Text = "Microsoft GS Wavetable Synth emulation";
            this.MSGSWSEmu.Click += new System.EventHandler(this.MSGSWSEmu_Click);
            // 
            // menuItem50
            // 
            this.menuItem50.Index = 1;
            this.menuItem50.Text = "-";
            // 
            // keppysSteinwayPianoRealismToolStripMenuItem
            // 
            this.keppysSteinwayPianoRealismToolStripMenuItem.Index = 2;
            this.keppysSteinwayPianoRealismToolStripMenuItem.Text = "High fidelity audio (For HQ SoundFonts)";
            this.keppysSteinwayPianoRealismToolStripMenuItem.Click += new System.EventHandler(this.keppysSteinwayPianoRealismToolStripMenuItem_Click);
            // 
            // menuItem41
            // 
            this.menuItem41.Index = 3;
            this.menuItem41.Text = "-";
            // 
            // lowLatencyPresetToolStripMenuItem
            // 
            this.lowLatencyPresetToolStripMenuItem.Index = 4;
            this.lowLatencyPresetToolStripMenuItem.Text = "Low latency (All sound cards)";
            this.lowLatencyPresetToolStripMenuItem.Click += new System.EventHandler(this.lowLatencyPresetToolStripMenuItem_Click);
            // 
            // SBLowLatToolStripMenuItem
            // 
            this.SBLowLatToolStripMenuItem.Index = 5;
            this.SBLowLatToolStripMenuItem.Text = "Low latency (SoundBlaster)";
            this.SBLowLatToolStripMenuItem.Click += new System.EventHandler(this.SBLowLatToolStripMenuItem_Click);
            // 
            // ProLowLatToolStripMenuItem
            // 
            this.ProLowLatToolStripMenuItem.Index = 6;
            this.ProLowLatToolStripMenuItem.Text = "Low latency (Professional environments)";
            this.ProLowLatToolStripMenuItem.Click += new System.EventHandler(this.ProLowLatToolStripMenuItem_Click);
            // 
            // menuItem36
            // 
            this.menuItem36.Index = 7;
            this.menuItem36.Text = "-";
            // 
            // blackMIDIsPresetToolStripMenuItem
            // 
            this.blackMIDIsPresetToolStripMenuItem.Index = 8;
            this.blackMIDIsPresetToolStripMenuItem.Text = "Black MIDIs";
            this.blackMIDIsPresetToolStripMenuItem.Click += new System.EventHandler(this.blackMIDIsPresetToolStripMenuItem_Click);
            // 
            // menuItem44
            // 
            this.menuItem44.Enabled = false;
            this.menuItem44.Index = 9;
            this.menuItem44.Text = "Roland MT-32 mode";
            this.menuItem44.Click += new System.EventHandler(this.MT32Mode_Click);
            // 
            // chiptunesRetrogamingToolStripMenuItem
            // 
            this.chiptunesRetrogamingToolStripMenuItem.Index = 10;
            this.chiptunesRetrogamingToolStripMenuItem.Text = "Chiptunes/Retrogaming";
            this.chiptunesRetrogamingToolStripMenuItem.Click += new System.EventHandler(this.chiptunesRetrogamingToolStripMenuItem_Click);
            // 
            // SavedLabel
            // 
            this.SavedLabel.Interval = 1;
            this.SavedLabel.Tick += new System.EventHandler(this.SavedLabel_Tick);
            // 
            // StatusStrip
            // 
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel,
            this.Status,
            this.StatusDoneOr,
            this.toolStripStatusLabel2,
            this.UpdateStatus,
            this.VersionLabel});
            this.StatusStrip.Location = new System.Drawing.Point(0, 420);
            this.StatusStrip.MaximumSize = new System.Drawing.Size(0, 22);
            this.StatusStrip.Name = "StatusStrip";
            this.StatusStrip.Size = new System.Drawing.Size(647, 22);
            this.StatusStrip.SizingGrip = false;
            this.StatusStrip.TabIndex = 33;
            this.StatusStrip.Text = "StatusStripperino";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(75, 17);
            this.StatusLabel.Text = "Last message:";
            // 
            // Status
            // 
            this.Status.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(430, 17);
            this.Status.Spring = true;
            this.Status.Text = "Error.";
            this.Status.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // StatusDoneOr
            // 
            this.StatusDoneOr.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatusDoneOr.Name = "StatusDoneOr";
            this.StatusDoneOr.Size = new System.Drawing.Size(51, 17);
            this.StatusDoneOr.Text = "Nothing";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabel2.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.toolStripStatusLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(17, 17);
            this.toolStripStatusLabel2.Text = "  ";
            // 
            // UpdateStatus
            // 
            this.UpdateStatus.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.UpdateStatus.Name = "UpdateStatus";
            this.UpdateStatus.Size = new System.Drawing.Size(0, 17);
            this.UpdateStatus.Click += new System.EventHandler(this.CheckUpdatesStartUp);
            this.UpdateStatus.MouseEnter += new System.EventHandler(this.SetHandCursor);
            this.UpdateStatus.MouseLeave += new System.EventHandler(this.SetDefaultCursor);
            // 
            // VersionLabel
            // 
            this.VersionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VersionLabel.ForeColor = System.Drawing.SystemColors.GrayText;
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(59, 17);
            this.VersionLabel.Text = "Version {0}";
            this.VersionLabel.Click += new System.EventHandler(this.CheckUpdatesStartUp);
            this.VersionLabel.MouseEnter += new System.EventHandler(this.SetHandCursor);
            this.VersionLabel.MouseLeave += new System.EventHandler(this.SetDefaultCursor);
            // 
            // ButtonsDesc
            // 
            this.ButtonsDesc.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.ButtonsDesc.ToolTipTitle = "Information";
            // 
            // Requirements
            // 
            this.Requirements.AutomaticDelay = 100;
            this.Requirements.AutoPopDelay = 10000;
            this.Requirements.InitialDelay = 100;
            this.Requirements.IsBalloon = true;
            this.Requirements.ReshowDelay = 20;
            this.Requirements.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.Requirements.ToolTipTitle = "Requirement";
            // 
            // CheckUpdates
            // 
            this.CheckUpdates.DoWork += new System.ComponentModel.DoWorkEventHandler(this.CheckUpdates_DoWork);
            // 
            // ExportPresetDialog
            // 
            this.ExportPresetDialog.Filter = "Preset files|*.ompr";
            // 
            // ImportPresetDialog
            // 
            this.ImportPresetDialog.Filter = "Preset files|*.kspr;*.ompr";
            // 
            // openDebugWindowToolStripMenuItem
            // 
            this.openDebugWindowToolStripMenuItem.Index = 0;
            this.openDebugWindowToolStripMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlD;
            this.openDebugWindowToolStripMenuItem.Text = "Open the debug window";
            this.openDebugWindowToolStripMenuItem.Click += new System.EventHandler(this.openDebugWindowToolStripMenuItem_Click);
            // 
            // openTheMixerToolStripMenuItem
            // 
            this.openTheMixerToolStripMenuItem.Index = 1;
            this.openTheMixerToolStripMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlM;
            this.openTheMixerToolStripMenuItem.Text = "Open the mixer";
            this.openTheMixerToolStripMenuItem.Click += new System.EventHandler(this.openTheMixerToolStripMenuItem_Click);
            // 
            // openTheBlacklistManagerToolStripMenuItem
            // 
            this.openTheBlacklistManagerToolStripMenuItem.Index = 2;
            this.openTheBlacklistManagerToolStripMenuItem.Text = "Open the blacklist manager";
            this.openTheBlacklistManagerToolStripMenuItem.Click += new System.EventHandler(this.openTheBlacklistManagerToolStripMenuItem_Click);
            // 
            // assignASoundfontListToASpecificAppToolStripMenuItemToolStripMenuItem
            // 
            this.assignASoundfontListToASpecificAppToolStripMenuItemToolStripMenuItem.Index = 10;
            this.assignASoundfontListToASpecificAppToolStripMenuItemToolStripMenuItem.Text = "Assign a soundfont list to a specific app";
            this.assignASoundfontListToASpecificAppToolStripMenuItemToolStripMenuItem.Click += new System.EventHandler(this.assignASoundfontListToASpecificAppToolStripMenuItem_Click);
            // 
            // menuItem9
            // 
            this.menuItem9.Index = 4;
            this.menuItem9.Text = "-";
            // 
            // OmniMapperCpl
            // 
            this.OmniMapperCpl.Index = 5;
            this.OmniMapperCpl.Text = "Open the OmniMapper control panel";
            this.OmniMapperCpl.Click += new System.EventHandler(this.AMIDIMapCpl_Click);
            // 
            // menuItem13
            // 
            this.menuItem13.Index = 6;
            this.menuItem13.Text = "-";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Index = 7;
            this.exitToolStripMenuItem.Shortcut = System.Windows.Forms.Shortcut.AltF4;
            this.exitToolStripMenuItem.Text = "Close the configurator";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.openDebugWindowToolStripMenuItem,
            this.openTheMixerToolStripMenuItem,
            this.openTheBlacklistManagerToolStripMenuItem,
            this.openTheRivaTunerOSDManagerToolStripMenuItem,
            this.menuItem9,
            this.OmniMapperCpl,
            this.menuItem13,
            this.exitToolStripMenuItem});
            this.menuItem1.MergeType = System.Windows.Forms.MenuMerge.MergeItems;
            this.menuItem1.Text = "File";
            // 
            // openTheRivaTunerOSDManagerToolStripMenuItem
            // 
            this.openTheRivaTunerOSDManagerToolStripMenuItem.Index = 3;
            this.openTheRivaTunerOSDManagerToolStripMenuItem.Text = "Open the RivaTuner OSD manager";
            this.openTheRivaTunerOSDManagerToolStripMenuItem.Click += new System.EventHandler(this.OpenTheRivaTunerOSDManagerToolStripMenuItem_Click);
            // 
            // LiveChangesTrigger
            // 
            this.LiveChangesTrigger.Index = 3;
            this.LiveChangesTrigger.Text = "Enable live changes for all the settings";
            this.LiveChangesTrigger.Click += new System.EventHandler(this.LiveChangesTrigger_Click);
            // 
            // hotkeys
            // 
            this.hotkeys.Index = 4;
            this.hotkeys.Text = "Enable fast hotkeys in MIDI application";
            this.hotkeys.Click += new System.EventHandler(this.hotkeys_Click);
            // 
            // AutoLoad
            // 
            this.AutoLoad.Index = 5;
            this.AutoLoad.Text = "Reload list automatically after editing it";
            this.AutoLoad.Click += new System.EventHandler(this.AutoLoad_Click);
            // 
            // ShowOutLevel
            // 
            this.ShowOutLevel.Index = 6;
            this.ShowOutLevel.Text = "Show output level meter";
            this.ShowOutLevel.Click += new System.EventHandler(this.ShowOutLevel_Click);
            // 
            // menuItem17
            // 
            this.menuItem17.Index = 7;
            this.menuItem17.Text = "-";
            // 
            // manageFolderFavouritesToolStripMenuItem
            // 
            this.manageFolderFavouritesToolStripMenuItem.Index = 8;
            this.manageFolderFavouritesToolStripMenuItem.Text = "Manage folder favourites";
            this.manageFolderFavouritesToolStripMenuItem.Click += new System.EventHandler(this.manageFolderFavouritesToolStripMenuItem_Click);
            // 
            // changeDirectoryOfTheOutputToWAVModeToolStripMenuItem
            // 
            this.changeDirectoryOfTheOutputToWAVModeToolStripMenuItem.Index = 9;
            this.changeDirectoryOfTheOutputToWAVModeToolStripMenuItem.Text = "Change WAV output directory";
            this.changeDirectoryOfTheOutputToWAVModeToolStripMenuItem.Click += new System.EventHandler(this.changeDirectoryOfTheOutputToWAVModeToolStripMenuItem_Click);
            // 
            // DePrio
            // 
            this.DePrio.Index = 0;
            this.DePrio.Text = "Leave default";
            this.DePrio.Click += new System.EventHandler(this.DePrio_Click);
            // 
            // menuItem34
            // 
            this.menuItem34.Index = 1;
            this.menuItem34.Text = "-";
            // 
            // RTPrio
            // 
            this.RTPrio.Index = 2;
            this.RTPrio.Text = "Real-time";
            this.RTPrio.Click += new System.EventHandler(this.RTPrio_Click);
            // 
            // HiPrio
            // 
            this.HiPrio.Index = 3;
            this.HiPrio.Text = "High";
            this.HiPrio.Click += new System.EventHandler(this.HiPrio_Click);
            // 
            // HNPrio
            // 
            this.HNPrio.Index = 4;
            this.HNPrio.Text = "Higher than normal";
            this.HNPrio.Click += new System.EventHandler(this.HNPrio_Click);
            // 
            // NoPrio
            // 
            this.NoPrio.Index = 5;
            this.NoPrio.Text = "Normal";
            this.NoPrio.Click += new System.EventHandler(this.NoPrio_Click);
            // 
            // LNPrio
            // 
            this.LNPrio.Index = 6;
            this.LNPrio.Text = "Lower than normal";
            this.LNPrio.Click += new System.EventHandler(this.LNPrio_Click);
            // 
            // LoPrio
            // 
            this.LoPrio.Index = 7;
            this.LoPrio.Text = "Low";
            this.LoPrio.Click += new System.EventHandler(this.LoPrio_Click);
            // 
            // menuItem31
            // 
            this.menuItem31.Index = 11;
            this.menuItem31.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.DePrio,
            this.menuItem34,
            this.RTPrio,
            this.HiPrio,
            this.HNPrio,
            this.NoPrio,
            this.LNPrio,
            this.LoPrio});
            this.menuItem31.Text = "Set driver\'s CPU cycles affinity";
            // 
            // menuItem21
            // 
            this.menuItem21.Index = 12;
            this.menuItem21.Text = "-";
            // 
            // SpatialSound
            // 
            this.SpatialSound.Index = 13;
            this.SpatialSound.Text = "Change spatial sound settings";
            this.SpatialSound.Visible = false;
            this.SpatialSound.Click += new System.EventHandler(this.menuItem46_Click);
            // 
            // MaskSynthesizerAsAnother
            // 
            this.MaskSynthesizerAsAnother.Index = 0;
            this.MaskSynthesizerAsAnother.Text = "Mask synthesizer as another";
            this.MaskSynthesizerAsAnother.Click += new System.EventHandler(this.MaskSynthesizerAsAnother_Click);
            // 
            // enableextra8sf
            // 
            this.enableextra8sf.Index = 1;
            this.enableextra8sf.Text = "Enable extra 8 soundfont lists";
            this.enableextra8sf.Click += new System.EventHandler(this.enableextra8sf_Click);
            // 
            // menuItem14
            // 
            this.menuItem14.Index = 14;
            this.menuItem14.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.MaskSynthesizerAsAnother,
            this.enableextra8sf});
            this.menuItem14.Text = "Driver settings";
            // 
            // ImportSettings
            // 
            this.ImportSettings.Index = 0;
            this.ImportSettings.Text = "Import driver settings";
            this.ImportSettings.Click += new System.EventHandler(this.ImportSettings_Click);
            // 
            // ExportSettings
            // 
            this.ExportSettings.Index = 1;
            this.ExportSettings.Text = "Export driver settings";
            this.ExportSettings.Click += new System.EventHandler(this.ExportSettings_Click);
            // 
            // menuItem12
            // 
            this.menuItem12.Index = 15;
            this.menuItem12.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.ImportSettings,
            this.ExportSettings});
            this.menuItem12.Text = "Driver\'s registry settings";
            // 
            // menuItem15
            // 
            this.menuItem15.Index = 16;
            this.menuItem15.Text = "-";
            // 
            // DebugModePls
            // 
            this.DebugModePls.Index = 0;
            this.DebugModePls.Text = "Enable debug log";
            this.DebugModePls.Click += new System.EventHandler(this.DebugModePls_Click);
            // 
            // DebugModeOpenNotepad
            // 
            this.DebugModeOpenNotepad.Index = 1;
            this.DebugModeOpenNotepad.Text = "Open debug folder on Windows Explorer";
            this.DebugModeOpenNotepad.Click += new System.EventHandler(this.DebugModeOpenNotepad_Click);
            // 
            // menuItem28
            // 
            this.menuItem28.Index = 17;
            this.menuItem28.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.DebugModePls,
            this.DebugModeOpenNotepad});
            this.menuItem28.Text = "Debug logging for troubleshooting";
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 1;
            this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem7,
            this.menuItem18,
            this.DisableChime,
            this.LiveChangesTrigger,
            this.hotkeys,
            this.AutoLoad,
            this.ShowOutLevel,
            this.menuItem17,
            this.manageFolderFavouritesToolStripMenuItem,
            this.changeDirectoryOfTheOutputToWAVModeToolStripMenuItem,
            this.assignASoundfontListToASpecificAppToolStripMenuItemToolStripMenuItem,
            this.menuItem31,
            this.menuItem21,
            this.SpatialSound,
            this.menuItem14,
            this.menuItem12,
            this.menuItem15,
            this.menuItem28});
            this.menuItem2.Text = "More settings";
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 0;
            this.menuItem7.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.LoudMaxInstallMenu,
            this.LoudMaxUninstallMenu,
            this.menuItem6,
            this.LMWarningARM64});
            this.menuItem7.Text = "LoudMax, anti-clipping solution";
            // 
            // LoudMaxInstallMenu
            // 
            this.LoudMaxInstallMenu.Index = 0;
            this.LoudMaxInstallMenu.Text = "Install LoudMax, to prevent clipping";
            this.LoudMaxInstallMenu.Click += new System.EventHandler(this.LoudMaxInstallMenu_Click);
            // 
            // LoudMaxUninstallMenu
            // 
            this.LoudMaxUninstallMenu.Index = 1;
            this.LoudMaxUninstallMenu.Text = "Uninstall LoudMax, to restore the original audio";
            this.LoudMaxUninstallMenu.Click += new System.EventHandler(this.LoudMaxUninstallMenu_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 2;
            this.menuItem6.Text = "-";
            // 
            // LMWarningARM64
            // 
            this.LMWarningARM64.Enabled = false;
            this.LMWarningARM64.Index = 3;
            this.LMWarningARM64.Text = "LoudMax isn\'t available for ARM64 processes";
            // 
            // menuItem18
            // 
            this.menuItem18.Index = 1;
            this.menuItem18.Text = "-";
            // 
            // DisableChime
            // 
            this.DisableChime.Index = 2;
            this.DisableChime.Text = "Disable minimum playback/debug mode chime";
            this.DisableChime.Click += new System.EventHandler(this.DisableChime_Click);
            // 
            // AMIDIMapInstallMenu
            // 
            this.AMIDIMapInstallMenu.Index = 0;
            this.AMIDIMapInstallMenu.Text = "Install/Restore";
            this.AMIDIMapInstallMenu.Click += new System.EventHandler(this.AMIDIMapInstallMenu_Click);
            // 
            // AMIDIMapUninstallMenu
            // 
            this.AMIDIMapUninstallMenu.Index = 1;
            this.AMIDIMapUninstallMenu.Text = "Uninstall";
            this.AMIDIMapUninstallMenu.Click += new System.EventHandler(this.AMIDIMapUninstallMenu_Click);
            // 
            // menuItem40
            // 
            this.menuItem40.Index = 1;
            this.menuItem40.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.AMIDIMapInstallMenu,
            this.AMIDIMapUninstallMenu});
            this.menuItem40.Text = "OmniMapper";
            // 
            // SignatureCheck
            // 
            this.SignatureCheck.Index = 4;
            this.SignatureCheck.Text = "Check the driver signature for tampering";
            this.SignatureCheck.Click += new System.EventHandler(this.SignatureCheck_Click);
            // 
            // SelfSignedCertificate
            // 
            this.SelfSignedCertificate.Index = 3;
            this.SelfSignedCertificate.Text = "Install the self-signed certificate by Keppy";
            this.SelfSignedCertificate.Click += new System.EventHandler(this.SelfSignedCertificate_Click);
            // 
            // menuItem39
            // 
            this.menuItem39.Index = 5;
            this.menuItem39.Text = "-";
            // 
            // SetAssociationWithSFs
            // 
            this.SetAssociationWithSFs.Index = 6;
            this.SetAssociationWithSFs.Text = "Set file association with SoundFonts";
            this.SetAssociationWithSFs.Click += new System.EventHandler(this.SetAssociationWithSFs_Click);
            // 
            // menuItem46
            // 
            this.menuItem46.Index = 8;
            this.menuItem46.Text = "-";
            // 
            // DeleteUserData
            // 
            this.DeleteUserData.Index = 9;
            this.DeleteUserData.Text = "Delete driver\'s data from user profile";
            this.DeleteUserData.Click += new System.EventHandler(this.DeleteUserData_Click);
            // 
            // ResetToDefault
            // 
            this.ResetToDefault.Index = 10;
            this.ResetToDefault.Text = "Reinstall the driver from scratch";
            this.ResetToDefault.Click += new System.EventHandler(this.ResetToDefault_Click);
            // 
            // menuItem25
            // 
            this.menuItem25.Index = 2;
            this.menuItem25.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.WMMPatches,
            this.menuItem40,
            this.menuItem60,
            this.SelfSignedCertificate,
            this.SignatureCheck,
            this.menuItem39,
            this.SetAssociationWithSFs,
            this.MIDIInOutTest,
            this.menuItem46,
            this.DeleteUserData,
            this.ResetToDefault});
            this.menuItem25.Text = "Tools";
            // 
            // WMMPatches
            // 
            this.WMMPatches.Index = 0;
            this.WMMPatches.Text = "Windows Multimedia Wrapper patch";
            this.WMMPatches.Click += new System.EventHandler(this.WMMPatches_Click);
            // 
            // menuItem60
            // 
            this.menuItem60.Index = 2;
            this.menuItem60.Text = "-";
            // 
            // MIDIInOutTest
            // 
            this.MIDIInOutTest.Index = 7;
            this.MIDIInOutTest.Text = "Test MIDI input and output";
            this.MIDIInOutTest.Click += new System.EventHandler(this.MIDIInOutTest_Click);
            // 
            // informationAboutTheDriverToolStripMenuItem
            // 
            this.informationAboutTheDriverToolStripMenuItem.Index = 0;
            this.informationAboutTheDriverToolStripMenuItem.Shortcut = System.Windows.Forms.Shortcut.F1;
            this.informationAboutTheDriverToolStripMenuItem.Text = "Information about the driver";
            this.informationAboutTheDriverToolStripMenuItem.Click += new System.EventHandler(this.informationAboutTheDriverToolStripMenuItem_Click);
            // 
            // openUpdaterToolStripMenuItem
            // 
            this.openUpdaterToolStripMenuItem.Index = 4;
            this.openUpdaterToolStripMenuItem.Shortcut = System.Windows.Forms.Shortcut.F2;
            this.openUpdaterToolStripMenuItem.Text = "Check for updates";
            this.openUpdaterToolStripMenuItem.Click += new System.EventHandler(this.openUpdaterToolStripMenuItem_Click);
            // 
            // menuItem49
            // 
            this.menuItem49.Index = 3;
            this.menuItem49.Text = "-";
            // 
            // donateToSupportUsToolStripMenuItem
            // 
            this.donateToSupportUsToolStripMenuItem.Index = 12;
            this.donateToSupportUsToolStripMenuItem.Text = "Support me with a PayPal donation";
            this.donateToSupportUsToolStripMenuItem.Click += new System.EventHandler(this.donateToSupportUsToolStripMenuItem_Click);
            // 
            // menuItem29
            // 
            this.menuItem29.Index = 7;
            this.menuItem29.Text = "-";
            // 
            // SeeChangelog
            // 
            this.SeeChangelog.Index = 5;
            this.SeeChangelog.Text = "Changelog of this driver release";
            this.SeeChangelog.Click += new System.EventHandler(this.SeeChangelog_Click);
            // 
            // SeeLatestChangelog
            // 
            this.SeeLatestChangelog.Index = 6;
            this.SeeLatestChangelog.Text = "Changelog of the latest driver release";
            this.SeeLatestChangelog.Click += new System.EventHandler(this.SeeLatestChangelog_Click);
            // 
            // downloadTheSourceCodeToolStripMenuItem
            // 
            this.downloadTheSourceCodeToolStripMenuItem.Index = 9;
            this.downloadTheSourceCodeToolStripMenuItem.Text = "Download the source code";
            this.downloadTheSourceCodeToolStripMenuItem.Click += new System.EventHandler(this.downloadTheSourceCodeToolStripMenuItem_Click);
            // 
            // menuItem33
            // 
            this.menuItem33.Index = 10;
            this.menuItem33.Text = "-";
            // 
            // HAPLink
            // 
            this.HAPLink.Index = 0;
            this.HAPLink.Text = "HtmlAgilityPack by Simon Mourier";
            this.HAPLink.Click += new System.EventHandler(this.HAPLink_Click);
            // 
            // BASSLink
            // 
            this.BASSLink.Index = 1;
            this.BASSLink.Text = "BASS libraries by Un4seen Developments";
            this.BASSLink.Click += new System.EventHandler(this.BASSLink_Click);
            // 
            // BASSNetLink
            // 
            this.BASSNetLink.Index = 2;
            this.BASSNetLink.Text = "BASS.NET by radio42";
            this.BASSNetLink.Click += new System.EventHandler(this.BASSNetLink_Click);
            // 
            // FodyCredit
            // 
            this.FodyCredit.Index = 3;
            this.FodyCredit.Text = "Fody by Simon Cropp";
            this.FodyCredit.Click += new System.EventHandler(this.FodyCredit_Click);
            // 
            // menuItem45
            // 
            this.menuItem45.Index = 11;
            this.menuItem45.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.HAPLink,
            this.BASSLink,
            this.BASSNetLink,
            this.FodyCredit,
            this.OctokitDev});
            this.menuItem45.Text = "Credits";
            // 
            // OctokitDev
            // 
            this.OctokitDev.Index = 4;
            this.OctokitDev.Text = "Octokit.net by GitHub Inc.";
            this.OctokitDev.Click += new System.EventHandler(this.OctokitDev_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 3;
            this.menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.informationAboutTheDriverToolStripMenuItem,
            this.BugReport,
            this.KSSD,
            this.menuItem49,
            this.openUpdaterToolStripMenuItem,
            this.SeeChangelog,
            this.SeeLatestChangelog,
            this.menuItem29,
            this.KDMAPIDoc,
            this.downloadTheSourceCodeToolStripMenuItem,
            this.menuItem33,
            this.menuItem45,
            this.donateToSupportUsToolStripMenuItem});
            this.menuItem3.Text = "?";
            // 
            // BugReport
            // 
            this.BugReport.Index = 1;
            this.BugReport.Text = "Report a bug on GitHub";
            this.BugReport.Click += new System.EventHandler(this.BugReport_Click);
            // 
            // KSSD
            // 
            this.KSSD.Index = 2;
            this.KSSD.Text = "Join the official Keppy\'s Software server on Discord";
            this.KSSD.Click += new System.EventHandler(this.KSSD_Click);
            // 
            // KDMAPIDoc
            // 
            this.KDMAPIDoc.Index = 8;
            this.KDMAPIDoc.Text = "Keppy\'s Direct MIDI API documentation";
            this.KDMAPIDoc.Click += new System.EventHandler(this.KDMAPIDoc_Click);
            // 
            // SynthMenu
            // 
            this.SynthMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem2,
            this.menuItem25,
            this.menuItem3});
            this.SynthMenu.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // VolTrackBarMenu
            // 
            this.VolTrackBarMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.FineTuneKnobIt,
            this.menuItem57,
            this.VolumeBoost});
            // 
            // FineTuneKnobIt
            // 
            this.FineTuneKnobIt.Index = 0;
            this.FineTuneKnobIt.Text = "Fine tune the volume knob";
            this.FineTuneKnobIt.Click += new System.EventHandler(this.FineTuneKnobIt_Click);
            // 
            // menuItem57
            // 
            this.menuItem57.Index = 1;
            this.menuItem57.Text = "-";
            // 
            // VolumeBoost
            // 
            this.VolumeBoost.Index = 2;
            this.VolumeBoost.Text = "Enable volume boost";
            this.VolumeBoost.Click += new System.EventHandler(this.VolumeBoost_Click);
            // 
            // VolumeCheck
            // 
            this.VolumeCheck.Tick += new System.EventHandler(this.VolumeCheck_Tick);
            // 
            // OmniMIDIConfiguratorMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(647, 442);
            this.Controls.Add(this.StatusStrip);
            this.Controls.Add(this.TabsForTheControls);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.SynthMenu;
            this.MinimumSize = new System.Drawing.Size(643, 467);
            this.Name = "OmniMIDIConfiguratorMain";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OmniMIDI ~ Configurator";
            this.Load += new System.EventHandler(this.OmniMIDIConfiguratorMain_Load);
            this.Settings.ResumeLayout(false);
            this.Settings.PerformLayout();
            this.VolPanel.ResumeLayout(false);
            this.MixerBox.ResumeLayout(false);
            this.MixerBox.PerformLayout();
            this.EnginesBox.ResumeLayout(false);
            this.EnginesBox.PerformLayout();
            this.AdditionalSettingsBox.ResumeLayout(false);
            this.OutputSettingsBox.ResumeLayout(false);
            this.OutputSettingsBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bufsize)).EndInit();
            this.SynthSettingsBox.ResumeLayout(false);
            this.SynthSettingsBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MaxCPU)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PolyphonyLimit)).EndInit();
            this.SoundFontTab.ResumeLayout(false);
            this.SoundFontTab.PerformLayout();
            this.TabsForTheControls.ResumeLayout(false);
            this.StatusStrip.ResumeLayout(false);
            this.StatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenu RightClickMenu;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem menuItem5;
        private System.Windows.Forms.MenuItem menuItem8;
        private System.Windows.Forms.MenuItem menuItem10;
        public System.Windows.Forms.OpenFileDialog SoundfontImport;
        public System.Windows.Forms.OpenFileDialog ExternalListImport;
        public System.Windows.Forms.SaveFileDialog ExternalListExport;
        private System.ComponentModel.BackgroundWorker ThemeCheck;
        public System.Windows.Forms.SaveFileDialog ExportSettingsDialog;
        public System.Windows.Forms.OpenFileDialog ImportSettingsDialog;
        internal System.Windows.Forms.GroupBox OutputSettingsBox;
        public System.Windows.Forms.CheckBox SysResetIgnore;
        public System.Windows.Forms.NumericUpDown bufsize;
        private System.Windows.Forms.GroupBox SynthSettingsBox;
        public System.Windows.Forms.NumericUpDown MaxCPU;
        public System.Windows.Forms.ComboBox Frequency;
        internal System.Windows.Forms.Label RenderingTimeLabel;
        internal System.Windows.Forms.Label VoiceLimitLabel;
        public System.Windows.Forms.NumericUpDown PolyphonyLimit;
        public System.Windows.Forms.CheckBox EnableSFX;
        public System.Windows.Forms.CheckBox Preload;
        public System.Windows.Forms.CheckBox NoteOffCheck;
        private System.Windows.Forms.TabPage SoundFontTab;
        private System.Windows.Forms.Button EL;
        private System.Windows.Forms.Button LoadToApp;
        private System.Windows.Forms.Button IEL;
        public System.Windows.Forms.CheckBox BankPresetOverride;
        public System.Windows.Forms.ComboBox SelectedListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button DisableSF;
        private System.Windows.Forms.Button EnableSF;
        private System.Windows.Forms.Label ListOverride;
        private System.Windows.Forms.Button CLi;
        private System.Windows.Forms.Button MvD;
        private System.Windows.Forms.Button MvU;
        private System.Windows.Forms.Button RmvSF;
        private System.Windows.Forms.Button AddSF;
        private System.Windows.Forms.TabControl TabsForTheControls;
        private System.Windows.Forms.MenuItem OpenSFDefaultApp;
        private System.Windows.Forms.Button MEPSButton;
        private System.Windows.Forms.Button AASButton;
        public MenuButton SettingsPresetsBtn;
        private System.Windows.Forms.Button resetToDefaultToolStripMenuItem;
        private System.Windows.Forms.Button applySettingsToolStripMenuItem;
        private System.Windows.Forms.ContextMenu SettingsPresets;
        private System.Windows.Forms.MenuItem lowLatencyPresetToolStripMenuItem;
        private System.Windows.Forms.MenuItem SBLowLatToolStripMenuItem;
        private System.Windows.Forms.MenuItem blackMIDIsPresetToolStripMenuItem;
        private System.Windows.Forms.MenuItem chiptunesRetrogamingToolStripMenuItem;
        private System.Windows.Forms.MenuItem keppysSteinwayPianoRealismToolStripMenuItem;
        private System.Windows.Forms.MenuItem OpenSFMainDirectory;
        public ListViewEx Lis;
        private System.Windows.Forms.ColumnHeader SoundFont;
        private System.Windows.Forms.ColumnHeader SFFormat;
        private System.Windows.Forms.ColumnHeader SFSize;
        private System.Windows.Forms.MenuItem menuItem35;
        private System.Windows.Forms.MenuItem menuItem38;
        private System.Windows.Forms.Timer SavedLabel;
        public System.Windows.Forms.ComboBox AudioEngBox;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label DrvHzLabel;
        public System.Windows.Forms.Label BufferText;
        private System.Windows.Forms.MenuItem ProLowLatToolStripMenuItem;
        private System.Windows.Forms.ImageList TabImgs;
        private System.Windows.Forms.StatusStrip StatusStrip;
        public System.Windows.Forms.ToolStripStatusLabel Status;
        public System.Windows.Forms.ToolStripStatusLabel StatusDoneOr;
        private System.Windows.Forms.ToolStripStatusLabel VersionLabel;
        public System.Windows.Forms.Label VolSimView;
        private System.Windows.Forms.GroupBox EnginesBox;
        private System.Windows.Forms.GroupBox AdditionalSettingsBox;
        public System.Windows.Forms.Panel RV22S;
        public System.Windows.Forms.Panel LV22S;
        public System.Windows.Forms.Panel RV18S;
        public System.Windows.Forms.Panel LV18S;
        public System.Windows.Forms.Panel RV21S;
        public System.Windows.Forms.Panel LV21S;
        public System.Windows.Forms.Panel RV13S;
        public System.Windows.Forms.Panel LV13S;
        public System.Windows.Forms.Panel RV20S;
        public System.Windows.Forms.Panel LV20S;
        public System.Windows.Forms.Panel RV19S;
        public System.Windows.Forms.Panel LV19S;
        public System.Windows.Forms.Panel RV17S;
        public System.Windows.Forms.Panel LV17S;
        public System.Windows.Forms.Panel RV8S;
        public System.Windows.Forms.Panel LV8S;
        public System.Windows.Forms.Panel RV16S;
        public System.Windows.Forms.Panel LV16S;
        public System.Windows.Forms.Panel RV12S;
        public System.Windows.Forms.Panel LV12S;
        public System.Windows.Forms.Panel RV15S;
        public System.Windows.Forms.Panel LV15S;
        public System.Windows.Forms.Panel RV14S;
        public System.Windows.Forms.Panel LV14S;
        public System.Windows.Forms.Panel RV7S;
        public System.Windows.Forms.Panel LV7S;
        public System.Windows.Forms.Panel RV11S;
        public System.Windows.Forms.Panel LV11S;
        public System.Windows.Forms.Panel RV6S;
        public System.Windows.Forms.Panel RV10S;
        public System.Windows.Forms.Panel LV6S;
        public System.Windows.Forms.Panel RV5S;
        public System.Windows.Forms.Panel LV10S;
        public System.Windows.Forms.Panel RV9S;
        public System.Windows.Forms.Panel LV5S;
        public System.Windows.Forms.Panel RV4S;
        public System.Windows.Forms.Panel LV9S;
        public System.Windows.Forms.Panel RV3S;
        public System.Windows.Forms.Panel LV4S;
        public System.Windows.Forms.Panel RV2S;
        public System.Windows.Forms.Panel LV3S;
        public System.Windows.Forms.Panel RV1S;
        public System.Windows.Forms.Panel LV2S;
        public System.Windows.Forms.Panel LV1S;
        private System.Windows.Forms.Label LLab;
        private System.Windows.Forms.Label RLab;
        public System.Windows.Forms.GroupBox MixerBox;
        private System.Windows.Forms.ToolTip ButtonsDesc;
        public System.Windows.Forms.Label SignalLabelS;
        public System.Windows.Forms.Panel LEDS;
        private System.Windows.Forms.ContextMenu MixerContext;
        private System.Windows.Forms.MenuItem OpenMixer;
        private System.Windows.Forms.MenuItem menuItem19;
        private System.Windows.Forms.MenuItem DisableOLM;
        public System.Windows.Forms.Label VolLevelS;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel UpdateStatus;
        private System.Windows.Forms.Label Separator;
        public System.Windows.Forms.TabPage Settings;
        private System.Windows.Forms.MenuItem MSGSWSEmu;
        private System.Windows.Forms.MenuItem menuItem50;
        private System.Windows.Forms.MenuItem menuItem41;
        private System.Windows.Forms.MenuItem menuItem36;
        private System.Windows.Forms.Panel VolPanel;
        private System.ComponentModel.BackgroundWorker CheckUpdates;
        private System.Windows.Forms.Button ExportPres;
        private System.Windows.Forms.Label SeparatorPres;
        private System.Windows.Forms.Button ImportPres;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.SaveFileDialog ExportPresetDialog;
        public System.Windows.Forms.OpenFileDialog ImportPresetDialog;
        private System.Windows.Forms.MenuItem menuItem44;
        private System.Windows.Forms.LinkLabelEx WhatIsXAudio;
        private System.Windows.Forms.LinkLabelEx WhatIsOutput;
        private System.Windows.Forms.Button ChangeMask;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.MenuItem openDebugWindowToolStripMenuItem;
        private System.Windows.Forms.MenuItem openTheMixerToolStripMenuItem;
        private System.Windows.Forms.MenuItem openTheBlacklistManagerToolStripMenuItem;
        private System.Windows.Forms.MenuItem assignASoundfontListToASpecificAppToolStripMenuItemToolStripMenuItem;
        private System.Windows.Forms.MenuItem menuItem9;
        public System.Windows.Forms.MenuItem OmniMapperCpl;
        private System.Windows.Forms.MenuItem menuItem13;
        private System.Windows.Forms.MenuItem exitToolStripMenuItem;
        private System.Windows.Forms.MenuItem menuItem1;
        public System.Windows.Forms.MenuItem LiveChangesTrigger;
        public System.Windows.Forms.MenuItem hotkeys;
        public System.Windows.Forms.MenuItem AutoLoad;
        public System.Windows.Forms.MenuItem ShowOutLevel;
        private System.Windows.Forms.MenuItem menuItem17;
        private System.Windows.Forms.MenuItem manageFolderFavouritesToolStripMenuItem;
        public System.Windows.Forms.MenuItem changeDirectoryOfTheOutputToWAVModeToolStripMenuItem;
        public System.Windows.Forms.MenuItem DePrio;
        private System.Windows.Forms.MenuItem menuItem34;
        public System.Windows.Forms.MenuItem RTPrio;
        public System.Windows.Forms.MenuItem HiPrio;
        public System.Windows.Forms.MenuItem HNPrio;
        public System.Windows.Forms.MenuItem NoPrio;
        public System.Windows.Forms.MenuItem LNPrio;
        public System.Windows.Forms.MenuItem LoPrio;
        private System.Windows.Forms.MenuItem menuItem31;
        private System.Windows.Forms.MenuItem menuItem21;
        public System.Windows.Forms.MenuItem SpatialSound;
        public System.Windows.Forms.MenuItem MaskSynthesizerAsAnother;
        public System.Windows.Forms.MenuItem enableextra8sf;
        private System.Windows.Forms.MenuItem menuItem14;
        private System.Windows.Forms.MenuItem ImportSettings;
        private System.Windows.Forms.MenuItem ExportSettings;
        private System.Windows.Forms.MenuItem menuItem12;
        private System.Windows.Forms.MenuItem menuItem15;
        public System.Windows.Forms.MenuItem DebugModePls;
        private System.Windows.Forms.MenuItem DebugModeOpenNotepad;
        private System.Windows.Forms.MenuItem menuItem28;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem AMIDIMapInstallMenu;
        private System.Windows.Forms.MenuItem AMIDIMapUninstallMenu;
        private System.Windows.Forms.MenuItem menuItem40;
        private System.Windows.Forms.MenuItem SignatureCheck;
        private System.Windows.Forms.MenuItem SelfSignedCertificate;
        private System.Windows.Forms.MenuItem menuItem39;
        private System.Windows.Forms.MenuItem SetAssociationWithSFs;
        private System.Windows.Forms.MenuItem menuItem46;
        private System.Windows.Forms.MenuItem DeleteUserData;
        private System.Windows.Forms.MenuItem ResetToDefault;
        private System.Windows.Forms.MenuItem menuItem25;
        private System.Windows.Forms.MenuItem informationAboutTheDriverToolStripMenuItem;
        private System.Windows.Forms.MenuItem openUpdaterToolStripMenuItem;
        private System.Windows.Forms.MenuItem menuItem49;
        private System.Windows.Forms.MenuItem donateToSupportUsToolStripMenuItem;
        private System.Windows.Forms.MenuItem menuItem29;
        private System.Windows.Forms.MenuItem SeeChangelog;
        private System.Windows.Forms.MenuItem SeeLatestChangelog;
        private System.Windows.Forms.MenuItem downloadTheSourceCodeToolStripMenuItem;
        private System.Windows.Forms.MenuItem menuItem33;
        private System.Windows.Forms.MenuItem HAPLink;
        private System.Windows.Forms.MenuItem BASSLink;
        private System.Windows.Forms.MenuItem BASSNetLink;
        private System.Windows.Forms.MenuItem FodyCredit;
        private System.Windows.Forms.MenuItem menuItem45;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MainMenu SynthMenu;
        private System.Windows.Forms.MenuItem menuItem54;
        private System.Windows.Forms.MenuItem menuItem55;
        public System.Windows.Forms.ComboBox SincConv;
        internal System.Windows.Forms.Label SincConvLab;
        public System.Windows.Forms.CheckBox SincInter;
        public System.Windows.Forms.Label VolLabel;
        private System.Windows.Forms.ContextMenu VolTrackBarMenu;
        private System.Windows.Forms.MenuItem FineTuneKnobIt;
        private System.Windows.Forms.MenuItem menuItem57;
        public System.Windows.Forms.MenuItem VolumeBoost;
        private System.Windows.Forms.MenuItem menuItem37;
        public System.Windows.Forms.ToolTip Requirements;
        private System.Windows.Forms.MenuItem KDMAPIDoc;
        private System.Windows.Forms.MenuItem menuItem60;
        private System.Windows.Forms.MenuItem WMMPatches;
        private System.Windows.Forms.MenuItem MIDIInOutTest;
        public KnobControl.KnobControl VolTrackBar;
        private System.Windows.Forms.MenuItem BugReport;
        private System.Windows.Forms.MenuItem OctokitDev;
        private System.Windows.Forms.MenuItem menuItem16;
        private System.Windows.Forms.ColumnHeader SrcPres;
        private System.Windows.Forms.ColumnHeader SrcBank;
        private System.Windows.Forms.ColumnHeader DesPres;
        private System.Windows.Forms.ColumnHeader DesBank;
        private System.Windows.Forms.ColumnHeader XGDrums;
        private System.Windows.Forms.MenuItem EditSFSettings;
        private System.Windows.Forms.MenuItem menuItem11;
        public System.Windows.Forms.Timer VolumeCheck;
        private System.Windows.Forms.MenuItem KSSD;
        private System.Windows.Forms.MenuItem menuItem7;
        private System.Windows.Forms.MenuItem LoudMaxInstallMenu;
        private System.Windows.Forms.MenuItem LoudMaxUninstallMenu;
        private System.Windows.Forms.MenuItem menuItem18;
        private System.Windows.Forms.MenuItem menuItem6;
        private System.Windows.Forms.MenuItem LMWarningARM64;
        public System.Windows.Forms.MenuItem DisableChime;
        private System.Windows.Forms.Button SFlg;
        private System.Windows.Forms.MenuItem openTheRivaTunerOSDManagerToolStripMenuItem;
    }
}

