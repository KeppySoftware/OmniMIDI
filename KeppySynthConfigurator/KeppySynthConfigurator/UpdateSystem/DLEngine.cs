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
    public partial class DLEngine : Form
    {
        WebClient webClient;
        String VersionToDownload;
        String FullURL;
        String thestring;
        String PutWhereHere;
        String DeleteThisIfFailed;
        Uri URL;
        int test;
        bool reinstallbool;

        public DLEngine(String text, String MessageText, String toDL, String PutWhere, int what, bool reinstall)
        {
            InitializeComponent();
            thestring = MessageText;
            Status.Text = String.Format(thestring, 0);
            VersionToDownload = text;
            PutWhereHere = PutWhere;
            FullURL = toDL;
            test = what;
            reinstallbool = reinstall;
        }

        private void KeppySynthUpdateDL_Load(object sender, EventArgs e)
        {
            using (webClient = new WebClient())
            {
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);

                if (test == 0)
                {
                    if (reinstallbool)
                    {
                        CancelBtn.Visible = false;
                        progressBar1.Size = new Size(271, 23);
                    }
                    URL = new Uri(String.Format("https://github.com/KaleidonKep99/Keppy-s-Synthesizer/releases/download/{0}/KeppysSynthUpdate.exe", VersionToDownload));
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
                        webClient.DownloadFileAsync(URL, String.Format("{0}KeppySynthUpdate.exe", Path.GetTempPath()));
                    }
                    else if (test == 1)
                    {
                        string userfolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Keppy's Synthesizer";
                        DeleteThisIfFailed = String.Format("{0}\\{1}", userfolder, FullURL.Split('/').Last());
                        webClient.DownloadFileAsync(URL, DeleteThisIfFailed);
                    }
                    else
                    {
                        DeleteThisIfFailed = String.Format("{0}\\{1}", PutWhereHere, FullURL.Split('/').Last());
                        webClient.DownloadFileAsync(URL, DeleteThisIfFailed);
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            Status.Text = thestring;
            DLPercent.Text = String.Format("{0}%", e.ProgressPercentage);
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                webClient.Dispose();
                if (File.Exists(Path.GetTempPath() + "KeppySynthUpdate.exe")) { File.Delete(Path.GetTempPath() + "KeppySynthUpdate.exe"); }
                MessageBox.Show("Download aborted.\n\nPress OK to close the download window.", "Keppy's Synthesizer - Download aborted", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.No;
                Close();
            }
            else if (e.Error != null)
            {
                MessageBox.Show("The configurator is unable to download the latest version.\nIt might not be available yet, or you might not be connected to the Internet.\n\nIf your connection is working, wait a few minutes for the update to appear online.\nIf your connection is malfunctioning or is not working at all, check your network connection, or contact your system administrator or network service provider.", "Keppy's Synthesizer - Connection error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                try { File.Delete(DeleteThisIfFailed); } catch { }
                DialogResult = DialogResult.No;
                Close();
            }
            else
            {
                if (test == 0)
                {
                    try
                    {
                        Process.Start(Path.GetTempPath() + "KeppySynthUpdate.exe");
                        DialogResult = DialogResult.OK;
                        Application.ExitThread();
                    }
                    catch (Exception ex)
                    {
                        if (ex.GetBaseException() is InvalidOperationException)
                            MessageBox.Show("Unable to locate the setup!\n\nThe file is missing from your storage or it's not even present in GitHub's servers.\n\nThis usually indicates an issue with your connection, or a problem at GitHub.", "Keppy's Synthesizer - Update error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else if (ex.GetBaseException() is Win32Exception)
                            MessageBox.Show("Can not open the setup!\n\nThe file is probably damaged, or its missing the Win32PE header.\n\nThis usually indicates an issue with your connection.", "Keppy's Synthesizer - Update error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        else if (ex.GetBaseException() is ObjectDisposedException)
                            MessageBox.Show("The process object has already been disposed.", "Keppy's Synthesizer - Update error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        DialogResult = DialogResult.No;

                        Close();
                    }
                }
                else
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            webClient.CancelAsync();
        }
    }
}
