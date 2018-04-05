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
using System.Media;

namespace KeppySynthConfigurator 
{
    public partial class UpdateYesNo : Form
    {
        public String ReturnVal { get; set; }
        String OnlineVersion;
        String CurrentVersion;
        UInt32 SoundToPlay = SoundEvent.SndOk;

        public UpdateYesNo(Version x, Version y, Boolean internetok, Boolean StartUp, Boolean IsItFromTheChangelogWindow)
        {
            try
            {
                InitializeComponent();
                if (StartUp)
                {
                    this.ShowInTaskbar = StartUp;
                }
                if (internetok)
                {
                    if (x == null || y == null)
                    {
                        CurrentIcon.Image = KeppySynthConfigurator.Properties.Resources.noupdateicon;
                        Text = "Keppy's Synthesizer - No updates found";
                        MessageText.Text = "No updates for Keppy's Synthesizer have been found.\nPlease try again later.\n\nPress OK to close this window.";
                        YesBtn.Visible = false;
                        ShowChangelog.Visible = false;
                        NoBtn.Text = "OK";
                        SoundToPlay = SoundEvent.SndInformation;
                        ReturnVal = "0.0.0.0";
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
                            ShowChangelog.Visible = false;
                            MessageText.Text = String.Format("Would you like to reinstall Keppy's Synthesizer?\nCurrent version online is {0}, the same as yours.\n\nPress Yes to confirm, or No to close the window.", CurrentVersion);
                            SoundToPlay = SoundEvent.SndQuestion;
                            ReturnVal = CurrentVersion;
                        }
                        else if (x < y)
                        {
                            CurrentIcon.Image = KeppySynthConfigurator.Properties.Resources.rollbackicon;
                            Text = String.Format("Keppy's Synthesizer - Downgrade to version {0}", OnlineVersion);
                            ShowChangelog.Visible = false;
                            MessageText.Text = String.Format("Are you sure you want to downgrade Keppy's Synthesizer?\nCurrent version online is {0}, you have {1}.\n\nPress Yes to confirm, or No to close the window.", OnlineVersion, CurrentVersion);
                            SoundToPlay = SoundEvent.SndWarning;
                            ReturnVal = OnlineVersion;
                        }
                        else
                        {
                            CurrentIcon.Image = KeppySynthConfigurator.Properties.Resources.updateicon;
                            Text = String.Format("Keppy's Synthesizer - Update found ({0})", OnlineVersion);
                            ShowChangelog.Visible = !IsItFromTheChangelogWindow;
                            MessageText.Text = String.Format("A new update for Keppy's Synthesizer has been found.\nCurrent version online is {0}, you have {1}.\n\nWould you like to update now?", OnlineVersion, CurrentVersion);
                            SoundToPlay = SoundEvent.SndQuestion;
                            ReturnVal = OnlineVersion;
                        }
                    }
                }
                else
                {
                    CurrentIcon.Image = KeppySynthConfigurator.Properties.Resources.erroricon;
                    Text = "Keppy's Synthesizer - Can not check for updates";
                    MessageText.Text = "The configurator can not connect to the GitHub servers.\nCheck your network connection, or contact your system administrator or network service provider.\n\nPress OK to close this window.";
                    YesBtn.Visible = false;
                    ShowChangelog.Visible = false;
                    NoBtn.Text = "OK";
                    SoundToPlay = SoundEvent.SndHand;
                    ReturnVal = "0.0.0.0";
                }
            }
            catch
            {
                CurrentIcon.Image = KeppySynthConfigurator.Properties.Resources.wi;
                Text = "Keppy's Synthesizer - Update error";
                MessageText.Text = "An unknown error has occurred while initializing the window.\nPlease try again later.\n\nPress OK to close this window.";
                YesBtn.Visible = false;
                ShowChangelog.Visible = false;
                NoBtn.Text = "OK";
                SoundToPlay = SoundEvent.SndHand;
                ReturnVal = "0.0.0.0";
            }
        }

        private void UpdateYesNo_Load(object sender, EventArgs e)
        {
            SoundEvent.MessageBeep(SoundToPlay);
        }

        private void YesBtn_Click(object sender, EventArgs e)
        {
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

        private void ShowChangelog_Click(object sender, EventArgs e)
        {
            try
            {
                SoundEvent.MessageBeep(SoundToPlay);
                ChangelogWindow frm = new ChangelogWindow(OnlineVersion, true);
                frm.ShowDialog(this);
                frm.Dispose();
            }
            catch { }
        }
    }
}
