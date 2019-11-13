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
using Microsoft.Win32;

namespace OmniMIDIConfigurator.Forms
{
    public partial class DLEngine : Form
    {
        WebClient DLSystem;
        System.Timers.Timer DLSystemTimeout = new System.Timers.Timer(5000);
        String VersionToDownload;
        String FullURL;
        String MessageToDisplay;
        String DestinationPath;
        String DeleteThisIfFailed;
        Uri URL;
        int InstallMode;
        bool DriverReinstall;

        string UserFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\OmniMIDI";

        public DLEngine(String Version, String MessageText, String toDL, String PutWhere, int IM)
        {
            InitializeComponent();
            MessageToDisplay = MessageText;
            Status.Text = String.Format(Version, 0);
            VersionToDownload = Version;
            DestinationPath = PutWhere;
            FullURL = toDL;
            InstallMode = IM;

            DriverReinstall = (InstallMode == UpdateSystem.WIPE_SETTINGS);
        }

        private void OmniMIDIUpdateDL_Load(object sender, EventArgs e)
        {
            using (DLSystem = new WebClient())
            {
                DLSystem.DownloadProgressChanged += (senderp, ep) => ProgressChanged(senderp, ep);
                DLSystem.DownloadDataCompleted += (senderd, ed) => Completed(senderd, ed);
                DLSystem.Proxy = null;

                if (InstallMode == UpdateSystem.NORMAL || InstallMode == UpdateSystem.WIPE_SETTINGS)
                {
                    if (DriverReinstall)
                    {
                        CancelBtn.Visible = false;
                        progressBar1.Size = new Size(271, 23);
                    }
                    URL = new Uri(String.Format(DriverReinstall ? UpdateSystem.SetupFile : UpdateSystem.UpdateFile, VersionToDownload));
                }
                else URL = new Uri(FullURL);

                try
                {
                    Status.Text = String.Format(MessageToDisplay, 0);
                    DLSystemTimeout.Elapsed += TimeOutCheck;
                    DLSystem.DownloadDataAsync(URL);
                    DLSystemTimeout.Start();
                }
                catch { }
            }
        }

        private void ResetTimeout()
        {
            DLSystemTimeout.Stop();
            DLSystemTimeout.Start();
        }

        private void TimeOutCheck(Object source, System.Timers.ElapsedEventArgs e)
        {
            DLSystemTimeout.Elapsed -= TimeOutCheck;
            DLSystem.CancelAsync();
            DLSystem.Dispose();
            MessageBox.Show("The download process has timed out.\nYour Internet may be slow, or GitHub may be experiencing high server load.\n\nIf your connection is working, wait a few minutes before updating.\nIf your connection is malfunctioning or is not working at all, check your network connection, or contact your system administrator or network service provider.", "OmniMIDI - Timed out", MessageBoxButtons.OK, MessageBoxIcon.Warning, DriverReinstall ? MessageBoxDefaultButton.Button1 : 0, DriverReinstall ? MessageBoxOptions.DefaultDesktopOnly : 0);
            DialogResult = DialogResult.No;
            Close();
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            ResetTimeout();
            progressBar1.Value = e.ProgressPercentage;
            Status.Text = String.Format(MessageToDisplay, 0);
            DLPercent.Text = String.Format("{0}%", e.ProgressPercentage);
        }

