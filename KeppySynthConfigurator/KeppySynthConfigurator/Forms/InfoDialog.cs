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
using Microsoft.VisualBasic.Devices;
using Microsoft.Win32;

namespace KeppySynthConfigurator
{
    public partial class InfoDialog : Form
    {
        // Funcs

        private RegistryKey CurrentVerKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", false);
        private FileVersionInfo Driver = FileVersionInfo.GetVersionInfo(Environment.SystemDirectory + "\\keppysynth\\keppysynth.dll");
        private FileVersionInfo BASS = FileVersionInfo.GetVersionInfo(Environment.SystemDirectory + "\\keppysynth\\bass.dll");
        private FileVersionInfo BASSMIDI = FileVersionInfo.GetVersionInfo(Environment.SystemDirectory + "\\keppysynth\\bassmidi.dll");
        private String License = Environment.SystemDirectory + "\\keppysynth\\license.txt";

        private DateTime GetLinkerTime(Assembly assembly, TimeZoneInfo target = null)
        {
            var filePath = assembly.Location;
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;

            var buffer = new byte[2048];

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                stream.Read(buffer, 0, 2048);

            var offset = BitConverter.ToInt32(buffer, c_PeHeaderOffset);
            var secondsSince1970 = BitConverter.ToInt32(buffer, offset + c_LinkerTimestampOffset);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var linkTimeUtc = epoch.AddSeconds(secondsSince1970);

            var tz = target ?? TimeZoneInfo.Local;
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, tz);

            return localTime;
        }

        // Dialog

        public InfoDialog(Int32 mode)
        {
            InitializeComponent();
            if (mode == 0)
                StartPosition = FormStartPosition.CenterParent;
            else
                StartPosition = FormStartPosition.CenterScreen;
        }

        private void InfoDialog_Load(object sender, EventArgs e)
        {
            ComputerInfo CI = new ComputerInfo();
            String Version = String.Format("{0}.{1}.{2}", Driver.FileMajorPart, Driver.FileMinorPart, Driver.FileBuildPart);
            VerLabel.Text = String.Format("Keppy's Synthesizer {0}\n\nCopyright Ⓒ 2013 - {1}\nKaleidonKep99, Kode54 && Mudlord", Version, DateTime.Now.Year.ToString());
            DriverVer.Text = String.Format("{0} (Bugfix {1})", Version, Driver.FilePrivatePart);
            BASSVer.Text = String.Format("{0} (Revision {1})", BASS.FileVersion, BASS.FilePrivatePart);
            BASSMIDIVer.Text = String.Format("{0} (Revision {1})", BASSMIDI.FileVersion, BASSMIDI.FilePrivatePart);
            CompiledOn.Text = GetLinkerTime(Assembly.GetExecutingAssembly(), TimeZoneInfo.Utc).ToString();

            if (Environment.Is64BitOperatingSystem == true) // If OS is 64-bit, show "64-bit"
            { 
                WinName.Text = String.Format("{0} ({1})", CI.OSFullName, "64-bit");
            }
            else // Else, show "32-bit"
            {
                WinName.Text = String.Format("{0} ({1})", CI.OSFullName, "32-bit");
            }

            if (Environment.OSVersion.Version.Major == 10) // If OS is Windows 10, get UBR too
            {
                WinVer.Text = String.Format("{0}.{1}.{2} (Update Build Revision {3})",
                   Environment.OSVersion.Version.Major.ToString(), Environment.OSVersion.Version.Minor.ToString(),
                   Environment.OSVersion.Version.Build.ToString(), CurrentVerKey.GetValue("UBR", 0).ToString());
            }
            else // Else, give normal version number
            {
                WinVer.Text = String.Format("{0}.{1}.{2}",
                   Environment.OSVersion.Version.Major.ToString(), Environment.OSVersion.Version.Minor.ToString(),
                   Environment.OSVersion.Version.Build.ToString());
            }
        }

        private void OKClose_Click(object sender, EventArgs e)
        {
            CurrentVerKey.Close();
            Close();
        }

        private void GitHubLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(GitHubLink.Text);
        }

        private void LicenseFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Keppy's Synthesizer\\license.txt");
        }

        private void CTC_Click(object sender, EventArgs e)
        {

        }

        private void CFU_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                Functions.CheckForUpdates(true, false);
            }
            else
            {
                Functions.CheckForUpdates(false, false);
            }
        }
    }
}
