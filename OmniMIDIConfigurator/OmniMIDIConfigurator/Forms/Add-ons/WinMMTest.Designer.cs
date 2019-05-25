namespace OmniMIDIConfigurator
{
    partial class WinMMTest
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
            this.InitMIDIOut = new System.Windows.Forms.Button();
            this.StopMIDIOut = new System.Windows.Forms.Button();
            this.MidiDisconnectA = new System.Windows.Forms.Button();
            this.MidiConnectA = new System.Windows.Forms.Button();
            this.OutCombo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.InCombo = new System.Windows.Forms.ComboBox();
            this.refreshdev = new System.Windows.Forms.Button();
            this.StopMIDIIn = new System.Windows.Forms.Button();
            this.InitMIDIIn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // InitMIDIOut
            // 
            this.InitMIDIOut.Location = new System.Drawing.Point(28, 39);
            this.InitMIDIOut.Name = "InitMIDIOut";
            this.InitMIDIOut.Size = new System.Drawing.Size(131, 23);
            this.InitMIDIOut.TabIndex = 0;
            this.InitMIDIOut.Text = "midiOutOpen sel";
            this.InitMIDIOut.UseVisualStyleBackColor = true;
            this.InitMIDIOut.Click += new System.EventHandler(this.InitMIDIOut_Click);
            // 
            // StopMIDIOut
            // 
            this.StopMIDIOut.Location = new System.Drawing.Point(28, 68);
            this.StopMIDIOut.Name = "StopMIDIOut";
            this.StopMIDIOut.Size = new System.Drawing.Size(131, 23);
            this.StopMIDIOut.TabIndex = 1;
            this.StopMIDIOut.Text = "midiOutClose sel";
            this.StopMIDIOut.UseVisualStyleBackColor = true;
            this.StopMIDIOut.Click += new System.EventHandler(this.StopMIDIOut_Click);
            // 
            // MidiDisconnectA
            // 
            this.MidiDisconnectA.Location = new System.Drawing.Point(109, 153);
            this.MidiDisconnectA.Name = "MidiDisconnectA";
            this.MidiDisconnectA.Size = new System.Drawing.Size(131, 23);
            this.MidiDisconnectA.TabIndex = 3;
            this.MidiDisconnectA.Text = "midiDisconnect sel";
            this.MidiDisconnectA.UseVisualStyleBackColor = true;
            this.MidiDisconnectA.Click += new System.EventHandler(this.MidiDisconnectA_Click);
            // 
            // MidiConnectA
            // 
            this.MidiConnectA.Location = new System.Drawing.Point(109, 124);
            this.MidiConnectA.Name = "MidiConnectA";
            this.MidiConnectA.Size = new System.Drawing.Size(131, 23);
            this.MidiConnectA.TabIndex = 2;
            this.MidiConnectA.Text = "midiConnect sel";
            this.MidiConnectA.UseVisualStyleBackColor = true;
            this.MidiConnectA.Click += new System.EventHandler(this.MidiConnectA_Click);
            // 
            // OutCombo
            // 
            this.OutCombo.Enabled = false;
            this.OutCombo.FormattingEnabled = true;
            this.OutCombo.Location = new System.Drawing.Point(37, 12);
            this.OutCombo.Name = "OutCombo";
            this.OutCombo.Size = new System.Drawing.Size(121, 21);
            this.OutCombo.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(22, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "out";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(179, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "in";
            // 
            // InCombo
            // 
            this.InCombo.Enabled = false;
            this.InCombo.FormattingEnabled = true;
            this.InCombo.Location = new System.Drawing.Point(194, 12);
            this.InCombo.Name = "InCombo";
            this.InCombo.Size = new System.Drawing.Size(121, 21);
            this.InCombo.TabIndex = 6;
            // 
            // refreshdev
            // 
            this.refreshdev.Location = new System.Drawing.Point(402, 12);
            this.refreshdev.Name = "refreshdev";
            this.refreshdev.Size = new System.Drawing.Size(75, 23);
            this.refreshdev.TabIndex = 8;
            this.refreshdev.Text = "RefreshDev";
            this.refreshdev.UseVisualStyleBackColor = true;
            this.refreshdev.Click += new System.EventHandler(this.Refreshdev_Click);
            // 
            // StopMIDIIn
            // 
            this.StopMIDIIn.Location = new System.Drawing.Point(185, 68);
            this.StopMIDIIn.Name = "StopMIDIIn";
            this.StopMIDIIn.Size = new System.Drawing.Size(131, 23);
            this.StopMIDIIn.TabIndex = 10;
            this.StopMIDIIn.Text = "midiInClose sel";
            this.StopMIDIIn.UseVisualStyleBackColor = true;
            this.StopMIDIIn.Click += new System.EventHandler(this.StopMIDIIn_Click);
            // 
            // InitMIDIIn
            // 
            this.InitMIDIIn.Location = new System.Drawing.Point(185, 39);
            this.InitMIDIIn.Name = "InitMIDIIn";
            this.InitMIDIIn.Size = new System.Drawing.Size(131, 23);
            this.InitMIDIIn.TabIndex = 9;
            this.InitMIDIIn.Text = "midiInOpen sel";
            this.InitMIDIIn.UseVisualStyleBackColor = true;
            this.InitMIDIIn.Click += new System.EventHandler(this.InitMIDIIn_Click);
            // 
            // WinMMTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 286);
            this.Controls.Add(this.StopMIDIIn);
            this.Controls.Add(this.InitMIDIIn);
            this.Controls.Add(this.refreshdev);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.InCombo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.OutCombo);
            this.Controls.Add(this.MidiDisconnectA);
            this.Controls.Add(this.MidiConnectA);
            this.Controls.Add(this.StopMIDIOut);
            this.Controls.Add(this.InitMIDIOut);
            this.Name = "WinMMTest";
            this.Text = "WinMMTest";
            this.Load += new System.EventHandler(this.WinMMTest_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button InitMIDIOut;
        private System.Windows.Forms.Button StopMIDIOut;
        private System.Windows.Forms.Button MidiDisconnectA;
        private System.Windows.Forms.Button MidiConnectA;
        private System.Windows.Forms.ComboBox OutCombo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox InCombo;
        private System.Windows.Forms.Button refreshdev;
        private System.Windows.Forms.Button StopMIDIIn;
        private System.Windows.Forms.Button InitMIDIIn;
    }
}