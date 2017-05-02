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

namespace KeppySynthConfigurator
{
    class Functions
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool Wow64RevertWow64FsRedirection(IntPtr ptr);

        public static Color SFEnabled = Color.FromArgb(0, 0, 0);
        public static Color SFDisabled = Color.FromArgb(170, 170, 170);

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

        public static void UserProfileMigration() // Migrates the Keppy's Synthesizer folder from %localappdata% (Unsupported on XP) to %userprofile% (Supported on XP, now used on Vista+ too)
        {
            try
            {
                string oldlocation = System.Environment.GetEnvironmentVariable("LOCALAPPDATA") + "\\Keppy's Synthesizer\\";
                string newlocation = System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Keppy's Synthesizer\\";
                Directory.Move(oldlocation, newlocation);
            }
            catch
            {

            }
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
            catch
            {

            }
        }

        public static void OpenSFWithDefaultApp(String SoundFont) // Basically changes the directory's name
        {
            try
            {
                if (SoundFont.ToLower().IndexOf('=') != -1)
                {
                    var matches = System.Text.RegularExpressions.Regex.Matches(SoundFont, "[0-9]+");
                    string sf = SoundFont.Substring(SoundFont.LastIndexOf('|') + 1);
                    Process.Start(sf);
                }
                else if (SoundFont.ToLower().IndexOf('@') != -1)
                {
                    string sf = SoundFont.Substring(SoundFont.LastIndexOf('@') + 1);
                    Process.Start(sf);
                }
                else
                {
                    Process.Start(SoundFont);
                }
            }
            catch
            {

            }
        }

