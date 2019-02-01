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
using System.Security.Principal;

namespace OmniMIDIConfigurator
{
    static class AudioEngine
    {
        // Internal
        public const int ENCODING_MODE = 0;
        public const int DSOUND_OR_WASAPI = 1;
        public const int PRO_INTERFACE = 2;

        // Explicit names
        public const int AUDTOWAV = 0;
        public const int DSOUND_ENGINE = 1;
        public const int ASIO_ENGINE = 2;
        public const int WASAPI_ENGINE = 3;
    }

    static class ErrorType
    {
        public const int Information = 0;
        public const int Error = 1;
    }

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

        [DllImport("kernel32.dll")]
        private static extern void GetNativeSystemInfo(ref SYSTEM_INFO lpSystemInfo);

        public const short PROCESSOR_ARCHITECTURE_UNKNOWN = 0xFF;
        public const short PROCESSOR_ARCHITECTURE_ARM64 = 12;
        public const short PROCESSOR_ARCHITECTURE_AMD64 = 9;
        public const short PROCESSOR_ARCHITECTURE_IA64 = 6;
        public const short PROCESSOR_ARCHITECTURE_INTEL = 0;

        [StructLayout(LayoutKind.Sequential)]
        private struct SYSTEM_INFO
        {
            public short wProcessorArchitecture;
            public short wReserved;
            public int dwPageSize;
            public IntPtr lpMinimumApplicationAddress;
            public IntPtr lpMaximumApplicationAddress;
            public IntPtr dwActiveProcessorMask;
            public int dwNumberOfProcessors;
            public int dwProcessorType;
            public int dwAllocationGranularity;
            public short wProcessorLevel;
            public short wProcessorRevision;
        }

