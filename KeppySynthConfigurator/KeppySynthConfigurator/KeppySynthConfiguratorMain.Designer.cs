namespace KeppySynthConfigurator
{
    partial class KeppySynthConfiguratorMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KeppySynthConfiguratorMain));
            this.TabsForTheControls = new System.Windows.Forms.TabControl();
            this.List = new System.Windows.Forms.TabPage();
            this.SelectedListBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.DisableSF = new System.Windows.Forms.Button();
            this.EnableSF = new System.Windows.Forms.Button();
            this.List1Override = new System.Windows.Forms.Label();
            this.IELPan1 = new System.Windows.Forms.Panel();
            this.EL = new System.Windows.Forms.Button();
            this.IEL = new System.Windows.Forms.Button();
            this.CLi = new System.Windows.Forms.Button();
            this.MvD = new System.Windows.Forms.Button();
            this.MvU = new System.Windows.Forms.Button();
            this.RmvSF = new System.Windows.Forms.Button();
            this.AddSF = new System.Windows.Forms.Button();
            this.Lis = new System.Windows.Forms.ListBox();
            this.Settings = new System.Windows.Forms.TabPage();
            this.WhatIsXAudio = new System.Windows.Forms.PictureBox();
            this.XAudioDisable = new System.Windows.Forms.CheckBox();
            this.WhatIsOutput = new System.Windows.Forms.PictureBox();
            this.OutputWAV = new System.Windows.Forms.CheckBox();
            this.GroupBox5 = new System.Windows.Forms.GroupBox();
            this.SPFSecondaryBut = new System.Windows.Forms.LinkLabel();
            this.VMSEmu = new System.Windows.Forms.CheckBox();
            this.SincInter = new System.Windows.Forms.CheckBox();
            this.TracksLimit = new System.Windows.Forms.NumericUpDown();
            this.Label4 = new System.Windows.Forms.Label();
            this.BufferText = new System.Windows.Forms.Label();
            this.SysResetIgnore = new System.Windows.Forms.CheckBox();
            this.bufsize = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Label6 = new System.Windows.Forms.Label();
            this.Frequency = new System.Windows.Forms.ComboBox();
            this.Label5 = new System.Windows.Forms.Label();
            this.MaxCPU = new System.Windows.Forms.ComboBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.PolyphonyLimit = new System.Windows.Forms.NumericUpDown();
            this.DisableSFX = new System.Windows.Forms.CheckBox();
            this.Preload = new System.Windows.Forms.CheckBox();
            this.NoteOffCheck = new System.Windows.Forms.CheckBox();
            this.VolIntView = new System.Windows.Forms.Label();
            this.VolSimView = new System.Windows.Forms.Label();
            this.VolStaticLab = new System.Windows.Forms.Label();
            this.VolTrackBar = new System.Windows.Forms.TrackBar();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.applySettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToDefaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsPresetsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lowLatencyPresetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blackMIDIsPresetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.assignSoundfontListToAppToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeDefaultSoundfontListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SoundfontImport = new System.Windows.Forms.OpenFileDialog();
            this.ExternalListImport = new System.Windows.Forms.OpenFileDialog();
            this.ExternalListExport = new System.Windows.Forms.SaveFileDialog();
            this.VolumeHotkeysCheck = new System.Windows.Forms.Timer(this.components);
            this.MainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.openDebugWindowToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.openTheMixerToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.openTheBlacklistManagerToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.assignASoundfontListToASpecificAppToolStripMenuItemToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.changeDefaultMIDIOutDeviceToolStripMenuItem1 = new System.Windows.Forms.MenuItem();
            this.changeDefaultMIDIOutDeviceToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.changeDefault64bitMIDIOutDeviceToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem13 = new System.Windows.Forms.MenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem38 = new System.Windows.Forms.MenuItem();
            this.checkEnabledToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.checkDisabledToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.killTheWatchdogToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem17 = new System.Windows.Forms.MenuItem();
            this.manageFolderFavouritesToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem19 = new System.Windows.Forms.MenuItem();
            this.menuItem20 = new System.Windows.Forms.MenuItem();
            this.enabledToolStripMenuItem2 = new System.Windows.Forms.MenuItem();
            this.disabledToolStripMenuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem21 = new System.Windows.Forms.MenuItem();
            this.enabledToolStripMenuItem3 = new System.Windows.Forms.MenuItem();
            this.disabledToolStripMenuItem3 = new System.Windows.Forms.MenuItem();
            this.changeTheSizeOfTheEVBufferToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.changeDirectoryOfTheOutputToWAVModeToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.changeTheMaximumSamplesPerFrameToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.changeDefaultSoundfontListToolStripMenuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem26 = new System.Windows.Forms.MenuItem();
            this.menuItem27 = new System.Windows.Forms.MenuItem();
            this.menuItem15 = new System.Windows.Forms.MenuItem();
            this.hLSEnabledToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.hLSDisabledToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem16 = new System.Windows.Forms.MenuItem();
            this.soundfontListChangeConfirmationDialogToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.enabledToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.disabledToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.volumeHotkeysToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.enabledToolStripMenuItem1 = new System.Windows.Forms.MenuItem();
            this.disabledToolStripMenuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem28 = new System.Windows.Forms.MenuItem();
            this.watchdogEnabledToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.watchdogDisabledToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.informationAboutTheDriverToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.openUpdaterToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.reportABugToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.getTheMIDIMapperForWindows10ToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem42 = new System.Windows.Forms.MenuItem();
            this.guidesToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.isThereAnyShortcutForToOpenTheConfiguratorToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.whatAreTheHotkeysToChangeTheVolumeToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.howCanIChangeTheSoundfontListToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.whatsTheBestSettingsForTheBufferToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.howCanIResetTheDriverToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem44 = new System.Windows.Forms.MenuItem();
            this.donateToSupportUsToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.downloadTheSourceCodeToolStripMenuItem = new System.Windows.Forms.MenuItem();
            this.TabsForTheControls.SuspendLayout();
            this.List.SuspendLayout();
            this.IELPan1.SuspendLayout();
            this.Settings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WhatIsXAudio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WhatIsOutput)).BeginInit();
            this.GroupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TracksLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bufsize)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PolyphonyLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VolTrackBar)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TabsForTheControls
            // 
            this.TabsForTheControls.Controls.Add(this.List);
            this.TabsForTheControls.Controls.Add(this.Settings);
            this.TabsForTheControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabsForTheControls.Location = new System.Drawing.Point(0, 0);
            this.TabsForTheControls.Name = "TabsForTheControls";
            this.TabsForTheControls.SelectedIndex = 0;
            this.TabsForTheControls.Size = new System.Drawing.Size(685, 423);
            this.TabsForTheControls.TabIndex = 1;
            // 
            // List
            // 
            this.List.BackColor = System.Drawing.Color.White;
            this.List.Controls.Add(this.SelectedListBox);
            this.List.Controls.Add(this.label1);
            this.List.Controls.Add(this.DisableSF);
            this.List.Controls.Add(this.EnableSF);
            this.List.Controls.Add(this.List1Override);
            this.List.Controls.Add(this.IELPan1);
            this.List.Controls.Add(this.CLi);
            this.List.Controls.Add(this.MvD);
            this.List.Controls.Add(this.MvU);
            this.List.Controls.Add(this.RmvSF);
            this.List.Controls.Add(this.AddSF);
            this.List.Controls.Add(this.Lis);
            this.List.Location = new System.Drawing.Point(4, 22);
            this.List.Name = "List";
            this.List.Padding = new System.Windows.Forms.Padding(3);
            this.List.Size = new System.Drawing.Size(677, 397);
            this.List.TabIndex = 0;
            this.List.Text = "Lists editor";
            this.List.UseVisualStyleBackColor = true;
            // 
            // SelectedListBox
            // 
            this.SelectedListBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SelectedListBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
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
            this.SelectedListBox.Location = new System.Drawing.Point(101, 8);
            this.SelectedListBox.Name = "SelectedListBox";
            this.SelectedListBox.Size = new System.Drawing.Size(57, 21);
            this.SelectedListBox.TabIndex = 2;
            this.SelectedListBox.SelectedIndexChanged += new System.EventHandler(this.SelectedListBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 34;
            this.label1.Text = "Select list to edit:";
            // 
            // DisableSF
            // 
            this.DisableSF.Location = new System.Drawing.Point(581, 262);
            this.DisableSF.Name = "DisableSF";
            this.DisableSF.Size = new System.Drawing.Size(89, 23);
            this.DisableSF.TabIndex = 9;
            this.DisableSF.Text = "Disable SF";
            this.DisableSF.UseVisualStyleBackColor = true;
            this.DisableSF.Click += new System.EventHandler(this.DisableSF_Click);
            // 
            // EnableSF
            // 
            this.EnableSF.Location = new System.Drawing.Point(581, 233);
            this.EnableSF.Name = "EnableSF";
            this.EnableSF.Size = new System.Drawing.Size(89, 23);
            this.EnableSF.TabIndex = 8;
            this.EnableSF.Text = "Enable SF";
            this.EnableSF.UseVisualStyleBackColor = true;
            this.EnableSF.Click += new System.EventHandler(this.EnableSF_Click);
            // 
            // List1Override
            // 
            this.List1Override.AutoSize = true;
            this.List1Override.Enabled = false;
            this.List1Override.Location = new System.Drawing.Point(8, 382);
            this.List1Override.Name = "List1Override";
            this.List1Override.Size = new System.Drawing.Size(250, 13);
            this.List1Override.TabIndex = 31;
            this.List1Override.Text = "The last soundfont will override the previous ones.";
            // 
            // IELPan1
            // 
            this.IELPan1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.IELPan1.Controls.Add(this.EL);
            this.IELPan1.Controls.Add(this.IEL);
            this.IELPan1.Location = new System.Drawing.Point(581, 316);
            this.IELPan1.Name = "IELPan1";
            this.IELPan1.Size = new System.Drawing.Size(89, 63);
            this.IELPan1.TabIndex = 30;
            // 
            // EL
            // 
            this.EL.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.EL.Location = new System.Drawing.Point(0, 29);
            this.EL.Name = "EL";
            this.EL.Size = new System.Drawing.Size(85, 30);
            this.EL.TabIndex = 11;
            this.EL.Text = "Export list";
            this.EL.UseVisualStyleBackColor = true;
            this.EL.Click += new System.EventHandler(this.EL_Click);
            // 
            // IEL
            // 
            this.IEL.Dock = System.Windows.Forms.DockStyle.Top;
            this.IEL.Location = new System.Drawing.Point(0, 0);
            this.IEL.Name = "IEL";
            this.IEL.Size = new System.Drawing.Size(85, 30);
            this.IEL.TabIndex = 10;
            this.IEL.Text = "Import list";
            this.IEL.UseVisualStyleBackColor = true;
            this.IEL.Click += new System.EventHandler(this.IEL_Click);
            // 
            // CLi
            // 
            this.CLi.Location = new System.Drawing.Point(581, 7);
            this.CLi.Name = "CLi";
            this.CLi.Size = new System.Drawing.Size(89, 23);
            this.CLi.TabIndex = 3;
            this.CLi.Text = "Clear list";
            this.CLi.UseVisualStyleBackColor = true;
            this.CLi.Click += new System.EventHandler(this.CLi_Click);
            // 
            // MvD
            // 
            this.MvD.Location = new System.Drawing.Point(581, 163);
            this.MvD.Name = "MvD";
            this.MvD.Size = new System.Drawing.Size(89, 23);
            this.MvD.TabIndex = 7;
            this.MvD.Text = "Move ▼";
            this.MvD.UseVisualStyleBackColor = true;
            this.MvD.Click += new System.EventHandler(this.MvD_Click);
            // 
            // MvU
            // 
            this.MvU.Location = new System.Drawing.Point(581, 134);
            this.MvU.Name = "MvU";
            this.MvU.Size = new System.Drawing.Size(89, 23);
            this.MvU.TabIndex = 6;
            this.MvU.Text = "Move ▲";
            this.MvU.UseVisualStyleBackColor = true;
            this.MvU.Click += new System.EventHandler(this.MvU_Click);
            // 
            // RmvSF
            // 
            this.RmvSF.Location = new System.Drawing.Point(581, 65);
            this.RmvSF.Name = "RmvSF";
            this.RmvSF.Size = new System.Drawing.Size(89, 23);
            this.RmvSF.TabIndex = 5;
            this.RmvSF.Text = "Remove -";
            this.RmvSF.UseVisualStyleBackColor = true;
            this.RmvSF.Click += new System.EventHandler(this.RmvSF_Click);
            // 
            // AddSF
            // 
            this.AddSF.Location = new System.Drawing.Point(581, 36);
            this.AddSF.Name = "AddSF";
            this.AddSF.Size = new System.Drawing.Size(89, 23);
            this.AddSF.TabIndex = 4;
            this.AddSF.Text = "Add +";
            this.AddSF.UseVisualStyleBackColor = true;
            this.AddSF.Click += new System.EventHandler(this.AddSF_Click);
            // 
            // Lis
            // 
            this.Lis.AllowDrop = true;
            this.Lis.FormattingEnabled = true;
            this.Lis.HorizontalScrollbar = true;
            this.Lis.Location = new System.Drawing.Point(6, 36);
            this.Lis.Name = "Lis";
            this.Lis.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.Lis.Size = new System.Drawing.Size(568, 342);
            this.Lis.TabIndex = 12;
            this.Lis.DragDrop += new System.Windows.Forms.DragEventHandler(this.Lis_DragDrop);
            this.Lis.DragEnter += new System.Windows.Forms.DragEventHandler(this.Lis_DragEnter);
            // 
            // Settings
            // 
            this.Settings.BackColor = System.Drawing.Color.White;
            this.Settings.Controls.Add(this.WhatIsXAudio);
            this.Settings.Controls.Add(this.XAudioDisable);
            this.Settings.Controls.Add(this.WhatIsOutput);
            this.Settings.Controls.Add(this.OutputWAV);
            this.Settings.Controls.Add(this.GroupBox5);
            this.Settings.Controls.Add(this.groupBox1);
            this.Settings.Controls.Add(this.VolIntView);
            this.Settings.Controls.Add(this.VolSimView);
            this.Settings.Controls.Add(this.VolStaticLab);
            this.Settings.Controls.Add(this.VolTrackBar);
            this.Settings.Controls.Add(this.menuStrip1);
            this.Settings.Location = new System.Drawing.Point(4, 22);
            this.Settings.Name = "Settings";
            this.Settings.Size = new System.Drawing.Size(677, 397);
            this.Settings.TabIndex = 4;
            this.Settings.Text = "Settings";
            this.Settings.UseVisualStyleBackColor = true;
            // 
            // WhatIsXAudio
            // 
            this.WhatIsXAudio.BackColor = System.Drawing.Color.Transparent;
            this.WhatIsXAudio.Cursor = System.Windows.Forms.Cursors.Hand;
            this.WhatIsXAudio.ErrorImage = global::KeppySynthConfigurator.Properties.Resources.what;
            this.WhatIsXAudio.Image = global::KeppySynthConfigurator.Properties.Resources.what;
            this.WhatIsXAudio.Location = new System.Drawing.Point(393, 9);
            this.WhatIsXAudio.Name = "WhatIsXAudio";
            this.WhatIsXAudio.Size = new System.Drawing.Size(21, 17);
            this.WhatIsXAudio.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.WhatIsXAudio.TabIndex = 37;
            this.WhatIsXAudio.TabStop = false;
            this.WhatIsXAudio.Click += new System.EventHandler(this.WhatIsXAudio_Click);
            // 
            // XAudioDisable
            // 
            this.XAudioDisable.AutoSize = true;
            this.XAudioDisable.Location = new System.Drawing.Point(242, 9);
            this.XAudioDisable.Name = "XAudioDisable";
            this.XAudioDisable.Size = new System.Drawing.Size(154, 17);
            this.XAudioDisable.TabIndex = 36;
            this.XAudioDisable.Text = "Disable the XAudio engine.";
            this.XAudioDisable.UseVisualStyleBackColor = true;
            this.XAudioDisable.CheckedChanged += new System.EventHandler(this.XAudioDisable_CheckedChanged);
            // 
            // WhatIsOutput
            // 
            this.WhatIsOutput.BackColor = System.Drawing.Color.Transparent;
            this.WhatIsOutput.Cursor = System.Windows.Forms.Cursors.Hand;
            this.WhatIsOutput.ErrorImage = global::KeppySynthConfigurator.Properties.Resources.what;
            this.WhatIsOutput.Image = global::KeppySynthConfigurator.Properties.Resources.what;
            this.WhatIsOutput.Location = new System.Drawing.Point(205, 9);
            this.WhatIsOutput.Name = "WhatIsOutput";
            this.WhatIsOutput.Size = new System.Drawing.Size(21, 17);
            this.WhatIsOutput.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.WhatIsOutput.TabIndex = 35;
            this.WhatIsOutput.TabStop = false;
            this.WhatIsOutput.Click += new System.EventHandler(this.WhatIsOutput_Click);
            // 
            // OutputWAV
            // 
            this.OutputWAV.AutoSize = true;
            this.OutputWAV.Location = new System.Drawing.Point(15, 9);
            this.OutputWAV.Name = "OutputWAV";
            this.OutputWAV.Size = new System.Drawing.Size(194, 17);
            this.OutputWAV.TabIndex = 27;
            this.OutputWAV.Text = "Enable the \"Output to WAV\" mode.";
            this.OutputWAV.UseVisualStyleBackColor = true;
            this.OutputWAV.CheckedChanged += new System.EventHandler(this.OutputWAV_CheckedChanged);
            // 
            // GroupBox5
            // 
            this.GroupBox5.Controls.Add(this.SPFSecondaryBut);
            this.GroupBox5.Controls.Add(this.VMSEmu);
            this.GroupBox5.Controls.Add(this.SincInter);
            this.GroupBox5.Controls.Add(this.TracksLimit);
            this.GroupBox5.Controls.Add(this.Label4);
            this.GroupBox5.Controls.Add(this.BufferText);
            this.GroupBox5.Controls.Add(this.SysResetIgnore);
            this.GroupBox5.Controls.Add(this.bufsize);
            this.GroupBox5.Location = new System.Drawing.Point(8, 258);
            this.GroupBox5.Name = "GroupBox5";
            this.GroupBox5.Size = new System.Drawing.Size(661, 107);
            this.GroupBox5.TabIndex = 31;
            this.GroupBox5.TabStop = false;
            this.GroupBox5.Text = "Advanced audio settings";
            // 
            // SPFSecondaryBut
            // 
            this.SPFSecondaryBut.AutoSize = true;
            this.SPFSecondaryBut.BackColor = System.Drawing.Color.Transparent;
            this.SPFSecondaryBut.Location = new System.Drawing.Point(243, 56);
            this.SPFSecondaryBut.Name = "SPFSecondaryBut";
            this.SPFSecondaryBut.Size = new System.Drawing.Size(39, 18);
            this.SPFSecondaryBut.TabIndex = 28;
            this.SPFSecondaryBut.TabStop = true;
            this.SPFSecondaryBut.Text = "More...";
            this.SPFSecondaryBut.UseCompatibleTextRendering = true;
            this.SPFSecondaryBut.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.SPFSecondaryBut_LinkClicked);
            // 
            // VMSEmu
            // 
            this.VMSEmu.AutoSize = true;
            this.VMSEmu.CheckAlign = System.Drawing.ContentAlignment.BottomRight;
            this.VMSEmu.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.VMSEmu.Location = new System.Drawing.Point(435, 55);
            this.VMSEmu.Name = "VMSEmu";
            this.VMSEmu.Size = new System.Drawing.Size(149, 17);
            this.VMSEmu.TabIndex = 27;
            this.VMSEmu.Text = "Alternative buffer system";
            this.VMSEmu.UseVisualStyleBackColor = true;
            this.VMSEmu.Visible = false;
            this.VMSEmu.CheckedChanged += new System.EventHandler(this.VMSEmu_CheckedChanged);
            // 
            // SincInter
            // 
            this.SincInter.AutoSize = true;
            this.SincInter.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.SincInter.Location = new System.Drawing.Point(7, 16);
            this.SincInter.Name = "SincInter";
            this.SincInter.Size = new System.Drawing.Size(526, 17);
            this.SincInter.TabIndex = 8;
            this.SincInter.Text = "Enable sinc interpolation. (Avoids audio corruptions, but can completely ruin the" +
    " audio with Black MIDIs.)";
            this.SincInter.UseVisualStyleBackColor = true;
            // 
            // TracksLimit
            // 
            this.TracksLimit.Location = new System.Drawing.Point(588, 77);
            this.TracksLimit.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.TracksLimit.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.TracksLimit.Name = "TracksLimit";
            this.TracksLimit.Size = new System.Drawing.Size(64, 21);
            this.TracksLimit.TabIndex = 11;
            this.TracksLimit.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Label4.Location = new System.Drawing.Point(4, 79);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(528, 13);
            this.Label4.TabIndex = 26;
            this.Label4.Text = "Set the channels the driver is allowed to use (Different values from 16 will set " +
    "the drums channel to melodic):";
            // 
            // BufferText
            // 
            this.BufferText.AutoSize = true;
            this.BufferText.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BufferText.Location = new System.Drawing.Point(4, 56);
            this.BufferText.Name = "BufferText";
            this.BufferText.Size = new System.Drawing.Size(291, 13);
            this.BufferText.TabIndex = 23;
            this.BufferText.Text = "Set a buffer length for the driver, from 1 to 100 (             ):";
            // 
            // SysResetIgnore
            // 
            this.SysResetIgnore.AutoSize = true;
            this.SysResetIgnore.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.SysResetIgnore.Location = new System.Drawing.Point(7, 34);
            this.SysResetIgnore.Name = "SysResetIgnore";
            this.SysResetIgnore.Size = new System.Drawing.Size(343, 17);
            this.SysResetIgnore.TabIndex = 9;
            this.SysResetIgnore.Text = "Ignore system reset events when the system mode is unchanged.";
            this.SysResetIgnore.UseVisualStyleBackColor = true;
            // 
            // bufsize
            // 
            this.bufsize.Location = new System.Drawing.Point(588, 54);
            this.bufsize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.bufsize.Name = "bufsize";
            this.bufsize.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.bufsize.Size = new System.Drawing.Size(64, 21);
            this.bufsize.TabIndex = 10;
            this.bufsize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.bufsize.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Label6);
            this.groupBox1.Controls.Add(this.Frequency);
            this.groupBox1.Controls.Add(this.Label5);
            this.groupBox1.Controls.Add(this.MaxCPU);
            this.groupBox1.Controls.Add(this.Label3);
            this.groupBox1.Controls.Add(this.PolyphonyLimit);
            this.groupBox1.Controls.Add(this.DisableSFX);
            this.groupBox1.Controls.Add(this.Preload);
            this.groupBox1.Controls.Add(this.NoteOffCheck);
            this.groupBox1.Location = new System.Drawing.Point(8, 93);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(661, 159);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Audio settings";
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Label6.Location = new System.Drawing.Point(6, 133);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(229, 13);
            this.Label6.TabIndex = 24;
            this.Label6.Text = "Set the audio frequency for the driver output:";
            // 
            // Frequency
            // 
            this.Frequency.FormattingEnabled = true;
            this.Frequency.Items.AddRange(new object[] {
            "192000",
            "176400",
            "142180",
            "96000",
            "88200",
            "74750",
            "66150",
            "50400",
            "50000",
            "48000",
            "47250 ",
            "44100",
            "44056 ",
            "37800",
            "32000",
            "22050",
            "16000",
            "11025",
            "8000",
            "4000"});
            this.Frequency.Location = new System.Drawing.Point(588, 128);
            this.Frequency.Name = "Frequency";
            this.Frequency.Size = new System.Drawing.Size(64, 21);
            this.Frequency.TabIndex = 7;
            this.Frequency.Text = "44100";
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Label5.Location = new System.Drawing.Point(6, 108);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(423, 13);
            this.Label5.TabIndex = 22;
            this.Label5.Text = "Set the maximum rendering time percentage/maximum CPU usage for BASS/BASSMIDI:";
            // 
            // MaxCPU
            // 
            this.MaxCPU.DropDownHeight = 150;
            this.MaxCPU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MaxCPU.DropDownWidth = 64;
            this.MaxCPU.FormattingEnabled = true;
            this.MaxCPU.IntegralHeight = false;
            this.MaxCPU.Items.AddRange(new object[] {
            "Disabled",
            "100",
            "99",
            "98",
            "97",
            "96",
            "95",
            "94",
            "93",
            "92",
            "91",
            "90",
            "89",
            "88",
            "87",
            "86",
            "85",
            "84",
            "83",
            "82",
            "81",
            "80",
            "79",
            "78",
            "77",
            "76",
            "75",
            "74",
            "73",
            "72",
            "71",
            "70",
            "69",
            "68",
            "67",
            "66",
            "65",
            "64",
            "63",
            "62",
            "61",
            "60",
            "59",
            "58",
            "57",
            "56",
            "55",
            "54",
            "53",
            "52",
            "51",
            "50",
            "49",
            "48",
            "47",
            "46",
            "45",
            "44",
            "43",
            "42",
            "41",
            "40",
            "39",
            "38",
            "37",
            "36",
            "35",
            "34",
            "33",
            "32",
            "31",
            "30",
            "29",
            "28",
            "27",
            "26",
            "25",
            "24",
            "23",
            "22",
            "21",
            "20",
            "19",
            "18",
            "17",
            "16",
            "15",
            "14",
            "13",
            "12",
            "11",
            "10",
            "9",
            "8",
            "7",
            "6",
            "5",
            "4",
            "3",
            "2",
            "1"});
            this.MaxCPU.Location = new System.Drawing.Point(588, 104);
            this.MaxCPU.Name = "MaxCPU";
            this.MaxCPU.Size = new System.Drawing.Size(64, 21);
            this.MaxCPU.TabIndex = 6;
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Label3.Location = new System.Drawing.Point(6, 82);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(252, 13);
            this.Label3.TabIndex = 20;
            this.Label3.Text = "Set the voice limit for the driver, from 1 to 100000:";
            // 
            // PolyphonyLimit
            // 
            this.PolyphonyLimit.Location = new System.Drawing.Point(588, 80);
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
            this.PolyphonyLimit.Size = new System.Drawing.Size(64, 21);
            this.PolyphonyLimit.TabIndex = 5;
            this.PolyphonyLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.PolyphonyLimit.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // DisableSFX
            // 
            this.DisableSFX.AutoSize = true;
            this.DisableSFX.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.DisableSFX.Location = new System.Drawing.Point(8, 37);
            this.DisableSFX.Name = "DisableSFX";
            this.DisableSFX.Size = new System.Drawing.Size(585, 17);
            this.DisableSFX.TabIndex = 3;
            this.DisableSFX.Text = "Disable sound effects. (Disable the sound effects, such as reverb and chorus. Als" +
    "o, this can reduce the CPU usage.)";
            this.DisableSFX.UseVisualStyleBackColor = true;
            // 
            // Preload
            // 
            this.Preload.AutoSize = true;
            this.Preload.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.Preload.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Preload.Location = new System.Drawing.Point(8, 17);
            this.Preload.Name = "Preload";
            this.Preload.Size = new System.Drawing.Size(410, 17);
            this.Preload.TabIndex = 2;
            this.Preload.Text = "Enable soundfont preload. (Useful with systems that have limited RAM resources.)";
            this.Preload.UseVisualStyleBackColor = true;
            // 
            // NoteOffCheck
            // 
            this.NoteOffCheck.AutoSize = true;
            this.NoteOffCheck.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.NoteOffCheck.Location = new System.Drawing.Point(8, 57);
            this.NoteOffCheck.Name = "NoteOffCheck";
            this.NoteOffCheck.Size = new System.Drawing.Size(534, 17);
            this.NoteOffCheck.TabIndex = 4;
            this.NoteOffCheck.Text = "Only release the oldest instance upon a note off event when there are overlapping" +
    " instances of the note.";
            this.NoteOffCheck.UseVisualStyleBackColor = true;
            // 
            // VolIntView
            // 
            this.VolIntView.AutoSize = true;
            this.VolIntView.Enabled = false;
            this.VolIntView.Location = new System.Drawing.Point(14, 27);
            this.VolIntView.Name = "VolIntView";
            this.VolIntView.Size = new System.Drawing.Size(70, 13);
            this.VolIntView.TabIndex = 3;
            this.VolIntView.Text = "Real value: X";
            // 
            // VolSimView
            // 
            this.VolSimView.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VolSimView.ForeColor = System.Drawing.Color.Teal;
            this.VolSimView.Location = new System.Drawing.Point(576, 2);
            this.VolSimView.Name = "VolSimView";
            this.VolSimView.Size = new System.Drawing.Size(93, 38);
            this.VolSimView.TabIndex = 2;
            this.VolSimView.Text = "100%";
            this.VolSimView.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.VolSimView.UseCompatibleTextRendering = true;
            // 
            // VolStaticLab
            // 
            this.VolStaticLab.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VolStaticLab.Location = new System.Drawing.Point(455, 2);
            this.VolStaticLab.Name = "VolStaticLab";
            this.VolStaticLab.Size = new System.Drawing.Size(134, 38);
            this.VolStaticLab.TabIndex = 1;
            this.VolStaticLab.Text = "Volume:";
            this.VolStaticLab.UseCompatibleTextRendering = true;
            // 
            // VolTrackBar
            // 
            this.VolTrackBar.BackColor = System.Drawing.Color.White;
            this.VolTrackBar.Location = new System.Drawing.Point(8, 47);
            this.VolTrackBar.Maximum = 10000;
            this.VolTrackBar.Name = "VolTrackBar";
            this.VolTrackBar.Size = new System.Drawing.Size(661, 45);
            this.VolTrackBar.TabIndex = 0;
            this.VolTrackBar.TickFrequency = 0;
            this.VolTrackBar.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.VolTrackBar.Scroll += new System.EventHandler(this.VolTrackBar_Scroll);
            // 
            // menuStrip1
            // 
            this.menuStrip1.AutoSize = false;
            this.menuStrip1.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.menuStrip1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.applySettingsToolStripMenuItem,
            this.resetToDefaultToolStripMenuItem,
            this.settingsPresetsToolStripMenuItem,
            this.assignSoundfontListToAppToolStripMenuItem,
            this.changeDefaultSoundfontListToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 366);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.menuStrip1.Size = new System.Drawing.Size(677, 31);
            this.menuStrip1.TabIndex = 40;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // applySettingsToolStripMenuItem
            // 
            this.applySettingsToolStripMenuItem.Name = "applySettingsToolStripMenuItem";
            this.applySettingsToolStripMenuItem.Size = new System.Drawing.Size(87, 27);
            this.applySettingsToolStripMenuItem.Text = "Apply settings";
            this.applySettingsToolStripMenuItem.Click += new System.EventHandler(this.applySettingsToolStripMenuItem_Click);
            // 
            // resetToDefaultToolStripMenuItem
            // 
            this.resetToDefaultToolStripMenuItem.Name = "resetToDefaultToolStripMenuItem";
            this.resetToDefaultToolStripMenuItem.Size = new System.Drawing.Size(97, 27);
            this.resetToDefaultToolStripMenuItem.Text = "Reset to default";
            this.resetToDefaultToolStripMenuItem.Click += new System.EventHandler(this.resetToDefaultToolStripMenuItem_Click);
            // 
            // settingsPresetsToolStripMenuItem
            // 
            this.settingsPresetsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lowLatencyPresetToolStripMenuItem,
            this.blackMIDIsPresetToolStripMenuItem});
            this.settingsPresetsToolStripMenuItem.Name = "settingsPresetsToolStripMenuItem";
            this.settingsPresetsToolStripMenuItem.Size = new System.Drawing.Size(97, 27);
            this.settingsPresetsToolStripMenuItem.Text = "Settings presets";
            // 
            // lowLatencyPresetToolStripMenuItem
            // 
            this.lowLatencyPresetToolStripMenuItem.Name = "lowLatencyPresetToolStripMenuItem";
            this.lowLatencyPresetToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.lowLatencyPresetToolStripMenuItem.Text = "Low Latency Preset";
            this.lowLatencyPresetToolStripMenuItem.Click += new System.EventHandler(this.lowLatencyPresetToolStripMenuItem_Click);
            // 
            // blackMIDIsPresetToolStripMenuItem
            // 
            this.blackMIDIsPresetToolStripMenuItem.Name = "blackMIDIsPresetToolStripMenuItem";
            this.blackMIDIsPresetToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.blackMIDIsPresetToolStripMenuItem.Text = "Black MIDIs Preset";
            this.blackMIDIsPresetToolStripMenuItem.Click += new System.EventHandler(this.blackMIDIsPresetToolStripMenuItem_Click);
            // 
            // assignSoundfontListToAppToolStripMenuItem
            // 
            this.assignSoundfontListToAppToolStripMenuItem.Name = "assignSoundfontListToAppToolStripMenuItem";
            this.assignSoundfontListToAppToolStripMenuItem.Size = new System.Drawing.Size(152, 27);
            this.assignSoundfontListToAppToolStripMenuItem.Text = "Assign soundfont list to app";
            this.assignSoundfontListToAppToolStripMenuItem.Click += new System.EventHandler(this.assignASoundfontListToASpecificAppToolStripMenuItem_Click);
            // 
            // changeDefaultSoundfontListToolStripMenuItem
            // 
            this.changeDefaultSoundfontListToolStripMenuItem.Name = "changeDefaultSoundfontListToolStripMenuItem";
            this.changeDefaultSoundfontListToolStripMenuItem.Size = new System.Drawing.Size(161, 27);
            this.changeDefaultSoundfontListToolStripMenuItem.Text = "Change default soundfont list";
            this.changeDefaultSoundfontListToolStripMenuItem.Click += new System.EventHandler(this.changeDefaultSoundfontListToolStripMenuItem_Click);
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
            this.ExternalListImport.Filter = "Soundfont lists|*.txt;*.sflist;";
            // 
            // ExternalListExport
            // 
            this.ExternalListExport.Filter = "Soundfont list (.sflist)|*.sflist|Text file (.txt)|*.txt";
            // 
            // VolumeHotkeysCheck
            // 
            this.VolumeHotkeysCheck.Interval = 1;
            this.VolumeHotkeysCheck.Tick += new System.EventHandler(this.VolumeHotkeysCheck_Tick);
            // 
            // MainMenu
            // 
            this.MainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem2,
            this.menuItem3});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.openDebugWindowToolStripMenuItem,
            this.openTheMixerToolStripMenuItem,
            this.openTheBlacklistManagerToolStripMenuItem,
            this.menuItem7,
            this.assignASoundfontListToASpecificAppToolStripMenuItemToolStripMenuItem,
            this.menuItem9,
            this.changeDefaultMIDIOutDeviceToolStripMenuItem1,
            this.changeDefaultMIDIOutDeviceToolStripMenuItem,
            this.changeDefault64bitMIDIOutDeviceToolStripMenuItem,
            this.menuItem13,
            this.exitToolStripMenuItem});
            this.menuItem1.Text = "File";
            // 
            // openDebugWindowToolStripMenuItem
            // 
            this.openDebugWindowToolStripMenuItem.Index = 0;
            this.openDebugWindowToolStripMenuItem.Text = "Open debug window";
            this.openDebugWindowToolStripMenuItem.Click += new System.EventHandler(this.openDebugWindowToolStripMenuItem_Click);
            // 
            // openTheMixerToolStripMenuItem
            // 
            this.openTheMixerToolStripMenuItem.Index = 1;
            this.openTheMixerToolStripMenuItem.Text = "Open the mixer";
            this.openTheMixerToolStripMenuItem.Click += new System.EventHandler(this.openTheMixerToolStripMenuItem_Click);
            // 
            // openTheBlacklistManagerToolStripMenuItem
            // 
            this.openTheBlacklistManagerToolStripMenuItem.Index = 2;
            this.openTheBlacklistManagerToolStripMenuItem.Text = "Open the blacklist manager";
            this.openTheBlacklistManagerToolStripMenuItem.Click += new System.EventHandler(this.openTheBlacklistManagerToolStripMenuItem_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 3;
            this.menuItem7.Text = "-";
            // 
            // assignASoundfontListToASpecificAppToolStripMenuItemToolStripMenuItem
            // 
            this.assignASoundfontListToASpecificAppToolStripMenuItemToolStripMenuItem.Index = 4;
            this.assignASoundfontListToASpecificAppToolStripMenuItemToolStripMenuItem.Text = "Assign a soundfont list to a specific app";
            this.assignASoundfontListToASpecificAppToolStripMenuItemToolStripMenuItem.Click += new System.EventHandler(this.assignASoundfontListToASpecificAppToolStripMenuItem_Click);
            // 
            // menuItem9
            // 
            this.menuItem9.Index = 5;
            this.menuItem9.Text = "-";
            // 
            // changeDefaultMIDIOutDeviceToolStripMenuItem1
            // 
            this.changeDefaultMIDIOutDeviceToolStripMenuItem1.Index = 6;
            this.changeDefaultMIDIOutDeviceToolStripMenuItem1.Text = "Change default MIDI out device";
            this.changeDefaultMIDIOutDeviceToolStripMenuItem1.Click += new System.EventHandler(this.changeDefaultMIDIOutDeviceToolStripMenuItem1_Click);
            // 
            // changeDefaultMIDIOutDeviceToolStripMenuItem
            // 
            this.changeDefaultMIDIOutDeviceToolStripMenuItem.Index = 7;
            this.changeDefaultMIDIOutDeviceToolStripMenuItem.Text = "Change default 32-bit MIDI out device";
            this.changeDefaultMIDIOutDeviceToolStripMenuItem.Click += new System.EventHandler(this.changeDefaultMIDIOutDeviceToolStripMenuItem1_Click);
            // 
            // changeDefault64bitMIDIOutDeviceToolStripMenuItem
            // 
            this.changeDefault64bitMIDIOutDeviceToolStripMenuItem.Index = 8;
            this.changeDefault64bitMIDIOutDeviceToolStripMenuItem.Text = "Change default 64-bit MIDI out device";
            this.changeDefault64bitMIDIOutDeviceToolStripMenuItem.Click += new System.EventHandler(this.changeDefault64bitMIDIOutDeviceToolStripMenuItem_Click);
            // 
            // menuItem13
            // 
            this.menuItem13.Index = 9;
            this.menuItem13.Text = "-";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Index = 10;
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 1;
            this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem38,
            this.killTheWatchdogToolStripMenuItem,
            this.menuItem17,
            this.manageFolderFavouritesToolStripMenuItem,
            this.menuItem19,
            this.menuItem20,
            this.menuItem21,
            this.changeTheSizeOfTheEVBufferToolStripMenuItem,
            this.changeDirectoryOfTheOutputToWAVModeToolStripMenuItem,
            this.changeTheMaximumSamplesPerFrameToolStripMenuItem,
            this.changeDefaultSoundfontListToolStripMenuItem1,
            this.menuItem26,
            this.menuItem27,
            this.menuItem28});
            this.menuItem2.Text = "Advanced settings";
            // 
            // menuItem38
            // 
            this.menuItem38.Index = 0;
            this.menuItem38.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.checkEnabledToolStripMenuItem,
            this.checkDisabledToolStripMenuItem});
            this.menuItem38.Text = "Automatically check for updates when starting the configurator";
            // 
            // checkEnabledToolStripMenuItem
            // 
            this.checkEnabledToolStripMenuItem.Index = 0;
            this.checkEnabledToolStripMenuItem.Text = "Enabled";
            this.checkEnabledToolStripMenuItem.Click += new System.EventHandler(this.checkEnabledToolStripMenuItem_Click);
            // 
            // checkDisabledToolStripMenuItem
            // 
            this.checkDisabledToolStripMenuItem.Index = 1;
            this.checkDisabledToolStripMenuItem.Text = "Disabled";
            this.checkDisabledToolStripMenuItem.Click += new System.EventHandler(this.checkDisabledToolStripMenuItem_Click);
            // 
            // killTheWatchdogToolStripMenuItem
            // 
            this.killTheWatchdogToolStripMenuItem.Index = 1;
            this.killTheWatchdogToolStripMenuItem.Text = "Kill the Watchdog (Useful when it gets stuck)";
            this.killTheWatchdogToolStripMenuItem.Click += new System.EventHandler(this.killTheWatchdogToolStripMenuItem_Click);
            // 
            // menuItem17
            // 
            this.menuItem17.Index = 2;
            this.menuItem17.Text = "-";
            // 
            // manageFolderFavouritesToolStripMenuItem
            // 
            this.manageFolderFavouritesToolStripMenuItem.Index = 3;
            this.manageFolderFavouritesToolStripMenuItem.Text = "Manage folder favourites";
            this.manageFolderFavouritesToolStripMenuItem.Click += new System.EventHandler(this.manageFolderFavouritesToolStripMenuItem_Click);
            // 
            // menuItem19
            // 
            this.menuItem19.Index = 4;
            this.menuItem19.Text = "-";
            // 
            // menuItem20
            // 
            this.menuItem20.Index = 5;
            this.menuItem20.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.enabledToolStripMenuItem2,
            this.disabledToolStripMenuItem2});
            this.menuItem20.Text = "Enable extra 8 soundfont lists";
            // 
            // enabledToolStripMenuItem2
            // 
            this.enabledToolStripMenuItem2.Index = 0;
            this.enabledToolStripMenuItem2.Text = "Enabled";
            this.enabledToolStripMenuItem2.Click += new System.EventHandler(this.enabledToolStripMenuItem2_Click);
            // 
            // disabledToolStripMenuItem2
            // 
            this.disabledToolStripMenuItem2.Index = 1;
            this.disabledToolStripMenuItem2.Text = "Disabled";
            this.disabledToolStripMenuItem2.Click += new System.EventHandler(this.disabledToolStripMenuItem2_Click);
            // 
            // menuItem21
            // 
            this.menuItem21.Index = 6;
            this.menuItem21.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.enabledToolStripMenuItem3,
            this.disabledToolStripMenuItem3});
            this.menuItem21.Text = "Use the old buffer system (No slowdowns)";
            // 
            // enabledToolStripMenuItem3
            // 
            this.enabledToolStripMenuItem3.Index = 0;
            this.enabledToolStripMenuItem3.Text = "Enabled";
            this.enabledToolStripMenuItem3.Click += new System.EventHandler(this.enabledToolStripMenuItem3_Click);
            // 
            // disabledToolStripMenuItem3
            // 
            this.disabledToolStripMenuItem3.Index = 1;
            this.disabledToolStripMenuItem3.Text = "Disabled";
            this.disabledToolStripMenuItem3.Click += new System.EventHandler(this.disabledToolStripMenuItem3_Click);
            // 
            // changeTheSizeOfTheEVBufferToolStripMenuItem
            // 
            this.changeTheSizeOfTheEVBufferToolStripMenuItem.Index = 7;
            this.changeTheSizeOfTheEVBufferToolStripMenuItem.Text = "Change the size of the EV buffer";
            this.changeTheSizeOfTheEVBufferToolStripMenuItem.Click += new System.EventHandler(this.changeTheSizeOfTheEVBufferToolStripMenuItem_Click);
            // 
            // changeDirectoryOfTheOutputToWAVModeToolStripMenuItem
            // 
            this.changeDirectoryOfTheOutputToWAVModeToolStripMenuItem.Index = 8;
            this.changeDirectoryOfTheOutputToWAVModeToolStripMenuItem.Text = "Change directory of the \"Output to WAV\" mode";
            this.changeDirectoryOfTheOutputToWAVModeToolStripMenuItem.Click += new System.EventHandler(this.changeDirectoryOfTheOutputToWAVModeToolStripMenuItem_Click);
            // 
            // changeTheMaximumSamplesPerFrameToolStripMenuItem
            // 
            this.changeTheMaximumSamplesPerFrameToolStripMenuItem.Index = 9;
            this.changeTheMaximumSamplesPerFrameToolStripMenuItem.Text = "Change the maximum samples per frame";
            this.changeTheMaximumSamplesPerFrameToolStripMenuItem.Click += new System.EventHandler(this.changeTheMaximumSamplesPerFrameToolStripMenuItem_Click);
            // 
            // changeDefaultSoundfontListToolStripMenuItem1
            // 
            this.changeDefaultSoundfontListToolStripMenuItem1.Index = 10;
            this.changeDefaultSoundfontListToolStripMenuItem1.Text = "Change default soundfont list";
            this.changeDefaultSoundfontListToolStripMenuItem1.Click += new System.EventHandler(this.changeDefaultSoundfontListToolStripMenuItem1_Click);
            // 
            // menuItem26
            // 
            this.menuItem26.Index = 11;
            this.menuItem26.Text = "-";
            // 
            // menuItem27
            // 
            this.menuItem27.Index = 12;
            this.menuItem27.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem15,
            this.menuItem16,
            this.soundfontListChangeConfirmationDialogToolStripMenuItem,
            this.volumeHotkeysToolStripMenuItem});
            this.menuItem27.Text = "Hotkeys";
            // 
            // menuItem15
            // 
            this.menuItem15.Index = 0;
            this.menuItem15.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.hLSEnabledToolStripMenuItem,
            this.hLSDisabledToolStripMenuItem});
            this.menuItem15.Text = "Hotkeys and list switching";
            // 
            // hLSEnabledToolStripMenuItem
            // 
            this.hLSEnabledToolStripMenuItem.Index = 0;
            this.hLSEnabledToolStripMenuItem.Text = "Enabled";
            this.hLSEnabledToolStripMenuItem.Click += new System.EventHandler(this.hLSEnabledToolStripMenuItem_Click);
            // 
            // hLSDisabledToolStripMenuItem
            // 
            this.hLSDisabledToolStripMenuItem.Index = 1;
            this.hLSDisabledToolStripMenuItem.Text = "Disabled";
            this.hLSDisabledToolStripMenuItem.Click += new System.EventHandler(this.hLSDisabledToolStripMenuItem_Click);
            // 
            // menuItem16
            // 
            this.menuItem16.Index = 1;
            this.menuItem16.Text = "-";
            // 
            // soundfontListChangeConfirmationDialogToolStripMenuItem
            // 
            this.soundfontListChangeConfirmationDialogToolStripMenuItem.Index = 2;
            this.soundfontListChangeConfirmationDialogToolStripMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.enabledToolStripMenuItem,
            this.disabledToolStripMenuItem});
            this.soundfontListChangeConfirmationDialogToolStripMenuItem.Text = "Soundfont list change confirmation dialog";
            // 
            // enabledToolStripMenuItem
            // 
            this.enabledToolStripMenuItem.Index = 0;
            this.enabledToolStripMenuItem.Text = "Enabled";
            this.enabledToolStripMenuItem.Click += new System.EventHandler(this.SFListConfirmationenabledToolStripMenuItem_Click);
            // 
            // disabledToolStripMenuItem
            // 
            this.disabledToolStripMenuItem.Index = 1;
            this.disabledToolStripMenuItem.Text = "Disabled";
            this.disabledToolStripMenuItem.Click += new System.EventHandler(this.SFListConfirmationdisabledToolStripMenuItem_Click);
            // 
            // volumeHotkeysToolStripMenuItem
            // 
            this.volumeHotkeysToolStripMenuItem.Index = 3;
            this.volumeHotkeysToolStripMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.enabledToolStripMenuItem1,
            this.disabledToolStripMenuItem1});
            this.volumeHotkeysToolStripMenuItem.Text = "Volume hotkeys";
            // 
            // enabledToolStripMenuItem1
            // 
            this.enabledToolStripMenuItem1.Index = 0;
            this.enabledToolStripMenuItem1.Text = "Enabled";
            this.enabledToolStripMenuItem1.Click += new System.EventHandler(this.enabledToolStripMenuItem1_Click);
            // 
            // disabledToolStripMenuItem1
            // 
            this.disabledToolStripMenuItem1.Index = 1;
            this.disabledToolStripMenuItem1.Text = "Disabled";
            this.disabledToolStripMenuItem1.Click += new System.EventHandler(this.disabledToolStripMenuItem1_Click);
            // 
            // menuItem28
            // 
            this.menuItem28.Index = 13;
            this.menuItem28.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.watchdogEnabledToolStripMenuItem,
            this.watchdogDisabledToolStripMenuItem});
            this.menuItem28.Text = "Watchdog";
            // 
            // watchdogEnabledToolStripMenuItem
            // 
            this.watchdogEnabledToolStripMenuItem.Index = 0;
            this.watchdogEnabledToolStripMenuItem.Text = "Enabled";
            this.watchdogEnabledToolStripMenuItem.Click += new System.EventHandler(this.watchdogEnabledToolStripMenuItem_Click);
            // 
            // watchdogDisabledToolStripMenuItem
            // 
            this.watchdogDisabledToolStripMenuItem.Index = 1;
            this.watchdogDisabledToolStripMenuItem.Text = "Disabled";
            this.watchdogDisabledToolStripMenuItem.Click += new System.EventHandler(this.watchdogDisabledToolStripMenuItem_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 2;
            this.menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.informationAboutTheDriverToolStripMenuItem,
            this.openUpdaterToolStripMenuItem,
            this.reportABugToolStripMenuItem,
            this.getTheMIDIMapperForWindows10ToolStripMenuItem,
            this.menuItem42,
            this.guidesToolStripMenuItem,
            this.menuItem44,
            this.donateToSupportUsToolStripMenuItem,
            this.downloadTheSourceCodeToolStripMenuItem});
            this.menuItem3.Text = "Help";
            // 
            // informationAboutTheDriverToolStripMenuItem
            // 
            this.informationAboutTheDriverToolStripMenuItem.Index = 0;
            this.informationAboutTheDriverToolStripMenuItem.Text = "Information about the driver";
            this.informationAboutTheDriverToolStripMenuItem.Click += new System.EventHandler(this.informationAboutTheDriverToolStripMenuItem_Click);
            // 
            // openUpdaterToolStripMenuItem
            // 
            this.openUpdaterToolStripMenuItem.Index = 1;
            this.openUpdaterToolStripMenuItem.Text = "Check for updates";
            this.openUpdaterToolStripMenuItem.Click += new System.EventHandler(this.openUpdaterToolStripMenuItem_Click);
            // 
            // reportABugToolStripMenuItem
            // 
            this.reportABugToolStripMenuItem.Index = 2;
            this.reportABugToolStripMenuItem.Text = "Report a bug";
            this.reportABugToolStripMenuItem.Click += new System.EventHandler(this.reportABugToolStripMenuItem_Click);
            // 
            // getTheMIDIMapperForWindows10ToolStripMenuItem
            // 
            this.getTheMIDIMapperForWindows10ToolStripMenuItem.Index = 3;
            this.getTheMIDIMapperForWindows10ToolStripMenuItem.Text = "Get the MIDI-Mapper for Windows 10";
            this.getTheMIDIMapperForWindows10ToolStripMenuItem.Click += new System.EventHandler(this.getTheMIDIMapperForWindows10ToolStripMenuItem_Click);
            // 
            // menuItem42
            // 
            this.menuItem42.Index = 4;
            this.menuItem42.Text = "-";
            // 
            // guidesToolStripMenuItem
            // 
            this.guidesToolStripMenuItem.Index = 5;
            this.guidesToolStripMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.isThereAnyShortcutForToOpenTheConfiguratorToolStripMenuItem,
            this.whatAreTheHotkeysToChangeTheVolumeToolStripMenuItem,
            this.howCanIChangeTheSoundfontListToolStripMenuItem,
            this.whatsTheBestSettingsForTheBufferToolStripMenuItem,
            this.howCanIResetTheDriverToolStripMenuItem});
            this.guidesToolStripMenuItem.Text = "Guides";
            // 
            // isThereAnyShortcutForToOpenTheConfiguratorToolStripMenuItem
            // 
            this.isThereAnyShortcutForToOpenTheConfiguratorToolStripMenuItem.Index = 0;
            this.isThereAnyShortcutForToOpenTheConfiguratorToolStripMenuItem.Text = "What are the hotkeys to open the configurator?";
            this.isThereAnyShortcutForToOpenTheConfiguratorToolStripMenuItem.Click += new System.EventHandler(this.isThereAnyShortcutForToOpenTheConfiguratorToolStripMenuItem_Click);
            // 
            // whatAreTheHotkeysToChangeTheVolumeToolStripMenuItem
            // 
            this.whatAreTheHotkeysToChangeTheVolumeToolStripMenuItem.Index = 1;
            this.whatAreTheHotkeysToChangeTheVolumeToolStripMenuItem.Text = "What are the hotkeys to change the volume?";
            this.whatAreTheHotkeysToChangeTheVolumeToolStripMenuItem.Click += new System.EventHandler(this.whatAreTheHotkeysToChangeTheVolumeToolStripMenuItem_Click);
            // 
            // howCanIChangeTheSoundfontListToolStripMenuItem
            // 
            this.howCanIChangeTheSoundfontListToolStripMenuItem.Index = 2;
            this.howCanIChangeTheSoundfontListToolStripMenuItem.Text = "How can I change the soundfont list?";
            this.howCanIChangeTheSoundfontListToolStripMenuItem.Click += new System.EventHandler(this.howCanIChangeTheSoundfontListToolStripMenuItem_Click);
            // 
            // whatsTheBestSettingsForTheBufferToolStripMenuItem
            // 
            this.whatsTheBestSettingsForTheBufferToolStripMenuItem.Index = 3;
            this.whatsTheBestSettingsForTheBufferToolStripMenuItem.Text = "What are the best settings for the buffer?";
            this.whatsTheBestSettingsForTheBufferToolStripMenuItem.Click += new System.EventHandler(this.whatsTheBestSettingsForTheBufferToolStripMenuItem_Click);
            // 
            // howCanIResetTheDriverToolStripMenuItem
            // 
            this.howCanIResetTheDriverToolStripMenuItem.Index = 4;
            this.howCanIResetTheDriverToolStripMenuItem.Text = "How can I reset the driver?";
            this.howCanIResetTheDriverToolStripMenuItem.Click += new System.EventHandler(this.howCanIResetTheDriverToolStripMenuItem_Click);
            // 
            // menuItem44
            // 
            this.menuItem44.Index = 6;
            this.menuItem44.Text = "-";
            // 
            // donateToSupportUsToolStripMenuItem
            // 
            this.donateToSupportUsToolStripMenuItem.Index = 7;
            this.donateToSupportUsToolStripMenuItem.Text = "Donate to support us";
            this.donateToSupportUsToolStripMenuItem.Click += new System.EventHandler(this.donateToSupportUsToolStripMenuItem_Click);
            // 
            // downloadTheSourceCodeToolStripMenuItem
            // 
            this.downloadTheSourceCodeToolStripMenuItem.Index = 8;
            this.downloadTheSourceCodeToolStripMenuItem.Text = "Download the source code";
            this.downloadTheSourceCodeToolStripMenuItem.Click += new System.EventHandler(this.downloadTheSourceCodeToolStripMenuItem_Click);
            // 
            // KeppySynthConfiguratorMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(685, 423);
            this.Controls.Add(this.TabsForTheControls);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Menu = this.MainMenu;
            this.Name = "KeppySynthConfiguratorMain";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Keppy\'s Synthesizer ~ Configurator";
            this.Load += new System.EventHandler(this.KeppySynthConfiguratorMain_Load);
            this.TabsForTheControls.ResumeLayout(false);
            this.List.ResumeLayout(false);
            this.List.PerformLayout();
            this.IELPan1.ResumeLayout(false);
            this.Settings.ResumeLayout(false);
            this.Settings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WhatIsXAudio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WhatIsOutput)).EndInit();
            this.GroupBox5.ResumeLayout(false);
            this.GroupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TracksLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bufsize)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PolyphonyLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VolTrackBar)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl TabsForTheControls;
        private System.Windows.Forms.TabPage List;
        private System.Windows.Forms.TabPage Settings;
        private System.Windows.Forms.Label VolIntView;
        private System.Windows.Forms.Label VolSimView;
        private System.Windows.Forms.Label VolStaticLab;
        private System.Windows.Forms.TrackBar VolTrackBar;
        internal System.Windows.Forms.GroupBox GroupBox5;
        internal System.Windows.Forms.CheckBox SincInter;
        internal System.Windows.Forms.NumericUpDown TracksLimit;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label BufferText;
        internal System.Windows.Forms.CheckBox SysResetIgnore;
        internal System.Windows.Forms.NumericUpDown bufsize;
        private System.Windows.Forms.GroupBox groupBox1;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.ComboBox Frequency;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.ComboBox MaxCPU;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.NumericUpDown PolyphonyLimit;
        internal System.Windows.Forms.CheckBox DisableSFX;
        internal System.Windows.Forms.CheckBox Preload;
        internal System.Windows.Forms.CheckBox NoteOffCheck;
        private System.Windows.Forms.OpenFileDialog SoundfontImport;
        private System.Windows.Forms.OpenFileDialog ExternalListImport;
        private System.Windows.Forms.CheckBox OutputWAV;
        private System.Windows.Forms.PictureBox WhatIsOutput;
        private System.Windows.Forms.CheckBox XAudioDisable;
        private System.Windows.Forms.PictureBox WhatIsXAudio;
        private System.Windows.Forms.CheckBox VMSEmu;
        private System.Windows.Forms.SaveFileDialog ExternalListExport;
        private System.Windows.Forms.LinkLabel SPFSecondaryBut;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem applySettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetToDefaultToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsPresetsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lowLatencyPresetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blackMIDIsPresetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeDefaultSoundfontListToolStripMenuItem;
        private System.Windows.Forms.Timer VolumeHotkeysCheck;
        private System.Windows.Forms.ToolStripMenuItem assignSoundfontListToAppToolStripMenuItem;
        private System.Windows.Forms.ComboBox SelectedListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button DisableSF;
        private System.Windows.Forms.Button EnableSF;
        private System.Windows.Forms.Label List1Override;
        private System.Windows.Forms.Panel IELPan1;
        private System.Windows.Forms.Button EL;
        private System.Windows.Forms.Button IEL;
        private System.Windows.Forms.Button CLi;
        private System.Windows.Forms.Button MvD;
        private System.Windows.Forms.Button MvU;
        private System.Windows.Forms.Button RmvSF;
        private System.Windows.Forms.Button AddSF;
        private System.Windows.Forms.ListBox Lis;
        private System.Windows.Forms.MainMenu MainMenu;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem openDebugWindowToolStripMenuItem;
        private System.Windows.Forms.MenuItem openTheMixerToolStripMenuItem;
        private System.Windows.Forms.MenuItem openTheBlacklistManagerToolStripMenuItem;
        private System.Windows.Forms.MenuItem menuItem7;
        private System.Windows.Forms.MenuItem assignASoundfontListToASpecificAppToolStripMenuItemToolStripMenuItem;
        private System.Windows.Forms.MenuItem menuItem9;
        private System.Windows.Forms.MenuItem changeDefaultMIDIOutDeviceToolStripMenuItem1;
        private System.Windows.Forms.MenuItem changeDefaultMIDIOutDeviceToolStripMenuItem;
        private System.Windows.Forms.MenuItem changeDefault64bitMIDIOutDeviceToolStripMenuItem;
        private System.Windows.Forms.MenuItem menuItem13;
        private System.Windows.Forms.MenuItem exitToolStripMenuItem;
        private System.Windows.Forms.MenuItem menuItem38;
        private System.Windows.Forms.MenuItem checkEnabledToolStripMenuItem;
        private System.Windows.Forms.MenuItem checkDisabledToolStripMenuItem;
        private System.Windows.Forms.MenuItem killTheWatchdogToolStripMenuItem;
        private System.Windows.Forms.MenuItem menuItem17;
        private System.Windows.Forms.MenuItem manageFolderFavouritesToolStripMenuItem;
        private System.Windows.Forms.MenuItem menuItem19;
        private System.Windows.Forms.MenuItem menuItem20;
        private System.Windows.Forms.MenuItem enabledToolStripMenuItem2;
        private System.Windows.Forms.MenuItem disabledToolStripMenuItem2;
        private System.Windows.Forms.MenuItem menuItem21;
        private System.Windows.Forms.MenuItem enabledToolStripMenuItem3;
        private System.Windows.Forms.MenuItem disabledToolStripMenuItem3;
        private System.Windows.Forms.MenuItem changeTheSizeOfTheEVBufferToolStripMenuItem;
        private System.Windows.Forms.MenuItem changeDirectoryOfTheOutputToWAVModeToolStripMenuItem;
        private System.Windows.Forms.MenuItem changeTheMaximumSamplesPerFrameToolStripMenuItem;
        private System.Windows.Forms.MenuItem changeDefaultSoundfontListToolStripMenuItem1;
        private System.Windows.Forms.MenuItem menuItem26;
        private System.Windows.Forms.MenuItem menuItem27;
        private System.Windows.Forms.MenuItem menuItem28;
        private System.Windows.Forms.MenuItem watchdogEnabledToolStripMenuItem;
        private System.Windows.Forms.MenuItem watchdogDisabledToolStripMenuItem;
        private System.Windows.Forms.MenuItem informationAboutTheDriverToolStripMenuItem;
        private System.Windows.Forms.MenuItem openUpdaterToolStripMenuItem;
        private System.Windows.Forms.MenuItem reportABugToolStripMenuItem;
        private System.Windows.Forms.MenuItem getTheMIDIMapperForWindows10ToolStripMenuItem;
        private System.Windows.Forms.MenuItem menuItem42;
        private System.Windows.Forms.MenuItem guidesToolStripMenuItem;
        private System.Windows.Forms.MenuItem isThereAnyShortcutForToOpenTheConfiguratorToolStripMenuItem;
        private System.Windows.Forms.MenuItem whatAreTheHotkeysToChangeTheVolumeToolStripMenuItem;
        private System.Windows.Forms.MenuItem howCanIChangeTheSoundfontListToolStripMenuItem;
        private System.Windows.Forms.MenuItem whatsTheBestSettingsForTheBufferToolStripMenuItem;
        private System.Windows.Forms.MenuItem howCanIResetTheDriverToolStripMenuItem;
        private System.Windows.Forms.MenuItem menuItem44;
        private System.Windows.Forms.MenuItem donateToSupportUsToolStripMenuItem;
        private System.Windows.Forms.MenuItem downloadTheSourceCodeToolStripMenuItem;
        private System.Windows.Forms.MenuItem menuItem15;
        private System.Windows.Forms.MenuItem hLSEnabledToolStripMenuItem;
        private System.Windows.Forms.MenuItem hLSDisabledToolStripMenuItem;
        private System.Windows.Forms.MenuItem menuItem16;
        private System.Windows.Forms.MenuItem soundfontListChangeConfirmationDialogToolStripMenuItem;
        private System.Windows.Forms.MenuItem enabledToolStripMenuItem;
        private System.Windows.Forms.MenuItem disabledToolStripMenuItem;
        private System.Windows.Forms.MenuItem volumeHotkeysToolStripMenuItem;
        private System.Windows.Forms.MenuItem enabledToolStripMenuItem1;
        private System.Windows.Forms.MenuItem disabledToolStripMenuItem1;
    }
}