        public static void DriverRegistry(int integer)
        {
            try
            {
                if (integer == 0)
                {
                    var process = System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\keppysynth\\KSDriverRegister.exe", "/registerv");
                    process.WaitForExit();
                }
                else
                {
                    var process = System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\keppysynth\\KSDriverRegister.exe", "/unregisterv");
                    process.WaitForExit();
                }
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
            SecretDialog frm = new SecretDialog(Type, sound, title, message);
            Program.DebugToConsole(IsException, null, ex);
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

        public static void LoudMaxInstall()
        {
            try
            {
                int isalreadyinstalled = 0;
                string userfolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Keppy's Synthesizer";
                string loudmax32 = "https://raw.githubusercontent.com/KaleidonKep99/Keppy-s-Synthesizer/master/external_packages/lib/LoudMax.dll";
                string loudmax64 = "https://raw.githubusercontent.com/KaleidonKep99/Keppy-s-Synthesizer/master/external_packages/lib64/LoudMax64.dll";

                // 32-bit DLL
                if (!File.Exists(userfolder + "\\LoudMax.dll"))
                {
                    Forms.DLEngine frm = new Forms.DLEngine(null, "Downloading LoudMax 32-bit...", loudmax32, 1, false);
                    frm.StartPosition = FormStartPosition.CenterScreen;
                    frm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("LoudMax 32-bit seems to be already installed.", "Keppy's Synthesizer - Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    isalreadyinstalled++;
                }

                // 64-bit DLL
                if (Environment.Is64BitOperatingSystem)
                {
                    if (!File.Exists(userfolder + "\\LoudMax64.dll"))
                    {
                        Forms.DLEngine frm = new Forms.DLEngine(null, "Downloading LoudMax 64-bit...", loudmax64, 1, false);
                        frm.StartPosition = FormStartPosition.CenterScreen;
                        frm.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("LoudMax 64-bit seems to be already installed.", "Keppy's Synthesizer - Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        isalreadyinstalled++;
                    }
                }

                if (isalreadyinstalled != 2)
                {
                    MessageBox.Show("LoudMax successfully installed!", "Keppy's Synthesizer - Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Functions.ShowErrorDialog(1, System.Media.SystemSounds.Exclamation, "LoudMax Installation", "Crap, an error!\nAre you sure you have a working Internet connection?", true, ex);
            }
        }

        public static void LoudMaxUninstall()
        {
            try
            {
                int isalreadyuninstalled = 0;
                string userfolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Keppy's Synthesizer";

                // 32-bit DLL
                if (File.Exists(userfolder + "\\LoudMax.dll"))
                {
                    File.Delete(userfolder + "\\LoudMax.dll");
                }
                else
                {
                    MessageBox.Show("LoudMax 32-bit seems to be already uninstalled.", "Keppy's Synthesizer - Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    isalreadyuninstalled++;
                }

                // 64-bit DLL
                if (File.Exists(userfolder + "\\LoudMax64.dll"))
                {
                    File.Delete(userfolder + "\\LoudMax64.dll");
                }
                else
                {
                    MessageBox.Show("LoudMax 64-bit seems to be already uninstalled.", "Keppy's Synthesizer - Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    isalreadyuninstalled++;
                }

                if (isalreadyuninstalled != 2)
                {
                    MessageBox.Show("LoudMax successfully uninstalled!", "Keppy's Synthesizer - Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Functions.ShowErrorDialog(1, System.Media.SystemSounds.Exclamation, "LoudMax Installation", "Crap, an error!\nAre you sure you closed all the apps using the driver? They might have locked LoudMax.", true, ex);
            }
        }

        public static string StripSFZValues(string SFToStrip)
        {
            if (SFToStrip.ToLower().IndexOf('=') != -1)
            {
                var matches = System.Text.RegularExpressions.Regex.Matches(SFToStrip, "[0-9]+");
                return SFToStrip.Substring(SFToStrip.LastIndexOf('|') + 1);
            }
            else
            {
                return SFToStrip;
            }
        }

        public static void SaveList(String SelectedList) // Saves the selected list to the hard drive
        {
            if (!KeppySynthConfiguratorMain.AvoidSave)
            {
                using (StreamWriter sw = new StreamWriter(SelectedList))
                {
                    foreach (ListViewItem item in KeppySynthConfiguratorMain.Delegate.Lis.Items)
                    {
                        String FirstChar;

                        if (item.ForeColor == SFEnabled)
                            FirstChar = "";
                        else if (item.ForeColor == SFDisabled)
                            FirstChar = "@";
                        else
                            FirstChar = "";

                        sw.WriteLine(String.Format("{0}{1}", FirstChar, item.Text.ToString()));
                    }
                }
                Program.DebugToConsole(false, String.Format("Soundfont list saved: {0}", SelectedList), null);
            }
        }

        // -------------------------
        // Soundfont lists functions

        public static void TriggerReload() // Tells Keppy's Synthesizer to load a specific list
        {
            try
            {
                if (Convert.ToInt32(KeppySynthConfiguratorMain.Watchdog.GetValue("currentsflist")) == KeppySynthConfiguratorMain.whichone)
                {
                    KeppySynthConfiguratorMain.Watchdog.SetValue("rel" + KeppySynthConfiguratorMain.whichone.ToString(), "1", RegistryValueKind.DWord);
                }
                Program.DebugToConsole(false, String.Format("(Re)Loaded soundfont list {0}.", KeppySynthConfiguratorMain.whichone), null);
                KeppySynthConfiguratorMain.Delegate.Lis.Refresh();
            }
            catch
            {
                Functions.InitializeLastPath();
            }
        }

        public static void SetLastPath(string path) // Saves the last path from the SoundfontImport dialog to the registry 
        {
            try
            {
                KeppySynthConfiguratorMain.LastBrowserPath = path;
                KeppySynthConfiguratorMain.SynthPaths.SetValue("lastpathsfimport", path);
                Program.DebugToConsole(false, String.Format("Last Explorer path is: ", path), null);
            }
            catch
            {
                Functions.InitializeLastPath();
            }
        }

        public static void SetLastImportExportPath(string path) // Saves the last path from the ExternalListImport/ExternalListExport dialog to the registry 
        {
            try
            {
                KeppySynthConfiguratorMain.LastImportExportPath = path;
                KeppySynthConfiguratorMain.SynthPaths.SetValue("lastpathlistimpexp", path);
                Program.DebugToConsole(false, String.Format("Last Import/Export path is: ", path), null);
            }
            catch
            {
                Functions.InitializeLastPath();
            }
        }

        public static void SetLastMIDIPath(string path) // Saves the last path from the SoundFont preview dialog to the registry 
        {
            try
            {
                SoundFontInfo.LastMIDIPath = path;
                KeppySynthConfiguratorMain.SynthPaths.SetValue("lastpathmidimport", path);
                Program.DebugToConsole(false, String.Format("Last MIDI preview path is: ", path), null);
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
            catch
            {
                return;
            }
        }
        // NOT SUPPORTED ON XP

        public static void SetDefaultDevice(string engine, int dev)
        {
            if (engine == "DirectSound")
                KeppySynthConfiguratorMain.SynthSettings.SetValue("defaultdev", dev, RegistryValueKind.DWord);
            else if (engine == "ASIO")
                KeppySynthConfiguratorMain.SynthSettings.SetValue("defaultAdev", dev, RegistryValueKind.DWord);
            else if (engine == "WASAPI")
                KeppySynthConfiguratorMain.SynthSettings.SetValue("defaultWdev", dev, RegistryValueKind.DWord);
        }

        public static void SetDefaultMIDIInDevice(int dev)
        {
            KeppySynthConfiguratorMain.SynthSettings.SetValue("defaultmidiindev", dev, RegistryValueKind.DWord);
        }

        public static void LoadSettings() // Loads the settings from the registry
        {
            // ======= Load settings from the registry
            try
            {
                // First, the most important settings
                KeppySynthConfiguratorMain.Delegate.PolyphonyLimit.Value = Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("polyphony", 512));
                KeppySynthConfiguratorMain.Delegate.MaxCPU.Value = Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("cpu", 75));
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("defaultmidiout", 0)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.SetSynthDefault.Checked = true;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("allhotkeys", 0)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.hotkeys.Checked = true;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("alternativecpu", 0)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.autopanicmode.Checked = true;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("capframerate", 1)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.CapFram.Checked = true;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("shortname", 0)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.MaskSynthesizerAsAnother.Enabled = false;
                    KeppySynthConfiguratorMain.Delegate.MIDINameNoSpace.Checked = true;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("fadeoutdisable", 0)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.FadeoutDisable.Checked = true;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("monorendering", 0)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.MonophonicFunc.Checked = true;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("sysexignore", 0)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.SysExIgnore.Checked = true;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("fullvelocity", 0)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.FullVelocityMode.Checked = true;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("turnnoteoffintonoteon", 0)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.NoteOFFtoON.Checked = true;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("driverprio", 0)) == 0)
                {
                    Functions.ButtonStatus(false);
                    KeppySynthConfiguratorMain.Delegate.DePrio.Checked = true;
                }
                else if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("driverprio", 0)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.RTPrio.Checked = true;
                }
                else if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("driverprio", 0)) == 2)
                {
                    KeppySynthConfiguratorMain.Delegate.HiPrio.Checked = true;
                }
                else if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("driverprio", 0)) == 3)
                {
                    KeppySynthConfiguratorMain.Delegate.HNPrio.Checked = true;
                }
                else if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("driverprio", 0)) == 4)
                {
                    KeppySynthConfiguratorMain.Delegate.NoPrio.Checked = true;
                }
                else if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("driverprio", 0)) == 5)
                {
                    KeppySynthConfiguratorMain.Delegate.LNPrio.Checked = true;
                }
                else
                {
                    KeppySynthConfiguratorMain.Delegate.LoPrio.Checked = true;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("allnotesignore", 0)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.AllNotesIgnore.Checked = true;
                    KeppySynthConfiguratorMain.Delegate.SysExIgnore.Checked = true;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("vms2emu", 0)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.slowdownnoskip.Checked = true;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("debugmode", 0)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.DebugModePls.Checked = true;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("ignorenotes1", 0)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.IgnoreNotes1.Checked = true;
                    KeppySynthConfiguratorMain.Delegate.IgnoreNotesInterval.Enabled = true;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("oldbuffersystem", 0)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.useoldbuffersystem.Checked = true;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("autoupdatecheck", 1)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.autoupdate.Checked = true;
                }
                int floatingpointaudioval = Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("32bit", 1));
                if (floatingpointaudioval == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.floatingpointaudio.Checked = true;
                    KeppySynthConfiguratorMain.Delegate.bit16audio.Checked = false;
                    KeppySynthConfiguratorMain.Delegate.bit8audio.Checked = false;
                }
                else if (floatingpointaudioval == 2 || floatingpointaudioval == 0)
                {
                    KeppySynthConfiguratorMain.Delegate.floatingpointaudio.Checked = false;
                    KeppySynthConfiguratorMain.Delegate.bit16audio.Checked = true;
                    KeppySynthConfiguratorMain.Delegate.bit8audio.Checked = false;
                }
                else if (floatingpointaudioval == 3)
                {
                    KeppySynthConfiguratorMain.Delegate.floatingpointaudio.Checked = false;
                    KeppySynthConfiguratorMain.Delegate.bit16audio.Checked = false;
                    KeppySynthConfiguratorMain.Delegate.bit8audio.Checked = true;
                }
                else
                {
                    KeppySynthConfiguratorMain.Delegate.floatingpointaudio.Checked = true;
                    KeppySynthConfiguratorMain.Delegate.bit16audio.Checked = false;
                    KeppySynthConfiguratorMain.Delegate.bit8audio.Checked = false;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("debugmode", 0)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.DebugModePls.Checked = true;
                }
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
                KeppySynthConfiguratorMain.Delegate.Frequency.Text = KeppySynthConfiguratorMain.SynthSettings.GetValue("frequency", 44100).ToString();
                KeppySynthConfiguratorMain.Delegate.SPFRate.Value = Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("sndbfvalue", 16));

                // Then the filthy checkboxes
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("preload", 1)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.Preload.Checked = true;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("nofx", 0)) == 0)
                {
                    KeppySynthConfiguratorMain.Delegate.EnableSFX.Checked = true;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("noteoff", 0)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.NoteOffCheck.Checked = true;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("sysresetignore", 0)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.SysResetIgnore.Checked = true;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("encmode", 0)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.OutputWAV.Checked = true;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("rco", 1)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.SleepStateRCO.Checked = false;
                }
                else
                {
                    KeppySynthConfiguratorMain.Delegate.SleepStateRCO.Checked = true;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("xaudiodisabled", 0)) == 0)
                {
                    KeppySynthConfiguratorMain.Delegate.AudioEngBox.Text = "XAudio";
                    KeppySynthConfiguratorMain.Delegate.ManualAddBuffer.Visible = false;
                    KeppySynthConfiguratorMain.Delegate.SleepStateRCO.Enabled = false;
                    KeppySynthConfiguratorMain.Delegate.bufsize.Enabled = true;
                    KeppySynthConfiguratorMain.Delegate.ChangeDefaultOutput.Enabled = false;
                }
                else if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("xaudiodisabled", 0)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.AudioEngBox.Text = "DirectSound";
                    KeppySynthConfiguratorMain.Delegate.ManualAddBuffer.Visible = true;
                    KeppySynthConfiguratorMain.Delegate.SleepStateRCO.Enabled = true;
                    KeppySynthConfiguratorMain.Delegate.changeDirectoryOfTheOutputToWAVModeToolStripMenuItem.Enabled = false;
                    KeppySynthConfiguratorMain.Delegate.ChangeDefaultOutput.Enabled = true;
                    if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("vmsemu", 0)) == 1)
                    {
                        KeppySynthConfiguratorMain.Delegate.ManualAddBuffer.Checked = true;
                        KeppySynthConfiguratorMain.Delegate.bufsize.Enabled = true;
                    }
                    else
                    {
                        KeppySynthConfiguratorMain.Delegate.ManualAddBuffer.Checked = false;
                        KeppySynthConfiguratorMain.Delegate.bufsize.Enabled = false;
                        KeppySynthConfiguratorMain.Delegate.bufsize.Value = 0;
                    }
                }
                else if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("xaudiodisabled", 0)) == 2)
                {
                    KeppySynthConfiguratorMain.Delegate.AudioEngBox.Text = "ASIO";
                    KeppySynthConfiguratorMain.Delegate.menuItem32.Enabled = false;
                    KeppySynthConfiguratorMain.Delegate.ManualAddBuffer.Visible = false;
                    KeppySynthConfiguratorMain.Delegate.SleepStateRCO.Enabled = false;
                    KeppySynthConfiguratorMain.Delegate.bufsize.Enabled = true;
                    KeppySynthConfiguratorMain.Delegate.ChangeDefaultOutput.Enabled = true;
                }
                else if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("xaudiodisabled", 0)) == 3)
                {
                    KeppySynthConfiguratorMain.Delegate.AudioEngBox.Text = "WASAPI";
                    KeppySynthConfiguratorMain.Delegate.menuItem32.Enabled = false;
                    KeppySynthConfiguratorMain.Delegate.ManualAddBuffer.Visible = false;
                    KeppySynthConfiguratorMain.Delegate.SleepStateRCO.Enabled = false;
                    KeppySynthConfiguratorMain.Delegate.bufsize.Enabled = true;
                    KeppySynthConfiguratorMain.Delegate.ChangeDefaultOutput.Enabled = true;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("sinc", 0)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.SincInter.Checked = true;
                }

                // LEL
                KeppySynthConfiguratorMain.Delegate.bufsize.Value = Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("buflen"));

                if (Environment.OSVersion.Version.Major == 10)
                    KeppySynthConfiguratorMain.Delegate.SpatialSound.Visible = true;

                // And finally, the volume!
                int VolumeValue = Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("volume", 10000));
                KeppySynthConfiguratorMain.Delegate.VolTrackBar.Value = VolumeValue;
                decimal VolVal = (decimal)VolumeValue / 100;
                if (KeppySynthConfiguratorMain.Delegate.VolTrackBar.Value <= 49)
                    KeppySynthConfiguratorMain.Delegate.VolSimView.ForeColor = Color.Red;
                else
                    KeppySynthConfiguratorMain.Delegate.VolSimView.ForeColor = Color.Blue;
                KeppySynthConfiguratorMain.Delegate.VolSimView.Text = String.Format("{0}%", Math.Round(VolVal, MidpointRounding.AwayFromZero).ToString("000"));
                KeppySynthConfiguratorMain.Delegate.VolIntView.Text = String.Format("Real value: {0}%", VolVal.ToString("000.00"));
                Program.DebugToConsole(false, "Done loading settings.", null);
            }
            catch (Exception ex)
            {
                Program.DebugToConsole(true, null, ex);
                ReinitializeSettings();
            }
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
                if (!Environment.Is64BitOperatingSystem)
                {
                    KeppySynthConfiguratorMain.Delegate.WinMMPatch32.Enabled = true;
                }
                else
                {
                    KeppySynthConfiguratorMain.Delegate.WinMMPatch32.Enabled = true;
                    KeppySynthConfiguratorMain.Delegate.WinMMPatch64.Enabled = true;
                }
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
                    KeppySynthConfiguratorMain.Delegate.WinMMPatch32.Enabled = true;
                }
                else
                {
                    KeppySynthConfiguratorMain.Delegate.changeDefaultMIDIOutDeviceToolStripMenuItem1.Visible = false;
                    KeppySynthConfiguratorMain.Delegate.changeDefaultMIDIOutDeviceToolStripMenuItem.Visible = true;
                    KeppySynthConfiguratorMain.Delegate.changeDefault64bitMIDIOutDeviceToolStripMenuItem.Visible = true;
                    KeppySynthConfiguratorMain.Delegate.WinMMPatch32.Enabled = true;
                    KeppySynthConfiguratorMain.Delegate.WinMMPatch64.Enabled = true;
                }
            }
        }

        public static void SaveSettings() // Saves the settings to the registry 
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
                KeppySynthConfiguratorMain.SynthSettings.SetValue("sndbfvalue", KeppySynthConfiguratorMain.Delegate.SPFRate.Value.ToString(), RegistryValueKind.DWord);

                // Let's not forget about the volume!
                decimal VolVal = (decimal)KeppySynthConfiguratorMain.Delegate.VolTrackBar.Value / 100;
                KeppySynthConfiguratorMain.Delegate.VolSimView.Text = String.Format("{0}%", Math.Round(VolVal, MidpointRounding.AwayFromZero).ToString("000"));
                KeppySynthConfiguratorMain.Delegate.VolIntView.Text = String.Format("Real value: {0}%", VolVal.ToString("000.00"));
                KeppySynthConfiguratorMain.SynthSettings.SetValue("volume", KeppySynthConfiguratorMain.Delegate.VolTrackBar.Value.ToString(), RegistryValueKind.DWord);

                // Checkbox stuff yay
                if (KeppySynthConfiguratorMain.Delegate.Preload.Checked == true)
                {
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("preload", "1", RegistryValueKind.DWord);
                }
                else
                {
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("preload", "0", RegistryValueKind.DWord);
                }
                if (KeppySynthConfiguratorMain.Delegate.EnableSFX.Checked == true)
                {
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("nofx", "0", RegistryValueKind.DWord);
                }
                else
                {
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("nofx", "1", RegistryValueKind.DWord);
                }
                if (KeppySynthConfiguratorMain.Delegate.ManualAddBuffer.Checked == true)
                {
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("vmsemu", "1", RegistryValueKind.DWord);
                }
                else
                {
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("vmsemu", "0", RegistryValueKind.DWord);
                }
                if (KeppySynthConfiguratorMain.Delegate.NoteOffCheck.Checked == true)
                {
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("noteoff", "1", RegistryValueKind.DWord);
                }
                else
                {
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("noteoff", "0", RegistryValueKind.DWord);
                }
                if (KeppySynthConfiguratorMain.Delegate.SysResetIgnore.Checked == true)
                {
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("sysresetignore", "1", RegistryValueKind.DWord);
                }
                else
                {
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("sysresetignore", "0", RegistryValueKind.DWord);
                }
                if (KeppySynthConfiguratorMain.Delegate.OutputWAV.Checked == true)
                {
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("encmode", "1", RegistryValueKind.DWord);
                }
                else
                {
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("encmode", "0", RegistryValueKind.DWord);
                }
                if (KeppySynthConfiguratorMain.Delegate.AudioEngBox.Text == "DirectSound")
                {
                    if (KeppySynthConfiguratorMain.Delegate.SincInter.Checked == true)
                    {
                        KeppySynthConfiguratorMain.SynthSettings.SetValue("sinc", "1", RegistryValueKind.DWord);
                    }
                    else
                    {
                        KeppySynthConfiguratorMain.SynthSettings.SetValue("sinc", "0", RegistryValueKind.DWord);
                    }
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("xaudiodisabled", "1", RegistryValueKind.DWord);
                }
                else if (KeppySynthConfiguratorMain.Delegate.AudioEngBox.Text == "XAudio")
                {
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("sinc", "0", RegistryValueKind.DWord);
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("xaudiodisabled", "0", RegistryValueKind.DWord);
                }
                else if (KeppySynthConfiguratorMain.Delegate.AudioEngBox.Text == "ASIO")
                {
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("sinc", "0", RegistryValueKind.DWord);
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("xaudiodisabled", "2", RegistryValueKind.DWord);
                }
                else if (KeppySynthConfiguratorMain.Delegate.AudioEngBox.Text == "WASAPI")
                {
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("sinc", "0", RegistryValueKind.DWord);
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("xaudiodisabled", "3", RegistryValueKind.DWord);
                }
                Program.DebugToConsole(false, "Done saving settings.", null);
            }
            catch (Exception ex)
            {
                // Something bad happened hehe
                Program.DebugToConsole(true, null, ex);
                ReinitializeSettings();
            }
        }

        public static void ReinitializeSettings() // If the registry is missing, reset it
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
                VolumeValue = Convert.ToInt32(x);
                KeppySynthConfiguratorMain.Delegate.VolSimView.Text = VolumeValue.ToString("000\\%");
                KeppySynthConfiguratorMain.Delegate.VolIntView.Text = "Value: " + KeppySynthConfiguratorMain.Delegate.VolTrackBar.Value.ToString("00000");
                KeppySynthConfiguratorMain.SynthSettings.SetValue("volume", KeppySynthConfiguratorMain.Delegate.VolTrackBar.Value.ToString(), RegistryValueKind.DWord);

                // Checkbox stuff yay
                KeppySynthConfiguratorMain.SynthSettings.SetValue("preload", "1", RegistryValueKind.DWord);
                KeppySynthConfiguratorMain.SynthSettings.SetValue("nofx", "0", RegistryValueKind.DWord);
                KeppySynthConfiguratorMain.SynthSettings.SetValue("vmsemu", "0", RegistryValueKind.DWord);
                KeppySynthConfiguratorMain.SynthSettings.SetValue("noteoff", "0", RegistryValueKind.DWord);
                KeppySynthConfiguratorMain.SynthSettings.SetValue("sysresetignore", "0", RegistryValueKind.DWord);
                KeppySynthConfiguratorMain.SynthSettings.SetValue("encmode", "0", RegistryValueKind.DWord);
                KeppySynthConfiguratorMain.SynthSettings.SetValue("sinc", "0", RegistryValueKind.DWord);
                KeppySynthConfiguratorMain.SynthSettings.SetValue("xaudiodisabled", "0", RegistryValueKind.DWord);

                // Reload the settings
                LoadSettings();
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

        public static void ImportSettings(String filename)
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

                    Functions.LoadSettings();
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
                MessageBox.Show("Fatal error during the execution of this program!\n\nPress OK to quit.", "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                Application.Exit();
            }
        }

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
                Functions.ShowErrorDialog(1, System.Media.SystemSounds.Hand, "Fatal error", "Fatal error during the execution of the program.\n\nPress OK to quit", true, ex);
                Application.Exit();
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

        public static string ReturnLength(long length)
        {
            try
            {
                if (length >= 1099511627776)
                {
                    if (length >= 1099511627776 && length < 10995116277760)
                        return ((((length / 1024f) / 1024f) / 1024f) / 1024f).ToString("0.00 TB");
                    else
                        return ((((length / 1024f) / 1024f) / 1024f) / 1024f).ToString("0.0 TB");
                }
                else if (length >= 1073741824)
                {
                    if (length >= 1073741824 && length < 10737418240)
                        return (((length / 1024f) / 1024f) / 1024f).ToString("0.00 GB");
                    else
                        return (((length / 1024f) / 1024f) / 1024f).ToString("0.0 GB");
                }
                else if (length >= 1048576)
                {
                    if (length >= 1048576 && length < 10485760)
                        return ((length / 1024f) / 1024f).ToString("0.00 MB");
                    else
                        return ((length / 1024f) / 1024f).ToString("0.0 MB");
                }
                else if (length >= 1024)
                {
                    if (length >= 1024 && length < 10240)
                        return (length / 1024f).ToString("0.00 KB");
                    else
                        return (length / 1024f).ToString("0.0 KB");
                }
                else
                {
                    if (length >= 1 && length < 1024)
                        return (length).ToString("0.00 B");
                    else
                        return (length / 1024f).ToString("0.0 B");
                }
            }
            catch { return "-"; }
        }

        public static string ReturnSoundFontSize(string ext, long length)
        {
            if (ext.ToLowerInvariant() != ".sfz")
            {
                string size = Functions.ReturnLength(length);
                return size;
            }
            else
            {
                return "N/A";
            }
        }

        public static string ReturnSoundFontFormat(string fileext)
        {
            if (fileext.ToLowerInvariant() == ".sf1")
                return "SF1";
            else if (fileext.ToLowerInvariant() == ".sf2")
                return "SF2";
            else if (fileext.ToLowerInvariant() == ".sfz")
                return "SFZ";
            else if (fileext.ToLowerInvariant() == ".ssx")
                return "Enc. SF";
            else if (fileext.ToLowerInvariant() == ".sfpack")
                return "SF Pack";
            else if (fileext.ToLowerInvariant() == ".sfark")
                return "SF Arch.";
            else
                return "N/A";
        }

        public static string ReturnSoundFontFormatMore(string fileext)
        {
            if (fileext.ToLowerInvariant() == ".sf1")
                return "SoundFont 1.x by E-mu Systems";
            else if (fileext.ToLowerInvariant() == ".sf2")
                return "SoundFont 2.x by Creative Labs";
            else if (fileext.ToLowerInvariant() == ".sfz")
                return "SoundFontZ by Cakewalk™";
            else if (fileext.ToLowerInvariant() == ".ssx")
                return "Princess Soft Encrypted SoundFont";
            else if (fileext.ToLowerInvariant() == ".sfpack")
                return "SoundFont compressed package";
            else if (fileext.ToLowerInvariant() == ".sfark")
                return "SoundFont Archive (SfARK)";
            else
                return "Unknown format";
        }

        private static Color ReturnColor(String result)
        {
            if (result == "@")
                return SFDisabled;
            else
                return SFEnabled;
        }

        public static void ChangeList(int SelectedList) // When you select a list from the combobox, it'll load the items from the selected list to the listbox
        {
            if (SelectedList == 1)
                KeppySynthConfiguratorMain.CurrentList = KeppySynthConfiguratorMain.List1Path;
            else if (SelectedList == 2)
                KeppySynthConfiguratorMain.CurrentList = KeppySynthConfiguratorMain.List2Path;
            else if (SelectedList == 3)
                KeppySynthConfiguratorMain.CurrentList = KeppySynthConfiguratorMain.List3Path;
            else if (SelectedList == 4)
                KeppySynthConfiguratorMain.CurrentList = KeppySynthConfiguratorMain.List4Path;
            else if (SelectedList == 5)
                KeppySynthConfiguratorMain.CurrentList = KeppySynthConfiguratorMain.List5Path;
            else if (SelectedList == 6)
                KeppySynthConfiguratorMain.CurrentList = KeppySynthConfiguratorMain.List6Path;
            else if (SelectedList == 7)
                KeppySynthConfiguratorMain.CurrentList = KeppySynthConfiguratorMain.List7Path;
            else if (SelectedList == 8)
                KeppySynthConfiguratorMain.CurrentList = KeppySynthConfiguratorMain.List8Path;
            else if (SelectedList == 9)
                KeppySynthConfiguratorMain.CurrentList = KeppySynthConfiguratorMain.List9Path;
            else if (SelectedList == 10)
                KeppySynthConfiguratorMain.CurrentList = KeppySynthConfiguratorMain.List10Path;
            else if (SelectedList == 11)
                KeppySynthConfiguratorMain.CurrentList = KeppySynthConfiguratorMain.List11Path;
            else if (SelectedList == 12)
                KeppySynthConfiguratorMain.CurrentList = KeppySynthConfiguratorMain.List12Path;
            else if (SelectedList == 13)
                KeppySynthConfiguratorMain.CurrentList = KeppySynthConfiguratorMain.List13Path;
            else if (SelectedList == 14)
                KeppySynthConfiguratorMain.CurrentList = KeppySynthConfiguratorMain.List14Path;
            else if (SelectedList == 15)
                KeppySynthConfiguratorMain.CurrentList = KeppySynthConfiguratorMain.List15Path;
            else if (SelectedList == 16)
                KeppySynthConfiguratorMain.CurrentList = KeppySynthConfiguratorMain.List16Path;
            else
                KeppySynthConfiguratorMain.CurrentList = KeppySynthConfiguratorMain.List1Path;

            KeppySynthConfiguratorMain.whichone = SelectedList;
            String WhichList = KeppySynthConfiguratorMain.CurrentList;

            try
            {
                if (!System.IO.Directory.Exists(KeppySynthConfiguratorMain.AbsolutePath))
                {
                    Directory.CreateDirectory(KeppySynthConfiguratorMain.AbsolutePath);
                    Directory.CreateDirectory(KeppySynthConfiguratorMain.ListsPath);
                    File.Create(WhichList).Dispose();
                    KeppySynthConfiguratorMain.Delegate.Lis.Items.Clear();
                }
                if (!System.IO.Directory.Exists(KeppySynthConfiguratorMain.ListsPath))
                {
                    Directory.CreateDirectory(KeppySynthConfiguratorMain.ListsPath);
                    File.Create(WhichList).Dispose();
                    KeppySynthConfiguratorMain.Delegate.Lis.Items.Clear();
                }
                if (!System.IO.File.Exists(WhichList))
                {
                    // Oops, list is missing
                    File.Create(WhichList).Dispose();
                    KeppySynthConfiguratorMain.Delegate.Lis.Items.Clear();
                    Functions.ShowErrorDialog(0, System.Media.SystemSounds.Question, "Information", "The soundfont list was missing, so the configurator automatically created it for you.", false, null);
                }
                try
                {
                    using (StreamReader r = new StreamReader(WhichList))
                    {
                        string line;
                        KeppySynthConfiguratorMain.Delegate.Lis.Items.Clear();
                        KeppySynthConfiguratorMain.Delegate.Lis.Refresh();
                        while ((line = r.ReadLine()) != null)
                        {
                            string result = line.Substring(0, 1);
                            string newvalue;

                            if (result == "@")
                                line = line.Remove(0, 1);

                            FileInfo file = new FileInfo(StripSFZValues(line));
                            ListViewItem SF = new ListViewItem(new[] {
                                line,
                                ReturnSoundFontFormat(Path.GetExtension(StripSFZValues(line))),
                                ReturnSoundFontSize(Path.GetExtension(StripSFZValues(line)), file.Length)
                            });

                            SF.ForeColor = ReturnColor(result);
                            KeppySynthConfiguratorMain.Delegate.Lis.Items.Add(SF);
                        }
                    }
                }
                catch
                {
                    // Oops, list is missing
                    File.Create(WhichList).Dispose();
                    KeppySynthConfiguratorMain.Delegate.Lis.Items.Clear();
                    Functions.ShowErrorDialog(0, System.Media.SystemSounds.Question, "Information", "The soundfont list was missing, so the configurator automatically created it for you.", false, null);
                }
                Program.DebugToConsole(false, String.Format("Switched to soundfont list {0}.", SelectedList), null);
            }
            catch (Exception ex)
            {
                // Oops, something went wrong
                Program.DebugToConsole(true, null, ex);
                Functions.ShowErrorDialog(2, System.Media.SystemSounds.Hand, "Fatal error", "Fatal error during the execution of the program.\n\nPress OK to quit.", true, ex);
                Application.ExitThread();
            }
        }

        public static void AddSoundfontsToSelectedList(String CurrentList, String[] Soundfonts)
        {
            for (int i = 0; i < Soundfonts.Length; i++)
            {
                if (Path.GetExtension(Soundfonts[i]).ToLowerInvariant() == ".sf1" | Path.GetExtension(Soundfonts[i]).ToLowerInvariant() == ".sf2" | Path.GetExtension(Soundfonts[i]).ToLowerInvariant() == ".sfark" | Path.GetExtension(Soundfonts[i]) == ".sfpack".ToLowerInvariant())
                {
                    int test = BassMidi.BASS_MIDI_FontInit(Soundfonts[i]);
                    if (Bass.BASS_ErrorGetCode() != 0)
                    {
                        Functions.ShowErrorDialog(1, System.Media.SystemSounds.Hand, "Error while adding soundfont", String.Format("{0} is not a valid soundfont file!", Path.GetFileName(Soundfonts[i])), false, null);
                    }
                    else
                    {
                        if (KeppySynthConfiguratorMain.Delegate.BankPresetOverride.Checked == true)
                        {
                            using (var form = new BankNPresetSel(Path.GetFileName(Soundfonts[i]), 0, 1))
                            {
                                var result = form.ShowDialog();
                                if (result == DialogResult.OK)
                                {
                                    string sbank = form.BankValueReturn;
                                    string spreset = form.PresetValueReturn;
                                    string dbank = form.DesBankValueReturn;
                                    string dpreset = form.DesPresetValueReturn;
                                    FileInfo file = new FileInfo(Soundfonts[i]);
                                    ListViewItem SF = new ListViewItem(new[] {
                                        "p" + sbank + "," + spreset + "=" + dbank + "," + dpreset + "|" + Soundfonts[i],
                                        ReturnSoundFontFormat(Path.GetExtension(Soundfonts[i])),
                                        ReturnSoundFontSize(Path.GetExtension(Soundfonts[i]), file.Length)
                                    });
                                    KeppySynthConfiguratorMain.Delegate.Lis.Items.Add(SF);
                                }
                            }
                        }
                        else
                        {
                            FileInfo file = new FileInfo(Soundfonts[i]);
                            ListViewItem SF = new ListViewItem(new[] {
                                        Soundfonts[i],
                                        ReturnSoundFontFormat(Path.GetExtension(Soundfonts[i])),
                                        ReturnSoundFontSize(Path.GetExtension(Soundfonts[i]), file.Length)
                                    });
                            KeppySynthConfiguratorMain.Delegate.Lis.Items.Add(SF);
                        }
                        Functions.SaveList(CurrentList);
                        Functions.TriggerReload();
                    }
                    Program.DebugToConsole(false, String.Format("Added soundfont to list: {0}", Soundfonts[i]), null);
                }
                else if (Path.GetExtension(Soundfonts[i]).ToLowerInvariant() == ".sfz")
                {
                    int test = BassMidi.BASS_MIDI_FontInit(Soundfonts[i]);
                    if (Bass.BASS_ErrorGetCode() != 0)
                    {
                        Functions.ShowErrorDialog(1, System.Media.SystemSounds.Hand, "Error while adding soundfont", String.Format("{0} is not a valid soundfont file!", Path.GetFileName(Soundfonts[i])), false, null);
                    }
                    else
                    {
                        using (var form = new BankNPresetSel(Path.GetFileName(Soundfonts[i]), 1, 0))
                        {
                            var result = form.ShowDialog();
                            if (result == DialogResult.OK)
                            {
                                string sbank = form.BankValueReturn;
                                string spreset = form.PresetValueReturn;
                                string dbank = form.DesBankValueReturn;
                                string dpreset = form.DesPresetValueReturn;
                                FileInfo file = new FileInfo(Soundfonts[i]);
                                ListViewItem SF = new ListViewItem(new[] {
                                        "p" + sbank + "," + spreset + "=" + dbank + "," + dpreset + "|" + Soundfonts[i],
                                        ReturnSoundFontFormat(Path.GetExtension(Soundfonts[i])),
                                        ReturnSoundFontSize(Path.GetExtension(Soundfonts[i]), file.Length)
                                    });
                                KeppySynthConfiguratorMain.Delegate.Lis.Items.Add(SF);
                            }
                        }
                        Functions.SaveList(CurrentList);
                        Functions.TriggerReload();
                    }
                    Program.DebugToConsole(false, String.Format("Added soundfont to list: {0}", Soundfonts[i]), null);
                }
                else if (Path.GetExtension(Soundfonts[i]).ToLowerInvariant() == ".dls")
                {
                    Functions.ShowErrorDialog(1, System.Media.SystemSounds.Exclamation, "Error", "BASSMIDI does NOT support the downloadable sounds (DLS) format!", false, null);
                }
                else if (Path.GetExtension(Soundfonts[i]).ToLowerInvariant() == ".exe" | Path.GetExtension(Soundfonts[i]).ToLowerInvariant() == ".dll")
                {
                    Functions.ShowErrorDialog(1, System.Media.SystemSounds.Exclamation, "Error", "Are you really trying to add executables to the soundfonts list?", false, null);
                }
                else
                {
                    Functions.ShowErrorDialog(1, System.Media.SystemSounds.Exclamation, "Error", "Invalid soundfont!\n\nPlease select a valid soundfont and try again!", false, null);
                }
            }
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

        public static void ReinitializeList(Exception ex, String selectedlistpath) // The app encountered an error, so it'll restore the original soundfont list back
        {
            try
            {
                Program.DebugToConsole(true, null, ex);
                Functions.ShowErrorDialog(2, System.Media.SystemSounds.Exclamation, "Error", "Your computer doesn't seem to like soundfont lists.\n\nThe configurator encountered an error while trying to save the following list:\n" + selectedlistpath, true, ex);
                KeppySynthConfiguratorMain.Delegate.Lis.Items.Clear();
                using (StreamReader r = new StreamReader(selectedlistpath))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        string newvaluenosfz = Functions.StripSFZValues(line);
                        FileInfo file = new FileInfo(newvaluenosfz);
                        ListViewItem SF = new ListViewItem(new[] {
                                        newvaluenosfz,
                                        Functions.ReturnSoundFontFormat(newvaluenosfz),
                                        Functions.ReturnSoundFontSize(newvaluenosfz, file.Length)
                                    });
                        KeppySynthConfiguratorMain.Delegate.Lis.Items.Add(SF);
                    }
                }
            }
            catch (Exception ex2)
            {
                Program.DebugToConsole(true, null, ex2);
                Functions.ShowErrorDialog(1, System.Media.SystemSounds.Asterisk, "Fatal error", "Fatal error during the execution of this program!\n\nPress OK to quit.", true, ex2);
                Environment.Exit(-1);
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

        public static void ApplyWinMMPatch(Boolean Is64Bit)
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
                WinMMDialog.Filter = "Executables (*.exe, *.dll)|*.exe;*.dll;";
                WinMMDialog.Title = "Select an application to patch";
                WinMMDialog.Multiselect = false;
                WinMMDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                if (WinMMDialog.ShowDialog() == DialogResult.OK)
                {
                    String DirectoryPath = Path.GetDirectoryName(WinMMDialog.FileName);
                    String MMName = "midimap.dll";
                    String MSACMDrvName = "msacm32.drv";
                    String MSACMName = "msacm32.dll";
                    String MSADPName = "msapd32.drv";
                    String WDMAUDDrvName = "wdmaud.drv";
                    String WDMAUDName = "wdmaud.sys";
                    String WinMMName = "winmm.dll";
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
                        if (Is64Bit)
                        {
                            File.WriteAllBytes(String.Format("{0}\\{1}", DirectoryPath, WDMAUDDrvName), Properties.Resources.wdmaud64drv);
                            File.WriteAllBytes(String.Format("{0}\\{1}", DirectoryPath, WinMMName), Properties.Resources.winmm64);
                        }
                        else
                        {
                            File.WriteAllBytes(String.Format("{0}\\{1}", DirectoryPath, WDMAUDDrvName), Properties.Resources.wdmaud32drv);
                            File.WriteAllBytes(String.Format("{0}\\{1}", DirectoryPath, WinMMName), Properties.Resources.winmm32);
                        }
                        MessageBox.Show(String.Format("\"{0}\" has been succesfully patched!", Path.GetFileName(WinMMDialog.FileName)), "Keppy's Synthesizer - Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch
            {
                Functions.ShowErrorDialog(2, System.Media.SystemSounds.Exclamation, "Error", "Unable to patch the following executable!\nAre you sure you have write permissions to its folder?\n\nPress OK to try again.", false, null);
                goto TryAgain;
            }
        }

        public static void RemoveWinMMPatch()
        {
            OpenFileDialog WinMMDialog = new OpenFileDialog();
            try
            {
                WinMMDialog.Filter = "Executables (*.exe, *.dll)|*.exe;*.dll;";
                WinMMDialog.Title = "Select an application to unpatch";
                WinMMDialog.Multiselect = false;
                WinMMDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                if (WinMMDialog.ShowDialog() == DialogResult.OK)
                {
                    String DirectoryPath = Path.GetDirectoryName(WinMMDialog.FileName);
                    String MMName = "midimap.dll";
                    String MSACMDrvName = "msacm32.drv";
                    String MSACMName = "msacm32.dll";
                    String MSADPName = "msapd32.drv";
                    String WDMAUDDrvName = "wdmaud.drv";
                    String WDMAUDName = "wdmaud.sys";
                    String WinMMName = "winmm.dll";
                    TryAgain:
                    try
                    {
                        File.Delete(String.Format("{0}\\{1}", DirectoryPath, MMName));
                        File.Delete(String.Format("{0}\\{1}", DirectoryPath, MSACMDrvName));
                        File.Delete(String.Format("{0}\\{1}", DirectoryPath, MSACMName));
                        File.Delete(String.Format("{0}\\{1}", DirectoryPath, MSADPName));
                        File.Delete(String.Format("{0}\\{1}", DirectoryPath, WDMAUDDrvName));
                        File.Delete(String.Format("{0}\\{1}", DirectoryPath, WDMAUDName));
                        File.Delete(String.Format("{0}\\{1}", DirectoryPath, WinMMName));
                    }
                    catch
                    {
                        Functions.ShowErrorDialog(2, System.Media.SystemSounds.Exclamation, "Error", "Unable to unpatch the following executable!\nAre you sure you have write permissions to its folder?\n\nPress OK to try again.", false, null);
                        goto TryAgain;
                    }
                    MessageBox.Show(String.Format("\"{0}\" has been succesfully unpatched!", Path.GetFileName(WinMMDialog.FileName)), "Keppy's Synthesizer - Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                Functions.ShowErrorDialog(2, System.Media.SystemSounds.Exclamation, "Error", "Unable to unpatch the following executable!\nAre you sure you have write permissions to its folder?\n\nPress OK to try again.", false, null);
            }
        }
    }
}