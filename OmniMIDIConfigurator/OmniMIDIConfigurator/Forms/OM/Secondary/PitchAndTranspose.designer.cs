namespace OmniMIDIConfigurator
{
    partial class PitchAndTranspose
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PitchAndTranspose));
            this.label1 = new System.Windows.Forms.Label();
            this.NewTranspose = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.ApplyBtn = new System.Windows.Forms.Button();
            this.CH1 = new System.Windows.Forms.CheckBox();
            this.CH2 = new System.Windows.Forms.CheckBox();
            this.CH3 = new System.Windows.Forms.CheckBox();
            this.CH4 = new System.Windows.Forms.CheckBox();
            this.CH5 = new System.Windows.Forms.CheckBox();
            this.CH9 = new System.Windows.Forms.CheckBox();
            this.CH8 = new System.Windows.Forms.CheckBox();
            this.CH7 = new System.Windows.Forms.CheckBox();
            this.CH6 = new System.Windows.Forms.CheckBox();
            this.CH15 = new System.Windows.Forms.CheckBox();
            this.CH14 = new System.Windows.Forms.CheckBox();
            this.CH13 = new System.Windows.Forms.CheckBox();
            this.CH12 = new System.Windows.Forms.CheckBox();
            this.CH16 = new System.Windows.Forms.CheckBox();
            this.LiveBtn = new System.Windows.Forms.CheckBox();
            this.CheckA = new System.Windows.Forms.Button();
            this.UncheckA = new System.Windows.Forms.Button();
            this.TimerLive = new System.Windows.Forms.Timer(this.components);
            this.CH11 = new System.Windows.Forms.CheckBox();
            this.CH10 = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.NewCPitch = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.NewTranspose)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NewCPitch)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(14, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(516, 61);
            this.label1.TabIndex = 1;
            this.label1.Text = resources.GetString("label1.Text");
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NewTranspose
            // 
            this.NewTranspose.Location = new System.Drawing.Point(119, 22);
            this.NewTranspose.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.NewTranspose.Minimum = new decimal(new int[] {
            127,
            0,
            0,
            -2147483648});
            this.NewTranspose.Name = "NewTranspose";
            this.NewTranspose.Size = new System.Drawing.Size(64, 23);
            this.NewTranspose.TabIndex = 6;
            this.NewTranspose.ValueChanged += new System.EventHandler(this.NewTranspose_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Transpose value:";
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelBtn.Location = new System.Drawing.Point(394, 308);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(66, 27);
            this.CancelBtn.TabIndex = 8;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // ApplyBtn
            // 
            this.ApplyBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ApplyBtn.Location = new System.Drawing.Point(463, 308);
            this.ApplyBtn.Name = "ApplyBtn";
            this.ApplyBtn.Size = new System.Drawing.Size(66, 27);
            this.ApplyBtn.TabIndex = 7;
            this.ApplyBtn.Text = "Apply";
            this.ApplyBtn.UseVisualStyleBackColor = true;
            this.ApplyBtn.Click += new System.EventHandler(this.ApplyBtn_Click);
            // 
            // CH1
            // 
            this.CH1.AutoSize = true;
            this.CH1.Location = new System.Drawing.Point(10, 22);
            this.CH1.Name = "CH1";
            this.CH1.Size = new System.Drawing.Size(79, 19);
            this.CH1.TabIndex = 10;
            this.CH1.Text = "Channel 1";
            this.CH1.UseVisualStyleBackColor = true;
            // 
            // CH2
            // 
            this.CH2.AutoSize = true;
            this.CH2.Location = new System.Drawing.Point(10, 42);
            this.CH2.Name = "CH2";
            this.CH2.Size = new System.Drawing.Size(79, 19);
            this.CH2.TabIndex = 11;
            this.CH2.Text = "Channel 2";
            this.CH2.UseVisualStyleBackColor = true;
            // 
            // CH3
            // 
            this.CH3.AutoSize = true;
            this.CH3.Location = new System.Drawing.Point(10, 61);
            this.CH3.Name = "CH3";
            this.CH3.Size = new System.Drawing.Size(79, 19);
            this.CH3.TabIndex = 12;
            this.CH3.Text = "Channel 3";
            this.CH3.UseVisualStyleBackColor = true;
            // 
            // CH4
            // 
            this.CH4.AutoSize = true;
            this.CH4.Location = new System.Drawing.Point(10, 81);
            this.CH4.Name = "CH4";
            this.CH4.Size = new System.Drawing.Size(79, 19);
            this.CH4.TabIndex = 13;
            this.CH4.Text = "Channel 4";
            this.CH4.UseVisualStyleBackColor = true;
            // 
            // CH5
            // 
            this.CH5.AutoSize = true;
            this.CH5.Location = new System.Drawing.Point(108, 22);
            this.CH5.Name = "CH5";
            this.CH5.Size = new System.Drawing.Size(79, 19);
            this.CH5.TabIndex = 14;
            this.CH5.Text = "Channel 6";
            this.CH5.UseVisualStyleBackColor = true;
            // 
            // CH9
            // 
            this.CH9.AutoSize = true;
            this.CH9.Location = new System.Drawing.Point(206, 23);
            this.CH9.Name = "CH9";
            this.CH9.Size = new System.Drawing.Size(79, 19);
            this.CH9.TabIndex = 18;
            this.CH9.Text = "Channel 9";
            this.CH9.UseVisualStyleBackColor = true;
            // 
            // CH8
            // 
            this.CH8.AutoSize = true;
            this.CH8.Location = new System.Drawing.Point(108, 81);
            this.CH8.Name = "CH8";
            this.CH8.Size = new System.Drawing.Size(79, 19);
            this.CH8.TabIndex = 17;
            this.CH8.Text = "Channel 8";
            this.CH8.UseVisualStyleBackColor = true;
            // 
            // CH7
            // 
            this.CH7.AutoSize = true;
            this.CH7.Location = new System.Drawing.Point(108, 61);
            this.CH7.Name = "CH7";
            this.CH7.Size = new System.Drawing.Size(79, 19);
            this.CH7.TabIndex = 16;
            this.CH7.Tag = "";
            this.CH7.Text = "Channel 7";
            this.CH7.UseVisualStyleBackColor = true;
            // 
            // CH6
            // 
            this.CH6.AutoSize = true;
            this.CH6.Location = new System.Drawing.Point(108, 42);
            this.CH6.Name = "CH6";
            this.CH6.Size = new System.Drawing.Size(79, 19);
            this.CH6.TabIndex = 15;
            this.CH6.Text = "Channel 7";
            this.CH6.UseVisualStyleBackColor = true;
            // 
            // CH15
            // 
            this.CH15.AutoSize = true;
            this.CH15.Location = new System.Drawing.Point(304, 62);
            this.CH15.Name = "CH15";
            this.CH15.Size = new System.Drawing.Size(85, 19);
            this.CH15.TabIndex = 24;
            this.CH15.Text = "Channel 15";
            this.CH15.UseVisualStyleBackColor = true;
            // 
            // CH14
            // 
            this.CH14.AutoSize = true;
            this.CH14.Location = new System.Drawing.Point(304, 43);
            this.CH14.Name = "CH14";
            this.CH14.Size = new System.Drawing.Size(85, 19);
            this.CH14.TabIndex = 23;
            this.CH14.Text = "Channel 14";
            this.CH14.UseVisualStyleBackColor = true;
            // 
            // CH13
            // 
            this.CH13.AutoSize = true;
            this.CH13.Location = new System.Drawing.Point(304, 23);
            this.CH13.Name = "CH13";
            this.CH13.Size = new System.Drawing.Size(85, 19);
            this.CH13.TabIndex = 22;
            this.CH13.Text = "Channel 13";
            this.CH13.UseVisualStyleBackColor = true;
            // 
            // CH12
            // 
            this.CH12.AutoSize = true;
            this.CH12.Location = new System.Drawing.Point(206, 82);
            this.CH12.Name = "CH12";
            this.CH12.Size = new System.Drawing.Size(85, 19);
            this.CH12.TabIndex = 21;
            this.CH12.Text = "Channel 12";
            this.CH12.UseVisualStyleBackColor = true;
            // 
            // CH16
            // 
            this.CH16.AutoSize = true;
            this.CH16.Location = new System.Drawing.Point(304, 82);
            this.CH16.Name = "CH16";
            this.CH16.Size = new System.Drawing.Size(85, 19);
            this.CH16.TabIndex = 25;
            this.CH16.Text = "Channel 16";
            this.CH16.UseVisualStyleBackColor = true;
            // 
            // LiveBtn
            // 
            this.LiveBtn.Appearance = System.Windows.Forms.Appearance.Button;
            this.LiveBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LiveBtn.ForeColor = System.Drawing.Color.DarkRed;
            this.LiveBtn.Location = new System.Drawing.Point(422, 15);
            this.LiveBtn.Name = "LiveBtn";
            this.LiveBtn.Size = new System.Drawing.Size(87, 27);
            this.LiveBtn.TabIndex = 46;
            this.LiveBtn.Text = "Live";
            this.LiveBtn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LiveBtn.UseVisualStyleBackColor = true;
            this.LiveBtn.CheckedChanged += new System.EventHandler(this.LiveBtn_CheckedChanged);
            // 
            // CheckA
            // 
            this.CheckA.Location = new System.Drawing.Point(422, 53);
            this.CheckA.Name = "CheckA";
            this.CheckA.Size = new System.Drawing.Size(87, 27);
            this.CheckA.TabIndex = 43;
            this.CheckA.Text = "Check all";
            this.CheckA.UseVisualStyleBackColor = true;
            this.CheckA.Click += new System.EventHandler(this.CheckA_Click);
            // 
            // UncheckA
            // 
            this.UncheckA.Location = new System.Drawing.Point(422, 81);
            this.UncheckA.Name = "UncheckA";
            this.UncheckA.Size = new System.Drawing.Size(87, 27);
            this.UncheckA.TabIndex = 44;
            this.UncheckA.Text = "Uncheck all";
            this.UncheckA.UseVisualStyleBackColor = true;
            this.UncheckA.Click += new System.EventHandler(this.UncheckA_Click);
            // 
            // TimerLive
            // 
            this.TimerLive.Tick += new System.EventHandler(this.TimerLive_Tick);
            // 
            // CH11
            // 
            this.CH11.AutoSize = true;
            this.CH11.Location = new System.Drawing.Point(206, 62);
            this.CH11.Name = "CH11";
            this.CH11.Size = new System.Drawing.Size(85, 19);
            this.CH11.TabIndex = 20;
            this.CH11.Text = "Channel 11";
            this.CH11.UseVisualStyleBackColor = true;
            // 
            // CH10
            // 
            this.CH10.AutoSize = true;
            this.CH10.Location = new System.Drawing.Point(206, 43);
            this.CH10.Name = "CH10";
            this.CH10.Size = new System.Drawing.Size(85, 19);
            this.CH10.TabIndex = 19;
            this.CH10.Text = "Channel 10";
            this.CH10.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CH1);
            this.groupBox1.Controls.Add(this.LiveBtn);
            this.groupBox1.Controls.Add(this.CH10);
            this.groupBox1.Controls.Add(this.UncheckA);
            this.groupBox1.Controls.Add(this.CH2);
            this.groupBox1.Controls.Add(this.CheckA);
            this.groupBox1.Controls.Add(this.CH3);
            this.groupBox1.Controls.Add(this.CH16);
            this.groupBox1.Controls.Add(this.CH4);
            this.groupBox1.Controls.Add(this.CH15);
            this.groupBox1.Controls.Add(this.CH5);
            this.groupBox1.Controls.Add(this.CH14);
            this.groupBox1.Controls.Add(this.CH6);
            this.groupBox1.Controls.Add(this.CH13);
            this.groupBox1.Controls.Add(this.CH7);
            this.groupBox1.Controls.Add(this.CH8);
            this.groupBox1.Controls.Add(this.CH12);
            this.groupBox1.Controls.Add(this.CH9);
            this.groupBox1.Controls.Add(this.CH11);
            this.groupBox1.Location = new System.Drawing.Point(14, 181);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(516, 115);
            this.groupBox1.TabIndex = 47;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Channels to affect";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.NewCPitch);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.NewTranspose);
            this.groupBox2.Location = new System.Drawing.Point(14, 85);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(516, 89);
            this.groupBox2.TabIndex = 48;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Settings";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(201, 54);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(300, 15);
            this.label5.TabIndex = 10;
            this.label5.Text = "Concert pitch:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(201, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(300, 15);
            this.label4.TabIndex = 9;
            this.label4.Text = "Root key:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "Concert pitch:";
            // 
            // NewCPitch
            // 
            this.NewCPitch.Location = new System.Drawing.Point(119, 50);
            this.NewCPitch.Maximum = new decimal(new int[] {
            8192,
            0,
            0,
            0});
            this.NewCPitch.Minimum = new decimal(new int[] {
            8192,
            0,
            0,
            -2147483648});
            this.NewCPitch.Name = "NewCPitch";
            this.NewCPitch.Size = new System.Drawing.Size(64, 23);
            this.NewCPitch.TabIndex = 8;
            this.NewCPitch.ValueChanged += new System.EventHandler(this.NewCPitch_ValueChanged);
            // 
            // PitchAndTranspose
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(544, 348);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.ApplyBtn);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "PitchAndTranspose";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Transposing and concert pitch settings";
            this.Load += new System.EventHandler(this.PitchShifting_Load);
            ((System.ComponentModel.ISupportInitialize)(this.NewTranspose)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NewCPitch)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown NewTranspose;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button ApplyBtn;
        private System.Windows.Forms.CheckBox CH1;
        private System.Windows.Forms.CheckBox CH2;
        private System.Windows.Forms.CheckBox CH3;
        private System.Windows.Forms.CheckBox CH4;
        private System.Windows.Forms.CheckBox CH5;
        private System.Windows.Forms.CheckBox CH9;
        private System.Windows.Forms.CheckBox CH8;
        private System.Windows.Forms.CheckBox CH7;
        private System.Windows.Forms.CheckBox CH6;
        private System.Windows.Forms.CheckBox CH15;
        private System.Windows.Forms.CheckBox CH14;
        private System.Windows.Forms.CheckBox CH13;
        private System.Windows.Forms.CheckBox CH12;
        private System.Windows.Forms.CheckBox CH16;
        private System.Windows.Forms.CheckBox LiveBtn;
        private System.Windows.Forms.Button CheckA;
        private System.Windows.Forms.Button UncheckA;
        private System.Windows.Forms.Timer TimerLive;
        private System.Windows.Forms.CheckBox CH11;
        private System.Windows.Forms.CheckBox CH10;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown NewCPitch;
        private System.Windows.Forms.Label label5;
    }
}