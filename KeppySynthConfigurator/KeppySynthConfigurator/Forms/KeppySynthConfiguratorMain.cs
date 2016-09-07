using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;

namespace KeppySynthConfigurator
{
    public partial class KeppySynthConfiguratorMain : Form
    {
        public string LastBrowserPath { get; set; }
        public string LastImportExportPath { get; set; }

        public string List1PathOld { get; set; }
        public string List2PathOld { get; set; }
        public string List3PathOld { get; set; }
        public string List4PathOld { get; set; }
        public string List5PathOld { get; set; }
        public string List6PathOld { get; set; }
        public string List7PathOld { get; set; }
        public string List8PathOld { get; set; }

        public static string soundfontnewlocation = System.Environment.GetEnvironmentVariable("USERPROFILE").ToString();

        public string AbsolutePath = soundfontnewlocation + "\\Keppy's Synthesizer";
        public string ListsPath = soundfontnewlocation + "\\Keppy's Synthesizer\\lists";
        public string List1Path = soundfontnewlocation + "\\Keppy's Synthesizer\\lists\\keppymidi.sflist";
        public string List2Path = soundfontnewlocation + "\\Keppy's Synthesizer\\lists\\keppymidib.sflist";
        public string List3Path = soundfontnewlocation + "\\Keppy's Synthesizer\\lists\\keppymidic.sflist";
        public string List4Path = soundfontnewlocation + "\\Keppy's Synthesizer\\lists\\keppymidid.sflist";
        public string List5Path = soundfontnewlocation + "\\Keppy's Synthesizer\\lists\\keppymidie.sflist";
        public string List6Path = soundfontnewlocation + "\\Keppy's Synthesizer\\lists\\keppymidif.sflist";
        public string List7Path = soundfontnewlocation + "\\Keppy's Synthesizer\\lists\\keppymidig.sflist";
        public string List8Path = soundfontnewlocation + "\\Keppy's Synthesizer\\lists\\keppymidih.sflist";
        public string List9Path = soundfontnewlocation + "\\Keppy's Synthesizer\\lists\\keppymidii.sflist";
        public string List10Path = soundfontnewlocation + "\\Keppy's Synthesizer\\lists\\keppymidij.sflist";
        public string List11Path = soundfontnewlocation + "\\Keppy's Synthesizer\\lists\\keppymidik.sflist";
        public string List12Path = soundfontnewlocation + "\\Keppy's Synthesizer\\lists\\keppymidil.sflist";
        public string List13Path = soundfontnewlocation + "\\Keppy's Synthesizer\\lists\\keppymidim.sflist";
        public string List14Path = soundfontnewlocation + "\\Keppy's Synthesizer\\lists\\keppymidin.sflist";
        public string List15Path = soundfontnewlocation + "\\Keppy's Synthesizer\\lists\\keppymidio.sflist";
        public string List16Path = soundfontnewlocation + "\\Keppy's Synthesizer\\lists\\keppymidip.sflist";

        public int openadvanced { get; set; }
        public int whichone { get; set; }
        public string CurrentList { get; set; }

        public RegistryKey SynthSettings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Settings", true);
        public RegistryKey Watchdog = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Watchdog", true);
        public RegistryKey SynthPaths = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Paths", true);

        public KeppySynthConfiguratorMain(String[] args)
        {
            try
            {
                foreach (String s in args)
                {
                    switch (s.Substring(0, 4).ToUpper())
                    {
                        case "/ASP":
                            UserProfileMigration();
                            Environment.Exit(0);
                            return;
                        case "/AST":
                            openadvanced = 1;
                            break;
                        case "/NUL":
                            break;
                        default:
                            // do other stuff...
                            break;
                    }
                }
            }
            catch
            {

            }
            InitializeComponent();
            VolTrackBar.BackColor = Color.Empty;
            this.FormClosing += new FormClosingEventHandler(CloseConfigurator);
        }

        private void KeppySynthConfiguratorMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SynthSettings.Close();
            Watchdog.Close();
        }

        static bool IsWindows8OrNewer()
        {
            var reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
            string productName = (string)reg.GetValue("ProductName");
            return productName.StartsWith("Windows 8") | productName.StartsWith("Windows 10");
        }

        static bool IsWindowsXP()
        {
            OperatingSystem OS = Environment.OSVersion;
            return (OS.Version.Major == 5) || ((OS.Version.Major == 5) && (OS.Version.Minor == 1));
        }

