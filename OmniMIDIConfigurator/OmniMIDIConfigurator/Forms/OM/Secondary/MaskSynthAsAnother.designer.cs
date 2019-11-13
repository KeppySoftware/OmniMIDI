namespace OmniMIDIConfigurator
{
    partial class MaskSynthAsAnother
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Names = new System.Windows.Forms.ComboBox();
            this.OK = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.DefName = new System.Windows.Forms.Button();
            this.SynthType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.VIDPIDList = new OmniMIDIConfigurator.LinkLabelEx();
            this.PIDValue = new OmniMIDIConfigurator.HexNumericUpDown();
            this.VIDValue = new OmniMIDIConfigurator.HexNumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.PIDValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIDValue)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(405, 72);
            this.label1.TabIndex = 1;
            this.label1.Text = "Some apps might be hardwired to a specific synthesizer.\r\nYou can try and fool the" +
    "m by renaming OmniMIDI to another MIDI output device.\r\n\r\nSelect a mask in the li" +
    "st below:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(47, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Mask name:";
            // 
            // Names
            // 
            this.Names.FormattingEnabled = true;
            this.Names.Items.AddRange(new object[] {
            "A-PRO",
            "A-PRO MIDI OUT",
            "AWE64 MIDI Synth",
            "Akai APC20",
            "Akai MAX49",
            "AudioPCI MIDI Out",
            "AudioPCI MIDI Synth",
            "Automap MIDI",
            "Automap Propellerhead",
            "Automap Propellerhead Mixer",
            "BASSMIDI Driver",
            "BASSMIDI Driver (Port A)",
            "BASSMIDI Driver (Port B)",
            "CEUS live",
            "CoolSoft VirtualMIDISynth",
            "Creative OPL3 FM",
            "Creative SB Live! External MIDI",
            "Daemon Output 0",
            "E-MU Xboard49",
            "FastLane USB Port A-PRO",
            "FastLane: Port A",
            "Impact LX49+ MIDI1",
            "Keppy\'s Driver",
            "Keppy\'s MIDI Driver",
            "Keppy\'s Synthesizer",
            "Launchpad S",
            "Liquid56 MIDI",
            "M-Audio Keystation Pro 88",
            "MIDISPORT 4x4 Out A",
            "MPC Port 1",
            "MPKmini2",
            "Microsoft GS Wavetable Synth",
            "Microsoft Synthesizer",
            "MiniNova",
            "NVIDIA® Wavetable Synthesizer",
            "Network MIDI Master",
            "OmniMIDI",
            "Out-A USB MidiSport 2x2",
            "PSR-290",
            "PSR-3000-1",
            "PSR-3000-2",
            "ReMOTE25 V2.0",
            "Reason Midi Out",
            "Roland MPU-401",
            "SB AWE32 MIDI Synth",
            "SB Live! MIDI Out",
            "SB Live! MIDI Synth",
            "SB Live! Synth A",
            "SB Live! Synth B",
            "Scarlet 6i6 USB",
            "SoundMAX Wavetable Synth",
            "Steinberg UR44-1",
            "TC Near",
            "Timidity++ Driver",
            "USB Audio Device",
            "USB Axiom 25",
            "VMeter 1.28 A",
            "VirtualMIDISynth #1",
            "VirtualMIDISynth #2",
            "VirtualMIDISynth #3",
            "VirtualMIDISynth #4",
            "Windows OPL3 Synth",
            "YAMAHA MOTIF XF7 Port1",
            "YAMAHA USB OUT 0-1",
            "YMF262 Synth Emulator",
            "Yamaha S-YXG50 SoftSynthesizer",
            "loopMIDI Port",
            "mLAN Network MOTIF XS:1"});
            this.Names.Location = new System.Drawing.Point(112, 72);
            this.Names.Name = "Names";
            this.Names.Size = new System.Drawing.Size(241, 21);
            this.Names.TabIndex = 3;
            this.Names.Text = " ";
            // 
            // OK
            // 
            this.OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OK.Location = new System.Drawing.Point(340, 156);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(53, 23);
            this.OK.TabIndex = 4;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelBtn.Location = new System.Drawing.Point(281, 156);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(53, 23);
            this.CancelBtn.TabIndex = 5;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // DefName
            // 
            this.DefName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DefName.Location = new System.Drawing.Point(222, 156);
            this.DefName.Name = "DefName";
            this.DefName.Size = new System.Drawing.Size(53, 23);
            this.DefName.TabIndex = 7;
            this.DefName.Text = "Reset";
            this.DefName.UseVisualStyleBackColor = true;
            this.DefName.Click += new System.EventHandler(this.DefName_Click);
            // 
            // SynthType
            // 
            this.SynthType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SynthType.FormattingEnabled = true;
            this.SynthType.Items.AddRange(new object[] {
            "FM internal synthesizer",
            "Generic internal synthesizer",
            "Hardware MIDI output port",
            "Hardware wavetable synthesizer",
            "Microsoft MIDI Mapper",
            "Software synthesizer",
            "Square wave internal synthesizer"});
            this.SynthType.Location = new System.Drawing.Point(112, 96);
            this.SynthType.Name = "SynthType";
            this.SynthType.Size = new System.Drawing.Size(241, 21);
            this.SynthType.TabIndex = 9;
            this.SynthType.SelectedIndexChanged += new System.EventHandler(this.SynthType_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(53, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Mask type:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(228, 125);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "PID:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(132, 125);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(28, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "VID:";
            // 
            // VIDPIDList
            // 
            this.VIDPIDList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.VIDPIDList.AutoSize = true;
            this.VIDPIDList.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(0)))), ((int)(((byte)(119)))));
            this.VIDPIDList.Location = new System.Drawing.Point(13, 161);
            this.VIDPIDList.Name = "VIDPIDList";
            this.VIDPIDList.Size = new System.Drawing.Size(167, 13);
            this.VIDPIDList.TabIndex = 15;
            this.VIDPIDList.TabStop = true;
            this.VIDPIDList.Text = "Vendor and product IDs database";
            this.VIDPIDList.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.VIDPIDList_LinkClicked);
            // 
            // PIDValue
            // 
            this.PIDValue.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PIDValue.Hexadecimal = true;
            this.PIDValue.Location = new System.Drawing.Point(256, 123);
            this.PIDValue.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.PIDValue.Name = "PIDValue";
            this.PIDValue.Size = new System.Drawing.Size(62, 22);
            this.PIDValue.TabIndex = 14;
            this.PIDValue.Value = new decimal(new int[] {
            57005,
            0,
            0,
            0});
            this.PIDValue.ValueChanged += new System.EventHandler(this.PIDValue_ValueChanged);
            // 
            // VIDValue
            // 
            this.VIDValue.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VIDValue.Hexadecimal = true;
            this.VIDValue.Location = new System.Drawing.Point(160, 123);
            this.VIDValue.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.VIDValue.Name = "VIDValue";
            this.VIDValue.Size = new System.Drawing.Size(62, 22);
            this.VIDValue.TabIndex = 13;
            this.VIDValue.Value = new decimal(new int[] {
            51966,
            0,
            0,
            0});
            this.VIDValue.ValueChanged += new System.EventHandler(this.VIDValue_ValueChanged);
            // 
            // MaskSynthAsAnother
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(405, 191);
            this.Controls.Add(this.VIDPIDList);
            this.Controls.Add(this.PIDValue);
            this.Controls.Add(this.VIDValue);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.SynthType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.DefName);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.Names);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MaskSynthAsAnother";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Mask synthesizer as another";
            this.Load += new System.EventHandler(this.MaskSynthAsAnother_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PIDValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIDValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox Names;
        private System.Windows.Forms.Button OK;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button DefName;
        private System.Windows.Forms.ComboBox SynthType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private HexNumericUpDown VIDValue;
        private HexNumericUpDown PIDValue;
        private OmniMIDIConfigurator.LinkLabelEx VIDPIDList;
    }
}