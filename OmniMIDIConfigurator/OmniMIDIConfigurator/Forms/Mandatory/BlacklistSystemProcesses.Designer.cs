namespace OmniMIDIConfigurator
{
    partial class BlacklistSystemProcesses
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
            this.OKDone = new System.Windows.Forms.Button();
            this.RunningProcessesList = new System.Windows.Forms.ListBox();
            this.Cancel = new System.Windows.Forms.Button();
            this.RefreshList = new System.Windows.Forms.PictureBox();
            this.RefrLab = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.RefreshList)).BeginInit();
            this.SuspendLayout();
            // 
            // OKDone
            // 
            this.OKDone.Location = new System.Drawing.Point(218, 260);
            this.OKDone.Name = "OKDone";
            this.OKDone.Size = new System.Drawing.Size(137, 23);
            this.OKDone.TabIndex = 0;
            this.OKDone.Text = "Blacklist these processes";
            this.OKDone.UseVisualStyleBackColor = true;
            this.OKDone.Click += new System.EventHandler(this.OKDone_Click);
            // 
            // RunningProcessesList
            // 
            this.RunningProcessesList.FormattingEnabled = true;
            this.RunningProcessesList.Location = new System.Drawing.Point(12, 12);
            this.RunningProcessesList.Name = "RunningProcessesList";
            this.RunningProcessesList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.RunningProcessesList.Size = new System.Drawing.Size(343, 238);
            this.RunningProcessesList.TabIndex = 1;
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(147, 260);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(65, 23);
            this.Cancel.TabIndex = 2;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // RefreshList
            // 
            this.RefreshList.Image = global::OmniMIDIConfigurator.Properties.Resources.ReloadIcon;
            this.RefreshList.Location = new System.Drawing.Point(12, 260);
            this.RefreshList.Name = "RefreshList";
            this.RefreshList.Size = new System.Drawing.Size(23, 23);
            this.RefreshList.TabIndex = 3;
            this.RefreshList.TabStop = false;
            this.RefreshList.Click += new System.EventHandler(this.RefreshList_Click);
            // 
            // RefrLab
            // 
            this.RefrLab.AutoSize = true;
            this.RefrLab.Location = new System.Drawing.Point(41, 265);
            this.RefrLab.Name = "RefrLab";
            this.RefrLab.Size = new System.Drawing.Size(67, 13);
            this.RefrLab.TabIndex = 4;
            this.RefrLab.Text = "Refreshing...";
            this.RefrLab.Visible = false;
            // 
            // BlacklistSystemProcesses
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(367, 295);
            this.ControlBox = false;
            this.Controls.Add(this.RefrLab);
            this.Controls.Add(this.RefreshList);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.RunningProcessesList);
            this.Controls.Add(this.OKDone);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BlacklistSystemProcesses";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select the processes to ban";
            this.Load += new System.EventHandler(this.BlacklistSystemProcesses_Load);
            ((System.ComponentModel.ISupportInitialize)(this.RefreshList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OKDone;
        private System.Windows.Forms.ListBox RunningProcessesList;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.PictureBox RefreshList;
        private System.Windows.Forms.Label RefrLab;
    }
}