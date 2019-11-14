namespace OmniMIDIConfigurator
{
    partial class NoAvailableControl
    {
        /// <summary> 
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione componenti

        /// <summary> 
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare 
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.Err = new System.Windows.Forms.Label();
            this.Desc = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Err
            // 
            this.Err.Dock = System.Windows.Forms.DockStyle.Top;
            this.Err.Font = new System.Drawing.Font("Arial", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Err.ForeColor = System.Drawing.Color.Red;
            this.Err.Location = new System.Drawing.Point(0, 0);
            this.Err.Name = "Err";
            this.Err.Size = new System.Drawing.Size(581, 97);
            this.Err.TabIndex = 0;
            this.Err.Text = "ERROR";
            this.Err.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Desc
            // 
            this.Desc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Desc.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Desc.ForeColor = System.Drawing.Color.Black;
            this.Desc.Location = new System.Drawing.Point(0, 97);
            this.Desc.Margin = new System.Windows.Forms.Padding(3);
            this.Desc.Name = "Desc";
            this.Desc.Size = new System.Drawing.Size(581, 342);
            this.Desc.TabIndex = 1;
            this.Desc.Text = "ERROR";
            // 
            // NoAvailableControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Desc);
            this.Controls.Add(this.Err);
            this.Name = "NoAvailableControl";
            this.Size = new System.Drawing.Size(581, 439);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label Err;
        private System.Windows.Forms.Label Desc;
    }
}
