using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace KeppySynthConfigurator.Forms
{
    public partial class KeppySynthUpdateDL : Form
    {
        WebClient webClient;
        String VersionToDownload;

        public KeppySynthUpdateDL(String text)
        {
            InitializeComponent();
            VersionToDownload = text;
        }

        private void KeppySynthUpdateDL_Load(object sender, EventArgs e)
        {
            using (webClient = new WebClient())
            {
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);

                Uri URL = new Uri(String.Format("https://github.com/KaleidonKep99/Keppy-s-Synthesizer/releases/download/{0}/KeppysSynthSetup.exe", VersionToDownload));

                try
                {
                    webClient.DownloadFileAsync(URL, Path.GetTempPath() + "KeppySynthSetup.exe");
                }
                catch
                {
                    MessageBox.Show("The configurator can not connect to the GitHub servers.\n\nCheck your network connection, or contact your system administrator or network service provider.", "Keppy's Synthesizer - Connection error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            Process.Start(Path.GetTempPath() + "KeppySynthSetup.exe");
            Application.ExitThread();
        }
    }
}
