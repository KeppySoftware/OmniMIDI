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

        public static void DriverRegistry(int integer)
        {
            try
            {
                if (integer == 0)
                {
                    var process = System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\keppysynth\\KSDriverRegister.exe", "/register");
                    process.WaitForExit();
                    MessageBox.Show("The driver has been registered on the Windows registry.", "Keppy's Synthesizer - Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    var process = System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\keppysynth\\KSDriverRegister.exe", "/unregister");
                    process.WaitForExit();
                    MessageBox.Show("The driver has been unregistered from the Windows registry.", "Keppy's Synthesizer - Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("There was an error while trying to register/unregister the driver.\n\n{0}", ex.ToString()), "Keppy's Synthesizer - Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
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
                    Forms.KeppySynthDLEngine frm = new Forms.KeppySynthDLEngine(null, "Downloading LoudMax 32-bit... {0}%", loudmax32, 1);
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
                        Forms.KeppySynthDLEngine frm = new Forms.KeppySynthDLEngine(null, "Downloading LoudMax 64-bit... {0}%", loudmax64, 1);
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
                MessageBox.Show("Crap, an error!\nAre you sure you have a working Internet connection?\n\nError:\n" + ex.ToString(), "Oh no! Keppy's Synthesizer encountered an error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show("Crap, an error!\nAre you sure you closed all the apps using the driver? They might have locked LoudMax.\n\nError:\n" + ex.ToString(), "Oh no! Keppy's Synthesizer encountered an error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public static bool IsInternetAvailable()
        {
            int Desc;
            return InternetGetConnectedState(out Desc, 0);
        }

        public static void CheckForUpdates(bool forced)
        {
            if (IsInternetAvailable() == false)
            {
                MessageBox.Show("The configurator can not connect to the GitHub servers.\n\nCheck your network connection, or contact your system administrator or network service provider.\n\nPress OK to continue and open the configurator's window.", "Keppy's Synthesizer - Connection error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                        Forms.KeppySynthDLEngine frm = new Forms.KeppySynthDLEngine(newestversion, String.Format("Downloading update {0}, please wait... {1}%", newestversion, @"{0}"), null, 0);
                        frm.StartPosition = FormStartPosition.CenterScreen;
                        frm.ShowDialog();
                    }
                    else
                    {
                        if (x > y)
                        {
                            DialogResult dialogResult = MessageBox.Show("A new update for Keppy's Synthesizer has been found.\n\nVersion installed: " + Driver.FileVersion.ToString() + "\nVersion available online: " + newestversion.ToString() + "\n\nWould you like to update now?\nIf you choose \"Yes\", the configurator will be automatically closed.\n\n(You can disable the automatic update check through the advanced settings.)", "New version of Keppy's Synthesizer found", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dialogResult == DialogResult.Yes)
                            {
                                Forms.KeppySynthDLEngine frm = new Forms.KeppySynthDLEngine(newestversion, String.Format("Downloading update {0}, please wait... {1}%", newestversion, @"{0}"), null, 0);
                                frm.StartPosition = FormStartPosition.CenterScreen;
                                frm.ShowDialog();
                            }
                        }
                        else
                        {
                            MessageBox.Show("No updates have been found.\n\nPlease try again later.", "Keppy's Synthesizer - No updates found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Unknown error.\n\nPress OK to continue and open the configurator's window.", "Keppy's Synthesizer - Connection error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                }
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

        public static void LoadSettings() // Loads the settings from the registry
        {
            // ======= Load settings from the registry
            try
            {
                // First, the most important settings
                KeppySynthConfiguratorMain.Delegate.PolyphonyLimit.Value = Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("polyphony", 512));
                KeppySynthConfiguratorMain.Delegate.MaxCPU.Value = Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("cpu", 75));
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("defaultmidiout", 0)) == 0)
                {
                    KeppySynthConfiguratorMain.Delegate.DefaultOut810enabledToolStripMenuItem.Checked = false;
                    KeppySynthConfiguratorMain.Delegate.DefaultOut810enabledToolStripMenuItem.Enabled = true;
                    KeppySynthConfiguratorMain.Delegate.DefaultOut810disabledToolStripMenuItem.Checked = true;
                    KeppySynthConfiguratorMain.Delegate.DefaultOut810disabledToolStripMenuItem.Enabled = false;
                }
                else
                {
                    KeppySynthConfiguratorMain.Delegate.DefaultOut810enabledToolStripMenuItem.Checked = true;
                    KeppySynthConfiguratorMain.Delegate.DefaultOut810enabledToolStripMenuItem.Enabled = false;
                    KeppySynthConfiguratorMain.Delegate.DefaultOut810disabledToolStripMenuItem.Checked = false;
                    KeppySynthConfiguratorMain.Delegate.DefaultOut810disabledToolStripMenuItem.Enabled = true;
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
                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("32bit", 1)) == 1)
                {
                    KeppySynthConfiguratorMain.Delegate.floatingpointaudio.Checked = true;
                }
                else
                {
                    KeppySynthConfiguratorMain.Delegate.floatingpointaudio.Checked = false;
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
                KeppySynthConfiguratorMain.Delegate.TracksLimit.Value = Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("tracks", 16));

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
                    KeppySynthConfiguratorMain.Delegate.SPFSecondaryBut.Visible = false;
                    KeppySynthConfiguratorMain.Delegate.changeTheMaximumSamplesPerFrameToolStripMenuItem.Enabled = false;
                    KeppySynthConfiguratorMain.Delegate.changeDirectoryOfTheOutputToWAVModeToolStripMenuItem.Enabled = false;
                    KeppySynthConfiguratorMain.Delegate.VolumeBoost.Checked = false;
                    KeppySynthConfiguratorMain.Delegate.VolumeBoost.Enabled = false;
                    KeppySynthConfiguratorMain.Delegate.ChangeDefaultOutput.Enabled = true;
                    if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("sinc", 0)) == 1)
                    {
                        KeppySynthConfiguratorMain.Delegate.SincInter.Checked = true;
                    }
                    else
                    {
                        KeppySynthConfiguratorMain.Delegate.SincInter.Checked = false;
                    }
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
                    KeppySynthConfiguratorMain.Delegate.SPFSecondaryBut.Visible = true;
                    KeppySynthConfiguratorMain.Delegate.bufsize.Enabled = true;
                    KeppySynthConfiguratorMain.Delegate.SincInter.Checked = true;
                    KeppySynthConfiguratorMain.Delegate.SincInter.Enabled = false;
                    KeppySynthConfiguratorMain.Delegate.ChangeDefaultOutput.Enabled = false;
                    KeppySynthConfiguratorMain.Delegate.SincInter.Text = "Enable sinc interpolation. (Already applied by XAudio itself, it doesn't cause CPU overhead.)";
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

                // LEL
                KeppySynthConfiguratorMain.Delegate.bufsize.Value = Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("buflen"));

                // And finally, the volume!
                int VolumeValue = Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("volume", 10000));
                double x = VolumeValue / 100;
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
                KeppySynthConfiguratorMain.Delegate.VolSimView.Text = x.ToString("000\\%");
                KeppySynthConfiguratorMain.Delegate.VolIntView.Text = "Value: " + KeppySynthConfiguratorMain.Delegate.VolTrackBar.Value.ToString("00000");
            }
            catch
            {
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
                if (string.IsNullOrWhiteSpace(KeppySynthConfiguratorMain.Delegate.Frequency.Text))
                {
                    KeppySynthConfiguratorMain.Delegate.Frequency.Text = KeppySynthConfiguratorMain.SynthSettings.GetValue("frequency", "44100").ToString();
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("frequency", KeppySynthConfiguratorMain.Delegate.Frequency.Text, RegistryValueKind.DWord);
                }
                else
                {
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("frequency", KeppySynthConfiguratorMain.Delegate.Frequency.Text, RegistryValueKind.DWord);
                }

                // Advanced SynthSettings
                KeppySynthConfiguratorMain.SynthSettings.SetValue("buflen", KeppySynthConfiguratorMain.Delegate.bufsize.Value.ToString(), RegistryValueKind.DWord);
                KeppySynthConfiguratorMain.SynthSettings.SetValue("tracks", KeppySynthConfiguratorMain.Delegate.TracksLimit.Value.ToString(), RegistryValueKind.DWord);

                // Let's not forget about the volume!
                int VolumeValue = 0;
                double x = KeppySynthConfiguratorMain.Delegate.VolTrackBar.Value / 100;
                VolumeValue = Convert.ToInt32(x);
                KeppySynthConfiguratorMain.Delegate.VolSimView.Text = VolumeValue.ToString("000\\%");
                KeppySynthConfiguratorMain.Delegate.VolIntView.Text = "Value: " + KeppySynthConfiguratorMain.Delegate.VolTrackBar.Value.ToString("00000");
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
            }
            catch
            {
                // Something bad happened hehe
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
                MessageBox.Show("Missing registry settings!\nPlease reinstall Keppy's Synthesizer to solve the issue.\n\nPress OK to quit.\n\n.NET error:\n" + ex.ToString(), "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
            }
            catch
            {
                // Something bad happened hehe
                MessageBox.Show("Fatal error during the execution of this program!\n\nPress OK to quit.", "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
                    MessageBox.Show("The soundfont list was missing, so the configurator automatically created it for you.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // Oops, something went wrong
                MessageBox.Show("Fatal error during the execution of the program.\n\nPress OK to quit.\n\n.NET error:\n" + ex.Message.ToString(), "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
                        MessageBox.Show(String.Format("{0} is not a valid soundfont file!", Path.GetFileName(Soundfonts[i])), "Error while adding soundfont", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                }
                else if (Path.GetExtension(Soundfonts[i]).ToLowerInvariant() == ".sfz")
                {
                    int test = BassMidi.BASS_MIDI_FontInit(Soundfonts[i]);
                    if (Bass.BASS_ErrorGetCode() != 0)
                    {
                        MessageBox.Show(String.Format("{0} is not a valid soundfont file!", Path.GetFileName(Soundfonts[i])), "Error while adding soundfont", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                }
                else if (Path.GetExtension(Soundfonts[i]).ToLowerInvariant() == ".dls")
                {
                    MessageBox.Show("BASSMIDI does NOT support the downloadable sounds (DLS) format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (Path.GetExtension(Soundfonts[i]).ToLowerInvariant() == ".exe" | Path.GetExtension(Soundfonts[i]).ToLowerInvariant() == ".dll")
                {
                    MessageBox.Show("Are you really trying to add executables to the soundfonts list?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Invalid soundfont!\n\nPlease select a valid soundfont and try again!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Your computer doesn't seem to like soundfont lists.\n\nThe configurator encountered an error while trying to save the following list:\n" + selectedlistpath + "\n\n.NET error:\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            catch
            {
                MessageBox.Show("Fatal error during the execution of this program!\n\nPress OK to quit.", "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                Environment.Exit(-1);
            }
        }
    }
}