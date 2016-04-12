namespace KeppyDriverConfigurator
{
    partial class KeppyDriverMIDIOutSelectorWin
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
            this.lvwOutputs = new System.Windows.Forms.ListView();
            this.clmName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblOutputDevices = new System.Windows.Forms.Label();
            this.btnSetAsDefault = new System.Windows.Forms.Button();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lvwOutputs
            // 
            this.lvwOutputs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmName,
            this.clmID});
            this.lvwOutputs.Enabled = false;
            this.lvwOutputs.FullRowSelect = true;
            this.lvwOutputs.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvwOutputs.HideSelection = false;
            this.lvwOutputs.Location = new System.Drawing.Point(12, 27);
            this.lvwOutputs.MultiSelect = false;
            this.lvwOutputs.Name = "lvwOutputs";
            this.lvwOutputs.Size = new System.Drawing.Size(299, 131);
            this.lvwOutputs.TabIndex = 5;
            this.lvwOutputs.UseCompatibleStateImageBehavior = false;
            this.lvwOutputs.View = System.Windows.Forms.View.Details;
            // 
            // clmName
            // 
            this.clmName.Text = "Name";
            this.clmName.Width = 235;
            // 
            // clmID
            // 
            this.clmID.Text = "ID";
            this.clmID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblOutputDevices
            // 
            this.lblOutputDevices.AutoSize = true;
            this.lblOutputDevices.Enabled = false;
            this.lblOutputDevices.Location = new System.Drawing.Point(12, 11);
            this.lblOutputDevices.Name = "lblOutputDevices";
            this.lblOutputDevices.Size = new System.Drawing.Size(107, 13);
            this.lblOutputDevices.TabIndex = 4;
            this.lblOutputDevices.Text = "&MIDI Output Devices";
            // 
            // btnSetAsDefault
            // 
            this.btnSetAsDefault.Enabled = false;
            this.btnSetAsDefault.Location = new System.Drawing.Point(12, 164);
            this.btnSetAsDefault.Name = "btnSetAsDefault";
            this.btnSetAsDefault.Size = new System.Drawing.Size(299, 23);
            this.btnSetAsDefault.TabIndex = 6;
            this.btnSetAsDefault.Text = "&Set Selected Device As Default";
            this.btnSetAsDefault.UseVisualStyleBackColor = true;
            this.btnSetAsDefault.Click += new System.EventHandler(this.btnSetAsDefault_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.Black;
            this.lblStatus.Location = new System.Drawing.Point(12, 194);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(299, 22);
            this.lblStatus.TabIndex = 7;
            this.lblStatus.Text = "Loading devices and reading registry... please wait!";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KeppyDriverMIDIOutSelectorWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 229);
            this.Controls.Add(this.lvwOutputs);
            this.Controls.Add(this.lblOutputDevices);
            this.Controls.Add(this.btnSetAsDefault);
            this.Controls.Add(this.lblStatus);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KeppyDriverMIDIOutSelectorWin";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Change default MIDI out device";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvwOutputs;
        private System.Windows.Forms.ColumnHeader clmName;
        private System.Windows.Forms.ColumnHeader clmID;
        private System.Windows.Forms.Label lblOutputDevices;
        private System.Windows.Forms.Button btnSetAsDefault;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.Label lblStatus;
    }
}