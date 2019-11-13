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
            this.OMMenu = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem11 = new System.Windows.Forms.MenuItem();
            this.OpenDW = new System.Windows.Forms.MenuItem();
            this.OpenM = new System.Windows.Forms.MenuItem();
            this.OpenBM = new System.Windows.Forms.MenuItem();
            this.OpenRTSSOSDM = new System.Windows.Forms.MenuItem();
            this.menuItem21 = new System.Windows.Forms.MenuItem();
            this.CloseConfigurator = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.InstallLM = new System.Windows.Forms.MenuItem();
            this.UninstallLM = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.WMWPatch = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItem10 = new System.Windows.Forms.MenuItem();
            this.OMAPInstall = new System.Windows.Forms.MenuItem();
            this.OMAPUninstall = new System.Windows.Forms.MenuItem();
            this.OMAPCpl = new System.Windows.Forms.MenuItem();
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.menuItem12 = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.KACGuide = new System.Windows.Forms.MenuItem();
            this.menuItem13 = new System.Windows.Forms.MenuItem();
            this.MIDIInOutTest = new System.Windows.Forms.MenuItem();
            this.menuItem15 = new System.Windows.Forms.MenuItem();
            this.DeleteUserData = new System.Windows.Forms.MenuItem();
            this.ReinstallDriver = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.DriverInfo = new System.Windows.Forms.MenuItem();
            this.BugReport = new System.Windows.Forms.MenuItem();
            this.menuItem14 = new System.Windows.Forms.MenuItem();
            this.CFUBtn = new System.Windows.Forms.MenuItem();
            this.ChangelogCurrent = new System.Windows.Forms.MenuItem();
            this.ChangelogLatest = new System.Windows.Forms.MenuItem();
            this.menuItem19 = new System.Windows.Forms.MenuItem();
            this.KDMAPIDoc = new System.Windows.Forms.MenuItem();
            this.DLDriverSrc = new System.Windows.Forms.MenuItem();
            this.MWTab.SuspendLayout();
            this.ListsEdt.SuspendLayout();
            this.Set.SuspendLayout();
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
            this.MWTab.Size = new System.Drawing.Size(706, 460);
            this.MWTab.TabIndex = 0;
            // 
            // ListsEdt
            // 
            this.ListsEdt.Controls.Add(this.SFLEPanel);
            this.ListsEdt.Location = new System.Drawing.Point(4, 22);
            this.ListsEdt.Name = "ListsEdt";
            this.ListsEdt.Padding = new System.Windows.Forms.Padding(3);
            this.ListsEdt.Size = new System.Drawing.Size(698, 434);
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
            this.SFLEPanel.Size = new System.Drawing.Size(698, 436);
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
            this.Set.Location = new System.Drawing.Point(4, 22);
            this.Set.Name = "Set";
            this.Set.Padding = new System.Windows.Forms.Padding(3);
            this.Set.Size = new System.Drawing.Size(698, 434);
            this.Set.TabIndex = 1;
            this.Set.Text = "Settings";
            this.Set.UseVisualStyleBackColor = true;
            // 
            // ExportPres
            // 
            this.ExportPres.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ExportPres.Location = new System.Drawing.Point(269, 407);
            this.ExportPres.Name = "ExportPres";
            this.ExportPres.Size = new System.Drawing.Size(45, 23);
            this.ExportPres.TabIndex = 52;
            this.ExportPres.Text = "Export";
            this.ExportPres.UseVisualStyleBackColor = true;
            // 
            // ImportPres
            // 
            this.ImportPres.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ImportPres.Location = new System.Drawing.Point(222, 407);
            this.ImportPres.Name = "ImportPres";
            this.ImportPres.Size = new System.Drawing.Size(45, 23);
            this.ImportPres.TabIndex = 51;
            this.ImportPres.Text = "Import";
            this.ImportPres.UseVisualStyleBackColor = true;
            // 
            // SeparatorPres
            // 
            this.SeparatorPres.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SeparatorPres.Enabled = false;
            this.SeparatorPres.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SeparatorPres.Location = new System.Drawing.Point(208, 406);
            this.SeparatorPres.Name = "SeparatorPres";
            this.SeparatorPres.Size = new System.Drawing.Size(10, 23);
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
            "Miscellaneous settings",
            "Legacy settings etc."});
            this.QICombo.Location = new System.Drawing.Point(77, 408);
            this.QICombo.Name = "QICombo";
            this.QICombo.Size = new System.Drawing.Size(130, 21);
            this.QICombo.TabIndex = 5;
            this.QICombo.SelectedIndexChanged += new System.EventHandler(this.QICombo_SelectedIndexChanged);
            // 
            // QILabel
            // 
            this.QILabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.QILabel.AutoSize = true;
            this.QILabel.Location = new System.Drawing.Point(8, 412);
            this.QILabel.Name = "QILabel";
            this.QILabel.Size = new System.Drawing.Size(66, 13);
            this.QILabel.TabIndex = 4;
            this.QILabel.Text = "Quick index:";
            // 
            // RestoreDefault
            // 
            this.RestoreDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RestoreDefault.Location = new System.Drawing.Point(516, 407);
            this.RestoreDefault.Name = "RestoreDefault";
            this.RestoreDefault.Size = new System.Drawing.Size(95, 23);
            this.RestoreDefault.TabIndex = 3;
            this.RestoreDefault.Text = "Restore default";
            this.RestoreDefault.UseVisualStyleBackColor = true;
            // 
            // ApplySettings
            // 
            this.ApplySettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ApplySettings.Location = new System.Drawing.Point(617, 407);
            this.ApplySettings.Name = "ApplySettings";
            this.ApplySettings.Size = new System.Drawing.Size(75, 23);
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
            this.SETPanel.Size = new System.Drawing.Size(698, 403);
            this.SETPanel.TabIndex = 1;
            // 
            // MWSStrip
            // 
            this.MWSStrip.Location = new System.Drawing.Point(0, 459);
            this.MWSStrip.Name = "MWSStrip";
            this.MWSStrip.Size = new System.Drawing.Size(704, 22);
            this.MWSStrip.SizingGrip = false;
            this.MWSStrip.TabIndex = 1;
            this.MWSStrip.Text = "statusStrip1";
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
            this.OpenRTSSOSDM,
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
            // OpenRTSSOSDM
            // 
            this.OpenRTSSOSDM.Index = 3;
            this.OpenRTSSOSDM.Text = "Open RTSS OSD manager";
            this.OpenRTSSOSDM.Click += new System.EventHandler(this.OpenRTSSOSDM_Click);
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
            this.menuItem4,
            this.menuItem6,
            this.WMWPatch,
            this.menuItem5,
            this.menuItem10,
            this.OMAPCpl,
            this.menuItem9,
            this.menuItem12});
            this.menuItem3.Text = "Extensions";
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 0;
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
            this.menuItem8.Index = 3;
            this.menuItem8.Text = "LoudMax is not available on ARM64 installs";
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 1;
            this.menuItem6.Text = "-";
            // 
            // WMWPatch
            // 
            this.WMWPatch.Index = 2;
            this.WMWPatch.Text = "Windows Multimedia Wrapper";
            this.WMWPatch.Click += new System.EventHandler(this.WMWPatch_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 3;
            this.menuItem5.Text = "-";
            // 
            // menuItem10
            // 
            this.menuItem10.Index = 4;
            this.menuItem10.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.OMAPInstall,
            this.OMAPUninstall});
            this.menuItem10.Text = "OmniMapper";
            // 
            // OMAPInstall
            // 
            this.OMAPInstall.Index = 0;
            this.OMAPInstall.Text = "Install/Fix";
            this.OMAPInstall.Click += new System.EventHandler(this.OMAPInstall_Click);
            // 
            // OMAPUninstall
            // 
            this.OMAPUninstall.Index = 1;
            this.OMAPUninstall.Text = "Uninstall";
            this.OMAPUninstall.Click += new System.EventHandler(this.OMAPUninstall_Click);
            // 
            // OMAPCpl
            // 
            this.OMAPCpl.Index = 5;
            this.OMAPCpl.Text = "OmniMapper control panel";
            this.OMAPCpl.Visible = false;
            this.OMAPCpl.Click += new System.EventHandler(this.OMAPCpl_Click);
            // 
            // menuItem9
            // 
            this.menuItem9.Index = 6;
            this.menuItem9.Text = "-";
            // 
            // menuItem12
            // 
            this.menuItem12.Enabled = false;
            this.menuItem12.Index = 7;
            this.menuItem12.Text = "More to come...";
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 2;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.KACGuide,
            this.menuItem13,
            this.MIDIInOutTest,
            this.menuItem15,
            this.DeleteUserData,
            this.ReinstallDriver});
            this.menuItem1.Text = "Tools";
            // 
            // KACGuide
            // 
            this.KACGuide.Index = 0;
            this.KACGuide.Text = "Install the Keppy\'s Authentication Certificate";
            this.KACGuide.Click += new System.EventHandler(this.KACGuide_Click);
            // 
            // menuItem13
            // 
            this.menuItem13.Index = 1;
            this.menuItem13.Text = "-";
            // 
            // MIDIInOutTest
            // 
            this.MIDIInOutTest.Index = 2;
            this.MIDIInOutTest.Text = "Test MIDI input and output";
            this.MIDIInOutTest.Click += new System.EventHandler(this.MIDIInOutTest_Click);
            // 
            // menuItem15
            // 
            this.menuItem15.Index = 3;
            this.menuItem15.Text = "-";
            // 
            // DeleteUserData
            // 
            this.DeleteUserData.Index = 4;
            this.DeleteUserData.Text = "Reinstall OmniMIDI from scratch";
            this.DeleteUserData.Click += new System.EventHandler(this.DeleteUserData_Click);
            // 
            // ReinstallDriver
            // 
            this.ReinstallDriver.Index = 5;
            this.ReinstallDriver.Text = "Delete OmniMIDI\'s user data";
            this.ReinstallDriver.Click += new System.EventHandler(this.ReinstallDriver_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 3;
            this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.DriverInfo,
            this.BugReport,
            this.menuItem14,
            this.CFUBtn,
            this.ChangelogCurrent,
            this.ChangelogLatest,
            this.menuItem19,
            this.KDMAPIDoc,
            this.DLDriverSrc});
            this.menuItem2.Text = "?";
            // 
            // DriverInfo
            // 
            this.DriverInfo.Index = 0;
            this.DriverInfo.Text = "Information about the driver";
            this.DriverInfo.Click += new System.EventHandler(this.DriverInfo_Click);
            // 
            // BugReport
            // 
            this.BugReport.Index = 1;
            this.BugReport.Text = "Create an issue on GitHub";
            this.BugReport.Click += new System.EventHandler(this.BugReport_Click);
            // 
            // menuItem14
            // 
            this.menuItem14.Index = 2;
            this.menuItem14.Text = "-";
            // 
            // CFUBtn
            // 
            this.CFUBtn.Index = 3;
            this.CFUBtn.Text = "Check for updates";
            this.CFUBtn.Click += new System.EventHandler(this.CFUBtn_Click);
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
            // KDMAPIDoc
            // 
            this.KDMAPIDoc.Index = 7;
            this.KDMAPIDoc.Text = "KDMAPI documentation";
            this.KDMAPIDoc.Click += new System.EventHandler(this.KDMAPIDoc_Click);
            // 
            // DLDriverSrc
            // 
            this.DLDriverSrc.Index = 8;
            this.DLDriverSrc.Text = "Download the driver\'s source code";
            this.DLDriverSrc.Click += new System.EventHandler(this.DLDriverSrc_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 481);
            this.Controls.Add(this.MWSStrip);
            this.Controls.Add(this.MWTab);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(720, 520);
            this.Name = "MainWindow";
            this.Text = "OmniMIDI ~ Configurator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.ResizeEnd += new System.EventHandler(this.MainWindow_ResizeEnd);
            this.MWTab.ResumeLayout(false);
            this.ListsEdt.ResumeLayout(false);
            this.Set.ResumeLayout(false);
            this.Set.PerformLayout();
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
        private System.Windows.Forms.MenuItem KACGuide;
        private System.Windows.Forms.MenuItem menuItem13;
        private System.Windows.Forms.MenuItem MIDIInOutTest;
        private System.Windows.Forms.MenuItem menuItem15;
        private System.Windows.Forms.MenuItem DeleteUserData;
        private System.Windows.Forms.MenuItem ReinstallDriver;
        private System.Windows.Forms.MenuItem OMAPInstall;
        private System.Windows.Forms.MenuItem OMAPUninstall;
        private System.Windows.Forms.MenuItem OMAPCpl;
        private System.Windows.Forms.MenuItem menuItem9;
        private System.Windows.Forms.MenuItem menuItem12;
        private System.Windows.Forms.MenuItem menuItem6;
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
        private System.Windows.Forms.MenuItem OpenRTSSOSDM;
        private System.Windows.Forms.MenuItem menuItem21;
        private System.Windows.Forms.MenuItem CloseConfigurator;
    }
}

