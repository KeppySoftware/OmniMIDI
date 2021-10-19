
namespace OmniMIDIConfigurator
{
    partial class Drv32Troubleshooter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Drv32Troubleshooter));
            this.Drv32L = new System.Windows.Forms.ListView();
            this.MD = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DLL = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FixLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Drv64L = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CloseBtn = new System.Windows.Forms.Button();
            this.FixBtn = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // Drv32L
            // 
            this.Drv32L.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Drv32L.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.MD,
            this.DLL});
            this.Drv32L.GridLines = true;
            this.Drv32L.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.Drv32L.HideSelection = false;
            this.Drv32L.Location = new System.Drawing.Point(6, 20);
            this.Drv32L.Name = "Drv32L";
            this.Drv32L.Size = new System.Drawing.Size(244, 267);
            this.Drv32L.TabIndex = 0;
            this.Drv32L.UseCompatibleStateImageBehavior = false;
            this.Drv32L.View = System.Windows.Forms.View.Details;
            // 
            // MD
            // 
            this.MD.Text = "Device ID";
            this.MD.Width = 70;
            // 
            // DLL
            // 
            this.DLL.Text = "Device library";
            this.DLL.Width = 135;
            // 
            // FixLabel
            // 
            this.FixLabel.Location = new System.Drawing.Point(14, 10);
            this.FixLabel.Name = "FixLabel";
            this.FixLabel.Size = new System.Drawing.Size(521, 70);
            this.FixLabel.TabIndex = 1;
            this.FixLabel.Text = resources.GetString("FixLabel.Text");
            this.FixLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Drv32L);
            this.groupBox1.Location = new System.Drawing.Point(14, 87);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(257, 293);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Drivers32 (x86)";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Drv64L);
            this.groupBox2.Location = new System.Drawing.Point(278, 87);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(257, 293);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Drivers32 (x64)";
            // 
            // Drv64L
            // 
            this.Drv64L.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Drv64L.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.Drv64L.GridLines = true;
            this.Drv64L.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.Drv64L.HideSelection = false;
            this.Drv64L.Location = new System.Drawing.Point(6, 20);
            this.Drv64L.Name = "Drv64L";
            this.Drv64L.Size = new System.Drawing.Size(244, 267);
            this.Drv64L.TabIndex = 0;
            this.Drv64L.UseCompatibleStateImageBehavior = false;
            this.Drv64L.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Device ID";
            this.columnHeader1.Width = 70;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Device library";
            this.columnHeader2.Width = 135;
            // 
            // CloseBtn
            // 
            this.CloseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseBtn.Location = new System.Drawing.Point(448, 395);
            this.CloseBtn.Name = "CloseBtn";
            this.CloseBtn.Size = new System.Drawing.Size(87, 27);
            this.CloseBtn.TabIndex = 4;
            this.CloseBtn.Text = "Close";
            this.CloseBtn.UseVisualStyleBackColor = true;
            this.CloseBtn.Click += new System.EventHandler(this.CloseBtn_Click);
            // 
            // FixBtn
            // 
            this.FixBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.FixBtn.Location = new System.Drawing.Point(353, 395);
            this.FixBtn.Name = "FixBtn";
            this.FixBtn.Size = new System.Drawing.Size(87, 27);
            this.FixBtn.TabIndex = 5;
            this.FixBtn.Text = "Fix";
            this.FixBtn.UseVisualStyleBackColor = true;
            this.FixBtn.Click += new System.EventHandler(this.FixBtn_Click);
            // 
            // Drv32Troubleshooter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 435);
            this.ControlBox = false;
            this.Controls.Add(this.FixBtn);
            this.Controls.Add(this.CloseBtn);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.FixLabel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Drv32Troubleshooter";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OmniMIDI Registration Troubleshooter";
            this.Load += new System.EventHandler(this.Drv32Troubleshooter_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView Drv32L;
        private System.Windows.Forms.ColumnHeader MD;
        private System.Windows.Forms.ColumnHeader DLL;
        private System.Windows.Forms.Label FixLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView Drv64L;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button CloseBtn;
        private System.Windows.Forms.Button FixBtn;
    }
}