        // Just stuff to reduce code's length
        private void SFZCompliant()
        {
            MessageBox.Show("This driver is \"SFZ format 2.0\" compliant.", "SFZ format support", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SaveList(String SelectedList)
        {
            using (StreamWriter sw = new StreamWriter(SelectedList))
            {
                foreach (var item in Lis.Items)
                {
                    sw.WriteLine(item.ToString());
                }
            }
        }

        private void ReinitializeList(Exception ex, String selectedlistpath)
        {
            try
            {
                MessageBox.Show("Your computer doesn't seem to like soundfont lists.\n\nThe configurator encountered an error while trying to save the following list:\n" + selectedlistpath + "\n\n.NET error:\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Lis.Items.Clear();
                using (StreamReader r = new StreamReader(selectedlistpath))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        Lis.Items.Add(line);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Fatal error during the execution of this program!\n\nPress OK to quit.", "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                Environment.Exit(-1);
            }
        }

        private void AddSoundfontDragNDrop(String SelectedList, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            int i;
            for (i = 0; i < s.Length; i++)
            {
                if (Path.GetExtension(s[i]) == ".sf2" | Path.GetExtension(s[i]) == ".SF2" | Path.GetExtension(s[i]) == ".sf3" | Path.GetExtension(s[i]) == ".SF3" | Path.GetExtension(s[i]) == ".sfpack" | Path.GetExtension(s[i]) == ".SFPACK")
                {
                    if (BankPresetOverride.Checked == true)
                    {
                        using (var form = new BankNPresetSel(Path.GetFileName(s[i]), 0, 1))
                        {
                            var result = form.ShowDialog();
                            if (result == DialogResult.OK)
                            {
                                string sbank = form.BankValueReturn;
                                string spreset = form.PresetValueReturn;
                                string dbank = form.DesBankValueReturn;
                                string dpreset = form.DesPresetValueReturn;
                                Lis.Items.Add("p" + sbank + "," + spreset + "=" + dbank + "," + dpreset + "|" + s[i]);
                            }
                        }
                    }
                    else
                    {
                        Lis.Items.Add(s[i]);
                    }
                    SaveList(CurrentList);
                    TriggerReload();
                }
                else if (Path.GetExtension(s[i]) == ".sfz" | Path.GetExtension(s[i]) == ".SFZ")
                {
                    using (var form = new BankNPresetSel(Path.GetFileName(s[i]), 1, 0))
                    {
                        var result = form.ShowDialog();
                        if (result == DialogResult.OK)
                        {
                            string sbank = form.BankValueReturn;
                            string spreset = form.PresetValueReturn;
                            string dbank = form.DesBankValueReturn;
                            string dpreset = form.DesPresetValueReturn;
                            Lis.Items.Add("p" + sbank + "," + spreset + "=" + dbank + "," + dpreset + "|" + s[i]);
                        }
                    }
                    SaveList(CurrentList);
                    TriggerReload();
                }
                else if (Path.GetExtension(s[i]) == ".dls" | Path.GetExtension(s[i]) == ".DLS")
                {
                    MessageBox.Show("BASSMIDI does NOT support the downloadable sounds (DLS) format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (Path.GetExtension(s[i]) == ".exe" | Path.GetExtension(s[i]) == ".EXE" | Path.GetExtension(s[i]) == ".dll" | Path.GetExtension(s[i]) == ".DLL")
                {
                    MessageBox.Show("Are you really trying to add executables to the soundfonts list?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Invalid soundfont!\n\nPlease select a valid soundfont and try again!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void AddSoundfontDragNDropTriv(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        public void UserProfileMigration()
        {
            try
            {
                string oldlocation = System.Environment.GetEnvironmentVariable("LOCALAPPDATA") + "\\Keppy's Synthesizer\\";
                string newlocation = System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Keppy's Synthesizer\\";
                if (IsWindowsXP() == false)
                {
                    Directory.Move(oldlocation, newlocation);
                }
            }
            catch
            {

            }
        }

        private void DriverToSynthMigration()
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

        private void ChangeList(string WhichList)
        {
            try
            {
                if (!System.IO.Directory.Exists(AbsolutePath))
                {
                    Directory.CreateDirectory(AbsolutePath);
                    Directory.CreateDirectory(ListsPath);
                    File.Create(WhichList).Dispose();
                    Lis.Items.Clear();
                }
                if (!System.IO.Directory.Exists(ListsPath))
                {
                    Directory.CreateDirectory(ListsPath);
                    File.Create(WhichList).Dispose();
                    Lis.Items.Clear();
                }
                if (!System.IO.File.Exists(WhichList))
                {
                    File.Create(WhichList).Dispose();
                    Lis.Items.Clear();
                }
                try
                {
                    using (StreamReader r = new StreamReader(WhichList))
                    {
                        string line;
                        Lis.Items.Clear();
                        while ((line = r.ReadLine()) != null)
                        {
                            Lis.Items.Add(line);
                        }
                    }
                }
                catch
                {
                    File.Create(WhichList).Dispose();
                    MessageBox.Show("The soundfont list was missing, so the configurator automatically created it for you.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fatal error during the execution of the program.\n\nPress OK to quit.\n\n.NET error:\n" + ex.Message.ToString(), "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void SaveSettings()
        {
            try
            {
                // Normal settings
                SynthSettings.SetValue("polyphony", PolyphonyLimit.Value.ToString(), RegistryValueKind.DWord);
                SynthSettings.SetValue("cpu", MaxCPU.Value, RegistryValueKind.DWord);
                if (string.IsNullOrWhiteSpace(Frequency.Text))
                {
                    Frequency.Text = "48000";
                    SynthSettings.SetValue("frequency", Frequency.Text, RegistryValueKind.DWord);
                }
                else
                {
                    SynthSettings.SetValue("frequency", Frequency.Text, RegistryValueKind.DWord);
                }

                // Advanced SynthSettings
                SynthSettings.SetValue("buflen", bufsize.Value.ToString(), RegistryValueKind.DWord);
                SynthSettings.SetValue("tracks", TracksLimit.Value.ToString(), RegistryValueKind.DWord);

                // Let's not forget about the volume!
                int VolumeValue = 0;
                double x = VolTrackBar.Value / 100;
                VolumeValue = Convert.ToInt32(x);
                VolSimView.Text = VolumeValue.ToString("000\\%");
                VolIntView.Text = "Value: " + VolTrackBar.Value.ToString("00000");
                SynthSettings.SetValue("volume", VolTrackBar.Value.ToString(), RegistryValueKind.DWord);
                
                // Checkbox stuff yay
                if (Preload.Checked == true)
                {
                    SynthSettings.SetValue("preload", "1", RegistryValueKind.DWord);
                }
                else
                {
                    SynthSettings.SetValue("preload", "0", RegistryValueKind.DWord);
                }
                if (DisableSFX.Checked == true)
                {
                    SynthSettings.SetValue("nofx", "1", RegistryValueKind.DWord);
                }
                else
                {
                    SynthSettings.SetValue("nofx", "0", RegistryValueKind.DWord);
                }
                if (VMSEmu.Checked == true)
                {
                    SynthSettings.SetValue("vmsemu", "1", RegistryValueKind.DWord);
                }
                else
                {
                    SynthSettings.SetValue("vmsemu", "0", RegistryValueKind.DWord);
                }
                if (NoteOffCheck.Checked == true)
                {
                    SynthSettings.SetValue("noteoff", "1", RegistryValueKind.DWord);
                }
                else
                {
                    SynthSettings.SetValue("noteoff", "0", RegistryValueKind.DWord);
                }
                if (SincInter.Checked == true)
                {
                    SynthSettings.SetValue("sinc", "1", RegistryValueKind.DWord);
                }
                else
                {
                    SynthSettings.SetValue("sinc", "0", RegistryValueKind.DWord);
                }
                if (SysResetIgnore.Checked == true)
                {
                    SynthSettings.SetValue("sysresetignore", "1", RegistryValueKind.DWord);
                }
                else
                {
                    SynthSettings.SetValue("sysresetignore", "0", RegistryValueKind.DWord);
                }
                if (OutputWAV.Checked == true)
                {
                    SynthSettings.SetValue("encmode", "1", RegistryValueKind.DWord);
                }
                else
                {
                    SynthSettings.SetValue("encmode", "0", RegistryValueKind.DWord);
                }
                if (XAudioDisable.Checked == true)
                {
                    SynthSettings.SetValue("xaudiodisabled", "1", RegistryValueKind.DWord);
                }
                else
                {
                    SynthSettings.SetValue("xaudiodisabled", "0", RegistryValueKind.DWord);
                }
            }
            catch
            {
                MessageBox.Show("Fatal error during the execution of this program!\n\nPress OK to quit.", "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                Application.Exit();
            }
        }

        private void InitializeLastPath()
        {
            try
            {
                if (SynthPaths.GetValue("lastpathsfimport", null) == null)
                {
                    Registry.CurrentUser.CreateSubKey("SOFTWARE\\Keppy's Synthesizer\\Paths");
                    SynthPaths = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Paths", true);
                    SynthPaths.SetValue("lastpathsfimport", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), RegistryValueKind.String);
                    LastBrowserPath = SynthPaths.GetValue("lastpathsfimport", Environment.GetFolderPath(Environment.SpecialFolder.Desktop)).ToString();
                    SoundfontImport.InitialDirectory = LastBrowserPath;
                }
                else if (SynthPaths.GetValue("lastpathlistimpexp", null) == null)
                {
                    Registry.CurrentUser.CreateSubKey("SOFTWARE\\Keppy's Synthesizer\\Paths");
                    SynthPaths = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Paths", true);
                    SynthPaths.SetValue("lastpathlistimpexp", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), RegistryValueKind.String);
                    LastImportExportPath = SynthPaths.GetValue("lastpathlistimpexp", Environment.GetFolderPath(Environment.SpecialFolder.Desktop)).ToString();
                    ExternalListImport.InitialDirectory = LastImportExportPath;
                    ExternalListExport.InitialDirectory = LastImportExportPath;
                }
                else if (SynthPaths.GetValue("lastpathsfimport", null) == null && SynthPaths.GetValue("lastpathlistimpexp", null) == null)
                {
                    Registry.CurrentUser.CreateSubKey("SOFTWARE\\Keppy's Synthesizer\\Paths");
                    SynthPaths = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Paths", true);
                    SynthPaths.SetValue("lastpathsfimport", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), RegistryValueKind.String);
                    SynthPaths.SetValue("lastpathlistimpexp", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), RegistryValueKind.String);
                    LastBrowserPath = SynthPaths.GetValue("lastpathsfimport", Environment.GetFolderPath(Environment.SpecialFolder.Desktop)).ToString();
                    LastImportExportPath = SynthPaths.GetValue("lastpathlistimpexp", Environment.GetFolderPath(Environment.SpecialFolder.Desktop)).ToString();
                    SoundfontImport.InitialDirectory = LastBrowserPath;
                    ExternalListImport.InitialDirectory = LastImportExportPath;
                    ExternalListExport.InitialDirectory = LastImportExportPath;
                }
                else
                {
                    LastBrowserPath = SynthPaths.GetValue("lastpathsfimport", Environment.GetFolderPath(Environment.SpecialFolder.Desktop)).ToString();
                    LastImportExportPath = SynthPaths.GetValue("lastpathlistimpexp", Environment.GetFolderPath(Environment.SpecialFolder.Desktop)).ToString();
                    SoundfontImport.InitialDirectory = LastBrowserPath;
                    ExternalListImport.InitialDirectory = LastImportExportPath;
                    ExternalListExport.InitialDirectory = LastImportExportPath;
                }
            }
            catch
            {
                Registry.CurrentUser.CreateSubKey("SOFTWARE\\Keppy's Synthesizer\\Paths");
                SynthPaths = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Paths", true);
                SynthPaths.SetValue("lastpathsfimport", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), RegistryValueKind.String);
                SynthPaths.SetValue("lastpathlistimpexp", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), RegistryValueKind.String);
                LastBrowserPath = SynthPaths.GetValue("lastpathsfimport", Environment.GetFolderPath(Environment.SpecialFolder.Desktop)).ToString();
                LastImportExportPath = SynthPaths.GetValue("lastpathlistimpexp", Environment.GetFolderPath(Environment.SpecialFolder.Desktop)).ToString();
                SoundfontImport.InitialDirectory = LastBrowserPath;
                ExternalListImport.InitialDirectory = LastImportExportPath;
                ExternalListExport.InitialDirectory = LastImportExportPath;
                SoundfontImport.InitialDirectory = LastBrowserPath;
            }
        }

        // Here we go!
        private void KeppySynthConfiguratorMain_Load(object sender, EventArgs e)
        {
            // MIDI out selector disabler
            if (IsWindows8OrNewer() == true)
            {
                changeDefaultMIDIOutDeviceToolStripMenuItem1.Text = "Change default MIDI out device for Windows Media Player";
                changeDefaultMIDIOutDeviceToolStripMenuItem.Text = "Change default MIDI out device for Windows Media Player 32-bit";
                changeDefault64bitMIDIOutDeviceToolStripMenuItem.Text = "Change default MIDI out device for Windows Media Player 64-bit";
                getTheMIDIMapperForWindows8xToolStripMenuItem.Visible = true;
                getTheMIDIMapperForWindows10ToolStripMenuItem.Visible = true;
                SetSynthDefault.Visible = true;
            }
            if (IsWindowsXP() == true)
            {
                menuItem17.Visible = false;
                manageFolderFavouritesToolStripMenuItem.Visible = false;
            }
            if (Environment.Is64BitOperatingSystem == false)
            {
                changeDefaultMIDIOutDeviceToolStripMenuItem1.Visible = true;
                changeDefaultMIDIOutDeviceToolStripMenuItem.Visible = false;
                changeDefault64bitMIDIOutDeviceToolStripMenuItem.Visible = false;
            }
            else
            {
                changeDefaultMIDIOutDeviceToolStripMenuItem1.Visible = false;
                changeDefaultMIDIOutDeviceToolStripMenuItem.Visible = true;
                changeDefault64bitMIDIOutDeviceToolStripMenuItem.Visible = true;
            }

            Lis.ContextMenu = RightClickMenu;
            InitializeLastPath();
            SelectedListBox.Text = "List 1";

            // ======= Load settings from the registry
            try
            {
                // First, the most important settings
                VolTrackBar.Value = Convert.ToInt32(SynthSettings.GetValue("volume", 10000));
                PolyphonyLimit.Value = Convert.ToInt32(SynthSettings.GetValue("polyphony", 512));
                MaxCPU.Value = Convert.ToInt32(SynthSettings.GetValue("cpu", 75)); 
                if (Convert.ToInt32(SynthSettings.GetValue("defaultmidiout", 0)) == 0)
                {
                    DefaultOut810enabledToolStripMenuItem.Checked = false;
                    DefaultOut810enabledToolStripMenuItem.Enabled = true;
                    DefaultOut810disabledToolStripMenuItem.Checked = true;
                    DefaultOut810disabledToolStripMenuItem.Enabled = false;
                }
                else
                {
                    DefaultOut810enabledToolStripMenuItem.Checked = true;
                    DefaultOut810enabledToolStripMenuItem.Enabled = false;
                    DefaultOut810disabledToolStripMenuItem.Checked = false;
                    DefaultOut810disabledToolStripMenuItem.Enabled = true;
                }
                if (Convert.ToInt32(SynthSettings.GetValue("allhotkeys", 0)) == 1)
                {
                    hLSEnabledToolStripMenuItem.Checked = true;
                    hLSDisabledToolStripMenuItem.Checked = false;
                    hLSEnabledToolStripMenuItem.Enabled = false;
                    hLSDisabledToolStripMenuItem.Enabled = true;
                }
                else
                {
                    hLSEnabledToolStripMenuItem.Checked = false;
                    hLSDisabledToolStripMenuItem.Checked = true;
                    hLSEnabledToolStripMenuItem.Enabled = true;
                    hLSDisabledToolStripMenuItem.Enabled = false;
                }
                if (Convert.ToInt32(SynthSettings.GetValue("vms2emu", 0)) == 1)
                {
                    VMSSimulatorEnabled.Checked = true;
                    VMSSimulatorDisabled.Checked = false;
                    VMSSimulatorEnabled.Enabled = false;
                    VMSSimulatorDisabled.Enabled = true;
                }
                else
                {
                    VMSSimulatorEnabled.Checked = false;
                    VMSSimulatorDisabled.Checked = true;
                    VMSSimulatorEnabled.Enabled = true;
                    VMSSimulatorDisabled.Enabled = false;
                }
                if (Convert.ToInt32(SynthSettings.GetValue("oldbuffersystem", 0)) == 1)
                {
                    enabledToolStripMenuItem3.Checked = true;
                    disabledToolStripMenuItem3.Checked = false;
                    enabledToolStripMenuItem3.Enabled = false;
                    disabledToolStripMenuItem3.Enabled = true;
                }
                else
                {
                    enabledToolStripMenuItem3.Checked = false;
                    disabledToolStripMenuItem3.Checked = true;
                    enabledToolStripMenuItem3.Enabled = true;
                    disabledToolStripMenuItem3.Enabled = false;
                }
                if (Convert.ToInt32(SynthSettings.GetValue("autoupdatecheck", 1)) == 1)
                {
                    checkEnabledToolStripMenuItem.Checked = true;
                    checkDisabledToolStripMenuItem.Checked = false;
                    checkEnabledToolStripMenuItem.Enabled = false;
                    checkDisabledToolStripMenuItem.Enabled = true;
                }
                else
                {
                    checkEnabledToolStripMenuItem.Checked = false;
                    checkDisabledToolStripMenuItem.Checked = true;
                    checkEnabledToolStripMenuItem.Enabled = true;
                    checkDisabledToolStripMenuItem.Enabled = false;
                }
                if (Convert.ToInt32(SynthSettings.GetValue("extra8lists", 0)) == 1)
                {
                    enabledToolStripMenuItem2.Checked = true;
                    disabledToolStripMenuItem2.Checked = false;
                    enabledToolStripMenuItem2.Enabled = false;
                    disabledToolStripMenuItem2.Enabled = true;
                    MoreLists.Visible = true;
                    SelectedListBox.Items.Add("List 9");
                    SelectedListBox.Items.Add("List 10");
                    SelectedListBox.Items.Add("List 11");
                    SelectedListBox.Items.Add("List 12");
                    SelectedListBox.Items.Add("List 13");
                    SelectedListBox.Items.Add("List 14");
                    SelectedListBox.Items.Add("List 15");
                    SelectedListBox.Items.Add("List 16");
                }
                else
                {
                    enabledToolStripMenuItem2.Checked = false;
                    disabledToolStripMenuItem2.Checked = true;
                    enabledToolStripMenuItem2.Enabled = true;
                    disabledToolStripMenuItem2.Enabled = false;
                    MoreLists.Visible = false;
                }
                if (Convert.ToInt32(Watchdog.GetValue("watchdog", 1)) == 1)
                {
                    watchdogEnabledToolStripMenuItem.Checked = true;
                    watchdogDisabledToolStripMenuItem.Checked = false;
                    watchdogEnabledToolStripMenuItem.Enabled = false;
                    watchdogDisabledToolStripMenuItem.Enabled = true;
                    runTheWatchdogToolStripMenuItem.Enabled = true;
                    killTheWatchdogToolStripMenuItem.Enabled = true;
                }
                else
                {
                    watchdogEnabledToolStripMenuItem.Checked = false;
                    watchdogDisabledToolStripMenuItem.Checked = true;
                    watchdogEnabledToolStripMenuItem.Enabled = true;
                    watchdogDisabledToolStripMenuItem.Enabled = false;
                    runTheWatchdogToolStripMenuItem.Enabled = false;
                    killTheWatchdogToolStripMenuItem.Enabled = false;
                }
                if (Convert.ToInt32(Watchdog.GetValue("watchdognotify", 1)) == 1)
                {
                    MIDIAppNotifyEnabledToolStripMenuItem.Checked = true;
                    MIDIAppNotifyDisabledToolStripMenuItem.Checked = false;
                    MIDIAppNotifyEnabledToolStripMenuItem.Enabled = false;
                    MIDIAppNotifyDisabledToolStripMenuItem.Enabled = true;
                }
                else
                {
                    MIDIAppNotifyEnabledToolStripMenuItem.Checked = false;
                    MIDIAppNotifyDisabledToolStripMenuItem.Checked = true;
                    MIDIAppNotifyEnabledToolStripMenuItem.Enabled = true;
                    MIDIAppNotifyDisabledToolStripMenuItem.Enabled = false;
                }
                Frequency.Text = SynthSettings.GetValue("frequency", 44100).ToString();
                TracksLimit.Value = Convert.ToInt32(SynthSettings.GetValue("tracks", 16));

                // Then the filthy checkboxes
                if (Convert.ToInt32(SynthSettings.GetValue("preload", 1)) == 1)
                {
                    Preload.Checked = true;
                }
                else
                {
                    Preload.Checked = false;
                }
                if (Convert.ToInt32(SynthSettings.GetValue("nofx", 0)) == 1)
                {
                    DisableSFX.Checked = true;
                }
                else
                {
                    DisableSFX.Checked = false;
                }
                if (Convert.ToInt32(SynthSettings.GetValue("noteoff", 0)) == 1)
                {
                    NoteOffCheck.Checked = true;
                }
                else
                {
                    NoteOffCheck.Checked = false;
                }
                if (Convert.ToInt32(SynthSettings.GetValue("sysresetignore", 0)) == 1)
                {
                    SysResetIgnore.Checked = true;
                }
                else
                {
                    SysResetIgnore.Checked = false;
                }
                if (Convert.ToInt32(SynthSettings.GetValue("encmode", 0)) == 1)
                {
                    OutputWAV.Checked = true;
                }
                else
                {
                    OutputWAV.Checked = false;
                }
                if (Convert.ToInt32(SynthSettings.GetValue("xaudiodisabled", 0)) == 1)
                {
                    XAudioDisable.Checked = true;
                    VMSEmu.Visible = true;
                    SPFSecondaryBut.Visible = false;
                    if (Convert.ToInt32(SynthSettings.GetValue("sinc", 0)) == 1)
                    {
                        SincInter.Checked = true;
                    }
                    else
                    {
                        SincInter.Checked = false;
                    }
                    if (Convert.ToInt32(SynthSettings.GetValue("vmsemu", 0)) == 1)
                    {
                        VMSEmu.Checked = true;
                        bufsize.Enabled = true;
                    }
                    else
                    {
                        VMSEmu.Checked = false;
                        bufsize.Enabled = false;
                        bufsize.Value = 0;
                    }
                }
                else
                {
                    XAudioDisable.Checked = false;
                    VMSEmu.Visible = false;
                    SPFSecondaryBut.Visible = true;
                    bufsize.Enabled = true;
                    SincInter.Checked = true;
                    SincInter.Enabled = false;
                    SincInter.Text = "Enable sinc interpolation. (Already applied by XAudio itself, it doesn't cause CPU overhead.)";
                }

                // LEL
                bufsize.Value = Convert.ToInt32(SynthSettings.GetValue("buflen"));

                // And finally, the volume!
                int VolumeValue = Convert.ToInt32(SynthSettings.GetValue("volume"));
                double x = VolumeValue / 100;
                VolSimView.Text = x.ToString("000\\%");
                VolIntView.Text = "Value: " + VolTrackBar.Value.ToString("00000");
                VolTrackBar.Value = VolumeValue;

                // Jakuberino
                if (openadvanced == 1)
                {
                    TabsForTheControls.SelectedIndex = 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Missing registry settings!\nPlease reinstall Keppy's Synthesizer to solve the issue.\n\nPress OK to quit.\n\n.NET error:\n" + ex.Message.ToString(), "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(ignored =>
                {
                    this.Hide();
                    throw new Exception();
                }));
            }
        }

        private void VolTrackBar_Scroll(object sender, EventArgs e)
        {
            try
            {
                int VolumeValue = 0;
                double x = VolTrackBar.Value / 100;
                VolumeValue = Convert.ToInt32(x);
                VolSimView.Text = VolumeValue.ToString("000\\%");
                VolIntView.Text = "Value: " + VolTrackBar.Value.ToString("00000");
                SynthSettings.SetValue("volume", VolTrackBar.Value.ToString(), RegistryValueKind.DWord);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Crap, an error!\n\nError:\n" + ex.ToString(), "Oh no! Keppy's Synthesizer encountered an error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // -------------------------
        // Soundfont lists functions

        private void TriggerReload() {
            try
            {
                if (Convert.ToInt32(Watchdog.GetValue("currentsflist")) == whichone)
                {
                    Watchdog.SetValue("rel" + whichone.ToString(), "1", RegistryValueKind.DWord);
                }
            }
            catch
            {
                InitializeLastPath();
            }
        }

        private void SetLastPath(string path)
        {
            try
            {
                SynthPaths.SetValue("lastpathsfimport", path);
            }
            catch
            {
                InitializeLastPath();
            }
        }

        private void SetLastImportExportPath(string path)
        {
            try
            {
                SynthPaths.SetValue("lastpathlistimpexp", path);
            }
            catch
            {

            }
        }

        private void OpenFileDialogAddCustomPaths(FileDialog dialog)
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


        private void CLi_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to clear the list?", "Keppy's Synthesizer Configurator ~ Clear list " + whichone.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    Lis.Items.Clear();
                    File.Delete(CurrentList);
                    var TempFile = File.Create(CurrentList);
                    TempFile.Close();
                    if (Convert.ToInt32(Watchdog.GetValue("currentsflist")) == whichone)
                    {
                        Watchdog.SetValue("rel" + whichone.ToString(), "1", RegistryValueKind.DWord);
                    }
                }
                catch (Exception ex)
                {
                    ReinitializeList(ex, CurrentList);
                }
            }
            else if (dialogResult == DialogResult.No)
            {

            }
        }

        private void AddSF_Click(object sender, EventArgs e)
        {
            try
            {
                SoundfontImport.InitialDirectory = LastBrowserPath;
                SoundfontImport.FileName = "";
                OpenFileDialogAddCustomPaths(SoundfontImport);
                if (SoundfontImport.ShowDialog() == DialogResult.OK)
                {
                    foreach (string str in SoundfontImport.FileNames)
                    {
                        if (Path.GetExtension(str).ToLower() == ".sf2" | Path.GetExtension(str).ToLower() == ".sfpack")
                        {
                            if (BankPresetOverride.Checked == true)
                            {
                                using (var form = new BankNPresetSel(Path.GetFileName(str), 0, 1))
                                {
                                    var result = form.ShowDialog();
                                    if (result == DialogResult.OK)
                                    {
                                        string sbank = form.BankValueReturn;
                                        string spreset = form.PresetValueReturn;
                                        string dbank = form.DesBankValueReturn;
                                        string dpreset = form.DesPresetValueReturn;
                                        Lis.Items.Add("p" + sbank + "," + spreset + "=" + dbank + "," + dpreset + "|" + str);
                                    }
                                }
                            }
                            else
                            {
                                Lis.Items.Add(str);
                            }
                        }
                        else if (Path.GetExtension(str).ToLower() == ".sfz")
                        {
                            using (var form = new BankNPresetSel(Path.GetFileName(str), 0, 0))
                            {
                                var result = form.ShowDialog();
                                if (result == DialogResult.OK)
                                {
                                    string sbank = form.BankValueReturn;
                                    string spreset = form.PresetValueReturn;
                                    string dbank = form.DesBankValueReturn;
                                    string dpreset = form.DesPresetValueReturn;
                                    Lis.Items.Add("p" + sbank + "," + spreset + "=" + dbank + "," + dpreset + "|" + str);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show(Path.GetFileName(str) + " is not a valid soundfont file!", "Error while adding soundfont", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        LastBrowserPath = Path.GetDirectoryName(str);
                        SetLastPath(LastBrowserPath);
                    }
                    SaveList(CurrentList);
                    TriggerReload();
                }
            }
            catch (Exception ex)
            {
                ReinitializeList(ex, CurrentList);
            }
        }

        private void RmvSF_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = Lis.SelectedIndices.Count - 1; i >= 0; i--)
                {
                    Lis.Items.RemoveAt(Lis.SelectedIndices[i]);
                }
                TriggerReload();
                SaveList(CurrentList);
            }
            catch (Exception ex)
            {
                ReinitializeList(ex, CurrentList);
            }
        }

        private void MvU_Click(object sender, EventArgs e)
        {
            try
            {
                int howmany = Lis.SelectedItems.Count;
                if (howmany > 0)
                {
                    object selected = Lis.SelectedItem;
                    int indx = Lis.Items.IndexOf(selected);
                    int totl = Lis.Items.Count;
                    if (indx == 0)
                    {
                        Lis.Items.Remove(selected);
                        Lis.Items.Insert(totl - 1, selected);
                        Lis.SetSelected(totl - 1, true);
                    }
                    else
                    {
                        Lis.Items.Remove(selected);
                        Lis.Items.Insert(indx - 1, selected);
                        Lis.SetSelected(indx - 1, true);
                    }
                }
                SaveList(CurrentList);
                TriggerReload();
            }
            catch (Exception ex)
            {
                ReinitializeList(ex, CurrentList);
            }
        }

        private void MvD_Click(object sender, EventArgs e)
        {
            try
            {
                int howmany = Lis.SelectedItems.Count;
                if (howmany > 0)
                {
                    object selected = Lis.SelectedItem;
                    int indx = Lis.Items.IndexOf(selected);
                    int totl = Lis.Items.Count;
                    if (indx == totl - 1)
                    {
                        Lis.Items.Remove(selected);
                        Lis.Items.Insert(0, selected);
                        Lis.SetSelected(0, true);
                    }
                    else
                    {
                        Lis.Items.Remove(selected);
                        Lis.Items.Insert(indx + 1, selected);
                        Lis.SetSelected(indx + 1, true);
                    }

                }
                SaveList(CurrentList);
                TriggerReload();
            }
            catch (Exception ex)
            {
                ReinitializeList(ex, CurrentList);
            }
        }

        private void LoadToApp_Click(object sender, EventArgs e)
        {
            Watchdog.SetValue("currentsflist", whichone, RegistryValueKind.DWord);
            Watchdog.SetValue("rel" + whichone.ToString(), "1", RegistryValueKind.DWord);
        }

        private void EnableSF_Click(object sender, EventArgs e)
        {
            try
            {
                if (Lis.SelectedIndex == -1)
                {
                    MessageBox.Show("Select a soundfont first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (Lis.SelectedIndices.Count > 1)
                    {
                        MessageBox.Show("You can only enable one soundfont at the time!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        string result = Lis.SelectedItem.ToString().Substring(0, 1);
                        if (result == "@")
                        {
                            string newvalue = Lis.SelectedItem.ToString().Remove(0, 1);
                            int index = Lis.Items.IndexOf(Lis.SelectedItem);
                            Lis.Items.RemoveAt(index);
                            Lis.Items.Insert(index, newvalue);
                        }
                        else
                        {
                            MessageBox.Show("The soundfont is already enabled!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        SaveList(CurrentList);
                        TriggerReload();
                    }
                }
            }
            catch (Exception ex)
            {
                ReinitializeList(ex, CurrentList);
            }
        }

        private void DisableSF_Click(object sender, EventArgs e)
        {
            try
            {
                if (Lis.SelectedIndex == -1)
                {
                    MessageBox.Show("Select a soundfont first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (Lis.SelectedIndices.Count > 1)
                    {
                        MessageBox.Show("You can only disable one soundfont at the time!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        string result = Lis.SelectedItem.ToString().Substring(0, 1);
                        if (result != "@")
                        {
                            string newvalue = "@" + Lis.SelectedItem.ToString();
                            int index = Lis.Items.IndexOf(Lis.SelectedItem);
                            Lis.Items.RemoveAt(index);
                            Lis.Items.Insert(index, newvalue);
                        }
                        else
                        {
                            MessageBox.Show("The soundfont is already disabled!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        SaveList(CurrentList);
                        TriggerReload();
                    }
                }
            }
            catch (Exception ex)
            {
                ReinitializeList(ex, CurrentList);
            }
        }

        private void IEL_Click(object sender, EventArgs e)
        {
            try
            {
                ExternalListImport.FileName = "";
                ExternalListImport.InitialDirectory = LastImportExportPath;
                if (ExternalListImport.ShowDialog() == DialogResult.OK)
                {
                    foreach (string file in ExternalListImport.FileNames)
                    {
                        LastImportExportPath = Path.GetDirectoryName(file);
                        SetLastPath(LastImportExportPath);
                        using (StreamReader r = new StreamReader(file))
                        {
                            string line;
                            while ((line = r.ReadLine()) != null)
                            {
                                Lis.Items.Add(line); // Read the external list and add the items to the selected list
                            }
                        }
                        SaveList(CurrentList);
                        TriggerReload();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Error during the import process of the list!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void EL_Click(object sender, EventArgs e)
        {
            ExternalListExport.FileName = "";
            ExternalListExport.InitialDirectory = LastImportExportPath;
            if (ExternalListExport.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamWriter SaveFile = new System.IO.StreamWriter(ExternalListExport.FileName);
                SetLastPath(LastImportExportPath);
                foreach (var item in Lis.Items)
                {
                    SaveFile.WriteLine(item.ToString());
                }
                SaveFile.Close();
                MessageBox.Show("Soundfont list exported succesfully to \"" + Path.GetDirectoryName(ExternalListExport.FileName) + "\\\"", "Soundfont list exported!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SelectedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedListBox.Text == "List 1")
            {
                CurrentList = List1Path;
                whichone = 1;
                ChangeList(List1Path);
            }
            else if (SelectedListBox.Text == "List 2")
            {
                CurrentList = List2Path;
                whichone = 2;
                ChangeList(List2Path);
            }
            else if (SelectedListBox.Text == "List 3")
            {
                CurrentList = List3Path;
                whichone = 3;
                ChangeList(List3Path);
            }
            else if (SelectedListBox.Text == "List 4")
            {
                CurrentList = List4Path;
                whichone = 4;
                ChangeList(List4Path);
            }
            else if (SelectedListBox.Text == "List 5")
            {
                CurrentList = List5Path;
                whichone = 5;
                ChangeList(List5Path);
            }
            else if (SelectedListBox.Text == "List 6")
            {
                CurrentList = List6Path;
                whichone = 6;
                ChangeList(List6Path);
            }
            else if (SelectedListBox.Text == "List 7")
            {
                CurrentList = List7Path;
                whichone = 7;
                ChangeList(List7Path);
            }
            else if (SelectedListBox.Text == "List 8")
            {
                CurrentList = List8Path;
                whichone = 8;
                ChangeList(List8Path);
            }
            else if (SelectedListBox.Text == "List 9")
            {
                CurrentList = List9Path;
                whichone = 9;
                ChangeList(List9Path);
            }
            else if (SelectedListBox.Text == "List 10")
            {
                CurrentList = List10Path;
                whichone = 10;
                ChangeList(List10Path);
            }
            else if (SelectedListBox.Text == "List 11")
            {
                CurrentList = List11Path;
                whichone = 11;
                ChangeList(List11Path);
            }
            else if (SelectedListBox.Text == "List 12")
            {
                CurrentList = List12Path;
                whichone = 12;
                ChangeList(List12Path);
            }
            else if (SelectedListBox.Text == "List 13")
            {
                CurrentList = List13Path;
                whichone = 13;
                ChangeList(List13Path);
            }
            else if (SelectedListBox.Text == "List 14")
            {
                CurrentList = List14Path;
                whichone = 14;
                ChangeList(List14Path);
            }
            else if (SelectedListBox.Text == "List 15")
            {
                CurrentList = List15Path;
                whichone = 15;
                ChangeList(List15Path);
            }
            else if (SelectedListBox.Text == "List 16")
            {
                CurrentList = List16Path;
                whichone = 16;
                ChangeList(List16Path);
            }
        }

        private void Lis_DragDrop(object sender, DragEventArgs e)
        {
            AddSoundfontDragNDrop(CurrentList, e);
        }

        private void Lis_DragEnter(object sender, DragEventArgs e)
        {
            AddSoundfontDragNDropTriv(e);
        }

        // End of the soundfont lists functions
        // ------------------------------------

        private void resetToDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Set some values...
            VolTrackBar.Value = 10000;
            PolyphonyLimit.Value = 512;
            MaxCPU.Value = 65;
            Frequency.Text = "48000";
            bufsize.Value = 30;
            TracksLimit.Value = 16;
            Preload.Checked = true;
            NoteOffCheck.Checked = false;
            SincInter.Checked = false;
            DisableSFX.Checked = false;
            SysResetIgnore.Checked = false;
            OutputWAV.Checked = false;
            XAudioDisable.Checked = false;
            VMSEmu.Checked = false;

            // And then...
            SaveSettings();

            // Messagebox here
            MessageBox.Show("Settings restored to the default values!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void applySettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Just save the Settings
            SaveSettings();

            // Messagebox here
            MessageBox.Show("Settings saved to the registry!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void blackMIDIsPresetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Set some values...
            VolTrackBar.Value = 10000;
            PolyphonyLimit.Value = 1000;
            MaxCPU.Value = 75;
            Frequency.Text = "44100";
            bufsize.Value = 15;
            TracksLimit.Value = 16;
            Preload.Checked = true;
            NoteOffCheck.Checked = false;
            SincInter.Checked = false;
            DisableSFX.Checked = true;
            SysResetIgnore.Checked = true;
            OutputWAV.Checked = false;
            XAudioDisable.Checked = false;
            VMSEmu.Checked = false;

            // And then...
            SaveSettings();

            // Messagebox here
            MessageBox.Show("The Black MIDIs preset has been applied!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void lowLatencyPresetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Set some values...
            VolTrackBar.Value = 10000;
            PolyphonyLimit.Value = 500;
            MaxCPU.Value = 80;
            Frequency.Text = "44100";
            bufsize.Value = 10;
            TracksLimit.Value = 16;
            Preload.Checked = true;
            NoteOffCheck.Checked = true;
            SincInter.Checked = true;
            DisableSFX.Checked = false;
            SysResetIgnore.Checked = true;
            OutputWAV.Checked = false;
            XAudioDisable.Checked = false;
            VMSEmu.Checked = false;

            // And then...
            SaveSettings();

            // Messagebox here
            MessageBox.Show("The low latency preset has been applied!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Now, menustrip functions here

        private void openDebugWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Process.GetProcessesByName("KeppySynthDebugWindow").Length > 0)
            {
                MessageBox.Show("The debug window is already opened!", "Keppy's Synthesizer Configurator - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\keppysynth\\KeppySynthDebugWindow.exe");
            }
        }

        private void openTheMixerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KeppySynthMixerWindow.GetForm.Show();
        }

        private void openTheBlacklistManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KeppySynthBlacklistSystem frm = new KeppySynthBlacklistSystem();
            frm.ShowDialog();
        }

        private void informationAboutTheDriverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KeppySynthInformation frm = new KeppySynthInformation();
            frm.ShowDialog();
        }

        private void changeDefaultMIDIOutDeviceToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (IsWindowsXP() == false)
            {
                Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\keppysynth\\midioutsetter32.exe");
            }
            else
            {
                Process MIDIPanelXP = new Process();
                MIDIPanelXP.StartInfo.FileName = "rundll32.exe";
                MIDIPanelXP.StartInfo.Arguments = "MMSYS.CPL,ShowAudioPropertySheet";
                MIDIPanelXP.Start();
                this.Enabled = false;
                MIDIPanelXP.WaitForExit();
                this.Enabled = true;
            }
        }

        private void changeDefault64bitMIDIOutDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsWindowsXP() == false)
            {
                Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\keppysynth\\midioutsetter64.exe");
            }
            else
            {
                Process MIDIPanelXP = new Process();
                MIDIPanelXP.StartInfo.FileName = "rundll32.exe";
                MIDIPanelXP.StartInfo.Arguments = "MMSYS.CPL,ShowAudioPropertySheet";
                MIDIPanelXP.Start();
                this.Enabled = false;
                MIDIPanelXP.WaitForExit();
                this.Enabled = true;
            }
        }

        private void openUpdaterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KeppySynthUpdater frm = new KeppySynthUpdater();
            frm.ShowDialog();
        }

        private void reportABugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to report a bug about Keppy's Synthesizer?\n\nWARNING: Only use this function to report serious bugs, like memory leaks and security flaws.", "Report a bug...", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                Process.Start("https://github.com/KaleidonKep99/Keppy-s-MIDI-Driver/issues");
            }
            else if (dialogResult == DialogResult.No)
            {
                
            }
        }

        private void downloadTheSourceCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/KaleidonKep99/Keppy-s-MIDI-Driver");
        }

        private void visitKeppyStudiosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://keppystudios.com");
        }

        private void getTheMIDIMapperForWindows8xToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://drive.google.com/file/d/0B05Sp4zxPFR6UW9CQ0RRak85eDA/view?usp=sharing");
        }

        private void getTheMIDIMapperForWindows10ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://plus.google.com/+RichardForhenson/posts/bkrqUfbV3xz");
        }

        private void donateToSupportUsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = "";

            string business = "prapapappo1999@gmail.com";
            string description = "Donation";
            string country = "US";
            string currency = "USD";

            url += "https://www.paypal.com/cgi-bin/webscr" +
                "?cmd=" + "_donations" +
                "&business=" + business +
                "&lc=" + country +
                "&item_name=" + description +
                "&currency_code=" + currency +
                "&bn=" + "PP%2dDonationsBF";

            Process.Start(url);
        }

        private void changeTheSizeOfTheEVBufferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KeppySynthEVBuffer frm = new KeppySynthEVBuffer();
            frm.ShowDialog();
        }

        private void changeDirectoryOfTheOutputToWAVModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KeppySynthOutputWAVDir frm = new KeppySynthOutputWAVDir();
            frm.ShowDialog();
        }

        private void changeDefaultSoundfontListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KeppySynthDefaultSFList frm = new KeppySynthDefaultSFList();
            frm.ShowDialog();
        }

        private void changeDefaultSoundfontListToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            KeppySynthDefaultSFList frm = new KeppySynthDefaultSFList();
            frm.ShowDialog();
        }

        private void changeTheMaximumSamplesPerFrameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KeppySynthSamplePerFrameSetting frm = new KeppySynthSamplePerFrameSetting();
            frm.ShowDialog();
        }

        private void SPFSecondaryBut_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            KeppySynthSamplePerFrameSetting frm = new KeppySynthSamplePerFrameSetting();
            frm.ShowDialog();
        }

        private void assignASoundfontListToASpecificAppToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KeppySynthSFListAssign frm = new KeppySynthSFListAssign();
            frm.ShowDialog();
        }

        private void manageFolderFavouritesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KeppySynthFavouritesManager frm = new KeppySynthFavouritesManager();
            frm.ShowDialog();
        }

        private void watchdogEnabledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Watchdog.SetValue("watchdog", "1", RegistryValueKind.DWord);
            watchdogEnabledToolStripMenuItem.Checked = true;
            watchdogDisabledToolStripMenuItem.Checked = false;
            watchdogEnabledToolStripMenuItem.Enabled = false;
            watchdogDisabledToolStripMenuItem.Enabled = true;
            runTheWatchdogToolStripMenuItem.Enabled = true;
            killTheWatchdogToolStripMenuItem.Enabled = true;
            menuItem18.Visible = false;
            sendAMIDIResetEventToAllTheChannelsStrip.Visible = false;
            menuItem22.Visible = false;
            ReloadList1.Visible = false;
            ReloadList2.Visible = false;
            ReloadList3.Visible = false;
            ReloadList4.Visible = false;
            ReloadList5.Visible = false;
            ReloadList6.Visible = false;
            ReloadList7.Visible = false;
            ReloadList8.Visible = false;
            MoreLists.Visible = false;
        }

