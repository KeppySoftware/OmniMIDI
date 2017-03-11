using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.IO;
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
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int connDescription, int ReservedValue);

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
                Functions.ShowErrorDialog(Properties.Resources.erroricon, System.Media.SystemSounds.Hand, "Error", "There was an error while trying to register/unregister the driver.", true, ex);
            }
        }

        public static void ShowErrorDialog(Image icon, System.Media.SystemSound sound, String title, String message, bool IsException, Exception ex)
        {
            SecretDialog frm = new SecretDialog(icon, sound, title, message);
            Program.DebugToConsole(IsException, null, ex);
            frm.ShowDialog();
            frm.Dispose();
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
                    Forms.DLEngine frm = new Forms.DLEngine(null, "Downloading LoudMax 32-bit... {0}%", loudmax32, 1, false);
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
                        Forms.DLEngine frm = new Forms.DLEngine(null, "Downloading LoudMax 64-bit... {0}%", loudmax64, 1, false);
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
                Functions.ShowErrorDialog(Properties.Resources.wi, System.Media.SystemSounds.Exclamation, "LoudMax Installation", "Crap, an error!\nAre you sure you have a working Internet connection?", true, ex);
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
                Functions.ShowErrorDialog(Properties.Resources.wi, System.Media.SystemSounds.Exclamation, "LoudMax Installation", "Crap, an error!\nAre you sure you closed all the apps using the driver? They might have locked LoudMax.", true, ex);
            }
        }

        public static bool IsInternetAvailable()
        {
            int Desc;
            return InternetGetConnectedState(out Desc, 0);
        }

        public static void TriggerUpdateWindow(Version x, Version y, String newestversion, bool forced, bool startup)
        {
            if (forced && startup)
            {
                Forms.DLEngine frm = new Forms.DLEngine(newestversion, String.Format("Downloading update {0}, please wait... {1}%", newestversion, @"{0}"), null, 0, true);
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.ShowDialog();
            }
            else
            {
                UpdateYesNo upd = new UpdateYesNo(x, y, true);
                if (startup)
                    upd.StartPosition = FormStartPosition.CenterScreen;
                else
                    upd.StartPosition = FormStartPosition.CenterParent;
                DialogResult dialogResult = upd.ShowDialog();
                upd.Dispose();
                if (dialogResult == DialogResult.Yes)
                {
                    Forms.DLEngine frm = new Forms.DLEngine(newestversion, String.Format("Downloading update {0}, please wait... {1}%", newestversion, @"{0}"), null, 0, false);
                    frm.StartPosition = FormStartPosition.CenterScreen;
                    frm.ShowDialog();
                }
            }
        }

        public static void NoUpdates(bool startup, bool internetok)
        {
            if (!startup)
            {
                UpdateYesNo upd = new UpdateYesNo(null, null, internetok);
                upd.StartPosition = FormStartPosition.CenterParent;
                upd.ShowDialog();
                upd.Dispose();
            }
        }

        public static void CheckChangelog()
        {
            bool internetok = IsInternetAvailable();
            if (internetok == false)
            {
                MessageBox.Show("There's no Internet connection.\n\nYou can't see the changelog without one.", "Keppy's Synthesizer - No Internet connection available", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    WebClient client = new WebClient();
                    Stream stream = client.OpenRead("https://raw.githubusercontent.com/KaleidonKep99/Keppy-s-Driver/master/output/keppydriverupdate.txt");
                    StreamReader reader = new StreamReader(stream);
                    String newestversion = reader.ReadToEnd();
                    Process.Start(String.Format("https://github.com/KaleidonKep99/Keppy-s-Synthesizer/releases/tag/{0}", newestversion));
                }
                catch (Exception ex)
                {
                    Functions.ShowErrorDialog(Properties.Resources.wi, System.Media.SystemSounds.Exclamation, "Unknown error", "An error has occurred while trying to show you the latest changelog.\nPlease try again later.\n\nPress OK to continue.", true, ex);
                }
            }
        }

        public static void CheckForUpdates(bool forced, bool startup)
        {
            bool internetok = IsInternetAvailable();
            if (internetok == false)
            {
                NoUpdates(startup, false);
            }
            else
            {
                try
                {
                    WebClient client = new WebClient();
                    Stream stream = client.OpenRead("https://raw.githubusercontent.com/KaleidonKep99/Keppy-s-Driver/master/output/keppydriverupdate.txt");
                    StreamReader reader = new StreamReader(stream);
                    String newestversion = reader.ReadToEnd();
                    FileVersionInfo Driver = FileVersionInfo.GetVersionInfo(Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\keppysynth\\keppysynth.dll");
                    Version x = null;
                    Version.TryParse(newestversion.ToString(), out x);
                    Version y = null;
                    Version.TryParse(Driver.FileVersion.ToString(), out y);
                    if (forced)
                    {
                        Program.DebugToConsole(false, String.Format("The user forced a reinstall/downgrade of the driver. ({0})", newestversion), null);
                        TriggerUpdateWindow(x, y, newestversion, forced, startup);
                    }
                    else
                    {
                        if (x > y)
                        {
                            Program.DebugToConsole(false, String.Format("New version found. Requesting user to download it. ({0})", newestversion), null);
                            TriggerUpdateWindow(x, y, newestversion, forced, startup);
                        }
                        else
                        {
                            Program.DebugToConsole(false, String.Format("No updates have been found. ({0})", newestversion), null);
                            NoUpdates(startup, internetok);
                        }
                    }
                }
                catch
                {
                    Program.DebugToConsole(false, "An error has occurred while checking for updates.", null);
                    NoUpdates(startup, internetok);
                }
            }
        }

        public static void SaveList(String SelectedList) // Saves the selected list to the hard drive
        {
            using (StreamWriter sw = new StreamWriter(SelectedList))
            {
                foreach (var item in KeppySynthConfiguratorMain.Delegate.Lis.Items)
                {
                    sw.WriteLine(item.ToString());
                    Program.DebugToConsole(false, String.Format("Adding soundfont to stream writer: {0}", item.ToString()), null);
                }
            }
            Program.DebugToConsole(false, String.Format("Soundfont list saved: {0}", SelectedList), null);
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

        public static void SetDefaultDevice(int dev)
        {
            KeppySynthConfiguratorMain.SynthSettings.SetValue("defaultdev", dev, RegistryValueKind.DWord);
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
                KeppySynthConfiguratorMain.Delegate.bufsize.Minimum = 1;
                KeppySynthConfiguratorMain.Delegate.PolyphonyLimit.Value = Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("polyphony", 512));
                KeppySynthConfiguratorMain.Delegate.MaxCPU.Value = Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("cpu", 75));
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("defaultmidiout", 0)) == 0)
                {
                    KeppySynthConfiguratorMain.Delegate.SetSynthDefault.Checked = false;
                }
                else
                {
                    KeppySynthConfiguratorMain.Delegate.SetSynthDefault.Checked = true;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("allhotkeys", 0)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.hotkeys.Checked = true;
                }
                else
                {
                    KeppySynthConfiguratorMain.Delegate.hotkeys.Checked = false;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("autopanic", 1)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.autopanicmode.Checked = true;
                }
                else
                {
                    KeppySynthConfiguratorMain.Delegate.autopanicmode.Checked = false;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("shortname", 0)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.MIDINameNoSpace.Checked = true;
                }
                else
                {
                    KeppySynthConfiguratorMain.Delegate.MIDINameNoSpace.Checked = false;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("fadeoutdisable", 0)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.FadeoutDisable.Checked = true;
                }
                else
                {
                    KeppySynthConfiguratorMain.Delegate.FadeoutDisable.Checked = false;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("monorendering", 0)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.MonophonicFunc.Checked = true;
                }
                else
                {
                    KeppySynthConfiguratorMain.Delegate.MonophonicFunc.Checked = false;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("sysexignore", 0)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.SysExIgnore.Checked = true;
                }
                else
                {
                    KeppySynthConfiguratorMain.Delegate.SysExIgnore.Checked = false;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("allnotesignore", 0)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.AllNotesIgnore.Checked = true;
                    KeppySynthConfiguratorMain.Delegate.SysExIgnore.Checked = true;
                }
                else
                {
                    KeppySynthConfiguratorMain.Delegate.AllNotesIgnore.Checked = false;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("rco", 1)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.ReduceCPUOver.Checked = true;
                }
                else
                {
                    KeppySynthConfiguratorMain.Delegate.ReduceCPUOver.Checked = false;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("vms2emu", 0)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.slowdownnoskip.Checked = true;
                }
                else
                {
                    KeppySynthConfiguratorMain.Delegate.slowdownnoskip.Checked = false;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("debugmode", 0)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.DebugModePls.Checked = true;
                }
                else
                {
                    KeppySynthConfiguratorMain.Delegate.DebugModePls.Checked = false;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("ignorenotes1", 0)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.IgnoreNotes1.Checked = true;
                    KeppySynthConfiguratorMain.Delegate.IgnoreNotesInterval.Enabled = true;
                }
                else
                {
                    KeppySynthConfiguratorMain.Delegate.IgnoreNotes1.Checked = false;
                    KeppySynthConfiguratorMain.Delegate.IgnoreNotesInterval.Enabled = false;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("oldbuffersystem", 0)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.useoldbuffersystem.Checked = true;
                }
                else
                {
                    KeppySynthConfiguratorMain.Delegate.useoldbuffersystem.Checked = false;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("autoupdatecheck", 1)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.autoupdate.Checked = true;
                }
                else
                {
                    KeppySynthConfiguratorMain.Delegate.autoupdate.Checked = false;
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
                else
                {
                    KeppySynthConfiguratorMain.Delegate.DebugModePls.Checked = false;
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
                else
                {
                    KeppySynthConfiguratorMain.Delegate.enableextra8sf.Checked = false;
                }
                KeppySynthConfiguratorMain.Delegate.Frequency.Text = KeppySynthConfiguratorMain.SynthSettings.GetValue("frequency", 44100).ToString();
                KeppySynthConfiguratorMain.Delegate.SPFRate.Value = Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("sndbfvalue", 16));

                // Then the filthy checkboxes
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("preload", 1)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.Preload.Checked = true;
                }
                else
                {
                    KeppySynthConfiguratorMain.Delegate.Preload.Checked = false;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("nofx", 0)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.EnableSFX.Checked = false;
                }
                else
                {
                    KeppySynthConfiguratorMain.Delegate.EnableSFX.Checked = true;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("noteoff", 0)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.NoteOffCheck.Checked = true;
                }
                else
                {
                    KeppySynthConfiguratorMain.Delegate.NoteOffCheck.Checked = false;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("sysresetignore", 0)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.SysResetIgnore.Checked = true;
                }
                else
                {
                    KeppySynthConfiguratorMain.Delegate.SysResetIgnore.Checked = false;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("encmode", 0)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.OutputWAV.Checked = true;
                }
                else
                {
                    KeppySynthConfiguratorMain.Delegate.OutputWAV.Checked = false;
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("xaudiodisabled", 0)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.XAudioDisable.Checked = true;
                    KeppySynthConfiguratorMain.Delegate.ManualAddBuffer.Visible = true;
                    KeppySynthConfiguratorMain.Delegate.changeTheMaximumSamplesPerFrameToolStripMenuItem.Enabled = false;
                    KeppySynthConfiguratorMain.Delegate.changeDirectoryOfTheOutputToWAVModeToolStripMenuItem.Enabled = false;
                    KeppySynthConfiguratorMain.Delegate.VolumeBoost.Checked = false;
                    KeppySynthConfiguratorMain.Delegate.VolumeBoost.Enabled = false;
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
                else
                {
                    KeppySynthConfiguratorMain.Delegate.XAudioDisable.Checked = false;
                    KeppySynthConfiguratorMain.Delegate.ManualAddBuffer.Visible = false;
                    KeppySynthConfiguratorMain.Delegate.bufsize.Enabled = true;
                    KeppySynthConfiguratorMain.Delegate.ChangeDefaultOutput.Enabled = false;
                    if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("volumeboost", 0)) == 1)
                    {
                        KeppySynthConfiguratorMain.Delegate.VolumeBoost.Checked = true;
                        KeppySynthConfiguratorMain.Delegate.VolTrackBar.Maximum = 20000;
                    }
                    else
                    {
                        KeppySynthConfiguratorMain.Delegate.VolumeBoost.Checked = false;
                        KeppySynthConfiguratorMain.Delegate.VolTrackBar.Maximum = 10000;
                    }
                }
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("sinc", 0)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.SincInter.Checked = true;
                }
                else
                {
                    KeppySynthConfiguratorMain.Delegate.SincInter.Checked = false;
                }

                // LEL
                KeppySynthConfiguratorMain.Delegate.bufsize.Value = Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("buflen"));

                // And finally, the volume!
                int VolumeValue = Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("volume", 10000));
                decimal VolVal = (decimal)VolumeValue / 100;
                if (KeppySynthConfiguratorMain.Delegate.VolumeBoost.Checked == true)
                {
                    KeppySynthConfiguratorMain.Delegate.VolTrackBar.Value = VolumeValue;
                }
                else
                {
                    if (VolumeValue > 10000)
                    {
                        KeppySynthConfiguratorMain.Delegate.VolTrackBar.Value = 10000;
                    }
                    else
                    {
                        KeppySynthConfiguratorMain.Delegate.VolTrackBar.Value = VolumeValue;
                    }
                }
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
                if (KeppySynthConfiguratorMain.Delegate.XAudioDisable.Checked == true)
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
                else
                {
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("sinc", "0", RegistryValueKind.DWord);
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("xaudiodisabled", "0", RegistryValueKind.DWord);
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
                KeppySynthConfiguratorMain.SynthSettings.SetValue("polyphony", "512", RegistryValueKind.DWord);
                KeppySynthConfiguratorMain.SynthSettings.SetValue("cpu", "65", RegistryValueKind.DWord);
                KeppySynthConfiguratorMain.Delegate.Frequency.Text = "48000";
                KeppySynthConfiguratorMain.SynthSettings.SetValue("frequency", KeppySynthConfiguratorMain.Delegate.Frequency.Text, RegistryValueKind.DWord);

                // Advanced SynthSettings
                KeppySynthConfiguratorMain.SynthSettings.SetValue("buflen", "30", RegistryValueKind.DWord);
                KeppySynthConfiguratorMain.SynthSettings.SetValue("tracks", "16", RegistryValueKind.DWord);

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
                Functions.ShowErrorDialog(Properties.Resources.wi, System.Media.SystemSounds.Hand, "Fatal error", "Missing registry settings!\nPlease reinstall Keppy's Synthesizer to solve the issue.\n\nPress OK to quit.", true, ex);
                System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(ignored =>
                {
                    KeppySynthConfiguratorMain.ActiveForm.Hide();
                    throw new Exception();
                }));
            }
        }

        public static void ExportSettings(String filename)
        {
            try
            {
                RegistryKey SynthSettings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Settings", true);
                RegistryKey SynthPaths = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Paths", true);

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Windows Registry Editor Version 5.00");

                sb.AppendLine("");
                sb.AppendLine("; Keppy's Synthesizer Settings File");
                sb.AppendLine("");

                sb.AppendLine(@"[HKEY_CURRENT_USER\SOFTWARE\Keppy's Synthesizer\Settings]");
                foreach (var keyname in KeppySynthConfiguratorMain.SynthSettings.GetValueNames())
                {
                    sb.AppendLine(String.Format("\"{0}\"={1}:{2}", keyname, SynthSettings.GetValueKind(keyname).ToString().ToLower(), Convert.ToInt32(SynthSettings.GetValue(keyname)).ToString("X")));
                }

                sb.AppendLine("");

                sb.AppendLine(@"[HKEY_CURRENT_USER\SOFTWARE\Keppy's Synthesizer\Paths]");
                foreach (var keyname in KeppySynthConfiguratorMain.SynthPaths.GetValueNames())
                {
                    sb.AppendLine(String.Format("\"{0}\"=\"{1}\"", keyname, SynthPaths.GetValue(keyname).ToString()));
                }

                sb.AppendLine(Environment.NewLine);

                File.WriteAllText(filename, sb.ToString());

                SynthSettings.Close();
                SynthPaths.Close();

                Program.DebugToConsole(false, String.Format("Settings exported to: {0}", filename), null);
            }
            catch (Exception ex)
            {
                // Something bad happened hehe
                Program.DebugToConsole(true, null, ex);
                Functions.ShowErrorDialog(Properties.Resources.wi, System.Media.SystemSounds.Hand, "Fatal error", "Fatal error during the execution of the program.\n\nPress OK to quit", true, ex);
                Application.Exit();
            }
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
                    File.Create(WhichList).Dispose();
                    KeppySynthConfiguratorMain.Delegate.Lis.Items.Clear();
                }
                try
                {
                    using (StreamReader r = new StreamReader(WhichList))
                    {
                        string line;
                        KeppySynthConfiguratorMain.Delegate.Lis.Items.Clear();
                        while ((line = r.ReadLine()) != null)
                        {
                            KeppySynthConfiguratorMain.Delegate.Lis.Items.Add(line);
                        }
                    }
                }
                catch
                {
                    // Oops, list is missing
                    File.Create(WhichList).Dispose();
                    Functions.ShowErrorDialog(Properties.Resources.infoicon, System.Media.SystemSounds.Question, "Information", "The soundfont list was missing, so the configurator automatically created it for you.", false, null); 
                }
                Program.DebugToConsole(false, String.Format("Switched to soundfont list {0}.", SelectedList), null);
            }
            catch (Exception ex)
            {
                // Oops, something went wrong
                Program.DebugToConsole(true, null, ex);
                Functions.ShowErrorDialog(Properties.Resources.wi, System.Media.SystemSounds.Hand, "Fatal error", "Fatal error during the execution of the program.\n\nPress OK to quit.", true, ex);
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
                        Functions.ShowErrorDialog(Properties.Resources.wi, System.Media.SystemSounds.Hand, "Error while adding soundfont", String.Format("{0} is not a valid soundfont file!", Path.GetFileName(Soundfonts[i])), false, null);
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
                                    KeppySynthConfiguratorMain.Delegate.Lis.Items.Add("p" + sbank + "," + spreset + "=" + dbank + "," + dpreset + "|" + Soundfonts[i]);
                                }
                            }
                        }
                        else
                        {
                            KeppySynthConfiguratorMain.Delegate.Lis.Items.Add(Soundfonts[i]);
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
                        Functions.ShowErrorDialog(Properties.Resources.wi, System.Media.SystemSounds.Hand, "Error while adding soundfont", String.Format("{0} is not a valid soundfont file!", Path.GetFileName(Soundfonts[i])), false, null);
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
                                KeppySynthConfiguratorMain.Delegate.Lis.Items.Add("p" + sbank + "," + spreset + "=" + dbank + "," + dpreset + "|" + Soundfonts[i]);
                            }
                        }
                        Functions.SaveList(CurrentList);
                        Functions.TriggerReload();
                    }
                    Program.DebugToConsole(false, String.Format("Added soundfont to list: {0}", Soundfonts[i]), null);
                }
                else if (Path.GetExtension(Soundfonts[i]).ToLowerInvariant() == ".dls")
                {
                    Functions.ShowErrorDialog(Properties.Resources.wi, System.Media.SystemSounds.Exclamation, "Error", "BASSMIDI does NOT support the downloadable sounds (DLS) format!", false, null);
                }
                else if (Path.GetExtension(Soundfonts[i]).ToLowerInvariant() == ".exe" | Path.GetExtension(Soundfonts[i]).ToLowerInvariant() == ".dll")
                {
                    Functions.ShowErrorDialog(Properties.Resources.wi, System.Media.SystemSounds.Exclamation, "Error", "Are you really trying to add executables to the soundfonts list?", false, null);
                }
                else
                {
                    Functions.ShowErrorDialog(Properties.Resources.wi, System.Media.SystemSounds.Exclamation, "Error", "Invalid soundfont!\n\nPlease select a valid soundfont and try again!", false, null);
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
                Functions.ShowErrorDialog(Properties.Resources.wi, System.Media.SystemSounds.Exclamation, "Error", "Your computer doesn't seem to like soundfont lists.\n\nThe configurator encountered an error while trying to save the following list:\n" + selectedlistpath, true, ex);
                KeppySynthConfiguratorMain.Delegate.Lis.Items.Clear();
                using (StreamReader r = new StreamReader(selectedlistpath))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        KeppySynthConfiguratorMain.Delegate.Lis.Items.Add(line);
                    }
                }
            }
            catch (Exception ex2)
            {
                Program.DebugToConsole(true, null, ex2);
                Functions.ShowErrorDialog(null, System.Media.SystemSounds.Hand, "Fatal error", "Fatal error during the execution of this program!\n\nPress OK to quit.", true, ex2);
                Environment.Exit(-1);
            }
        }
    }
}