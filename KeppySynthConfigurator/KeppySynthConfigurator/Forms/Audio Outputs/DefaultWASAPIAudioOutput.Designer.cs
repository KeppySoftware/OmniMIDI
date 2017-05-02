namespace KeppySynthConfigurator
{
    partial class DefaultWASAPIAudioOutput
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
            this.Quit = new System.Windows.Forms.Button();
            this.DefOut = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.DevicesList = new System.Windows.Forms.ComboBox();
            this.ExAccess = new System.Windows.Forms.CheckBox();
            this.WASAPIExInfo = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // Quit
            // 
            this.Quit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Quit.Location = new System.Drawing.Point(479, 64);
            this.Quit.Name = "Quit";
            this.Quit.Size = new System.Drawing.Size(75, 23);
            this.Quit.TabIndex = 7;
            this.Quit.Text = "OK";
            this.Quit.UseVisualStyleBackColor = true;
            this.Quit.Click += new System.EventHandler(this.Quit_Click);
            // 
            // DefOut
            // 
            this.DefOut.AutoSize = true;
            this.DefOut.Location = new System.Drawing.Point(12, 41);
            this.DefOut.Name = "DefOut";
            this.DefOut.Size = new System.Drawing.Size(149, 13);
            this.DefOut.TabIndex = 6;
            this.DefOut.Text = "Default Windows output: NaN";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Current output:";
            // 
            // DevicesList
            // 
            this.DevicesList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DevicesList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DevicesList.FormattingEnabled = true;
            this.DevicesList.Location = new System.Drawing.Point(95, 11);
            this.DevicesList.Name = "DevicesList";
            this.DevicesList.Size = new System.Drawing.Size(458, 21);
            this.DevicesList.TabIndex = 4;
            this.DevicesList.SelectedIndexChanged += new System.EventHandler(this.DevicesList_SelectedIndexChanged);
            // 
            // ExAccess
            // 
            this.ExAccess.AutoSize = true;
            this.ExAccess.Location = new System.Drawing.Point(15, 68);
            this.ExAccess.Name = "ExAccess";
            this.ExAccess.Size = new System.Drawing.Size(192, 17);
            this.ExAccess.TabIndex = 8;
            this.ExAccess.Text = "Get exclusive access to the device";
            this.WASAPIExInfo.SetToolTip(this.ExAccess, "The buffer size will not affect WASAPI, when in exclusive mode.\\nChanging it is u" +
        "seless.");
            this.ExAccess.UseVisualStyleBackColor = true;
            this.ExAccess.CheckedChanged += new System.EventHandler(this.ExAccess_CheckedChanged);
            // 
            // WASAPIExInfo
            // 
            this.WASAPIExInfo.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.WASAPIExInfo.ToolTipTitle = "About the WASAPI exclusive mode";
            // 
            // DefaultWASAPIAudioOutput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 98);
            this.Controls.Add(this.ExAccess);
            this.Controls.Add(this.Quit);
            this.Controls.Add(this.DefOut);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DevicesList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DefaultWASAPIAudioOutput";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Change default WASAPI audio output";
            this.Load += new System.EventHandler(this.DefaultWASAPIAudioOutput_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Quit;
        private System.Windows.Forms.Label DefOut;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox DevicesList;
        private System.Windows.Forms.CheckBox ExAccess;
        private System.Windows.Forms.ToolTip WASAPIExInfo;
    }
}