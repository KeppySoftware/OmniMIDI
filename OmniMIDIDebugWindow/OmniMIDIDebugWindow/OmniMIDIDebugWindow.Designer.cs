namespace OmniMIDIDebugWindow
{
    partial class OmniMIDIDebugWindow
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
            try
            {
                base.Dispose(disposing);
            }
            catch
            {
                System.Windows.Forms.Application.Exit();
            }
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OmniMIDIDebugWindow));
            this.MainCont = new System.Windows.Forms.ContextMenu();
            this.OpenConfigurator = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.OpenAppLocat = new System.Windows.Forms.MenuItem();
            this.DebugWinTop = new System.Windows.Forms.MenuItem();
            this.CopyToClipboard = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.ExitMenu = new System.Windows.Forms.MenuItem();
            this.Tabs = new System.Windows.Forms.TabControl();
            this.SynthDbg = new System.Windows.Forms.TabPage();
            this.CurSFsList = new System.Windows.Forms.Label();
            this.CurSFsListLabel = new System.Windows.Forms.Label();
            this.KDMAPI = new System.Windows.Forms.Label();
            this.KDMAPILabel = new System.Windows.Forms.Label();
            this.Latency = new System.Windows.Forms.Label();
            this.LatencyLabel = new System.Windows.Forms.Label();
            this.HeadsPos = new System.Windows.Forms.Label();
            this.HeadsPosLabel = new System.Windows.Forms.Label();
            this.HCountV = new System.Windows.Forms.Label();
            this.HCountVLabel = new System.Windows.Forms.Label();
            this.RAMUsageV = new System.Windows.Forms.Label();
            this.RAMUsageVLabel = new System.Windows.Forms.Label();
            this.RT = new System.Windows.Forms.Label();
            this.RTLabel = new System.Windows.Forms.Label();
            this.AV = new System.Windows.Forms.Label();
            this.AVLabel = new System.Windows.Forms.Label();
            this.CMA = new System.Windows.Forms.Label();
            this.CMALabel = new System.Windows.Forms.Label();
            this.ChannelVoices = new System.Windows.Forms.TabPage();
            this.CHV16 = new System.Windows.Forms.Label();
            this.CHV16L = new System.Windows.Forms.Label();
            this.CHV15 = new System.Windows.Forms.Label();
            this.CHV15L = new System.Windows.Forms.Label();
            this.CHV14 = new System.Windows.Forms.Label();
            this.CHV14L = new System.Windows.Forms.Label();
            this.CHV13 = new System.Windows.Forms.Label();
            this.CHV13L = new System.Windows.Forms.Label();
            this.CHV12 = new System.Windows.Forms.Label();
            this.CHV12L = new System.Windows.Forms.Label();
            this.CHV11 = new System.Windows.Forms.Label();
            this.CHV11L = new System.Windows.Forms.Label();
            this.CHV10 = new System.Windows.Forms.Label();
            this.CHV10L = new System.Windows.Forms.Label();
            this.CHV9 = new System.Windows.Forms.Label();
            this.CHV9L = new System.Windows.Forms.Label();
            this.CHV8 = new System.Windows.Forms.Label();
            this.CHV8L = new System.Windows.Forms.Label();
            this.CHV7 = new System.Windows.Forms.Label();
            this.CHV7L = new System.Windows.Forms.Label();
            this.CHV6 = new System.Windows.Forms.Label();
            this.CHV6L = new System.Windows.Forms.Label();
            this.CHV5 = new System.Windows.Forms.Label();
            this.CHV5L = new System.Windows.Forms.Label();
            this.CHV4 = new System.Windows.Forms.Label();
            this.CHV4L = new System.Windows.Forms.Label();
            this.CHV3 = new System.Windows.Forms.Label();
            this.CHV3L = new System.Windows.Forms.Label();
            this.CHV2 = new System.Windows.Forms.Label();
            this.CHV2L = new System.Windows.Forms.Label();
            this.CHV1 = new System.Windows.Forms.Label();
            this.CHV1L = new System.Windows.Forms.Label();
            this.PCSpecs = new System.Windows.Forms.TabPage();
            this.CPULogo = new System.Windows.Forms.PictureBox();
            this.MT = new System.Windows.Forms.Label();
            this.MTLabel = new System.Windows.Forms.Label();
            this.GPUInternalChip = new System.Windows.Forms.Label();
            this.GPUInternalChipLabel = new System.Windows.Forms.Label();
            this.GPUInfo = new System.Windows.Forms.Label();
            this.GPUInfoLabel = new System.Windows.Forms.Label();
            this.GPU = new System.Windows.Forms.Label();
            this.GPULabel = new System.Windows.Forms.Label();
            this.CPUInfo = new System.Windows.Forms.Label();
            this.CPUInfoLabel = new System.Windows.Forms.Label();
            this.CPU = new System.Windows.Forms.Label();
            this.CPULabel = new System.Windows.Forms.Label();
            this.AM = new System.Windows.Forms.Label();
            this.AMLabel = new System.Windows.Forms.Label();
            this.TM = new System.Windows.Forms.Label();
            this.TMLabel = new System.Windows.Forms.Label();
            this.COS = new System.Windows.Forms.Label();
            this.COSLabel = new System.Windows.Forms.Label();
            this.WinLogo = new System.Windows.Forms.PictureBox();
            this.SelectDebugPipe = new System.Windows.Forms.Button();
            this.VersionLabel = new System.Windows.Forms.Label();
            this.CopyToClip1 = new System.Windows.Forms.Button();
            this.KSLogo = new System.Windows.Forms.PictureBox();
            this.WinLogoTT = new System.Windows.Forms.ToolTip(this.components);
            this.CPULogoTT = new System.Windows.Forms.ToolTip(this.components);
            this.VoiceAverage = new System.Windows.Forms.ToolTip(this.components);
            this.DebugInfo = new System.Windows.Forms.Timer(this.components);
            this.DebugInfoCheck = new System.ComponentModel.BackgroundWorker();
            this.ReloadDebugInfo = new System.Windows.Forms.ToolTip(this.components);
            this.CheckMem = new System.ComponentModel.BackgroundWorker();
            this.Tabs.SuspendLayout();
            this.SynthDbg.SuspendLayout();
            this.ChannelVoices.SuspendLayout();
            this.PCSpecs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CPULogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WinLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.KSLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // MainCont
            // 
            this.MainCont.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.OpenConfigurator,
            this.menuItem3,
            this.OpenAppLocat,
            this.DebugWinTop,
            this.CopyToClipboard,
            this.menuItem1,
            this.ExitMenu});
            // 
            // OpenConfigurator
            // 
            this.OpenConfigurator.Index = 0;
            this.OpenConfigurator.Text = "Open the configurator";
            this.OpenConfigurator.Click += new System.EventHandler(this.OpenConfigurator_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 1;
            this.menuItem3.Text = "-";
            // 
            // OpenAppLocat
            // 
            this.OpenAppLocat.Index = 2;
            this.OpenAppLocat.Text = "Open app location";
            this.OpenAppLocat.Click += new System.EventHandler(this.OpenAppLocat_Click);
            // 
            // DebugWinTop
            // 
            this.DebugWinTop.Index = 3;
            this.DebugWinTop.Text = "Keep debug window on top";
            this.DebugWinTop.Click += new System.EventHandler(this.DebugWinTop_Click);
            // 
            // CopyToClipboard
            // 
            this.CopyToClipboard.Index = 4;
            this.CopyToClipboard.Text = "Copy info from all tabs to clipboard";
            this.CopyToClipboard.Click += new System.EventHandler(this.CopyToClip_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 5;
            this.menuItem1.Text = "-";
            // 
            // ExitMenu
            // 
            this.ExitMenu.Index = 6;
            this.ExitMenu.Text = "Exit";
            this.ExitMenu.Click += new System.EventHandler(this.Exit_Click);
            // 
            // Tabs
            // 
            this.Tabs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Tabs.Controls.Add(this.SynthDbg);
            this.Tabs.Controls.Add(this.ChannelVoices);
            this.Tabs.Controls.Add(this.PCSpecs);
            this.Tabs.Location = new System.Drawing.Point(0, 0);
            this.Tabs.Name = "Tabs";
            this.Tabs.SelectedIndex = 0;
            this.Tabs.Size = new System.Drawing.Size(432, 196);
            this.Tabs.TabIndex = 8;
            // 
            // SynthDbg
            // 
            this.SynthDbg.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.SynthDbg.Controls.Add(this.CurSFsList);
            this.SynthDbg.Controls.Add(this.CurSFsListLabel);
            this.SynthDbg.Controls.Add(this.KDMAPI);
            this.SynthDbg.Controls.Add(this.KDMAPILabel);
            this.SynthDbg.Controls.Add(this.Latency);
            this.SynthDbg.Controls.Add(this.LatencyLabel);
            this.SynthDbg.Controls.Add(this.HeadsPos);
            this.SynthDbg.Controls.Add(this.HeadsPosLabel);
            this.SynthDbg.Controls.Add(this.HCountV);
            this.SynthDbg.Controls.Add(this.HCountVLabel);
            this.SynthDbg.Controls.Add(this.RAMUsageV);
            this.SynthDbg.Controls.Add(this.RAMUsageVLabel);
            this.SynthDbg.Controls.Add(this.RT);
            this.SynthDbg.Controls.Add(this.RTLabel);
            this.SynthDbg.Controls.Add(this.AV);
            this.SynthDbg.Controls.Add(this.AVLabel);
            this.SynthDbg.Controls.Add(this.CMA);
            this.SynthDbg.Controls.Add(this.CMALabel);
            this.SynthDbg.Location = new System.Drawing.Point(4, 22);
            this.SynthDbg.Name = "SynthDbg";
            this.SynthDbg.Padding = new System.Windows.Forms.Padding(3);
            this.SynthDbg.Size = new System.Drawing.Size(424, 170);
            this.SynthDbg.TabIndex = 0;
            this.SynthDbg.Text = "Synth debug info";
            // 
            // CurSFsList
            // 
            this.CurSFsList.AutoSize = true;
            this.CurSFsList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CurSFsList.Location = new System.Drawing.Point(186, 150);
            this.CurSFsList.Name = "CurSFsList";
            this.CurSFsList.Size = new System.Drawing.Size(32, 13);
            this.CurSFsList.TabIndex = 49;
            this.CurSFsList.Text = "List 1";
            this.CurSFsList.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CurSFsListLabel
            // 
            this.CurSFsListLabel.AutoSize = true;
            this.CurSFsListLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CurSFsListLabel.Location = new System.Drawing.Point(2, 150);
            this.CurSFsListLabel.Name = "CurSFsListLabel";
            this.CurSFsListLabel.Size = new System.Drawing.Size(185, 13);
            this.CurSFsListLabel.TabIndex = 48;
            this.CurSFsListLabel.Text = "Current SoundFonts list loaded:";
            this.CurSFsListLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // KDMAPI
            // 
            this.KDMAPI.AutoSize = true;
            this.KDMAPI.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KDMAPI.Location = new System.Drawing.Point(97, 132);
            this.KDMAPI.Name = "KDMAPI";
            this.KDMAPI.Size = new System.Drawing.Size(56, 13);
            this.KDMAPI.TabIndex = 47;
            this.KDMAPI.Text = "Unknown.";
            this.KDMAPI.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // KDMAPILabel
            // 
            this.KDMAPILabel.AutoSize = true;
            this.KDMAPILabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KDMAPILabel.Location = new System.Drawing.Point(2, 132);
            this.KDMAPILabel.Name = "KDMAPILabel";
            this.KDMAPILabel.Size = new System.Drawing.Size(96, 13);
            this.KDMAPILabel.TabIndex = 46;
            this.KDMAPILabel.Text = "KDMAPI status:";
            this.KDMAPILabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Latency
            // 
            this.Latency.AutoSize = true;
            this.Latency.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Latency.Location = new System.Drawing.Point(89, 60);
            this.Latency.Name = "Latency";
            this.Latency.Size = new System.Drawing.Size(113, 13);
            this.Latency.TabIndex = 45;
            this.Latency.Text = "Input 0ms, Output 0ms";
            this.Latency.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LatencyLabel
            // 
            this.LatencyLabel.AutoSize = true;
            this.LatencyLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LatencyLabel.Location = new System.Drawing.Point(2, 60);
            this.LatencyLabel.Name = "LatencyLabel";
            this.LatencyLabel.Size = new System.Drawing.Size(88, 13);
            this.LatencyLabel.TabIndex = 44;
            this.LatencyLabel.Text = "Audio latency:";
            this.LatencyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // HeadsPos
            // 
            this.HeadsPos.AutoSize = true;
            this.HeadsPos.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HeadsPos.Location = new System.Drawing.Point(105, 78);
            this.HeadsPos.Name = "HeadsPos";
            this.HeadsPos.Size = new System.Drawing.Size(13, 13);
            this.HeadsPos.TabIndex = 41;
            this.HeadsPos.Text = "0";
            this.HeadsPos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.VoiceAverage.SetToolTip(this.HeadsPos, "It shows you the current position of both the readhead and writehead in the event" +
        "s buffer.");
            // 
            // HeadsPosLabel
            // 
            this.HeadsPosLabel.AutoSize = true;
            this.HeadsPosLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HeadsPosLabel.Location = new System.Drawing.Point(2, 78);
            this.HeadsPosLabel.Name = "HeadsPosLabel";
            this.HeadsPosLabel.Size = new System.Drawing.Size(104, 13);
            this.HeadsPosLabel.TabIndex = 40;
            this.HeadsPosLabel.Text = "RH/WH position:";
            this.HeadsPosLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.VoiceAverage.SetToolTip(this.HeadsPosLabel, "It shows you the current position of both the readhead and writehead in the event" +
        "s buffer.");
            // 
            // HCountV
            // 
            this.HCountV.AutoSize = true;
            this.HCountV.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HCountV.Location = new System.Drawing.Point(127, 114);
            this.HCountV.Name = "HCountV";
            this.HCountV.Size = new System.Drawing.Size(13, 13);
            this.HCountV.TabIndex = 39;
            this.HCountV.Text = "0";
            this.HCountV.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // HCountVLabel
            // 
            this.HCountVLabel.AutoSize = true;
            this.HCountVLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HCountVLabel.Location = new System.Drawing.Point(2, 114);
            this.HCountVLabel.Name = "HCountVLabel";
            this.HCountVLabel.Size = new System.Drawing.Size(126, 13);
            this.HCountVLabel.TabIndex = 38;
            this.HCountVLabel.Text = "App\'s handles count:";
            this.HCountVLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RAMUsageV
            // 
            this.RAMUsageV.AutoSize = true;
            this.RAMUsageV.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RAMUsageV.Location = new System.Drawing.Point(138, 96);
            this.RAMUsageV.Name = "RAMUsageV";
            this.RAMUsageV.Size = new System.Drawing.Size(32, 13);
            this.RAMUsageV.TabIndex = 37;
            this.RAMUsageV.Text = "0.0 B";
            this.RAMUsageV.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RAMUsageVLabel
            // 
            this.RAMUsageVLabel.AutoSize = true;
            this.RAMUsageVLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RAMUsageVLabel.Location = new System.Drawing.Point(2, 96);
            this.RAMUsageVLabel.Name = "RAMUsageVLabel";
            this.RAMUsageVLabel.Size = new System.Drawing.Size(137, 13);
            this.RAMUsageVLabel.TabIndex = 36;
            this.RAMUsageVLabel.Text = "App\'s working set size:";
            this.RAMUsageVLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RT
            // 
            this.RT.AutoSize = true;
            this.RT.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RT.Location = new System.Drawing.Point(97, 42);
            this.RT.Name = "RT";
            this.RT.Size = new System.Drawing.Size(21, 13);
            this.RT.TabIndex = 13;
            this.RT.Text = "0%";
            this.RT.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RTLabel
            // 
            this.RTLabel.AutoSize = true;
            this.RTLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RTLabel.Location = new System.Drawing.Point(2, 42);
            this.RTLabel.Name = "RTLabel";
            this.RTLabel.Size = new System.Drawing.Size(96, 13);
            this.RTLabel.TabIndex = 12;
            this.RTLabel.Text = "Rendering time:";
            this.RTLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AV
            // 
            this.AV.AutoSize = true;
            this.AV.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AV.Location = new System.Drawing.Point(89, 24);
            this.AV.Name = "AV";
            this.AV.Size = new System.Drawing.Size(13, 13);
            this.AV.TabIndex = 11;
            this.AV.Text = "0";
            this.AV.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AVLabel
            // 
            this.AVLabel.AutoSize = true;
            this.AVLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AVLabel.Location = new System.Drawing.Point(2, 24);
            this.AVLabel.Name = "AVLabel";
            this.AVLabel.Size = new System.Drawing.Size(88, 13);
            this.AVLabel.TabIndex = 10;
            this.AVLabel.Text = "Active voices:";
            this.AVLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CMA
            // 
            this.CMA.AutoSize = true;
            this.CMA.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CMA.Location = new System.Drawing.Point(109, 6);
            this.CMA.Name = "CMA";
            this.CMA.Size = new System.Drawing.Size(124, 13);
            this.CMA.TabIndex = 9;
            this.CMA.Text = "Starting debug window...";
            this.CMA.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CMALabel
            // 
            this.CMALabel.AutoSize = true;
            this.CMALabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CMALabel.Location = new System.Drawing.Point(2, 6);
            this.CMALabel.Name = "CMALabel";
            this.CMALabel.Size = new System.Drawing.Size(108, 13);
            this.CMALabel.TabIndex = 8;
            this.CMALabel.Text = "Current MIDI app:";
            this.CMALabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ChannelVoices
            // 
            this.ChannelVoices.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ChannelVoices.Controls.Add(this.CHV16);
            this.ChannelVoices.Controls.Add(this.CHV16L);
            this.ChannelVoices.Controls.Add(this.CHV15);
            this.ChannelVoices.Controls.Add(this.CHV15L);
            this.ChannelVoices.Controls.Add(this.CHV14);
            this.ChannelVoices.Controls.Add(this.CHV14L);
            this.ChannelVoices.Controls.Add(this.CHV13);
            this.ChannelVoices.Controls.Add(this.CHV13L);
            this.ChannelVoices.Controls.Add(this.CHV12);
            this.ChannelVoices.Controls.Add(this.CHV12L);
            this.ChannelVoices.Controls.Add(this.CHV11);
            this.ChannelVoices.Controls.Add(this.CHV11L);
            this.ChannelVoices.Controls.Add(this.CHV10);
            this.ChannelVoices.Controls.Add(this.CHV10L);
            this.ChannelVoices.Controls.Add(this.CHV9);
            this.ChannelVoices.Controls.Add(this.CHV9L);
            this.ChannelVoices.Controls.Add(this.CHV8);
            this.ChannelVoices.Controls.Add(this.CHV8L);
            this.ChannelVoices.Controls.Add(this.CHV7);
            this.ChannelVoices.Controls.Add(this.CHV7L);
            this.ChannelVoices.Controls.Add(this.CHV6);
            this.ChannelVoices.Controls.Add(this.CHV6L);
            this.ChannelVoices.Controls.Add(this.CHV5);
            this.ChannelVoices.Controls.Add(this.CHV5L);
            this.ChannelVoices.Controls.Add(this.CHV4);
            this.ChannelVoices.Controls.Add(this.CHV4L);
            this.ChannelVoices.Controls.Add(this.CHV3);
            this.ChannelVoices.Controls.Add(this.CHV3L);
            this.ChannelVoices.Controls.Add(this.CHV2);
            this.ChannelVoices.Controls.Add(this.CHV2L);
            this.ChannelVoices.Controls.Add(this.CHV1);
            this.ChannelVoices.Controls.Add(this.CHV1L);
            this.ChannelVoices.Location = new System.Drawing.Point(4, 22);
            this.ChannelVoices.Name = "ChannelVoices";
            this.ChannelVoices.Padding = new System.Windows.Forms.Padding(3);
            this.ChannelVoices.Size = new System.Drawing.Size(424, 170);
            this.ChannelVoices.TabIndex = 2;
            this.ChannelVoices.Text = "Channels voice count";
            // 
            // CHV16
            // 
            this.CHV16.AutoSize = true;
            this.CHV16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHV16.Location = new System.Drawing.Point(297, 132);
            this.CHV16.Name = "CHV16";
            this.CHV16.Size = new System.Drawing.Size(43, 13);
            this.CHV16.TabIndex = 47;
            this.CHV16.Text = "100000";
            this.CHV16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CHV16L
            // 
            this.CHV16L.AutoSize = true;
            this.CHV16L.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHV16L.Location = new System.Drawing.Point(222, 132);
            this.CHV16L.Name = "CHV16L";
            this.CHV16L.Size = new System.Drawing.Size(75, 13);
            this.CHV16L.TabIndex = 46;
            this.CHV16L.Text = "Channel 16:";
            this.CHV16L.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CHV15
            // 
            this.CHV15.AutoSize = true;
            this.CHV15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHV15.Location = new System.Drawing.Point(297, 114);
            this.CHV15.Name = "CHV15";
            this.CHV15.Size = new System.Drawing.Size(43, 13);
            this.CHV15.TabIndex = 45;
            this.CHV15.Text = "100000";
            this.CHV15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CHV15L
            // 
            this.CHV15L.AutoSize = true;
            this.CHV15L.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHV15L.Location = new System.Drawing.Point(222, 114);
            this.CHV15L.Name = "CHV15L";
            this.CHV15L.Size = new System.Drawing.Size(75, 13);
            this.CHV15L.TabIndex = 44;
            this.CHV15L.Text = "Channel 15:";
            this.CHV15L.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CHV14
            // 
            this.CHV14.AutoSize = true;
            this.CHV14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHV14.Location = new System.Drawing.Point(297, 96);
            this.CHV14.Name = "CHV14";
            this.CHV14.Size = new System.Drawing.Size(43, 13);
            this.CHV14.TabIndex = 43;
            this.CHV14.Text = "100000";
            this.CHV14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CHV14L
            // 
            this.CHV14L.AutoSize = true;
            this.CHV14L.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHV14L.Location = new System.Drawing.Point(222, 96);
            this.CHV14L.Name = "CHV14L";
            this.CHV14L.Size = new System.Drawing.Size(75, 13);
            this.CHV14L.TabIndex = 42;
            this.CHV14L.Text = "Channel 14:";
            this.CHV14L.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CHV13
            // 
            this.CHV13.AutoSize = true;
            this.CHV13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHV13.Location = new System.Drawing.Point(297, 78);
            this.CHV13.Name = "CHV13";
            this.CHV13.Size = new System.Drawing.Size(43, 13);
            this.CHV13.TabIndex = 41;
            this.CHV13.Text = "100000";
            this.CHV13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CHV13L
            // 
            this.CHV13L.AutoSize = true;
            this.CHV13L.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHV13L.Location = new System.Drawing.Point(222, 78);
            this.CHV13L.Name = "CHV13L";
            this.CHV13L.Size = new System.Drawing.Size(75, 13);
            this.CHV13L.TabIndex = 40;
            this.CHV13L.Text = "Channel 13:";
            this.CHV13L.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CHV12
            // 
            this.CHV12.AutoSize = true;
            this.CHV12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHV12.Location = new System.Drawing.Point(297, 60);
            this.CHV12.Name = "CHV12";
            this.CHV12.Size = new System.Drawing.Size(43, 13);
            this.CHV12.TabIndex = 39;
            this.CHV12.Text = "100000";
            this.CHV12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CHV12L
            // 
            this.CHV12L.AutoSize = true;
            this.CHV12L.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHV12L.Location = new System.Drawing.Point(222, 60);
            this.CHV12L.Name = "CHV12L";
            this.CHV12L.Size = new System.Drawing.Size(75, 13);
            this.CHV12L.TabIndex = 38;
            this.CHV12L.Text = "Channel 12:";
            this.CHV12L.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CHV11
            // 
            this.CHV11.AutoSize = true;
            this.CHV11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHV11.Location = new System.Drawing.Point(297, 42);
            this.CHV11.Name = "CHV11";
            this.CHV11.Size = new System.Drawing.Size(43, 13);
            this.CHV11.TabIndex = 37;
            this.CHV11.Text = "100000";
            this.CHV11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CHV11L
            // 
            this.CHV11L.AutoSize = true;
            this.CHV11L.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHV11L.Location = new System.Drawing.Point(222, 42);
            this.CHV11L.Name = "CHV11L";
            this.CHV11L.Size = new System.Drawing.Size(75, 13);
            this.CHV11L.TabIndex = 36;
            this.CHV11L.Text = "Channel 11:";
            this.CHV11L.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CHV10
            // 
            this.CHV10.AutoSize = true;
            this.CHV10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHV10.Location = new System.Drawing.Point(297, 24);
            this.CHV10.Name = "CHV10";
            this.CHV10.Size = new System.Drawing.Size(43, 13);
            this.CHV10.TabIndex = 35;
            this.CHV10.Text = "100000";
            this.CHV10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CHV10L
            // 
            this.CHV10L.AutoSize = true;
            this.CHV10L.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHV10L.Location = new System.Drawing.Point(222, 24);
            this.CHV10L.Name = "CHV10L";
            this.CHV10L.Size = new System.Drawing.Size(75, 13);
            this.CHV10L.TabIndex = 34;
            this.CHV10L.Text = "Channel 10:";
            this.CHV10L.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CHV9
            // 
            this.CHV9.AutoSize = true;
            this.CHV9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHV9.Location = new System.Drawing.Point(297, 6);
            this.CHV9.Name = "CHV9";
            this.CHV9.Size = new System.Drawing.Size(43, 13);
            this.CHV9.TabIndex = 33;
            this.CHV9.Text = "100000";
            this.CHV9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CHV9L
            // 
            this.CHV9L.AutoSize = true;
            this.CHV9L.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHV9L.Location = new System.Drawing.Point(229, 6);
            this.CHV9L.Name = "CHV9L";
            this.CHV9L.Size = new System.Drawing.Size(68, 13);
            this.CHV9L.TabIndex = 32;
            this.CHV9L.Text = "Channel 9:";
            this.CHV9L.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CHV8
            // 
            this.CHV8.AutoSize = true;
            this.CHV8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHV8.Location = new System.Drawing.Point(70, 132);
            this.CHV8.Name = "CHV8";
            this.CHV8.Size = new System.Drawing.Size(43, 13);
            this.CHV8.TabIndex = 31;
            this.CHV8.Text = "100000";
            this.CHV8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CHV8L
            // 
            this.CHV8L.AutoSize = true;
            this.CHV8L.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHV8L.Location = new System.Drawing.Point(2, 132);
            this.CHV8L.Name = "CHV8L";
            this.CHV8L.Size = new System.Drawing.Size(68, 13);
            this.CHV8L.TabIndex = 30;
            this.CHV8L.Text = "Channel 8:";
            this.CHV8L.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CHV7
            // 
            this.CHV7.AutoSize = true;
            this.CHV7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHV7.Location = new System.Drawing.Point(70, 114);
            this.CHV7.Name = "CHV7";
            this.CHV7.Size = new System.Drawing.Size(43, 13);
            this.CHV7.TabIndex = 29;
            this.CHV7.Text = "100000";
            this.CHV7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CHV7L
            // 
            this.CHV7L.AutoSize = true;
            this.CHV7L.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHV7L.Location = new System.Drawing.Point(2, 114);
            this.CHV7L.Name = "CHV7L";
            this.CHV7L.Size = new System.Drawing.Size(68, 13);
            this.CHV7L.TabIndex = 28;
            this.CHV7L.Text = "Channel 7:";
            this.CHV7L.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CHV6
            // 
            this.CHV6.AutoSize = true;
            this.CHV6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHV6.Location = new System.Drawing.Point(70, 96);
            this.CHV6.Name = "CHV6";
            this.CHV6.Size = new System.Drawing.Size(43, 13);
            this.CHV6.TabIndex = 27;
            this.CHV6.Text = "100000";
            this.CHV6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CHV6L
            // 
            this.CHV6L.AutoSize = true;
            this.CHV6L.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHV6L.Location = new System.Drawing.Point(2, 96);
            this.CHV6L.Name = "CHV6L";
            this.CHV6L.Size = new System.Drawing.Size(68, 13);
            this.CHV6L.TabIndex = 26;
            this.CHV6L.Text = "Channel 6:";
            this.CHV6L.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CHV5
            // 
            this.CHV5.AutoSize = true;
            this.CHV5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHV5.Location = new System.Drawing.Point(70, 78);
            this.CHV5.Name = "CHV5";
            this.CHV5.Size = new System.Drawing.Size(43, 13);
            this.CHV5.TabIndex = 25;
            this.CHV5.Text = "100000";
            this.CHV5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CHV5L
            // 
            this.CHV5L.AutoSize = true;
            this.CHV5L.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHV5L.Location = new System.Drawing.Point(2, 78);
            this.CHV5L.Name = "CHV5L";
            this.CHV5L.Size = new System.Drawing.Size(68, 13);
            this.CHV5L.TabIndex = 24;
            this.CHV5L.Text = "Channel 5:";
            this.CHV5L.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CHV4
            // 
            this.CHV4.AutoSize = true;
            this.CHV4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHV4.Location = new System.Drawing.Point(70, 60);
            this.CHV4.Name = "CHV4";
            this.CHV4.Size = new System.Drawing.Size(43, 13);
            this.CHV4.TabIndex = 23;
            this.CHV4.Text = "100000";
            this.CHV4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CHV4L
            // 
            this.CHV4L.AutoSize = true;
            this.CHV4L.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHV4L.Location = new System.Drawing.Point(2, 60);
            this.CHV4L.Name = "CHV4L";
            this.CHV4L.Size = new System.Drawing.Size(68, 13);
            this.CHV4L.TabIndex = 22;
            this.CHV4L.Text = "Channel 4:";
            this.CHV4L.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CHV3
            // 
            this.CHV3.AutoSize = true;
            this.CHV3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHV3.Location = new System.Drawing.Point(70, 42);
            this.CHV3.Name = "CHV3";
            this.CHV3.Size = new System.Drawing.Size(43, 13);
            this.CHV3.TabIndex = 21;
            this.CHV3.Text = "100000";
            this.CHV3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CHV3L
            // 
            this.CHV3L.AutoSize = true;
            this.CHV3L.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHV3L.Location = new System.Drawing.Point(2, 42);
            this.CHV3L.Name = "CHV3L";
            this.CHV3L.Size = new System.Drawing.Size(68, 13);
            this.CHV3L.TabIndex = 20;
            this.CHV3L.Text = "Channel 3:";
            this.CHV3L.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CHV2
            // 
            this.CHV2.AutoSize = true;
            this.CHV2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHV2.Location = new System.Drawing.Point(70, 24);
            this.CHV2.Name = "CHV2";
            this.CHV2.Size = new System.Drawing.Size(43, 13);
            this.CHV2.TabIndex = 19;
            this.CHV2.Text = "100000";
            this.CHV2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CHV2L
            // 
            this.CHV2L.AutoSize = true;
            this.CHV2L.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHV2L.Location = new System.Drawing.Point(2, 24);
            this.CHV2L.Name = "CHV2L";
            this.CHV2L.Size = new System.Drawing.Size(68, 13);
            this.CHV2L.TabIndex = 18;
            this.CHV2L.Text = "Channel 2:";
            this.CHV2L.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CHV1
            // 
            this.CHV1.AutoSize = true;
            this.CHV1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHV1.Location = new System.Drawing.Point(70, 6);
            this.CHV1.Name = "CHV1";
            this.CHV1.Size = new System.Drawing.Size(43, 13);
            this.CHV1.TabIndex = 17;
            this.CHV1.Text = "100000";
            this.CHV1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CHV1L
            // 
            this.CHV1L.AutoSize = true;
            this.CHV1L.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHV1L.Location = new System.Drawing.Point(2, 6);
            this.CHV1L.Name = "CHV1L";
            this.CHV1L.Size = new System.Drawing.Size(68, 13);
            this.CHV1L.TabIndex = 16;
            this.CHV1L.Text = "Channel 1:";
            this.CHV1L.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PCSpecs
            // 
            this.PCSpecs.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.PCSpecs.Controls.Add(this.CPULogo);
            this.PCSpecs.Controls.Add(this.MT);
            this.PCSpecs.Controls.Add(this.MTLabel);
            this.PCSpecs.Controls.Add(this.GPUInternalChip);
            this.PCSpecs.Controls.Add(this.GPUInternalChipLabel);
            this.PCSpecs.Controls.Add(this.GPUInfo);
            this.PCSpecs.Controls.Add(this.GPUInfoLabel);
            this.PCSpecs.Controls.Add(this.GPU);
            this.PCSpecs.Controls.Add(this.GPULabel);
            this.PCSpecs.Controls.Add(this.CPUInfo);
            this.PCSpecs.Controls.Add(this.CPUInfoLabel);
            this.PCSpecs.Controls.Add(this.CPU);
            this.PCSpecs.Controls.Add(this.CPULabel);
            this.PCSpecs.Controls.Add(this.AM);
            this.PCSpecs.Controls.Add(this.AMLabel);
            this.PCSpecs.Controls.Add(this.TM);
            this.PCSpecs.Controls.Add(this.TMLabel);
            this.PCSpecs.Controls.Add(this.COS);
            this.PCSpecs.Controls.Add(this.COSLabel);
            this.PCSpecs.Controls.Add(this.WinLogo);
            this.PCSpecs.Location = new System.Drawing.Point(4, 22);
            this.PCSpecs.Name = "PCSpecs";
            this.PCSpecs.Padding = new System.Windows.Forms.Padding(3);
            this.PCSpecs.Size = new System.Drawing.Size(424, 170);
            this.PCSpecs.TabIndex = 1;
            this.PCSpecs.Text = "Computer specifications";
            // 
            // CPULogo
            // 
            this.CPULogo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CPULogo.Cursor = System.Windows.Forms.Cursors.Help;
            this.CPULogo.Location = new System.Drawing.Point(389, 37);
            this.CPULogo.Name = "CPULogo";
            this.CPULogo.Size = new System.Drawing.Size(32, 32);
            this.CPULogo.TabIndex = 38;
            this.CPULogo.TabStop = false;
            // 
            // MT
            // 
            this.MT.AutoSize = true;
            this.MT.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MT.Location = new System.Drawing.Point(88, 150);
            this.MT.Name = "MT";
            this.MT.Size = new System.Drawing.Size(92, 13);
            this.MT.TabIndex = 37;
            this.MT.Text = "Lapdeskservertop";
            this.MT.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MTLabel
            // 
            this.MTLabel.AutoSize = true;
            this.MTLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MTLabel.Location = new System.Drawing.Point(2, 150);
            this.MTLabel.Name = "MTLabel";
            this.MTLabel.Size = new System.Drawing.Size(87, 13);
            this.MTLabel.TabIndex = 36;
            this.MTLabel.Text = "Machine type:";
            this.MTLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // GPUInternalChip
            // 
            this.GPUInternalChip.AutoSize = true;
            this.GPUInternalChip.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GPUInternalChip.Location = new System.Drawing.Point(65, 78);
            this.GPUInternalChip.Name = "GPUInternalChip";
            this.GPUInternalChip.Size = new System.Drawing.Size(203, 13);
            this.GPUInternalChip.TabIndex = 35;
            this.GPUInternalChip.Text = "NVIDIA or AMD, no preference here lmao";
            this.GPUInternalChip.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // GPUInternalChipLabel
            // 
            this.GPUInternalChipLabel.AutoSize = true;
            this.GPUInternalChipLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GPUInternalChipLabel.Location = new System.Drawing.Point(2, 78);
            this.GPUInternalChipLabel.Name = "GPUInternalChipLabel";
            this.GPUInternalChipLabel.Size = new System.Drawing.Size(65, 13);
            this.GPUInternalChipLabel.TabIndex = 34;
            this.GPUInternalChipLabel.Text = "GPU chip:";
            this.GPUInternalChipLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // GPUInfo
            // 
            this.GPUInfo.AutoSize = true;
            this.GPUInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GPUInfo.Location = new System.Drawing.Point(62, 96);
            this.GPUInfo.Name = "GPUInfo";
            this.GPUInfo.Size = new System.Drawing.Size(108, 13);
            this.GPUInfo.TabIndex = 32;
            this.GPUInfo.Text = "I have lotas of VRAM";
            this.GPUInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // GPUInfoLabel
            // 
            this.GPUInfoLabel.AutoSize = true;
            this.GPUInfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GPUInfoLabel.Location = new System.Drawing.Point(2, 96);
            this.GPUInfoLabel.Name = "GPUInfoLabel";
            this.GPUInfoLabel.Size = new System.Drawing.Size(62, 13);
            this.GPUInfoLabel.TabIndex = 31;
            this.GPUInfoLabel.Text = "GPU info:";
            this.GPUInfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // GPU
            // 
            this.GPU.AutoSize = true;
            this.GPU.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GPU.Location = new System.Drawing.Point(37, 60);
            this.GPU.Name = "GPU";
            this.GPU.Size = new System.Drawing.Size(203, 13);
            this.GPU.TabIndex = 30;
            this.GPU.Text = "NVIDIA or AMD, no preference here lmao";
            this.GPU.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // GPULabel
            // 
            this.GPULabel.AutoSize = true;
            this.GPULabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GPULabel.Location = new System.Drawing.Point(2, 60);
            this.GPULabel.Name = "GPULabel";
            this.GPULabel.Size = new System.Drawing.Size(37, 13);
            this.GPULabel.TabIndex = 29;
            this.GPULabel.Text = "GPU:";
            this.GPULabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CPUInfo
            // 
            this.CPUInfo.AutoSize = true;
            this.CPUInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CPUInfo.Location = new System.Drawing.Point(62, 42);
            this.CPUInfo.Name = "CPUInfo";
            this.CPUInfo.Size = new System.Drawing.Size(110, 13);
            this.CPUInfo.TabIndex = 28;
            this.CPUInfo.Text = "My CPU runs at 6THz";
            this.CPUInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CPUInfoLabel
            // 
            this.CPUInfoLabel.AutoSize = true;
            this.CPUInfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CPUInfoLabel.Location = new System.Drawing.Point(2, 42);
            this.CPUInfoLabel.Name = "CPUInfoLabel";
            this.CPUInfoLabel.Size = new System.Drawing.Size(61, 13);
            this.CPUInfoLabel.TabIndex = 27;
            this.CPUInfoLabel.Text = "CPU info:";
            this.CPUInfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CPU
            // 
            this.CPU.AutoSize = true;
            this.CPU.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CPU.Location = new System.Drawing.Point(37, 24);
            this.CPU.Name = "CPU";
            this.CPU.Size = new System.Drawing.Size(187, 13);
            this.CPU.TabIndex = 26;
            this.CPU.Text = "Intel or AMD, no preference here lmao";
            this.CPU.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CPULabel
            // 
            this.CPULabel.AutoSize = true;
            this.CPULabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CPULabel.Location = new System.Drawing.Point(2, 24);
            this.CPULabel.Name = "CPULabel";
            this.CPULabel.Size = new System.Drawing.Size(36, 13);
            this.CPULabel.TabIndex = 25;
            this.CPULabel.Text = "CPU:";
            this.CPULabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AM
            // 
            this.AM.AutoSize = true;
            this.AM.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AM.Location = new System.Drawing.Point(110, 132);
            this.AM.Name = "AM";
            this.AM.Size = new System.Drawing.Size(74, 13);
            this.AM.TabIndex = 23;
            this.AM.Text = "256EB (Lotas)";
            this.AM.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AMLabel
            // 
            this.AMLabel.AutoSize = true;
            this.AMLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AMLabel.Location = new System.Drawing.Point(2, 132);
            this.AMLabel.Name = "AMLabel";
            this.AMLabel.Size = new System.Drawing.Size(109, 13);
            this.AMLabel.TabIndex = 22;
            this.AMLabel.Text = "Available memory:";
            this.AMLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TM
            // 
            this.TM.AutoSize = true;
            this.TM.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TM.Location = new System.Drawing.Point(87, 114);
            this.TM.Name = "TM";
            this.TM.Size = new System.Drawing.Size(74, 13);
            this.TM.TabIndex = 21;
            this.TM.Text = "256EB (Lotas)";
            this.TM.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TMLabel
            // 
            this.TMLabel.AutoSize = true;
            this.TMLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TMLabel.Location = new System.Drawing.Point(2, 114);
            this.TMLabel.Name = "TMLabel";
            this.TMLabel.Size = new System.Drawing.Size(86, 13);
            this.TMLabel.TabIndex = 20;
            this.TMLabel.Text = "Total memory:";
            this.TMLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // COS
            // 
            this.COS.AutoSize = true;
            this.COS.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.COS.Location = new System.Drawing.Point(109, 6);
            this.COS.Name = "COS";
            this.COS.Size = new System.Drawing.Size(128, 13);
            this.COS.TabIndex = 19;
            this.COS.Text = "OSName (Prob Windows)";
            this.COS.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // COSLabel
            // 
            this.COSLabel.AutoSize = true;
            this.COSLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.COSLabel.Location = new System.Drawing.Point(2, 6);
            this.COSLabel.Name = "COSLabel";
            this.COSLabel.Size = new System.Drawing.Size(108, 13);
            this.COSLabel.TabIndex = 18;
            this.COSLabel.Text = "Operating system:";
            this.COSLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // WinLogo
            // 
            this.WinLogo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.WinLogo.Cursor = System.Windows.Forms.Cursors.Help;
            this.WinLogo.Location = new System.Drawing.Point(389, 3);
            this.WinLogo.Name = "WinLogo";
            this.WinLogo.Size = new System.Drawing.Size(32, 32);
            this.WinLogo.TabIndex = 24;
            this.WinLogo.TabStop = false;
            // 
            // SelectDebugPipe
            // 
            this.SelectDebugPipe.BackColor = System.Drawing.Color.Transparent;
            this.SelectDebugPipe.Location = new System.Drawing.Point(281, 223);
            this.SelectDebugPipe.Name = "SelectDebugPipe";
            this.SelectDebugPipe.Size = new System.Drawing.Size(147, 23);
            this.SelectDebugPipe.TabIndex = 50;
            this.SelectDebugPipe.Text = "Select debug pipe";
            this.SelectDebugPipe.UseVisualStyleBackColor = false;
            this.SelectDebugPipe.Click += new System.EventHandler(this.SelectDebugPipe_Click);
            // 
            // VersionLabel
            // 
            this.VersionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.VersionLabel.BackColor = System.Drawing.Color.Transparent;
            this.VersionLabel.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.VersionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VersionLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(0)))), ((int)(((byte)(119)))));
            this.VersionLabel.Location = new System.Drawing.Point(42, 207);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(237, 32);
            this.VersionLabel.TabIndex = 35;
            this.VersionLabel.Text = "Checking version...";
            this.VersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CopyToClip1
            // 
            this.CopyToClip1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CopyToClip1.BackColor = System.Drawing.Color.Transparent;
            this.CopyToClip1.Location = new System.Drawing.Point(281, 199);
            this.CopyToClip1.Name = "CopyToClip1";
            this.CopyToClip1.Size = new System.Drawing.Size(147, 23);
            this.CopyToClip1.TabIndex = 34;
            this.CopyToClip1.Text = "Copy all tabs to clipboard";
            this.CopyToClip1.UseVisualStyleBackColor = false;
            this.CopyToClip1.Click += new System.EventHandler(this.CopyToClip_Click);
            // 
            // KSLogo
            // 
            this.KSLogo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.KSLogo.BackgroundImage = global::OmniMIDIDebugWindow.Properties.Resources.DebugIcon;
            this.KSLogo.Cursor = System.Windows.Forms.Cursors.Help;
            this.KSLogo.Location = new System.Drawing.Point(9, 207);
            this.KSLogo.Name = "KSLogo";
            this.KSLogo.Size = new System.Drawing.Size(32, 32);
            this.KSLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.KSLogo.TabIndex = 25;
            this.KSLogo.TabStop = false;
            this.KSLogo.Click += new System.EventHandler(this.KSLogo_Click);
            // 
            // WinLogoTT
            // 
            this.WinLogoTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.WinLogoTT.ToolTipTitle = "What OS am I using?";
            // 
            // CPULogoTT
            // 
            this.CPULogoTT.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.CPULogoTT.ToolTipTitle = "What CPU am I using?";
            // 
            // VoiceAverage
            // 
            this.VoiceAverage.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.VoiceAverage.ToolTipTitle = "What does this mean?";
            // 
            // DebugInfo
            // 
            this.DebugInfo.Interval = 1;
            this.DebugInfo.Tick += new System.EventHandler(this.DebugInfo_Tick);
            // 
            // DebugInfoCheck
            // 
            this.DebugInfoCheck.WorkerSupportsCancellation = true;
            this.DebugInfoCheck.DoWork += new System.ComponentModel.DoWorkEventHandler(this.DebugInfoCheck_DoWork);
            this.DebugInfoCheck.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.DebugInfoCheck_RunWorkerCompleted);
            // 
            // ReloadDebugInfo
            // 
            this.ReloadDebugInfo.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.ReloadDebugInfo.ToolTipTitle = "What does this do?";
            // 
            // CheckMem
            // 
            this.CheckMem.DoWork += new System.ComponentModel.DoWorkEventHandler(this.CheckMem_DoWork);
            // 
            // OmniMIDIDebugWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(432, 251);
            this.Controls.Add(this.SelectDebugPipe);
            this.Controls.Add(this.Tabs);
            this.Controls.Add(this.CopyToClip1);
            this.Controls.Add(this.VersionLabel);
            this.Controls.Add(this.KSLogo);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "OmniMIDIDebugWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OmniMIDI Debug Window";
            this.Load += new System.EventHandler(this.KeppySynthDebugWindow_Load);
            this.Tabs.ResumeLayout(false);
            this.SynthDbg.ResumeLayout(false);
            this.SynthDbg.PerformLayout();
            this.ChannelVoices.ResumeLayout(false);
            this.ChannelVoices.PerformLayout();
            this.PCSpecs.ResumeLayout(false);
            this.PCSpecs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CPULogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WinLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.KSLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenu MainCont;
        private System.Windows.Forms.MenuItem OpenAppLocat;
        private System.Windows.Forms.MenuItem CopyToClipboard;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem ExitMenu;
        private System.Windows.Forms.MenuItem OpenConfigurator;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem DebugWinTop;
        private System.Windows.Forms.TabControl Tabs;
        private System.Windows.Forms.TabPage SynthDbg;
        private System.Windows.Forms.Label CMALabel;
        private System.Windows.Forms.TabPage PCSpecs;
        private System.Windows.Forms.Label AV;
        private System.Windows.Forms.Label AVLabel;
        private System.Windows.Forms.Label CMA;
        private System.Windows.Forms.Label RT;
        private System.Windows.Forms.Label RTLabel;
        private System.Windows.Forms.Label COS;
        private System.Windows.Forms.Label COSLabel;
        private System.Windows.Forms.Label AM;
        private System.Windows.Forms.Label AMLabel;
        private System.Windows.Forms.Label TM;
        private System.Windows.Forms.Label TMLabel;
        private System.Windows.Forms.PictureBox WinLogo;
        private System.Windows.Forms.Label CPU;
        private System.Windows.Forms.Label CPULabel;
        private System.Windows.Forms.Label CPUInfo;
        private System.Windows.Forms.Label CPUInfoLabel;
        private System.Windows.Forms.Label GPUInfo;
        private System.Windows.Forms.Label GPUInfoLabel;
        private System.Windows.Forms.Label GPU;
        private System.Windows.Forms.Label GPULabel;
        private System.Windows.Forms.Button CopyToClip1;
        private System.Windows.Forms.Label GPUInternalChip;
        private System.Windows.Forms.Label GPUInternalChipLabel;
        private System.Windows.Forms.Label MT;
        private System.Windows.Forms.Label MTLabel;
        private System.Windows.Forms.PictureBox CPULogo;
        private System.Windows.Forms.TabPage ChannelVoices;
        private System.Windows.Forms.Label CHV16;
        private System.Windows.Forms.Label CHV16L;
        private System.Windows.Forms.Label CHV15;
        private System.Windows.Forms.Label CHV15L;
        private System.Windows.Forms.Label CHV14;
        private System.Windows.Forms.Label CHV14L;
        private System.Windows.Forms.Label CHV13;
        private System.Windows.Forms.Label CHV13L;
        private System.Windows.Forms.Label CHV12;
        private System.Windows.Forms.Label CHV12L;
        private System.Windows.Forms.Label CHV11;
        private System.Windows.Forms.Label CHV11L;
        private System.Windows.Forms.Label CHV10;
        private System.Windows.Forms.Label CHV10L;
        private System.Windows.Forms.Label CHV9;
        private System.Windows.Forms.Label CHV9L;
        private System.Windows.Forms.Label CHV8;
        private System.Windows.Forms.Label CHV8L;
        private System.Windows.Forms.Label CHV7;
        private System.Windows.Forms.Label CHV7L;
        private System.Windows.Forms.Label CHV6;
        private System.Windows.Forms.Label CHV6L;
        private System.Windows.Forms.Label CHV5;
        private System.Windows.Forms.Label CHV5L;
        private System.Windows.Forms.Label CHV4;
        private System.Windows.Forms.Label CHV4L;
        private System.Windows.Forms.Label CHV3;
        private System.Windows.Forms.Label CHV3L;
        private System.Windows.Forms.Label CHV2;
        private System.Windows.Forms.Label CHV2L;
        private System.Windows.Forms.Label CHV1;
        private System.Windows.Forms.Label CHV1L;
        private System.Windows.Forms.PictureBox KSLogo;
        private System.Windows.Forms.Label VersionLabel;
        private System.Windows.Forms.ToolTip WinLogoTT;
        private System.Windows.Forms.ToolTip CPULogoTT;
        private System.Windows.Forms.Label HeadsPos;
        private System.Windows.Forms.Label HeadsPosLabel;
        private System.Windows.Forms.ToolTip VoiceAverage;
        private System.Windows.Forms.Label HCountV;
        private System.Windows.Forms.Label HCountVLabel;
        private System.Windows.Forms.Label RAMUsageV;
        private System.Windows.Forms.Label RAMUsageVLabel;
        public System.Windows.Forms.Timer DebugInfo;
        private System.Windows.Forms.Label Latency;
        private System.Windows.Forms.Label LatencyLabel;
        private System.ComponentModel.BackgroundWorker DebugInfoCheck;
        private System.Windows.Forms.ToolTip ReloadDebugInfo;
        private System.ComponentModel.BackgroundWorker CheckMem;
        private System.Windows.Forms.Label KDMAPI;
        private System.Windows.Forms.Label KDMAPILabel;
        private System.Windows.Forms.Label CurSFsList;
        private System.Windows.Forms.Label CurSFsListLabel;
        private System.Windows.Forms.Button SelectDebugPipe;
    }
}

