namespace KeppySynthDebugWindow
{
    partial class KeppySynthDebugWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KeppySynthDebugWindow));
            this.MainCont = new System.Windows.Forms.ContextMenu();
            this.OpenConfigurator = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.OpenAppLocat = new System.Windows.Forms.MenuItem();
            this.debugwintop = new System.Windows.Forms.MenuItem();
            this.CopyToClipboard = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.ExitMenu = new System.Windows.Forms.MenuItem();
            this.DebugWorker = new System.ComponentModel.BackgroundWorker();
            this.Tabs = new System.Windows.Forms.TabControl();
            this.SynthDbg = new System.Windows.Forms.TabPage();
            this.CopyToClip1 = new System.Windows.Forms.Button();
            this.KSLogo = new System.Windows.Forms.PictureBox();
            this.DDS = new System.Windows.Forms.Label();
            this.DDSLabel = new System.Windows.Forms.Label();
            this.RT = new System.Windows.Forms.Label();
            this.RTLabel = new System.Windows.Forms.Label();
            this.AV = new System.Windows.Forms.Label();
            this.AVLabel = new System.Windows.Forms.Label();
            this.CMA = new System.Windows.Forms.Label();
            this.CMALabel = new System.Windows.Forms.Label();
            this.PCSpecs = new System.Windows.Forms.TabPage();
            this.CPULogo = new System.Windows.Forms.PictureBox();
            this.MT = new System.Windows.Forms.Label();
            this.MTLabel = new System.Windows.Forms.Label();
            this.GPUInternalChip = new System.Windows.Forms.Label();
            this.GPUInternalChipLabel = new System.Windows.Forms.Label();
            this.CopyToClip2 = new System.Windows.Forms.Button();
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
            this.MemoryThread = new System.Windows.Forms.Timer(this.components);
            this.Tabs.SuspendLayout();
            this.SynthDbg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.KSLogo)).BeginInit();
            this.PCSpecs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CPULogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WinLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // MainCont
            // 
            this.MainCont.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.OpenConfigurator,
            this.menuItem3,
            this.OpenAppLocat,
            this.debugwintop,
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
            // debugwintop
            // 
            this.debugwintop.Index = 3;
            this.debugwintop.Text = "Keep debug window on top";
            this.debugwintop.Click += new System.EventHandler(this.debugwintop_Click);
            // 
            // CopyToClipboard
            // 
            this.CopyToClipboard.Index = 4;
            this.CopyToClipboard.Text = "Copy info from all tabs to clipboard";
            this.CopyToClipboard.Click += new System.EventHandler(this.CopyToClipboard_Click);
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
            // DebugWorker
            // 
            this.DebugWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.DebugWorker_DoWork);
            // 
            // Tabs
            // 
            this.Tabs.Controls.Add(this.SynthDbg);
            this.Tabs.Controls.Add(this.PCSpecs);
            this.Tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tabs.Location = new System.Drawing.Point(0, 0);
            this.Tabs.Name = "Tabs";
            this.Tabs.SelectedIndex = 0;
            this.Tabs.Size = new System.Drawing.Size(432, 206);
            this.Tabs.TabIndex = 8;
            // 
            // SynthDbg
            // 
            this.SynthDbg.Controls.Add(this.CopyToClip1);
            this.SynthDbg.Controls.Add(this.KSLogo);
            this.SynthDbg.Controls.Add(this.DDS);
            this.SynthDbg.Controls.Add(this.DDSLabel);
            this.SynthDbg.Controls.Add(this.RT);
            this.SynthDbg.Controls.Add(this.RTLabel);
            this.SynthDbg.Controls.Add(this.AV);
            this.SynthDbg.Controls.Add(this.AVLabel);
            this.SynthDbg.Controls.Add(this.CMA);
            this.SynthDbg.Controls.Add(this.CMALabel);
            this.SynthDbg.Location = new System.Drawing.Point(4, 22);
            this.SynthDbg.Name = "SynthDbg";
            this.SynthDbg.Padding = new System.Windows.Forms.Padding(3);
            this.SynthDbg.Size = new System.Drawing.Size(424, 180);
            this.SynthDbg.TabIndex = 0;
            this.SynthDbg.Text = "Synth debug info";
            this.SynthDbg.UseVisualStyleBackColor = true;
            // 
            // CopyToClip1
            // 
            this.CopyToClip1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CopyToClip1.Location = new System.Drawing.Point(266, 152);
            this.CopyToClip1.Name = "CopyToClip1";
            this.CopyToClip1.Size = new System.Drawing.Size(153, 23);
            this.CopyToClip1.TabIndex = 34;
            this.CopyToClip1.Text = "Copy both tabs to clipboard";
            this.CopyToClip1.UseVisualStyleBackColor = true;
            this.CopyToClip1.Click += new System.EventHandler(this.CopyToClip_Click);
            // 
            // KSLogo
            // 
            this.KSLogo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.KSLogo.Image = global::KeppySynthDebugWindow.Properties.Resources.DebugIcon;
            this.KSLogo.Location = new System.Drawing.Point(389, 3);
            this.KSLogo.Name = "KSLogo";
            this.KSLogo.Size = new System.Drawing.Size(32, 32);
            this.KSLogo.TabIndex = 25;
            this.KSLogo.TabStop = false;
            // 
            // DDS
            // 
            this.DDS.AutoSize = true;
            this.DDS.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DDS.Location = new System.Drawing.Point(161, 60);
            this.DDS.Name = "DDS";
            this.DDS.Size = new System.Drawing.Size(45, 13);
            this.DDS.TabIndex = 15;
            this.DDS.Text = "0 (0 x 4)";
            this.DDS.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DDSLabel
            // 
            this.DDSLabel.AutoSize = true;
            this.DDSLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DDSLabel.Location = new System.Drawing.Point(2, 60);
            this.DDSLabel.Name = "DDSLabel";
            this.DDSLabel.Size = new System.Drawing.Size(159, 13);
            this.DDSLabel.TabIndex = 14;
            this.DDSLabel.Text = "Decoded data size (bytes):";
            this.DDSLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.CMA.Location = new System.Drawing.Point(110, 6);
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
            // PCSpecs
            // 
            this.PCSpecs.Controls.Add(this.CPULogo);
            this.PCSpecs.Controls.Add(this.MT);
            this.PCSpecs.Controls.Add(this.MTLabel);
            this.PCSpecs.Controls.Add(this.GPUInternalChip);
            this.PCSpecs.Controls.Add(this.GPUInternalChipLabel);
            this.PCSpecs.Controls.Add(this.CopyToClip2);
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
            this.PCSpecs.Size = new System.Drawing.Size(424, 180);
            this.PCSpecs.TabIndex = 1;
            this.PCSpecs.Text = "Computer specifications";
            this.PCSpecs.UseVisualStyleBackColor = true;
            // 
            // CPULogo
            // 
            this.CPULogo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CPULogo.Cursor = System.Windows.Forms.Cursors.Help;
            this.CPULogo.Image = global::KeppySynthDebugWindow.Properties.Resources.unknown;
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
            // CopyToClip2
            // 
            this.CopyToClip2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CopyToClip2.Location = new System.Drawing.Point(266, 152);
            this.CopyToClip2.Name = "CopyToClip2";
            this.CopyToClip2.Size = new System.Drawing.Size(153, 23);
            this.CopyToClip2.TabIndex = 33;
            this.CopyToClip2.Text = "Copy both tabs to clipboard";
            this.CopyToClip2.UseVisualStyleBackColor = true;
            this.CopyToClip2.Click += new System.EventHandler(this.CopyToClip_Click);
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
            this.WinLogo.Image = global::KeppySynthDebugWindow.Properties.Resources.unknown;
            this.WinLogo.Location = new System.Drawing.Point(389, 3);
            this.WinLogo.Name = "WinLogo";
            this.WinLogo.Size = new System.Drawing.Size(32, 32);
            this.WinLogo.TabIndex = 24;
            this.WinLogo.TabStop = false;
            // 
            // MemoryThread
            // 
            this.MemoryThread.Enabled = true;
            this.MemoryThread.Tick += new System.EventHandler(this.MemoryThread_Tick);
            // 
            // KeppySynthDebugWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(432, 206);
            this.Controls.Add(this.Tabs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "KeppySynthDebugWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Keppy\'s Synthesizer Debug Window";
            this.Load += new System.EventHandler(this.KeppySynthDebugWindow_Load);
            this.Tabs.ResumeLayout(false);
            this.SynthDbg.ResumeLayout(false);
            this.SynthDbg.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.KSLogo)).EndInit();
            this.PCSpecs.ResumeLayout(false);
            this.PCSpecs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CPULogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WinLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenu MainCont;
        private System.Windows.Forms.MenuItem OpenAppLocat;
        private System.Windows.Forms.MenuItem CopyToClipboard;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem ExitMenu;
        private System.ComponentModel.BackgroundWorker DebugWorker;
        private System.Windows.Forms.MenuItem OpenConfigurator;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem debugwintop;
        private System.Windows.Forms.TabControl Tabs;
        private System.Windows.Forms.TabPage SynthDbg;
        private System.Windows.Forms.Label CMALabel;
        private System.Windows.Forms.TabPage PCSpecs;
        private System.Windows.Forms.Label AV;
        private System.Windows.Forms.Label AVLabel;
        private System.Windows.Forms.Label CMA;
        private System.Windows.Forms.Label RT;
        private System.Windows.Forms.Label RTLabel;
        private System.Windows.Forms.Label DDS;
        private System.Windows.Forms.Label DDSLabel;
        private System.Windows.Forms.Label COS;
        private System.Windows.Forms.Label COSLabel;
        private System.Windows.Forms.Label AM;
        private System.Windows.Forms.Label AMLabel;
        private System.Windows.Forms.Label TM;
        private System.Windows.Forms.Label TMLabel;
        private System.Windows.Forms.Timer MemoryThread;
        private System.Windows.Forms.PictureBox WinLogo;
        private System.Windows.Forms.Label CPU;
        private System.Windows.Forms.Label CPULabel;
        private System.Windows.Forms.Label CPUInfo;
        private System.Windows.Forms.Label CPUInfoLabel;
        private System.Windows.Forms.Label GPUInfo;
        private System.Windows.Forms.Label GPUInfoLabel;
        private System.Windows.Forms.Label GPU;
        private System.Windows.Forms.Label GPULabel;
        private System.Windows.Forms.Button CopyToClip2;
        private System.Windows.Forms.PictureBox KSLogo;
        private System.Windows.Forms.Button CopyToClip1;
        private System.Windows.Forms.Label GPUInternalChip;
        private System.Windows.Forms.Label GPUInternalChipLabel;
        private System.Windows.Forms.Label MT;
        private System.Windows.Forms.Label MTLabel;
        private System.Windows.Forms.PictureBox CPULogo;
    }
}

