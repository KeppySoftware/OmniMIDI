namespace OmniMIDIConfigurator
{
    partial class PitchShifting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PitchShifting));
            this.label1 = new System.Windows.Forms.Label();
            this.NewPitch = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.ApplyBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
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
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.LiveBtn = new System.Windows.Forms.CheckBox();
            this.CheckA = new System.Windows.Forms.Button();
            this.UncheckA = new System.Windows.Forms.Button();
            this.TimerLive = new System.Windows.Forms.Timer(this.components);
            this.CH11 = new System.Windows.Forms.CheckBox();
            this.CH10 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.NewPitch)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(461, 53);
            this.label1.TabIndex = 1;
            this.label1.Text = resources.GetString("label1.Text");
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NewPitch
            // 
            this.NewPitch.Location = new System.Drawing.Point(55, 74);
            this.NewPitch.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.NewPitch.Minimum = new decimal(new int[] {
            127,
            0,
            0,
            -2147483648});
            this.NewPitch.Name = "NewPitch";
            this.NewPitch.Size = new System.Drawing.Size(55, 20);
            this.NewPitch.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Pitch:";
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(357, 107);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(57, 23);
            this.CancelBtn.TabIndex = 8;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // ApplyBtn
            // 
            this.ApplyBtn.Location = new System.Drawing.Point(416, 107);
            this.ApplyBtn.Name = "ApplyBtn";
            this.ApplyBtn.Size = new System.Drawing.Size(57, 23);
            this.ApplyBtn.TabIndex = 7;
            this.ApplyBtn.Text = "Apply";
            this.ApplyBtn.UseVisualStyleBackColor = true;
            this.ApplyBtn.Click += new System.EventHandler(this.ApplyBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(124, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Channels:";
            // 
            // CH1
            // 
            this.CH1.AutoSize = true;
            this.CH1.Location = new System.Drawing.Point(182, 71);
            this.CH1.Name = "CH1";
            this.CH1.Size = new System.Drawing.Size(15, 14);
            this.CH1.TabIndex = 10;
            this.CH1.UseVisualStyleBackColor = true;
            // 
            // CH2
            // 
            this.CH2.AutoSize = true;
            this.CH2.Location = new System.Drawing.Point(200, 71);
            this.CH2.Name = "CH2";
            this.CH2.Size = new System.Drawing.Size(15, 14);
            this.CH2.TabIndex = 11;
            this.CH2.UseVisualStyleBackColor = true;
            // 
            // CH3
            // 
            this.CH3.AutoSize = true;
            this.CH3.Location = new System.Drawing.Point(218, 71);
            this.CH3.Name = "CH3";
            this.CH3.Size = new System.Drawing.Size(15, 14);
            this.CH3.TabIndex = 12;
            this.CH3.UseVisualStyleBackColor = true;
            // 
            // CH4
            // 
            this.CH4.AutoSize = true;
            this.CH4.Location = new System.Drawing.Point(236, 71);
            this.CH4.Name = "CH4";
            this.CH4.Size = new System.Drawing.Size(15, 14);
            this.CH4.TabIndex = 13;
            this.CH4.UseVisualStyleBackColor = true;
            // 
            // CH5
            // 
            this.CH5.AutoSize = true;
            this.CH5.Location = new System.Drawing.Point(254, 71);
            this.CH5.Name = "CH5";
            this.CH5.Size = new System.Drawing.Size(15, 14);
            this.CH5.TabIndex = 14;
            this.CH5.UseVisualStyleBackColor = true;
            // 
            // CH9
            // 
            this.CH9.AutoSize = true;
            this.CH9.Location = new System.Drawing.Point(326, 71);
            this.CH9.Name = "CH9";
            this.CH9.Size = new System.Drawing.Size(15, 14);
            this.CH9.TabIndex = 18;
            this.CH9.UseVisualStyleBackColor = true;
            // 
            // CH8
            // 
            this.CH8.AutoSize = true;
            this.CH8.Location = new System.Drawing.Point(308, 71);
            this.CH8.Name = "CH8";
            this.CH8.Size = new System.Drawing.Size(15, 14);
            this.CH8.TabIndex = 17;
            this.CH8.UseVisualStyleBackColor = true;
            // 
            // CH7
            // 
            this.CH7.AutoSize = true;
            this.CH7.Location = new System.Drawing.Point(290, 71);
            this.CH7.Name = "CH7";
            this.CH7.Size = new System.Drawing.Size(15, 14);
            this.CH7.TabIndex = 16;
            this.CH7.UseVisualStyleBackColor = true;
            // 
            // CH6
            // 
            this.CH6.AutoSize = true;
            this.CH6.Location = new System.Drawing.Point(272, 71);
            this.CH6.Name = "CH6";
            this.CH6.Size = new System.Drawing.Size(15, 14);
            this.CH6.TabIndex = 15;
            this.CH6.UseVisualStyleBackColor = true;
            // 
            // CH15
            // 
            this.CH15.AutoSize = true;
            this.CH15.Location = new System.Drawing.Point(434, 71);
            this.CH15.Name = "CH15";
            this.CH15.Size = new System.Drawing.Size(15, 14);
            this.CH15.TabIndex = 24;
            this.CH15.UseVisualStyleBackColor = true;
            // 
            // CH14
            // 
            this.CH14.AutoSize = true;
            this.CH14.Location = new System.Drawing.Point(416, 71);
            this.CH14.Name = "CH14";
            this.CH14.Size = new System.Drawing.Size(15, 14);
            this.CH14.TabIndex = 23;
            this.CH14.UseVisualStyleBackColor = true;
            // 
            // CH13
            // 
            this.CH13.AutoSize = true;
            this.CH13.Location = new System.Drawing.Point(398, 71);
            this.CH13.Name = "CH13";
            this.CH13.Size = new System.Drawing.Size(15, 14);
            this.CH13.TabIndex = 22;
            this.CH13.UseVisualStyleBackColor = true;
            // 
            // CH12
            // 
            this.CH12.AutoSize = true;
            this.CH12.Location = new System.Drawing.Point(380, 71);
            this.CH12.Name = "CH12";
            this.CH12.Size = new System.Drawing.Size(15, 14);
            this.CH12.TabIndex = 21;
            this.CH12.UseVisualStyleBackColor = true;
            // 
            // CH16
            // 
            this.CH16.AutoSize = true;
            this.CH16.Location = new System.Drawing.Point(452, 71);
            this.CH16.Name = "CH16";
            this.CH16.Size = new System.Drawing.Size(15, 14);
            this.CH16.TabIndex = 25;
            this.CH16.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(182, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(13, 13);
            this.label4.TabIndex = 26;
            this.label4.Text = "1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(200, 88);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(13, 13);
            this.label5.TabIndex = 27;
            this.label5.Text = "2";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(449, 88);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(19, 13);
            this.label6.TabIndex = 28;
            this.label6.Text = "16";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(218, 88);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(13, 13);
            this.label7.TabIndex = 29;
            this.label7.Text = "3";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(236, 88);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(13, 13);
            this.label8.TabIndex = 30;
            this.label8.Text = "4";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(254, 88);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(13, 13);
            this.label9.TabIndex = 31;
            this.label9.Text = "5";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(272, 88);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(13, 13);
            this.label10.TabIndex = 32;
            this.label10.Text = "6";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(290, 88);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(13, 13);
            this.label11.TabIndex = 33;
            this.label11.Text = "7";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(308, 88);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(13, 13);
            this.label12.TabIndex = 34;
            this.label12.Text = "8";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(326, 88);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(13, 13);
            this.label13.TabIndex = 35;
            this.label13.Text = "9";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(341, 88);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(19, 13);
            this.label14.TabIndex = 36;
            this.label14.Text = "10";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(359, 88);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(19, 13);
            this.label15.TabIndex = 37;
            this.label15.Text = "11";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(377, 88);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(19, 13);
            this.label16.TabIndex = 38;
            this.label16.Text = "12";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(395, 88);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(19, 13);
            this.label17.TabIndex = 39;
            this.label17.Text = "13";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(413, 88);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(19, 13);
            this.label18.TabIndex = 40;
            this.label18.Text = "14";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(431, 88);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(19, 13);
            this.label19.TabIndex = 41;
            this.label19.Text = "15";
            // 
            // LiveBtn
            // 
            this.LiveBtn.Appearance = System.Windows.Forms.Appearance.Button;
            this.LiveBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LiveBtn.ForeColor = System.Drawing.Color.DarkRed;
            this.LiveBtn.Location = new System.Drawing.Point(298, 107);
            this.LiveBtn.Name = "LiveBtn";
            this.LiveBtn.Size = new System.Drawing.Size(57, 23);
            this.LiveBtn.TabIndex = 46;
            this.LiveBtn.Text = "Live";
            this.LiveBtn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LiveBtn.UseVisualStyleBackColor = true;
            this.LiveBtn.CheckedChanged += new System.EventHandler(this.LiveBtn_CheckedChanged);
            // 
            // CheckA
            // 
            this.CheckA.Location = new System.Drawing.Point(15, 107);
            this.CheckA.Name = "CheckA";
            this.CheckA.Size = new System.Drawing.Size(75, 23);
            this.CheckA.TabIndex = 43;
            this.CheckA.Text = "Check all";
            this.CheckA.UseVisualStyleBackColor = true;
            this.CheckA.Click += new System.EventHandler(this.CheckA_Click);
            // 
            // UncheckA
            // 
            this.UncheckA.Location = new System.Drawing.Point(92, 107);
            this.UncheckA.Name = "UncheckA";
            this.UncheckA.Size = new System.Drawing.Size(75, 23);
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
            this.CH11.Location = new System.Drawing.Point(362, 71);
            this.CH11.Name = "CH11";
            this.CH11.Size = new System.Drawing.Size(15, 14);
            this.CH11.TabIndex = 20;
            this.CH11.UseVisualStyleBackColor = true;
            // 
            // CH10
            // 
            this.CH10.AutoSize = true;
            this.CH10.Location = new System.Drawing.Point(344, 71);
            this.CH10.Name = "CH10";
            this.CH10.Size = new System.Drawing.Size(15, 14);
            this.CH10.TabIndex = 19;
            this.CH10.UseVisualStyleBackColor = true;
            // 
            // PitchShifting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(485, 142);
            this.Controls.Add(this.CH10);
            this.Controls.Add(this.LiveBtn);
            this.Controls.Add(this.UncheckA);
            this.Controls.Add(this.CheckA);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.CH16);
            this.Controls.Add(this.CH15);
            this.Controls.Add(this.CH14);
            this.Controls.Add(this.CH13);
            this.Controls.Add(this.CH12);
            this.Controls.Add(this.CH11);
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
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.ApplyBtn);
            this.Controls.Add(this.NewPitch);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "PitchShifting";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Change transpose value";
            this.Load += new System.EventHandler(this.PitchShifting_Load);
            ((System.ComponentModel.ISupportInitialize)(this.NewPitch)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown NewPitch;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button ApplyBtn;
        private System.Windows.Forms.Label label2;
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
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.CheckBox LiveBtn;
        private System.Windows.Forms.Button CheckA;
        private System.Windows.Forms.Button UncheckA;
        private System.Windows.Forms.Timer TimerLive;
        private System.Windows.Forms.CheckBox CH11;
        private System.Windows.Forms.CheckBox CH10;
    }
}