        private void Completed(object sender, DownloadDataCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                DLSystem.Dispose();
                MessageBox.Show("Download aborted.\n\nPress OK to close the download window.", "OmniMIDI - Download aborted", MessageBoxButtons.OK, MessageBoxIcon.Warning, DriverReinstall ? MessageBoxDefaultButton.Button1 : 0, DriverReinstall ? MessageBoxOptions.DefaultDesktopOnly : 0);
                DialogResult = DialogResult.No;
                Close();
            }
            else if (e.Error != null)
            {
                DLSystem.Dispose();
                MessageBox.Show("The configurator is unable to download the latest version.\nIt might not be available yet, or you might not be connected to the Internet.\n\nIf your connection is working, wait a few minutes for the update to appear online.\nIf your connection is malfunctioning or is not working at all, check your network connection, or contact your system administrator or network service provider.", "OmniMIDI - Connection error", MessageBoxButtons.OK, MessageBoxIcon.Warning, DriverReinstall ? MessageBoxDefaultButton.Button1 : 0, DriverReinstall ? MessageBoxOptions.DefaultDesktopOnly : 0);
                DialogResult = DialogResult.No;

                if (DriverReinstall && InstallMode == UpdateSystem.WIPE_SETTINGS)
                {
                    var p = new System.Diagnostics.Process();
                    p.StartInfo.FileName = System.Reflection.Assembly.GetEntryAssembly().Location;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.CreateNoWindow = true;
                    p.Start();
                    Application.ExitThread();
                }

                Close();
            }
            else
            {
                if (InstallMode == UpdateSystem.NORMAL || InstallMode == UpdateSystem.WIPE_SETTINGS)
                {
                    try
                    {
                        byte[] fileData = e.Result;

                        using (FileStream fileStream = new FileStream(String.Format(DriverReinstall ? "{0}OmniMIDISetup.exe" : "{0}OmniMIDIUpdate.exe", Path.GetTempPath()), FileMode.Create))
                            fileStream.Write(fileData, 0, fileData.Length);

                        DLSystem.Dispose();

                        MessageBox.Show("Be sure to save all your data in the apps using OmniMIDI, before updating.\n\nClick OK when you're ready.", "OmniMIDI - Update warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, DriverReinstall ? MessageBoxDefaultButton.Button1 : 0, DriverReinstall ? MessageBoxOptions.DefaultDesktopOnly : 0);
                        Process.Start(String.Format("{0}{1}", Path.GetTempPath() , DriverReinstall ? "OmniMIDISetup.exe" : "OmniMIDIUpdate.exe"), "/SILENT /CLOSEAPPLICATIONS /RESTARTAPPLICATIONS /NOCANCEL /SP-");

                        if (DriverReinstall && InstallMode == UpdateSystem.WIPE_SETTINGS)
                        {
                            RegistryKey sourceKey = Registry.CurrentUser.OpenSubKey("SOFTWARE", true);
                            sourceKey.DeleteSubKeyTree("OmniMIDI", true);
                            sourceKey.Close();
                        }

                        DialogResult = DialogResult.OK;
                        Application.ExitThread();
                    }
                    catch (Exception ex)
                    {
                        if (ex.GetBaseException() is InvalidOperationException)
                            MessageBox.Show("Unable to locate the setup!\n\nThe file is missing from your storage or it's not even present in GitHub's servers.\n\nThis usually indicates an issue with your connection, or a problem at GitHub.", "OmniMIDI - Update error", MessageBoxButtons.OK, MessageBoxIcon.Error, DriverReinstall ? MessageBoxDefaultButton.Button1 : 0, DriverReinstall ? MessageBoxOptions.DefaultDesktopOnly : 0);
                        else if (ex.GetBaseException() is Win32Exception)
                            MessageBox.Show("Can not open the setup!\n\nThe file is probably damaged, or its missing the Win32PE header.\n\nThis usually indicates an issue with your connection.", "OmniMIDI - Update error", MessageBoxButtons.OK, MessageBoxIcon.Error, DriverReinstall ? MessageBoxDefaultButton.Button1 : 0, DriverReinstall ? MessageBoxOptions.DefaultDesktopOnly : 0);
                        else if (ex.GetBaseException() is ObjectDisposedException)
                            MessageBox.Show("The process object has already been disposed.", "OmniMIDI - Update error", MessageBoxButtons.OK, MessageBoxIcon.Error, DriverReinstall ? MessageBoxDefaultButton.Button1 : 0, DriverReinstall ? MessageBoxOptions.DefaultDesktopOnly : 0);
                        else
                            MessageBox.Show("Unknown error.", "OmniMIDI - Update error", MessageBoxButtons.OK, MessageBoxIcon.Error, DriverReinstall ? MessageBoxDefaultButton.Button1 : 0, DriverReinstall ? MessageBoxOptions.DefaultDesktopOnly : 0);

                        DialogResult = DialogResult.No;
                        Close();
                    }
                }
                else if (InstallMode == UpdateSystem.USERFOLDER_PATH)
                {
                    byte[] fileData = e.Result;

                    using (FileStream fileStream = new FileStream(String.Format("{0}\\{1}", UserFolder, FullURL.Split('/').Last()), FileMode.Create))
                        fileStream.Write(fileData, 0, fileData.Length);

                    DLSystem.Dispose();

                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    byte[] fileData = e.Result;

                    using (FileStream fileStream = new FileStream(String.Format("{0}\\{1}", DestinationPath, FullURL.Split('/').Last()), FileMode.Create))
                        fileStream.Write(fileData, 0, fileData.Length);

                    DLSystem.Dispose();

                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            DLSystem.CancelAsync();
        }
    }
}