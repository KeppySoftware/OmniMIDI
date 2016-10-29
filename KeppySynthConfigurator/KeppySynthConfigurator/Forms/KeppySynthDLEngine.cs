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
    public partial class KeppySynthDLEngine : Form
    {
        WebClient webClient;
        String VersionToDownload;
        String FullURL;
        String thestring;
        Uri URL;
        int test;

        public KeppySynthDLEngine(String text, String MessageText, String toDL, int what)
        {
            InitializeComponent();
            thestring = MessageText;
            label1.Text = String.Format(thestring, 0);
            VersionToDownload = text;
            FullURL = toDL;
            test = what;
        }

        private void KeppySynthUpdateDL_Load(object sender, EventArgs e)
        {
            using (webClient = new WebClient())
            {
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);

                if (test == 0)
                {
                    URL = new Uri(String.Format("https://github.com/KaleidonKep99/Keppy-s-Synthesizer/releases/download/{0}/KeppysSynthSetup.exe", VersionToDownload));
                }
                else
                {
                    webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                    URL = new Uri(FullURL);
                }

                try
                {
                    if (test == 0)
                    {
                        webClient.DownloadFileAsync(URL, String.Format("{0}KeppySynthSetup.exe", Path.GetTempPath()));
                    }
                    else
                    {
                        string userfolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Keppy's Synthesizer";
                        webClient.DownloadFileAsync(URL, String.Format("{0}\\{1}", userfolder, FullURL.Split('/').Last()));
                    }
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
            label1.Text = String.Format(thestring, e.ProgressPercentage);
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            if (test == 0)
            {
                Process.Start(Path.GetTempPath() + "KeppySynthSetup.exe");
                Application.ExitThread();
            }
            else
            {
                Close();
            }
        }
    }
}
