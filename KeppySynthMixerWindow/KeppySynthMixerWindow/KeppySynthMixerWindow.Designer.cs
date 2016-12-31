namespace KeppySynthMixerWindow
{
    partial class KeppySynthMixerWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KeppySynthMixerWindow));
            this.label4 = new System.Windows.Forms.Label();
            this.ChannelVolume = new System.Windows.Forms.Timer(this.components);
            this.showTheConfiguratorWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToDefaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.muteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainVol = new System.Windows.Forms.TrackBar();
            this.CH16 = new System.Windows.Forms.Label();
            this.fullVolumeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MIDIVolumeOverride = new System.Windows.Forms.CheckBox();
            this.VolumeMonitor = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.CH15 = new System.Windows.Forms.Label();
            this.CH14 = new System.Windows.Forms.Label();
            this.CH13 = new System.Windows.Forms.Label();
            this.VolumeTip = new System.Windows.Forms.ToolTip(this.components);
            this.CH12 = new System.Windows.Forms.Label();
            this.CH11 = new System.Windows.Forms.Label();
            this.CH10 = new System.Windows.Forms.Label();
            this.CH9 = new System.Windows.Forms.Label();
            this.CH8 = new System.Windows.Forms.Label();
            this.CH7 = new System.Windows.Forms.Label();
            this.CH6 = new System.Windows.Forms.Label();
            this.CH5 = new System.Windows.Forms.Label();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CH4 = new System.Windows.Forms.Label();
            this.CH3 = new System.Windows.Forms.Label();
            this.CH2 = new System.Windows.Forms.Label();
            this.CH1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.LeftChannel = new System.Windows.Forms.ProgressBar();
            this.CH16VOL = new System.Windows.Forms.TrackBar();
            this.CH15VOL = new System.Windows.Forms.TrackBar();
            this.CH14VOL = new System.Windows.Forms.TrackBar();
            this.CH13VOL = new System.Windows.Forms.TrackBar();
            this.CH12VOL = new System.Windows.Forms.TrackBar();
            this.CH11VOL = new System.Windows.Forms.TrackBar();
            this.CH10VOL = new System.Windows.Forms.TrackBar();
            this.CH9VOL = new System.Windows.Forms.TrackBar();
            this.CH8VOL = new System.Windows.Forms.TrackBar();
            this.CH7VOL = new System.Windows.Forms.TrackBar();
            this.CH6VOL = new System.Windows.Forms.TrackBar();
            this.CH5VOL = new System.Windows.Forms.TrackBar();
            this.CH4VOL = new System.Windows.Forms.TrackBar();
            this.CH3VOL = new System.Windows.Forms.TrackBar();
            this.CH2VOL = new System.Windows.Forms.TrackBar();
            this.CH1VOL = new System.Windows.Forms.TrackBar();
            this.Main = new System.Windows.Forms.MenuStrip();
            this.RightChannel = new System.Windows.Forms.ProgressBar();
            this.VolumeCheck = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.MainVol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH16VOL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH15VOL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH14VOL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH13VOL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH12VOL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH11VOL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH10VOL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH9VOL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH8VOL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH7VOL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH6VOL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH5VOL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH4VOL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH3VOL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH2VOL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH1VOL)).BeginInit();
            this.Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(554, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 71;
            this.label4.Text = "All";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ChannelVolume
            // 
            this.ChannelVolume.Enabled = true;
            this.ChannelVolume.Interval = 17;
            this.ChannelVolume.Tick += new System.EventHandler(this.ChannelVolume_Tick);
            // 
            // showTheConfiguratorWindowToolStripMenuItem
            // 
            this.showTheConfiguratorWindowToolStripMenuItem.Name = "showTheConfiguratorWindowToolStripMenuItem";
            this.showTheConfiguratorWindowToolStripMenuItem.Size = new System.Drawing.Size(162, 20);
            this.showTheConfiguratorWindowToolStripMenuItem.Text = "Show the configurator window";
            this.showTheConfiguratorWindowToolStripMenuItem.Click += new System.EventHandler(this.showTheConfiguratorWindowToolStripMenuItem_Click);
            // 
            // resetToDefaultToolStripMenuItem
            // 
            this.resetToDefaultToolStripMenuItem.Name = "resetToDefaultToolStripMenuItem";
            this.resetToDefaultToolStripMenuItem.Size = new System.Drawing.Size(94, 20);
            this.resetToDefaultToolStripMenuItem.Text = "Reset to default";
            this.resetToDefaultToolStripMenuItem.Click += new System.EventHandler(this.resetToDefaultToolStripMenuItem_Click);
            // 
            // muteToolStripMenuItem
            // 
            this.muteToolStripMenuItem.Name = "muteToolStripMenuItem";
            this.muteToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.muteToolStripMenuItem.Text = "Mute";
            this.muteToolStripMenuItem.Click += new System.EventHandler(this.muteToolStripMenuItem_Click);
            // 
            // MainVol
            // 
            this.MainVol.AutoSize = false;
            this.MainVol.LargeChange = 10;
            this.MainVol.Location = new System.Drawing.Point(559, 55);
            this.MainVol.Maximum = 127;
            this.MainVol.Name = "MainVol";
            this.MainVol.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.MainVol.Size = new System.Drawing.Size(27, 123);
            this.MainVol.TabIndex = 70;
            this.MainVol.TickFrequency = 16;
            this.MainVol.Value = 127;
            this.MainVol.Scroll += new System.EventHandler(this.MainVol_Scroll);
            // 
            // CH16
            // 
            this.CH16.Location = new System.Drawing.Point(504, 39);
            this.CH16.Name = "CH16";
            this.CH16.Size = new System.Drawing.Size(27, 13);
            this.CH16.TabIndex = 52;
            this.CH16.Text = "16";
            this.CH16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // fullVolumeToolStripMenuItem
            // 
            this.fullVolumeToolStripMenuItem.Name = "fullVolumeToolStripMenuItem";
            this.fullVolumeToolStripMenuItem.Size = new System.Drawing.Size(72, 20);
            this.fullVolumeToolStripMenuItem.Text = "Full volume";
            this.fullVolumeToolStripMenuItem.Click += new System.EventHandler(this.fullVolumeToolStripMenuItem_Click);
            // 
            // MIDIVolumeOverride
            // 
            this.MIDIVolumeOverride.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.MIDIVolumeOverride.AutoSize = true;
            this.MIDIVolumeOverride.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.MIDIVolumeOverride.Location = new System.Drawing.Point(600, 102);
            this.MIDIVolumeOverride.Name = "MIDIVolumeOverride";
            this.MIDIVolumeOverride.Size = new System.Drawing.Size(193, 17);
            this.MIDIVolumeOverride.TabIndex = 69;
            this.MIDIVolumeOverride.Text = "Enable MIDI volume event override";
            this.MIDIVolumeOverride.UseVisualStyleBackColor = true;
            this.MIDIVolumeOverride.CheckedChanged += new System.EventHandler(this.MIDIVolumeOverride_CheckedChanged);
            // 
            // VolumeMonitor
            // 
            this.VolumeMonitor.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.VolumeMonitor.AutoSize = true;
            this.VolumeMonitor.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.VolumeMonitor.Location = new System.Drawing.Point(646, 120);
            this.VolumeMonitor.Name = "VolumeMonitor";
            this.VolumeMonitor.Size = new System.Drawing.Size(147, 17);
            this.VolumeMonitor.TabIndex = 67;
            this.VolumeMonitor.Text = "Enable volume monitoring";
            this.VolumeMonitor.UseVisualStyleBackColor = true;
            this.VolumeMonitor.CheckedChanged += new System.EventHandler(this.VolumeMonitor_CheckedChanged);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.Location = new System.Drawing.Point(611, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(181, 60);
            this.label3.TabIndex = 66;
            this.label3.Text = "The volume meter only works when the XAudio interface is disabled.";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CH15
            // 
            this.CH15.Location = new System.Drawing.Point(471, 39);
            this.CH15.Name = "CH15";
            this.CH15.Size = new System.Drawing.Size(27, 13);
            this.CH15.TabIndex = 53;
            this.CH15.Text = "15";
            this.CH15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CH14
            // 
            this.CH14.Location = new System.Drawing.Point(438, 39);
            this.CH14.Name = "CH14";
            this.CH14.Size = new System.Drawing.Size(27, 13);
            this.CH14.TabIndex = 54;
            this.CH14.Text = "14";
            this.CH14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CH13
            // 
            this.CH13.Location = new System.Drawing.Point(405, 39);
            this.CH13.Name = "CH13";
            this.CH13.Size = new System.Drawing.Size(27, 13);
            this.CH13.TabIndex = 58;
            this.CH13.Text = "13";
            this.CH13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // VolumeTip
            // 
            this.VolumeTip.AutomaticDelay = 0;
            // 
            // CH12
            // 
            this.CH12.Location = new System.Drawing.Point(372, 39);
            this.CH12.Name = "CH12";
            this.CH12.Size = new System.Drawing.Size(27, 13);
            this.CH12.TabIndex = 56;
            this.CH12.Text = "12";
            this.CH12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CH11
            // 
            this.CH11.Location = new System.Drawing.Point(341, 39);
            this.CH11.Name = "CH11";
            this.CH11.Size = new System.Drawing.Size(27, 13);
            this.CH11.TabIndex = 57;
            this.CH11.Text = "11";
            this.CH11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CH10
            // 
            this.CH10.Location = new System.Drawing.Point(308, 39);
            this.CH10.Name = "CH10";
            this.CH10.Size = new System.Drawing.Size(27, 13);
            this.CH10.TabIndex = 65;
            this.CH10.Text = "10";
            this.CH10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CH9
            // 
            this.CH9.Location = new System.Drawing.Point(275, 39);
            this.CH9.Name = "CH9";
            this.CH9.Size = new System.Drawing.Size(27, 13);
            this.CH9.TabIndex = 59;
            this.CH9.Text = "9";
            this.CH9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CH8
            // 
            this.CH8.Location = new System.Drawing.Point(242, 39);
            this.CH8.Name = "CH8";
            this.CH8.Size = new System.Drawing.Size(27, 13);
            this.CH8.TabIndex = 60;
            this.CH8.Text = "8";
            this.CH8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CH7
            // 
            this.CH7.Location = new System.Drawing.Point(209, 39);
            this.CH7.Name = "CH7";
            this.CH7.Size = new System.Drawing.Size(27, 13);
            this.CH7.TabIndex = 61;
            this.CH7.Text = "7";
            this.CH7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CH6
            // 
            this.CH6.Location = new System.Drawing.Point(176, 39);
            this.CH6.Name = "CH6";
            this.CH6.Size = new System.Drawing.Size(27, 13);
            this.CH6.TabIndex = 62;
            this.CH6.Text = "6";
            this.CH6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CH5
            // 
            this.CH5.Location = new System.Drawing.Point(143, 39);
            this.CH5.Name = "CH5";
            this.CH5.Size = new System.Drawing.Size(27, 13);
            this.CH5.TabIndex = 63;
            this.CH5.Text = "5";
            this.CH5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(36, 20);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // CH4
            // 
            this.CH4.Location = new System.Drawing.Point(110, 39);
            this.CH4.Name = "CH4";
            this.CH4.Size = new System.Drawing.Size(27, 13);
            this.CH4.TabIndex = 64;
            this.CH4.Text = "4";
            this.CH4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CH3
            // 
            this.CH3.Location = new System.Drawing.Point(77, 39);
            this.CH3.Name = "CH3";
            this.CH3.Size = new System.Drawing.Size(27, 13);
            this.CH3.TabIndex = 51;
            this.CH3.Text = "3";
            this.CH3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CH2
            // 
            this.CH2.Location = new System.Drawing.Point(44, 39);
            this.CH2.Name = "CH2";
            this.CH2.Size = new System.Drawing.Size(27, 13);
            this.CH2.TabIndex = 55;
            this.CH2.Text = "2";
            this.CH2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CH1
            // 
            this.CH1.Location = new System.Drawing.Point(11, 39);
            this.CH1.Name = "CH1";
            this.CH1.Size = new System.Drawing.Size(27, 13);
            this.CH1.TabIndex = 50;
            this.CH1.Text = "1";
            this.CH1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.Location = new System.Drawing.Point(593, 160);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 49;
            this.label2.Text = "R:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.Location = new System.Drawing.Point(593, 142);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 48;
            this.label1.Text = "L:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LeftChannel
            // 
            this.LeftChannel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.LeftChannel.Location = new System.Drawing.Point(642, 142);
            this.LeftChannel.Maximum = 32768;
            this.LeftChannel.Name = "LeftChannel";
            this.LeftChannel.Size = new System.Drawing.Size(151, 13);
            this.LeftChannel.Step = 1;
            this.LeftChannel.TabIndex = 47;
            // 
            // CH16VOL
            // 
            this.CH16VOL.AutoSize = false;
            this.CH16VOL.LargeChange = 10;
            this.CH16VOL.Location = new System.Drawing.Point(507, 55);
            this.CH16VOL.Maximum = 127;
            this.CH16VOL.Name = "CH16VOL";
            this.CH16VOL.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.CH16VOL.Size = new System.Drawing.Size(27, 123);
            this.CH16VOL.TabIndex = 46;
            this.CH16VOL.TickFrequency = 16;
            this.CH16VOL.Scroll += new System.EventHandler(this.CH16VOL_Scroll);
            // 
            // CH15VOL
            // 
            this.CH15VOL.AutoSize = false;
            this.CH15VOL.LargeChange = 10;
            this.CH15VOL.Location = new System.Drawing.Point(474, 55);
            this.CH15VOL.Maximum = 127;
            this.CH15VOL.Name = "CH15VOL";
            this.CH15VOL.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.CH15VOL.Size = new System.Drawing.Size(27, 123);
            this.CH15VOL.TabIndex = 45;
            this.CH15VOL.TickFrequency = 16;
            this.CH15VOL.Scroll += new System.EventHandler(this.CH15VOL_Scroll);
            // 
            // CH14VOL
            // 
            this.CH14VOL.AutoSize = false;
            this.CH14VOL.LargeChange = 10;
            this.CH14VOL.Location = new System.Drawing.Point(441, 55);
            this.CH14VOL.Maximum = 127;
            this.CH14VOL.Name = "CH14VOL";
            this.CH14VOL.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.CH14VOL.Size = new System.Drawing.Size(27, 123);
            this.CH14VOL.TabIndex = 44;
            this.CH14VOL.TickFrequency = 16;
            this.CH14VOL.Scroll += new System.EventHandler(this.CH14VOL_Scroll);
            // 
            // CH13VOL
            // 
            this.CH13VOL.AutoSize = false;
            this.CH13VOL.LargeChange = 10;
            this.CH13VOL.Location = new System.Drawing.Point(408, 55);
            this.CH13VOL.Maximum = 127;
            this.CH13VOL.Name = "CH13VOL";
            this.CH13VOL.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.CH13VOL.Size = new System.Drawing.Size(27, 123);
            this.CH13VOL.TabIndex = 43;
            this.CH13VOL.TickFrequency = 16;
            this.CH13VOL.Scroll += new System.EventHandler(this.CH13VOL_Scroll);
            // 
            // CH12VOL
            // 
            this.CH12VOL.AutoSize = false;
            this.CH12VOL.LargeChange = 10;
            this.CH12VOL.Location = new System.Drawing.Point(375, 55);
            this.CH12VOL.Maximum = 127;
            this.CH12VOL.Name = "CH12VOL";
            this.CH12VOL.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.CH12VOL.Size = new System.Drawing.Size(27, 123);
            this.CH12VOL.TabIndex = 42;
            this.CH12VOL.TickFrequency = 16;
            this.CH12VOL.Scroll += new System.EventHandler(this.CH12VOL_Scroll);
            // 
            // CH11VOL
            // 
            this.CH11VOL.AutoSize = false;
            this.CH11VOL.LargeChange = 10;
            this.CH11VOL.Location = new System.Drawing.Point(344, 55);
            this.CH11VOL.Maximum = 127;
            this.CH11VOL.Name = "CH11VOL";
            this.CH11VOL.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.CH11VOL.Size = new System.Drawing.Size(27, 123);
            this.CH11VOL.TabIndex = 41;
            this.CH11VOL.TickFrequency = 16;
            this.CH11VOL.Scroll += new System.EventHandler(this.CH11VOL_Scroll);
            // 
            // CH10VOL
            // 
            this.CH10VOL.AutoSize = false;
            this.CH10VOL.LargeChange = 10;
            this.CH10VOL.Location = new System.Drawing.Point(311, 55);
            this.CH10VOL.Maximum = 127;
            this.CH10VOL.Name = "CH10VOL";
            this.CH10VOL.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.CH10VOL.Size = new System.Drawing.Size(27, 123);
            this.CH10VOL.TabIndex = 40;
            this.CH10VOL.TickFrequency = 16;
            this.CH10VOL.Scroll += new System.EventHandler(this.CH10VOL_Scroll);
            // 
            // CH9VOL
            // 
            this.CH9VOL.AutoSize = false;
            this.CH9VOL.LargeChange = 10;
            this.CH9VOL.Location = new System.Drawing.Point(278, 55);
            this.CH9VOL.Maximum = 127;
            this.CH9VOL.Name = "CH9VOL";
            this.CH9VOL.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.CH9VOL.Size = new System.Drawing.Size(27, 123);
            this.CH9VOL.TabIndex = 39;
            this.CH9VOL.TickFrequency = 16;
            this.CH9VOL.Scroll += new System.EventHandler(this.CH9VOL_Scroll);
            // 
            // CH8VOL
            // 
            this.CH8VOL.AutoSize = false;
            this.CH8VOL.LargeChange = 10;
            this.CH8VOL.Location = new System.Drawing.Point(245, 55);
            this.CH8VOL.Maximum = 127;
            this.CH8VOL.Name = "CH8VOL";
            this.CH8VOL.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.CH8VOL.Size = new System.Drawing.Size(27, 123);
            this.CH8VOL.TabIndex = 38;
            this.CH8VOL.TickFrequency = 16;
            this.CH8VOL.Scroll += new System.EventHandler(this.CH8VOL_Scroll);
            // 
            // CH7VOL
            // 
            this.CH7VOL.AutoSize = false;
            this.CH7VOL.LargeChange = 10;
            this.CH7VOL.Location = new System.Drawing.Point(212, 55);
            this.CH7VOL.Maximum = 127;
            this.CH7VOL.Name = "CH7VOL";
            this.CH7VOL.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.CH7VOL.Size = new System.Drawing.Size(27, 123);
            this.CH7VOL.TabIndex = 37;
            this.CH7VOL.TickFrequency = 16;
            this.CH7VOL.Scroll += new System.EventHandler(this.CH7VOL_Scroll);
            // 
            // CH6VOL
            // 
            this.CH6VOL.AutoSize = false;
            this.CH6VOL.LargeChange = 10;
            this.CH6VOL.Location = new System.Drawing.Point(179, 55);
            this.CH6VOL.Maximum = 127;
            this.CH6VOL.Name = "CH6VOL";
            this.CH6VOL.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.CH6VOL.Size = new System.Drawing.Size(27, 123);
            this.CH6VOL.TabIndex = 36;
            this.CH6VOL.TickFrequency = 16;
            this.CH6VOL.Scroll += new System.EventHandler(this.CH6VOL_Scroll);
            // 
            // CH5VOL
            // 
            this.CH5VOL.AutoSize = false;
            this.CH5VOL.LargeChange = 10;
            this.CH5VOL.Location = new System.Drawing.Point(146, 55);
            this.CH5VOL.Maximum = 127;
            this.CH5VOL.Name = "CH5VOL";
            this.CH5VOL.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.CH5VOL.Size = new System.Drawing.Size(27, 123);
            this.CH5VOL.TabIndex = 35;
            this.CH5VOL.TickFrequency = 16;
            this.CH5VOL.Scroll += new System.EventHandler(this.CH5VOL_Scroll);
            // 
            // CH4VOL
            // 
            this.CH4VOL.AutoSize = false;
            this.CH4VOL.LargeChange = 10;
            this.CH4VOL.Location = new System.Drawing.Point(113, 55);
            this.CH4VOL.Maximum = 127;
            this.CH4VOL.Name = "CH4VOL";
            this.CH4VOL.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.CH4VOL.Size = new System.Drawing.Size(27, 123);
            this.CH4VOL.TabIndex = 34;
            this.CH4VOL.TickFrequency = 16;
            this.CH4VOL.Scroll += new System.EventHandler(this.CH4VOL_Scroll);
            // 
            // CH3VOL
            // 
            this.CH3VOL.AutoSize = false;
            this.CH3VOL.LargeChange = 10;
            this.CH3VOL.Location = new System.Drawing.Point(80, 55);
            this.CH3VOL.Maximum = 127;
            this.CH3VOL.Name = "CH3VOL";
            this.CH3VOL.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.CH3VOL.Size = new System.Drawing.Size(27, 123);
            this.CH3VOL.TabIndex = 33;
            this.CH3VOL.TickFrequency = 16;
            this.CH3VOL.Scroll += new System.EventHandler(this.CH3VOL_Scroll);
            // 
            // CH2VOL
            // 
            this.CH2VOL.AutoSize = false;
            this.CH2VOL.LargeChange = 10;
            this.CH2VOL.Location = new System.Drawing.Point(47, 55);
            this.CH2VOL.Maximum = 127;
            this.CH2VOL.Name = "CH2VOL";
            this.CH2VOL.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.CH2VOL.Size = new System.Drawing.Size(27, 123);
            this.CH2VOL.TabIndex = 32;
            this.CH2VOL.TickFrequency = 16;
            this.CH2VOL.Scroll += new System.EventHandler(this.CH2VOL_Scroll);
            // 
            // CH1VOL
            // 
            this.CH1VOL.AutoSize = false;
            this.CH1VOL.LargeChange = 10;
            this.CH1VOL.Location = new System.Drawing.Point(14, 55);
            this.CH1VOL.Maximum = 127;
            this.CH1VOL.Name = "CH1VOL";
            this.CH1VOL.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.CH1VOL.Size = new System.Drawing.Size(27, 123);
            this.CH1VOL.TabIndex = 31;
            this.CH1VOL.TickFrequency = 16;
            this.CH1VOL.Scroll += new System.EventHandler(this.CH1VOL_Scroll);
            // 
            // Main
            // 
            this.Main.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showTheConfiguratorWindowToolStripMenuItem,
            this.resetToDefaultToolStripMenuItem,
            this.muteToolStripMenuItem,
            this.fullVolumeToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.Main.Location = new System.Drawing.Point(0, 0);
            this.Main.Name = "Main";
            this.Main.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.Main.Size = new System.Drawing.Size(805, 24);
            this.Main.TabIndex = 68;
            this.Main.Text = "menuStrip1";
            // 
            // RightChannel
            // 
            this.RightChannel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.RightChannel.Location = new System.Drawing.Point(642, 160);
            this.RightChannel.Maximum = 32768;
            this.RightChannel.Name = "RightChannel";
            this.RightChannel.Size = new System.Drawing.Size(151, 13);
            this.RightChannel.Step = 1;
            this.RightChannel.TabIndex = 30;
            // 
            // VolumeCheck
            // 
            this.VolumeCheck.DoWork += new System.ComponentModel.DoWorkEventHandler(this.VolumeCheck_DoWork);
            // 
            // KeppySynthMixerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(805, 185);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.MainVol);
            this.Controls.Add(this.CH16);
            this.Controls.Add(this.MIDIVolumeOverride);
            this.Controls.Add(this.VolumeMonitor);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.CH15);
            this.Controls.Add(this.CH14);
            this.Controls.Add(this.CH13);
            this.Controls.Add(this.CH12);
            this.Controls.Add(this.CH11);
            this.Controls.Add(this.CH10);
            this.Controls.Add(this.CH9);
            this.Controls.Add(this.CH8);
            this.Controls.Add(this.CH7);
            this.Controls.Add(this.CH6);
            this.Controls.Add(this.CH5);
            this.Controls.Add(this.CH4);
            this.Controls.Add(this.CH3);
            this.Controls.Add(this.CH2);
            this.Controls.Add(this.CH1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LeftChannel);
            this.Controls.Add(this.CH16VOL);
            this.Controls.Add(this.CH15VOL);
            this.Controls.Add(this.CH14VOL);
            this.Controls.Add(this.CH13VOL);
            this.Controls.Add(this.CH12VOL);
            this.Controls.Add(this.CH11VOL);
            this.Controls.Add(this.CH10VOL);
            this.Controls.Add(this.CH9VOL);
            this.Controls.Add(this.CH8VOL);
            this.Controls.Add(this.CH7VOL);
            this.Controls.Add(this.CH6VOL);
            this.Controls.Add(this.CH5VOL);
            this.Controls.Add(this.CH4VOL);
            this.Controls.Add(this.CH3VOL);
            this.Controls.Add(this.CH2VOL);
            this.Controls.Add(this.CH1VOL);
            this.Controls.Add(this.Main);
            this.Controls.Add(this.RightChannel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "KeppySynthMixerWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Keppy\'s Synthesizer Mixer";
            this.Load += new System.EventHandler(this.KeppySynthMixerWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.MainVol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH16VOL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH15VOL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH14VOL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH13VOL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH12VOL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH11VOL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH10VOL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH9VOL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH8VOL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH7VOL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH6VOL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH5VOL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH4VOL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH3VOL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH2VOL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH1VOL)).EndInit();
            this.Main.ResumeLayout(false);
            this.Main.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Timer ChannelVolume;
        private System.Windows.Forms.ToolStripMenuItem showTheConfiguratorWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetToDefaultToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem muteToolStripMenuItem;
        private System.Windows.Forms.TrackBar MainVol;
        private System.Windows.Forms.Label CH16;
        private System.Windows.Forms.ToolStripMenuItem fullVolumeToolStripMenuItem;
        private System.Windows.Forms.CheckBox MIDIVolumeOverride;
        private System.Windows.Forms.CheckBox VolumeMonitor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label CH15;
        private System.Windows.Forms.Label CH14;
        private System.Windows.Forms.Label CH13;
        private System.Windows.Forms.ToolTip VolumeTip;
        private System.Windows.Forms.Label CH12;
        private System.Windows.Forms.Label CH11;
        private System.Windows.Forms.Label CH10;
        private System.Windows.Forms.Label CH9;
        private System.Windows.Forms.Label CH8;
        private System.Windows.Forms.Label CH7;
        private System.Windows.Forms.Label CH6;
        private System.Windows.Forms.Label CH5;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Label CH4;
        private System.Windows.Forms.Label CH3;
        private System.Windows.Forms.Label CH2;
        private System.Windows.Forms.Label CH1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar LeftChannel;
        private System.Windows.Forms.TrackBar CH16VOL;
        private System.Windows.Forms.TrackBar CH15VOL;
        private System.Windows.Forms.TrackBar CH14VOL;
        private System.Windows.Forms.TrackBar CH13VOL;
        private System.Windows.Forms.TrackBar CH12VOL;
        private System.Windows.Forms.TrackBar CH11VOL;
        private System.Windows.Forms.TrackBar CH10VOL;
        private System.Windows.Forms.TrackBar CH9VOL;
        private System.Windows.Forms.TrackBar CH8VOL;
        private System.Windows.Forms.TrackBar CH7VOL;
        private System.Windows.Forms.TrackBar CH6VOL;
        private System.Windows.Forms.TrackBar CH5VOL;
        private System.Windows.Forms.TrackBar CH4VOL;
        private System.Windows.Forms.TrackBar CH3VOL;
        private System.Windows.Forms.TrackBar CH2VOL;
        private System.Windows.Forms.TrackBar CH1VOL;
        private System.Windows.Forms.MenuStrip Main;
        private System.Windows.Forms.ProgressBar RightChannel;
        private System.ComponentModel.BackgroundWorker VolumeCheck;
    }
}

