namespace KeppySynthDebugWindow
{
    partial class KeppySynthDebugWindow
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
            try
            {
                base.Dispose(disposing);
            }
            catch
            {
                System.Windows.Forms.Application.Exit();
            }
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KeppySynthDebugWindow));
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.MainCont = new System.Windows.Forms.ContextMenu();
            this.OpenAppLocat = new System.Windows.Forms.MenuItem();
            this.CopyToClipboard = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.ExitMenu = new System.Windows.Forms.MenuItem();
            this.DebugWorker = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.richTextBox1.DetectUrls = false;
            this.richTextBox1.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.Location = new System.Drawing.Point(8, 9);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(405, 180);
            this.richTextBox1.TabIndex = 7;
            this.richTextBox1.Text = "Loading...";
            // 
            // MainCont
            // 
            this.MainCont.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.OpenAppLocat,
            this.CopyToClipboard,
            this.menuItem1,
            this.ExitMenu});
            // 
            // OpenAppLocat
            // 
            this.OpenAppLocat.Index = 0;
            this.OpenAppLocat.Text = "Open app location";
            this.OpenAppLocat.Click += new System.EventHandler(this.OpenAppLocat_Click);
            // 
            // CopyToClipboard
            // 
            this.CopyToClipboard.Index = 1;
            this.CopyToClipboard.Text = "Copy debug info to clipboard";
            this.CopyToClipboard.Click += new System.EventHandler(this.CopyToClipboard_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 2;
            this.menuItem1.Text = "-";
            // 
            // ExitMenu
            // 
            this.ExitMenu.Index = 3;
            this.ExitMenu.Text = "Exit";
            this.ExitMenu.Click += new System.EventHandler(this.Exit_Click);
            // 
            // DebugWorker
            // 
            this.DebugWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.DebugWorker_DoWork);
            // 
            // KeppySynthDebugWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(421, 198);
            this.Controls.Add(this.richTextBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "KeppySynthDebugWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Keppy\'s Synthesizer Debug Window";
            this.Load += new System.EventHandler(this.KeppySynthDebugWindow_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ContextMenu MainCont;
        private System.Windows.Forms.MenuItem OpenAppLocat;
        private System.Windows.Forms.MenuItem CopyToClipboard;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem ExitMenu;
        private System.ComponentModel.BackgroundWorker DebugWorker;
    }
}

