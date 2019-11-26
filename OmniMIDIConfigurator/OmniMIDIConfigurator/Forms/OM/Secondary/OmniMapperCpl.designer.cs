namespace OmniMIDIConfigurator
{
    partial class OmniMapperCpl
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
            this.MIDIOutList = new System.Windows.Forms.ComboBox();
            this.ApplyBtn = new System.Windows.Forms.Button();
            this.CurDevice = new System.Windows.Forms.Label();
            this.Pltfrm = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Map mapper to device:";
            // 
            // MIDIOutList
            // 
            this.MIDIOutList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MIDIOutList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MIDIOutList.FormattingEnabled = true;
            this.MIDIOutList.Location = new System.Drawing.Point(12, 54);
            this.MIDIOutList.Name = "MIDIOutList";
            this.MIDIOutList.Size = new System.Drawing.Size(335, 21);
            this.MIDIOutList.TabIndex = 3;
            this.MIDIOutList.SelectedIndexChanged += new System.EventHandler(this.MIDIOutList_SelectedIndexChanged);
            // 
            // ApplyBtn
            // 
            this.ApplyBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ApplyBtn.Location = new System.Drawing.Point(273, 80);
            this.ApplyBtn.Name = "ApplyBtn";
            this.ApplyBtn.Size = new System.Drawing.Size(75, 23);
            this.ApplyBtn.TabIndex = 4;
            this.ApplyBtn.Text = "Apply";
            this.ApplyBtn.UseVisualStyleBackColor = true;
            this.ApplyBtn.Click += new System.EventHandler(this.ApplyBtn_Click);
            // 
            // CurDevice
            // 
            this.CurDevice.AutoSize = true;
            this.CurDevice.Location = new System.Drawing.Point(10, 12);
            this.CurDevice.Name = "CurDevice";
            this.CurDevice.Size = new System.Drawing.Size(108, 13);
            this.CurDevice.TabIndex = 5;
            this.CurDevice.Text = "Current device: None";
            // 
            // Pltfrm
            // 
            this.Pltfrm.AutoSize = true;
            this.Pltfrm.Location = new System.Drawing.Point(9, 85);
            this.Pltfrm.Name = "Pltfrm";
            this.Pltfrm.Size = new System.Drawing.Size(71, 13);
            this.Pltfrm.TabIndex = 6;
            this.Pltfrm.Text = "Platform: i386";
            // 
            // OmniMapperCpl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 114);
            this.Controls.Add(this.Pltfrm);
            this.Controls.Add(this.CurDevice);
            this.Controls.Add(this.ApplyBtn);
            this.Controls.Add(this.MIDIOutList);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OmniMapperCpl";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Change OmniMapper settings";
            this.Load += new System.EventHandler(this.OmniMapperCpl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox MIDIOutList;
        private System.Windows.Forms.Button ApplyBtn;
        private System.Windows.Forms.Label CurDevice;
        private System.Windows.Forms.Label Pltfrm;
    }
}