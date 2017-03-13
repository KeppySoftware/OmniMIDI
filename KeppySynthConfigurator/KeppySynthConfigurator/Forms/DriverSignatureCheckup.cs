using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Media;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KeppySynthConfigurator
{
    public partial class DriverSignatureCheckup : Form
    {
        Boolean Is32BitNoMatch = true;
        Boolean Is64BitNoMatch = true;
        Boolean IsNewVerAvailable = false;
        Stream OnlyOneError = Properties.Resources.error;
        Stream TwoError = Properties.Resources.warning;
        IntPtr WOW64Value = IntPtr.Zero;

        public DriverSignatureCheckup(String SHA25632, String SHA25664, String NewSHA25632, String NewSHA25664, Boolean Is64Bit)
        {
            InitializeComponent();

            var sha32 = new SHA256Managed();
            var DLL32bit = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\keppysynth\\keppysynth.dll", FileMode.OpenOrCreate, FileAccess.Read);
            byte[] checksum32 = sha32.ComputeHash(DLL32bit);
            String Driver32SHA256 = BitConverter.ToString(checksum32).Replace("-", String.Empty);
            String Driver64SHA256 = null;

            Driver32Current.Text = Driver32SHA256;
            Driver32Expected.Text = SHA25632;

            if (Driver32SHA256 != SHA25632)
            {
                if (SHA25632 != NewSHA25632)
                {
                    Driver32Status.Image = KeppySynthConfigurator.Properties.Resources.erroriconupd;
                }
                else
                {
                    Driver32Status.Image = KeppySynthConfigurator.Properties.Resources.erroricon;
                }
                Is32BitNoMatch = false;
            }
            else
            {
                if (SHA25632 != NewSHA25632)
                {
                    Driver32Status.Image = KeppySynthConfigurator.Properties.Resources.successiconupd;
                }
                else
                {
                    Driver32Status.Image = KeppySynthConfigurator.Properties.Resources.successicon;
                }
                Is32BitNoMatch = true;
            }

            if (Environment.Is64BitOperatingSystem)
            {
                Functions.Wow64DisableWow64FsRedirection(ref WOW64Value);
                var sha64 = new SHA256Managed();
                var DLL64bit = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\keppysynth\\keppysynth.dll", FileMode.OpenOrCreate, FileAccess.Read);
                byte[] checksum64 = sha64.ComputeHash(DLL64bit);
                Driver64SHA256 = BitConverter.ToString(checksum64).Replace("-", String.Empty);
                Functions.Wow64RevertWow64FsRedirection(WOW64Value);

                Driver64Current.Text = Driver64SHA256;
                Driver64Expected.Text = SHA25664;

                if (Driver64SHA256 != SHA25664)
                {
                    if (SHA25664 != NewSHA25664)
                    {
                        Driver64Status.Image = KeppySynthConfigurator.Properties.Resources.erroriconupd;
                    }
                    else
                    {
                        Driver64Status.Image = KeppySynthConfigurator.Properties.Resources.erroricon;
                    }
                    Is64BitNoMatch = false;
                }
                else
                {
                    if (SHA25664 != NewSHA25664)
                    {
                        Driver64Status.Image = KeppySynthConfigurator.Properties.Resources.successiconupd;
                    }
                    else
                    {
                        Driver64Status.Image = KeppySynthConfigurator.Properties.Resources.successicon;
                    }
                    Is64BitNoMatch = true;
                }
            }

            String OriginalRelease = "{0} not the original from GitHub. Click here to download the original release.";
            String LatestRelease = "{0} not the original from GitHub, but newer versions are available.\nClick here to download the latest release.";
            String EverythingFine = "Both drivers are the originals from GitHub. Everything's good, press OK to close the dialog.";
            
            if (!Is32BitNoMatch && !Is64BitNoMatch)
            {
                if (SHA25632 != NewSHA25632 || SHA25664 != NewSHA25664)
                {
                    BothDriverStatus.Text = String.Format(LatestRelease, "Both drivers are");
                    BothDriverStatus.ForeColor = Color.DarkRed;
                    BothDriverStatus.Cursor = Cursors.Hand;
                    BothDriverStatus.Font = new Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
                else
                {
                    BothDriverStatus.Text = String.Format(OriginalRelease, "Both drivers are");
                    BothDriverStatus.ForeColor = Color.DarkRed;
                    BothDriverStatus.Cursor = Cursors.Hand;
                    BothDriverStatus.Font = new Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
            }
            else if (!Is32BitNoMatch)
            {
                if (SHA25632 != NewSHA25632 || SHA25664 != NewSHA25664)
                {
                    BothDriverStatus.Text = String.Format(LatestRelease, "The 32-bit driver is");
                    BothDriverStatus.ForeColor = Color.Peru;
                    BothDriverStatus.Cursor = Cursors.Hand;
                    BothDriverStatus.Font = new Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
                else
                {
                    BothDriverStatus.Text = String.Format(OriginalRelease, "The 32-bit driver is");
                    BothDriverStatus.ForeColor = Color.Peru;
                    BothDriverStatus.Cursor = Cursors.Hand;
                    BothDriverStatus.Font = new Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
            }
            else if (!Is64BitNoMatch)
            {
                if (SHA25632 != NewSHA25632 || SHA25664 != NewSHA25664)
                {
                    BothDriverStatus.Text = String.Format(LatestRelease, "The 64-bit driver is");
                    BothDriverStatus.ForeColor = Color.Peru;
                    BothDriverStatus.Cursor = Cursors.Hand;
                    BothDriverStatus.Font = new Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
                else
                {
                    BothDriverStatus.Text = String.Format(OriginalRelease, "The 64-bit driver is");
                    BothDriverStatus.ForeColor = Color.Peru;
                    BothDriverStatus.Cursor = Cursors.Hand;
                    BothDriverStatus.Font = new Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
            }
            else
            {
                if (SHA25632 != NewSHA25632 || SHA25664 != NewSHA25664)
                {
                    BothDriverStatus.Text = "Both drivers are the originals from GitHub, but newer versions are available.\nClick here to download the latest release.";
                    BothDriverStatus.ForeColor = Color.Blue;
                    BothDriverStatus.Cursor = Cursors.Hand;
                    BothDriverStatus.Font = new Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
                else
                {
                    BothDriverStatus.Text = "Both drivers are the originals from GitHub. Everything's good, press OK to close the dialog.";
                    BothDriverStatus.ForeColor = Color.Green;
                    BothDriverStatus.Cursor = Cursors.Arrow;
                    BothDriverStatus.Font = new Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
            }
        }

        private void DriverSignatureCheckup_Load(object sender, EventArgs e)
        {
            if (!Is32BitNoMatch && !Is64BitNoMatch)
            {
                SoundPlayer snd = new SoundPlayer(TwoError);
                snd.Play();
            }
            else if (!Is32BitNoMatch || !Is64BitNoMatch)
            {
                SoundPlayer snd = new SoundPlayer(OnlyOneError);
                snd.Play();
            }
        }

        private void ClosePls_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BothDriverStatus_Click(object sender, EventArgs e)
        {
            if (!Is32BitNoMatch || !Is64BitNoMatch)
            {
                var p = new System.Diagnostics.Process();
                p.StartInfo.FileName = Application.ExecutablePath;
                p.StartInfo.Arguments = "/REI";
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                Application.ExitThread();
            }
        }
    }
}
