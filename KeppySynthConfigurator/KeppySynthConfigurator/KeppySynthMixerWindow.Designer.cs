namespace KeppySynthConfigurator
{
    partial class KeppySynthMixerWindow
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
            this.RightChannel = new System.Windows.Forms.ProgressBar();
            this.CH1VOL = new System.Windows.Forms.TrackBar();
            this.CH2VOL = new System.Windows.Forms.TrackBar();
            this.CH3VOL = new System.Windows.Forms.TrackBar();
            this.CH4VOL = new System.Windows.Forms.TrackBar();
            this.CH5VOL = new System.Windows.Forms.TrackBar();
            this.CH6VOL = new System.Windows.Forms.TrackBar();
            this.CH7VOL = new System.Windows.Forms.TrackBar();
            this.CH8VOL = new System.Windows.Forms.TrackBar();
            this.CH9VOL = new System.Windows.Forms.TrackBar();
            this.CH10VOL = new System.Windows.Forms.TrackBar();
            this.CH11VOL = new System.Windows.Forms.TrackBar();
            this.CH12VOL = new System.Windows.Forms.TrackBar();
            this.CH13VOL = new System.Windows.Forms.TrackBar();
            this.CH14VOL = new System.Windows.Forms.TrackBar();
            this.CH15VOL = new System.Windows.Forms.TrackBar();
            this.CH16VOL = new System.Windows.Forms.TrackBar();
            this.LeftChannel = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.VolumeCheck = new System.Windows.Forms.Timer(this.components);
            this.CH1 = new System.Windows.Forms.Label();
            this.CH2 = new System.Windows.Forms.Label();
            this.CH3 = new System.Windows.Forms.Label();
            this.CH4 = new System.Windows.Forms.Label();
            this.CH5 = new System.Windows.Forms.Label();
            this.CH6 = new System.Windows.Forms.Label();
            this.CH7 = new System.Windows.Forms.Label();
            this.CH8 = new System.Windows.Forms.Label();
            this.CH9 = new System.Windows.Forms.Label();
            this.CH10 = new System.Windows.Forms.Label();
            this.CH11 = new System.Windows.Forms.Label();
            this.CH12 = new System.Windows.Forms.Label();
            this.CH13 = new System.Windows.Forms.Label();
            this.CH14 = new System.Windows.Forms.Label();
            this.CH15 = new System.Windows.Forms.Label();
            this.CH16 = new System.Windows.Forms.Label();
            this.ChannelVolume = new System.Windows.Forms.Timer(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.VolumeMonitor = new System.Windows.Forms.CheckBox();
            this.Main = new System.Windows.Forms.MenuStrip();
            this.showTheConfiguratorWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToDefaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.muteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fullVolumeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.VolumeTip = new System.Windows.Forms.ToolTip(this.components);
            this.MIDIVolumeOverride = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.CH1VOL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH2VOL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH3VOL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH4VOL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH5VOL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH6VOL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH7VOL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH8VOL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH9VOL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH10VOL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH11VOL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH12VOL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH13VOL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH14VOL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH15VOL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH16VOL)).BeginInit();
            this.Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // RightChannel
            // 
            this.RightChannel.Location = new System.Drawing.Point(586, 146);
            this.RightChannel.Maximum = 32768;
            this.RightChannel.Name = "RightChannel";
            this.RightChannel.Size = new System.Drawing.Size(151, 23);
            this.RightChannel.Step = 1;
            this.RightChannel.TabIndex = 0;
            // 
            // CH1VOL
            // 
            this.CH1VOL.AutoSize = false;
            this.CH1VOL.LargeChange = 10;
            this.CH1VOL.Location = new System.Drawing.Point(12, 53);
            this.CH1VOL.Maximum = 127;
            this.CH1VOL.Name = "CH1VOL";
            this.CH1VOL.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.CH1VOL.Size = new System.Drawing.Size(27, 123);
            this.CH1VOL.TabIndex = 3;
            this.CH1VOL.TickFrequency = 16;
            this.CH1VOL.Scroll += new System.EventHandler(this.CH1VOL_Scroll);
            // 
            // CH2VOL
            // 
            this.CH2VOL.AutoSize = false;
            this.CH2VOL.LargeChange = 10;
            this.CH2VOL.Location = new System.Drawing.Point(45, 53);
            this.CH2VOL.Maximum = 127;
            this.CH2VOL.Name = "CH2VOL";
            this.CH2VOL.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.CH2VOL.Size = new System.Drawing.Size(27, 123);
            this.CH2VOL.TabIndex = 4;
            this.CH2VOL.TickFrequency = 16;
            this.CH2VOL.Scroll += new System.EventHandler(this.CH2VOL_Scroll);
            // 
            // CH3VOL
            // 
            this.CH3VOL.AutoSize = false;
            this.CH3VOL.LargeChange = 10;
            this.CH3VOL.Location = new System.Drawing.Point(78, 53);
            this.CH3VOL.Maximum = 127;
            this.CH3VOL.Name = "CH3VOL";
            this.CH3VOL.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.CH3VOL.Size = new System.Drawing.Size(27, 123);
            this.CH3VOL.TabIndex = 5;
            this.CH3VOL.TickFrequency = 16;
            this.CH3VOL.Scroll += new System.EventHandler(this.CH3VOL_Scroll);
            // 
            // CH4VOL
            // 
            this.CH4VOL.AutoSize = false;
            this.CH4VOL.LargeChange = 10;
            this.CH4VOL.Location = new System.Drawing.Point(111, 53);
            this.CH4VOL.Maximum = 127;
            this.CH4VOL.Name = "CH4VOL";
            this.CH4VOL.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.CH4VOL.Size = new System.Drawing.Size(27, 123);
            this.CH4VOL.TabIndex = 6;
            this.CH4VOL.TickFrequency = 16;
            this.CH4VOL.Scroll += new System.EventHandler(this.CH4VOL_Scroll);
            // 
            // CH5VOL
            // 
            this.CH5VOL.AutoSize = false;
            this.CH5VOL.LargeChange = 10;
            this.CH5VOL.Location = new System.Drawing.Point(144, 53);
            this.CH5VOL.Maximum = 127;
            this.CH5VOL.Name = "CH5VOL";
            this.CH5VOL.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.CH5VOL.Size = new System.Drawing.Size(27, 123);
            this.CH5VOL.TabIndex = 7;
            this.CH5VOL.TickFrequency = 16;
            this.CH5VOL.Scroll += new System.EventHandler(this.CH5VOL_Scroll);
            // 
            // CH6VOL
            // 
            this.CH6VOL.AutoSize = false;
            this.CH6VOL.LargeChange = 10;
            this.CH6VOL.Location = new System.Drawing.Point(177, 53);
            this.CH6VOL.Maximum = 127;
            this.CH6VOL.Name = "CH6VOL";
            this.CH6VOL.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.CH6VOL.Size = new System.Drawing.Size(27, 123);
            this.CH6VOL.TabIndex = 8;
            this.CH6VOL.TickFrequency = 16;
            this.CH6VOL.Scroll += new System.EventHandler(this.CH6VOL_Scroll);
            // 
            // CH7VOL
            // 
            this.CH7VOL.AutoSize = false;
            this.CH7VOL.LargeChange = 10;
            this.CH7VOL.Location = new System.Drawing.Point(210, 53);
            this.CH7VOL.Maximum = 127;
            this.CH7VOL.Name = "CH7VOL";
            this.CH7VOL.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.CH7VOL.Size = new System.Drawing.Size(27, 123);
            this.CH7VOL.TabIndex = 9;
            this.CH7VOL.TickFrequency = 16;
            this.CH7VOL.Scroll += new System.EventHandler(this.CH7VOL_Scroll);
            // 
            // CH8VOL
            // 
            this.CH8VOL.AutoSize = false;
            this.CH8VOL.LargeChange = 10;
            this.CH8VOL.Location = new System.Drawing.Point(243, 53);
            this.CH8VOL.Maximum = 127;
            this.CH8VOL.Name = "CH8VOL";
            this.CH8VOL.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.CH8VOL.Size = new System.Drawing.Size(27, 123);
            this.CH8VOL.TabIndex = 10;
            this.CH8VOL.TickFrequency = 16;
            this.CH8VOL.Scroll += new System.EventHandler(this.CH8VOL_Scroll);
            // 
            // CH9VOL
            // 
            this.CH9VOL.AutoSize = false;
            this.CH9VOL.LargeChange = 10;
            this.CH9VOL.Location = new System.Drawing.Point(276, 53);
            this.CH9VOL.Maximum = 127;
            this.CH9VOL.Name = "CH9VOL";
            this.CH9VOL.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.CH9VOL.Size = new System.Drawing.Size(27, 123);
            this.CH9VOL.TabIndex = 11;
            this.CH9VOL.TickFrequency = 16;
            this.CH9VOL.Scroll += new System.EventHandler(this.CH9VOL_Scroll);
            // 
            // CH10VOL
            // 
            this.CH10VOL.AutoSize = false;
            this.CH10VOL.LargeChange = 10;
            this.CH10VOL.Location = new System.Drawing.Point(309, 53);
            this.CH10VOL.Maximum = 127;
            this.CH10VOL.Name = "CH10VOL";
            this.CH10VOL.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.CH10VOL.Size = new System.Drawing.Size(27, 123);
            this.CH10VOL.TabIndex = 12;
            this.CH10VOL.TickFrequency = 16;
            this.CH10VOL.Scroll += new System.EventHandler(this.CH10VOL_Scroll);
            // 
            // CH11VOL
            // 
            this.CH11VOL.AutoSize = false;
            this.CH11VOL.LargeChange = 10;
            this.CH11VOL.Location = new System.Drawing.Point(342, 53);
            this.CH11VOL.Maximum = 127;
            this.CH11VOL.Name = "CH11VOL";
            this.CH11VOL.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.CH11VOL.Size = new System.Drawing.Size(27, 123);
            this.CH11VOL.TabIndex = 13;
            this.CH11VOL.TickFrequency = 16;
            this.CH11VOL.Scroll += new System.EventHandler(this.CH11VOL_Scroll);
            // 
            // CH12VOL
            // 
            this.CH12VOL.AutoSize = false;
            this.CH12VOL.LargeChange = 10;
            this.CH12VOL.Location = new System.Drawing.Point(373, 53);
            this.CH12VOL.Maximum = 127;
            this.CH12VOL.Name = "CH12VOL";
            this.CH12VOL.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.CH12VOL.Size = new System.Drawing.Size(27, 123);
            this.CH12VOL.TabIndex = 14;
            this.CH12VOL.TickFrequency = 16;
            this.CH12VOL.Scroll += new System.EventHandler(this.CH12VOL_Scroll);
            // 
            // CH13VOL
            // 
            this.CH13VOL.AutoSize = false;
            this.CH13VOL.LargeChange = 10;
            this.CH13VOL.Location = new System.Drawing.Point(406, 53);
            this.CH13VOL.Maximum = 127;
            this.CH13VOL.Name = "CH13VOL";
            this.CH13VOL.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.CH13VOL.Size = new System.Drawing.Size(27, 123);
            this.CH13VOL.TabIndex = 15;
            this.CH13VOL.TickFrequency = 16;
            this.CH13VOL.Scroll += new System.EventHandler(this.CH13VOL_Scroll);
            // 
            // CH14VOL
            // 
            this.CH14VOL.AutoSize = false;
            this.CH14VOL.LargeChange = 10;
            this.CH14VOL.Location = new System.Drawing.Point(439, 53);
            this.CH14VOL.Maximum = 127;
            this.CH14VOL.Name = "CH14VOL";
            this.CH14VOL.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.CH14VOL.Size = new System.Drawing.Size(27, 123);
            this.CH14VOL.TabIndex = 16;
            this.CH14VOL.TickFrequency = 16;
            this.CH14VOL.Scroll += new System.EventHandler(this.CH14VOL_Scroll);
            // 
            // CH15VOL
            // 
            this.CH15VOL.AutoSize = false;
            this.CH15VOL.LargeChange = 10;
            this.CH15VOL.Location = new System.Drawing.Point(472, 53);
            this.CH15VOL.Maximum = 127;
            this.CH15VOL.Name = "CH15VOL";
            this.CH15VOL.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.CH15VOL.Size = new System.Drawing.Size(27, 123);
            this.CH15VOL.TabIndex = 17;
            this.CH15VOL.TickFrequency = 16;
            this.CH15VOL.Scroll += new System.EventHandler(this.CH15VOL_Scroll);
            // 
            // CH16VOL
            // 
            this.CH16VOL.AutoSize = false;
            this.CH16VOL.LargeChange = 10;
            this.CH16VOL.Location = new System.Drawing.Point(505, 53);
            this.CH16VOL.Maximum = 127;
            this.CH16VOL.Name = "CH16VOL";
            this.CH16VOL.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.CH16VOL.Size = new System.Drawing.Size(27, 123);
            this.CH16VOL.TabIndex = 18;
            this.CH16VOL.TickFrequency = 16;
            this.CH16VOL.Scroll += new System.EventHandler(this.CH16VOL_Scroll);
            // 
            // LeftChannel
            // 
            this.LeftChannel.Location = new System.Drawing.Point(586, 117);
            this.LeftChannel.Maximum = 32768;
            this.LeftChannel.Name = "LeftChannel";
            this.LeftChannel.Size = new System.Drawing.Size(151, 23);
            this.LeftChannel.Step = 1;
            this.LeftChannel.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(556, 122);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Left:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(550, 151);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Right:";
            // 
            // VolumeCheck
            // 
            this.VolumeCheck.Enabled = true;
            this.VolumeCheck.Interval = 1;
            this.VolumeCheck.Tick += new System.EventHandler(this.VolumeCheck_Tick);
            // 
            // CH1
            // 
            this.CH1.Location = new System.Drawing.Point(9, 37);
            this.CH1.Name = "CH1";
            this.CH1.Size = new System.Drawing.Size(27, 13);
            this.CH1.TabIndex = 22;
            this.CH1.Text = "1";
            this.CH1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CH2
            // 
            this.CH2.Location = new System.Drawing.Point(42, 37);
            this.CH2.Name = "CH2";
            this.CH2.Size = new System.Drawing.Size(27, 13);
            this.CH2.TabIndex = 23;
            this.CH2.Text = "2";
            this.CH2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CH3
            // 
            this.CH3.Location = new System.Drawing.Point(75, 37);
            this.CH3.Name = "CH3";
            this.CH3.Size = new System.Drawing.Size(27, 13);
            this.CH3.TabIndex = 23;
            this.CH3.Text = "3";
            this.CH3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CH4
            // 
            this.CH4.Location = new System.Drawing.Point(108, 37);
            this.CH4.Name = "CH4";
            this.CH4.Size = new System.Drawing.Size(27, 13);
            this.CH4.TabIndex = 23;
            this.CH4.Text = "4";
            this.CH4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CH5
            // 
            this.CH5.Location = new System.Drawing.Point(141, 37);
            this.CH5.Name = "CH5";
            this.CH5.Size = new System.Drawing.Size(27, 13);
            this.CH5.TabIndex = 23;
            this.CH5.Text = "5";
            this.CH5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CH6
            // 
            this.CH6.Location = new System.Drawing.Point(174, 37);
            this.CH6.Name = "CH6";
            this.CH6.Size = new System.Drawing.Size(27, 13);
            this.CH6.TabIndex = 23;
            this.CH6.Text = "6";
            this.CH6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CH7
            // 
            this.CH7.Location = new System.Drawing.Point(207, 37);
            this.CH7.Name = "CH7";
            this.CH7.Size = new System.Drawing.Size(27, 13);
            this.CH7.TabIndex = 23;
            this.CH7.Text = "7";
            this.CH7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CH8
            // 
            this.CH8.Location = new System.Drawing.Point(240, 37);
            this.CH8.Name = "CH8";
            this.CH8.Size = new System.Drawing.Size(27, 13);
            this.CH8.TabIndex = 23;
            this.CH8.Text = "8";
            this.CH8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CH9
            // 
            this.CH9.Location = new System.Drawing.Point(273, 37);
            this.CH9.Name = "CH9";
            this.CH9.Size = new System.Drawing.Size(27, 13);
            this.CH9.TabIndex = 23;
            this.CH9.Text = "9";
            this.CH9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CH10
            // 
            this.CH10.Location = new System.Drawing.Point(306, 37);
            this.CH10.Name = "CH10";
            this.CH10.Size = new System.Drawing.Size(27, 13);
            this.CH10.TabIndex = 23;
            this.CH10.Text = "10";
            this.CH10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CH11
            // 
            this.CH11.Location = new System.Drawing.Point(339, 37);
            this.CH11.Name = "CH11";
            this.CH11.Size = new System.Drawing.Size(27, 13);
            this.CH11.TabIndex = 23;
            this.CH11.Text = "11";
            this.CH11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CH12
            // 
            this.CH12.Location = new System.Drawing.Point(370, 37);
            this.CH12.Name = "CH12";
            this.CH12.Size = new System.Drawing.Size(27, 13);
            this.CH12.TabIndex = 23;
            this.CH12.Text = "12";
            this.CH12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CH13
            // 
            this.CH13.Location = new System.Drawing.Point(403, 37);
            this.CH13.Name = "CH13";
            this.CH13.Size = new System.Drawing.Size(27, 13);
            this.CH13.TabIndex = 23;
            this.CH13.Text = "13";
            this.CH13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CH14
            // 
            this.CH14.Location = new System.Drawing.Point(436, 37);
            this.CH14.Name = "CH14";
            this.CH14.Size = new System.Drawing.Size(27, 13);
            this.CH14.TabIndex = 23;
            this.CH14.Text = "14";
            this.CH14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CH15
            // 
            this.CH15.Location = new System.Drawing.Point(469, 37);
            this.CH15.Name = "CH15";
            this.CH15.Size = new System.Drawing.Size(27, 13);
            this.CH15.TabIndex = 23;
            this.CH15.Text = "15";
            this.CH15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CH16
            // 
            this.CH16.Location = new System.Drawing.Point(502, 37);
            this.CH16.Name = "CH16";
            this.CH16.Size = new System.Drawing.Size(27, 13);
            this.CH16.TabIndex = 23;
            this.CH16.Text = "16";
            this.CH16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ChannelVolume
            // 
            this.ChannelVolume.Enabled = true;
            this.ChannelVolume.Interval = 1;
            this.ChannelVolume.Tick += new System.EventHandler(this.ChannelVolume_Tick);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(556, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(181, 29);
            this.label3.TabIndex = 24;
            this.label3.Text = "The volume meter only works when the XAudio interface is disabled.";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // VolumeMonitor
            // 
            this.VolumeMonitor.AutoSize = true;
            this.VolumeMonitor.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.VolumeMonitor.Location = new System.Drawing.Point(589, 94);
            this.VolumeMonitor.Name = "VolumeMonitor";
            this.VolumeMonitor.Size = new System.Drawing.Size(148, 17);
            this.VolumeMonitor.TabIndex = 25;
            this.VolumeMonitor.Text = "Enable volume monitoring";
            this.VolumeMonitor.UseVisualStyleBackColor = true;
            this.VolumeMonitor.CheckedChanged += new System.EventHandler(this.VolumeMonitor_CheckedChanged);
            // 
            // Main
            // 
            this.Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showTheConfiguratorWindowToolStripMenuItem,
            this.resetToDefaultToolStripMenuItem,
            this.muteToolStripMenuItem,
            this.fullVolumeToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.Main.Location = new System.Drawing.Point(0, 0);
            this.Main.Name = "Main";
            this.Main.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.Main.Size = new System.Drawing.Size(748, 24);
            this.Main.TabIndex = 26;
            this.Main.Text = "menuStrip1";
            // 
            // showTheConfiguratorWindowToolStripMenuItem
            // 
            this.showTheConfiguratorWindowToolStripMenuItem.Name = "showTheConfiguratorWindowToolStripMenuItem";
            this.showTheConfiguratorWindowToolStripMenuItem.Size = new System.Drawing.Size(182, 20);
            this.showTheConfiguratorWindowToolStripMenuItem.Text = "Show the configurator window";
            this.showTheConfiguratorWindowToolStripMenuItem.Visible = false;
            this.showTheConfiguratorWindowToolStripMenuItem.Click += new System.EventHandler(this.showTheConfiguratorWindowToolStripMenuItem_Click);
            // 
            // resetToDefaultToolStripMenuItem
            // 
            this.resetToDefaultToolStripMenuItem.Name = "resetToDefaultToolStripMenuItem";
            this.resetToDefaultToolStripMenuItem.Size = new System.Drawing.Size(101, 20);
            this.resetToDefaultToolStripMenuItem.Text = "Reset to default";
            this.resetToDefaultToolStripMenuItem.Click += new System.EventHandler(this.resetToDefaultToolStripMenuItem_Click);
            // 
            // muteToolStripMenuItem
            // 
            this.muteToolStripMenuItem.Name = "muteToolStripMenuItem";
            this.muteToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.muteToolStripMenuItem.Text = "Mute";
            this.muteToolStripMenuItem.Click += new System.EventHandler(this.muteToolStripMenuItem_Click);
            // 
            // fullVolumeToolStripMenuItem
            // 
            this.fullVolumeToolStripMenuItem.Name = "fullVolumeToolStripMenuItem";
            this.fullVolumeToolStripMenuItem.Size = new System.Drawing.Size(81, 20);
            this.fullVolumeToolStripMenuItem.Text = "Full volume";
            this.fullVolumeToolStripMenuItem.Click += new System.EventHandler(this.fullVolumeToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // VolumeTip
            // 
            this.VolumeTip.AutomaticDelay = 0;
            // 
            // MIDIVolumeOverride
            // 
            this.MIDIVolumeOverride.AutoSize = true;
            this.MIDIVolumeOverride.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.MIDIVolumeOverride.Location = new System.Drawing.Point(542, 76);
            this.MIDIVolumeOverride.Name = "MIDIVolumeOverride";
            this.MIDIVolumeOverride.Size = new System.Drawing.Size(195, 17);
            this.MIDIVolumeOverride.TabIndex = 27;
            this.MIDIVolumeOverride.Text = "Enable MIDI volume event override";
            this.MIDIVolumeOverride.UseVisualStyleBackColor = true;
            this.MIDIVolumeOverride.CheckedChanged += new System.EventHandler(this.MIDIVolumeOverride_CheckedChanged);
            // 
            // KeppySynthMixerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(748, 181);
            this.Controls.Add(this.MIDIVolumeOverride);
            this.Controls.Add(this.VolumeMonitor);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.CH16);
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
            this.Controls.Add(this.RightChannel);
            this.Controls.Add(this.Main);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MainMenuStrip = this.Main;
            this.MaximizeBox = false;
            this.Name = "KeppySynthMixerWindow";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Keppy\'s Synthesizer Mixer";
            this.Load += new System.EventHandler(this.KeppyDriverMixerWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.CH1VOL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH2VOL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH3VOL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH4VOL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH5VOL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH6VOL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH7VOL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH8VOL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH9VOL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH10VOL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH11VOL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH12VOL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH13VOL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH14VOL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH15VOL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CH16VOL)).EndInit();
            this.Main.ResumeLayout(false);
            this.Main.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar RightChannel;
        private System.Windows.Forms.TrackBar CH1VOL;
        private System.Windows.Forms.TrackBar CH2VOL;
        private System.Windows.Forms.TrackBar CH3VOL;
        private System.Windows.Forms.TrackBar CH4VOL;
        private System.Windows.Forms.TrackBar CH5VOL;
        private System.Windows.Forms.TrackBar CH6VOL;
        private System.Windows.Forms.TrackBar CH7VOL;
        private System.Windows.Forms.TrackBar CH8VOL;
        private System.Windows.Forms.TrackBar CH9VOL;
        private System.Windows.Forms.TrackBar CH10VOL;
        private System.Windows.Forms.TrackBar CH11VOL;
        private System.Windows.Forms.TrackBar CH12VOL;
        private System.Windows.Forms.TrackBar CH13VOL;
        private System.Windows.Forms.TrackBar CH14VOL;
        private System.Windows.Forms.TrackBar CH15VOL;
        private System.Windows.Forms.TrackBar CH16VOL;
        private System.Windows.Forms.ProgressBar LeftChannel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer VolumeCheck;
        private System.Windows.Forms.Label CH1;
        private System.Windows.Forms.Label CH2;
        private System.Windows.Forms.Label CH3;
        private System.Windows.Forms.Label CH4;
        private System.Windows.Forms.Label CH5;
        private System.Windows.Forms.Label CH6;
        private System.Windows.Forms.Label CH7;
        private System.Windows.Forms.Label CH8;
        private System.Windows.Forms.Label CH9;
        private System.Windows.Forms.Label CH10;
        private System.Windows.Forms.Label CH11;
        private System.Windows.Forms.Label CH12;
        private System.Windows.Forms.Label CH13;
        private System.Windows.Forms.Label CH14;
        private System.Windows.Forms.Label CH15;
        private System.Windows.Forms.Label CH16;
        private System.Windows.Forms.Timer ChannelVolume;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox VolumeMonitor;
        private System.Windows.Forms.MenuStrip Main;
        private System.Windows.Forms.ToolStripMenuItem resetToDefaultToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem muteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolTip VolumeTip;
        private System.Windows.Forms.ToolStripMenuItem fullVolumeToolStripMenuItem;
        private System.Windows.Forms.CheckBox MIDIVolumeOverride;
        private System.Windows.Forms.ToolStripMenuItem showTheConfiguratorWindowToolStripMenuItem;
    }
}