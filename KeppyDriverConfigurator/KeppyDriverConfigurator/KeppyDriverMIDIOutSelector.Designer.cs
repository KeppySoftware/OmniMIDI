namespace MIDIOutSetter
{
    partial class FormMain
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
            this.lblOutputDevices = new System.Windows.Forms.Label();
            this.lvwOutputs = new System.Windows.Forms.ListView();
            this.clmName = new System.Windows.Forms.ColumnHeader();
            this.clmID = new System.Windows.Forms.ColumnHeader();
            this.btnSetAsDefault = new System.Windows.Forms.Button();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblOutputDevices
            // 
            this.lblOutputDevices.AutoSize = true;
            this.lblOutputDevices.Enabled = false;
            this.lblOutputDevices.Location = new System.Drawing.Point(12, 9);
            this.lblOutputDevices.Name = "lblOutputDevices";
            this.lblOutputDevices.Size = new System.Drawing.Size(107, 13);
            this.lblOutputDevices.TabIndex = 0;
            this.lblOutputDevices.Text = "&MIDI Output Devices";
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
            this.lvwOutputs.Location = new System.Drawing.Point(12, 25);
            this.lvwOutputs.MultiSelect = false;
            this.lvwOutputs.Name = "lvwOutputs";
            this.lvwOutputs.Size = new System.Drawing.Size(282, 131);
            this.lvwOutputs.TabIndex = 1;
            this.lvwOutputs.UseCompatibleStateImageBehavior = false;
            this.lvwOutputs.View = System.Windows.Forms.View.Details;
            // 
            // clmName
            // 
            this.clmName.Text = "Name";
            this.clmName.Width = 199;
            // 
            // clmID
            // 
            this.clmID.Text = "ID";
            this.clmID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnSetAsDefault
            // 
            this.btnSetAsDefault.Enabled = false;
            this.btnSetAsDefault.Location = new System.Drawing.Point(12, 162);
            this.btnSetAsDefault.Name = "btnSetAsDefault";
            this.btnSetAsDefault.Size = new System.Drawing.Size(282, 23);
            this.btnSetAsDefault.TabIndex = 2;
            this.btnSetAsDefault.Text = "&Set Selected Device As Default";
            this.btnSetAsDefault.UseVisualStyleBackColor = true;
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.SystemColors.Info;
            this.lblStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblStatus.Location = new System.Drawing.Point(4, 77);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(299, 42);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "Loading devices and reading registry... please wait!";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormMain
            // 
            this.AcceptButton = this.btnSetAsDefault;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 197);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnSetAsDefault);
            this.Controls.Add(this.lvwOutputs);
            this.Controls.Add(this.lblOutputDevices);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Vista and Weven MIDI Out Setter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblOutputDevices;
        private System.Windows.Forms.ListView lvwOutputs;
        private System.Windows.Forms.ColumnHeader clmName;
        private System.Windows.Forms.ColumnHeader clmID;
        private System.Windows.Forms.Button btnSetAsDefault;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.Label lblStatus;
    }
}

