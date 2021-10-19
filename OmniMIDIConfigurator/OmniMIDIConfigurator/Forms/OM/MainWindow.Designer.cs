namespace OmniMIDIConfigurator
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.MWTab = new System.Windows.Forms.TabControl();
            this.ListsEdt = new System.Windows.Forms.TabPage();
            this.SFLEPanel = new OmniMIDIConfigurator.BufferedPanel();
            this.Set = new System.Windows.Forms.TabPage();
            this.ExportPres = new System.Windows.Forms.Button();
            this.ImportPres = new System.Windows.Forms.Button();
            this.SeparatorPres = new System.Windows.Forms.Label();
            this.QICombo = new System.Windows.Forms.ComboBox();
            this.QILabel = new System.Windows.Forms.Label();
            this.RestoreDefault = new System.Windows.Forms.Button();
            this.ApplySettings = new System.Windows.Forms.Button();
            this.SETPanel = new OmniMIDIConfigurator.BufferedPanel();
            this.MWSStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.VersionLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.OMMenu = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem11 = new System.Windows.Forms.MenuItem();
            this.OpenDW = new System.Windows.Forms.MenuItem();
            this.OpenM = new System.Windows.Forms.MenuItem();
            this.OpenBM = new System.Windows.Forms.MenuItem();
            this.AssignListToApp = new System.Windows.Forms.MenuItem();
            this.menuItem21 = new System.Windows.Forms.MenuItem();
            this.CloseConfigurator = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.WMWPatch = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItem16 = new System.Windows.Forms.MenuItem();
            this.OMDRegister = new System.Windows.Forms.MenuItem();
            this.OMDUnregister = new System.Windows.Forms.MenuItem();
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.RegTroubleshooter = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.InstallLM = new System.Windows.Forms.MenuItem();
            this.UninstallLM = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.menuItem20 = new System.Windows.Forms.MenuItem();
            this.menuItem10 = new System.Windows.Forms.MenuItem();
            this.OMAPInstall = new System.Windows.Forms.MenuItem();
            this.OMAPUninstall = new System.Windows.Forms.MenuItem();
            this.OMAPCpl = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.MIDIInOutTest = new System.Windows.Forms.MenuItem();
            this.menuItem15 = new System.Windows.Forms.MenuItem();
            this.ReinstallDriver = new System.Windows.Forms.MenuItem();
            this.DeleteUserData = new System.Windows.Forms.MenuItem();
            this.menuItem17 = new System.Windows.Forms.MenuItem();
            this.RestoreSFListEdWidth = new System.Windows.Forms.MenuItem();
            this.RestoreConfSettings = new System.Windows.Forms.MenuItem();
            this.DWCF = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.DriverInfo = new System.Windows.Forms.MenuItem();
            this.CUBBtn = new System.Windows.Forms.MenuItem();
            this.CFUBtn = new System.Windows.Forms.MenuItem();
            this.menuItem14 = new System.Windows.Forms.MenuItem();
            this.ChangelogCurrent = new System.Windows.Forms.MenuItem();
            this.ChangelogLatest = new System.Windows.Forms.MenuItem();
            this.menuItem19 = new System.Windows.Forms.MenuItem();
            this.BugReport = new System.Windows.Forms.MenuItem();
            this.DLDriverSrc = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.KDMAPIDoc = new System.Windows.Forms.MenuItem();
            this.CheckUpdates = new System.ComponentModel.BackgroundWorker();
            this.UpdateNow = new System.Windows.Forms.Timer(this.components);
            this.MWTab.SuspendLayout();
            this.ListsEdt.SuspendLayout();
            this.Set.SuspendLayout();
            this.MWSStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // MWTab
            // 
            this.MWTab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MWTab.Controls.Add(this.ListsEdt);
            this.MWTab.Controls.Add(this.Set);
            this.MWTab.Location = new System.Drawing.Point(0, 0);
            this.MWTab.Name = "MWTab";
            this.MWTab.SelectedIndex = 0;
            this.MWTab.Size = new System.Drawing.Size(834, 534);
            this.MWTab.TabIndex = 0;
            this.MWTab.SelectedIndexChanged += new System.EventHandler(this.MWTab_SelectedIndexChanged);
            // 
            // ListsEdt
            // 
            this.ListsEdt.Controls.Add(this.SFLEPanel);
            this.ListsEdt.Location = new System.Drawing.Point(4, 24);
            this.ListsEdt.Name = "ListsEdt";
            this.ListsEdt.Padding = new System.Windows.Forms.Padding(3);
            this.ListsEdt.Size = new System.Drawing.Size(826, 506);
            this.ListsEdt.TabIndex = 0;
            this.ListsEdt.Text = "Lists editor";
            this.ListsEdt.UseVisualStyleBackColor = true;
            // 
            // SFLEPanel
            // 
            this.SFLEPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SFLEPanel.Location = new System.Drawing.Point(0, 0);
            this.SFLEPanel.Name = "SFLEPanel";
            this.SFLEPanel.Size = new System.Drawing.Size(825, 502);
            this.SFLEPanel.TabIndex = 0;
            // 
            // Set
            // 
            this.Set.Controls.Add(this.ExportPres);
            this.Set.Controls.Add(this.ImportPres);
            this.Set.Controls.Add(this.SeparatorPres);
            this.Set.Controls.Add(this.QICombo);
            this.Set.Controls.Add(this.QILabel);
            this.Set.Controls.Add(this.RestoreDefault);
            this.Set.Controls.Add(this.ApplySettings);
            this.Set.Controls.Add(this.SETPanel);
            this.Set.Location = new System.Drawing.Point(4, 24);
            this.Set.Name = "Set";
            this.Set.Padding = new System.Windows.Forms.Padding(3);
            this.Set.Size = new System.Drawing.Size(826, 505);
            this.Set.TabIndex = 1;
            this.Set.Text = "Settings";
            this.Set.UseVisualStyleBackColor = true;
            // 
            // ExportPres
            // 
            this.ExportPres.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ExportPres.Location = new System.Drawing.Point(313, 473);
            this.ExportPres.Name = "ExportPres";
            this.ExportPres.Size = new System.Drawing.Size(52, 27);
            this.ExportPres.TabIndex = 6;
            this.ExportPres.Text = "Export";
            this.ExportPres.UseVisualStyleBackColor = true;
            this.ExportPres.Click += new System.EventHandler(this.ExportPres_Click);
            // 
            // ImportPres
            // 
            this.ImportPres.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ImportPres.Location = new System.Drawing.Point(258, 473);
            this.ImportPres.Name = "ImportPres";
            this.ImportPres.Size = new System.Drawing.Size(52, 27);
            this.ImportPres.TabIndex = 5;
            this.ImportPres.Text = "Import";
            this.ImportPres.UseVisualStyleBackColor = true;
            this.ImportPres.Click += new System.EventHandler(this.ImportPres_Click);
            // 
            // SeparatorPres
            // 
            this.SeparatorPres.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SeparatorPres.Enabled = false;
            this.SeparatorPres.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SeparatorPres.Location = new System.Drawing.Point(241, 472);
            this.SeparatorPres.Name = "SeparatorPres";
            this.SeparatorPres.Size = new System.Drawing.Size(12, 27);
            this.SeparatorPres.TabIndex = 50;
            this.SeparatorPres.Text = "|";
            // 
            // QICombo
            // 
            this.QICombo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.QICombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.QICombo.FormattingEnabled = true;
            this.QICombo.Items.AddRange(new object[] {
            "Audio engine settings",
            "Synthesizer settings",
            "Debug & legacy set."});
            this.QICombo.Location = new System.Drawing.Point(89, 474);
            this.QICombo.Name = "QICombo";
            this.QICombo.Size = new System.Drawing.Size(151, 23);
            this.QICombo.TabIndex = 4;
            this.QICombo.SelectedIndexChanged += new System.EventHandler(this.QICombo_SelectedIndexChanged);
            // 
            // QILabel
            // 
            this.QILabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.QILabel.AutoSize = true;
            this.QILabel.Location = new System.Drawing.Point(7, 479);
            this.QILabel.Name = "QILabel";
            this.QILabel.Size = new System.Drawing.Size(73, 15);
            this.QILabel.TabIndex = 4;
            this.QILabel.Text = "Quick index:";
            // 
            // RestoreDefault
            // 
            this.RestoreDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RestoreDefault.Location = new System.Drawing.Point(615, 473);
            this.RestoreDefault.Name = "RestoreDefault";
            this.RestoreDefault.Size = new System.Drawing.Size(111, 27);
            this.RestoreDefault.TabIndex = 3;
            this.RestoreDefault.Text = "Restore default";
            this.RestoreDefault.UseVisualStyleBackColor = true;
            // 
            // ApplySettings
            // 
            this.ApplySettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ApplySettings.Location = new System.Drawing.Point(733, 473);
            this.ApplySettings.Name = "ApplySettings";
            this.ApplySettings.Size = new System.Drawing.Size(87, 27);
            this.ApplySettings.TabIndex = 2;
            this.ApplySettings.Text = "Apply";
            this.ApplySettings.UseVisualStyleBackColor = true;
            // 
            // SETPanel
            // 
            this.SETPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SETPanel.Location = new System.Drawing.Point(0, 0);
            this.SETPanel.Name = "SETPanel";
            this.SETPanel.Size = new System.Drawing.Size(825, 468);
            this.SETPanel.TabIndex = 1;
            // 
            // MWSStrip
            // 
            this.MWSStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.VersionLabel});
            this.MWSStrip.Location = new System.Drawing.Point(0, 533);
            this.MWSStrip.Name = "MWSStrip";
            this.MWSStrip.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.MWSStrip.Size = new System.Drawing.Size(832, 22);
            this.MWSStrip.SizingGrip = false;
            this.MWSStrip.TabIndex = 1;
            this.MWSStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(687, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.Text = " ";
            // 
            // VersionLabel
            // 
            this.VersionLabel.Image = global::OmniMIDIConfigurator.Properties.Resources.ClearIcon;
            this.VersionLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(97, 17);
            this.VersionLabel.Text = "Version 0.0.0.0";
            this.VersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // OMMenu
            // 
            this.OMMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem11,
            this.menuItem3,
            this.menuItem1,
            this.menuItem2});
            // 
            // menuItem11
            // 
            this.menuItem11.Index = 0;
            this.menuItem11.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.OpenDW,
            this.OpenM,
            this.OpenBM,
            this.AssignListToApp,
            this.menuItem21,
            this.CloseConfigurator});
            this.menuItem11.Text = "Functions";
            // 
            // OpenDW
            // 
            this.OpenDW.Index = 0;
            this.OpenDW.Text = "Open debug window";
            this.OpenDW.Click += new System.EventHandler(this.OpenDW_Click);
            // 
            // OpenM
            // 
            this.OpenM.Index = 1;
            this.OpenM.Text = "Open mixer";
            this.OpenM.Click += new System.EventHandler(this.OpenM_Click);
            // 
            // OpenBM
            // 
            this.OpenBM.Index = 2;
            this.OpenBM.Text = "Open blacklist manager";
            this.OpenBM.Click += new System.EventHandler(this.OpenBM_Click);
            // 
            // AssignListToApp
            // 
            this.AssignListToApp.Index = 3;
            this.AssignListToApp.Text = "Assign a soundfont list to a specific app";
            this.AssignListToApp.Click += new System.EventHandler(this.AssignListToApp_Click);
            // 
            // menuItem21
            // 
            this.menuItem21.Index = 4;
            this.menuItem21.Text = "-";
            // 
            // CloseConfigurator
            // 
            this.CloseConfigurator.Index = 5;
            this.CloseConfigurator.Text = "Exit";
            this.CloseConfigurator.Click += new System.EventHandler(this.CloseConfigurator_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 1;
            this.menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.WMWPatch,
            this.menuItem5,
            this.menuItem16,
            this.menuItem4,
            this.menuItem20,
            this.menuItem10,
            this.OMAPCpl});
            this.menuItem3.Text = "Extensions";
            // 
            // WMWPatch
            // 
            this.WMWPatch.Index = 0;
            this.WMWPatch.Text = "Windows Multimedia Wrapper";
            this.WMWPatch.Click += new System.EventHandler(this.WMWPatch_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 1;
            this.menuItem5.Text = "-";
            // 
            // menuItem16
            // 
            this.menuItem16.Index = 2;
            this.menuItem16.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.OMDRegister,
            this.OMDUnregister,
            this.menuItem9,
            this.RegTroubleshooter});
            this.menuItem16.Text = "OmniMIDI register tool";
            // 
            // OMDRegister
            // 
            this.OMDRegister.Index = 0;
            this.OMDRegister.Text = "Register the driver";
            this.OMDRegister.Click += new System.EventHandler(this.OMDRegister_Click);
            // 
            // OMDUnregister
            // 
            this.OMDUnregister.Index = 1;
            this.OMDUnregister.Text = "Unregister the driver";
            this.OMDUnregister.Click += new System.EventHandler(this.OMDUnregister_Click);
            // 
            // menuItem9
            // 
            this.menuItem9.Index = 2;
            this.menuItem9.Text = "-";
            // 
            // RegTroubleshooter
            // 
            this.RegTroubleshooter.Index = 3;
            this.RegTroubleshooter.Text = "Registration troubleshooter";
            this.RegTroubleshooter.Click += new System.EventHandler(this.RegTroubleshooter_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 3;
            this.menuItem4.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.InstallLM,
            this.UninstallLM,
            this.menuItem7,
            this.menuItem8});
            this.menuItem4.Text = "LoudMax, anti-clipping solution";
            // 
            // InstallLM
            // 
            this.InstallLM.Index = 0;
            this.InstallLM.Text = "Install LoudMax, to prevent clipping";
            this.InstallLM.Click += new System.EventHandler(this.InstallLM_Click);
            // 
            // UninstallLM
            // 
            this.UninstallLM.Index = 1;
            this.UninstallLM.Text = "Uninstall LoudMax, to restore the original audio";
            this.UninstallLM.Click += new System.EventHandler(this.UninstallLM_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 2;
            this.menuItem7.Text = "-";
            // 
            // menuItem8
            // 
            this.menuItem8.Enabled = false;
            this.menuItem8.Index = 3;
            this.menuItem8.Text = "LoudMax is not available on ARM64 installs";
            // 
            // menuItem20
            // 
            this.menuItem20.Index = 4;
            this.menuItem20.Text = "-";
            // 
            // menuItem10
            // 
            this.menuItem10.Index = 5;
            this.menuItem10.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.OMAPInstall,
            this.OMAPUninstall});
            this.menuItem10.Text = "OmniMapper";
            // 
            // OMAPInstall
            // 
            this.OMAPInstall.Index = 0;
            this.OMAPInstall.Text = "Install the MIDI mapper";
            this.OMAPInstall.Click += new System.EventHandler(this.OMAPInstall_Click);
            // 
            // OMAPUninstall
            // 
            this.OMAPUninstall.Index = 1;
            this.OMAPUninstall.Text = "Uninstall the MIDI mapper";
            this.OMAPUninstall.Click += new System.EventHandler(this.OMAPUninstall_Click);
            // 
            // OMAPCpl
            // 
            this.OMAPCpl.Index = 6;
            this.OMAPCpl.Text = "OmniMapper control panel";
            this.OMAPCpl.Visible = false;
            this.OMAPCpl.Click += new System.EventHandler(this.OMAPCpl_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 2;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.MIDIInOutTest,
            this.menuItem15,
            this.ReinstallDriver,
            this.DeleteUserData,
            this.menuItem17,
            this.RestoreSFListEdWidth,
            this.RestoreConfSettings,
            this.DWCF});
            this.menuItem1.Text = "Tools";
            // 
            // MIDIInOutTest
            // 
            this.MIDIInOutTest.Index = 0;
            this.MIDIInOutTest.Text = "Test MIDI input and output";
            this.MIDIInOutTest.Click += new System.EventHandler(this.MIDIInOutTest_Click);
            // 
            // menuItem15
            // 
            this.menuItem15.Index = 1;
            this.menuItem15.Text = "-";
            // 
            // ReinstallDriver
            // 
            this.ReinstallDriver.Index = 2;
            this.ReinstallDriver.Text = "Reinstall OmniMIDI with default settings";
            this.ReinstallDriver.Click += new System.EventHandler(this.ReinstallDriver_Click);
            // 
            // DeleteUserData
            // 
            this.DeleteUserData.Index = 3;
            this.DeleteUserData.Text = "Clear all OmniMIDI\'s user data";
            this.DeleteUserData.Click += new System.EventHandler(this.DeleteUserData_Click);
            // 
            // menuItem17
            // 
            this.menuItem17.Index = 4;
            this.menuItem17.Text = "-";
            // 
            // RestoreSFListEdWidth
            // 
            this.RestoreSFListEdWidth.Index = 5;
            this.RestoreSFListEdWidth.Text = "Restore default SoundFonts list editor\'s column width";
            this.RestoreSFListEdWidth.Visible = false;
            this.RestoreSFListEdWidth.Click += new System.EventHandler(this.RestoreSFListEdWidth_Click);
            // 
            // RestoreConfSettings
            // 
            this.RestoreConfSettings.Index = 6;
            this.RestoreConfSettings.Text = "Restore all the configurator\'s internal settings";
            this.RestoreConfSettings.Click += new System.EventHandler(this.RestoreConfSettings_Click);
            // 
            // DWCF
            // 
            this.DWCF.Index = 7;
            this.DWCF.Text = "Enable UI performance mode";
            this.DWCF.Click += new System.EventHandler(this.DWCF_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 3;
            this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.DriverInfo,
            this.CUBBtn,
            this.CFUBtn,
            this.menuItem14,
            this.ChangelogCurrent,
            this.ChangelogLatest,
            this.menuItem19,
            this.BugReport,
            this.DLDriverSrc,
            this.menuItem6,
            this.KDMAPIDoc});
            this.menuItem2.Text = "?";
            // 
            // DriverInfo
            // 
            this.DriverInfo.Index = 0;
            this.DriverInfo.Text = "Information about the driver";
            this.DriverInfo.Click += new System.EventHandler(this.DriverInfo_Click);
            // 
            // CUBBtn
            // 
            this.CUBBtn.Index = 1;
            this.CUBBtn.Text = "Change update branch";
            this.CUBBtn.Click += new System.EventHandler(this.CUBBtn_Click);
            // 
            // CFUBtn
            // 
            this.CFUBtn.Index = 2;
            this.CFUBtn.Text = "Check for updates";
            this.CFUBtn.Click += new System.EventHandler(this.CFUBtn_Click);
            // 
            // menuItem14
            // 
            this.menuItem14.Index = 3;
            this.menuItem14.Text = "-";
            // 
            // ChangelogCurrent
            // 
            this.ChangelogCurrent.Index = 4;
            this.ChangelogCurrent.Text = "Changelog of this driver release";
            this.ChangelogCurrent.Click += new System.EventHandler(this.ChangelogCurrent_Click);
            // 
            // ChangelogLatest
            // 
            this.ChangelogLatest.Index = 5;
            this.ChangelogLatest.Text = "Changelog of the latest driver release";
            this.ChangelogLatest.Click += new System.EventHandler(this.ChangelogLatest_Click);
            // 
            // menuItem19
            // 
            this.menuItem19.Index = 6;
            this.menuItem19.Text = "-";
            // 
            // BugReport
            // 
            this.BugReport.Index = 7;
            this.BugReport.Text = "Create an issue on GitHub";
            this.BugReport.Click += new System.EventHandler(this.BugReport_Click);
            // 
            // DLDriverSrc
            // 
            this.DLDriverSrc.Index = 8;
            this.DLDriverSrc.Text = "Download the driver\'s source code";
            this.DLDriverSrc.Click += new System.EventHandler(this.DLDriverSrc_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 9;
            this.menuItem6.Text = "-";
            // 
            // KDMAPIDoc
            // 
            this.KDMAPIDoc.Index = 10;
            this.KDMAPIDoc.Text = "KDMAPI documentation";
            this.KDMAPIDoc.Click += new System.EventHandler(this.KDMAPIDoc_Click);
            // 
            // CheckUpdates
            // 
            this.CheckUpdates.DoWork += new System.ComponentModel.DoWorkEventHandler(this.CheckUpdates_DoWork);
            // 
            // UpdateNow
            // 
            this.UpdateNow.Interval = 500;
            this.UpdateNow.Tick += new System.EventHandler(this.UpdateNow_Tick);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 555);
            this.Controls.Add(this.MWSStrip);
            this.Controls.Add(this.MWTab);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(837, 594);
            this.Name = "MainWindow";
            this.Text = "OmniMIDI ~ Configurator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.ResizeEnd += new System.EventHandler(this.MainWindow_ResizeEnd);
            this.MWTab.ResumeLayout(false);
            this.ListsEdt.ResumeLayout(false);
            this.Set.ResumeLayout(false);
            this.Set.PerformLayout();
            this.MWSStrip.ResumeLayout(false);
            this.MWSStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl MWTab;
        private System.Windows.Forms.TabPage ListsEdt;
        private System.Windows.Forms.TabPage Set;
        private System.Windows.Forms.StatusStrip MWSStrip;
        private BufferedPanel SFLEPanel;
        private System.Windows.Forms.MainMenu OMMenu;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem DriverInfo;
        private BufferedPanel SETPanel;
        private System.Windows.Forms.Button ApplySettings;
        private System.Windows.Forms.Button RestoreDefault;
        private System.Windows.Forms.ComboBox QICombo;
        private System.Windows.Forms.Label QILabel;
        private System.Windows.Forms.Button ExportPres;
        private System.Windows.Forms.Button ImportPres;
        private System.Windows.Forms.Label SeparatorPres;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem InstallLM;
        private System.Windows.Forms.MenuItem UninstallLM;
        private System.Windows.Forms.MenuItem menuItem7;
        private System.Windows.Forms.MenuItem menuItem8;
        private System.Windows.Forms.MenuItem WMWPatch;
        private System.Windows.Forms.MenuItem menuItem10;
        private System.Windows.Forms.MenuItem MIDIInOutTest;
        private System.Windows.Forms.MenuItem menuItem15;
        private System.Windows.Forms.MenuItem DeleteUserData;
        private System.Windows.Forms.MenuItem ReinstallDriver;
        private System.Windows.Forms.MenuItem OMAPInstall;
        private System.Windows.Forms.MenuItem OMAPUninstall;
        private System.Windows.Forms.MenuItem OMAPCpl;
        private System.Windows.Forms.MenuItem menuItem5;
        private System.Windows.Forms.MenuItem BugReport;
        private System.Windows.Forms.MenuItem menuItem14;
        private System.Windows.Forms.MenuItem CFUBtn;
        private System.Windows.Forms.MenuItem ChangelogCurrent;
        private System.Windows.Forms.MenuItem ChangelogLatest;
        private System.Windows.Forms.MenuItem menuItem19;
        private System.Windows.Forms.MenuItem KDMAPIDoc;
        private System.Windows.Forms.MenuItem DLDriverSrc;
        private System.Windows.Forms.MenuItem menuItem11;
        private System.Windows.Forms.MenuItem OpenDW;
        private System.Windows.Forms.MenuItem OpenM;
        private System.Windows.Forms.MenuItem OpenBM;
        private System.Windows.Forms.MenuItem menuItem21;
        private System.Windows.Forms.MenuItem CloseConfigurator;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel VersionLabel;
        private System.ComponentModel.BackgroundWorker CheckUpdates;
        private System.Windows.Forms.MenuItem RestoreConfSettings;
        private System.Windows.Forms.MenuItem menuItem17;
        private System.Windows.Forms.MenuItem RestoreSFListEdWidth;
        private System.Windows.Forms.MenuItem AssignListToApp;
        private System.Windows.Forms.MenuItem menuItem16;
        private System.Windows.Forms.MenuItem OMDRegister;
        private System.Windows.Forms.MenuItem OMDUnregister;
        private System.Windows.Forms.MenuItem menuItem20;
        private System.Windows.Forms.MenuItem DWCF;
        private System.Windows.Forms.MenuItem CUBBtn;
        private System.Windows.Forms.MenuItem menuItem6;
        private System.Windows.Forms.MenuItem menuItem9;
        private System.Windows.Forms.MenuItem RegTroubleshooter;
        private System.Windows.Forms.Timer UpdateNow;
    }
}

