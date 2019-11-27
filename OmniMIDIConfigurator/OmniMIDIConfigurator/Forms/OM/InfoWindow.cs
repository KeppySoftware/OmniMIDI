using Microsoft.VisualBasic.Devices;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OmniMIDIConfigurator
{
    public partial class InfoWindow : Form
    {
        private ToolTip DynamicToolTip = new ToolTip();
        private String LicensePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\OmniMIDI\\LICENSE.TXT";
        private RegistryKey WVerKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", false);
        private FileVersionInfo Driver = FileVersionInfo.GetVersionInfo(Environment.SystemDirectory + "\\OmniMIDI\\OmniMIDI.dll");
        private FileVersionInfo BASS = FileVersionInfo.GetVersionInfo(Environment.SystemDirectory + "\\OmniMIDI\\bass.dll");
        private FileVersionInfo BASSMIDI = FileVersionInfo.GetVersionInfo(Environment.SystemDirectory + "\\OmniMIDI\\bassmidi.dll");

        private string ReturnDriverAssemblyVersion(String Component, String Type, Int32[] VI)
        {
            return String.Format("{0}{1}{2}{3}{4}",
                (!String.IsNullOrEmpty(Component)) ? Component + " " : null,
                VI[0],
                String.Format(".{0}", VI[1]),
                String.Format(".{0}", VI[2]),
                (VI[3] < 1) ? "" : String.Format(" - {0}{1}", Type, VI[3])
                );
        }

        public InfoWindow()
        {
            InitializeComponent();

            VerLabel.Text = ReturnDriverAssemblyVersion(
                "OmniMIDI", 
                "CR", 
                new int[] { Driver.FileMajorPart, Driver.FileMinorPart, Driver.FileBuildPart, Driver.FilePrivatePart }
                );
            VerLabel.Cursor = Program.SystemHandCursor;

            BASSVer.Text = ReturnDriverAssemblyVersion(
                null,
                "U",
                new int[] { BASS.FileMajorPart, BASS.FileMinorPart, BASS.FileBuildPart, BASS.FilePrivatePart }
                );

            BASSMIDIVer.Text = ReturnDriverAssemblyVersion(
                null,
                "U",
                new int[] { BASSMIDI.FileMajorPart, BASSMIDI.FileMinorPart, BASSMIDI.FileBuildPart, BASSMIDI.FilePrivatePart }
                );

            int[] KDMAPIVerRef = { 0, 0, 0, 0 };
            if (KDMAPI.ReturnKDMAPIVer(ref KDMAPIVerRef[0], ref KDMAPIVerRef[1], ref KDMAPIVerRef[2], ref KDMAPIVerRef[3]) != 0)
            {
                KDMAPIVer.Text = ReturnDriverAssemblyVersion(
                    null,
                    "U",
                    KDMAPIVerRef
                    );
            }
            else KDMAPIVer.Text = "N/A";

            CopyrightLabel.Text = String.Format(CopyrightLabel.Text, DateTime.Today.Year);

            CurBranch.Text = UpdateSystem.GetCurrentBranch();
            CurBranch.ForeColor = UpdateSystem.GetCurrentBranchColor();
            BranchToolTip.SetToolTip(CurBranch, UpdateSystem.GetCurrentBranchToolTip());
            if (Properties.Settings.Default.PreRelease) VerLabel.Text += " (PR)";

            OMBigLogo.Image = 
                (DateTime.Today.Month == 4 && DateTime.Today.Day == 1) ? Properties.Resources.OMLauncherFish : Properties.Resources.OMLauncher;

            BB.Location = new Point(OMBigLogo.Size.Width - BB.Size.Width - 8, OMBigLogo.Size.Height - BB.Size.Height - 8);
            BB.Parent = OMBigLogo;
            BB.Image = Properties.Resources.BB;
            BB.BackColor = Color.Transparent;

            BecomePatron.Cursor = Program.SystemHandCursor;
            BecomePatron.Image = Properties.Resources.PatreonLogo;

            PayPalDonation.Cursor = Program.SystemHandCursor;
            PayPalDonation.Image = Properties.Resources.PayPalLogo;

            GitHubPage.Cursor = Program.SystemHandCursor;
            GitHubPage.Image = Properties.Resources.Octocat;

            OMLicense.Cursor = Program.SystemHandCursor;
            OMLicense.Image = Properties.Resources.TextLogo;

            WinName.Text = String.Format("{0}", OSInfo.Name.Replace("Microsoft ", ""));
            RAMAmount.Text = SoundFontListExtension.ReturnSoundFontSize(null, "ram", Convert.ToInt64((new ComputerInfo()).TotalPhysicalMemory));
            switch (Environment.OSVersion.Version.Major)
            {
                case 10:
                    WinVer.Text = String.Format(
                        "Version {0} ({1})\nRelease {2}, Revision {3}",
                        WVerKey.GetValue("ReleaseId", 0).ToString(),
                        Environment.Is64BitOperatingSystem ? "64-bit" : "32-bit",
                        Environment.OSVersion.Version.Build,
                        WVerKey.GetValue("UBR", 0).ToString()
                        );
                    break;
                case 6:
                    if (Environment.OSVersion.Version.Minor > 1)
                    {
                        WinVer.Text = String.Format(
                            "Version {0}.{1}\nBuild {2}",
                            Environment.OSVersion.Version.Major,
                            Environment.OSVersion.Version.Minor,
                            Environment.OSVersion.Version.Build
                            );
                    }
                    else
                    {
                        if (Int32.Parse(Regex.Match(Environment.OSVersion.ServicePack, @"\d+").Value, NumberFormatInfo.InvariantInfo) > 0)
                        {
                            WinVer.Text = String.Format("{0}.{1}\nBuild {2}, Service Pack {3}",
                                Environment.OSVersion.Version.Major, Environment.OSVersion.Version.Minor,
                                Environment.OSVersion.Version.Build, Environment.OSVersion.ServicePack);
                        }
                        else
                        {
                            WinVer.Text = String.Format("{0}.{1}\nBuild {2}",
                                Environment.OSVersion.Version.Major, Environment.OSVersion.Version.Minor,
                                Environment.OSVersion.Version.Build);
                        }
                    }
                    break;
            }
        }

        private void InfoWindow_Load(object sender, EventArgs e)
        {
            // Nothing lul
        }

        private void VerLabel_Click(object sender, EventArgs e)
        {
            new ChangelogWindow(Driver.FileVersion.ToString(), false).ShowDialog();
        }

        private void BecomePatron_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.patreon.com/KaleidonKep99");
        }

        private void PayPalDonation_Click(object sender, EventArgs e)
        {
            Process.Start("https://paypal.me/Keppy99");
        }

        private void GitHubPage_Click(object sender, EventArgs e)
        {
            Process.Start(Properties.Settings.Default.ProjectLink);
        }

        private void OMLicense_Click(object sender, EventArgs e)
        {
            String License = File.ReadAllText(LicensePath, Encoding.UTF8);
            new TextReader("License", License).ShowDialog();
        }

        private void ChangeBranch_Click(object sender, EventArgs e)
        {
            new SelectBranch().ShowDialog();

            CurBranch.Text = UpdateSystem.GetCurrentBranch();
            CurBranch.ForeColor = UpdateSystem.GetCurrentBranchColor();
            BranchToolTip.SetToolTip(CurBranch, UpdateSystem.GetCurrentBranchToolTip());
        }

        private void CheckForUpdates_Click(object sender, EventArgs e)
        {
            UpdateSystem.CheckForUpdates((Control.ModifierKeys == Keys.Shift), false, false);
        }

        private void BecomePatron_MouseHover(object sender, EventArgs e)
        {
            DynamicToolTip.Dispose();
            DynamicToolTip = new ToolTip();
            DynamicToolTip.ToolTipIcon = ToolTipIcon.Info;
            DynamicToolTip.ToolTipTitle = "Patreon";
            DynamicToolTip.SetToolTip(BecomePatron, "Click here to visit my Patreon page");
        }

        private void PayPalDonation_MouseHover(object sender, EventArgs e)
        {
            DynamicToolTip.Dispose();
            DynamicToolTip = new ToolTip();
            DynamicToolTip.ToolTipIcon = ToolTipIcon.Info;
            DynamicToolTip.ToolTipTitle = "PayPal";
            DynamicToolTip.SetToolTip(PayPalDonation, "Click here to donate");
        }

        private void GitHubPage_MouseHover(object sender, EventArgs e)
        {
            DynamicToolTip.Dispose();
            DynamicToolTip = new ToolTip();
            DynamicToolTip.ToolTipIcon = ToolTipIcon.Info;
            DynamicToolTip.ToolTipTitle = "GitHub";
            DynamicToolTip.SetToolTip(GitHubPage, "Open the official GitHub project page");
        }

        private void OMLicense_MouseHover(object sender, EventArgs e)
        {
            DynamicToolTip.Dispose();
            DynamicToolTip = new ToolTip();
            DynamicToolTip.ToolTipIcon = ToolTipIcon.Info;
            DynamicToolTip.ToolTipTitle = "License";
            DynamicToolTip.SetToolTip(OMLicense, "Read the license for this piece of software");
        }

        private void OMBigLogo_MouseEnter(object sender, EventArgs e)
        {
            BB.Visible = true;
        }

        private void OMBigLogo_MouseLeave(object sender, EventArgs e)
        {
            BB.Visible = false;
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
