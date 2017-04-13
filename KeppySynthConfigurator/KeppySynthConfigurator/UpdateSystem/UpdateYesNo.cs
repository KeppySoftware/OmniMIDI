using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace KeppySynthConfigurator 
{
    public partial class UpdateYesNo : Form
    {
        String OnlineVersion;
        String CurrentVersion;

        public UpdateYesNo(Version x, Version y, Boolean internetok, Boolean StartUp)
        {
            try
            {
                InitializeComponent();
                if (StartUp)
                {
                    this.ShowInTaskbar = StartUp;
                }
                if (UpdateSystem.IsInternetAvailable())
                {
                    if (x == null || y == null)
                    {
                        CurrentIcon.Image = KeppySynthConfigurator.Properties.Resources.noupdateicon;
                        Text = "Keppy's Synthesizer - No updates found";
                        MessageText.Text = "No updates for Keppy's Synthesizer have been found.\nPlease try again later.\n\nPress OK to close this window.";
                        YesBtn.Visible = false;
                        ShowChangelogCheck.Visible = false;
                        NoBtn.Text = "OK";
                    }
                    else
                    {
                        FileVersionInfo Driver = FileVersionInfo.GetVersionInfo(Environment.SystemDirectory + "\\keppysynth\\keppysynth.dll");
                        OnlineVersion = x.ToString();
                        CurrentVersion = y.ToString();
                        if (x == y)
                        {
                            CurrentIcon.Image = KeppySynthConfigurator.Properties.Resources.refreshicon;
                            Text = String.Format("Keppy's Synthesizer - Reinstall version ({0})", OnlineVersion);
                            MessageText.Text = String.Format("Would you like to reinstall Keppy's Synthesizer?\nCurrent version online is {0}, the same as yours.\n\nPress Yes to confirm, or No to close the window.", CurrentVersion);
                        }
                        else if (x < y)
                        {
                            CurrentIcon.Image = KeppySynthConfigurator.Properties.Resources.rollbackicon;
                            Text = String.Format("Keppy's Synthesizer - Downgrade to version {0}", OnlineVersion);
                            MessageText.Text = String.Format("Are you sure you want to downgrade Keppy's Synthesizer?\nCurrent version online is {0}, you have {1}.\n\nPress Yes to confirm, or No to close the window.", OnlineVersion, CurrentVersion);
                        }
                        else
                        {
                            CurrentIcon.Image = KeppySynthConfigurator.Properties.Resources.updateicon;
                            Text = String.Format("Keppy's Synthesizer - Update found ({0})", OnlineVersion);
                            MessageText.Text = String.Format("A new update for Keppy's Synthesizer has been found.\nCurrent version online is {0}, you have {1}.\n\nWould you like to update now?", OnlineVersion, CurrentVersion);
                        }
                    }
                }
                else
                {
                    CurrentIcon.Image = KeppySynthConfigurator.Properties.Resources.erroricon;
                    Text = "Keppy's Synthesizer - Can not check for updates";
                    MessageText.Text = "The configurator can not connect to the GitHub servers.\nCheck your network connection, or contact your system administrator or network service provider.\n\nPress OK to close this window.";
                    YesBtn.Visible = false;
                    ShowChangelogCheck.Visible = false;
                    NoBtn.Text = "OK";
                }
            }
            catch
            {
                CurrentIcon.Image = KeppySynthConfigurator.Properties.Resources.wi;
                Text = "Keppy's Synthesizer - Update error";
                MessageText.Text = "An unknown error has occurred while initializing the window.\nPlease try again later.\n\nPress OK to close this window.";
                YesBtn.Visible = false;
                ShowChangelogCheck.Visible = false;
                NoBtn.Text = "OK";
            }
        }

        private void UpdateYesNo_Load(object sender, EventArgs e)
        {
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void YesBtn_Click(object sender, EventArgs e)
        {
            if (ShowChangelogCheck.Checked == true)
            {
                Process.Start(String.Format("https://github.com/KaleidonKep99/Keppy-s-Synthesizer/releases/tag/{0}", OnlineVersion));
            }
            DialogResult = DialogResult.Yes;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void NoBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
