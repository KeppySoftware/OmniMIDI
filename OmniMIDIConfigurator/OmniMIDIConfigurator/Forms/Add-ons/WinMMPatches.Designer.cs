namespace OmniMIDIConfigurator
{
    partial class WinMMPatches
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
            this.OKClose = new System.Windows.Forms.Button();
            this.UnpatchApp = new System.Windows.Forms.Button();
            this.PatchStatusLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.WMMW = new System.Windows.Forms.Button();
            this.BMPatch = new System.Windows.Forms.GroupBox();
            this.WMMD = new System.Windows.Forms.Button();
            this.DAWPatch = new System.Windows.Forms.GroupBox();
            this.BMPatch.SuspendLayout();
            this.DAWPatch.SuspendLayout();
            this.SuspendLayout();
            // 
            // OKClose
            // 
            this.OKClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKClose.Location = new System.Drawing.Point(198, 133);
            this.OKClose.Name = "OKClose";
            this.OKClose.Size = new System.Drawing.Size(37, 23);
            this.OKClose.TabIndex = 5;
            this.OKClose.Text = "OK";
            this.OKClose.UseVisualStyleBackColor = true;
            this.OKClose.Click += new System.EventHandler(this.OKClose_Click);
            // 
            // UnpatchApp
            // 
            this.UnpatchApp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.UnpatchApp.Location = new System.Drawing.Point(12, 133);
            this.UnpatchApp.Name = "UnpatchApp";
            this.UnpatchApp.Size = new System.Drawing.Size(180, 23);
            this.UnpatchApp.TabIndex = 4;
            this.UnpatchApp.Text = "Unpatch an already patched app";
            this.UnpatchApp.UseVisualStyleBackColor = true;
            this.UnpatchApp.Click += new System.EventHandler(this.UnpatchApp_Click);
            // 
            // PatchStatusLabel
            // 
            this.PatchStatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PatchStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PatchStatusLabel.ForeColor = System.Drawing.Color.Gray;
            this.PatchStatusLabel.Location = new System.Drawing.Point(52, 110);
            this.PatchStatusLabel.Name = "PatchStatusLabel";
            this.PatchStatusLabel.Size = new System.Drawing.Size(183, 13);
            this.PatchStatusLabel.TabIndex = 7;
            this.PatchStatusLabel.Text = "Waiting...";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 110);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Status:";
            // 
            // WMMW
            // 
            this.WMMW.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WMMW.Location = new System.Drawing.Point(4, 16);
            this.WMMW.Name = "WMMW";
            this.WMMW.Size = new System.Drawing.Size(215, 24);
            this.WMMW.TabIndex = 2;
            this.WMMW.Text = "Patch an application";
            this.WMMW.UseVisualStyleBackColor = true;
            this.WMMW.Click += new System.EventHandler(this.WMMW_Click);
            this.WMMW.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.BMPatch_HelpRequested);
            // 
            // BMPatch
            // 
            this.BMPatch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BMPatch.Controls.Add(this.WMMW);
            this.BMPatch.Location = new System.Drawing.Point(12, 9);
            this.BMPatch.Name = "BMPatch";
            this.BMPatch.Size = new System.Drawing.Size(223, 45);
            this.BMPatch.TabIndex = 8;
            this.BMPatch.TabStop = false;
            this.BMPatch.Text = "For Black MIDIs";
            this.BMPatch.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.BMPatch_HelpRequested);
            // 
            // WMMD
            // 
            this.WMMD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WMMD.Location = new System.Drawing.Point(4, 16);
            this.WMMD.Name = "WMMD";
            this.WMMD.Size = new System.Drawing.Size(215, 24);
            this.WMMD.TabIndex = 2;
            this.WMMD.Text = "Patch an application";
            this.WMMD.UseVisualStyleBackColor = true;
            this.WMMD.Click += new System.EventHandler(this.WMMD_Click);
            this.WMMD.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.DAWPatch_HelpRequested);
            // 
            // DAWPatch
            // 
            this.DAWPatch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DAWPatch.Controls.Add(this.WMMD);
            this.DAWPatch.Location = new System.Drawing.Point(12, 57);
            this.DAWPatch.Name = "DAWPatch";
            this.DAWPatch.Size = new System.Drawing.Size(223, 45);
            this.DAWPatch.TabIndex = 9;
            this.DAWPatch.TabStop = false;
            this.DAWPatch.Text = "For DAWs";
            this.DAWPatch.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.DAWPatch_HelpRequested);
            // 
            // WinMMPatches
            // 
            this.AcceptButton = this.OKClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(247, 168);
            this.Controls.Add(this.DAWPatch);
            this.Controls.Add(this.BMPatch);
            this.Controls.Add(this.PatchStatusLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.UnpatchApp);
            this.Controls.Add(this.OKClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WinMMPatches";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "WinMM Wrapper patch";
            this.BMPatch.ResumeLayout(false);
            this.DAWPatch.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button OKClose;
        private System.Windows.Forms.Button UnpatchApp;
        private System.Windows.Forms.Label PatchStatusLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button WMMW;
        private System.Windows.Forms.GroupBox BMPatch;
        private System.Windows.Forms.Button WMMD;
        private System.Windows.Forms.GroupBox DAWPatch;
    }
}