        public static int GetProcessorArchitecture()
        {
            SYSTEM_INFO si = new SYSTEM_INFO();
            GetNativeSystemInfo(ref si);
            return si.wProcessorArchitecture;
        }

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
                string newlocation = System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\OmniMIDI\\";
                Directory.Move(oldlocation, newlocation);
            }
            catch { }
        }

        public static void DriverRegistry()
        {
            try
            {
                Program.DebugToConsole(false, "Opening register/unregister dialog...", null);
                var process = System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\OmniMIDI\\OmniMIDIDriverRegister.exe");
                process.WaitForExit();
                Program.DebugToConsole(false, "Done.", null);
            }
            catch (Exception ex)
            {
                Functions.ShowErrorDialog(ErrorType.Error, System.Media.SystemSounds.Hand, "Error", "There was an error while trying to register/unregister the driver.", true, ex);
            }
        }

        public static void MIDIMapRegistry(Boolean Uninstall)
        {
            try
            {
                Process Proc = Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\OmniMIDI\\OmniMIDIDriverRegister.exe", Uninstall ? "/umidimapv" : "/rmidimapv");
                Proc.WaitForExit();
                CheckMIDIMapper();
            }
            catch (Exception ex)
            {
                ShowErrorDialog(ErrorType.Error, System.Media.SystemSounds.Hand, "Error", "There was an error while trying to register/unregister the driver.", true, ex);
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
                    var DLL32bit = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\OmniMIDI\\OmniMIDI.dll", FileMode.OpenOrCreate, FileAccess.Read);
                    byte[] checksum32 = sha32.ComputeHash(DLL32bit);
                    String Driver32SHA256 = BitConverter.ToString(checksum32).Replace("-", String.Empty);
                    String Driver64SHA256 = null;

                    if (Environment.Is64BitOperatingSystem)
                    {
                        Functions.Wow64DisableWow64FsRedirection(ref WOW64Value);
                        var sha64 = new SHA256Managed();
                        var DLL64bit = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\OmniMIDI\\OmniMIDI.dll", FileMode.OpenOrCreate, FileAccess.Read);
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

                    Stream Stream32 = client.OpenRead("https://raw.githubusercontent.com/KaleidonKep99/Keppy-s-Synthesizer/master/OmniMIDIConfigurator/OmniMIDIConfigurator/Resources/Hashes/DRV32.SHA");
                    StreamReader Reader32 = new StreamReader(Stream32);
                    String New32SHA256 = Reader32.ReadToEnd();
                    New32SHA256 = rgx.Replace(New32SHA256, "");
                    String New64SHA256 = null;
                    String Current32SHA256 = null;
                    String Current64SHA256 = null;

                    Current32SHA256 = System.Text.Encoding.UTF8.GetString(OmniMIDIConfigurator.Properties.Resources.DRV32);

                    String Potato = "";
                    if (Functions.GetProcessorArchitecture() == Functions.PROCESSOR_ARCHITECTURE_AMD64)
                        Potato = "64";
                    else if (Functions.GetProcessorArchitecture() == Functions.PROCESSOR_ARCHITECTURE_ARM64)
                        Potato = "ARM64";

                    if (!String.IsNullOrEmpty(Potato))
                    {
                        Stream Stream64 = client.OpenRead(String.Format("https://raw.githubusercontent.com/KaleidonKep99/Keppy-s-Synthesizer/master/OmniMIDIConfigurator/OmniMIDIConfigurator/Resources/Hashes/DRV{0}.SHA", Potato));
                        StreamReader Reader64 = new StreamReader(Stream64);
                        New64SHA256 = Reader64.ReadToEnd();
                        New64SHA256 = rgx.Replace(New64SHA256, "");
                        Current64SHA256 = System.Text.Encoding.UTF8.GetString(OmniMIDIConfigurator.Properties.Resources.DRV64);
                    }

                    DriverSignatureCheckup frm = new DriverSignatureCheckup(Current32SHA256, Current64SHA256, New32SHA256, New64SHA256);
                    frm.ShowDialog();
                    frm.Dispose();
                }
            }
            catch
            {
                MessageBox.Show("Can not check for the current signatures on GitHub!\nYou may not be connected to the Internet, or GitHub may have server issues.\n\nCheck if your Internet connection is working properly, or try again later.", "OmniMIDI - Driver signature check error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
            OmniMIDIConfiguratorMain.SynthSettings.SetValue("DriverPriority", priority, RegistryValueKind.DWord);
        }

        public static void SetFramerate(int yesno)
        {
            OmniMIDIConfiguratorMain.SynthSettings.SetValue("CapFramerate", yesno, RegistryValueKind.DWord);
        }

        public static void SleepStates(int yesno)
        {
            OmniMIDIConfiguratorMain.SynthSettings.SetValue("SleepStates", yesno, RegistryValueKind.DWord);
        }

        public static void OldBufferMode(int yesno)
        {
            OmniMIDIConfiguratorMain.SynthSettings.SetValue("NotesCatcherWithAudio", yesno, RegistryValueKind.DWord);
        }

        public static void LoudMaxInstall()
        {
            try
            {
                bool bit32 = false;
                bool bit64 = false;
                string userfolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\OmniMIDI\\LoudMax";

                if (!Directory.Exists(userfolder))
                    Directory.CreateDirectory(userfolder);

                // 32-bit DLL
                if (!File.Exists(userfolder + "\\LoudMax32.dll"))
                {
                    Program.DebugToConsole(false, "Extracting LoudMax 32-bit...", null);
                    File.WriteAllBytes(String.Format("{0}\\{1}", userfolder, "LoudMax32.dll"), Properties.Resources.LoudMax32);
                    Program.DebugToConsole(false, "LoudMax 32-bit is now installed.", null);
                    bit32 = true;
                }
                else
                {
                    Program.DebugToConsole(false, "LoudMax 32-bit is already installed.", null);
                    MessageBox.Show("LoudMax 32-bit seems to be already installed.", "OmniMIDI - Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // 64-bit DLL
                if (Environment.Is64BitOperatingSystem)
                {
                    if (!File.Exists(userfolder + "\\LoudMax64.dll"))
                    {
                        Program.DebugToConsole(false, "Extracting LoudMax 64-bit...", null);
                        File.WriteAllBytes(String.Format("{0}\\{1}", userfolder, "LoudMax64.dll"), Properties.Resources.LoudMax64);
                        Program.DebugToConsole(false, "LoudMax 64-bit is now installed.", null);
                        bit64 = true;
                    }
                    else
                    {
                        Program.DebugToConsole(false, "LoudMax 64-bit is already installed.", null);
                        MessageBox.Show("LoudMax 64-bit seems to be already installed.", "OmniMIDI - Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else { bit32 = true; }

                if (bit32 == true && bit64 == true)
                {
                    MessageBox.Show("LoudMax successfully installed!", "OmniMIDI - Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Program.DebugToConsole(false, "LoudMax has been installed properly.", null);
                }
            }
            catch (Exception ex)
            {
                Functions.ShowErrorDialog(1, System.Media.SystemSounds.Exclamation, "LoudMax Installation", "Crap, an error!\nAre you sure the files aren't locked?", true, ex);
            }
        }

        public static void LoudMaxUninstall()
        {
            try
            {
                bool bit32 = false;
                bool bit64 = false;
                string userfolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\OmniMIDI\\LoudMax";

                // 32-bit DLL
                if (File.Exists(userfolder + "\\LoudMax32.dll"))
                {
                    Program.DebugToConsole(false, "Uninstalling LoudMax 32-bit...", null);
                    File.Delete(userfolder + "\\LoudMax32.dll");
                    bit32 = true;
                }
                else
                {
                    Program.DebugToConsole(false, "LoudMax 32-bit is already uninstalled.", null);
                    MessageBox.Show("LoudMax 32-bit seems to be already uninstalled.", "OmniMIDI - Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // 64-bit DLL
                if (File.Exists(userfolder + "\\LoudMax64.dll"))
                {
                    Program.DebugToConsole(false, "Uninstalling LoudMax 64-bit...", null);
                    File.Delete(userfolder + "\\LoudMax64.dll");
                    bit64 = true;
                }
                else
                {
                    Program.DebugToConsole(false, "LoudMax 64-bit is already uninstalled.", null);
                    MessageBox.Show("LoudMax 64-bit seems to be already uninstalled.", "OmniMIDI - Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                Directory.Delete(userfolder);

                if (bit32 == true && bit64 == true)
                {
                    MessageBox.Show("LoudMax successfully uninstalled!", "OmniMIDI - Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Functions.ShowErrorDialog(1, System.Media.SystemSounds.Exclamation, "LoudMax Installation", "Crap, an error!\nAre you sure you closed all the apps using the driver? They might have locked LoudMax.", true, ex);
            }
        }

        // -------------------------
        // Soundfont lists functions

        public static void SetLastMIDIPath(string path) // Saves the last path from the SoundFont preview dialog to the registry 
        {
            try
            {
                SoundFontInfo.LastMIDIPath = path;
                OmniMIDIConfiguratorMain.SynthPaths.SetValue("lastpathmidimport", path);
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
                using (StreamReader r = new StreamReader(System.Environment.GetEnvironmentVariable("USERPROFILE").ToString() + "\\OmniMIDI\\OmniMIDI.favlist"))
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
            if (OmniMIDIConfiguratorMain.Delegate.VolumeBoost.Checked)
            {
                if (OmniMIDIConfiguratorMain.Delegate.VolTrackBar.Value > 10000)
                {
                    OmniMIDIConfiguratorMain.Delegate.VolTrackBar.Value = 10000;
                    OmniMIDIConfiguratorMain.SynthSettings.SetValue("OutputVolume", 10000, RegistryValueKind.DWord);
                }
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("VolumeBoost", 0, RegistryValueKind.DWord);
                OmniMIDIConfiguratorMain.Delegate.VolTrackBar.Maximum = 10000;
                OmniMIDIConfiguratorMain.Delegate.VolumeBoost.Checked = false;
                OmniMIDIConfiguratorMain.Delegate.VolTrackBar.Refresh();
            }
            else
            {
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("VolumeBoost", 1, RegistryValueKind.DWord);
                OmniMIDIConfiguratorMain.Delegate.VolTrackBar.Maximum = 50000;
                OmniMIDIConfiguratorMain.Delegate.VolumeBoost.Checked = true;
                OmniMIDIConfiguratorMain.Delegate.VolTrackBar.Refresh();
            }
        }

        public static void SetDefaultDevice(int engine, int dev, string asiodev)
        {
            if (engine == AudioEngine.DSOUND_ENGINE || engine == AudioEngine.WASAPI_ENGINE)
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("AudioOutput", dev, RegistryValueKind.DWord);
            else if (engine == AudioEngine.ASIO_ENGINE)
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("ASIOOutput", asiodev, RegistryValueKind.String);

            if (Properties.Settings.Default.LiveChanges) OmniMIDIConfiguratorMain.SynthSettings.SetValue("LiveChanges", "1", RegistryValueKind.DWord);
        }

        public static void CheckMIDIMapper() // Check if the Alternative MIDI Mapper is installed
        {
            RegistryKey CLSID = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\Drivers32", false);
            if (CLSID.GetValue("midimapper", "midimap.dll").ToString() == "OmniMIDI\\amidimap.cpl")
            {
                OmniMIDIConfiguratorMain.Delegate.AMIDIMapCpl.Visible = true;
                OmniMIDIConfiguratorMain.Delegate.changeDefaultMIDIOutDeviceToolStripMenuItem1.Visible = false;
                OmniMIDIConfiguratorMain.Delegate.changeDefaultMIDIOutDeviceToolStripMenuItem.Visible = false;
                OmniMIDIConfiguratorMain.Delegate.changeDefault64bitMIDIOutDeviceToolStripMenuItem.Visible = false;
            }
            else
            {
                OmniMIDIConfiguratorMain.Delegate.AMIDIMapCpl.Visible = false;
                if (Environment.OSVersion.Version.Major > 6)
                {
                    OmniMIDIConfiguratorMain.Delegate.changeDefaultMIDIOutDeviceToolStripMenuItem1.Text = "Change default MIDI out device for Windows Media Player";
                    OmniMIDIConfiguratorMain.Delegate.changeDefaultMIDIOutDeviceToolStripMenuItem.Text = "Change default MIDI out device for Windows Media Player 32-bit";
                    OmniMIDIConfiguratorMain.Delegate.changeDefault64bitMIDIOutDeviceToolStripMenuItem.Text = "Change default MIDI out device for Windows Media Player 64-bit";
                }

                if (!Environment.Is64BitOperatingSystem)
                {
                    OmniMIDIConfiguratorMain.Delegate.changeDefaultMIDIOutDeviceToolStripMenuItem1.Visible = true;
                    OmniMIDIConfiguratorMain.Delegate.changeDefaultMIDIOutDeviceToolStripMenuItem.Visible = false;
                    OmniMIDIConfiguratorMain.Delegate.changeDefault64bitMIDIOutDeviceToolStripMenuItem.Visible = false;
                }
                else
                {
                    OmniMIDIConfiguratorMain.Delegate.changeDefaultMIDIOutDeviceToolStripMenuItem1.Visible = false;
                    OmniMIDIConfiguratorMain.Delegate.changeDefaultMIDIOutDeviceToolStripMenuItem.Visible = true;
                    OmniMIDIConfiguratorMain.Delegate.changeDefault64bitMIDIOutDeviceToolStripMenuItem.Visible = true;
                }
            }
        }


        public static void CheckSincEnabled()
        {
            OmniMIDIConfiguratorMain.Delegate.SincConvLab.Enabled = OmniMIDIConfiguratorMain.Delegate.SincInter.Checked;
            OmniMIDIConfiguratorMain.Delegate.SincConv.Enabled = OmniMIDIConfiguratorMain.Delegate.SincInter.Checked;
        }

        public static void AudioEngBoxTrigger(bool save)
        {
            if (OmniMIDIConfiguratorMain.Delegate.AudioEngBox.SelectedIndex == AudioEngine.AUDTOWAV)
            {
                OmniMIDIConfiguratorMain.Delegate.BufferText.Enabled = false;
                OmniMIDIConfiguratorMain.Delegate.BufferText.Text = "The output buffer isn't needed when outputting to a .WAV file";
                OmniMIDIConfiguratorMain.Delegate.DrvHzLabel.Enabled = true;
                OmniMIDIConfiguratorMain.Delegate.Frequency.Enabled = true;
                OmniMIDIConfiguratorMain.Delegate.MaxCPU.Enabled = false;
                OmniMIDIConfiguratorMain.Delegate.RenderingTimeLabel.Enabled = false;
                OmniMIDIConfiguratorMain.Delegate.VolLabel.Enabled = false;
                OmniMIDIConfiguratorMain.Delegate.VolSimView.Enabled = false;
                OmniMIDIConfiguratorMain.Delegate.VolTrackBar.Enabled = false;
                OmniMIDIConfiguratorMain.Delegate.bufsize.Enabled = false;
            }
            else if (OmniMIDIConfiguratorMain.Delegate.AudioEngBox.SelectedIndex == AudioEngine.DSOUND_ENGINE)
            {
                OmniMIDIConfiguratorMain.Delegate.BufferText.Enabled = true;
                OmniMIDIConfiguratorMain.Delegate.BufferText.Text = "Output buffer (in ms, from 1 to 1000)\n(If the buffer is too small, it'll be set automatically to the lowest value possible)";
                OmniMIDIConfiguratorMain.Delegate.DrvHzLabel.Enabled = true;
                OmniMIDIConfiguratorMain.Delegate.Frequency.Enabled = true;
                OmniMIDIConfiguratorMain.Delegate.MaxCPU.Enabled = true;
                OmniMIDIConfiguratorMain.Delegate.RenderingTimeLabel.Enabled = true;
                OmniMIDIConfiguratorMain.Delegate.VolLabel.Enabled = true;
                OmniMIDIConfiguratorMain.Delegate.VolSimView.Enabled = true;
                OmniMIDIConfiguratorMain.Delegate.VolTrackBar.Enabled = true;
                OmniMIDIConfiguratorMain.Delegate.bufsize.Enabled = true;
            }
            else if (OmniMIDIConfiguratorMain.Delegate.AudioEngBox.SelectedIndex == AudioEngine.ASIO_ENGINE)
            {
                if (DefaultASIOAudioOutput.GetASIODevicesCount() < 1)
                {
                    ShowErrorDialog(ErrorType.Error, System.Media.SystemSounds.Asterisk, "Error", "No ASIO devices installed!\n\nClick OK to switch to WASAPI.", false, null);
                    OmniMIDIConfiguratorMain.Delegate.AudioEngBox.SelectedIndex = 3;
                    AudioEngBoxTrigger(true);
                    return;
                }

                OmniMIDIConfiguratorMain.Delegate.BufferText.Enabled = false;
                OmniMIDIConfiguratorMain.Delegate.BufferText.Text = "The output buffer is controlled by the ASIO device itself";
                OmniMIDIConfiguratorMain.Delegate.DrvHzLabel.Enabled = true;
                OmniMIDIConfiguratorMain.Delegate.Frequency.Enabled = true;
                OmniMIDIConfiguratorMain.Delegate.MaxCPU.Enabled = true;
                OmniMIDIConfiguratorMain.Delegate.RenderingTimeLabel.Enabled = true;
                OmniMIDIConfiguratorMain.Delegate.VolLabel.Enabled = true;
                OmniMIDIConfiguratorMain.Delegate.VolSimView.Enabled = true;
                OmniMIDIConfiguratorMain.Delegate.VolTrackBar.Enabled = true;
                OmniMIDIConfiguratorMain.Delegate.bufsize.Enabled = false;
            }
            else if (OmniMIDIConfiguratorMain.Delegate.AudioEngBox.SelectedIndex == AudioEngine.WASAPI_ENGINE)
            {
                OmniMIDIConfiguratorMain.Delegate.BufferText.Enabled = true;
                OmniMIDIConfiguratorMain.Delegate.BufferText.Text = "Output buffer (in ms, from 1 to 1000)\n(If the buffer is too small, it'll be set automatically to the lowest value possible)";
                OmniMIDIConfiguratorMain.Delegate.DrvHzLabel.Enabled = true;
                OmniMIDIConfiguratorMain.Delegate.Frequency.Enabled = true;
                OmniMIDIConfiguratorMain.Delegate.MaxCPU.Enabled = true;
                OmniMIDIConfiguratorMain.Delegate.RenderingTimeLabel.Enabled = true;
                OmniMIDIConfiguratorMain.Delegate.VolLabel.Enabled = true;
                OmniMIDIConfiguratorMain.Delegate.VolSimView.Enabled = true;
                OmniMIDIConfiguratorMain.Delegate.VolTrackBar.Enabled = true;
                OmniMIDIConfiguratorMain.Delegate.bufsize.Enabled = true;
            }
            else OmniMIDIConfiguratorMain.Delegate.AudioEngBox.SelectedIndex = AudioEngine.WASAPI_ENGINE;

            OmniMIDIConfiguratorMain.Delegate.changeDirectoryOfTheOutputToWAVModeToolStripMenuItem.Enabled = true;
            if (save) OmniMIDIConfiguratorMain.SynthSettings.SetValue("xaudiodisabled", OmniMIDIConfiguratorMain.Delegate.AudioEngBox.SelectedIndex, RegistryValueKind.DWord);
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
                OmniMIDIConfiguratorMain.Delegate.PolyphonyLimit.Value = Convert.ToInt32(OmniMIDIConfiguratorMain.SynthSettings.GetValue("MaxVoices", 512));
                OmniMIDIConfiguratorMain.Delegate.MaxCPU.Value = Convert.ToInt32(OmniMIDIConfiguratorMain.SynthSettings.GetValue("MaxRenderingTime", 75));
                OmniMIDIConfiguratorMain.Delegate.hotkeys.Checked = Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("FastHotkeys", 0));

                if (Convert.ToInt32(OmniMIDIConfiguratorMain.SynthSettings.GetValue("DriverPriority", 0)) == 0)
                {
                    ButtonStatus(false);
                    OmniMIDIConfiguratorMain.Delegate.DePrio.Checked = true;
                }
                else if (Convert.ToInt32(OmniMIDIConfiguratorMain.SynthSettings.GetValue("DriverPriority", 0)) == 1)
                    OmniMIDIConfiguratorMain.Delegate.RTPrio.Checked = true;
                else if (Convert.ToInt32(OmniMIDIConfiguratorMain.SynthSettings.GetValue("DriverPriority", 0)) == 2)
                    OmniMIDIConfiguratorMain.Delegate.HiPrio.Checked = true;
                else if (Convert.ToInt32(OmniMIDIConfiguratorMain.SynthSettings.GetValue("DriverPriority", 0)) == 3)
                    OmniMIDIConfiguratorMain.Delegate.HNPrio.Checked = true;
                else if (Convert.ToInt32(OmniMIDIConfiguratorMain.SynthSettings.GetValue("DriverPriority", 0)) == 4)
                    OmniMIDIConfiguratorMain.Delegate.NoPrio.Checked = true;
                else if (Convert.ToInt32(OmniMIDIConfiguratorMain.SynthSettings.GetValue("DriverPriority", 0)) == 5)
                    OmniMIDIConfiguratorMain.Delegate.LNPrio.Checked = true;
                else
                    OmniMIDIConfiguratorMain.Delegate.LoPrio.Checked = true;

                if (Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("DebugMode", 0)))
                    OmniMIDIConfiguratorMain.Delegate.DebugModePls.Checked = true;

                if (Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("DisableChime", 0)))
                    OmniMIDIConfiguratorMain.Delegate.DisableChime.Checked = true;

                if (Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("Extra8Lists", 0)))
                {
                    OmniMIDIConfiguratorMain.Delegate.enableextra8sf.Checked = true;
                    OmniMIDIConfiguratorMain.Delegate.SelectedListBox.Items.Add("List 9");
                    OmniMIDIConfiguratorMain.Delegate.SelectedListBox.Items.Add("List 10");
                    OmniMIDIConfiguratorMain.Delegate.SelectedListBox.Items.Add("List 11");
                    OmniMIDIConfiguratorMain.Delegate.SelectedListBox.Items.Add("List 12");
                    OmniMIDIConfiguratorMain.Delegate.SelectedListBox.Items.Add("List 13");
                    OmniMIDIConfiguratorMain.Delegate.SelectedListBox.Items.Add("List 14");
                    OmniMIDIConfiguratorMain.Delegate.SelectedListBox.Items.Add("List 15");
                    OmniMIDIConfiguratorMain.Delegate.SelectedListBox.Items.Add("List 16");
                }

                OmniMIDIConfiguratorMain.Delegate.ShowOutLevel.Checked = Properties.Settings.Default.ShowOutputLevel;
                OmniMIDIConfiguratorMain.Delegate.MixerBox.Visible = Properties.Settings.Default.ShowOutputLevel;
                OmniMIDIConfiguratorMain.Delegate.VolumeCheck.Enabled = Properties.Settings.Default.ShowOutputLevel;
                OmniMIDIConfiguratorMain.Delegate.LiveChangesTrigger.Checked = Properties.Settings.Default.LiveChanges;
                OmniMIDIConfiguratorMain.Delegate.Requirements.Active = !Properties.Settings.Default.LiveChanges;

                OmniMIDIConfiguratorMain.Delegate.VolumeBoost.Checked = Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("VolumeBoost", 0));
                OmniMIDIConfiguratorMain.Delegate.VolTrackBar.Maximum = OmniMIDIConfiguratorMain.Delegate.VolumeBoost.Checked ? 50000 : 10000;

                OmniMIDIConfiguratorMain.Delegate.AutoLoad.Checked = Properties.Settings.Default.AutoLoadList;
                OmniMIDIConfiguratorMain.Delegate.Frequency.Text = OmniMIDIConfiguratorMain.SynthSettings.GetValue("AudioFrequency", 44100).ToString();

                // Then the filthy checkboxes
                OmniMIDIConfiguratorMain.Delegate.Preload.Checked = Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("PreloadSoundfonts", 1));
                OmniMIDIConfiguratorMain.Delegate.EnableSFX.Checked = Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("EnableSFX", 1));
                OmniMIDIConfiguratorMain.Delegate.NoteOffCheck.Checked = Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("NoteOff1", 0));
                OmniMIDIConfiguratorMain.Delegate.SysResetIgnore.Checked = Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("IgnoreSysReset", 0));
                OmniMIDIConfiguratorMain.Delegate.bufsize.Value = Convert.ToInt32(OmniMIDIConfiguratorMain.SynthSettings.GetValue("BufferLength", 30));
                OmniMIDIConfiguratorMain.Delegate.SincInter.Checked = Convert.ToBoolean(OmniMIDIConfiguratorMain.SynthSettings.GetValue("SincInter", 0));
                CheckSincEnabled();

                if (Convert.ToInt32(OmniMIDIConfiguratorMain.SynthSettings.GetValue("CurrentEngine", AudioEngine.WASAPI_ENGINE)) == AudioEngine.AUDTOWAV)
                    OmniMIDIConfiguratorMain.Delegate.AudioEngBox.SelectedIndex = AudioEngine.AUDTOWAV;
                else if (Convert.ToInt32(OmniMIDIConfiguratorMain.SynthSettings.GetValue("CurrentEngine", AudioEngine.WASAPI_ENGINE)) == AudioEngine.DSOUND_ENGINE)
                    OmniMIDIConfiguratorMain.Delegate.AudioEngBox.SelectedIndex = AudioEngine.DSOUND_ENGINE;
                else if (Convert.ToInt32(OmniMIDIConfiguratorMain.SynthSettings.GetValue("CurrentEngine", AudioEngine.WASAPI_ENGINE)) == AudioEngine.ASIO_ENGINE)
                    OmniMIDIConfiguratorMain.Delegate.AudioEngBox.SelectedIndex = AudioEngine.ASIO_ENGINE;
                else if (Convert.ToInt32(OmniMIDIConfiguratorMain.SynthSettings.GetValue("CurrentEngine", AudioEngine.WASAPI_ENGINE)) == AudioEngine.WASAPI_ENGINE)
                    OmniMIDIConfiguratorMain.Delegate.AudioEngBox.SelectedIndex = AudioEngine.WASAPI_ENGINE;
                else
                    OmniMIDIConfiguratorMain.Delegate.AudioEngBox.SelectedIndex = AudioEngine.WASAPI_ENGINE;

                AudioEngBoxTrigger(false);

                try { OmniMIDIConfiguratorMain.Delegate.SincConv.SelectedIndex = Convert.ToInt32(OmniMIDIConfiguratorMain.SynthSettings.GetValue("SincConv", 0)); }
                catch { OmniMIDIConfiguratorMain.Delegate.SincConv.SelectedIndex = 2; }

                if (Environment.OSVersion.Version.Major == 10 && Environment.OSVersion.Version.Build >= 15063)
                    OmniMIDIConfiguratorMain.Delegate.SpatialSound.Visible = true;

                // And finally, the volume!
                int VolumeValue = Convert.ToInt32(OmniMIDIConfiguratorMain.SynthSettings.GetValue("OutputVolume", 10000));
                OmniMIDIConfiguratorMain.Delegate.VolTrackBar.Value = VolumeValue;
                decimal VolVal = (decimal)VolumeValue / 100;

                if (OmniMIDIConfiguratorMain.Delegate.VolTrackBar.Value <= 49)
                    OmniMIDIConfiguratorMain.Delegate.VolSimView.ForeColor = Color.Red;
                else
                    OmniMIDIConfiguratorMain.Delegate.VolSimView.ForeColor = Color.FromArgb(255, 53, 0, 119);

                OmniMIDIConfiguratorMain.Delegate.VolSimView.Text = String.Format("{0}", Math.Round(VolVal, MidpointRounding.AwayFromZero).ToString());

                LiveChanges.PreviousEngine = (int)OmniMIDIConfiguratorMain.SynthSettings.GetValue("CurrentEngine", AudioEngine.WASAPI_ENGINE);
                LiveChanges.PreviousFrequency = (int)OmniMIDIConfiguratorMain.SynthSettings.GetValue("AudioFrequency", 44100);
                LiveChanges.PreviousBuffer = (int)OmniMIDIConfiguratorMain.SynthSettings.GetValue("BufferLength", 50);
                LiveChanges.MonophonicRender = (int)OmniMIDIConfiguratorMain.SynthSettings.GetValue("MonoRendering", 0);
                LiveChanges.AudioBitDepth = (int)OmniMIDIConfiguratorMain.SynthSettings.GetValue("AudioBitDepth", 1);

                Program.DebugToConsole(false, "Done loading settings.", null);
            }
            catch (Exception ex)
            {
                Functions.ShowErrorDialog(ErrorType.Error, System.Media.SystemSounds.Hand, "Error", "An error has occurred while loading the driver's settings.", true, ex);
                Program.DebugToConsole(true, null, ex);
                ReinitializeSettings(thisform);
            }
        }

        public static bool SaveSettings(Form thisform, Boolean Override) // Saves the settings to the registry 
        {
            /*
             * Key: HKEY_CURRENT_USER\Software\OmniMIDI\Settings\
             */
            try
            {
                // Normal settings
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("MaxVoices", OmniMIDIConfiguratorMain.Delegate.PolyphonyLimit.Value.ToString(), RegistryValueKind.DWord);
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("MaxRenderingTime", OmniMIDIConfiguratorMain.Delegate.MaxCPU.Value, RegistryValueKind.DWord);
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("AudioFrequency", OmniMIDIConfiguratorMain.Delegate.Frequency.Text, RegistryValueKind.DWord);

                // Advanced SynthSettings
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("BufferLength", OmniMIDIConfiguratorMain.Delegate.bufsize.Value.ToString(), RegistryValueKind.DWord);

                // Let's not forget about the volume!
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("OutputVolume", OmniMIDIConfiguratorMain.Delegate.VolTrackBar.Value.ToString(), RegistryValueKind.DWord);

                // Checkbox stuff yay
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("PreloadSoundFonts", Convert.ToInt32(OmniMIDIConfiguratorMain.Delegate.Preload.Checked), RegistryValueKind.DWord);
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("EnableSFX", Convert.ToInt32(OmniMIDIConfiguratorMain.Delegate.EnableSFX.Checked), RegistryValueKind.DWord);
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("NoteOff1", Convert.ToInt32(OmniMIDIConfiguratorMain.Delegate.NoteOffCheck.Checked), RegistryValueKind.DWord);
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("IgnoreSysReset", Convert.ToInt32(OmniMIDIConfiguratorMain.Delegate.SysResetIgnore.Checked), RegistryValueKind.DWord);
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("CurrentEngine", OmniMIDIConfiguratorMain.Delegate.AudioEngBox.SelectedIndex, RegistryValueKind.DWord);
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("SincInter", Convert.ToInt32(OmniMIDIConfiguratorMain.Delegate.SincInter.Checked), RegistryValueKind.DWord);
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("SincConv", OmniMIDIConfiguratorMain.Delegate.SincConv.SelectedIndex, RegistryValueKind.DWord);

                if (Override)
                {
                    OmniMIDIConfiguratorMain.SynthSettings.SetValue("LiveChanges", "1", RegistryValueKind.DWord);
                    LiveChanges.PreviousEngine = (int)OmniMIDIConfiguratorMain.SynthSettings.GetValue("CurrentEngine", AudioEngine.WASAPI_ENGINE);
                    LiveChanges.PreviousFrequency = (int)OmniMIDIConfiguratorMain.SynthSettings.GetValue("AudioFrequency", 44100);
                    LiveChanges.PreviousBuffer = (int)OmniMIDIConfiguratorMain.SynthSettings.GetValue("BufferLength", 50);
                    LiveChanges.MonophonicRender = (int)OmniMIDIConfiguratorMain.SynthSettings.GetValue("MonoRendering", 0);
                    LiveChanges.AudioBitDepth = (int)OmniMIDIConfiguratorMain.SynthSettings.GetValue("AudioBitDepth", 1);
                    Program.DebugToConsole(false, "Done saving settings with force reload.", null);
                }
                else
                {
                    if (Properties.Settings.Default.LiveChanges)
                    {
                        if (LiveChanges.PreviousEngine != (int)OmniMIDIConfiguratorMain.SynthSettings.GetValue("CurrentEngine", AudioEngine.WASAPI_ENGINE) ||
                            LiveChanges.PreviousFrequency != (int)OmniMIDIConfiguratorMain.SynthSettings.GetValue("AudioFrequency", 44100) ||
                            LiveChanges.PreviousBuffer != (int)OmniMIDIConfiguratorMain.SynthSettings.GetValue("BufferLength", 50) ||
                            LiveChanges.MonophonicRender != (int)OmniMIDIConfiguratorMain.SynthSettings.GetValue("MonoRendering", 0) ||
                            LiveChanges.AudioBitDepth != (int)OmniMIDIConfiguratorMain.SynthSettings.GetValue("AudioBitDepth", 1))
                        {
                            OmniMIDIConfiguratorMain.SynthSettings.SetValue("LiveChanges", 1, RegistryValueKind.DWord);
                            LiveChanges.PreviousEngine = (int)OmniMIDIConfiguratorMain.SynthSettings.GetValue("CurrentEngine", AudioEngine.WASAPI_ENGINE);
                            LiveChanges.PreviousFrequency = (int)OmniMIDIConfiguratorMain.SynthSettings.GetValue("AudioFrequency", 44100);
                            LiveChanges.PreviousBuffer = (int)OmniMIDIConfiguratorMain.SynthSettings.GetValue("BufferLength", 50);
                            LiveChanges.MonophonicRender = (int)OmniMIDIConfiguratorMain.SynthSettings.GetValue("MonoRendering", 0);
                            LiveChanges.AudioBitDepth = (int)OmniMIDIConfiguratorMain.SynthSettings.GetValue("AudioBitDepth", 1);
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
                ShowErrorDialog(ErrorType.Error, System.Media.SystemSounds.Hand, "Error", "An error has occurred while saving the driver's settings.", true, ex);
                Program.DebugToConsole(true, null, ex);
                ReinitializeSettings(thisform);
                return false;
            }
        }

        public static void ReinitializeSettings(Form thisform) // If the registry is missing, reset it
        {
            /*
             * Key: HKEY_CURRENT_USER\Software\OmniMIDI\Settings\
             */
            try
            {
                // Initialize the registry
                Registry.CurrentUser.CreateSubKey("SOFTWARE\\OmniMIDI\\Settings");
                Registry.CurrentUser.CreateSubKey("SOFTWARE\\OmniMIDI\\Watchdog");
                Registry.CurrentUser.CreateSubKey("SOFTWARE\\OmniMIDI\\Paths");
                OmniMIDIConfiguratorMain.SynthSettings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\OmniMIDI\\Settings", true);
                OmniMIDIConfiguratorMain.Watchdog = Registry.CurrentUser.OpenSubKey("SOFTWARE\\OmniMIDI\\Settings", true);
                OmniMIDIConfiguratorMain.SynthPaths = Registry.CurrentUser.OpenSubKey("SOFTWARE\\OmniMIDI\\Settings", true);

                // Normal settings
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("MaxVoices", "500", RegistryValueKind.DWord);
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("MaxRenderingTime", "65", RegistryValueKind.DWord);
                OmniMIDIConfiguratorMain.Delegate.Frequency.Text = "48000";
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("AudioFrequency", OmniMIDIConfiguratorMain.Delegate.Frequency.Text, RegistryValueKind.DWord);

                // Advanced SynthSettings
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("BufferLength", "30", RegistryValueKind.DWord);

                // Let's not forget about the volume!
                int VolumeValue = 0;
                OmniMIDIConfiguratorMain.Delegate.VolTrackBar.Value = 10000;
                double x = OmniMIDIConfiguratorMain.Delegate.VolTrackBar.Value / 100;
                OmniMIDIConfiguratorMain.Delegate.VolSimView.Text = String.Format("{0}", Math.Round(x, MidpointRounding.AwayFromZero).ToString());
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("OutputVolume", OmniMIDIConfiguratorMain.Delegate.VolTrackBar.Value.ToString(), RegistryValueKind.DWord);

                // Checkbox stuff yay
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("PreloadSoundFonts", "1", RegistryValueKind.DWord);
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("EnableSFX", "1", RegistryValueKind.DWord);
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("NotesCatcherWithAudio", "0", RegistryValueKind.DWord);
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("NoteOff1", "0", RegistryValueKind.DWord);
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("IgnoreSysReset", "0", RegistryValueKind.DWord);
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("SincInter", "0", RegistryValueKind.DWord);
                OmniMIDIConfiguratorMain.SynthSettings.SetValue("CurrentEngine", "3", RegistryValueKind.DWord);

                // Reload the settings
                LoadSettings(thisform);
            }
            catch (Exception ex)
            {
                // Something bad happened hehe
                Program.DebugToConsole(true, null, ex);
                Functions.ShowErrorDialog(ErrorType.Error, System.Media.SystemSounds.Hand, "Fatal error", "Missing registry settings!\nPlease reinstall OmniMIDI to solve the issue.\n\nPress OK to quit.", true, ex);
                System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(ignored =>
                {
                    OmniMIDIConfiguratorMain.ActiveForm.Hide();
                    throw new Exception();
                }));
            }
        }

        public static void ImportSettings(Form thisform, String filename)
        {
            try
            {
                string line = File.ReadLines(filename).Skip(2).Take(1).First();

                if (line == "; OmniMIDI Settings File")
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

                    LoadSettings(thisform);
                }
                else
                {
                    MessageBox.Show("Invalid registry file!\n\nThis file doesn't contain valid OmniMIDI settings!!!", "OmniMIDI - Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            catch (Exception ex)
            {
                // Something bad happened hehe
                Program.DebugToConsole(true, null, ex);
                Functions.ShowErrorDialog(ErrorType.Error, System.Media.SystemSounds.Hand, "Fatal error", "Fatal error during the execution of this program!\n\nPress OK to quit.", true, ex);
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
        /// <param name="audioengine">Select the audio engine. AUDTOWAV = To .WAV, AudioEngine.DSOUND_ENGINE = DirectSound, AudioEngine.ASIO_ENGINE = ASIO, AudioEngine.WASAPI_ENGINE = WASAPI</param>
        public static void ApplyPresetValues(
            int volume, int voices, int maxcpu, int frequency, int bufsize,
            bool preload, bool noteoffcheck, bool sincinter, int sincconv, 
            bool enablesfx, bool sysresetignore, bool outputwav, int audioengine)
        {
            OmniMIDIConfiguratorMain.Delegate.VolTrackBar.Value = volume;
            OmniMIDIConfiguratorMain.Delegate.PolyphonyLimit.Value = voices;
            OmniMIDIConfiguratorMain.Delegate.MaxCPU.Value = maxcpu;
            OmniMIDIConfiguratorMain.Delegate.Frequency.Text = frequency.ToString();
            OmniMIDIConfiguratorMain.Delegate.Preload.Checked = preload;
            OmniMIDIConfiguratorMain.Delegate.NoteOffCheck.Checked = noteoffcheck;
            OmniMIDIConfiguratorMain.Delegate.SincInter.Checked = sincinter;
            OmniMIDIConfiguratorMain.Delegate.SincConv.SelectedIndex = sincconv;
            OmniMIDIConfiguratorMain.Delegate.EnableSFX.Checked = enablesfx;
            OmniMIDIConfiguratorMain.Delegate.SysResetIgnore.Checked = sysresetignore;
            OmniMIDIConfiguratorMain.Delegate.AudioEngBox.SelectedIndex = audioengine;
            OmniMIDIConfiguratorMain.Delegate.bufsize.Value = bufsize;
        }

        /// <summary>
        /// Changes the advanced audio settings automatically
        /// </summary>
        /// <param name="audiodepth">Set the audio depth of the stream. 1 = 32-bit float, 2 = 16-bit integer, 3 = 8-bit integer</param>
        /// <param name="monorendering">Set the stream to only output to one audio channel. 0 = Disabled, 1 = Enabled</param>
        /// <param name="fadeoutdisable">Set the fade out for when a note gets killed. 0 = Disabled, 1 = Enabled</param>
        /// <param name="vms2emu">Set if the driver has to emulate VirtualMIDISynth 2.x (Example: Slowdowns when the EVBuffer is full). 0 = Disabled, 1 = Enabled</param>
        /// <param name="allowksdapi">Set if the driver has to allow apps to use the KSDirect API. 0 = Disallow, 1 = Allow</param>
        /// <param name="oldbuffermode">Set if the driver should use the old buffer mode (Only DirectSound and XAudio). 0 = Disabled, 1 = Enabled</param>
        /// <param name="sleepstates">Set if the driver should disable sleepstates (Only DirectSound). 0 = Disable them, 1 = Keep them enabled</param>
        public static void ChangeAdvancedAudioSettings(int audiodepth, int monorendering, int fadeoutdisable, int vms2emu, int allowksdapi, int oldbuffermode, int sleepstates)
        {
            OmniMIDIConfiguratorMain.SynthSettings.SetValue("AudioBitDepth", audiodepth, RegistryValueKind.DWord);
            OmniMIDIConfiguratorMain.SynthSettings.SetValue("MonoRendering", monorendering, RegistryValueKind.DWord);
            OmniMIDIConfiguratorMain.SynthSettings.SetValue("DisableNotesFadeOut", fadeoutdisable, RegistryValueKind.DWord);
            OmniMIDIConfiguratorMain.SynthSettings.SetValue("DontMissNotes", vms2emu, RegistryValueKind.DWord);
            OmniMIDIConfiguratorMain.SynthSettings.SetValue("KSDAPIEnabled", allowksdapi, RegistryValueKind.DWord);
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
            OmniMIDIConfiguratorMain.SynthSettings.SetValue("GetEvBuffSizeFromRAM", getbuffbyram, Microsoft.Win32.RegistryValueKind.DWord);

            if (buffsize < 1) OmniMIDIConfiguratorMain.SynthSettings.SetValue("EvBufferSize", memStatus.ullTotalPhys, Microsoft.Win32.RegistryValueKind.QWord);
            else OmniMIDIConfiguratorMain.SynthSettings.SetValue("EvBufferSize", buffsize, Microsoft.Win32.RegistryValueKind.QWord);

            if (buffsize > 1) OmniMIDIConfiguratorMain.SynthSettings.SetValue("EvBufferMultRatio", "1", Microsoft.Win32.RegistryValueKind.DWord);
            else OmniMIDIConfiguratorMain.SynthSettings.SetValue("EvBufferMultRatio", buffratio, Microsoft.Win32.RegistryValueKind.DWord);
        }

        /// <summary>
        /// Changes the driver's mask values automatically
        /// </summary>
        /// <param name="maskname">Set the mask name.</param>
        /// <param name="masktype">Set the mask type. 0 = FM, 1 = Generic synth, 2 = Hardware synth, 3 = MIDI Mapper, 4 = Output port, 5 = Software synth, 6 = Square wave synth</param>
        public static void ChangeDriverMask(string maskname, int masktype, int vid, int pid)
        {
            OmniMIDIConfiguratorMain.SynthSettings.SetValue("SynthName", maskname, RegistryValueKind.String);
            OmniMIDIConfiguratorMain.SynthSettings.SetValue("SynthType", masktype, RegistryValueKind.DWord);
            OmniMIDIConfiguratorMain.SynthSettings.SetValue("VID", vid, RegistryValueKind.DWord);
            OmniMIDIConfiguratorMain.SynthSettings.SetValue("PID", pid, RegistryValueKind.DWord);
        }

        public static void ImportPreset(Form thisform)
        {
            String PresetTitle = "";
            Boolean dummy = false;
            try
            {
                if (OmniMIDIConfiguratorMain.Delegate.ImportPresetDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (string x in File.ReadLines(OmniMIDIConfiguratorMain.Delegate.ImportPresetDialog.FileName, Encoding.UTF8))
                    {
                        try
                        {
                            // Normal settings
                            if (SettingName(x) == "ConfName") PresetTitle = SettingValue(x);
                            else if (SettingName(x) == "AudioEngine") OmniMIDIConfiguratorMain.Delegate.AudioEngBox.SelectedIndex = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "BufferLength") OmniMIDIConfiguratorMain.Delegate.bufsize.Value = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "Frequency") OmniMIDIConfiguratorMain.Delegate.Frequency.Text = SettingValue(x);
                            else if (SettingName(x) == "IgnoreSysEx") OmniMIDIConfiguratorMain.Delegate.SysResetIgnore.Checked = Boolean.TryParse(SettingValue(x), out dummy);
                            else if (SettingName(x) == "MaxCPU") OmniMIDIConfiguratorMain.Delegate.MaxCPU.Value = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "NoteOff") OmniMIDIConfiguratorMain.Delegate.NoteOffCheck.Checked = Boolean.TryParse(SettingValue(x), out dummy);
                            else if (SettingName(x) == "Preload") OmniMIDIConfiguratorMain.Delegate.Preload.Checked = Boolean.TryParse(SettingValue(x), out dummy);
                            else if (SettingName(x) == "SincInter") OmniMIDIConfiguratorMain.Delegate.SincInter.Checked = Boolean.TryParse(SettingValue(x), out dummy);
                            else if (SettingName(x) == "SincConv") OmniMIDIConfiguratorMain.Delegate.SincConv.SelectedIndex = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "VoiceLimit") OmniMIDIConfiguratorMain.Delegate.PolyphonyLimit.Value = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "Volume") OmniMIDIConfiguratorMain.Delegate.VolTrackBar.Value = Convert.ToInt32(SettingValue(x));
                            // Advanced audio settings
                            else if (SettingName(x) == "AllowKSDAPI") OmniMIDIConfiguratorMain.SynthSettings.SetValue("KSDAPIEnabled", SettingValue(x), RegistryValueKind.DWord);
                            else if (SettingName(x) == "HyperMode") OmniMIDIConfiguratorMain.SynthSettings.SetValue("HyperPlayback", SettingValue(x), RegistryValueKind.DWord);
                            else if (SettingName(x) == "AudioDepth") OmniMIDIConfiguratorMain.SynthSettings.SetValue("AudioBitDepth", SettingValue(x), RegistryValueKind.DWord);
                            else if (SettingName(x) == "FadeOutDisable") OmniMIDIConfiguratorMain.SynthSettings.SetValue("DisableNotesFadeOut", SettingValue(x), RegistryValueKind.DWord);
                            else if (SettingName(x) == "MonoRendering") OmniMIDIConfiguratorMain.SynthSettings.SetValue("MonoRendering", SettingValue(x), RegistryValueKind.DWord);
                            else if (SettingName(x) == "SlowdownPlayback") OmniMIDIConfiguratorMain.SynthSettings.SetValue("DontMissNotes", SettingValue(x), RegistryValueKind.DWord);

                            // MIDI events parser settings
                            else if (SettingName(x) == "CapFramerate") OmniMIDIConfiguratorMain.SynthSettings.SetValue("CapFramerate", SettingValue(x), RegistryValueKind.DWord);
                            else if (SettingName(x) == "EVBufferByRAM") OmniMIDIConfiguratorMain.SynthSettings.SetValue("GetEvBuffSizeFromRAM", SettingValue(x), RegistryValueKind.DWord);
                            else if (SettingName(x) == "EVBufferRatio") OmniMIDIConfiguratorMain.SynthSettings.SetValue("EvBufferMultRatio", SettingValue(x), RegistryValueKind.DWord);
                            else if (SettingName(x) == "EVBufferSize") OmniMIDIConfiguratorMain.SynthSettings.SetValue("EvBufferSize", SettingValue(x), RegistryValueKind.DWord);
                            else if (SettingName(x) == "FullVelocityMode") OmniMIDIConfiguratorMain.SynthSettings.SetValue("FullVelocityMode", SettingValue(x), RegistryValueKind.DWord);
                            else if (SettingName(x) == "IgnoreAllNotes") OmniMIDIConfiguratorMain.SynthSettings.SetValue("IgnoreAllNotes", SettingValue(x), RegistryValueKind.DWord);
                            else if (SettingName(x) == "IgnoreNotes") OmniMIDIConfiguratorMain.SynthSettings.SetValue("IgnoreNotesBetweenVel", SettingValue(x), RegistryValueKind.DWord);
                            else if (SettingName(x) == "Limit88") OmniMIDIConfiguratorMain.SynthSettings.SetValue("LimitTo88Keys", SettingValue(x), RegistryValueKind.DWord);
                            else if (SettingName(x) == "MT32Mode") OmniMIDIConfiguratorMain.SynthSettings.SetValue("MT32Mode", SettingValue(x), RegistryValueKind.DWord);
                            else if (SettingName(x) == "SysExIgnore") OmniMIDIConfiguratorMain.SynthSettings.SetValue("IgnoreSysEx", SettingValue(x), RegistryValueKind.DWord);

                            // Driver mask
                            else if (SettingName(x) == "SynthName") OmniMIDIConfiguratorMain.SynthSettings.SetValue("SynthName", SettingValue(x), RegistryValueKind.String);
                            else if (SettingName(x) == "SynthType") OmniMIDIConfiguratorMain.SynthSettings.SetValue("SynthType", SettingValue(x), RegistryValueKind.DWord);
                            else if (SettingName(x) == "VID") OmniMIDIConfiguratorMain.SynthSettings.SetValue("VID", SettingValue(x), RegistryValueKind.DWord);
                            else if (SettingName(x) == "PID") OmniMIDIConfiguratorMain.SynthSettings.SetValue("PID", SettingValue(x), RegistryValueKind.DWord);
                        }
                        catch (Exception ex) {
                            Functions.ShowErrorDialog(ErrorType.Error, System.Media.SystemSounds.Hand, "Error", "Invalid preset!", false, ex);

                            // Set some values...
                            Functions.ApplyPresetValues(10000, 500, 75, 44100, 30, true, false, false, 2, true, false, false, AudioEngine.WASAPI_ENGINE);

                            // Advanced settings here...
                            Functions.ChangeAdvancedAudioSettings(1, 0, 0, 0, 1, 0, 1);
                            Functions.ChangeMIDIEventParserSettings(0, 0, 0, 0, 16384, 1);
                            Functions.ChangeDriverMask("OmniMIDI", 4, 0xFFFF, 0x000A);

                            // And then...
                            Functions.SaveSettings(thisform, true);

                            // Messagebox here
                            Program.DebugToConsole(false, "Settings restored.", null);

                            return;
                        }
                    }

                    MessageBox.Show(
                        String.Format("The preset \"{0}\" has been applied.", Path.GetFileNameWithoutExtension(OmniMIDIConfiguratorMain.Delegate.ImportPresetDialog.FileName)),
                        "OmniMIDI - Import preset", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // Something bad happened hehe
                Program.DebugToConsole(true, null, ex);
                Functions.ShowErrorDialog(ErrorType.Error, System.Media.SystemSounds.Hand, "Fatal error", "Fatal error during the execution of the program.\n\nPress OK to quit", true, ex);
                Application.Exit();
            }
        }

        public static void ExportPreset()
        {
            try
            {
                if (OmniMIDIConfiguratorMain.Delegate.ExportPresetDialog.ShowDialog() == DialogResult.OK)
                {
                    StringBuilder SettingsToText = new StringBuilder();

                    SettingsToText.AppendLine(String.Format("ConfName = {0}", Path.GetFileNameWithoutExtension(OmniMIDIConfiguratorMain.Delegate.ExportPresetDialog.FileName)));
                    SettingsToText.AppendLine();
                    SettingsToText.AppendLine("// Normal settings");
                    SettingsToText.AppendLine(String.Format("AudioEngine = {0}", OmniMIDIConfiguratorMain.Delegate.AudioEngBox.SelectedIndex));
                    SettingsToText.AppendLine(String.Format("BufferLength = {0}", OmniMIDIConfiguratorMain.Delegate.bufsize.Value));
                    SettingsToText.AppendLine(String.Format("Frequency = {0}", OmniMIDIConfiguratorMain.Delegate.Frequency.Text));
                    SettingsToText.AppendLine(String.Format("IgnoreSysEx = {0}", OmniMIDIConfiguratorMain.Delegate.SysResetIgnore.Checked));
                    SettingsToText.AppendLine(String.Format("MaxCPU = {0}", OmniMIDIConfiguratorMain.Delegate.MaxCPU.Value));
                    SettingsToText.AppendLine(String.Format("NoteOff = {0}", OmniMIDIConfiguratorMain.Delegate.NoteOffCheck.Checked));
                    SettingsToText.AppendLine(String.Format("Preload = {0}", OmniMIDIConfiguratorMain.Delegate.Preload.Checked));
                    SettingsToText.AppendLine(String.Format("SincInter = {0}", OmniMIDIConfiguratorMain.Delegate.SincInter.Checked));
                    SettingsToText.AppendLine(String.Format("SincConv = {0}", OmniMIDIConfiguratorMain.Delegate.SincConv.SelectedIndex));
                    SettingsToText.AppendLine(String.Format("VoiceLimit = {0}", OmniMIDIConfiguratorMain.Delegate.PolyphonyLimit.Value));
                    SettingsToText.AppendLine(String.Format("Volume = {0}", OmniMIDIConfiguratorMain.Delegate.VolTrackBar.Value));
                    SettingsToText.AppendLine();
                    SettingsToText.AppendLine("// Advanced audio settings");
                    SettingsToText.AppendLine(String.Format("AllowKSDAPI = {0}", OmniMIDIConfiguratorMain.SynthSettings.GetValue("KSDAPIEnabled", 1)));
                    SettingsToText.AppendLine(String.Format("HyperMode = {0}", OmniMIDIConfiguratorMain.SynthSettings.GetValue("HyperPlayback", 0)));
                    SettingsToText.AppendLine(String.Format("AudioDepth = {0}", OmniMIDIConfiguratorMain.SynthSettings.GetValue("AudioBitDepth", 1)));
                    SettingsToText.AppendLine(String.Format("FadeOutDisable = {0}", OmniMIDIConfiguratorMain.SynthSettings.GetValue("DisableNotesFadeOut", 0)));
                    SettingsToText.AppendLine(String.Format("MonoRendering = {0}", OmniMIDIConfiguratorMain.SynthSettings.GetValue("MonoRendering", 0)));
                    SettingsToText.AppendLine(String.Format("SlowdownPlayback = {0}", OmniMIDIConfiguratorMain.SynthSettings.GetValue("DontMissNotes", 0)));
                    SettingsToText.AppendLine();
                    SettingsToText.AppendLine("// MIDI events parser settings");
                    SettingsToText.AppendLine(String.Format("CapFramerate = {0}", OmniMIDIConfiguratorMain.SynthSettings.GetValue("CapFramerate", 0)));
                    SettingsToText.AppendLine(String.Format("EVBufferByRAM = {0}", OmniMIDIConfiguratorMain.SynthSettings.GetValue("GetEvBuffSizeFromRAM", 0)));
                    SettingsToText.AppendLine(String.Format("EVBufferRatio = {0}", OmniMIDIConfiguratorMain.SynthSettings.GetValue("EvBufferMultRatio", 1)));
                    SettingsToText.AppendLine(String.Format("EVBufferSize = {0}", OmniMIDIConfiguratorMain.SynthSettings.GetValue("EvBuffSize", 4096)));
                    SettingsToText.AppendLine(String.Format("FullVelocityMode = {0}", OmniMIDIConfiguratorMain.SynthSettings.GetValue("FullVelocityMode", 0)));
                    SettingsToText.AppendLine(String.Format("IgnoreAllNotes = {0}", OmniMIDIConfiguratorMain.SynthSettings.GetValue("IgnoreAllNotes", 0)));
                    SettingsToText.AppendLine(String.Format("IgnoreNotes = {0}", OmniMIDIConfiguratorMain.SynthSettings.GetValue("IgnoreNotesBetweenVel", 1)));
                    SettingsToText.AppendLine(String.Format("Limit88 = {0}", OmniMIDIConfiguratorMain.SynthSettings.GetValue("LimitTo88Keys", 0)));
                    SettingsToText.AppendLine(String.Format("MT32Mode = {0}", OmniMIDIConfiguratorMain.SynthSettings.GetValue("MT32Mode", 0)));
                    SettingsToText.AppendLine(String.Format("SysExIgnore = {0}", OmniMIDIConfiguratorMain.SynthSettings.GetValue("IgnoreSysEx", 0)));
                    SettingsToText.AppendLine();
                    SettingsToText.AppendLine("// Driver mask");
                    SettingsToText.AppendLine(String.Format("SynthName = {0}", OmniMIDIConfiguratorMain.SynthSettings.GetValue("SynthName", "OmniMIDI")));
                    SettingsToText.AppendLine(String.Format("SynthType = {0}", OmniMIDIConfiguratorMain.SynthSettings.GetValue("SynthType", 4)));
                    SettingsToText.AppendLine(String.Format("VID = {0}", OmniMIDIConfiguratorMain.SynthSettings.GetValue("VID", 0xFFFF)));
                    SettingsToText.AppendLine(String.Format("PID = {0}", OmniMIDIConfiguratorMain.SynthSettings.GetValue("PID", 0x000A)));

                    File.WriteAllText(OmniMIDIConfiguratorMain.Delegate.ExportPresetDialog.FileName, SettingsToText.ToString());

                    MessageBox.Show(
                        String.Format("The preset \"{0}\" has been saved to:\n\n{1}",
                        Path.GetFileNameWithoutExtension(OmniMIDIConfiguratorMain.Delegate.ExportPresetDialog.FileName),
                        OmniMIDIConfiguratorMain.Delegate.ExportPresetDialog.FileName), 
                        "OmniMIDI - Export preset", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // Something bad happened hehe
                Program.DebugToConsole(true, null, ex);
                Functions.ShowErrorDialog(ErrorType.Error, System.Media.SystemSounds.Hand, "Fatal error", "Fatal error during the execution of the program.\n\nPress OK to quit", true, ex);
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
                sb.AppendLine("; OmniMIDI Settings File");
                sb.AppendLine("");

                sb.AppendLine(@"[HKEY_CURRENT_USER\SOFTWARE\OmniMIDI\Settings]");
                foreach (var keyname in OmniMIDIConfiguratorMain.SynthSettings.GetValueNames())
                {
                    if (Regex.IsMatch(OmniMIDIConfiguratorMain.SynthSettings.GetValue(keyname).ToString(), @"[a-zA-Z]"))
                        sb.AppendLine(String.Format("\"{0}\"={1}:{2}", keyname, OmniMIDIConfiguratorMain.SynthSettings.GetValueKind(keyname).ToString().ToLower(), OmniMIDIConfiguratorMain.SynthSettings.GetValue(keyname)));
                    else if (Regex.IsMatch(OmniMIDIConfiguratorMain.SynthSettings.GetValue(keyname).ToString(), @"\d+"))
                        sb.AppendLine(String.Format("\"{0}\"={1}:{2}", keyname, OmniMIDIConfiguratorMain.SynthSettings.GetValueKind(keyname).ToString().ToLower(), Convert.ToInt32(OmniMIDIConfiguratorMain.SynthSettings.GetValue(keyname)).ToString("X")));
                    else
                        Program.DebugToConsole(false, String.Format("Unknown value detected on {0}", keyname), null);
                }

                sb.AppendLine("");

                sb.AppendLine(@"[HKEY_CURRENT_USER\SOFTWARE\OmniMIDI\Paths]");
                foreach (var keyname in OmniMIDIConfiguratorMain.SynthPaths.GetValueNames())
                {
                    sb.AppendLine(String.Format("\"{0}\"=\"{1}\"", keyname, OmniMIDIConfiguratorMain.SynthPaths.GetValue(keyname).ToString()));
                }

                sb.AppendLine(Environment.NewLine);

                File.WriteAllText(filename, sb.ToString());

                Program.DebugToConsole(false, String.Format("Settings exported to: {0}", filename), null);
            }
            catch (Exception ex)
            {
                // Something bad happened hehe
                Program.DebugToConsole(true, null, ex);
                Functions.ShowErrorDialog(ErrorType.Error, System.Media.SystemSounds.Hand, "Fatal error", "Fatal error during the execution of the program.\n\nPress OK to quit", true, ex);
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
            OmniMIDIConfiguratorMain.Delegate.RTPrio.Enabled = Status;
            OmniMIDIConfiguratorMain.Delegate.HiPrio.Enabled = Status;
            OmniMIDIConfiguratorMain.Delegate.HNPrio.Enabled = Status;
            OmniMIDIConfiguratorMain.Delegate.NoPrio.Enabled = Status;
            OmniMIDIConfiguratorMain.Delegate.LNPrio.Enabled = Status;
            OmniMIDIConfiguratorMain.Delegate.LoPrio.Enabled = Status;
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
                if (OmniMIDIConfiguratorMain.SynthPaths.GetValue("lastpathsfimport", null) == null)
                {
                    Registry.CurrentUser.CreateSubKey("SOFTWARE\\OmniMIDI\\Paths");
                    OmniMIDIConfiguratorMain.SynthPaths = Registry.CurrentUser.OpenSubKey("SOFTWARE\\OmniMIDI\\Paths", true);
                    OmniMIDIConfiguratorMain.SynthPaths.SetValue("lastpathsfimport", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), RegistryValueKind.String);
                    OmniMIDIConfiguratorMain.LastBrowserPath = OmniMIDIConfiguratorMain.SynthPaths.GetValue("lastpathsfimport", Environment.GetFolderPath(Environment.SpecialFolder.Desktop)).ToString();
                    OmniMIDIConfiguratorMain.Delegate.SoundfontImport.InitialDirectory = OmniMIDIConfiguratorMain.LastBrowserPath;
                }
                else if (OmniMIDIConfiguratorMain.SynthPaths.GetValue("lastpathlistimpexp", null) == null)
                {
                    Registry.CurrentUser.CreateSubKey("SOFTWARE\\OmniMIDI\\Paths");
                    OmniMIDIConfiguratorMain.SynthPaths = Registry.CurrentUser.OpenSubKey("SOFTWARE\\OmniMIDI\\Paths", true);
                    OmniMIDIConfiguratorMain.SynthPaths.SetValue("lastpathlistimpexp", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), RegistryValueKind.String);
                    OmniMIDIConfiguratorMain.LastImportExportPath = OmniMIDIConfiguratorMain.SynthPaths.GetValue("lastpathlistimpexp", Environment.GetFolderPath(Environment.SpecialFolder.Desktop)).ToString();
                    OmniMIDIConfiguratorMain.Delegate.ExternalListImport.InitialDirectory = OmniMIDIConfiguratorMain.LastImportExportPath;
                    OmniMIDIConfiguratorMain.Delegate.ExternalListExport.InitialDirectory = OmniMIDIConfiguratorMain.LastImportExportPath;
                }
                else if (OmniMIDIConfiguratorMain.SynthPaths.GetValue("lastpathsfimport", null) == null && OmniMIDIConfiguratorMain.SynthPaths.GetValue("lastpathlistimpexp", null) == null)
                {
                    Registry.CurrentUser.CreateSubKey("SOFTWARE\\OmniMIDI\\Paths");
                    OmniMIDIConfiguratorMain.SynthPaths = Registry.CurrentUser.OpenSubKey("SOFTWARE\\OmniMIDI\\Paths", true);
                    OmniMIDIConfiguratorMain.SynthPaths.SetValue("lastpathsfimport", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), RegistryValueKind.String);
                    OmniMIDIConfiguratorMain.SynthPaths.SetValue("lastpathlistimpexp", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), RegistryValueKind.String);
                    OmniMIDIConfiguratorMain.LastBrowserPath = OmniMIDIConfiguratorMain.SynthPaths.GetValue("lastpathsfimport", Environment.GetFolderPath(Environment.SpecialFolder.Desktop)).ToString();
                    OmniMIDIConfiguratorMain.LastImportExportPath = OmniMIDIConfiguratorMain.SynthPaths.GetValue("lastpathlistimpexp", Environment.GetFolderPath(Environment.SpecialFolder.Desktop)).ToString();
                    OmniMIDIConfiguratorMain.Delegate.SoundfontImport.InitialDirectory = OmniMIDIConfiguratorMain.LastBrowserPath;
                    OmniMIDIConfiguratorMain.Delegate.ExternalListImport.InitialDirectory = OmniMIDIConfiguratorMain.LastImportExportPath;
                    OmniMIDIConfiguratorMain.Delegate.ExternalListExport.InitialDirectory = OmniMIDIConfiguratorMain.LastImportExportPath;
                }
                else
                {
                    OmniMIDIConfiguratorMain.LastBrowserPath = OmniMIDIConfiguratorMain.SynthPaths.GetValue("lastpathsfimport", Environment.GetFolderPath(Environment.SpecialFolder.Desktop)).ToString();
                    OmniMIDIConfiguratorMain.LastImportExportPath = OmniMIDIConfiguratorMain.SynthPaths.GetValue("lastpathlistimpexp", Environment.GetFolderPath(Environment.SpecialFolder.Desktop)).ToString();
                    OmniMIDIConfiguratorMain.Delegate.SoundfontImport.InitialDirectory = OmniMIDIConfiguratorMain.LastBrowserPath;
                    OmniMIDIConfiguratorMain.Delegate.ExternalListImport.InitialDirectory = OmniMIDIConfiguratorMain.LastImportExportPath;
                    OmniMIDIConfiguratorMain.Delegate.ExternalListExport.InitialDirectory = OmniMIDIConfiguratorMain.LastImportExportPath;
                }
            }
            catch
            {
                Registry.CurrentUser.CreateSubKey("SOFTWARE\\OmniMIDI\\Paths");
                OmniMIDIConfiguratorMain.SynthPaths = Registry.CurrentUser.OpenSubKey("SOFTWARE\\OmniMIDI\\Paths", true);
                OmniMIDIConfiguratorMain.SynthPaths.SetValue("lastpathsfimport", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), RegistryValueKind.String);
                OmniMIDIConfiguratorMain.SynthPaths.SetValue("lastpathlistimpexp", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), RegistryValueKind.String);
                OmniMIDIConfiguratorMain.LastBrowserPath = OmniMIDIConfiguratorMain.SynthPaths.GetValue("lastpathsfimport", Environment.GetFolderPath(Environment.SpecialFolder.Desktop)).ToString();
                OmniMIDIConfiguratorMain.LastImportExportPath = OmniMIDIConfiguratorMain.SynthPaths.GetValue("lastpathlistimpexp", Environment.GetFolderPath(Environment.SpecialFolder.Desktop)).ToString();
                OmniMIDIConfiguratorMain.Delegate.SoundfontImport.InitialDirectory = OmniMIDIConfiguratorMain.LastBrowserPath;
                OmniMIDIConfiguratorMain.Delegate.ExternalListImport.InitialDirectory = OmniMIDIConfiguratorMain.LastImportExportPath;
                OmniMIDIConfiguratorMain.Delegate.ExternalListExport.InitialDirectory = OmniMIDIConfiguratorMain.LastImportExportPath;
                OmniMIDIConfiguratorMain.Delegate.SoundfontImport.InitialDirectory = OmniMIDIConfiguratorMain.LastBrowserPath;
            }
        }

        // WinMM Patch
        public enum MachineType { Native = 0x0000, x86 = 0x014C, Itanium = 0x0200, x64 = 0x8664, ARM64 = 0xAA64 }

        public static MachineType GetAppCompiledMachineType(string fileName)
        {
            const int PE_POINTER_OFFSET = 60, MACHINE_OFFSET = 4;
            byte[] data = new byte[4096];

            using (Stream s = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                s.Read(data, 0, 4096);

            int PE_HEADER_ADDR = BitConverter.ToInt32(data, PE_POINTER_OFFSET);
            int machineUint = BitConverter.ToUInt16(data, PE_HEADER_ADDR + MACHINE_OFFSET);
            return (MachineType)machineUint;
        }

        public static bool IsProcessElevated()
        {
            bool isElevated = false;
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                isElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);
                identity.Dispose();
            }
            return isElevated;
        }

        public static void RestartAsAdmin()
        {
            Boolean isElevated = IsProcessElevated();

            String MessageString = String.Format("You don't have the rights to edit the target folder!{0}", isElevated ? "" : "\n\nDo you want to restart the configurator with admin permissions?");
            MessageBoxButtons MessageButtons = isElevated ? MessageBoxButtons.OK : MessageBoxButtons.YesNo;
            MessageBoxIcon MessageIcon = isElevated ? MessageBoxIcon.Hand : MessageBoxIcon.Exclamation;

            DialogResult Message = MessageBox.Show(MessageString, "OmniMIDI - Permissions error", MessageButtons, MessageIcon);
            if (Message == DialogResult.Yes)
            {
                ProcessStartInfo elevated = new ProcessStartInfo(System.Reflection.Assembly.GetEntryAssembly().Location, "/WINMMWRP");
                elevated.UseShellExecute = true;
                elevated.Verb = "runas";
                Process.Start(elevated);
                Application.ExitThread();
            }
        }

        public static Boolean ApplyWinMMWRPPatch(Boolean DAWMode)
        {
            if ((Environment.OSVersion.Version.Major <= 6 && Environment.OSVersion.Version.Minor < 2) && Properties.Settings.Default.PatchInfoShow == true)
            {
                Properties.Settings.Default.PatchInfoShow = false;
                Properties.Settings.Default.Save();
                MessageBox.Show("The patch is not needed on Windows 7 and older, but you can install it anyway.", "OmniMIDI - Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            OpenFileDialog WinMMDialog = new OpenFileDialog();
            try
            {
                WinMMDialog.Filter = "Executables (*.exe)|*.exe;";
                WinMMDialog.Title = "Select an application to patch";
                WinMMDialog.Multiselect = false;
                WinMMDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                if (WinMMDialog.ShowDialog() == DialogResult.OK)
                {
                    MachineType BitApp = GetAppCompiledMachineType(WinMMDialog.FileName);
                    String DirectoryPath = Path.GetDirectoryName(WinMMDialog.FileName);

                    if (DirectoryPath.Contains(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32")) ||
                        DirectoryPath.Contains(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "SysWOW64")) ||
                        DirectoryPath.Contains(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "SysArm32")))
                    {
                        ShowErrorDialog(ErrorType.Error, System.Media.SystemSounds.Exclamation, "Error", "I'm afraid I can't do that, Dave.", false, null);
                        WinMMDialog.Dispose();
                        return false;
                    }

                    RemovePatchFiles(WinMMDialog.FileName, true);
                    if (BitApp == MachineType.x86)
                        File.WriteAllBytes(String.Format("{0}\\{1}", DirectoryPath, "winmm.dll"), DAWMode ? Properties.Resources.winmm32DAW : Properties.Resources.winmm32wrp);
                    else if (BitApp == MachineType.x64)
                        File.WriteAllBytes(String.Format("{0}\\{1}", DirectoryPath, "winmm.dll"), DAWMode ? Properties.Resources.winmm64DAW : Properties.Resources.winmm64wrp);
                    else if (BitApp == MachineType.ARM64)
                        File.WriteAllBytes(String.Format("{0}\\{1}", DirectoryPath, "winmm.dll"), DAWMode ? Properties.Resources.winmmARM64DAW : Properties.Resources.winmmARM64wrp);
                    else
                    {
                        ShowErrorDialog(ErrorType.Error, System.Media.SystemSounds.Exclamation, "Error", "Unable to patch the following executable!\nThe configurator can only patch x86, x86-64 and ARM64 executables.\n\nPress OK to continue", false, null);
                        WinMMDialog.Dispose();
                        return false;
                    }

                    WinMMDialog.Dispose();
                    return true;
                }
            }
            catch { RestartAsAdmin(); }

            WinMMDialog.Dispose();
            return false;
        }

        public static Boolean RemoveWinMMPatch()
        {
            OpenFileDialog WinMMDialog = new OpenFileDialog();
            try
            {
                WinMMDialog.Filter = "Executables (*.exe)|*.exe;";
                WinMMDialog.Title = "Select an application to unpatch";
                WinMMDialog.Multiselect = false;
                WinMMDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                if (WinMMDialog.ShowDialog() == DialogResult.OK)
                {
                    String DirectoryPath = Path.GetDirectoryName(WinMMDialog.FileName);

                    if (DirectoryPath.Contains(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32")) ||
                        DirectoryPath.Contains(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "SysWOW64")) ||
                        DirectoryPath.Contains(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "SysArm32")))
                    {
                        ShowErrorDialog(ErrorType.Error, System.Media.SystemSounds.Exclamation, "Error", "I'm afraid I can't do that, Dave.", false, null);
                        WinMMDialog.Dispose();
                        return false;
                    }

                    RemovePatchFiles(WinMMDialog.FileName, false);
                }

                return true;
            }
            catch { RestartAsAdmin(); }

            return false;
        }

        private static void RemovePatchFiles(String DirectoryPath, Boolean Silent)
        {
            String[] DeleteTheseFiles = { "midimap.dll", "msacm32.drv", "msacm32.dll", "msapd32.drv", "msapd32.dll", "wdmaud.drv", "wdmaud.sys", "winmm.dll", "owinmm.dll" };

            foreach (String DeleteMe in DeleteTheseFiles)
                File.Delete(String.Format("{0}\\{1}", Path.GetDirectoryName(DirectoryPath), DeleteMe));

            if (!Silent) MessageBox.Show(String.Format("\"{0}\" has been succesfully unpatched!", Path.GetFileName(DirectoryPath)), "OmniMIDI - Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static bool CheckDarkTheme()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize", false))
            {
                if (key != null)
                {
                    // Windows is using the dark theme
                    if (Convert.ToBoolean(key.GetValue("AppsUseLightTheme", 1)) != false)
                        return true;
                }
            }

            // Windows is not using the dark theme
            return false;
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