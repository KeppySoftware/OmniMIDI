using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Text;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using System.Diagnostics;
using Microsoft.Win32;
// For SF info
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Midi;
using System.Threading;
using Un4seen.BassAsio;

namespace KeppySynthConfigurator
{
    class Functions
    {
        // Disable WoW64 directory redirection
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);

        // Enable WoW64 directory redirection
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool Wow64RevertWow64FsRedirection(IntPtr ptr);

        // Notify for file association updates
        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

        // Get size of memory
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);

        public static string IsWindows8OrNewer() // Checks if you're using Windows 8.1 or newer
        {
            var reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
            string productName = (string)reg.GetValue("ProductName");
            return productName;
        }

        public static bool IsWindowsVistaOrNewer() // Checks if you're using Windows Vista or newer
        {
            OperatingSystem OS = Environment.OSVersion;
            return (OS.Version.Major >= 6);
        }

        // Buffer stuff

        public static void ChangeRecommendedBuffer(Int32 Index, out Int32[] valuearray)
        {
            valuearray = new Int32[10];

            valuearray[0] = 0;
            valuearray[9] = 100;

            if (Index == 0)
            {
                valuearray[1] = 50;
                valuearray[2] = 51;
                valuearray[3] = 60;
                valuearray[4] = 61;
                valuearray[5] = 70;
                valuearray[6] = 71;
                valuearray[7] = 90;
                valuearray[8] = 91;
            }
            else if (Index == 1)
            {
                valuearray[1] = 45;
                valuearray[2] = 46;
                valuearray[3] = 55;
                valuearray[4] = 56;
                valuearray[5] = 65;
                valuearray[6] = 66;
                valuearray[7] = 85;
                valuearray[8] = 86;
            }
            else if (Index == 2)
            {
                valuearray[1] = 40;
                valuearray[2] = 41;
                valuearray[3] = 44;
                valuearray[4] = 45;
                valuearray[5] = 55;
                valuearray[6] = 56;
                valuearray[7] = 60;
                valuearray[8] = 61;
            }
            else if (Index >= 3 && Index <= 5)
            {
                valuearray[1] = 24;
                valuearray[2] = 25;
                valuearray[3] = 29;
                valuearray[4] = 30;
                valuearray[5] = 49;
                valuearray[6] = 50;
                valuearray[7] = 79;
                valuearray[8] = 80;
            }
            else if (Index >= 6 && Index <= 10)
            {
                valuearray[1] = 14;
                valuearray[2] = 15;
                valuearray[3] = 19;
                valuearray[4] = 20;
                valuearray[5] = 49;
                valuearray[6] = 50;
                valuearray[7] = 69;
                valuearray[8] = 70;
            }
            else if (Index >= 11 && Index <= 12)
            {
                valuearray[1] = 9;
                valuearray[2] = 10;
                valuearray[3] = 14;
                valuearray[4] = 15;
                valuearray[5] = 39;
                valuearray[6] = 40;
                valuearray[7] = 59;
                valuearray[8] = 60;
            }
            else if (Index >= 13 && Index <= 15)
            {
                valuearray[1] = 9;
                valuearray[2] = 10;
                valuearray[3] = 14;
                valuearray[4] = 15;
                valuearray[5] = 39;
                valuearray[6] = 40;
                valuearray[7] = 59;
                valuearray[8] = 60;
            }
            else if (Index >= 16 && Index <= 18)
            {
                valuearray[1] = 4;
                valuearray[2] = 5;
                valuearray[3] = 9;
                valuearray[4] = 10;
                valuearray[5] = 19;
                valuearray[6] = 20;
                valuearray[7] = 39;
                valuearray[8] = 40;
            }
            else if (Index >= 19 && Index <= 20)
            {
                valuearray[1] = 0;
                valuearray[2] = 1;
                valuearray[3] = 4;
                valuearray[4] = 4;
                valuearray[5] = 10;
                valuearray[6] = 11;
                valuearray[7] = 29;
                valuearray[8] = 30;
            }
            else
            {
                valuearray[1] = 9;
                valuearray[2] = 10;
                valuearray[3] = 14;
                valuearray[4] = 15;
                valuearray[5] = 39;
                valuearray[6] = 40;
                valuearray[7] = 59;
                valuearray[8] = 60;
            }

            // Done, it'll output the array now
        }

        // Buffer stuff

        public static void DriverToSynthMigration() // Basically changes the directory's name
        {
            try
            {
                string oldlocation = System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Keppy's Driver\\";
                string newlocation = System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Keppy's Synthesizer\\";
                Directory.Move(oldlocation, newlocation);
            }
            catch { }
        }

        public static void UserProfileMigration() // Migrates the Keppy's Synthesizer folder from %localappdata% (Unsupported on XP) to %userprofile% (Supported on XP, now used on Vista+ too)
        {
            try
            {
                string oldlocation = System.Environment.GetEnvironmentVariable("LOCALAPPDATA") + "\\Keppy's Synthesizer\\";
                string newlocation = System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Keppy's Synthesizer\\";
                Directory.Move(oldlocation, newlocation);
            }
            catch { }
        }

        public static void DriverRegistry()
        {
            try
            {
                Program.DebugToConsole(false, "Opening register/unregister dialog...", null);
                var process = System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\keppysynth\\KSDriverRegister.exe");
                process.WaitForExit();
                Program.DebugToConsole(false, "Done.", null);
            }
            catch (Exception ex)
            {
                Functions.ShowErrorDialog(1, System.Media.SystemSounds.Hand, "Error", "There was an error while trying to register/unregister the driver.", true, ex);
            }
        }

        public static void MIDIMapRegistry(int integer)
        {
            try
            {
                if (integer == 0)
                {
                    var process = System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\keppysynth\\KSDriverRegister.exe", "/rmidimapv");
                    process.WaitForExit();
                }
                else
                {
                    var process = System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\keppysynth\\KSDriverRegister.exe", "/umidimapv");
                    process.WaitForExit();
                }
                CheckMIDIMapper();
            }
            catch (Exception ex)
            {
                Functions.ShowErrorDialog(1, System.Media.SystemSounds.Hand, "Error", "There was an error while trying to register/unregister the driver.", true, ex);
            }
        }

        public static void GetSHA256OfDLLs(Boolean GetSHAToClipboard)
        {
            try
            {
                if (GetSHAToClipboard)
                {
                    IntPtr WOW64Value = IntPtr.Zero;

                    var sha32 = new SHA256Managed();
                    var DLL32bit = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\keppysynth\\keppysynth.dll", FileMode.OpenOrCreate, FileAccess.Read);
                    byte[] checksum32 = sha32.ComputeHash(DLL32bit);
                    String Driver32SHA256 = BitConverter.ToString(checksum32).Replace("-", String.Empty);
                    String Driver64SHA256 = null;

                    if (Environment.Is64BitOperatingSystem)
                    {
                        Functions.Wow64DisableWow64FsRedirection(ref WOW64Value);
                        var sha64 = new SHA256Managed();
                        var DLL64bit = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\keppysynth\\keppysynth.dll", FileMode.OpenOrCreate, FileAccess.Read);
                        byte[] checksum64 = sha64.ComputeHash(DLL64bit);
                        Driver64SHA256 = BitConverter.ToString(checksum64).Replace("-", String.Empty);
                        Functions.Wow64RevertWow64FsRedirection(WOW64Value);
                    }

                    StringBuilder sb = new StringBuilder();
                    sb.Append(String.Format("32-bit SHA256: {0}\n", Driver32SHA256));
                    sb.Append(String.Format("64-bit SHA256: {0}", Driver64SHA256));
                    Clipboard.SetText(sb.ToString());

                    MessageBox.Show("Driver signatures copied to the clipboard.", "Driver signature check", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    WebClient client = new WebClient();
                    Regex rgx = new Regex("[^a-zA-Z0-9 -]");

                    Stream Stream32 = client.OpenRead("https://raw.githubusercontent.com/KaleidonKep99/Keppy-s-Synthesizer/master/KeppySynthConfigurator/KeppySynthConfigurator/Resources/Hashes/DRV32.SHA");
                    StreamReader Reader32 = new StreamReader(Stream32);
                    String New32SHA256 = Reader32.ReadToEnd();
                    New32SHA256 = rgx.Replace(New32SHA256, "");
                    String New64SHA256 = null;
                    String Current32SHA256 = null;
                    String Current64SHA256 = null;

                    Current32SHA256 = System.Text.Encoding.UTF8.GetString(KeppySynthConfigurator.Properties.Resources.DRV32);

                    if (Environment.Is64BitOperatingSystem)
                    {
                        Stream Stream64 = client.OpenRead("https://raw.githubusercontent.com/KaleidonKep99/Keppy-s-Synthesizer/master/KeppySynthConfigurator/KeppySynthConfigurator/Resources/Hashes/DRV64.SHA");
                        StreamReader Reader64 = new StreamReader(Stream64);
                        New64SHA256 = Reader64.ReadToEnd();
                        New64SHA256 = rgx.Replace(New64SHA256, "");
                        Current64SHA256 = System.Text.Encoding.UTF8.GetString(KeppySynthConfigurator.Properties.Resources.DRV64);
                    }

                    DriverSignatureCheckup frm = new DriverSignatureCheckup(Current32SHA256, Current64SHA256, New32SHA256, New64SHA256, Environment.Is64BitOperatingSystem);
                    frm.ShowDialog();
                    frm.Dispose();
                }
            }
            catch
            {
                MessageBox.Show("Can not check for the current signatures on GitHub!\nYou may not be connected to the Internet, or GitHub may have server issues.\n\nCheck if your Internet connection is working properly, or try again later.", "Keppy's Synthesizer - Driver signature check error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        public static void ShowErrorDialog(Int32 Type, System.Media.SystemSound sound, String title, String message, bool IsException, Exception ex)
        {
            SecretDialog frm = new SecretDialog(Type, sound, title, message, ex);
            Program.DebugToConsole(IsException, message, ex);
            frm.ShowDialog();
            frm.Dispose();
        }

        public static void SetDriverPriority(int priority)
        {
            KeppySynthConfiguratorMain.SynthSettings.SetValue("driverprio", priority, RegistryValueKind.DWord);
        }

        public static void SetFramerate(int yesno)
        {
            KeppySynthConfiguratorMain.SynthSettings.SetValue("capframerate", yesno, RegistryValueKind.DWord);
        }

        public static void SleepStates(int yesno)
        {
            KeppySynthConfiguratorMain.SynthSettings.SetValue("rco", yesno, RegistryValueKind.DWord);
        }

        public static void OldBufferMode(int yesno)
        {
            KeppySynthConfiguratorMain.SynthSettings.SetValue("oldbuffermode", yesno, RegistryValueKind.DWord);
        }

        // -------------------------
        // Soundfont lists functions

        public static void SetLastMIDIPath(string path) // Saves the last path from the SoundFont preview dialog to the registry 
        {
            try
            {
                SoundFontInfo.LastMIDIPath = path;
                KeppySynthConfiguratorMain.SynthPaths.SetValue("lastpathmidimport", path);
                Program.DebugToConsole(false, String.Format("Last MIDI preview path is: {0}", path), null);
            }
            catch
            {
                Functions.InitializeLastPath();
            }
        }

        // NOT SUPPORTED ON XP
        public static void OpenFileDialogAddCustomPaths(FileDialog dialog) // Allows you to add favorites to the SoundfontImport dialog
        {
            try
            {
                // Import the blacklist file
                using (StreamReader r = new StreamReader(System.Environment.GetEnvironmentVariable("USERPROFILE").ToString() + "\\Keppy's Synthesizer\\keppymididrv.favlist"))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        dialog.CustomPlaces.Add(line);
                    }
                    return;
                }
            }
            catch { return; }
        }
        // NOT SUPPORTED ON XP

        public static void VolumeBoostSwitch()
        {
            if (KeppySynthConfiguratorMain.Delegate.VolumeBoost.Checked)
            {
                if (KeppySynthConfiguratorMain.Delegate.VolTrackBar.Value > 10000)
                {
                    KeppySynthConfiguratorMain.Delegate.VolTrackBar.Value = 10000;
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("volume", 10000, RegistryValueKind.DWord);
                }
                KeppySynthConfiguratorMain.SynthSettings.SetValue("volumeboost", 0, RegistryValueKind.DWord);
                KeppySynthConfiguratorMain.Delegate.VolTrackBar.Maximum = 10000;
                KeppySynthConfiguratorMain.Delegate.VolumeBoost.Checked = false;
                KeppySynthConfiguratorMain.Delegate.VolTrackBar.Refresh();
            }
            else
            {
                KeppySynthConfiguratorMain.SynthSettings.SetValue("volumeboost", 1, RegistryValueKind.DWord);
                KeppySynthConfiguratorMain.Delegate.VolTrackBar.Maximum = 50000;
                KeppySynthConfiguratorMain.Delegate.VolumeBoost.Checked = true;
                KeppySynthConfiguratorMain.Delegate.VolTrackBar.Refresh();
            }
        }

        public static void SetDefaultDevice(int engine, int dev)
        {
            if (engine == 0)
                KeppySynthConfiguratorMain.SynthSettings.SetValue("defaultdev", dev, RegistryValueKind.DWord);
            else if (engine == 1)
                KeppySynthConfiguratorMain.SynthSettings.SetValue("defaultAdev", dev, RegistryValueKind.DWord);
            else if (engine == 2)
                KeppySynthConfiguratorMain.SynthSettings.SetValue("defaultWdev", dev, RegistryValueKind.DWord);


            if (Properties.Settings.Default.LiveChanges) KeppySynthConfiguratorMain.SynthSettings.SetValue("livechange", "1", RegistryValueKind.DWord);
        }

        public static void SetDefaultMIDIInDevice(int dev)
        {
            KeppySynthConfiguratorMain.SynthSettings.SetValue("defaultmidiindev", dev, RegistryValueKind.DWord);
        }

        public static void CheckMIDIMapper() // Check if the Alternative MIDI Mapper is installed
        {
            RegistryKey CLSID = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\Drivers32", false);
            if (CLSID.GetValue("midimapper", "midimap.dll").ToString() == "keppysynth\\amidimap.cpl")
            {
                KeppySynthConfiguratorMain.Delegate.AMIDIMapCpl.Visible = true;
                KeppySynthConfiguratorMain.Delegate.changeDefaultMIDIOutDeviceToolStripMenuItem1.Visible = false;
                KeppySynthConfiguratorMain.Delegate.changeDefaultMIDIOutDeviceToolStripMenuItem.Visible = false;
                KeppySynthConfiguratorMain.Delegate.changeDefault64bitMIDIOutDeviceToolStripMenuItem.Visible = false;
                KeppySynthConfiguratorMain.Delegate.SetSynthDefault.Visible = false;
            }
            else
            {
                KeppySynthConfiguratorMain.Delegate.AMIDIMapCpl.Visible = false;
                if (Environment.OSVersion.Version.Major > 6)
                {
                    KeppySynthConfiguratorMain.Delegate.changeDefaultMIDIOutDeviceToolStripMenuItem1.Text = "Change default MIDI out device for Windows Media Player";
                    KeppySynthConfiguratorMain.Delegate.changeDefaultMIDIOutDeviceToolStripMenuItem.Text = "Change default MIDI out device for Windows Media Player 32-bit";
                    KeppySynthConfiguratorMain.Delegate.changeDefault64bitMIDIOutDeviceToolStripMenuItem.Text = "Change default MIDI out device for Windows Media Player 64-bit";
                    KeppySynthConfiguratorMain.Delegate.SetSynthDefault.Visible = true;
                }

                if (!Environment.Is64BitOperatingSystem)
                {
                    KeppySynthConfiguratorMain.Delegate.changeDefaultMIDIOutDeviceToolStripMenuItem1.Visible = true;
                    KeppySynthConfiguratorMain.Delegate.changeDefaultMIDIOutDeviceToolStripMenuItem.Visible = false;
                    KeppySynthConfiguratorMain.Delegate.changeDefault64bitMIDIOutDeviceToolStripMenuItem.Visible = false;
                }
                else
                {
                    KeppySynthConfiguratorMain.Delegate.changeDefaultMIDIOutDeviceToolStripMenuItem1.Visible = false;
                    KeppySynthConfiguratorMain.Delegate.changeDefaultMIDIOutDeviceToolStripMenuItem.Visible = true;
                    KeppySynthConfiguratorMain.Delegate.changeDefault64bitMIDIOutDeviceToolStripMenuItem.Visible = true;
                }
            }
        }


        public static void CheckSincEnabled()
        {
            KeppySynthConfiguratorMain.Delegate.SincConvLab.Enabled = KeppySynthConfiguratorMain.Delegate.SincInter.Checked;
            KeppySynthConfiguratorMain.Delegate.SincConv.Enabled = KeppySynthConfiguratorMain.Delegate.SincInter.Checked;
        }

        public static void AudioEngBoxTrigger(bool save)
        {
            if (KeppySynthConfiguratorMain.Delegate.AudioEngBox.SelectedIndex == 0)
            {
                KeppySynthConfiguratorMain.Delegate.BufferText.Enabled = false;
                KeppySynthConfiguratorMain.Delegate.BufferText.Text = "Driver buffer length (in ms, from 1 to 1000)";
                KeppySynthConfiguratorMain.Delegate.DrvHzLabel.Enabled = true;
                KeppySynthConfiguratorMain.Delegate.Frequency.Enabled = true;
                KeppySynthConfiguratorMain.Delegate.MaxCPU.Enabled = false;
                KeppySynthConfiguratorMain.Delegate.RenderingTimeLabel.Enabled = false;
                KeppySynthConfiguratorMain.Delegate.StatusBuf.Enabled = false;
                KeppySynthConfiguratorMain.Delegate.StatusBuf.Visible = false;
                KeppySynthConfiguratorMain.Delegate.VolLabel.Enabled = false;
                KeppySynthConfiguratorMain.Delegate.VolSimView.Enabled = false;
                KeppySynthConfiguratorMain.Delegate.VolTrackBar.Enabled = false;
                KeppySynthConfiguratorMain.Delegate.bufsize.Enabled = false;
            }
            else if (KeppySynthConfiguratorMain.Delegate.AudioEngBox.SelectedIndex == 1)
            {
                KeppySynthConfiguratorMain.Delegate.VolLabel.Enabled = true;
                KeppySynthConfiguratorMain.Delegate.VolSimView.Enabled = true;
                KeppySynthConfiguratorMain.Delegate.VolTrackBar.Enabled = true;
                KeppySynthConfiguratorMain.Delegate.DrvHzLabel.Enabled = true;
                KeppySynthConfiguratorMain.Delegate.Frequency.Enabled = true;
                KeppySynthConfiguratorMain.Delegate.MaxCPU.Enabled = true;
                KeppySynthConfiguratorMain.Delegate.BufferText.Enabled = true;
                KeppySynthConfiguratorMain.Delegate.BufferText.Text = "Output buffer (in ms, from 1 to 1000)\n(Helps to reduce stuttering, keep it between 60-70ms for best quality)";
                KeppySynthConfiguratorMain.Delegate.bufsize.Enabled = true;
                KeppySynthConfiguratorMain.Delegate.StatusBuf.Visible = false;
                KeppySynthConfiguratorMain.Delegate.StatusBuf.Enabled = false;
            }
            else if (KeppySynthConfiguratorMain.Delegate.AudioEngBox.SelectedIndex == 2)
            {
                int numbdev;
                BASS_ASIO_DEVICEINFO info = new BASS_ASIO_DEVICEINFO();
                for (numbdev = 0; BassAsio.BASS_ASIO_GetDeviceInfo(numbdev, info); numbdev++) ;

                if (numbdev < 1)
                {
                    Functions.ShowErrorDialog(1, System.Media.SystemSounds.Asterisk, "Error", "No ASIO devices installed!\n\nClick OK to switch to WASAPI.", false, null);
                    KeppySynthConfiguratorMain.Delegate.AudioEngBox.SelectedIndex = 3;
                    AudioEngBoxTrigger(true);
                    return;
                }

                KeppySynthConfiguratorMain.Delegate.VolLabel.Enabled = true;
                KeppySynthConfiguratorMain.Delegate.VolSimView.Enabled = true;
                KeppySynthConfiguratorMain.Delegate.VolTrackBar.Enabled = true;
                KeppySynthConfiguratorMain.Delegate.DrvHzLabel.Enabled = true;
                KeppySynthConfiguratorMain.Delegate.Frequency.Enabled = true;
                KeppySynthConfiguratorMain.Delegate.BufferText.Enabled = false;
                KeppySynthConfiguratorMain.Delegate.StatusBuf.Visible = false;
                KeppySynthConfiguratorMain.Delegate.StatusBuf.Enabled = false;
                KeppySynthConfiguratorMain.Delegate.BufferText.Text = "Driver buffer length (in ms, from 1 to 1000)";
                KeppySynthConfiguratorMain.Delegate.bufsize.Enabled = false;
            }
            else if (KeppySynthConfiguratorMain.Delegate.AudioEngBox.SelectedIndex == 3)
            {
                KeppySynthConfiguratorMain.Delegate.VolLabel.Enabled = true;
                KeppySynthConfiguratorMain.Delegate.VolSimView.Enabled = true;
                KeppySynthConfiguratorMain.Delegate.VolTrackBar.Enabled = true;
                KeppySynthConfiguratorMain.Delegate.DrvHzLabel.Enabled = true;
                KeppySynthConfiguratorMain.Delegate.Frequency.Enabled = true;
                KeppySynthConfiguratorMain.Delegate.MaxCPU.Enabled = true;
                KeppySynthConfiguratorMain.Delegate.BufferText.Enabled = true;
                KeppySynthConfiguratorMain.Delegate.BufferText.Text = "Output buffer (in ms, from 1 to 1000)\n(It'll be set automatically to the lowest value possible)";
                KeppySynthConfiguratorMain.Delegate.bufsize.Enabled = true;
                KeppySynthConfiguratorMain.Delegate.StatusBuf.Visible = false;
                KeppySynthConfiguratorMain.Delegate.StatusBuf.Enabled = false;
            }
            else KeppySynthConfiguratorMain.Delegate.AudioEngBox.SelectedIndex = 3;

            KeppySynthConfiguratorMain.Delegate.changeDirectoryOfTheOutputToWAVModeToolStripMenuItem.Enabled = true;
            if (save) KeppySynthConfiguratorMain.SynthSettings.SetValue("xaudiodisabled", KeppySynthConfiguratorMain.Delegate.AudioEngBox.SelectedIndex, RegistryValueKind.DWord);
        }


        public static class LiveChanges
        {
            public static int PreviousEngine = 0;
            public static int PreviousFrequency = 0;
            public static int PreviousBuffer = 0;
            public static int MonophonicRender = 0;
            public static int AudioBitDepth = 0;
        }

        public static void LoadSettings(Form thisform) // Loads the settings from the registry
        {
            // ======= Load settings from the registry
            try
            {
                // First, the most important settings
                KeppySynthConfiguratorMain.Delegate.PolyphonyLimit.Value = Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("polyphony", 512));
                KeppySynthConfiguratorMain.Delegate.MaxCPU.Value = Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("cpu", 75));

                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("defaultmidiout", 0)) == 1)
                    KeppySynthConfiguratorMain.Delegate.SetSynthDefault.Checked = true;

                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("allhotkeys", 0)) == 1)
                    KeppySynthConfiguratorMain.Delegate.hotkeys.Checked = true;

                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("alternativecpu", 0)) == 1)
                    KeppySynthConfiguratorMain.Delegate.autopanicmode.Checked = true;

                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("printmidievent", 0)) == 1)
                    KeppySynthConfiguratorMain.Delegate.PrintMIDIEventsLog.Checked = true;

                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("printimportant", 1)) == 1)
                    KeppySynthConfiguratorMain.Delegate.PrintImportantLog.Checked = true;

                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("driverprio", 0)) == 0)
                {
                    ButtonStatus(false);
                    KeppySynthConfiguratorMain.Delegate.DePrio.Checked = true;
                }
                else if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("driverprio", 0)) == 1)
                    KeppySynthConfiguratorMain.Delegate.RTPrio.Checked = true;
                else if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("driverprio", 0)) == 2)
                    KeppySynthConfiguratorMain.Delegate.HiPrio.Checked = true;
                else if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("driverprio", 0)) == 3)
                    KeppySynthConfiguratorMain.Delegate.HNPrio.Checked = true;
                else if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("driverprio", 0)) == 4)
                    KeppySynthConfiguratorMain.Delegate.NoPrio.Checked = true;
                else if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("driverprio", 0)) == 5)
                    KeppySynthConfiguratorMain.Delegate.LNPrio.Checked = true;
                else
                    KeppySynthConfiguratorMain.Delegate.LoPrio.Checked = true;

                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("debugmode", 0)) == 1)
                    KeppySynthConfiguratorMain.Delegate.DebugModePls.Checked = true;

                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("debugmode", 0)) == 1)
                    KeppySynthConfiguratorMain.Delegate.DebugModePls.Checked = true;

                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("extra8lists", 0)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.enableextra8sf.Checked = true;
                    KeppySynthConfiguratorMain.Delegate.SelectedListBox.Items.Add("List 9");
                    KeppySynthConfiguratorMain.Delegate.SelectedListBox.Items.Add("List 10");
                    KeppySynthConfiguratorMain.Delegate.SelectedListBox.Items.Add("List 11");
                    KeppySynthConfiguratorMain.Delegate.SelectedListBox.Items.Add("List 12");
                    KeppySynthConfiguratorMain.Delegate.SelectedListBox.Items.Add("List 13");
                    KeppySynthConfiguratorMain.Delegate.SelectedListBox.Items.Add("List 14");
                    KeppySynthConfiguratorMain.Delegate.SelectedListBox.Items.Add("List 15");
                    KeppySynthConfiguratorMain.Delegate.SelectedListBox.Items.Add("List 16");
                }

                KeppySynthConfiguratorMain.Delegate.ShowOutLevel.Checked = Properties.Settings.Default.ShowOutputLevel;
                KeppySynthConfiguratorMain.Delegate.ShowMixerTools.Checked = Properties.Settings.Default.ShowMixerUnder;
                KeppySynthConfiguratorMain.Delegate.MixerBox.Visible = Properties.Settings.Default.ShowOutputLevel;
                KeppySynthConfiguratorMain.Delegate.VolumeCheck.Enabled = Properties.Settings.Default.ShowOutputLevel;
                KeppySynthConfiguratorMain.Delegate.LiveChangesTrigger.Checked = Properties.Settings.Default.LiveChanges;
                KeppySynthConfiguratorMain.Delegate.Requirements.Active = !Properties.Settings.Default.LiveChanges;

                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("volumeboost", 0)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.VolTrackBar.Maximum = 50000;
                    KeppySynthConfiguratorMain.Delegate.VolumeBoost.Checked = true;
                }
                else
                {
                    KeppySynthConfiguratorMain.Delegate.VolTrackBar.Maximum = 10000;
                    KeppySynthConfiguratorMain.Delegate.VolumeBoost.Checked = false;
                }

                if (Properties.Settings.Default.ShowMixerUnder)
                {
                    if (MeterFunc.CheckIfDedicatedMixerIsRunning(true))
                    {
                        KeppySynthConfiguratorMain.Delegate.ClientSize = new System.Drawing.Size(649, 442);
                        KeppySynthConfiguratorMain.Delegate.ShowOutLevel.Checked = true;
                        KeppySynthConfiguratorMain.Delegate.ShowOutLevel.Enabled = true;
                        KeppySynthConfiguratorMain.Delegate.ShowMixerTools.Checked = false;
                        Properties.Settings.Default.ShowOutputLevel = true;
                        Properties.Settings.Default.ShowMixerUnder = false;
                        KeppySynthConfiguratorMain.Delegate.MixerBox.Visible = true;
                        KeppySynthConfiguratorMain.Delegate.MixerPanel.Visible = false;
                        KeppySynthConfiguratorMain.Delegate.VolumeCheck.Enabled = true;
                        Properties.Settings.Default.Save();
                    }
                    else
                    {
                        MeterFunc.LoadChannelValues();
                        KeppySynthConfiguratorMain.Delegate.ClientSize = new System.Drawing.Size(649, 630);
                        KeppySynthConfiguratorMain.SynthSettings.SetValue("volumemon", "1", RegistryValueKind.DWord);
                        KeppySynthConfiguratorMain.Delegate.ShowOutLevel.Checked = true;
                        KeppySynthConfiguratorMain.Delegate.ShowOutLevel.Enabled = false;
                        KeppySynthConfiguratorMain.Delegate.ShowMixerTools.Checked = true;
                        KeppySynthConfiguratorMain.Delegate.MixerBox.Visible = false;
                        KeppySynthConfiguratorMain.Delegate.MixerPanel.Visible = true;
                        KeppySynthConfiguratorMain.Delegate.VolumeCheck.Enabled = true;
                    }
                }
                else
                {
                    {
                        KeppySynthConfiguratorMain.Delegate.ClientSize = new System.Drawing.Size(649, 442);
                        KeppySynthConfiguratorMain.Delegate.ShowOutLevel.Checked = true;
                        KeppySynthConfiguratorMain.Delegate.ShowOutLevel.Enabled = true;
                        KeppySynthConfiguratorMain.Delegate.ShowMixerTools.Checked = false;
                        KeppySynthConfiguratorMain.Delegate.MixerBox.Visible = true;
                        KeppySynthConfiguratorMain.Delegate.MixerPanel.Visible = false;
                        KeppySynthConfiguratorMain.Delegate.VolumeCheck.Enabled = true;
                        Properties.Settings.Default.Save();

                        thisform.Top = (Screen.PrimaryScreen.Bounds.Height - thisform.Height) / 2;
                        thisform.Left = (Screen.PrimaryScreen.Bounds.Width - thisform.Width) / 2;
                    }
                }

                KeppySynthConfiguratorMain.Delegate.AutoLoad.Checked = Properties.Settings.Default.AutoLoadList;

                KeppySynthConfiguratorMain.Delegate.Frequency.Text = KeppySynthConfiguratorMain.SynthSettings.GetValue("frequency", 44100).ToString();

                // Then the filthy checkboxes
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("preload", 1)) == 1)
                    KeppySynthConfiguratorMain.Delegate.Preload.Checked = true;

                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("nofx", 0)) == 0)
                    KeppySynthConfiguratorMain.Delegate.EnableSFX.Checked = true;

                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("noteoff", 0)) == 1)
                    KeppySynthConfiguratorMain.Delegate.NoteOffCheck.Checked = true;

                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("sysresetignore", 0)) == 1)
                    KeppySynthConfiguratorMain.Delegate.SysResetIgnore.Checked = true;

                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("xaudiodisabled", 0)) == 0)
                {
                    KeppySynthConfiguratorMain.Delegate.DrvHzLabel.Enabled = true;
                    KeppySynthConfiguratorMain.Delegate.Frequency.Enabled = true;
                    KeppySynthConfiguratorMain.Delegate.AudioEngBox.SelectedIndex = 0;
                    KeppySynthConfiguratorMain.Delegate.bufsize.Enabled = true;
                    KeppySynthConfiguratorMain.Delegate.BufferText.Text = "Driver buffer length (in ms, from 1 to 1000)";         
                }

                KeppySynthConfiguratorMain.Delegate.bufsize.Value = Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("buflen", 50));

                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("xaudiodisabled", 0)) == 0) KeppySynthConfiguratorMain.Delegate.AudioEngBox.SelectedIndex = 0;
                else if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("xaudiodisabled", 0)) == 1) KeppySynthConfiguratorMain.Delegate.AudioEngBox.SelectedIndex = 1;
                else if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("xaudiodisabled", 0)) == 2) KeppySynthConfiguratorMain.Delegate.AudioEngBox.SelectedIndex = 2;
                else if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("xaudiodisabled", 0)) == 3) KeppySynthConfiguratorMain.Delegate.AudioEngBox.SelectedIndex = 3;
                else KeppySynthConfiguratorMain.Delegate.AudioEngBox.SelectedIndex = 3;
                AudioEngBoxTrigger(false);

                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("sinc", 0)) == 1)
                    KeppySynthConfiguratorMain.Delegate.SincInter.Checked = true;
                Functions.CheckSincEnabled();

                try { KeppySynthConfiguratorMain.Delegate.SincConv.SelectedIndex = Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("sincconv", 0)); }
                catch { KeppySynthConfiguratorMain.Delegate.SincConv.SelectedIndex = 2; }

                if (Environment.OSVersion.Version.Major == 10 && Environment.OSVersion.Version.Build >= 15063)
                    KeppySynthConfiguratorMain.Delegate.SpatialSound.Visible = true;

                // And finally, the volume!
                int VolumeValue = Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("volume", 10000));
                KeppySynthConfiguratorMain.Delegate.VolTrackBar.Value = VolumeValue;
                decimal VolVal = (decimal)VolumeValue / 100;

                if (KeppySynthConfiguratorMain.Delegate.VolTrackBar.Value <= 49)
                    KeppySynthConfiguratorMain.Delegate.VolSimView.ForeColor = Color.Red;
                else
                    KeppySynthConfiguratorMain.Delegate.VolSimView.ForeColor = Color.Blue;

                KeppySynthConfiguratorMain.Delegate.VolSimView.Text = String.Format("{0}", Math.Round(VolVal, MidpointRounding.AwayFromZero).ToString());

                LiveChanges.PreviousEngine = (int)KeppySynthConfiguratorMain.SynthSettings.GetValue("xaudiodisabled", 0);
                LiveChanges.PreviousFrequency = (int)KeppySynthConfiguratorMain.SynthSettings.GetValue("frequency", 44100);
                LiveChanges.PreviousBuffer = (int)KeppySynthConfiguratorMain.SynthSettings.GetValue("buflen", 50);
                LiveChanges.MonophonicRender = (int)KeppySynthConfiguratorMain.SynthSettings.GetValue("monorendering", 0);
                LiveChanges.AudioBitDepth = (int)KeppySynthConfiguratorMain.SynthSettings.GetValue("32bit", 1);

                Program.DebugToConsole(false, "Done loading settings.", null);
            }
            catch (Exception ex)
            {
                Functions.ShowErrorDialog(1, System.Media.SystemSounds.Hand, "Error", "An error has occurred while loading the driver's settings.", true, ex);
                Program.DebugToConsole(true, null, ex);
                ReinitializeSettings(thisform);
            }
        }

        public static bool SaveSettings(Form thisform, Boolean Override) // Saves the settings to the registry 
        {
            /*
             * Key: HKEY_CURRENT_USER\Software\Keppy's Synthesizer\Settings\
             */
            try
            {
                // Normal settings
                KeppySynthConfiguratorMain.SynthSettings.SetValue("polyphony", KeppySynthConfiguratorMain.Delegate.PolyphonyLimit.Value.ToString(), RegistryValueKind.DWord);
                KeppySynthConfiguratorMain.SynthSettings.SetValue("cpu", KeppySynthConfiguratorMain.Delegate.MaxCPU.Value, RegistryValueKind.DWord);
                KeppySynthConfiguratorMain.SynthSettings.SetValue("frequency", KeppySynthConfiguratorMain.Delegate.Frequency.Text, RegistryValueKind.DWord);

                // Advanced SynthSettings
                KeppySynthConfiguratorMain.SynthSettings.SetValue("buflen", KeppySynthConfiguratorMain.Delegate.bufsize.Value.ToString(), RegistryValueKind.DWord);

                // Let's not forget about the volume!
                KeppySynthConfiguratorMain.SynthSettings.SetValue("volume", KeppySynthConfiguratorMain.Delegate.VolTrackBar.Value.ToString(), RegistryValueKind.DWord);

                // Checkbox stuff yay
                if (KeppySynthConfiguratorMain.Delegate.Preload.Checked == true)
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("preload", "1", RegistryValueKind.DWord);
                else
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("preload", "0", RegistryValueKind.DWord);

                if (KeppySynthConfiguratorMain.Delegate.EnableSFX.Checked == true)
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("nofx", "0", RegistryValueKind.DWord);
                else
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("nofx", "1", RegistryValueKind.DWord);

                if (KeppySynthConfiguratorMain.Delegate.NoteOffCheck.Checked == true)
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("noteoff", "1", RegistryValueKind.DWord);            
                else
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("noteoff", "0", RegistryValueKind.DWord);

                if (KeppySynthConfiguratorMain.Delegate.SysResetIgnore.Checked == true)
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("sysresetignore", "1", RegistryValueKind.DWord);
                else
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("sysresetignore", "0", RegistryValueKind.DWord);

                if (KeppySynthConfiguratorMain.Delegate.AudioEngBox.SelectedIndex == 0)
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("xaudiodisabled", "0", RegistryValueKind.DWord);
                else if (KeppySynthConfiguratorMain.Delegate.AudioEngBox.SelectedIndex == 1)
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("xaudiodisabled", "1", RegistryValueKind.DWord);
                else if (KeppySynthConfiguratorMain.Delegate.AudioEngBox.SelectedIndex == 2)
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("xaudiodisabled", "2", RegistryValueKind.DWord);
                else if (KeppySynthConfiguratorMain.Delegate.AudioEngBox.SelectedIndex == 3)
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("xaudiodisabled", "3", RegistryValueKind.DWord);

                if (KeppySynthConfiguratorMain.Delegate.SincInter.Checked == true)
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("sinc", "1", RegistryValueKind.DWord);
                else
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("sinc", "0", RegistryValueKind.DWord);

                KeppySynthConfiguratorMain.SynthSettings.SetValue("sincconv", KeppySynthConfiguratorMain.Delegate.SincConv.SelectedIndex, RegistryValueKind.DWord);

                if (Override)
                {
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("livechange", "1", RegistryValueKind.DWord);
                    LiveChanges.PreviousEngine = (int)KeppySynthConfiguratorMain.SynthSettings.GetValue("xaudiodisabled", 0);
                    LiveChanges.PreviousFrequency = (int)KeppySynthConfiguratorMain.SynthSettings.GetValue("frequency", 44100);
                    LiveChanges.PreviousBuffer = (int)KeppySynthConfiguratorMain.SynthSettings.GetValue("buflen", 50);
                    LiveChanges.MonophonicRender = (int)KeppySynthConfiguratorMain.SynthSettings.GetValue("monorendering", 0);
                    LiveChanges.AudioBitDepth = (int)KeppySynthConfiguratorMain.SynthSettings.GetValue("32bit", 1);
                    Program.DebugToConsole(false, "Done saving settings with force reload.", null);
                }
                else
                {
                    if (Properties.Settings.Default.LiveChanges)
                    {
                        if (LiveChanges.PreviousEngine != (int)KeppySynthConfiguratorMain.SynthSettings.GetValue("xaudiodisabled", 0) ||
                            LiveChanges.PreviousFrequency != (int)KeppySynthConfiguratorMain.SynthSettings.GetValue("frequency", 44100) ||
                            LiveChanges.PreviousBuffer != (int)KeppySynthConfiguratorMain.SynthSettings.GetValue("buflen", 50) ||
                            LiveChanges.MonophonicRender != (int)KeppySynthConfiguratorMain.SynthSettings.GetValue("monorendering", 0) ||
                            LiveChanges.AudioBitDepth != (int)KeppySynthConfiguratorMain.SynthSettings.GetValue("32bit", 1))
                        {
                            KeppySynthConfiguratorMain.SynthSettings.SetValue("livechange", "1", RegistryValueKind.DWord);
                            LiveChanges.PreviousEngine = (int)KeppySynthConfiguratorMain.SynthSettings.GetValue("xaudiodisabled", 0);
                            LiveChanges.PreviousFrequency = (int)KeppySynthConfiguratorMain.SynthSettings.GetValue("frequency", 44100);
                            LiveChanges.PreviousBuffer = (int)KeppySynthConfiguratorMain.SynthSettings.GetValue("buflen", 50);
                            LiveChanges.MonophonicRender = (int)KeppySynthConfiguratorMain.SynthSettings.GetValue("monorendering", 0);
                            LiveChanges.AudioBitDepth = (int)KeppySynthConfiguratorMain.SynthSettings.GetValue("32bit", 1);
                            Program.DebugToConsole(false, "Done saving settings. Restarting streams if any is present...", null);
                        }
                        else Program.DebugToConsole(false, "Done saving settings.", null);
                    }
                    else Program.DebugToConsole(false, "Done saving settings.", null);
                }

                return true;
            }
            catch (Exception ex)
            {
                Functions.ShowErrorDialog(1, System.Media.SystemSounds.Hand, "Error", "An error has occurred while saving the driver's settings.", true, ex);
                Program.DebugToConsole(true, null, ex);
                ReinitializeSettings(thisform);
                return false;
            }
        }

        public static void ReinitializeSettings(Form thisform) // If the registry is missing, reset it
        {
            /*
             * Key: HKEY_CURRENT_USER\Software\Keppy's Synthesizer\Settings\
             */
            try
            {
                // Initialize the registry
                Registry.CurrentUser.CreateSubKey("SOFTWARE\\Keppy's Synthesizer\\Settings");
                Registry.CurrentUser.CreateSubKey("SOFTWARE\\Keppy's Synthesizer\\Watchdog");
                Registry.CurrentUser.CreateSubKey("SOFTWARE\\Keppy's Synthesizer\\Paths");
                KeppySynthConfiguratorMain.SynthSettings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Settings", true);
                KeppySynthConfiguratorMain.Watchdog = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Settings", true);
                KeppySynthConfiguratorMain.SynthPaths = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Settings", true);

                // Normal settings
                KeppySynthConfiguratorMain.SynthSettings.SetValue("polyphony", "500", RegistryValueKind.DWord);
                KeppySynthConfiguratorMain.SynthSettings.SetValue("cpu", "65", RegistryValueKind.DWord);
                KeppySynthConfiguratorMain.Delegate.Frequency.Text = "48000";
                KeppySynthConfiguratorMain.SynthSettings.SetValue("frequency", KeppySynthConfiguratorMain.Delegate.Frequency.Text, RegistryValueKind.DWord);

                // Advanced SynthSettings
                KeppySynthConfiguratorMain.SynthSettings.SetValue("buflen", "30", RegistryValueKind.DWord);
                KeppySynthConfiguratorMain.SynthSettings.SetValue("sndbfvalue", "100", RegistryValueKind.DWord);

                // Let's not forget about the volume!
                int VolumeValue = 0;
                KeppySynthConfiguratorMain.Delegate.VolTrackBar.Value = 10000;
                double x = KeppySynthConfiguratorMain.Delegate.VolTrackBar.Value / 100;
                KeppySynthConfiguratorMain.Delegate.VolSimView.Text = String.Format("{0}", Math.Round(x, MidpointRounding.AwayFromZero).ToString());
                KeppySynthConfiguratorMain.SynthSettings.SetValue("volume", KeppySynthConfiguratorMain.Delegate.VolTrackBar.Value.ToString(), RegistryValueKind.DWord);

                // Checkbox stuff yay
                KeppySynthConfiguratorMain.SynthSettings.SetValue("preload", "1", RegistryValueKind.DWord);
                KeppySynthConfiguratorMain.SynthSettings.SetValue("nofx", "0", RegistryValueKind.DWord);
                KeppySynthConfiguratorMain.SynthSettings.SetValue("vmsemu", "0", RegistryValueKind.DWord);
                KeppySynthConfiguratorMain.SynthSettings.SetValue("noteoff", "0", RegistryValueKind.DWord);
                KeppySynthConfiguratorMain.SynthSettings.SetValue("sysresetignore", "0", RegistryValueKind.DWord);
                KeppySynthConfiguratorMain.SynthSettings.SetValue("encmode", "0", RegistryValueKind.DWord);
                KeppySynthConfiguratorMain.SynthSettings.SetValue("sinc", "0", RegistryValueKind.DWord);
                KeppySynthConfiguratorMain.SynthSettings.SetValue("xaudiodisabled", "3", RegistryValueKind.DWord);

                // Reload the settings
                LoadSettings(thisform);
            }
            catch (Exception ex)
            {
                // Something bad happened hehe
                Program.DebugToConsole(true, null, ex);
                Functions.ShowErrorDialog(1, System.Media.SystemSounds.Hand, "Fatal error", "Missing registry settings!\nPlease reinstall Keppy's Synthesizer to solve the issue.\n\nPress OK to quit.", true, ex);
                System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(ignored =>
                {
                    KeppySynthConfiguratorMain.ActiveForm.Hide();
                    throw new Exception();
                }));
            }
        }

        public static void ImportSettings(Form thisform, String filename)
        {
            try
            {
                string line = File.ReadLines(filename).Skip(2).Take(1).First();

                if (line == "; Keppy's Synthesizer Settings File")
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = "reg.exe";
                    startInfo.Arguments = String.Format("import \"{0}\"", filename);
                    startInfo.RedirectStandardOutput = true;
                    startInfo.RedirectStandardError = true;
                    startInfo.UseShellExecute = false;
                    startInfo.CreateNoWindow = true;

                    Process processTemp = new Process();
                    processTemp.StartInfo = startInfo;
                    processTemp.EnableRaisingEvents = true;
                    processTemp.Start();
                    processTemp.WaitForExit();

                    Functions.LoadSettings(thisform);
                }
                else
                {
                    MessageBox.Show("Invalid registry file!\n\nThis file doesn't contain valid Keppy's Synthesizer settings!!!", "Keppy's Synthesizer - Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            catch (Exception ex)
            {
                // Something bad happened hehe
                Program.DebugToConsole(true, null, ex);
                Functions.ShowErrorDialog(2, System.Media.SystemSounds.Hand, "Fatal error", "Fatal error during the execution of this program!\n\nPress OK to quit.", true, ex);
                Application.Exit();
            }
        }

        // Presets system

        /// <summary>
        /// Applies the values for the presets
        /// </summary>
        /// <param name="volume">Set the volume</param>
        /// <param name="voices">Set the maximum voices</param>
        /// <param name="maxcpu">Set the maximum rendering time allowed</param>
        /// <param name="frequency">Change the frequency</param>
        /// <param name="bufsize">Change the buffer value</param>
        /// <param name="preload">Enable or disable the soundfont preload</param>
        /// <param name="noteoffcheck">Enable or disable the check for oldest instances of notes</param>
        /// <param name="sincinter">Enable or disable the sinc interpolation</param>
        /// <param name="sincconv">Set the quality level of the sinc interpolation. 0 = Linear, 1 = 8 point, 2 = 16 points, 3 = 32 points</param>
        /// <param name="enablesfx">Enable or disable the audio effects</param>
        /// <param name="sysresetignore">Ignore or accept SysEx events</param>
        /// <param name="outputwav">Enable or disable the output to WAV mode</param>
        /// <param name="audioengine">Select the audio engine. 0 = XAudio2, 1 = DirectSound, 2 = ASIO, 3 = WASAPI</param>
        public static void ApplyPresetValues(
            int volume, int voices, int maxcpu, int frequency, int bufsize,
            bool preload, bool noteoffcheck, bool sincinter, int sincconv, 
            bool enablesfx, bool sysresetignore, bool outputwav, int audioengine)
        {
            KeppySynthConfiguratorMain.Delegate.VolTrackBar.Value = volume;
            KeppySynthConfiguratorMain.Delegate.PolyphonyLimit.Value = voices;
            KeppySynthConfiguratorMain.Delegate.MaxCPU.Value = maxcpu;
            KeppySynthConfiguratorMain.Delegate.Frequency.Text = frequency.ToString();
            KeppySynthConfiguratorMain.Delegate.Preload.Checked = preload;
            KeppySynthConfiguratorMain.Delegate.NoteOffCheck.Checked = noteoffcheck;
            KeppySynthConfiguratorMain.Delegate.SincInter.Checked = sincinter;
            KeppySynthConfiguratorMain.Delegate.SincConv.SelectedIndex = sincconv;
            KeppySynthConfiguratorMain.Delegate.EnableSFX.Checked = enablesfx;
            KeppySynthConfiguratorMain.Delegate.SysResetIgnore.Checked = sysresetignore;
            KeppySynthConfiguratorMain.Delegate.AudioEngBox.SelectedIndex = audioengine;
            KeppySynthConfiguratorMain.Delegate.bufsize.Value = bufsize;
        }

        /// <summary>
        /// Changes the advanced audio settings automatically
        /// </summary>
        /// <param name="audiodepth">Set the audio depth of the stream. 1 = 32-bit float, 2 = 16-bit integer, 3 = 8-bit integer</param>
        /// <param name="monorendering">Set the stream to only output to one audio channel. 0 = Disabled, 1 = Enabled</param>
        /// <param name="fadeoutdisable">Set the fade out for when a note gets killed. 0 = Disabled, 1 = Enabled</param>
        /// <param name="vms2emu">Set if the driver has to emulate VirtualMIDISynth 2.x (Example: Slowdowns when the EVBuffer is full). 0 = Disabled, 1 = Enabled</param>
        /// <param name="oldbuffermode">Set if the driver should use the old buffer mode (Only DirectSound and XAudio). 0 = Disabled, 1 = Enabled</param>
        /// <param name="sleepstates">Set if the driver should disable sleepstates (Only DirectSound). 0 = Disable them, 1 = Keep them enabled</param>
        public static void ChangeAdvancedAudioSettings(int audiodepth, int monorendering, int fadeoutdisable, int vms2emu, int oldbuffermode, int sleepstates)
        {
            KeppySynthConfiguratorMain.SynthSettings.SetValue("32bit", audiodepth, RegistryValueKind.DWord);
            KeppySynthConfiguratorMain.SynthSettings.SetValue("monorendering", monorendering, RegistryValueKind.DWord);
            KeppySynthConfiguratorMain.SynthSettings.SetValue("fadeoutdisable", fadeoutdisable, RegistryValueKind.DWord);
            KeppySynthConfiguratorMain.SynthSettings.SetValue("vms2emu", vms2emu, RegistryValueKind.DWord);
            Functions.OldBufferMode(oldbuffermode);
            Functions.SleepStates(sleepstates);
        }

        /// <summary>
        /// Changes the MIDI Event Parser settings automatically
        /// </summary>
        /// <param name="capframerate">Set if the driver should cap the input framerate to 60FPS. 0 = Disabled, 1 = Enabled</param>
        /// <param name="getbuffbyram">Set if the driver should get the EVBuffer size from the RAM. 0 = Disabled, 1 = Enabled</param>
        /// <param name="buffsize">Set the EVBuffer size (Only when "evbuffbyram" is set to 0).</param>
        /// <param name="buffratio">Set the EVBuffer division ratio (Only when "evbuffbyram" is set to 1).</param>
        public static void ChangeMIDIEventParserSettings(int capframerate, int limit88, int mt32mode, int getbuffbyram, int buffsize, int buffratio)
        {
            MEMORYSTATUSEX memStatus = new MEMORYSTATUSEX();
            if (!GlobalMemoryStatusEx(memStatus)) MessageBox.Show("Unknown error.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            Functions.SetFramerate(capframerate);
            KeppySynthConfiguratorMain.SynthSettings.SetValue("evbuffbyram", getbuffbyram, Microsoft.Win32.RegistryValueKind.DWord);

            if (buffsize < 1) KeppySynthConfiguratorMain.SynthSettings.SetValue("evbuffsize", memStatus.ullTotalPhys, Microsoft.Win32.RegistryValueKind.QWord);
            else KeppySynthConfiguratorMain.SynthSettings.SetValue("evbuffsize", buffsize, Microsoft.Win32.RegistryValueKind.QWord);

            if (buffsize > 1) KeppySynthConfiguratorMain.SynthSettings.SetValue("evbuffratio", "1", Microsoft.Win32.RegistryValueKind.DWord);
            else KeppySynthConfiguratorMain.SynthSettings.SetValue("evbuffratio", buffratio, Microsoft.Win32.RegistryValueKind.DWord);
        }

        /// <summary>
        /// Changes the driver's mask values automatically
        /// </summary>
        /// <param name="maskname">Set the mask name.</param>
        /// <param name="masktype">Set the mask type. 0 = FM, 1 = Generic synth, 2 = Hardware synth, 3 = MIDI Mapper, 4 = Output port, 5 = Software synth, 6 = Square wave synth</param>
        public static void ChangeDriverMask(string maskname, int masktype, int vid, int pid)
        {
            KeppySynthConfiguratorMain.SynthSettings.SetValue("synthname", maskname, RegistryValueKind.String);
            KeppySynthConfiguratorMain.SynthSettings.SetValue("synthtype", masktype, RegistryValueKind.DWord);
            KeppySynthConfiguratorMain.SynthSettings.SetValue("vid", vid, RegistryValueKind.DWord);
            KeppySynthConfiguratorMain.SynthSettings.SetValue("pid", pid, RegistryValueKind.DWord);
        }

        public static void ImportPreset(Form thisform)
        {
            String PresetTitle = "";
            Boolean dummy = false;
            try
            {
                if (KeppySynthConfiguratorMain.Delegate.ImportPresetDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (string x in File.ReadLines(KeppySynthConfiguratorMain.Delegate.ImportPresetDialog.FileName, Encoding.UTF8))
                    {
                        try
                        {
                            // Normal settings
                            if (SettingName(x) == "ConfName") PresetTitle = SettingValue(x);
                            else if (SettingName(x) == "AudioEngine") KeppySynthConfiguratorMain.Delegate.AudioEngBox.SelectedIndex = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "BufferLength") KeppySynthConfiguratorMain.Delegate.bufsize.Value = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "Frequency") KeppySynthConfiguratorMain.Delegate.Frequency.Text = SettingValue(x);
                            else if (SettingName(x) == "IgnoreSysEx") KeppySynthConfiguratorMain.Delegate.SysResetIgnore.Checked = Boolean.TryParse(SettingValue(x), out dummy);
                            else if (SettingName(x) == "MaxCPU") KeppySynthConfiguratorMain.Delegate.MaxCPU.Value = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "NoteOff") KeppySynthConfiguratorMain.Delegate.NoteOffCheck.Checked = Boolean.TryParse(SettingValue(x), out dummy);
                            else if (SettingName(x) == "Preload") KeppySynthConfiguratorMain.Delegate.Preload.Checked = Boolean.TryParse(SettingValue(x), out dummy);
                            else if (SettingName(x) == "SincInter") KeppySynthConfiguratorMain.Delegate.SincInter.Checked = Boolean.TryParse(SettingValue(x), out dummy);
                            else if (SettingName(x) == "SincConv") KeppySynthConfiguratorMain.Delegate.SincConv.SelectedIndex = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "VoiceLimit") KeppySynthConfiguratorMain.Delegate.PolyphonyLimit.Value = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "Volume") KeppySynthConfiguratorMain.Delegate.VolTrackBar.Value = Convert.ToInt32(SettingValue(x));
                            // Advanced audio settings
                            else if (SettingName(x) == "AudioDepth") KeppySynthConfiguratorMain.SynthSettings.SetValue("32bit", SettingValue(x), RegistryValueKind.DWord);
                            else if (SettingName(x) == "FadeOutDisable") KeppySynthConfiguratorMain.SynthSettings.SetValue("fadeoutdisable", SettingValue(x), RegistryValueKind.DWord);
                            else if (SettingName(x) == "MonoRendering") KeppySynthConfiguratorMain.SynthSettings.SetValue("monorendering", SettingValue(x), RegistryValueKind.DWord);
                            else if (SettingName(x) == "SlowdownPlayback") KeppySynthConfiguratorMain.SynthSettings.SetValue("vms2emu", SettingValue(x), RegistryValueKind.DWord);

                            // MIDI events parser settings
                            else if (SettingName(x) == "CapFramerate") KeppySynthConfiguratorMain.SynthSettings.SetValue("capframerate", SettingValue(x), RegistryValueKind.DWord);
                            else if (SettingName(x) == "EVBufferByRAM") KeppySynthConfiguratorMain.SynthSettings.SetValue("evbuffbyram", SettingValue(x), RegistryValueKind.DWord);
                            else if (SettingName(x) == "EVBufferRatio") KeppySynthConfiguratorMain.SynthSettings.SetValue("evbuffratio", SettingValue(x), RegistryValueKind.DWord);
                            else if (SettingName(x) == "EVBufferSize") KeppySynthConfiguratorMain.SynthSettings.SetValue("evbuffsize", SettingValue(x), RegistryValueKind.DWord);
                            else if (SettingName(x) == "FullVelocityMode") KeppySynthConfiguratorMain.SynthSettings.SetValue("fullvelocity", SettingValue(x), RegistryValueKind.DWord);
                            else if (SettingName(x) == "IgnoreAllNotes") KeppySynthConfiguratorMain.SynthSettings.SetValue("allnotesignore", SettingValue(x), RegistryValueKind.DWord);
                            else if (SettingName(x) == "IgnoreNotes") KeppySynthConfiguratorMain.SynthSettings.SetValue("ignorenotes1", SettingValue(x), RegistryValueKind.DWord);
                            else if (SettingName(x) == "Limit88") KeppySynthConfiguratorMain.SynthSettings.SetValue("limit88", SettingValue(x), RegistryValueKind.DWord);
                            else if (SettingName(x) == "MT32Mode") KeppySynthConfiguratorMain.SynthSettings.SetValue("mt32mode", SettingValue(x), RegistryValueKind.DWord);
                            else if (SettingName(x) == "SysExIgnore") KeppySynthConfiguratorMain.SynthSettings.SetValue("sysexignore", SettingValue(x), RegistryValueKind.DWord);

                            // Driver mask
                            else if (SettingName(x) == "SynthName") KeppySynthConfiguratorMain.SynthSettings.SetValue("synthname", SettingValue(x), RegistryValueKind.String);
                            else if (SettingName(x) == "SynthType") KeppySynthConfiguratorMain.SynthSettings.SetValue("synthtype", SettingValue(x), RegistryValueKind.DWord);
                            else if (SettingName(x) == "VID") KeppySynthConfiguratorMain.SynthSettings.SetValue("vid", SettingValue(x), RegistryValueKind.DWord);
                            else if (SettingName(x) == "PID") KeppySynthConfiguratorMain.SynthSettings.SetValue("pid", SettingValue(x), RegistryValueKind.DWord);
                        }
                        catch (Exception ex) {
                            Functions.ShowErrorDialog(2, System.Media.SystemSounds.Hand, "Error", "Invalid preset!", false, ex);

                            // Set some values...
                            Functions.ApplyPresetValues(10000, 500, 75, 44100, 20, true, false, false, 2, true, false, false, 3);

                            // Advanced settings here...
                            Functions.ChangeAdvancedAudioSettings(1, 0, 0, 0, 0, 1);
                            Functions.ChangeMIDIEventParserSettings(0, 0, 0, 0, 16384, 1);
                            Functions.ChangeDriverMask("Keppy's Synthesizer", 4, 0xFFFF, 0x000A);

                            // And then...
                            Functions.SaveSettings(thisform, true);

                            // Messagebox here
                            Program.DebugToConsole(false, "Settings restored.", null);

                            return;
                        }
                    }

                    MessageBox.Show(
                        String.Format("The preset \"{0}\" has been applied.", Path.GetFileNameWithoutExtension(KeppySynthConfiguratorMain.Delegate.ImportPresetDialog.FileName)),
                        "Keppy's Synthesizer - Import preset", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // Something bad happened hehe
                Program.DebugToConsole(true, null, ex);
                Functions.ShowErrorDialog(2, System.Media.SystemSounds.Hand, "Fatal error", "Fatal error during the execution of the program.\n\nPress OK to quit", true, ex);
                Application.Exit();
            }
        }

        public static void ExportPreset()
        {
            try
            {
                if (KeppySynthConfiguratorMain.Delegate.ExportPresetDialog.ShowDialog() == DialogResult.OK)
                {
                    StringBuilder SettingsToText = new StringBuilder();

                    SettingsToText.AppendLine(String.Format("ConfName = {0}", Path.GetFileNameWithoutExtension(KeppySynthConfiguratorMain.Delegate.ExportPresetDialog.FileName)));
                    SettingsToText.AppendLine();
                    SettingsToText.AppendLine("// Normal settings");
                    SettingsToText.AppendLine(String.Format("AudioEngine = {0}", KeppySynthConfiguratorMain.Delegate.AudioEngBox.SelectedIndex));
                    SettingsToText.AppendLine(String.Format("BufferLength = {0}", KeppySynthConfiguratorMain.Delegate.bufsize.Value));
                    SettingsToText.AppendLine(String.Format("Frequency = {0}", KeppySynthConfiguratorMain.Delegate.Frequency.Text));
                    SettingsToText.AppendLine(String.Format("IgnoreSysEx = {0}", KeppySynthConfiguratorMain.Delegate.SysResetIgnore.Checked));
                    SettingsToText.AppendLine(String.Format("MaxCPU = {0}", KeppySynthConfiguratorMain.Delegate.MaxCPU.Value));
                    SettingsToText.AppendLine(String.Format("NoteOff = {0}", KeppySynthConfiguratorMain.Delegate.NoteOffCheck.Checked));
                    SettingsToText.AppendLine(String.Format("Preload = {0}", KeppySynthConfiguratorMain.Delegate.Preload.Checked));
                    SettingsToText.AppendLine(String.Format("SincInter = {0}", KeppySynthConfiguratorMain.Delegate.SincInter.Checked));
                    SettingsToText.AppendLine(String.Format("SincConv = {0}", KeppySynthConfiguratorMain.Delegate.SincConv.SelectedIndex));
                    SettingsToText.AppendLine(String.Format("VoiceLimit = {0}", KeppySynthConfiguratorMain.Delegate.PolyphonyLimit.Value));
                    SettingsToText.AppendLine(String.Format("Volume = {0}", KeppySynthConfiguratorMain.Delegate.VolTrackBar.Value));
                    SettingsToText.AppendLine();
                    SettingsToText.AppendLine("// Advanced audio settings");
                    SettingsToText.AppendLine(String.Format("AudioDepth = {0}", KeppySynthConfiguratorMain.SynthSettings.GetValue("32bit", 1)));
                    SettingsToText.AppendLine(String.Format("FadeOutDisable = {0}", KeppySynthConfiguratorMain.SynthSettings.GetValue("fadeoutdisable", 0)));
                    SettingsToText.AppendLine(String.Format("MonoRendering = {0}", KeppySynthConfiguratorMain.SynthSettings.GetValue("monorendering", 0)));
                    SettingsToText.AppendLine(String.Format("SlowdownPlayback = {0}", KeppySynthConfiguratorMain.SynthSettings.GetValue("vms2emu", 0)));
                    SettingsToText.AppendLine();
                    SettingsToText.AppendLine("// MIDI events parser settings");
                    SettingsToText.AppendLine(String.Format("CapFramerate = {0}", KeppySynthConfiguratorMain.SynthSettings.GetValue("capframerate", 0)));
                    SettingsToText.AppendLine(String.Format("EVBufferByRAM = {0}", KeppySynthConfiguratorMain.SynthSettings.GetValue("evbuffbyram", 0)));
                    SettingsToText.AppendLine(String.Format("EVBufferRatio = {0}", KeppySynthConfiguratorMain.SynthSettings.GetValue("evbuffratio", 1)));
                    SettingsToText.AppendLine(String.Format("EVBufferSize = {0}", KeppySynthConfiguratorMain.SynthSettings.GetValue("evbuffsize", 16384)));
                    SettingsToText.AppendLine(String.Format("FullVelocityMode = {0}", KeppySynthConfiguratorMain.SynthSettings.GetValue("fullvelocity", 0)));
                    SettingsToText.AppendLine(String.Format("IgnoreAllNotes = {0}", KeppySynthConfiguratorMain.SynthSettings.GetValue("allnotesignore", 0)));
                    SettingsToText.AppendLine(String.Format("IgnoreNotes = {0}", KeppySynthConfiguratorMain.SynthSettings.GetValue("ignorenotes1", 1)));
                    SettingsToText.AppendLine(String.Format("Limit88 = {0}", KeppySynthConfiguratorMain.SynthSettings.GetValue("limit88", 0)));
                    SettingsToText.AppendLine(String.Format("MT32Mode = {0}", KeppySynthConfiguratorMain.SynthSettings.GetValue("mt32mode", 0)));
                    SettingsToText.AppendLine(String.Format("SysExIgnore = {0}", KeppySynthConfiguratorMain.SynthSettings.GetValue("sysexignore", 0)));
                    SettingsToText.AppendLine();
                    SettingsToText.AppendLine("// Driver mask");
                    SettingsToText.AppendLine(String.Format("SynthName = {0}", KeppySynthConfiguratorMain.SynthSettings.GetValue("synthname", "Keppy's Synthesizer")));
                    SettingsToText.AppendLine(String.Format("SynthType = {0}", KeppySynthConfiguratorMain.SynthSettings.GetValue("synthtype", 4)));
                    SettingsToText.AppendLine(String.Format("VID = {0}", KeppySynthConfiguratorMain.SynthSettings.GetValue("vid", 0xFFFF)));
                    SettingsToText.AppendLine(String.Format("PID = {0}", KeppySynthConfiguratorMain.SynthSettings.GetValue("pid", 0x000A)));

                    File.WriteAllText(KeppySynthConfiguratorMain.Delegate.ExportPresetDialog.FileName, SettingsToText.ToString());

                    MessageBox.Show(
                        String.Format("The preset \"{0}\" has been saved to:\n\n{1}",
                        Path.GetFileNameWithoutExtension(KeppySynthConfiguratorMain.Delegate.ExportPresetDialog.FileName),
                        KeppySynthConfiguratorMain.Delegate.ExportPresetDialog.FileName), 
                        "Keppy's Synthesizer - Export preset", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // Something bad happened hehe
                Program.DebugToConsole(true, null, ex);
                Functions.ShowErrorDialog(2, System.Media.SystemSounds.Hand, "Fatal error", "Fatal error during the execution of the program.\n\nPress OK to quit", true, ex);
                Application.Exit();
            }
        }

        /// <summary>
        /// Set setting's name
        /// </summary>
        public static string SettingName(string value)
        {
            int A = value.IndexOf(" = ");
            if (A == -1) return "";
            return value.Substring(0, A);
        }

        /// <summary>
        /// Get setting's value
        /// </summary>
        public static string SettingValue(string value)
        {
            int A = value.LastIndexOf(" = ");
            if (A == -1) return "";
            int A2 = A + (" = ").Length;
            if (A2 >= value.Length) return "";
            return value.Substring(A2);
        }

        // End of the preset system

        public static void ExportSettings(String filename)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Windows Registry Editor Version 5.00");

                sb.AppendLine("");
                sb.AppendLine("; Keppy's Synthesizer Settings File");
                sb.AppendLine("");

                sb.AppendLine(@"[HKEY_CURRENT_USER\SOFTWARE\Keppy's Synthesizer\Settings]");
                foreach (var keyname in KeppySynthConfiguratorMain.SynthSettings.GetValueNames())
                {
                    if (Regex.IsMatch(KeppySynthConfiguratorMain.SynthSettings.GetValue(keyname).ToString(), @"[a-zA-Z]"))
                        sb.AppendLine(String.Format("\"{0}\"={1}:{2}", keyname, KeppySynthConfiguratorMain.SynthSettings.GetValueKind(keyname).ToString().ToLower(), KeppySynthConfiguratorMain.SynthSettings.GetValue(keyname)));
                    else if (Regex.IsMatch(KeppySynthConfiguratorMain.SynthSettings.GetValue(keyname).ToString(), @"\d+"))
                        sb.AppendLine(String.Format("\"{0}\"={1}:{2}", keyname, KeppySynthConfiguratorMain.SynthSettings.GetValueKind(keyname).ToString().ToLower(), Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue(keyname)).ToString("X")));
                    else
                        Program.DebugToConsole(false, String.Format("Unknown value detected on {0}", keyname), null);
                }

                sb.AppendLine("");

                sb.AppendLine(@"[HKEY_CURRENT_USER\SOFTWARE\Keppy's Synthesizer\Paths]");
                foreach (var keyname in KeppySynthConfiguratorMain.SynthPaths.GetValueNames())
                {
                    sb.AppendLine(String.Format("\"{0}\"=\"{1}\"", keyname, KeppySynthConfiguratorMain.SynthPaths.GetValue(keyname).ToString()));
                }

                sb.AppendLine(Environment.NewLine);

                File.WriteAllText(filename, sb.ToString());

                Program.DebugToConsole(false, String.Format("Settings exported to: {0}", filename), null);
            }
            catch (Exception ex)
            {
                // Something bad happened hehe
                Program.DebugToConsole(true, null, ex);
                Functions.ShowErrorDialog(2, System.Media.SystemSounds.Hand, "Fatal error", "Fatal error during the execution of the program.\n\nPress OK to quit", true, ex);
                Application.Exit();
            }
        }

        public static void OpenAdvancedAudioSettings(String TabToOpen, String ErrorNoWork)
        {
            try
            {
                BASS_DEVICEINFO info = new BASS_DEVICEINFO();
                String DeviceID = "0";
                Bass.BASS_GetDeviceInfo(0, info);
                for (int n = 0; Bass.BASS_GetDeviceInfo(n, info); n++)
                {
                    if (info.IsDefault == true)
                    {
                        DeviceID = info.driver;
                        break;
                    }
                }
                Process.Start(
                    @"C:\Windows\System32\rundll32.exe",
                    String.Format(@"C:\Windows\System32\shell32.dll,Control_RunDLL C:\Windows\System32\mmsys.cpl ms-mmsys:,{0},{1}", DeviceID, TabToOpen));
            }
            catch
            {
                MessageBox.Show(ErrorNoWork, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void ButtonStatus(Boolean Status)
        {
            KeppySynthConfiguratorMain.Delegate.RTPrio.Enabled = Status;
            KeppySynthConfiguratorMain.Delegate.HiPrio.Enabled = Status;
            KeppySynthConfiguratorMain.Delegate.HNPrio.Enabled = Status;
            KeppySynthConfiguratorMain.Delegate.NoPrio.Enabled = Status;
            KeppySynthConfiguratorMain.Delegate.LNPrio.Enabled = Status;
            KeppySynthConfiguratorMain.Delegate.LoPrio.Enabled = Status;
        }

        public static string ReturnLength(long length, bool apx)
        {
            try
            {
                if (length >= 1099511627776)
                {
                    if (length >= 1099511627776 && length < 10995116277760)
                        return ((((length / 1024f) / 1024f) / 1024f) / 1024f).ToString(apx ? "0.0 TB~" : "0.00 TB");
                    else
                        return ((((length / 1024f) / 1024f) / 1024f) / 1024f).ToString(apx ? "0 TB~" : "0.0 TB");
                }
                else if (length >= 1073741824)
                {
                    if (length >= 1073741824 && length < 10737418240)
                        return (((length / 1024f) / 1024f) / 1024f).ToString(apx ? "0.0 GB~" : "0.00 GB");
                    else
                        return (((length / 1024f) / 1024f) / 1024f).ToString(apx ? "0 GB~" : "0.0 GB");
                }
                else if (length >= 1048576)
                {
                    if (length >= 1048576 && length < 10485760)
                        return ((length / 1024f) / 1024f).ToString(apx ? "0.0 MB~" : "0.00 MB");
                    else
                        return ((length / 1024f) / 1024f).ToString(apx ? "0 MB~" : "0.0 MB");
                }
                else if (length >= 1024)
                {
                    if (length >= 1024 && length < 10240)
                        return (length / 1024f).ToString(apx ? "0.0 kB~" : "0.00 kB");
                    else
                        return (length / 1024f).ToString(apx ? "0 kB~" : "0.0 kB");
                }
                else
                {
                    if (length >= 1 && length < 1024)
                        return (length).ToString(apx ? "0.0 B~" : "0.00 B");
                    else
                        return (length / 1024f).ToString(apx ? "0 B~" : "0.0 B");
                }
            }
            catch { return "-"; }
        }

        public static void SetAssociation()
        {
            string ExecutableName = Path.GetFileName(Application.ExecutablePath);
            string OpenWith = Application.ExecutablePath;
            string[] extensions = { "sf2", "sfz", "sfpack" };
            try
            {
                foreach (string ext in extensions)
                {
                    Program.DebugToConsole(false, String.Format("Setting association for {0} files...", ext), null);
                    using (RegistryKey User_Classes = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Classes\\", true))
                    using (RegistryKey User_Ext = User_Classes.CreateSubKey("." + ext))
                    using (RegistryKey User_AutoFile = User_Classes.CreateSubKey(ext + "_auto_file"))
                    using (RegistryKey User_AutoFile_Command = User_AutoFile.CreateSubKey("shell").CreateSubKey("open").CreateSubKey("command"))
                    using (RegistryKey ApplicationAssociationToasts = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\ApplicationAssociationToasts\\", true))
                    using (RegistryKey User_Classes_Applications = User_Classes.CreateSubKey("Applications"))
                    using (RegistryKey User_Classes_Applications_Exe = User_Classes_Applications.CreateSubKey(ExecutableName))
                    using (RegistryKey User_Application_Command = User_Classes_Applications_Exe.CreateSubKey("shell").CreateSubKey("open").CreateSubKey("command"))
                    using (RegistryKey User_Explorer = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\." + ext))
                    using (RegistryKey User_Choice = User_Explorer.OpenSubKey("UserChoice"))
                    {
                        User_Classes_Applications_Exe.SetValue("", "SoundFont file", RegistryValueKind.String);
                        User_Ext.SetValue("", ext + "_auto_file", RegistryValueKind.String);
                        User_Classes.SetValue("", ext + "_auto_file", RegistryValueKind.String);
                        User_Classes.CreateSubKey(ext + "_auto_file");
                        User_AutoFile_Command.SetValue("", "\"" + OpenWith + "\"" + " \"%1\"");
                        ApplicationAssociationToasts.SetValue(ext + "_auto_file_." + ext, 0);
                        ApplicationAssociationToasts.SetValue(@"Applications\" + ext + "_." + ext, 0);
                        User_Application_Command.SetValue("", "\"" + OpenWith + "\"" + " \"%1\"");
                        User_Explorer.CreateSubKey("OpenWithList").SetValue("a", ExecutableName);
                        User_Explorer.CreateSubKey("OpenWithProgids").SetValue(ext + "_auto_file", "0");
                        if (User_Choice != null) User_Explorer.DeleteSubKey("UserChoice");
                        User_Explorer.CreateSubKey("UserChoice").SetValue("ProgId", @"Applications\" + ExecutableName);
                    }
                }
                Program.DebugToConsole(false, "File association is done.", null);
                SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
            }
            catch { }
        }

        public static void InitializeLastPath() // Initialize the paths the app saved before (SetLastPath() and SetLastImportExportPath())
        {
            try
            {
                if (KeppySynthConfiguratorMain.SynthPaths.GetValue("lastpathsfimport", null) == null)
                {
                    Registry.CurrentUser.CreateSubKey("SOFTWARE\\Keppy's Synthesizer\\Paths");
                    KeppySynthConfiguratorMain.SynthPaths = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Paths", true);
                    KeppySynthConfiguratorMain.SynthPaths.SetValue("lastpathsfimport", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), RegistryValueKind.String);
                    KeppySynthConfiguratorMain.LastBrowserPath = KeppySynthConfiguratorMain.SynthPaths.GetValue("lastpathsfimport", Environment.GetFolderPath(Environment.SpecialFolder.Desktop)).ToString();
                    KeppySynthConfiguratorMain.Delegate.SoundfontImport.InitialDirectory = KeppySynthConfiguratorMain.LastBrowserPath;
                }
                else if (KeppySynthConfiguratorMain.SynthPaths.GetValue("lastpathlistimpexp", null) == null)
                {
                    Registry.CurrentUser.CreateSubKey("SOFTWARE\\Keppy's Synthesizer\\Paths");
                    KeppySynthConfiguratorMain.SynthPaths = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Paths", true);
                    KeppySynthConfiguratorMain.SynthPaths.SetValue("lastpathlistimpexp", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), RegistryValueKind.String);
                    KeppySynthConfiguratorMain.LastImportExportPath = KeppySynthConfiguratorMain.SynthPaths.GetValue("lastpathlistimpexp", Environment.GetFolderPath(Environment.SpecialFolder.Desktop)).ToString();
                    KeppySynthConfiguratorMain.Delegate.ExternalListImport.InitialDirectory = KeppySynthConfiguratorMain.LastImportExportPath;
                    KeppySynthConfiguratorMain.Delegate.ExternalListExport.InitialDirectory = KeppySynthConfiguratorMain.LastImportExportPath;
                }
                else if (KeppySynthConfiguratorMain.SynthPaths.GetValue("lastpathsfimport", null) == null && KeppySynthConfiguratorMain.SynthPaths.GetValue("lastpathlistimpexp", null) == null)
                {
                    Registry.CurrentUser.CreateSubKey("SOFTWARE\\Keppy's Synthesizer\\Paths");
                    KeppySynthConfiguratorMain.SynthPaths = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Paths", true);
                    KeppySynthConfiguratorMain.SynthPaths.SetValue("lastpathsfimport", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), RegistryValueKind.String);
                    KeppySynthConfiguratorMain.SynthPaths.SetValue("lastpathlistimpexp", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), RegistryValueKind.String);
                    KeppySynthConfiguratorMain.LastBrowserPath = KeppySynthConfiguratorMain.SynthPaths.GetValue("lastpathsfimport", Environment.GetFolderPath(Environment.SpecialFolder.Desktop)).ToString();
                    KeppySynthConfiguratorMain.LastImportExportPath = KeppySynthConfiguratorMain.SynthPaths.GetValue("lastpathlistimpexp", Environment.GetFolderPath(Environment.SpecialFolder.Desktop)).ToString();
                    KeppySynthConfiguratorMain.Delegate.SoundfontImport.InitialDirectory = KeppySynthConfiguratorMain.LastBrowserPath;
                    KeppySynthConfiguratorMain.Delegate.ExternalListImport.InitialDirectory = KeppySynthConfiguratorMain.LastImportExportPath;
                    KeppySynthConfiguratorMain.Delegate.ExternalListExport.InitialDirectory = KeppySynthConfiguratorMain.LastImportExportPath;
                }
                else
                {
                    KeppySynthConfiguratorMain.LastBrowserPath = KeppySynthConfiguratorMain.SynthPaths.GetValue("lastpathsfimport", Environment.GetFolderPath(Environment.SpecialFolder.Desktop)).ToString();
                    KeppySynthConfiguratorMain.LastImportExportPath = KeppySynthConfiguratorMain.SynthPaths.GetValue("lastpathlistimpexp", Environment.GetFolderPath(Environment.SpecialFolder.Desktop)).ToString();
                    KeppySynthConfiguratorMain.Delegate.SoundfontImport.InitialDirectory = KeppySynthConfiguratorMain.LastBrowserPath;
                    KeppySynthConfiguratorMain.Delegate.ExternalListImport.InitialDirectory = KeppySynthConfiguratorMain.LastImportExportPath;
                    KeppySynthConfiguratorMain.Delegate.ExternalListExport.InitialDirectory = KeppySynthConfiguratorMain.LastImportExportPath;
                }
            }
            catch
            {
                Registry.CurrentUser.CreateSubKey("SOFTWARE\\Keppy's Synthesizer\\Paths");
                KeppySynthConfiguratorMain.SynthPaths = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Paths", true);
                KeppySynthConfiguratorMain.SynthPaths.SetValue("lastpathsfimport", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), RegistryValueKind.String);
                KeppySynthConfiguratorMain.SynthPaths.SetValue("lastpathlistimpexp", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), RegistryValueKind.String);
                KeppySynthConfiguratorMain.LastBrowserPath = KeppySynthConfiguratorMain.SynthPaths.GetValue("lastpathsfimport", Environment.GetFolderPath(Environment.SpecialFolder.Desktop)).ToString();
                KeppySynthConfiguratorMain.LastImportExportPath = KeppySynthConfiguratorMain.SynthPaths.GetValue("lastpathlistimpexp", Environment.GetFolderPath(Environment.SpecialFolder.Desktop)).ToString();
                KeppySynthConfiguratorMain.Delegate.SoundfontImport.InitialDirectory = KeppySynthConfiguratorMain.LastBrowserPath;
                KeppySynthConfiguratorMain.Delegate.ExternalListImport.InitialDirectory = KeppySynthConfiguratorMain.LastImportExportPath;
                KeppySynthConfiguratorMain.Delegate.ExternalListExport.InitialDirectory = KeppySynthConfiguratorMain.LastImportExportPath;
                KeppySynthConfiguratorMain.Delegate.SoundfontImport.InitialDirectory = KeppySynthConfiguratorMain.LastBrowserPath;
            }
        }

        // WinMM Patch
        public enum MachineType { Native = 0, x86 = 0x014c, Itanium = 0x0200, x64 = 0x8664 }

        public static string GetAppCompiledMachineType(string fileName)
        {
            const int PE_POINTER_OFFSET = 60;
            const int MACHINE_OFFSET = 4;
            byte[] data = new byte[4096];
            using (Stream s = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                s.Read(data, 0, 4096);
            }
            // dos header is 64 bytes, last element, long (4 bytes) is the address of the PE header
            int PE_HEADER_ADDR = BitConverter.ToInt32(data, PE_POINTER_OFFSET);
            int machineUint = BitConverter.ToUInt16(data, PE_HEADER_ADDR + MACHINE_OFFSET);
            return ((MachineType)machineUint).ToString();
        }

        public static Boolean ApplyWinMMWRPPatch(Boolean Is64Bit)
        {
            if ((Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor <= 1) && Properties.Settings.Default.PatchInfoShow == true)
            {
                Properties.Settings.Default.PatchInfoShow = false;
                Properties.Settings.Default.Save();
                MessageBox.Show("The patch is not needed on Windows 7 and older, but you can install it anyway.", "Keppy's Synthesizer - Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            OpenFileDialog WinMMDialog = new OpenFileDialog();
            TryAgain:
            try
            {
                WinMMDialog.Filter = "Executables (*.exe)|*.exe;";
                WinMMDialog.Title = "Select an application to patch";
                WinMMDialog.Multiselect = false;
                WinMMDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                if (WinMMDialog.ShowDialog() == DialogResult.OK)
                {
                    String DirectoryPath = Path.GetDirectoryName(WinMMDialog.FileName);
                    if (GetAppCompiledMachineType(WinMMDialog.FileName) == "x86" && Is64Bit)
                    {
                        MessageBox.Show("You can't patch a 32-bit application with the 64-bit patch!", "Keppy's Synthesizer - Patch error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                    else if (GetAppCompiledMachineType(WinMMDialog.FileName) == "x64" && !Is64Bit)
                    {
                        MessageBox.Show("You can't patch a 64-bit application with the 32-bit patch!", "Keppy's Synthesizer - Patch error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                    else
                    {
                        RemovePatchFiles(WinMMDialog.FileName, true);

                        if (Is64Bit) File.WriteAllBytes(String.Format("{0}\\{1}", DirectoryPath, "winmm.dll"), Properties.Resources.winmm64wrp);
                        else File.WriteAllBytes(String.Format("{0}\\{1}", DirectoryPath, "winmm.dll"), Properties.Resources.winmm32wrp);

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorDialog(2, System.Media.SystemSounds.Exclamation, "Error", "Unable to patch the following executable!\nAre you sure you have write permissions to its folder?\n\nPress OK to try again.", false, ex);
                goto TryAgain;
            }

            return false;
        }

        public static Boolean RemoveWinMMPatch()
        {
            OpenFileDialog WinMMDialog = new OpenFileDialog();
            TryAgain:
            try
            {
                WinMMDialog.Filter = "Executables (*.exe)|*.exe;";
                WinMMDialog.Title = "Select an application to unpatch";
                WinMMDialog.Multiselect = false;
                WinMMDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                if (WinMMDialog.ShowDialog() == DialogResult.OK) RemovePatchFiles(WinMMDialog.FileName, false);

                return true;
            }
            catch
            {
                ShowErrorDialog(2, System.Media.SystemSounds.Exclamation, "Error", "Unable to unpatch the following executable!\nAre you sure you have write permissions to its folder?\n\nPress OK to try again.", false, null);
                goto TryAgain;
            }

            return false;
        }

        private static void RemovePatchFiles(String DirectoryPath, Boolean Silent)
        {
            String[] DeleteTheseFiles = { "midimap.dll", "msacm32.drv", "msacm32.dll", "msapd32.drv", "msapd32.dll", "wdmaud.drv", "wdmaud.sys", "winmm.dll", "owinmm.dll" };
            TryAgain:
            try
            {
                foreach (String DeleteMe in DeleteTheseFiles) File.Delete(String.Format("{0}\\{1}", Path.GetDirectoryName(DirectoryPath), DeleteMe));
            }
            catch
            {
                ShowErrorDialog(2, System.Media.SystemSounds.Exclamation, "Error", "Unable to unpatch the following executable!\nAre you sure you have write permissions to its folder?\n\nPress OK to try again.", false, null);
                goto TryAgain;
            }
            if (!Silent) MessageBox.Show(String.Format("\"{0}\" has been succesfully unpatched!", Path.GetFileName(DirectoryPath)), "Keppy's Synthesizer - Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static bool MonitorStop = true;
        public static void MonitorTailOfFile(string filePath)
        {
             MonitorStop = false;

            var initialFileSize = new FileInfo(filePath).Length;
            var lastReadLength = initialFileSize - 1024;
            if (lastReadLength < 0) lastReadLength = 0;

            while (true)
            {
                try
                {
                    var fileSize = new FileInfo(filePath).Length;
                    if (fileSize > lastReadLength)
                    {
                        using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            fs.Seek(lastReadLength, SeekOrigin.Begin);
                            var buffer = new byte[1024];

                            while (true)
                            {
                                MessageBox.Show("new line");

                                var bytesRead = fs.Read(buffer, 0, buffer.Length);
                                lastReadLength += bytesRead;

                                if (bytesRead == 0)
                                    break;

                                var text = ASCIIEncoding.ASCII.GetString(buffer, 0, bytesRead);

                                KeppySynthConfiguratorMain.Delegate.Invoke((MethodInvoker)delegate ()
                                {
                                    KeppySynthConfiguratorMain.Delegate.DebugLogShow.AppendText(Environment.NewLine + text);
                                    KeppySynthConfiguratorMain.Delegate.DebugLogShow.ScrollToCaret();
                                });
                            }
                        }
                    }
                }
                catch { }

                if (MonitorStop == true) break;

                Thread.Sleep(10);
            }
        }
    }

    public static class InputExtensions
    {
        public static int LimitToRange(
            this int value, int inclusiveMinimum, int inclusiveMaximum)
        {
            if (value < inclusiveMinimum) { return inclusiveMinimum; }
            if (value > inclusiveMaximum) { return inclusiveMaximum; }
            return value;
        }
    }

    class SoundEvent
    {
        // Beep function
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool MessageBeep(uint type);

        public static UInt32 SndBeep = 0xFFFFFFFF;
        public static UInt32 SndInformation = 0x00000040;
        public static UInt32 SndWarning = 0x00000030;
        public static UInt32 SndHand = 0x00000040;
        public static UInt32 SndQuestion = 0x00000020;
        public static UInt32 SndOk = 0x00000000;
    }
}

namespace System.Windows.Forms
{
    // Hand cursor fix by Harnido-San (https://stackoverflow.com/users/1805892/hamido-san)

    public class LinkLabelEx : LinkLabel
    {
        private const int IDC_HAND = 32649;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);

        public static readonly Cursor SystemHandCursor = new Cursor(LoadCursor(IntPtr.Zero, IDC_HAND));

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            // If the base class decided to show the ugly hand cursor
            if (OverrideCursor == Cursors.Hand)
            {
                // Show the system hand cursor instead
                OverrideCursor = SystemHandCursor;
            }
        }
    }
}