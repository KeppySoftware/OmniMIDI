namespace OmniMIDIConfigurator
{
    partial class SettingsPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsPanel));
            this.EnginesBox = new System.Windows.Forms.GroupBox();
            this.NoSFGenLimits = new System.Windows.Forms.CheckBox();
            this.LinDecVol = new System.Windows.Forms.CheckBox();
            this.LinAttMod = new System.Windows.Forms.CheckBox();
            this.AudioRampIn = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.MaxCPU = new System.Windows.Forms.NumericUpDown();
            this.RenderingTimeLabel = new System.Windows.Forms.Label();
            this.VolSimView = new System.Windows.Forms.Label();
            this.VoiceLimitLabel = new System.Windows.Forms.Label();
            this.PolyphonyLimit = new System.Windows.Forms.NumericUpDown();
            this.VolLabel = new System.Windows.Forms.Label();
            this.DrvHzLabel = new System.Windows.Forms.Label();
            this.Frequency = new System.Windows.Forms.ComboBox();
            this.BufferText = new System.Windows.Forms.Label();
            this.bufsize = new System.Windows.Forms.NumericUpDown();
            this.SincConv = new System.Windows.Forms.ComboBox();
            this.SincConvLab = new System.Windows.Forms.Label();
            this.SincInter = new System.Windows.Forms.CheckBox();
            this.EnableSFX = new System.Windows.Forms.CheckBox();
            this.ChangeDefaultOutput = new System.Windows.Forms.Button();
            this.MonophonicFunc = new System.Windows.Forms.CheckBox();
            this.FadeoutDisable = new System.Windows.Forms.CheckBox();
            this.AudioBitDepth = new System.Windows.Forms.ComboBox();
            this.AudioBitDepthLabel = new System.Windows.Forms.Label();
            this.AudioEngBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.HMode = new System.Windows.Forms.CheckBox();
            this.KSDAPIBoxWhat = new System.Windows.Forms.PictureBox();
            this.KSDAPIBox = new System.Windows.Forms.CheckBox();
            this.SlowDownPlayback = new System.Windows.Forms.CheckBox();
            this.OldBuff = new System.Windows.Forms.CheckBox();
            this.SynthBox = new System.Windows.Forms.GroupBox();
            this.AutoLoad = new System.Windows.Forms.CheckBox();
            this.PrioLab = new System.Windows.Forms.Label();
            this.IgnoreNotesLV = new System.Windows.Forms.NumericUpDown();
            this.PrioBox = new System.Windows.Forms.ComboBox();
            this.IgnoreNotesHV = new System.Windows.Forms.NumericUpDown();
            this.IgnoreNotesHL = new System.Windows.Forms.Label();
            this.IgnoreNotesLL = new System.Windows.Forms.Label();
            this.SysResetIgnore = new System.Windows.Forms.CheckBox();
            this.Preload = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.NoteOffCheck = new System.Windows.Forms.CheckBox();
            this.CBRuler = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.Limit88 = new System.Windows.Forms.CheckBox();
            this.NoteOffDelayValue = new System.Windows.Forms.NumericUpDown();
            this.IgnoreNotes = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.OverrideNoteLengthWA2 = new System.Windows.Forms.PictureBox();
            this.DelayNoteOff = new System.Windows.Forms.CheckBox();
            this.NoteLengthValueMS = new System.Windows.Forms.Label();
            this.NoteLengthValue = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.OverrideNoteLengthWA1 = new System.Windows.Forms.PictureBox();
            this.OverrideNoteLength = new System.Windows.Forms.CheckBox();
            this.IgnoreAllEvents = new System.Windows.Forms.CheckBox();
            this.FullVelocityMode = new System.Windows.Forms.CheckBox();
            this.ButtonsDesc = new System.Windows.Forms.ToolTip(this.components);
            this.Requirements = new System.Windows.Forms.ToolTip(this.components);
            this.DebugMode = new System.Windows.Forms.CheckBox();
            this.IgnoreCloseCalls = new System.Windows.Forms.CheckBox();
            this.UseTGT = new System.Windows.Forms.CheckBox();
            this.VolTrackBarMenu = new System.Windows.Forms.ContextMenu();
            this.FineTuneKnobIt = new System.Windows.Forms.MenuItem();
            this.menuItem57 = new System.Windows.Forms.MenuItem();
            this.VolumeBoost = new System.Windows.Forms.MenuItem();
            this.LiveChangesTrigger = new System.Windows.Forms.CheckBox();
            this.label15 = new System.Windows.Forms.Label();
            this.FastHotKeys = new System.Windows.Forms.CheckBox();
            this.LegacySetDia = new System.Windows.Forms.GroupBox();
            this.Troubleshooter = new System.Windows.Forms.Button();
            this.ShowChangelogUpdate = new System.Windows.Forms.CheckBox();
            this.AsyncProcessing = new System.Windows.Forms.CheckBox();
            this.MinidumpsFolder = new OmniMIDIConfigurator.LinkLabelEx();
            this.MIDIFeedbackTool = new OmniMIDIConfigurator.LinkLabelEx();
            this.WinMMSpeedDiag = new OmniMIDIConfigurator.LinkLabelEx();
            this.DebugModeFolder = new OmniMIDIConfigurator.LinkLabelEx();
            this.SpatialSound = new OmniMIDIConfigurator.LinkLabelEx();
            this.ChangeEVBuf = new OmniMIDIConfigurator.LinkLabelEx();
            this.PitchShifting = new OmniMIDIConfigurator.LinkLabelEx();
            this.DSPSettingsBox = new OmniMIDIConfigurator.LinkLabelEx();
            this.VolKnob = new KnobControl.KnobControl();
            this.EnginesBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MaxCPU)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PolyphonyLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bufsize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.KSDAPIBoxWhat)).BeginInit();
            this.SynthBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IgnoreNotesLV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IgnoreNotesHV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NoteOffDelayValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OverrideNoteLengthWA2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NoteLengthValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OverrideNoteLengthWA1)).BeginInit();
            this.LegacySetDia.SuspendLayout();
            this.SuspendLayout();
            // 
            // EnginesBox
            // 
            this.EnginesBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EnginesBox.Controls.Add(this.DSPSettingsBox);
            this.EnginesBox.Controls.Add(this.NoSFGenLimits);
            this.EnginesBox.Controls.Add(this.LinDecVol);
            this.EnginesBox.Controls.Add(this.LinAttMod);
            this.EnginesBox.Controls.Add(this.AudioRampIn);
            this.EnginesBox.Controls.Add(this.label11);
            this.EnginesBox.Controls.Add(this.label10);
            this.EnginesBox.Controls.Add(this.label9);
            this.EnginesBox.Controls.Add(this.label8);
            this.EnginesBox.Controls.Add(this.MaxCPU);
            this.EnginesBox.Controls.Add(this.RenderingTimeLabel);
            this.EnginesBox.Controls.Add(this.VolSimView);
            this.EnginesBox.Controls.Add(this.VoiceLimitLabel);
            this.EnginesBox.Controls.Add(this.PolyphonyLimit);
            this.EnginesBox.Controls.Add(this.VolLabel);
            this.EnginesBox.Controls.Add(this.DrvHzLabel);
            this.EnginesBox.Controls.Add(this.Frequency);
            this.EnginesBox.Controls.Add(this.VolKnob);
            this.EnginesBox.Controls.Add(this.BufferText);
            this.EnginesBox.Controls.Add(this.bufsize);
            this.EnginesBox.Controls.Add(this.SincConv);
            this.EnginesBox.Controls.Add(this.SincConvLab);
            this.EnginesBox.Controls.Add(this.SincInter);
            this.EnginesBox.Controls.Add(this.EnableSFX);
            this.EnginesBox.Controls.Add(this.ChangeDefaultOutput);
            this.EnginesBox.Controls.Add(this.MonophonicFunc);
            this.EnginesBox.Controls.Add(this.FadeoutDisable);
            this.EnginesBox.Controls.Add(this.AudioBitDepth);
            this.EnginesBox.Controls.Add(this.AudioBitDepthLabel);
            this.EnginesBox.Controls.Add(this.AudioEngBox);
            this.EnginesBox.Controls.Add(this.label2);
            this.EnginesBox.Location = new System.Drawing.Point(3, 3);
            this.EnginesBox.Name = "EnginesBox";
            this.EnginesBox.Size = new System.Drawing.Size(782, 486);
            this.EnginesBox.TabIndex = 0;
            this.EnginesBox.TabStop = false;
            this.EnginesBox.Text = "Audio engine settings";
            // 
            // NoSFGenLimits
            // 
            this.NoSFGenLimits.AutoSize = true;
            this.NoSFGenLimits.Location = new System.Drawing.Point(10, 435);
            this.NoSFGenLimits.Name = "NoSFGenLimits";
            this.NoSFGenLimits.Size = new System.Drawing.Size(432, 19);
            this.NoSFGenLimits.TabIndex = 63;
            this.NoSFGenLimits.Text = "Do not limit SF2 generator values to emulate Creative/SoundBlaster hardware";
            this.NoSFGenLimits.UseVisualStyleBackColor = true;
            // 
            // LinDecVol
            // 
            this.LinDecVol.AutoSize = true;
            this.LinDecVol.Location = new System.Drawing.Point(10, 413);
            this.LinDecVol.Name = "LinDecVol";
            this.LinDecVol.Size = new System.Drawing.Size(504, 19);
            this.LinDecVol.TabIndex = 62;
            this.LinDecVol.Text = "Use linear decay and release phases in volume envelopes (The attack phase is alwa" +
    "ys linear)";
            this.LinDecVol.UseVisualStyleBackColor = true;
            // 
            // LinAttMod
            // 
            this.LinAttMod.AutoSize = true;
            this.LinAttMod.Location = new System.Drawing.Point(10, 391);
            this.LinAttMod.Name = "LinAttMod";
            this.LinAttMod.Size = new System.Drawing.Size(612, 19);
            this.LinAttMod.TabIndex = 61;
            this.LinAttMod.Text = "Use linear attack phase in SoundFont modulation envelopes (The attack phase is al" +
    "ways linear in SFZ envelopes)";
            this.LinAttMod.UseVisualStyleBackColor = true;
            // 
            // AudioRampIn
            // 
            this.AudioRampIn.AutoSize = true;
            this.AudioRampIn.Location = new System.Drawing.Point(10, 369);
            this.AudioRampIn.Name = "AudioRampIn";
            this.AudioRampIn.Size = new System.Drawing.Size(503, 19);
            this.AudioRampIn.TabIndex = 60;
            this.AudioRampIn.Text = "Always ramp-in the start of a sample in a SoundFont (Disabling it could cause aud" +
    "io clicks)";
            this.AudioRampIn.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(7, 70);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(462, 13);
            this.label11.TabIndex = 0;
            this.label11.Text = "Change output\'s bit depth, maximum voices allowed and maximum rendering time";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(7, 18);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(81, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Audio engine";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(7, 181);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(233, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Change engine\'s buffer/latency settings";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(7, 263);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(178, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Change engine\'s DSP settings";
            // 
            // MaxCPU
            // 
            this.MaxCPU.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MaxCPU.Location = new System.Drawing.Point(700, 145);
            this.MaxCPU.Name = "MaxCPU";
            this.MaxCPU.Size = new System.Drawing.Size(75, 23);
            this.MaxCPU.TabIndex = 5;
            this.MaxCPU.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.MaxCPU.Value = new decimal(new int[] {
            75,
            0,
            0,
            0});
            // 
            // RenderingTimeLabel
            // 
            this.RenderingTimeLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.RenderingTimeLabel.Location = new System.Drawing.Point(7, 148);
            this.RenderingTimeLabel.Name = "RenderingTimeLabel";
            this.RenderingTimeLabel.Size = new System.Drawing.Size(685, 15);
            this.RenderingTimeLabel.TabIndex = 29;
            this.RenderingTimeLabel.Text = "Maximum rendering time (Percentage, set to 0% to disable it)";
            // 
            // VolSimView
            // 
            this.VolSimView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.VolSimView.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VolSimView.Location = new System.Drawing.Point(682, 355);
            this.VolSimView.Name = "VolSimView";
            this.VolSimView.Size = new System.Drawing.Size(94, 28);
            this.VolSimView.TabIndex = 36;
            this.VolSimView.Text = "100";
            this.VolSimView.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.VolSimView.UseCompatibleTextRendering = true;
            // 
            // VoiceLimitLabel
            // 
            this.VoiceLimitLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.VoiceLimitLabel.Location = new System.Drawing.Point(7, 118);
            this.VoiceLimitLabel.Name = "VoiceLimitLabel";
            this.VoiceLimitLabel.Size = new System.Drawing.Size(685, 15);
            this.VoiceLimitLabel.TabIndex = 26;
            this.VoiceLimitLabel.Text = "Driver voice limit (1 to 100,000 voices)";
            this.ButtonsDesc.SetToolTip(this.VoiceLimitLabel, "If there are currently more voices active than the new limit, then some voices wi" +
        "ll be killed to meet the limit.");
            // 
            // PolyphonyLimit
            // 
            this.PolyphonyLimit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PolyphonyLimit.Location = new System.Drawing.Point(700, 115);
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
            this.PolyphonyLimit.Size = new System.Drawing.Size(75, 23);
            this.PolyphonyLimit.TabIndex = 4;
            this.PolyphonyLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.PolyphonyLimit.ThousandsSeparator = true;
            this.ButtonsDesc.SetToolTip(this.PolyphonyLimit, "If there are currently more voices active than the new limit, then some voices wi" +
        "ll be killed to meet the limit.");
            this.PolyphonyLimit.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // VolLabel
            // 
            this.VolLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.VolLabel.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VolLabel.Location = new System.Drawing.Point(681, 335);
            this.VolLabel.Name = "VolLabel";
            this.VolLabel.Size = new System.Drawing.Size(99, 21);
            this.VolLabel.TabIndex = 35;
            this.VolLabel.Text = "VOLUME";
            this.VolLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DrvHzLabel
            // 
            this.DrvHzLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.DrvHzLabel.Location = new System.Drawing.Point(7, 200);
            this.DrvHzLabel.Name = "DrvHzLabel";
            this.DrvHzLabel.Size = new System.Drawing.Size(635, 15);
            this.DrvHzLabel.TabIndex = 22;
            this.DrvHzLabel.Text = "Output sample rate (in Hertz)";
            // 
            // Frequency
            // 
            this.Frequency.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
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
            this.Frequency.Location = new System.Drawing.Point(700, 196);
            this.Frequency.Name = "Frequency";
            this.Frequency.Size = new System.Drawing.Size(74, 23);
            this.Frequency.TabIndex = 6;
            this.Requirements.SetToolTip(this.Frequency, "This will require a restart of the audio stream.");
            // 
            // BufferText
            // 
            this.BufferText.AutoSize = true;
            this.BufferText.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BufferText.Location = new System.Drawing.Point(7, 230);
            this.BufferText.Name = "BufferText";
            this.BufferText.Size = new System.Drawing.Size(232, 15);
            this.BufferText.TabIndex = 25;
            this.BufferText.Text = "Driver buffer length (in ms, from 1 to 1000)";
            this.BufferText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bufsize
            // 
            this.bufsize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bufsize.Location = new System.Drawing.Point(700, 227);
            this.bufsize.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.bufsize.Name = "bufsize";
            this.bufsize.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.bufsize.Size = new System.Drawing.Size(75, 23);
            this.bufsize.TabIndex = 7;
            this.bufsize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Requirements.SetToolTip(this.bufsize, "This will require a restart of the audio stream.");
            this.bufsize.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // SincConv
            // 
            this.SincConv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SincConv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SincConv.Enabled = false;
            this.SincConv.FormattingEnabled = true;
            this.SincConv.Items.AddRange(new object[] {
            "Linear inter.",
            "8 point sinc",
            "16 point sinc",
            "32 point sinc",
            "64 point sinc"});
            this.SincConv.Location = new System.Drawing.Point(675, 279);
            this.SincConv.Name = "SincConv";
            this.SincConv.Size = new System.Drawing.Size(98, 23);
            this.SincConv.TabIndex = 9;
            this.ButtonsDesc.SetToolTip(this.SincConv, "Higher interpolation settings will require more CPU cycles.");
            // 
            // SincConvLab
            // 
            this.SincConvLab.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SincConvLab.AutoSize = true;
            this.SincConvLab.Enabled = false;
            this.SincConvLab.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.SincConvLab.Location = new System.Drawing.Point(563, 283);
            this.SincConvLab.Name = "SincConvLab";
            this.SincConvLab.Size = new System.Drawing.Size(109, 15);
            this.SincConvLab.TabIndex = 21;
            this.SincConvLab.Text = "Conversion quality:";
            // 
            // SincInter
            // 
            this.SincInter.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.SincInter.Location = new System.Drawing.Point(10, 282);
            this.SincInter.Name = "SincInter";
            this.SincInter.Size = new System.Drawing.Size(498, 20);
            this.SincInter.TabIndex = 8;
            this.SincInter.Text = "Enable sinc interpolation (Improves audio quality, but increases rendering time)";
            this.SincInter.UseVisualStyleBackColor = true;
            this.SincInter.CheckedChanged += new System.EventHandler(this.SincInter_CheckedChanged);
            // 
            // EnableSFX
            // 
            this.EnableSFX.AutoSize = true;
            this.EnableSFX.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.EnableSFX.Location = new System.Drawing.Point(10, 303);
            this.EnableSFX.Name = "EnableSFX";
            this.EnableSFX.Size = new System.Drawing.Size(383, 19);
            this.EnableSFX.TabIndex = 10;
            this.EnableSFX.Text = "Enable reverb and chorus (Disabling this can reduce rendering time)";
            this.EnableSFX.UseVisualStyleBackColor = true;
            // 
            // ChangeDefaultOutput
            // 
            this.ChangeDefaultOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ChangeDefaultOutput.Location = new System.Drawing.Point(482, 31);
            this.ChangeDefaultOutput.Name = "ChangeDefaultOutput";
            this.ChangeDefaultOutput.Size = new System.Drawing.Size(103, 25);
            this.ChangeDefaultOutput.TabIndex = 1;
            this.ChangeDefaultOutput.Text = "Change output";
            this.ChangeDefaultOutput.UseVisualStyleBackColor = true;
            this.ChangeDefaultOutput.Click += new System.EventHandler(this.ChangeDefaultOutput_Click);
            // 
            // MonophonicFunc
            // 
            this.MonophonicFunc.AutoSize = true;
            this.MonophonicFunc.Location = new System.Drawing.Point(10, 325);
            this.MonophonicFunc.Name = "MonophonicFunc";
            this.MonophonicFunc.Size = new System.Drawing.Size(459, 19);
            this.MonophonicFunc.TabIndex = 11;
            this.MonophonicFunc.Text = "Disable stereophonic rendering (Will downsample any stereo SoundFont to mono)";
            this.Requirements.SetToolTip(this.MonophonicFunc, "This will require a restart of the audio stream.");
            this.MonophonicFunc.UseVisualStyleBackColor = true;
            // 
            // FadeoutDisable
            // 
            this.FadeoutDisable.AutoSize = true;
            this.FadeoutDisable.Location = new System.Drawing.Point(10, 347);
            this.FadeoutDisable.Name = "FadeoutDisable";
            this.FadeoutDisable.Size = new System.Drawing.Size(580, 19);
            this.FadeoutDisable.TabIndex = 12;
            this.FadeoutDisable.Text = "Disable fade-out when killing an old note (Disabling it could cause clicks in the" +
    " audio when killing a note)";
            this.FadeoutDisable.UseVisualStyleBackColor = true;
            // 
            // AudioBitDepth
            // 
            this.AudioBitDepth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AudioBitDepth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AudioBitDepth.FormattingEnabled = true;
            this.AudioBitDepth.Items.AddRange(new object[] {
            "32-bit float",
            "16-bit integer",
            "8-bit integer"});
            this.AudioBitDepth.Location = new System.Drawing.Point(675, 85);
            this.AudioBitDepth.Name = "AudioBitDepth";
            this.AudioBitDepth.Size = new System.Drawing.Size(98, 23);
            this.AudioBitDepth.TabIndex = 3;
            this.Requirements.SetToolTip(this.AudioBitDepth, "This will require a restart of the audio stream.");
            // 
            // AudioBitDepthLabel
            // 
            this.AudioBitDepthLabel.AutoSize = true;
            this.AudioBitDepthLabel.Location = new System.Drawing.Point(7, 89);
            this.AudioBitDepthLabel.Name = "AudioBitDepthLabel";
            this.AudioBitDepthLabel.Size = new System.Drawing.Size(476, 15);
            this.AudioBitDepthLabel.TabIndex = 4;
            this.AudioBitDepthLabel.Text = "Audio bit depth (A higher audio bit depth indicates a more detailed sound reprodu" +
    "ction):";
            // 
            // AudioEngBox
            // 
            this.AudioEngBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AudioEngBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AudioEngBox.FormattingEnabled = true;
            this.AudioEngBox.Items.AddRange(new object[] {
            "Audio to .WAV mode",
            "BASS default output engine",
            "Audio Stream Input/Output",
            "Windows Audio Session API",
            "Microsoft XAudio 2.9"});
            this.AudioEngBox.Location = new System.Drawing.Point(589, 32);
            this.AudioEngBox.Name = "AudioEngBox";
            this.AudioEngBox.Size = new System.Drawing.Size(185, 23);
            this.AudioEngBox.TabIndex = 2;
            this.Requirements.SetToolTip(this.AudioEngBox, "This will require a restart of the audio stream.");
            this.AudioEngBox.SelectedIndexChanged += new System.EventHandler(this.AudioEngBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(435, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Engine (Using a different API might improve or worsen the performance, YMMV):";
            // 
            // HMode
            // 
            this.HMode.AutoSize = true;
            this.HMode.Location = new System.Drawing.Point(10, 60);
            this.HMode.Name = "HMode";
            this.HMode.Size = new System.Drawing.Size(490, 19);
            this.HMode.TabIndex = 18;
            this.HMode.Text = "Enable fast playback mode (Will cause some functions and checks to break in the d" +
    "river)";
            this.ButtonsDesc.SetToolTip(this.HMode, "Enabling the minimum playback mode will disable some internal checks,\r\nwhich coul" +
        "d cause some settings to break or crashes in the worst cases.");
            this.HMode.UseVisualStyleBackColor = true;
            this.HMode.CheckedChanged += new System.EventHandler(this.HMode_CheckedChanged);
            // 
            // KSDAPIBoxWhat
            // 
            this.KSDAPIBoxWhat.Cursor = System.Windows.Forms.Cursors.Hand;
            this.KSDAPIBoxWhat.Location = new System.Drawing.Point(301, 39);
            this.KSDAPIBoxWhat.Name = "KSDAPIBoxWhat";
            this.KSDAPIBoxWhat.Size = new System.Drawing.Size(16, 16);
            this.KSDAPIBoxWhat.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.KSDAPIBoxWhat.TabIndex = 15;
            this.KSDAPIBoxWhat.TabStop = false;
            this.KSDAPIBoxWhat.Click += new System.EventHandler(this.KSDAPIBoxWhat_Click);
            // 
            // KSDAPIBox
            // 
            this.KSDAPIBox.AutoSize = true;
            this.KSDAPIBox.Location = new System.Drawing.Point(10, 38);
            this.KSDAPIBox.Name = "KSDAPIBox";
            this.KSDAPIBox.Size = new System.Drawing.Size(289, 19);
            this.KSDAPIBox.TabIndex = 17;
            this.KSDAPIBox.Text = "Allow KDMAPI-aware apps to make use of the API";
            this.ButtonsDesc.SetToolTip(this.KSDAPIBox, "Unchecking this will force OmniMIDI to not report the KDMAPI status as present.\r\n" +
        "Some apps might be programmed to ignore for this check.");
            this.KSDAPIBox.UseVisualStyleBackColor = true;
            // 
            // SlowDownPlayback
            // 
            this.SlowDownPlayback.AutoSize = true;
            this.SlowDownPlayback.Location = new System.Drawing.Point(10, 104);
            this.SlowDownPlayback.Name = "SlowDownPlayback";
            this.SlowDownPlayback.Size = new System.Drawing.Size(621, 19);
            this.SlowDownPlayback.TabIndex = 20;
            this.SlowDownPlayback.Text = "Slow down the events processer instead of skipping events (Might cause lag or sta" +
    "lls on MIDIs with lots of events)";
            this.ButtonsDesc.SetToolTip(this.SlowDownPlayback, "The driver will try and play all the notes, instead of skipping them.");
            this.SlowDownPlayback.UseVisualStyleBackColor = true;
            // 
            // OldBuff
            // 
            this.OldBuff.AutoSize = true;
            this.OldBuff.Location = new System.Drawing.Point(10, 148);
            this.OldBuff.Name = "OldBuff";
            this.OldBuff.Size = new System.Drawing.Size(630, 19);
            this.OldBuff.TabIndex = 21;
            this.OldBuff.Text = "Run events processer and audio engine on the same thread (Could cause crackling, " +
    "but could also improve timing)";
            this.Requirements.SetToolTip(this.OldBuff, "This will require a restart of the audio stream.");
            this.OldBuff.UseVisualStyleBackColor = true;
            // 
            // SynthBox
            // 
            this.SynthBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SynthBox.Controls.Add(this.AsyncProcessing);
            this.SynthBox.Controls.Add(this.PitchShifting);
            this.SynthBox.Controls.Add(this.AutoLoad);
            this.SynthBox.Controls.Add(this.PrioLab);
            this.SynthBox.Controls.Add(this.IgnoreNotesLV);
            this.SynthBox.Controls.Add(this.PrioBox);
            this.SynthBox.Controls.Add(this.IgnoreNotesHV);
            this.SynthBox.Controls.Add(this.IgnoreNotesHL);
            this.SynthBox.Controls.Add(this.IgnoreNotesLL);
            this.SynthBox.Controls.Add(this.SysResetIgnore);
            this.SynthBox.Controls.Add(this.Preload);
            this.SynthBox.Controls.Add(this.label7);
            this.SynthBox.Controls.Add(this.label6);
            this.SynthBox.Controls.Add(this.NoteOffCheck);
            this.SynthBox.Controls.Add(this.CBRuler);
            this.SynthBox.Controls.Add(this.label3);
            this.SynthBox.Controls.Add(this.Limit88);
            this.SynthBox.Controls.Add(this.NoteOffDelayValue);
            this.SynthBox.Controls.Add(this.IgnoreNotes);
            this.SynthBox.Controls.Add(this.label1);
            this.SynthBox.Controls.Add(this.OverrideNoteLengthWA2);
            this.SynthBox.Controls.Add(this.DelayNoteOff);
            this.SynthBox.Controls.Add(this.NoteLengthValueMS);
            this.SynthBox.Controls.Add(this.NoteLengthValue);
            this.SynthBox.Controls.Add(this.label4);
            this.SynthBox.Controls.Add(this.OverrideNoteLengthWA1);
            this.SynthBox.Controls.Add(this.OverrideNoteLength);
            this.SynthBox.Controls.Add(this.HMode);
            this.SynthBox.Controls.Add(this.OldBuff);
            this.SynthBox.Controls.Add(this.SlowDownPlayback);
            this.SynthBox.Controls.Add(this.KSDAPIBox);
            this.SynthBox.Controls.Add(this.KSDAPIBoxWhat);
            this.SynthBox.Location = new System.Drawing.Point(3, 496);
            this.SynthBox.Name = "SynthBox";
            this.SynthBox.Size = new System.Drawing.Size(782, 450);
            this.SynthBox.TabIndex = 1;
            this.SynthBox.TabStop = false;
            this.SynthBox.Text = "Synthesizer settings";
            // 
            // AutoLoad
            // 
            this.AutoLoad.AutoSize = true;
            this.AutoLoad.Location = new System.Drawing.Point(10, 192);
            this.AutoLoad.Name = "AutoLoad";
            this.AutoLoad.Size = new System.Drawing.Size(665, 19);
            this.AutoLoad.TabIndex = 23;
            this.AutoLoad.Text = "Reload SoundFont list automatically after an edit (Does not apply to \"Shared list" +
    "\", it will always be reloaded automatically)";
            this.AutoLoad.UseVisualStyleBackColor = true;
            // 
            // PrioLab
            // 
            this.PrioLab.AutoSize = true;
            this.PrioLab.Location = new System.Drawing.Point(7, 222);
            this.PrioLab.Name = "PrioLab";
            this.PrioLab.Size = new System.Drawing.Size(514, 15);
            this.PrioLab.TabIndex = 59;
            this.PrioLab.Text = "Threads affinity (Reducing the affinity could help on weak systems, but might int" +
    "roduce latency)";
            // 
            // IgnoreNotesLV
            // 
            this.IgnoreNotesLV.Cursor = System.Windows.Forms.Cursors.Default;
            this.IgnoreNotesLV.Location = new System.Drawing.Point(100, 363);
            this.IgnoreNotesLV.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.IgnoreNotesLV.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.IgnoreNotesLV.Name = "IgnoreNotesLV";
            this.IgnoreNotesLV.Size = new System.Drawing.Size(44, 23);
            this.IgnoreNotesLV.TabIndex = 33;
            this.IgnoreNotesLV.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // PrioBox
            // 
            this.PrioBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PrioBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PrioBox.FormattingEnabled = true;
            this.PrioBox.Items.AddRange(new object[] {
            "Default",
            "Real-time",
            "High",
            "Higher than normal",
            "Normal",
            "Lower than normal",
            "Low"});
            this.PrioBox.Location = new System.Drawing.Point(623, 219);
            this.PrioBox.Name = "PrioBox";
            this.PrioBox.Size = new System.Drawing.Size(151, 23);
            this.PrioBox.TabIndex = 24;
            // 
            // IgnoreNotesHV
            // 
            this.IgnoreNotesHV.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.IgnoreNotesHV.Location = new System.Drawing.Point(267, 363);
            this.IgnoreNotesHV.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.IgnoreNotesHV.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.IgnoreNotesHV.Name = "IgnoreNotesHV";
            this.IgnoreNotesHV.Size = new System.Drawing.Size(44, 23);
            this.IgnoreNotesHV.TabIndex = 34;
            this.IgnoreNotesHV.Value = new decimal(new int[] {
            127,
            0,
            0,
            0});
            // 
            // IgnoreNotesHL
            // 
            this.IgnoreNotesHL.AutoSize = true;
            this.IgnoreNotesHL.Location = new System.Drawing.Point(171, 365);
            this.IgnoreNotesHL.Name = "IgnoreNotesHL";
            this.IgnoreNotesHL.Size = new System.Drawing.Size(91, 15);
            this.IgnoreNotesHL.TabIndex = 58;
            this.IgnoreNotesHL.Text = "Highest (0-127):";
            // 
            // IgnoreNotesLL
            // 
            this.IgnoreNotesLL.AutoSize = true;
            this.IgnoreNotesLL.Location = new System.Drawing.Point(7, 365);
            this.IgnoreNotesLL.Name = "IgnoreNotesLL";
            this.IgnoreNotesLL.Size = new System.Drawing.Size(87, 15);
            this.IgnoreNotesLL.TabIndex = 57;
            this.IgnoreNotesLL.Text = "Lowest (0-127):";
            // 
            // SysResetIgnore
            // 
            this.SysResetIgnore.AutoSize = true;
            this.SysResetIgnore.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.SysResetIgnore.Location = new System.Drawing.Point(10, 318);
            this.SysResetIgnore.Name = "SysResetIgnore";
            this.SysResetIgnore.Size = new System.Drawing.Size(629, 19);
            this.SysResetIgnore.TabIndex = 31;
            this.SysResetIgnore.Text = "Ignore system reset events when the system mode is unchanged (Might cause issues " +
    "with program change events)";
            this.SysResetIgnore.UseVisualStyleBackColor = true;
            // 
            // Preload
            // 
            this.Preload.AutoSize = true;
            this.Preload.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Preload.Location = new System.Drawing.Point(10, 170);
            this.Preload.Name = "Preload";
            this.Preload.Size = new System.Drawing.Size(609, 19);
            this.Preload.TabIndex = 22;
            this.Preload.Text = "Preload SoundFonts to RAM (Will increase the startup time of the driver, dependin" +
    "g on speed of the computer)";
            this.ButtonsDesc.SetToolTip(this.Preload, "You can also choose not to preload specific SoundFonts, to reduce the memory usag" +
        "e.");
            this.Preload.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(7, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(229, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Change the behavior of the synthesizer";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(7, 256);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(242, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Change how the events should be parsed";
            // 
            // NoteOffCheck
            // 
            this.NoteOffCheck.AutoSize = true;
            this.NoteOffCheck.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.NoteOffCheck.Location = new System.Drawing.Point(10, 82);
            this.NoteOffCheck.Name = "NoteOffCheck";
            this.NoteOffCheck.Size = new System.Drawing.Size(522, 19);
            this.NoteOffCheck.TabIndex = 19;
            this.NoteOffCheck.Text = "Only release the oldest instance of a note upon note-off event (Could increase re" +
    "ndering time)";
            this.ButtonsDesc.SetToolTip(this.NoteOffCheck, "Only release the oldest instance upon a note off event (Notes with velocity value" +
        " of 0) when there are overlapping instances of the note.\r\nOtherwise all instance" +
        "s are released.");
            this.NoteOffCheck.UseVisualStyleBackColor = true;
            // 
            // CBRuler
            // 
            this.CBRuler.Location = new System.Drawing.Point(743, 371);
            this.CBRuler.Name = "CBRuler";
            this.CBRuler.Size = new System.Drawing.Size(23, 23);
            this.CBRuler.TabIndex = 41;
            this.CBRuler.Visible = false;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Location = new System.Drawing.Point(767, 421);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(10, 15);
            this.label3.TabIndex = 33;
            this.label3.Text = "s";
            // 
            // Limit88
            // 
            this.Limit88.AutoSize = true;
            this.Limit88.Location = new System.Drawing.Point(10, 296);
            this.Limit88.Name = "Limit88";
            this.Limit88.Size = new System.Drawing.Size(307, 19);
            this.Limit88.TabIndex = 30;
            this.Limit88.Text = "Limit key range to 88 keys (Excluding drums channel)";
            this.Limit88.UseVisualStyleBackColor = true;
            // 
            // NoteOffDelayValue
            // 
            this.NoteOffDelayValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NoteOffDelayValue.DecimalPlaces = 3;
            this.NoteOffDelayValue.Location = new System.Drawing.Point(691, 418);
            this.NoteOffDelayValue.Name = "NoteOffDelayValue";
            this.NoteOffDelayValue.Size = new System.Drawing.Size(76, 23);
            this.NoteOffDelayValue.TabIndex = 38;
            this.NoteOffDelayValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // IgnoreNotes
            // 
            this.IgnoreNotes.AutoSize = true;
            this.IgnoreNotes.Location = new System.Drawing.Point(10, 340);
            this.IgnoreNotes.Name = "IgnoreNotes";
            this.IgnoreNotes.Size = new System.Drawing.Size(575, 19);
            this.IgnoreNotes.TabIndex = 32;
            this.IgnoreNotes.Text = "Ignore notes in between two velocity values (Could help with some SoundFonts with" +
    " bad velocity layers)";
            this.IgnoreNotes.UseVisualStyleBackColor = true;
            this.IgnoreNotes.CheckedChanged += new System.EventHandler(this.IgnoreNotes_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(642, 421);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 15);
            this.label1.TabIndex = 32;
            this.label1.Text = "Length:";
            // 
            // OverrideNoteLengthWA2
            // 
            this.OverrideNoteLengthWA2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.OverrideNoteLengthWA2.Location = new System.Drawing.Point(184, 421);
            this.OverrideNoteLengthWA2.Name = "OverrideNoteLengthWA2";
            this.OverrideNoteLengthWA2.Size = new System.Drawing.Size(16, 16);
            this.OverrideNoteLengthWA2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.OverrideNoteLengthWA2.TabIndex = 31;
            this.OverrideNoteLengthWA2.TabStop = false;
            this.OverrideNoteLengthWA2.Click += new System.EventHandler(this.OverrideNoteLengthWA1_Click);
            // 
            // DelayNoteOff
            // 
            this.DelayNoteOff.AutoSize = true;
            this.DelayNoteOff.Location = new System.Drawing.Point(10, 420);
            this.DelayNoteOff.Name = "DelayNoteOff";
            this.DelayNoteOff.Size = new System.Drawing.Size(172, 19);
            this.DelayNoteOff.TabIndex = 37;
            this.DelayNoteOff.Text = "Add delay to noteoff events";
            this.DelayNoteOff.UseVisualStyleBackColor = true;
            this.DelayNoteOff.CheckedChanged += new System.EventHandler(this.DelayNoteOff_CheckedChanged);
            // 
            // NoteLengthValueMS
            // 
            this.NoteLengthValueMS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NoteLengthValueMS.Location = new System.Drawing.Point(767, 399);
            this.NoteLengthValueMS.Name = "NoteLengthValueMS";
            this.NoteLengthValueMS.Size = new System.Drawing.Size(10, 15);
            this.NoteLengthValueMS.TabIndex = 30;
            this.NoteLengthValueMS.Text = "s";
            // 
            // NoteLengthValue
            // 
            this.NoteLengthValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.NoteLengthValue.DecimalPlaces = 3;
            this.NoteLengthValue.Location = new System.Drawing.Point(691, 397);
            this.NoteLengthValue.Name = "NoteLengthValue";
            this.NoteLengthValue.Size = new System.Drawing.Size(76, 23);
            this.NoteLengthValue.TabIndex = 36;
            this.NoteLengthValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(642, 399);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 15);
            this.label4.TabIndex = 29;
            this.label4.Text = "Length:";
            // 
            // OverrideNoteLengthWA1
            // 
            this.OverrideNoteLengthWA1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.OverrideNoteLengthWA1.Location = new System.Drawing.Point(198, 399);
            this.OverrideNoteLengthWA1.Name = "OverrideNoteLengthWA1";
            this.OverrideNoteLengthWA1.Size = new System.Drawing.Size(16, 16);
            this.OverrideNoteLengthWA1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.OverrideNoteLengthWA1.TabIndex = 28;
            this.OverrideNoteLengthWA1.TabStop = false;
            this.OverrideNoteLengthWA1.Click += new System.EventHandler(this.OverrideNoteLengthWA1_Click);
            // 
            // OverrideNoteLength
            // 
            this.OverrideNoteLength.AutoSize = true;
            this.OverrideNoteLength.Location = new System.Drawing.Point(10, 398);
            this.OverrideNoteLength.Name = "OverrideNoteLength";
            this.OverrideNoteLength.Size = new System.Drawing.Size(186, 19);
            this.OverrideNoteLength.TabIndex = 35;
            this.OverrideNoteLength.Text = "Override length of note events";
            this.OverrideNoteLength.UseVisualStyleBackColor = true;
            this.OverrideNoteLength.CheckedChanged += new System.EventHandler(this.OverrideNoteLength_CheckedChanged);
            // 
            // IgnoreAllEvents
            // 
            this.IgnoreAllEvents.AutoSize = true;
            this.IgnoreAllEvents.Location = new System.Drawing.Point(10, 204);
            this.IgnoreAllEvents.Name = "IgnoreAllEvents";
            this.IgnoreAllEvents.Size = new System.Drawing.Size(344, 19);
            this.IgnoreAllEvents.TabIndex = 27;
            this.IgnoreAllEvents.Text = "Ignore all MIDI events (For MIDI application developers only)";
            this.IgnoreAllEvents.UseVisualStyleBackColor = true;
            // 
            // FullVelocityMode
            // 
            this.FullVelocityMode.AutoSize = true;
            this.FullVelocityMode.Location = new System.Drawing.Point(10, 226);
            this.FullVelocityMode.Name = "FullVelocityMode";
            this.FullVelocityMode.Size = new System.Drawing.Size(443, 19);
            this.FullVelocityMode.TabIndex = 28;
            this.FullVelocityMode.Text = "Set all the NoteON events to full velocity (For MIDI application developers only)" +
    "";
            this.FullVelocityMode.UseVisualStyleBackColor = true;
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
            // DebugMode
            // 
            this.DebugMode.AutoSize = true;
            this.DebugMode.Location = new System.Drawing.Point(10, 73);
            this.DebugMode.Name = "DebugMode";
            this.DebugMode.Size = new System.Drawing.Size(499, 19);
            this.DebugMode.TabIndex = 39;
            this.DebugMode.Text = "Enable troubleshooting mode (Will slow down the MIDI application, use it for debu" +
    "g only)";
            this.Requirements.SetToolTip(this.DebugMode, "This will require a restart of the audio stream.");
            this.DebugMode.UseVisualStyleBackColor = true;
            this.DebugMode.CheckedChanged += new System.EventHandler(this.DebugMode_CheckedChanged);
            // 
            // IgnoreCloseCalls
            // 
            this.IgnoreCloseCalls.AutoSize = true;
            this.IgnoreCloseCalls.Location = new System.Drawing.Point(10, 160);
            this.IgnoreCloseCalls.Name = "IgnoreCloseCalls";
            this.IgnoreCloseCalls.Size = new System.Drawing.Size(707, 19);
            this.IgnoreCloseCalls.TabIndex = 45;
            this.IgnoreCloseCalls.Text = "Ignore midiOutClose/midiStreamClose calls (WinMMWRP BM patched apps only, useful " +
    "to keep OmniMIDI ready for new events)";
            this.Requirements.SetToolTip(this.IgnoreCloseCalls, "This will require a restart of the audio stream.");
            this.IgnoreCloseCalls.UseVisualStyleBackColor = true;
            // 
            // UseTGT
            // 
            this.UseTGT.AutoSize = true;
            this.UseTGT.Location = new System.Drawing.Point(10, 138);
            this.UseTGT.Name = "UseTGT";
            this.UseTGT.Size = new System.Drawing.Size(673, 19);
            this.UseTGT.TabIndex = 44;
            this.UseTGT.Text = "Use WinMM\'s time function instead of NtDelayExecution (WinMMWRP BM/DAW patched ap" +
    "ps only, could improve audio)";
            this.Requirements.SetToolTip(this.UseTGT, "This will require a restart of the audio stream.");
            this.UseTGT.UseVisualStyleBackColor = true;
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
            // LiveChangesTrigger
            // 
            this.LiveChangesTrigger.AutoSize = true;
            this.LiveChangesTrigger.Location = new System.Drawing.Point(10, 95);
            this.LiveChangesTrigger.Name = "LiveChangesTrigger";
            this.LiveChangesTrigger.Size = new System.Drawing.Size(518, 19);
            this.LiveChangesTrigger.TabIndex = 42;
            this.LiveChangesTrigger.Text = "Enable live changes for all the settings (Could lead to application crashes if yo" +
    "u\'re not careful)";
            this.LiveChangesTrigger.UseVisualStyleBackColor = true;
            this.LiveChangesTrigger.CheckedChanged += new System.EventHandler(this.LiveChangesTrigger_CheckedChanged);
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(7, 21);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(766, 48);
            this.label15.TabIndex = 0;
            this.label15.Text = resources.GetString("label15.Text");
            // 
            // FastHotKeys
            // 
            this.FastHotKeys.AutoSize = true;
            this.FastHotKeys.Location = new System.Drawing.Point(10, 117);
            this.FastHotKeys.Name = "FastHotKeys";
            this.FastHotKeys.Size = new System.Drawing.Size(545, 19);
            this.FastHotKeys.TabIndex = 43;
            this.FastHotKeys.Text = "Enable fast hotkey combinations (Allows you to quickly switch between SoundFont l" +
    "ists and more)";
            this.FastHotKeys.UseVisualStyleBackColor = true;
            // 
            // LegacySetDia
            // 
            this.LegacySetDia.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LegacySetDia.Controls.Add(this.Troubleshooter);
            this.LegacySetDia.Controls.Add(this.MinidumpsFolder);
            this.LegacySetDia.Controls.Add(this.MIDIFeedbackTool);
            this.LegacySetDia.Controls.Add(this.IgnoreCloseCalls);
            this.LegacySetDia.Controls.Add(this.WinMMSpeedDiag);
            this.LegacySetDia.Controls.Add(this.UseTGT);
            this.LegacySetDia.Controls.Add(this.ShowChangelogUpdate);
            this.LegacySetDia.Controls.Add(this.DebugModeFolder);
            this.LegacySetDia.Controls.Add(this.DebugMode);
            this.LegacySetDia.Controls.Add(this.SpatialSound);
            this.LegacySetDia.Controls.Add(this.FastHotKeys);
            this.LegacySetDia.Controls.Add(this.label15);
            this.LegacySetDia.Controls.Add(this.LiveChangesTrigger);
            this.LegacySetDia.Controls.Add(this.ChangeEVBuf);
            this.LegacySetDia.Controls.Add(this.IgnoreAllEvents);
            this.LegacySetDia.Controls.Add(this.FullVelocityMode);
            this.LegacySetDia.Location = new System.Drawing.Point(3, 952);
            this.LegacySetDia.Name = "LegacySetDia";
            this.LegacySetDia.Size = new System.Drawing.Size(782, 339);
            this.LegacySetDia.TabIndex = 2;
            this.LegacySetDia.TabStop = false;
            this.LegacySetDia.Text = "Debug and legacy settings";
            // 
            // Troubleshooter
            // 
            this.Troubleshooter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Troubleshooter.Location = new System.Drawing.Point(601, 306);
            this.Troubleshooter.Name = "Troubleshooter";
            this.Troubleshooter.Size = new System.Drawing.Size(175, 27);
            this.Troubleshooter.TabIndex = 53;
            this.Troubleshooter.Text = "Run driver troubleshooter";
            this.Troubleshooter.UseVisualStyleBackColor = true;
            this.Troubleshooter.Click += new System.EventHandler(this.Troubleshooter_Click);
            // 
            // ShowChangelogUpdate
            // 
            this.ShowChangelogUpdate.AutoSize = true;
            this.ShowChangelogUpdate.Location = new System.Drawing.Point(10, 182);
            this.ShowChangelogUpdate.Name = "ShowChangelogUpdate";
            this.ShowChangelogUpdate.Size = new System.Drawing.Size(350, 19);
            this.ShowChangelogUpdate.TabIndex = 46;
            this.ShowChangelogUpdate.Text = "Always show changelog on start-up, after applying an update";
            this.ShowChangelogUpdate.UseVisualStyleBackColor = true;
            // 
            // AsyncProcessing
            // 
            this.AsyncProcessing.AutoSize = true;
            this.AsyncProcessing.Location = new System.Drawing.Point(10, 126);
            this.AsyncProcessing.Name = "AsyncProcessing";
            this.AsyncProcessing.Size = new System.Drawing.Size(587, 19);
            this.AsyncProcessing.TabIndex = 60;
            this.AsyncProcessing.Text = "Process MIDI events asynchronously from audio output buffer (Could increase laten" +
    "cy and rendering time)";
            this.ButtonsDesc.SetToolTip(this.AsyncProcessing, resources.GetString("AsyncProcessing.ToolTip"));
            this.AsyncProcessing.UseVisualStyleBackColor = true;
            // 
            // MinidumpsFolder
            // 
            this.MinidumpsFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MinidumpsFolder.AutoSize = true;
            this.MinidumpsFolder.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(0)))), ((int)(((byte)(119)))));
            this.MinidumpsFolder.Location = new System.Drawing.Point(566, 74);
            this.MinidumpsFolder.Name = "MinidumpsFolder";
            this.MinidumpsFolder.Size = new System.Drawing.Size(102, 15);
            this.MinidumpsFolder.TabIndex = 52;
            this.MinidumpsFolder.TabStop = true;
            this.MinidumpsFolder.Text = "Minidumps folder";
            this.MinidumpsFolder.Visible = false;
            this.MinidumpsFolder.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.MinidumpsFolder_LinkClicked);
            // 
            // MIDIFeedbackTool
            // 
            this.MIDIFeedbackTool.AutoSize = true;
            this.MIDIFeedbackTool.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(0)))), ((int)(((byte)(119)))));
            this.MIDIFeedbackTool.Location = new System.Drawing.Point(7, 272);
            this.MIDIFeedbackTool.Name = "MIDIFeedbackTool";
            this.MIDIFeedbackTool.Size = new System.Drawing.Size(307, 15);
            this.MIDIFeedbackTool.TabIndex = 51;
            this.MIDIFeedbackTool.TabStop = true;
            this.MIDIFeedbackTool.Text = "Enable/disable MIDI feedback to another MIDI out device";
            this.MIDIFeedbackTool.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.MIDIFeedbackTool_LinkClicked);
            // 
            // WinMMSpeedDiag
            // 
            this.WinMMSpeedDiag.AutoSize = true;
            this.WinMMSpeedDiag.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(0)))), ((int)(((byte)(119)))));
            this.WinMMSpeedDiag.Location = new System.Drawing.Point(7, 293);
            this.WinMMSpeedDiag.Name = "WinMMSpeedDiag";
            this.WinMMSpeedDiag.Size = new System.Drawing.Size(280, 15);
            this.WinMMSpeedDiag.TabIndex = 48;
            this.WinMMSpeedDiag.TabStop = true;
            this.WinMMSpeedDiag.Text = "Change speed of the Windows Multimedia Wrapper";
            this.WinMMSpeedDiag.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.WinMMSpeedDiag_LinkClicked);
            // 
            // DebugModeFolder
            // 
            this.DebugModeFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DebugModeFolder.AutoSize = true;
            this.DebugModeFolder.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(0)))), ((int)(((byte)(119)))));
            this.DebugModeFolder.Location = new System.Drawing.Point(668, 74);
            this.DebugModeFolder.Name = "DebugModeFolder";
            this.DebugModeFolder.Size = new System.Drawing.Size(101, 15);
            this.DebugModeFolder.TabIndex = 40;
            this.DebugModeFolder.TabStop = true;
            this.DebugModeFolder.Text = "Debug logs folder";
            this.DebugModeFolder.Visible = false;
            this.DebugModeFolder.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.DebugModeFolder_LinkClicked);
            // 
            // SpatialSound
            // 
            this.SpatialSound.AutoSize = true;
            this.SpatialSound.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(0)))), ((int)(((byte)(119)))));
            this.SpatialSound.Location = new System.Drawing.Point(7, 314);
            this.SpatialSound.Name = "SpatialSound";
            this.SpatialSound.Size = new System.Drawing.Size(165, 15);
            this.SpatialSound.TabIndex = 50;
            this.SpatialSound.TabStop = true;
            this.SpatialSound.Text = "Change spatial sound settings";
            this.SpatialSound.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.SpatialSound_LinkClicked);
            // 
            // ChangeEVBuf
            // 
            this.ChangeEVBuf.AutoSize = true;
            this.ChangeEVBuf.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(0)))), ((int)(((byte)(119)))));
            this.ChangeEVBuf.Location = new System.Drawing.Point(7, 252);
            this.ChangeEVBuf.Name = "ChangeEVBuf";
            this.ChangeEVBuf.Size = new System.Drawing.Size(232, 15);
            this.ChangeEVBuf.TabIndex = 47;
            this.ChangeEVBuf.TabStop = true;
            this.ChangeEVBuf.Text = "Change size of the events buffer (EVBuffer)";
            this.ChangeEVBuf.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ChangeEVBuf_LinkClicked);
            // 
            // PitchShifting
            // 
            this.PitchShifting.AutoSize = true;
            this.PitchShifting.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(0)))), ((int)(((byte)(119)))));
            this.PitchShifting.Location = new System.Drawing.Point(7, 275);
            this.PitchShifting.Name = "PitchShifting";
            this.PitchShifting.Size = new System.Drawing.Size(381, 15);
            this.PitchShifting.TabIndex = 25;
            this.PitchShifting.TabStop = true;
            this.PitchShifting.Text = ">>> Change transposing and concert pitch settings (Separate window)";
            this.PitchShifting.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.PitchShifting_LinkClicked);
            // 
            // DSPSettingsBox
            // 
            this.DSPSettingsBox.AutoSize = true;
            this.DSPSettingsBox.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(0)))), ((int)(((byte)(119)))));
            this.DSPSettingsBox.Location = new System.Drawing.Point(7, 459);
            this.DSPSettingsBox.Name = "DSPSettingsBox";
            this.DSPSettingsBox.Size = new System.Drawing.Size(350, 15);
            this.DSPSettingsBox.TabIndex = 60;
            this.DSPSettingsBox.TabStop = true;
            this.DSPSettingsBox.Text = ">>> Change reverb, chorus and echo settings (Separate window)";
            this.DSPSettingsBox.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.DSPSettingsBox_LinkClicked);
            // 
            // VolKnob
            // 
            this.VolKnob.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.VolKnob.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.VolKnob.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.VolKnob.EndAngle = 405F;
            this.VolKnob.ImeMode = System.Windows.Forms.ImeMode.On;
            this.VolKnob.knobBackColor = System.Drawing.Color.White;
            this.VolKnob.KnobPointerStyle = KnobControl.KnobControl.knobPointerStyle.line;
            this.VolKnob.LargeChange = 1000;
            this.VolKnob.Location = new System.Drawing.Point(682, 385);
            this.VolKnob.Maximum = 10000;
            this.VolKnob.Minimum = 0;
            this.VolKnob.Name = "VolKnob";
            this.VolKnob.PointerColor = System.Drawing.Color.White;
            this.VolKnob.ScaleColor = System.Drawing.Color.Black;
            this.VolKnob.ScaleDivisions = 10;
            this.VolKnob.ScaleSubDivisions = 10;
            this.VolKnob.ShowLargeScale = false;
            this.VolKnob.ShowSmallScale = false;
            this.VolKnob.Size = new System.Drawing.Size(93, 93);
            this.VolKnob.SmallChange = 500;
            this.VolKnob.StartAngle = 135F;
            this.VolKnob.TabIndex = 16;
            this.VolKnob.Value = 10000;
            this.VolKnob.ValueChanged += new KnobControl.ValueChangedEventHandler(this.VolTrackBar_Scroll);
            // 
            // SettingsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LegacySetDia);
            this.Controls.Add(this.SynthBox);
            this.Controls.Add(this.EnginesBox);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SettingsPanel";
            this.Size = new System.Drawing.Size(791, 1294);
            this.Load += new System.EventHandler(this.SettingsPanel_Load);
            this.EnginesBox.ResumeLayout(false);
            this.EnginesBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MaxCPU)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PolyphonyLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bufsize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.KSDAPIBoxWhat)).EndInit();
            this.SynthBox.ResumeLayout(false);
            this.SynthBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IgnoreNotesLV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IgnoreNotesHV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NoteOffDelayValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OverrideNoteLengthWA2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NoteLengthValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OverrideNoteLengthWA1)).EndInit();
            this.LegacySetDia.ResumeLayout(false);
            this.LegacySetDia.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.ComboBox AudioEngBox;
        private System.Windows.Forms.PictureBox KSDAPIBoxWhat;
        private System.Windows.Forms.Label AudioBitDepthLabel;
        private System.Windows.Forms.Button ChangeDefaultOutput;
        public System.Windows.Forms.ComboBox SincConv;
        internal System.Windows.Forms.Label SincConvLab;
        public System.Windows.Forms.CheckBox SincInter;
        public System.Windows.Forms.CheckBox EnableSFX;
        public System.Windows.Forms.Label DrvHzLabel;
        public System.Windows.Forms.ComboBox Frequency;
        public System.Windows.Forms.Label BufferText;
        public System.Windows.Forms.NumericUpDown bufsize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox OverrideNoteLengthWA2;
        private System.Windows.Forms.Label NoteLengthValueMS;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox OverrideNoteLengthWA1;
        public KnobControl.KnobControl VolKnob;
        private System.Windows.Forms.Label VolSimView;
        private System.Windows.Forms.Label VolLabel;
        private System.Windows.Forms.ToolTip ButtonsDesc;
        public System.Windows.Forms.ToolTip Requirements;
        public System.Windows.Forms.GroupBox EnginesBox;
        public System.Windows.Forms.GroupBox SynthBox;
        public System.Windows.Forms.CheckBox HMode;
        public System.Windows.Forms.CheckBox KSDAPIBox;
        public System.Windows.Forms.CheckBox SlowDownPlayback;
        public System.Windows.Forms.CheckBox OldBuff;
        public System.Windows.Forms.CheckBox MonophonicFunc;
        public System.Windows.Forms.CheckBox FadeoutDisable;
        public System.Windows.Forms.ComboBox AudioBitDepth;
        public System.Windows.Forms.NumericUpDown NoteOffDelayValue;
        public System.Windows.Forms.CheckBox DelayNoteOff;
        public System.Windows.Forms.NumericUpDown NoteLengthValue;
        public System.Windows.Forms.CheckBox OverrideNoteLength;
        public System.Windows.Forms.CheckBox IgnoreNotes;
        public System.Windows.Forms.CheckBox FullVelocityMode;
        public System.Windows.Forms.CheckBox IgnoreAllEvents;
        public System.Windows.Forms.CheckBox Limit88;
        public System.Windows.Forms.NumericUpDown MaxCPU;
        internal System.Windows.Forms.Label RenderingTimeLabel;
        internal System.Windows.Forms.Label VoiceLimitLabel;
        public System.Windows.Forms.NumericUpDown PolyphonyLimit;
        public System.Windows.Forms.CheckBox NoteOffCheck;
        private System.Windows.Forms.Panel CBRuler;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        public System.Windows.Forms.CheckBox AutoLoad;
        private System.Windows.Forms.ContextMenu VolTrackBarMenu;
        private System.Windows.Forms.MenuItem FineTuneKnobIt;
        private System.Windows.Forms.MenuItem menuItem57;
        public System.Windows.Forms.MenuItem VolumeBoost;
        public System.Windows.Forms.CheckBox Preload;
        public System.Windows.Forms.CheckBox SysResetIgnore;
        private System.Windows.Forms.NumericUpDown IgnoreNotesLV;
        private System.Windows.Forms.NumericUpDown IgnoreNotesHV;
        private System.Windows.Forms.Label IgnoreNotesHL;
        private System.Windows.Forms.Label IgnoreNotesLL;
        private System.Windows.Forms.Label PrioLab;
        private System.Windows.Forms.ComboBox PrioBox;
        private LinkLabelEx ChangeEVBuf;
        public System.Windows.Forms.CheckBox LiveChangesTrigger;
        private System.Windows.Forms.Label label15;
        public System.Windows.Forms.CheckBox FastHotKeys;
        private LinkLabelEx SpatialSound;
        public System.Windows.Forms.CheckBox DebugMode;
        private LinkLabelEx DebugModeFolder;
        public System.Windows.Forms.GroupBox LegacySetDia;
        public System.Windows.Forms.CheckBox ShowChangelogUpdate;
        public System.Windows.Forms.CheckBox UseTGT;
        private LinkLabelEx WinMMSpeedDiag;
        public System.Windows.Forms.CheckBox IgnoreCloseCalls;
        private LinkLabelEx PitchShifting;
        public System.Windows.Forms.CheckBox AudioRampIn;
        public System.Windows.Forms.CheckBox LinDecVol;
        public System.Windows.Forms.CheckBox LinAttMod;
        public System.Windows.Forms.CheckBox NoSFGenLimits;
        private LinkLabelEx MIDIFeedbackTool;
        private LinkLabelEx MinidumpsFolder;
        private System.Windows.Forms.Button Troubleshooter;
        private LinkLabelEx DSPSettingsBox;
        public System.Windows.Forms.CheckBox AsyncProcessing;
    }
}