        private void watchdogDisabledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Watchdog.SetValue("watchdog", "0", RegistryValueKind.DWord);
            watchdogEnabledToolStripMenuItem.Checked = false;
            watchdogDisabledToolStripMenuItem.Checked = true;
            watchdogEnabledToolStripMenuItem.Enabled = true;
            watchdogDisabledToolStripMenuItem.Enabled = false;
            runTheWatchdogToolStripMenuItem.Enabled = false;
            killTheWatchdogToolStripMenuItem.Enabled = false;
            menuItem18.Visible = true;
            sendAMIDIResetEventToAllTheChannelsStrip.Visible = true;
            menuItem22.Visible = true;
            ReloadList1.Visible = true;
            ReloadList2.Visible = true;
            ReloadList3.Visible = true;
            ReloadList4.Visible = true;
            ReloadList5.Visible = true;
            ReloadList6.Visible = true;
            ReloadList7.Visible = true;
            ReloadList8.Visible = true;
            if (enabledToolStripMenuItem2.Checked == true)
            {
                MoreLists.Visible = true;
            }
        }


        private void MIDIAppNotifyEnabledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Watchdog.SetValue("watchdognotify", "1", RegistryValueKind.DWord);
            MIDIAppNotifyEnabledToolStripMenuItem.Checked = true;
            MIDIAppNotifyDisabledToolStripMenuItem.Checked = false;
            MIDIAppNotifyEnabledToolStripMenuItem.Enabled = false;
            MIDIAppNotifyDisabledToolStripMenuItem.Enabled = true;
        }

        private void MIDIAppNotifyDisabledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Watchdog.SetValue("watchdognotify", "0", RegistryValueKind.DWord);
            MIDIAppNotifyEnabledToolStripMenuItem.Checked = false;
            MIDIAppNotifyDisabledToolStripMenuItem.Checked = true;
            MIDIAppNotifyEnabledToolStripMenuItem.Enabled = true;
            MIDIAppNotifyDisabledToolStripMenuItem.Enabled = false;
        }

        private void checkEnabledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SynthSettings.SetValue("autoupdatecheck", "1", RegistryValueKind.DWord);
            checkEnabledToolStripMenuItem.Checked = true;
            checkDisabledToolStripMenuItem.Checked = false;
            checkEnabledToolStripMenuItem.Enabled = false;
            checkDisabledToolStripMenuItem.Enabled = true;
        }

        private void checkDisabledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SynthSettings.SetValue("autoupdatecheck", "0", RegistryValueKind.DWord);
            checkEnabledToolStripMenuItem.Checked = false;
            checkDisabledToolStripMenuItem.Checked = true;
            checkEnabledToolStripMenuItem.Enabled = true;
            checkDisabledToolStripMenuItem.Enabled = false;
        }

        private void DefaultOut810enabledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SynthSettings.SetValue("defaultmidiout", "1", RegistryValueKind.DWord);
            DefaultOut810enabledToolStripMenuItem.Checked = true;
            DefaultOut810disabledToolStripMenuItem.Checked = false;
            DefaultOut810enabledToolStripMenuItem.Enabled = false;
            DefaultOut810disabledToolStripMenuItem.Enabled = true;
        }

        private void DefaultOut810disabledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SynthSettings.SetValue("defaultmidiout", "0", RegistryValueKind.DWord);
            DefaultOut810enabledToolStripMenuItem.Checked = false;
            DefaultOut810disabledToolStripMenuItem.Checked = true;
            DefaultOut810enabledToolStripMenuItem.Enabled = true;
            DefaultOut810disabledToolStripMenuItem.Enabled = false;
        }

        private void enabledToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            SynthSettings.SetValue("oldbuffersystem", "1", RegistryValueKind.DWord);
            enabledToolStripMenuItem3.Checked = true;
            disabledToolStripMenuItem3.Checked = false;
            enabledToolStripMenuItem3.Enabled = false;
            disabledToolStripMenuItem3.Enabled = true;
        }

        private void disabledToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            SynthSettings.SetValue("oldbuffersystem", "0", RegistryValueKind.DWord);
            enabledToolStripMenuItem3.Checked = false;
            disabledToolStripMenuItem3.Checked = true;
            enabledToolStripMenuItem3.Enabled = true;
            disabledToolStripMenuItem3.Enabled = false;
        }

        private void VMSSimulatorEnabled_Click(object sender, EventArgs e)
        {
            SynthSettings.SetValue("vms2emu", "1", RegistryValueKind.DWord);
            VMSSimulatorEnabled.Checked = true;
            VMSSimulatorDisabled.Checked = false;
            VMSSimulatorEnabled.Enabled = false;
            VMSSimulatorDisabled.Enabled = true;
        }

        private void VMSSimulatorDisabled_Click(object sender, EventArgs e)
        {
            SynthSettings.SetValue("vms2emu", "0", RegistryValueKind.DWord);
            VMSSimulatorEnabled.Checked = false;
            VMSSimulatorDisabled.Checked = true;
            VMSSimulatorEnabled.Enabled = true;
            VMSSimulatorDisabled.Enabled = false;
        }

        private void enabledToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            SynthSettings.SetValue("extra8lists", "1", RegistryValueKind.DWord);
            enabledToolStripMenuItem2.Checked = true;
            disabledToolStripMenuItem2.Checked = false;
            enabledToolStripMenuItem2.Enabled = false;
            disabledToolStripMenuItem2.Enabled = true;
            MoreLists.Visible = true;
            SelectedListBox.Items.Add("List 9");
            SelectedListBox.Items.Add("List 10");
            SelectedListBox.Items.Add("List 11");
            SelectedListBox.Items.Add("List 12");
            SelectedListBox.Items.Add("List 13");
            SelectedListBox.Items.Add("List 14");
            SelectedListBox.Items.Add("List 15");
            SelectedListBox.Items.Add("List 16");
        }

        private void disabledToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            SynthSettings.SetValue("extra8lists", "0", RegistryValueKind.DWord);
            enabledToolStripMenuItem2.Checked = false;
            disabledToolStripMenuItem2.Checked = true;
            enabledToolStripMenuItem2.Enabled = true;
            disabledToolStripMenuItem2.Enabled = false;
            MoreLists.Visible = false;
            SelectedListBox.Items.RemoveAt(8);
            SelectedListBox.Items.RemoveAt(8);
            SelectedListBox.Items.RemoveAt(8);
            SelectedListBox.Items.RemoveAt(8);
            SelectedListBox.Items.RemoveAt(8);
            SelectedListBox.Items.RemoveAt(8);
            SelectedListBox.Items.RemoveAt(8);
            SelectedListBox.Items.RemoveAt(8);
        }

        private void runTheWatchdogToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Watchdog.SetValue("closewatchdog", "0", RegistryValueKind.DWord);
            Watchdog.SetValue("watchdog", "1", RegistryValueKind.DWord);
            Watchdog.SetValue("wdrun", "1", RegistryValueKind.DWord);
            watchdogEnabledToolStripMenuItem.Checked = true;
            watchdogDisabledToolStripMenuItem.Checked = false;
            watchdogEnabledToolStripMenuItem.Enabled = false;
            watchdogDisabledToolStripMenuItem.Enabled = true;
            System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\keppysynth\\KeppySynthWatchdog.exe");
        }

        private void killTheWatchdogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (Process proc in Process.GetProcessesByName("KeppySynthWatchdog"))
                {
                    proc.Kill();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Watchdog.SetValue("closewatchdog", "1", RegistryValueKind.DWord);
            Watchdog.SetValue("wdrun", "0", RegistryValueKind.DWord);
        }

        private void FLStudioLicenseDiscount_Click(object sender, EventArgs e)
        {
            Process.Start("http://affiliate.image-line.com/BFHHCGAE552");
        }

        private void CloseConfigurator(object sender, CancelEventArgs e)
        {
            Application.ExitThread();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
        }

        // Guide part
        private void isThereAnyShortcutForToOpenTheConfiguratorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To open the configurator while playing a MIDI, press ALT+9.\nYou could also press ALT+0 to directly open the \"Settings\" tab.",
                "What are the hotkeys to open the configurator?", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        private void whatAreTheHotkeysToChangeTheVolumeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To change the volume, simply press \"Add\" or \"Subtract\" buttons of the numeric keypad.\n\nYou can disable the hotkeys through \"Advanced settings > Volume hotkeys\".",
                "What are the hotkeys to change the volume?", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        private void howCanIChangeTheSoundfontListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To change the current soundfont list, press and hold ALT, then click a number from 1 to 8.\n\n" +
                "ALT+1: Load soundfont list 1\nALT+2: Load soundfont list 2\nALT+3: Load soundfont list 3\nALT+4: Load soundfont list 4\nALT+5: Load soundfont list 5\nALT+6: Load soundfont list 6\nALT+7: Load soundfont list 7\nALT+8: Load soundfont list 8\nCTRL+ALT+1: Load soundfont list 9\nCTRL+ALT+2: Load soundfont list 10\nCTRL+ALT+3: Load soundfont list 11\nCTRL+ALT+4: Load soundfont list 12\nCTRL+ALT+5: Load soundfont list 13\nCTRL+ALT+6: Load soundfont list 14\nCTRL+ALT+7: Load soundfont list 15\nCTRL+ALT+8: Load soundfont list 16\n\n" +
                "You can also reload lists that are already loaded in memory.", "How can I change the soundfont list?", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        private void howCanIResetTheDriverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To reset the driver, press INS.\nThis will stop all the samples that are currently playing, and it'll also send a \"System Reset\" to all the MIDI channels.", "How can I reset the driver?", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        private void whatsTheBestSettingsForTheBufferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("For SoundBlaster-based audio cards, it's 10.\nFor Realtek audio cards, it's 15-20.\nFor VIA audio cards, it's 20.\nFor Conexant audio cards, it's 30.\nFor USB DACs, it's 25-35.\nFor all the AC'97 audio cards, it's 35.\n\nIt's possible to set it to 10 with really fast computers.", "What's the best settings for the buffer?", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        // Brand new output mode
        private void WhatIsOutput_Click(object sender, EventArgs e)
        {
            MessageBox.Show("If you check this option, the driver will create a WAV file on your desktop, called \"(programname).exe - Keppy's Synthesizer Output File.wav\".\n\n" + 
                "You can change the output directory by clicking \"Settings > Change directory of the \"Output to WAV\" mode\".\n\n" + 
                "(The audio output to the speakers/headphones will be disabled, to avoid corruptions in the audio export.)", "\"Output to WAV mode\"? What is it?", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Brand new XAudio disabler
        private void WhatIsXAudio_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Check this, if you don't want static noises or the XAudio interface doesn't work properly and/or it's buggy.\n\n(Notice: Disabling XAudio also increases the latency by a bit, and disables the \"Output to WAV\" mode.)", "\"Disable the XAudio engine\"? What is it?", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void XAudioDisable_CheckedChanged(object sender, EventArgs e)
        {
            if (XAudioDisable.Checked == true)
            {
                OutputWAV.Enabled = false;
                OutputWAV.Checked = false;
                VMSEmu.Visible = true;
                SPFSecondaryBut.Visible = false;
                BufferText.Text = "Set a additional buffer length for the driver, from 0 to 1000:";
                SincInter.Checked = false;
                SincInter.Enabled = true;
                SincInter.Text = "Enable sinc interpolation. (Improves audio quality with cheap soundfont, but increases CPU usage.)";
                bufsize.Minimum = 0;
                bufsize.Maximum = 1000;
                bufsize.Enabled = false;
                if (VMSEmu.Checked == true)
                {
                    bufsize.Enabled = true;
                }
                else
                {
                    bufsize.Enabled = false;
                    bufsize.Value = 0;
                }
            }
            else if (XAudioDisable.Checked == false)
            {
                OutputWAV.Enabled = true;
                VMSEmu.Visible = false;
                SPFSecondaryBut.Visible = true;
                BufferText.Text = "Set a buffer length for the driver, from 1 to 100 (             ):";
                SincInter.Checked = true;
                SincInter.Enabled = false;
                SincInter.Text = "Enable sinc interpolation. (Already applied by XAudio itself, it doesn't cause CPU overhead.)";
                bufsize.Minimum = 1;
                bufsize.Maximum = 100;
                bufsize.Enabled = true;
                bufsize.Value = 15;
            }
        }

        private void OutputWAV_CheckedChanged(object sender, EventArgs e)
        {
            if (OutputWAV.Checked == true)
            {
                XAudioDisable.Enabled = false;
                XAudioDisable.Checked = false;
                Label5.Enabled = false;
                bufsize.Enabled = false;
                MaxCPU.Enabled = false;
                BufferText.Enabled = false;
                bufsize.Enabled = false;
                bufsize.Minimum = 0;
                bufsize.Value = 0;
                MaxCPU.Value = 0;
            }
            else if (OutputWAV.Checked == false)
            {
                XAudioDisable.Enabled = true;
                Label5.Enabled = true;
                bufsize.Enabled = true;
                MaxCPU.Enabled = true;
                BufferText.Enabled = true;
                bufsize.Enabled = true;
                bufsize.Minimum = 1;
                bufsize.Value = 15;
                MaxCPU.Value = 75;
            }
        }

        private void VMSEmu_CheckedChanged(object sender, EventArgs e)
        {
            if (VMSEmu.Checked == true)
            {
                bufsize.Enabled = true;
            }
            else
            {
                bufsize.Enabled = false;
            } 
        }

        // Watchdog features in the configurator and hotkeys

        void LoadSoundfont(int whichone)
        {
            Watchdog.SetValue("currentsflist", whichone, RegistryValueKind.DWord);
            Watchdog.SetValue("rel" + whichone.ToString(), "1", RegistryValueKind.DWord);
        }

        private void ReloadList1_Click(object sender, EventArgs e)
        {
            LoadSoundfont(1);
        }

        private void ReloadList2_Click(object sender, EventArgs e)
        {
            LoadSoundfont(2);
        }

        private void ReloadList3_Click(object sender, EventArgs e)
        {
            LoadSoundfont(3);
        }

        private void ReloadList4_Click(object sender, EventArgs e)
        {
            LoadSoundfont(4);
        }

        private void ReloadList5_Click(object sender, EventArgs e)
        {
            LoadSoundfont(5);
        }

        private void ReloadList6_Click(object sender, EventArgs e)
        {
            LoadSoundfont(6);
        }

        private void ReloadList7_Click(object sender, EventArgs e)
        {
            LoadSoundfont(7);
        }

        private void ReloadList8_Click(object sender, EventArgs e)
        {
            LoadSoundfont(8);
        }

        private void ReloadList9_Click(object sender, EventArgs e)
        {
            LoadSoundfont(9);
        }

        private void ReloadList10_Click(object sender, EventArgs e)
        {
            LoadSoundfont(10);
        }

        private void ReloadList11_Click(object sender, EventArgs e)
        {
            LoadSoundfont(11);
        }

        private void ReloadList12_Click(object sender, EventArgs e)
        {
            LoadSoundfont(12);
        }

        private void ReloadList13_Click(object sender, EventArgs e)
        {
            LoadSoundfont(13);
        }

        private void ReloadList14_Click(object sender, EventArgs e)
        {
            LoadSoundfont(14);
        }

        private void ReloadList15_Click(object sender, EventArgs e)
        {
            LoadSoundfont(15);
        }

        private void ReloadList16_Click(object sender, EventArgs e)
        {
            LoadSoundfont(16);
        }

        private void sendAMIDIResetEventToAllTheChannelsStrip_Click(object sender, EventArgs e)
        {
            Watchdog.SetValue("resetchannels", 1, RegistryValueKind.DWord);
        }

        private void hLSEnabledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SynthSettings.SetValue("allhotkeys", "1", RegistryValueKind.DWord);
            hLSEnabledToolStripMenuItem.Checked = true;
            hLSDisabledToolStripMenuItem.Checked = false;
            hLSEnabledToolStripMenuItem.Enabled = false;
            hLSDisabledToolStripMenuItem.Enabled = true;
        }

        private void hLSDisabledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SynthSettings.SetValue("allhotkeys", "0", RegistryValueKind.DWord);
            hLSEnabledToolStripMenuItem.Checked = false;
            hLSDisabledToolStripMenuItem.Checked = true;
            hLSEnabledToolStripMenuItem.Enabled = true;
            hLSDisabledToolStripMenuItem.Enabled = false;
        }
    }
}