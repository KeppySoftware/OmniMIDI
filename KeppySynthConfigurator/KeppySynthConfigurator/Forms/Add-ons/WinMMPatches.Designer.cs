namespace KeppySynthConfigurator
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
            this.WMMW32 = new System.Windows.Forms.Button();
            this.WMMW64 = new System.Windows.Forms.Button();
            this.OKClose = new System.Windows.Forms.Button();
            this.UnpatchApp = new System.Windows.Forms.Button();
            this.PatchStatusLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.BMPatch = new System.Windows.Forms.GroupBox();
            this.DAWPatch = new System.Windows.Forms.GroupBox();
            this.WMMD32 = new System.Windows.Forms.Button();
            this.WMMD64 = new System.Windows.Forms.Button();
            this.BMPatch.SuspendLayout();
            this.DAWPatch.SuspendLayout();
            this.SuspendLayout();
            // 
            // WMMW32
            // 
            this.WMMW32.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WMMW32.Location = new System.Drawing.Point(4, 16);
            this.WMMW32.Name = "WMMW32";
            this.WMMW32.Size = new System.Drawing.Size(215, 24);
            this.WMMW32.TabIndex = 2;
            this.WMMW32.Text = "Patch a 32-bit app";
            this.WMMW32.UseVisualStyleBackColor = true;
            this.WMMW32.Click += new System.EventHandler(this.WMMW32_Click);
            this.WMMW32.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.BMPatch_HelpRequested);
            // 
            // WMMW64
            // 
            this.WMMW64.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WMMW64.Location = new System.Drawing.Point(4, 42);
            this.WMMW64.Name = "WMMW64";
            this.WMMW64.Size = new System.Drawing.Size(215, 24);
            this.WMMW64.TabIndex = 3;
            this.WMMW64.Text = "Patch a 64-bit app";
            this.WMMW64.UseVisualStyleBackColor = true;
            this.WMMW64.Click += new System.EventHandler(this.WMMW64_Click);
            this.WMMW64.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.BMPatch_HelpRequested);
            // 
            // OKClose
            // 
            this.OKClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKClose.Location = new System.Drawing.Point(198, 193);
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
            this.UnpatchApp.Location = new System.Drawing.Point(12, 193);
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
            this.PatchStatusLabel.Location = new System.Drawing.Point(52, 170);
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
            this.label1.Location = new System.Drawing.Point(12, 170);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Status:";
            // 
            // BMPatch
            // 
            this.BMPatch.Controls.Add(this.WMMW32);
            this.BMPatch.Controls.Add(this.WMMW64);
            this.BMPatch.Location = new System.Drawing.Point(12, 12);
            this.BMPatch.Name = "BMPatch";
            this.BMPatch.Size = new System.Drawing.Size(223, 71);
            this.BMPatch.TabIndex = 8;
            this.BMPatch.TabStop = false;
            this.BMPatch.Text = "For Black MIDIs";
            this.BMPatch.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.BMPatch_HelpRequested);
            // 
            // DAWPatch
            // 
            this.DAWPatch.Controls.Add(this.WMMD32);
            this.DAWPatch.Controls.Add(this.WMMD64);
            this.DAWPatch.Location = new System.Drawing.Point(12, 89);
            this.DAWPatch.Name = "DAWPatch";
            this.DAWPatch.Size = new System.Drawing.Size(223, 71);
            this.DAWPatch.TabIndex = 9;
            this.DAWPatch.TabStop = false;
            this.DAWPatch.Text = "For DAWs";
            this.DAWPatch.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.DAWPatch_HelpRequested);
            // 
            // WMMD32
            // 
            this.WMMD32.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WMMD32.Location = new System.Drawing.Point(4, 16);
            this.WMMD32.Name = "WMMD32";
            this.WMMD32.Size = new System.Drawing.Size(215, 24);
            this.WMMD32.TabIndex = 2;
            this.WMMD32.Text = "Patch a 32-bit app";
            this.WMMD32.UseVisualStyleBackColor = true;
            this.WMMD32.Click += new System.EventHandler(this.WMMD32_Click);
            this.WMMD32.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.DAWPatch_HelpRequested);
            // 
            // WMMD64
            // 
            this.WMMD64.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WMMD64.Location = new System.Drawing.Point(4, 42);
            this.WMMD64.Name = "WMMD64";
            this.WMMD64.Size = new System.Drawing.Size(215, 24);
            this.WMMD64.TabIndex = 3;
            this.WMMD64.Text = "Patch a 64-bit app";
            this.WMMD64.UseVisualStyleBackColor = true;
            this.WMMD64.Click += new System.EventHandler(this.WMMD64_Click);
            this.WMMD64.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.DAWPatch_HelpRequested);
            // 
            // WinMMPatches
            // 
            this.AcceptButton = this.OKClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(247, 228);
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
        private System.Windows.Forms.Button WMMW32;
        private System.Windows.Forms.Button WMMW64;
        private System.Windows.Forms.Button OKClose;
        private System.Windows.Forms.Button UnpatchApp;
        private System.Windows.Forms.Label PatchStatusLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox BMPatch;
        private System.Windows.Forms.GroupBox DAWPatch;
        private System.Windows.Forms.Button WMMD32;
        private System.Windows.Forms.Button WMMD64;
    }
}