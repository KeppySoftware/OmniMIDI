namespace KeppyDriverConfigurator
{
    partial class KeppyDriverDebugWindow
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
            this.label1 = new System.Windows.Forms.Label();
            this.Voices = new System.Windows.Forms.Label();
            this.DebugRefresh = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.CPU = new System.Windows.Forms.Label();
            this.DecodedInt = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 31);
            this.label1.TabIndex = 0;
            this.label1.Text = "Voices:";
            this.label1.UseCompatibleTextRendering = true;
            // 
            // Voices
            // 
            this.Voices.AutoSize = true;
            this.Voices.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Voices.Location = new System.Drawing.Point(81, 9);
            this.Voices.Name = "Voices";
            this.Voices.Size = new System.Drawing.Size(99, 31);
            this.Voices.TabIndex = 1;
            this.Voices.Text = "0000/0000";
            this.Voices.UseCompatibleTextRendering = true;
            // 
            // DebugRefresh
            // 
            this.DebugRefresh.Enabled = true;
            this.DebugRefresh.Interval = 1;
            this.DebugRefresh.Tick += new System.EventHandler(this.DebugRefresh_Tick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 31);
            this.label2.TabIndex = 2;
            this.label2.Text = "CPU usage*:";
            this.label2.UseCompatibleTextRendering = true;
            // 
            // CPU
            // 
            this.CPU.AutoSize = true;
            this.CPU.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CPU.Location = new System.Drawing.Point(125, 34);
            this.CPU.Name = "CPU";
            this.CPU.Size = new System.Drawing.Size(54, 31);
            this.CPU.TabIndex = 3;
            this.CPU.Text = "000%";
            this.CPU.UseCompatibleTextRendering = true;
            // 
            // DecodedInt
            // 
            this.DecodedInt.Location = new System.Drawing.Point(12, 178);
            this.DecodedInt.Name = "DecodedInt";
            this.DecodedInt.Size = new System.Drawing.Size(268, 14);
            this.DecodedInt.TabIndex = 4;
            this.DecodedInt.Text = "Decoded data size: 0 frames (Int32 value)";
            this.DecodedInt.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(268, 39);
            this.label3.TabIndex = 5;
            this.label3.Text = "* = That value doesn\'t reflect the real CPU usage, \r\nit shows the number of cycle" +
    "s BASS needs to complete\r\nthe rendering.";
            // 
            // KeppyDriverDebugWindow
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(289, 201);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.DecodedInt);
            this.Controls.Add(this.CPU);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Voices);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KeppyDriverDebugWindow";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Debug window";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Voices;
        private System.Windows.Forms.Timer DebugRefresh;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label CPU;
        private System.Windows.Forms.Label DecodedInt;
        private System.Windows.Forms.Label label3;
    }
}