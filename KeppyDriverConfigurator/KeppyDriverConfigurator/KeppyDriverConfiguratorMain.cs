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

namespace KeppyDriverConfigurator
{
    public partial class KeppyDriverConfiguratorMain : Form
    {
        public string List1PathOld { get; set; }
        public string List2PathOld { get; set; }
        public string List3PathOld { get; set; }
        public string List4PathOld { get; set; }
        public string List5PathOld { get; set; }
        public string List6PathOld { get; set; }
        public string List7PathOld { get; set; }
        public string List8PathOld { get; set; }
        public string List1Path { get; set; }
        public string List2Path { get; set; }
        public string List3Path { get; set; }
        public string List4Path { get; set; }
        public string List5Path { get; set; }
        public string List6Path { get; set; }
        public string List7Path { get; set; }
        public string List8Path { get; set; }
        public int openadvanced { get; set; }

        public KeppyDriverConfiguratorMain(String[] args)
        {
            foreach (String s in args)
            {
                switch (s.Substring(0, 3).ToUpper())
                {
                    case "/AS":
                        UserProfileMigration();
                        Environment.Exit(0);
                        break;
                    case "/AT":
                        openadvanced = 1;
                        break;
                    default:
                        // do other stuff...
                        break;
                }
            }
            InitializeComponent();
        }

        static bool IsWindows10()
        {
            var reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
            string productName = (string)reg.GetValue("ProductName");
            return productName.StartsWith("Windows 10");
        }

        static bool IsWindowsXP()
        {
            var reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
            string productName = (string)reg.GetValue("ProductName");
            return productName.StartsWith("Windows XP");
        }

        // Just stuff to reduce code's length
        private void SFZCompliant()
        {
            MessageBox.Show("This driver is \"SFZ format 2.0\" compliant.", "SFZ format support", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ReinitializeList(Exception ex, ListBox selectedlist, String selectedlistpath)
        {
            try
            {
                MessageBox.Show("There was an error while trying to save the soundfont list!\n\n.NET error:\n" + ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                selectedlist.Items.Clear();
                using (StreamReader r = new StreamReader(selectedlistpath))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        selectedlist.Items.Add(line); // The program is copying the entire text file to the List I's listbox because it wasn't able to save the soundfont list.
                    }
                }
            }
            catch
            {
                MessageBox.Show("Fatal error during the execution of this program!\n\nPress OK to quit.", "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                Application.Exit();
            }
        }

        private void ImportExternalList(String SelectedExternalList, ListBox DestinationList)
        {
            try
            {
                using (StreamReader r = new StreamReader(SelectedExternalList))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        DestinationList.Items.Add(line); // Read the external list and add the items to the selected list
                    }
                }
            }
            catch
            {
                MessageBox.Show("Error during the import process of the list!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void ExportListToFile(String FinalPathList, ListBox SelectedList)
        {
            System.IO.StreamWriter SaveFile = new System.IO.StreamWriter(FinalPathList);
            foreach (var item in SelectedList.Items)
            {
                SaveFile.WriteLine(item.ToString());
            }
            SaveFile.Close();
            MessageBox.Show("Soundfont list exported succesfully to \"" + Path.GetDirectoryName(FinalPathList) + "\\\"", "Soundfont list exported!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SaveList(String SelectedList, ListBox OriginalList)
        {
            using (StreamWriter sw = new StreamWriter(SelectedList))
            {
                foreach (var item in OriginalList.Items)
                {
                    sw.WriteLine(item.ToString());
                }
            }
        }

        private void AddSoundfont(String SelectedList, ListBox OriginalList)
        {
            try
            {
                SoundfontImport.FileName = "";
                if (SoundfontImport.ShowDialog() == DialogResult.OK)
                {
                    foreach (string str in SoundfontImport.FileNames)
                    {
                        if (Path.GetExtension(str) == ".sf2" | Path.GetExtension(str) == ".SF2" | Path.GetExtension(str) == ".sfpack" | Path.GetExtension(str) == ".SFPACK")
                        {
                            OriginalList.Items.Add(str);
                        }
                        else if (Path.GetExtension(str) == ".sfz" | Path.GetExtension(str) == ".SFZ")
                        {
                            using (var form = new BankNPresetSel(Path.GetFileName(str), 0))
                            {
                                var result = form.ShowDialog();
                                if (result == DialogResult.OK)
                                {
                                    string bank = form.BankValueReturn;
                                    string preset = form.PresetValueReturn;
                                    OriginalList.Items.Add("p" + bank + "," + preset + "=0,0|" + str);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show(Path.GetFileName(str) + " is not a valid soundfont file!", "Error while adding soundfont", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    SaveList(SelectedList, OriginalList);
                }
            }
            catch (Exception ex)
            {
                ReinitializeList(ex, OriginalList, SelectedList);
            }
        }

        private void AddSoundfontDragNDrop(String SelectedList, ListBox OriginalList, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            int i;
            for (i = 0; i < s.Length; i++)
            {
                if (Path.GetExtension(s[i]) == ".sf2" | Path.GetExtension(s[i]) == ".SF2" | Path.GetExtension(s[i]) == ".sf3" | Path.GetExtension(s[i]) == ".SF3" | Path.GetExtension(s[i]) == ".sfpack" | Path.GetExtension(s[i]) == ".SFPACK")
                {
                    OriginalList.Items.Add(s[i]);
                }
                else if (Path.GetExtension(s[i]) == ".sfz" | Path.GetExtension(s[i]) == ".SFZ")
                {
                    using (var form = new BankNPresetSel(Path.GetFileName(s[i]), 1))
                    {
                        var result = form.ShowDialog();
                        if (result == DialogResult.OK)
                        {
                            string bank = form.BankValueReturn;
                            string preset = form.PresetValueReturn;
                            Lis1.Items.Add("p" + bank + "," + preset + "=0,0|" + s[i]);
                            SaveList(SelectedList, OriginalList);
                        }
                    }
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

        private void RemoveSoundfont(String SelectedList, ListBox OriginalList)
        {
            try
            {
                for (int i = OriginalList.SelectedIndices.Count - 1; i >= 0; i--)
                {
                    OriginalList.Items.RemoveAt(OriginalList.SelectedIndices[i]);
                }
                SaveList(SelectedList, OriginalList);
            }
            catch (Exception ex)
            {
                ReinitializeList(ex, OriginalList, SelectedList);
            }
        }

        private void MoveUpSoundfont(String SelectedList, ListBox OriginalList)
        {
            try
            {
                object selected = OriginalList.SelectedItem;
                int indx = OriginalList.Items.IndexOf(selected);
                int totl = OriginalList.Items.Count;
                if (indx == 0)
                {
                    OriginalList.Items.Remove(selected);
                    OriginalList.Items.Insert(totl - 1, selected);
                    OriginalList.SetSelected(totl - 1, true);
                }
                else
                {
                    OriginalList.Items.Remove(selected);
                    OriginalList.Items.Insert(indx - 1, selected);
                    OriginalList.SetSelected(indx - 1, true);
                }
                SaveList(SelectedList, OriginalList);
            }
            catch (Exception ex)
            {
                ReinitializeList(ex, OriginalList, SelectedList);
            }
        }

        private void MoveDownSoundfont(String SelectedList, ListBox OriginalList)
        {
            try
            {
                object selected = OriginalList.SelectedItem;
                int indx = OriginalList.Items.IndexOf(selected);
                int totl = OriginalList.Items.Count;
                if (indx == totl - 1)
                {
                    OriginalList.Items.Remove(selected);
                    OriginalList.Items.Insert(0, selected);
                    OriginalList.SetSelected(0, true);
                }
                else
                {
                    OriginalList.Items.Remove(selected);
                    OriginalList.Items.Insert(indx + 1, selected);
                    OriginalList.SetSelected(indx + 1, true);
                }
                SaveList(SelectedList, OriginalList);
            }
            catch (Exception ex)
            {
                ReinitializeList(ex, OriginalList, SelectedList);
            }
        }

        private void CleanList(String SelectedList, ListBox OriginalList)
        {
            try
            {
                OriginalList.Items.Clear();
                File.Delete(SelectedList);
                var TempFile = File.Create(SelectedList);
                TempFile.Close();
            }
            catch (Exception ex)
            {
                ReinitializeList(ex, OriginalList, SelectedList);
            }
        }

        private void EnableSoundfont(String SelectedList, ListBox OriginalList)
        {
            try
            {
                if (OriginalList.SelectedIndex == -1)
                {
                    MessageBox.Show("Select a soundfont first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    string result = OriginalList.SelectedItem.ToString().Substring(0, 1);
                    if (result == "@")
                    {
                        string newvalue = OriginalList.SelectedItem.ToString().Remove(0, 1);
                        int index = OriginalList.Items.IndexOf(OriginalList.SelectedItem);
                        OriginalList.Items.RemoveAt(index);
                        OriginalList.Items.Insert(index, newvalue);
                    }
                    else
                    {
                        MessageBox.Show("The soundfont is already enabled!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    SaveList(SelectedList, OriginalList);
                }
            }
            catch (Exception ex)
            {
                ReinitializeList(ex, OriginalList, SelectedList);
            }
        }

        private void DisableSoundfont(String SelectedList, ListBox OriginalList)
        {
            try
            {
                if (OriginalList.SelectedIndex == -1)
                {
                    MessageBox.Show("Select a soundfont first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    string result = OriginalList.SelectedItem.ToString().Substring(0, 1);
                    if (result != "@")
                    {
                        string newvalue = "@" + OriginalList.SelectedItem.ToString();
                        int index = OriginalList.Items.IndexOf(OriginalList.SelectedItem);
                        OriginalList.Items.RemoveAt(index);
                        OriginalList.Items.Insert(index, newvalue);
                    }
                    else
                    {
                        MessageBox.Show("The soundfont is already disabled!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    SaveList(SelectedList, OriginalList);
                }
            }
            catch (Exception ex)
            {
                ReinitializeList(ex, OriginalList, SelectedList);
            }
        }

        private void UserProfileMigration()
        {
            try
            {
                string oldlocation = System.Environment.GetEnvironmentVariable("LOCALAPPDATA") + "\\Keppy's Driver\\";
                string newlocation = System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Keppy's Driver\\";
                if (IsWindowsXP() == false)
                {
                    Directory.Move(oldlocation, newlocation);
                }
            }
            catch
            {

            }
        }

        private void InitializeLists()
        {
            string soundfontnewlocation = System.Environment.GetEnvironmentVariable("USERPROFILE").ToString();

            // Initialize the eight list paths
            List1Path = soundfontnewlocation + "\\Keppy's Driver\\lists\\keppymidi.sflist";
            List2Path = soundfontnewlocation + "\\Keppy's Driver\\lists\\keppymidib.sflist";
            List3Path = soundfontnewlocation + "\\Keppy's Driver\\lists\\keppymidic.sflist";
            List4Path = soundfontnewlocation + "\\Keppy's Driver\\lists\\keppymidid.sflist";
            List5Path = soundfontnewlocation + "\\Keppy's Driver\\lists\\keppymidie.sflist";
            List6Path = soundfontnewlocation + "\\Keppy's Driver\\lists\\keppymidif.sflist";
            List7Path = soundfontnewlocation + "\\Keppy's Driver\\lists\\keppymidig.sflist";
            List8Path = soundfontnewlocation + "\\Keppy's Driver\\lists\\keppymidih.sflist";

            // ======= Read soundfont lists
            try
            {
                if (!System.IO.Directory.Exists(soundfontnewlocation + "\\Keppy's Driver\\lists\\"))
                {
                    System.IO.Directory.CreateDirectory(soundfontnewlocation + "\\Keppy's Driver\\lists\\");
                    File.Create(List1Path).Dispose();
                    File.Create(List2Path).Dispose();
                    File.Create(List3Path).Dispose();
                    File.Create(List4Path).Dispose();
                    File.Create(List5Path).Dispose();
                    File.Create(List6Path).Dispose();
                    File.Create(List7Path).Dispose();
                    File.Create(List8Path).Dispose();
                }
                else
                {
                    try
                    {
                        // == List 1
                        using (StreamReader r = new StreamReader(List1Path))
                        {
                            string line;
                            while ((line = r.ReadLine()) != null)
                            {
                                Lis1.Items.Add(line); // The program is copying the entire text file to the List I's listbox.
                            }
                        }
                        // == List 2
                        using (StreamReader r = new StreamReader(List2Path))
                        {
                            string line;
                            while ((line = r.ReadLine()) != null)
                            {
                                Lis2.Items.Add(line); // The program is copying the entire text file to the List II's listbox.
                            }
                        }
                        // == List 3
                        using (StreamReader r = new StreamReader(List3Path))
                        {
                            string line;
                            while ((line = r.ReadLine()) != null)
                            {
                                Lis3.Items.Add(line); // The program is copying the entire text file to the List III's listbox.
                            }
                        }
                        // == List 4
                        using (StreamReader r = new StreamReader(List4Path))
                        {
                            string line;
                            while ((line = r.ReadLine()) != null)
                            {
                                Lis4.Items.Add(line); // The program is copying the entire text file to the List IV's listbox.
                            }
                        }
                        // == List 5
                        using (StreamReader r = new StreamReader(List5Path))
                        {
                            string line;
                            while ((line = r.ReadLine()) != null)
                            {
                                Lis5.Items.Add(line); // The program is copying the entire text file to the List IV's listbox.
                            }
                        }
                        // == List 6
                        using (StreamReader r = new StreamReader(List6Path))
                        {
                            string line;
                            while ((line = r.ReadLine()) != null)
                            {
                                Lis6.Items.Add(line); // The program is copying the entire text file to the List IV's listbox.
                            }
                        }
                        // == List 7
                        using (StreamReader r = new StreamReader(List7Path))
                        {
                            string line;
                            while ((line = r.ReadLine()) != null)
                            {
                                Lis7.Items.Add(line); // The program is copying the entire text file to the List IV's listbox.
                            }
                        }
                        // == List 8
                        using (StreamReader r = new StreamReader(List8Path))
                        {
                            string line;
                            while ((line = r.ReadLine()) != null)
                            {
                                Lis8.Items.Add(line); // The program is copying the entire text file to the List IV's listbox.
                            }
                        }
                    }
                    catch
                    {
                        // If the program fails at reading the lists, it'll create them for you
                        if (File.Exists(List1Path) == false)
                        {
                            File.Create(List1Path).Dispose();
                        }
                        if (File.Exists(List2Path) == false)
                        {
                            File.Create(List2Path).Dispose();
                        }
                        if (File.Exists(List3Path) == false)
                        {
                            File.Create(List3Path).Dispose();
                        }
                        if (File.Exists(List4Path) == false)
                        {
                            File.Create(List4Path).Dispose();
                        }
                        if (File.Exists(List5Path) == false)
                        {
                            File.Create(List5Path).Dispose();
                        }
                        if (File.Exists(List6Path) == false)
                        {
                            File.Create(List6Path).Dispose();
                        }
                        if (File.Exists(List7Path) == false)
                        {
                            File.Create(List7Path).Dispose();
                        }
                        if (File.Exists(List8Path) == false)
                        {
                            File.Create(List8Path).Dispose();
                        }
                        MessageBox.Show("One of the soundfont lists were missing, so the configurator automatically restored them.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
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
                RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Driver\\Settings", true);
                // Normal settings
                Settings.SetValue("polyphony", PolyphonyLimit.Value.ToString(), RegistryValueKind.DWord);
                if (MaxCPU.Text == "Disabled")
                {
                    Settings.SetValue("cpu", "0", RegistryValueKind.DWord);
                }
                else
                {
                    Settings.SetValue("cpu", MaxCPU.Text, RegistryValueKind.DWord);
                }
                if (string.IsNullOrWhiteSpace(Frequency.Text))
                {
                    Frequency.Text = "48000";
                    Settings.SetValue("frequency", Frequency.Text, RegistryValueKind.DWord);
                }
                else
                {
                    Settings.SetValue("frequency", Frequency.Text, RegistryValueKind.DWord);
                }

                // Advanced settings
                Settings.SetValue("buflen", bufsize.Value.ToString(), RegistryValueKind.DWord);
                Settings.SetValue("tracks", TracksLimit.Value.ToString(), RegistryValueKind.DWord);

                // Let's not forget about the volume!
                int VolumeValue = 0;
                double x = VolTrackBar.Value / 100;
                VolumeValue = Convert.ToInt32(x);
                VolSimView.Text = VolumeValue.ToString("000\\%");
                VolIntView.Text = "Volume in 32-bit integer: " + VolTrackBar.Value.ToString("00000") + " (" + (VolTrackBar.Value / 100).ToString("000") + "%)";
                Settings.SetValue("volume", VolTrackBar.Value.ToString(), RegistryValueKind.DWord);
                
                // Checkbox stuff yay
                if (Preload.Checked == true)
                {
                    Settings.SetValue("preload", "1", RegistryValueKind.DWord);
                }
                else
                {
                    Settings.SetValue("preload", "0", RegistryValueKind.DWord);
                }
                if (DisableSFX.Checked == true)
                {
                    Settings.SetValue("nofx", "1", RegistryValueKind.DWord);
                }
                else
                {
                    Settings.SetValue("nofx", "0", RegistryValueKind.DWord);
                }
                if (VMSEmu.Checked == true)
                {
                    Settings.SetValue("vmsemu", "1", RegistryValueKind.DWord);
                }
                else
                {
                    Settings.SetValue("vmsemu", "0", RegistryValueKind.DWord);
                }
                if (NoteOffCheck.Checked == true)
                {
                    Settings.SetValue("noteoff", "1", RegistryValueKind.DWord);
                }
                else
                {
                    Settings.SetValue("noteoff", "0", RegistryValueKind.DWord);
                }
                if (SincInter.Checked == true)
                {
                    Settings.SetValue("sinc", "1", RegistryValueKind.DWord);
                }
                else
                {
                    Settings.SetValue("sinc", "0", RegistryValueKind.DWord);
                }
                if (SysResetIgnore.Checked == true)
                {
                    Settings.SetValue("sysresetignore", "1", RegistryValueKind.DWord);
                }
                else
                {
                    Settings.SetValue("sysresetignore", "0", RegistryValueKind.DWord);
                }
                if (OutputWAV.Checked == true)
                {
                    Settings.SetValue("encmode", "1", RegistryValueKind.DWord);
                }
                else
                {
                    Settings.SetValue("encmode", "0", RegistryValueKind.DWord);
                }
                if (XAudioDisable.Checked == true)
                {
                    Settings.SetValue("xaudiodisabled", "1", RegistryValueKind.DWord);
                }
                else
                {
                    Settings.SetValue("xaudiodisabled", "0", RegistryValueKind.DWord);
                }
            }
            catch
            {
                MessageBox.Show("Fatal error during the execution of this program!\n\nPress OK to quit.", "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                Application.Exit();
            }
        }

        // Here we go!
        private void KeppyDriverConfiguratorMain_Load(object sender, EventArgs e)
        {
            // MIDI out selector disabler
            if (IsWindows10() == true)
            {
                changeDefaultMIDIOutDeviceToolStripMenuItem.Visible = false;
                changeDefault64bitMIDIOutDeviceToolStripMenuItem.Visible = false;
                changeDefaultMIDIOutDeviceToolStripMenuItem1.Visible = false;
                getTheMIDIMapperForWindows10ToolStripMenuItem.Visible = true;
                toolStripSeparator1.Visible = false;
            }
            else
            {
                if (Environment.Is64BitOperatingSystem == false)
                {
                    changeDefaultMIDIOutDeviceToolStripMenuItem1.Visible = true;
                    changeDefaultMIDIOutDeviceToolStripMenuItem.Visible = false;
                    changeDefault64bitMIDIOutDeviceToolStripMenuItem.Visible = false;
                }
                else if (Environment.Is64BitOperatingSystem == true)
                {
                    changeDefaultMIDIOutDeviceToolStripMenuItem1.Visible = false;
                    changeDefaultMIDIOutDeviceToolStripMenuItem.Visible = true;
                    changeDefault64bitMIDIOutDeviceToolStripMenuItem.Visible = true;
                }
            }

            InitializeLists();

            // ======= Load settings from the registry
            try
            {
                // First, the most important settings
                RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Driver\\Settings", true);
                VolTrackBar.Value = Convert.ToInt32(Settings.GetValue("volume"));
                PolyphonyLimit.Value = Convert.ToInt32(Settings.GetValue("polyphony"));
                if (Settings.GetValue("cpu").ToString() == "0")
                {
                    MaxCPU.Text = "Disabled";
                }
                else
                {
                    MaxCPU.Text = Settings.GetValue("cpu").ToString();
                }
                if (Convert.ToInt32(Settings.GetValue("sfdisableconf")) == 0)
                {
                    enabledToolStripMenuItem.Checked = true;
                    enabledToolStripMenuItem.Enabled = false;
                    disabledToolStripMenuItem.Checked = false;
                    disabledToolStripMenuItem.Enabled = true;
                }
                else
                {
                    enabledToolStripMenuItem.Checked = false;
                    enabledToolStripMenuItem.Enabled = true;
                    disabledToolStripMenuItem.Checked = true;
                    disabledToolStripMenuItem.Enabled = false;
                }
                if (Convert.ToInt32(Settings.GetValue("volumehotkeys")) == 1)
                {
                    VolumeHotkeysCheck.Enabled = true;
                    enabledToolStripMenuItem1.Checked = true;
                    enabledToolStripMenuItem1.Enabled = false;
                    disabledToolStripMenuItem1.Checked = false;
                    disabledToolStripMenuItem1.Enabled = true;
                }
                else
                {
                    VolumeHotkeysCheck.Enabled = false;
                    enabledToolStripMenuItem1.Checked = false;
                    enabledToolStripMenuItem1.Enabled = true;
                    disabledToolStripMenuItem1.Checked = true;
                    disabledToolStripMenuItem1.Enabled = false;
                }
                if (Convert.ToInt32(Settings.GetValue("allhotkeys")) == 1)
                {
                    soundfontListChangeConfirmationDialogToolStripMenuItem.Enabled = true;
                    volumeHotkeysToolStripMenuItem.Enabled = true;
                    hLSEnabledToolStripMenuItem.Checked = true;
                    hLSDisabledToolStripMenuItem.Checked = false;
                    hLSEnabledToolStripMenuItem.Enabled = false;
                    hLSDisabledToolStripMenuItem.Enabled = true;
                }
                else
                {
                    soundfontListChangeConfirmationDialogToolStripMenuItem.Enabled = false;
                    volumeHotkeysToolStripMenuItem.Enabled = false;
                    hLSEnabledToolStripMenuItem.Checked = false;
                    hLSDisabledToolStripMenuItem.Checked = true;
                    hLSEnabledToolStripMenuItem.Enabled = true;
                    hLSDisabledToolStripMenuItem.Enabled = false;
                }
                if (Convert.ToInt32(Settings.GetValue("watchdog")) == 1)
                {
                    soundfontListChangeConfirmationDialogToolStripMenuItem.Enabled = true;
                    volumeHotkeysToolStripMenuItem.Enabled = true;
                    watchdogEnabledToolStripMenuItem.Checked = true;
                    watchdogDisabledToolStripMenuItem.Checked = false;
                    watchdogEnabledToolStripMenuItem.Enabled = false;
                    watchdogDisabledToolStripMenuItem.Enabled = true;
                }
                else
                {
                    soundfontListChangeConfirmationDialogToolStripMenuItem.Enabled = false;
                    watchdogEnabledToolStripMenuItem.Checked = false;
                    watchdogDisabledToolStripMenuItem.Checked = true;
                    watchdogEnabledToolStripMenuItem.Enabled = true;
                    watchdogDisabledToolStripMenuItem.Enabled = false;
                }
                Frequency.Text = Settings.GetValue("frequency").ToString();
                TracksLimit.Value = Convert.ToInt32(Settings.GetValue("tracks"));

                // Then the filthy checkboxes
                if (Convert.ToInt32(Settings.GetValue("preload")) == 1)
                {
                    Preload.Checked = true;
                }
                else
                {
                    Preload.Checked = false;
                }
                if (Convert.ToInt32(Settings.GetValue("nofx")) == 1)
                {
                    DisableSFX.Checked = true;
                }
                else
                {
                    DisableSFX.Checked = false;
                }
                if (Convert.ToInt32(Settings.GetValue("noteoff")) == 1)
                {
                    NoteOffCheck.Checked = true;
                }
                else
                {
                    NoteOffCheck.Checked = false;
                }
                if (Convert.ToInt32(Settings.GetValue("sinc")) == 1)
                {
                    SincInter.Checked = true;
                }
                else
                {
                    SincInter.Checked = false;
                }
                if (Convert.ToInt32(Settings.GetValue("sysresetignore")) == 1)
                {
                    SysResetIgnore.Checked = true;
                }
                else
                {
                    SysResetIgnore.Checked = false;
                }
                if (Convert.ToInt32(Settings.GetValue("encmode")) == 1)
                {
                    OutputWAV.Checked = true;
                }
                else
                {
                    OutputWAV.Checked = false;
                }
                if (Convert.ToInt32(Settings.GetValue("xaudiodisabled")) == 1)
                {
                    XAudioDisable.Checked = true;
                    VMSEmu.Visible = true;
                    SPFSecondaryBut.Visible = false;
                    if (Convert.ToInt32(Settings.GetValue("vmsemu")) == 1)
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
                }

                // LEL
                bufsize.Value = Convert.ToInt32(Settings.GetValue("buflen"));

                // And finally, the volume!
                int VolumeValue = Convert.ToInt32(Settings.GetValue("volume"));
                double x = VolumeValue / 100;
                VolSimView.Text = x.ToString("000\\%");
                VolIntView.Text = "Volume in 32-bit integer: " + VolumeValue.ToString("00000") + " (" + (VolumeValue / 100).ToString("000") + "%)";
                VolTrackBar.Value = VolumeValue;

                // Jakuberino
                if (openadvanced == 1)
                {
                    TabsForTheControls.SelectedIndex = 8;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not read settings from the registry!\n\nPress OK to quit.\n\n.NET error:\n" + ex.Message.ToString(), "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void VolTrackBar_Scroll(object sender, EventArgs e)
        {
            try
            {
                RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Driver\\Settings", true);
                int VolumeValue = 0;
                double x = VolTrackBar.Value / 100;
                VolumeValue = Convert.ToInt32(x);
                VolSimView.Text = VolumeValue.ToString("000\\%");
                VolIntView.Text = "Volume in 32-bit integer: " + VolTrackBar.Value.ToString("00000") + " (" + (VolTrackBar.Value / 100).ToString("000") + "%)";
                Settings.SetValue("volume", VolTrackBar.Value.ToString(), RegistryValueKind.DWord);
            }
            catch
            {

            }
        }

        // -------------------------
        // Soundfont lists functions

        private void AddSF1_Click(object sender, EventArgs e)
        {
            AddSoundfont(List1Path, Lis1);
        }

        private void AddSF2_Click(object sender, EventArgs e)
        {
            AddSoundfont(List2Path, Lis2);
        }

        private void AddSF3_Click(object sender, EventArgs e)
        {
            AddSoundfont(List3Path, Lis3);
        }

        private void AddSF4_Click(object sender, EventArgs e)
        {
            AddSoundfont(List4Path, Lis4);
        }

        private void AddSF5_Click(object sender, EventArgs e)
        {
            AddSoundfont(List5Path, Lis5);
        }

        private void AddSF6_Click(object sender, EventArgs e)
        {
            AddSoundfont(List6Path, Lis6);
        }

        private void AddSF7_Click(object sender, EventArgs e)
        {
            AddSoundfont(List7Path, Lis7);
        }

        private void AddSF8_Click(object sender, EventArgs e)
        {
            AddSoundfont(List8Path, Lis8);
        }

        private void RmvSF1_Click(object sender, EventArgs e)
        {
            RemoveSoundfont(List1Path, Lis1);
        }

        private void RmvSF2_Click(object sender, EventArgs e)
        {
            RemoveSoundfont(List2Path, Lis2);
        }

        private void RmvSF3_Click(object sender, EventArgs e)
        {
            RemoveSoundfont(List3Path, Lis3);
        }

        private void RmvSF4_Click(object sender, EventArgs e)
        {
            RemoveSoundfont(List4Path, Lis4);
        }

        private void RmvSF5_Click(object sender, EventArgs e)
        {
            RemoveSoundfont(List5Path, Lis5);
        }

        private void RmvSF6_Click(object sender, EventArgs e)
        {
            RemoveSoundfont(List6Path, Lis6);
        }

        private void RmvSF7_Click(object sender, EventArgs e)
        {
            RemoveSoundfont(List7Path, Lis7);
        }

        private void RmvSF8_Click(object sender, EventArgs e)
        {
            RemoveSoundfont(List8Path, Lis8);
        }

        private void Lis1_DragDrop(object sender, DragEventArgs e)
        {
            AddSoundfontDragNDrop(List1Path, Lis1, e);
        }

        private void Lis1_DragEnter(object sender, DragEventArgs e)
        {
            AddSoundfontDragNDropTriv(e);
        }

        private void Lis2_DragDrop(object sender, DragEventArgs e)
        {
            AddSoundfontDragNDrop(List2Path, Lis2, e);
        }

        private void Lis2_DragEnter(object sender, DragEventArgs e)
        {
            AddSoundfontDragNDropTriv(e);
        }

        private void Lis3_DragDrop(object sender, DragEventArgs e)
        {
            AddSoundfontDragNDrop(List3Path, Lis3, e);
        }

        private void Lis3_DragEnter(object sender, DragEventArgs e)
        {
            AddSoundfontDragNDropTriv(e);
        }

        private void Lis4_DragDrop(object sender, DragEventArgs e)
        {
            AddSoundfontDragNDrop(List4Path, Lis4, e);
        }

        private void Lis4_DragEnter(object sender, DragEventArgs e)
        {
            AddSoundfontDragNDropTriv(e);
        }

        private void Lis5_DragDrop(object sender, DragEventArgs e)
        {
            AddSoundfontDragNDrop(List5Path, Lis5, e);
        }

        private void Lis5_DragEnter(object sender, DragEventArgs e)
        {
            AddSoundfontDragNDropTriv(e);
        }

        private void Lis6_DragDrop(object sender, DragEventArgs e)
        {
            AddSoundfontDragNDrop(List6Path, Lis6, e);
        }

        private void Lis6_DragEnter(object sender, DragEventArgs e)
        {
            AddSoundfontDragNDropTriv(e);
        }

        private void Lis7_DragDrop(object sender, DragEventArgs e)
        {
            AddSoundfontDragNDrop(List7Path, Lis7, e);
        }

        private void Lis7_DragEnter(object sender, DragEventArgs e)
        {
            AddSoundfontDragNDropTriv(e);
        }

        private void Lis8_DragDrop(object sender, DragEventArgs e)
        {
            AddSoundfontDragNDrop(List8Path, Lis8, e);
        }

        private void Lis8_DragEnter(object sender, DragEventArgs e)
        {
            AddSoundfontDragNDropTriv(e);
        }

        private void MvU1_Click(object sender, EventArgs e)
        {
            MoveUpSoundfont(List1Path, Lis1);
        }

        private void MvU2_Click(object sender, EventArgs e)
        {
            MoveUpSoundfont(List2Path, Lis2);
        }

        private void MvU3_Click(object sender, EventArgs e)
        {
            MoveUpSoundfont(List3Path, Lis3);
        }

        private void MvU4_Click(object sender, EventArgs e)
        {
            MoveUpSoundfont(List4Path, Lis4);
        }

        private void MvU5_Click(object sender, EventArgs e)
        {
            MoveDownSoundfont(List5Path, Lis5);
        }

        private void MvU6_Click(object sender, EventArgs e)
        {
            MoveDownSoundfont(List6Path, Lis6);
        }

        private void MvU7_Click(object sender, EventArgs e)
        {
            MoveDownSoundfont(List7Path, Lis7);
        }

        private void MvU8_Click(object sender, EventArgs e)
        {
            MoveDownSoundfont(List8Path, Lis8);
        }

        private void MvD1_Click(object sender, EventArgs e)
        {
            MoveDownSoundfont(List1Path, Lis1);
        }

        private void MvD2_Click(object sender, EventArgs e)
        {
            MoveDownSoundfont(List2Path, Lis2);
        }

        private void MvD3_Click(object sender, EventArgs e)
        {
            MoveDownSoundfont(List3Path, Lis3);
        }

        private void MvD4_Click(object sender, EventArgs e)
        {
            MoveDownSoundfont(List4Path, Lis4);
        }

        private void MvD5_Click(object sender, EventArgs e)
        {
            MoveUpSoundfont(List5Path, Lis5);
        }

        private void MvD6_Click(object sender, EventArgs e)
        {
            MoveUpSoundfont(List6Path, Lis6);
        }

        private void MvD7_Click(object sender, EventArgs e)
        {
            MoveUpSoundfont(List7Path, Lis7);
        }

        private void MvD8_Click(object sender, EventArgs e)
        {
            MoveUpSoundfont(List8Path, Lis8);
        }

        private void CLi1_Click(object sender, EventArgs e)
        {
            CleanList(List1Path, Lis1);
        }

        private void CLi2_Click(object sender, EventArgs e)
        {
            CleanList(List2Path, Lis2);
        }

        private void CLi3_Click(object sender, EventArgs e)
        {
            CleanList(List3Path, Lis3);
        }

        private void CLi4_Click(object sender, EventArgs e)
        {
            CleanList(List4Path, Lis4);
        }

        private void CLi5_Click(object sender, EventArgs e)
        {
            CleanList(List5Path, Lis5);
        }

        private void CLi6_Click(object sender, EventArgs e)
        {
            CleanList(List6Path, Lis6);
        }

        private void CLi7_Click(object sender, EventArgs e)
        {
            CleanList(List7Path, Lis7);
        }

        private void CLi8_Click(object sender, EventArgs e)
        {
            CleanList(List8Path, Lis8);
        }

        private void IEL1_Click(object sender, EventArgs e)
        {
            ExternalListImport.FileName = "";
            if (ExternalListImport.ShowDialog() == DialogResult.OK)
            {
                ImportExternalList(ExternalListImport.FileName, Lis1);
                SaveList(List1Path, Lis1);
            }  
        }

        private void IEL2_Click(object sender, EventArgs e)
        {
            ExternalListImport.FileName = "";
            if (ExternalListImport.ShowDialog() == DialogResult.OK)
            {
                ImportExternalList(ExternalListImport.FileName, Lis2);
                SaveList(List2Path, Lis2);
            }
        }

        private void IEL3_Click(object sender, EventArgs e)
        {
            ExternalListImport.FileName = "";
            if (ExternalListImport.ShowDialog() == DialogResult.OK)
            {
                ImportExternalList(ExternalListImport.FileName, Lis3);
                SaveList(List3Path, Lis3);
            }
        }

        private void IEL4_Click(object sender, EventArgs e)
        {
            ExternalListImport.FileName = "";
            if (ExternalListImport.ShowDialog() == DialogResult.OK)
            {
                ImportExternalList(ExternalListImport.FileName, Lis4);
                SaveList(List4Path, Lis4);
            }
        }

        private void IEL6_Click(object sender, EventArgs e)
        {
            ExternalListImport.FileName = "";
            if (ExternalListImport.ShowDialog() == DialogResult.OK)
            {
                ImportExternalList(ExternalListImport.FileName, Lis6);
                SaveList(List4Path, Lis6);
            }
        }

        private void IEL7_Click(object sender, EventArgs e)
        {
            ExternalListImport.FileName = "";
            if (ExternalListImport.ShowDialog() == DialogResult.OK)
            {
                ImportExternalList(ExternalListImport.FileName, Lis7);
                SaveList(List4Path, Lis7);
            }
        }

        private void IEL8_Click(object sender, EventArgs e)
        {
            ExternalListImport.FileName = "";
            if (ExternalListImport.ShowDialog() == DialogResult.OK)
            {
                ImportExternalList(ExternalListImport.FileName, Lis8);
                SaveList(List4Path, Lis8);
            }
        }

        private void IEL5_Click(object sender, EventArgs e)
        {
            ExternalListImport.FileName = "";
            if (ExternalListImport.ShowDialog() == DialogResult.OK)
            {
                ImportExternalList(ExternalListImport.FileName, Lis5);
                SaveList(List4Path, Lis5);
            }
        }

        private void EL1_Click(object sender, EventArgs e)
        {
            ExportList.FileName = "";
            if (ExportList.ShowDialog() == DialogResult.OK)
            {
                ExportListToFile(ExportList.FileName, Lis1);
            }
        }

        private void EL2_Click(object sender, EventArgs e)
        {
            ExportList.FileName = "";
            if (ExportList.ShowDialog() == DialogResult.OK)
            {
                ExportListToFile(ExportList.FileName, Lis2);
            }
        }

        private void EL3_Click(object sender, EventArgs e)
        {
            ExportList.FileName = "";
            if (ExportList.ShowDialog() == DialogResult.OK)
            {
                ExportListToFile(ExportList.FileName, Lis3);
            }
        }

        private void EL4_Click(object sender, EventArgs e)
        {
            ExportList.FileName = "";
            if (ExportList.ShowDialog() == DialogResult.OK)
            {
                ExportListToFile(ExportList.FileName, Lis4);
            }
        }

        private void EL5_Click(object sender, EventArgs e)
        {
            ExportList.FileName = "";
            if (ExportList.ShowDialog() == DialogResult.OK)
            {
                ExportListToFile(ExportList.FileName, Lis5);
            }
        }

        private void EL6_Click(object sender, EventArgs e)
        {
            ExportList.FileName = "";
            if (ExportList.ShowDialog() == DialogResult.OK)
            {
                ExportListToFile(ExportList.FileName, Lis6);
            }
        }

        private void EL7_Click(object sender, EventArgs e)
        {
            ExportList.FileName = "";
            if (ExportList.ShowDialog() == DialogResult.OK)
            {
                ExportListToFile(ExportList.FileName, Lis7);
            }
        }

        private void EL8_Click(object sender, EventArgs e)
        {
            ExportList.FileName = "";
            if (ExportList.ShowDialog() == DialogResult.OK)
            {
                ExportListToFile(ExportList.FileName, Lis8);
            }
        }

        private void DisableSF1_Click(object sender, EventArgs e)
        {
            DisableSoundfont(List1Path, Lis1);
        }

        private void EnableSF1_Click(object sender, EventArgs e)
        {
            EnableSoundfont(List1Path, Lis1);
        }

        private void DisableSF2_Click(object sender, EventArgs e)
        {
            DisableSoundfont(List2Path, Lis2);
        }

        private void EnableSF2_Click(object sender, EventArgs e)
        {
            EnableSoundfont(List2Path, Lis2);
        }

        private void DisableSF3_Click(object sender, EventArgs e)
        {
            DisableSoundfont(List3Path, Lis3);
        }

        private void EnableSF3_Click(object sender, EventArgs e)
        {
            EnableSoundfont(List4Path, Lis4);
        }

        private void DisableSF4_Click(object sender, EventArgs e)
        {
            DisableSoundfont(List4Path, Lis4);
        }

        private void EnableSF4_Click(object sender, EventArgs e)
        {
            EnableSoundfont(List1Path, Lis1);
        }

        private void DisableSF5_Click(object sender, EventArgs e)
        {
            DisableSoundfont(List5Path, Lis5);
        }

        private void EnableSF5_Click(object sender, EventArgs e)
        {
            EnableSoundfont(List5Path, Lis5);
        }

        private void DisableSF6_Click(object sender, EventArgs e)
        {
            DisableSoundfont(List6Path, Lis6);
        }

        private void EnableSF6_Click(object sender, EventArgs e)
        {
            EnableSoundfont(List6Path, Lis6);
        }

        private void DisableSF7_Click(object sender, EventArgs e)
        {
            DisableSoundfont(List7Path, Lis7);
        }

        private void EnableSF7_Click(object sender, EventArgs e)
        {
            EnableSoundfont(List7Path, Lis7);
        }

        private void DisableSF8_Click(object sender, EventArgs e)
        {
            DisableSoundfont(List8Path, Lis8);
        }

        private void EnableSF8_Click(object sender, EventArgs e)
        {
            EnableSoundfont(List8Path, Lis8);
        }

        // End of the soundfont lists functions
        // ------------------------------------

        private void resetToDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Set some values...
            VolTrackBar.Value = 10000;
            PolyphonyLimit.Value = 512;
            MaxCPU.Text = "65";
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
            // Just save the settings
            SaveSettings();

            // Messagebox here
            MessageBox.Show("Settings saved to the registry!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void blackMIDIsPresetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Set some values...
            VolTrackBar.Value = 10000;
            PolyphonyLimit.Value = 1000;
            MaxCPU.Text = "75";
            Frequency.Text = "48000";
            bufsize.Value = 15;
            TracksLimit.Value = 16;
            Preload.Checked = true;
            NoteOffCheck.Checked = true;
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
            MaxCPU.Text = "80";
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

        private void VolumeHotkeysCheck_Tick(object sender, EventArgs e)
        {
            try
            {
                RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Driver\\Settings", true);
                VolTrackBar.Value = Convert.ToInt32(Settings.GetValue("volume"));
                double x = VolTrackBar.Value / 100;
                int VolumeValue = Convert.ToInt32(x);
                VolSimView.Text = VolumeValue.ToString("000\\%");
                VolIntView.Text = "Volume in 32-bit integer: " + VolTrackBar.Value.ToString("00000") + " (" + (VolTrackBar.Value / 100).ToString("000") + "%)";
            }
            catch
            {

            }
        }

        // Now, menustrip functions here

        private void openDebugWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KeppyDriverDebugWindow.GetForm.Show();
        }


        private void openTheMixerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KeppyDriverMixerWindow.GetForm.Show();
        }

        private void openTheBlacklistManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KeppyDriverBlacklistSystem frm = new KeppyDriverBlacklistSystem();
            frm.ShowDialog();
        }

        private void informationAboutTheDriverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KeppyDriverInformation frm = new KeppyDriverInformation();
            frm.ShowDialog();
        }

        private void changeDefaultMIDIOutDeviceToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\keppydrv\\midioutsetter32.exe");
        }

        private void changeDefault32bitMIDIOutDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\keppydrv\\midioutsetter32.exe");
        }

        private void changeDefault64bitMIDIOutDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\keppydrv\\midioutsetter64.exe");
        }

        private void openUpdaterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KeppyDriverUpdater frm = new KeppyDriverUpdater();
            frm.ShowDialog();
        }

        private void reportABugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to report a bug about Keppy's Driver?\n\nWARNING: Only use this function to report serious bugs, like memory leaks and security flaws.", "Report a bug...", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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

        private void getTheMIDIMapperForWindows10ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://plus.google.com/+RichardForhenson/posts/bkrqUfbV3xz");
        }

        private void changeDirectoryOfTheOutputToWAVModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KeppyDriverOutputWAVDir frm = new KeppyDriverOutputWAVDir();
            frm.ShowDialog();
        }

        private void changeDefaultSoundfontListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KeppyDriverDefaultSFList frm = new KeppyDriverDefaultSFList();
            frm.ShowDialog();
        }

        private void changeDefaultSoundfontListToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            KeppyDriverDefaultSFList frm = new KeppyDriverDefaultSFList();
            frm.ShowDialog();
        }

        private void changeTheMaximumSamplesPerFrameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KeppyDriverSamplePerFrameSetting frm = new KeppyDriverSamplePerFrameSetting();
            frm.ShowDialog();
        }

        private void SPFSecondaryBut_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            KeppyDriverSamplePerFrameSetting frm = new KeppyDriverSamplePerFrameSetting();
            frm.ShowDialog();
        }

        private void SFListConfirmationenabledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Driver\\Settings", true);
            Settings.SetValue("sfdisableconf", "0", RegistryValueKind.DWord);
            Settings.Close();
            enabledToolStripMenuItem.Checked = true;
            enabledToolStripMenuItem.Enabled = false;
            disabledToolStripMenuItem.Checked = false;
            disabledToolStripMenuItem.Enabled = true;
        }

        private void SFListConfirmationdisabledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Driver\\Settings", true);
            Settings.SetValue("sfdisableconf", "1", RegistryValueKind.DWord);
            Settings.Close();
            enabledToolStripMenuItem.Checked = false;
            enabledToolStripMenuItem.Enabled = true;
            disabledToolStripMenuItem.Checked = true;
            disabledToolStripMenuItem.Enabled = false;
        }

        private void enabledToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Driver\\Settings", true);
            Settings.SetValue("volumehotkeys", "1", RegistryValueKind.DWord);
            Settings.Close();
            VolumeHotkeysCheck.Enabled = true;
            enabledToolStripMenuItem1.Checked = true;
            enabledToolStripMenuItem1.Enabled = false;
            disabledToolStripMenuItem1.Checked = false;
            disabledToolStripMenuItem1.Enabled = true;
        }

        private void disabledToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Driver\\Settings", true);
            Settings.SetValue("volumehotkeys", "0", RegistryValueKind.DWord);
            Settings.Close();
            VolumeHotkeysCheck.Enabled = false;
            enabledToolStripMenuItem1.Checked = false;
            enabledToolStripMenuItem1.Enabled = true;
            disabledToolStripMenuItem1.Checked = true;
            disabledToolStripMenuItem1.Enabled = false;
        }


        private void hLSEnabledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Driver\\Settings", true);
            Settings.SetValue("allhotkeys", "1", RegistryValueKind.DWord);
            Settings.Close();
            soundfontListChangeConfirmationDialogToolStripMenuItem.Enabled = true;
            volumeHotkeysToolStripMenuItem.Enabled = true;
            hLSEnabledToolStripMenuItem.Checked = true;
            hLSDisabledToolStripMenuItem.Checked = false;
            hLSEnabledToolStripMenuItem.Enabled = false;
            hLSDisabledToolStripMenuItem.Enabled = true;
        }

        private void hLSDisabledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Driver\\Settings", true);
            Settings.SetValue("allhotkeys", "0", RegistryValueKind.DWord);
            Settings.Close();
            soundfontListChangeConfirmationDialogToolStripMenuItem.Enabled = false;
            volumeHotkeysToolStripMenuItem.Enabled = false;
            hLSEnabledToolStripMenuItem.Checked = false;
            hLSDisabledToolStripMenuItem.Checked = true;
            hLSEnabledToolStripMenuItem.Enabled = true;
            hLSDisabledToolStripMenuItem.Enabled = false;
        }


        private void watchdogEnabledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Driver\\Watchdog", true);
            Settings.SetValue("watchdog", "1", RegistryValueKind.DWord);
            Settings.Close();
            watchdogEnabledToolStripMenuItem.Checked = true;
            watchdogDisabledToolStripMenuItem.Checked = false;
            watchdogEnabledToolStripMenuItem.Enabled = false;
            watchdogDisabledToolStripMenuItem.Enabled = true;
        }

        private void watchdogDisabledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Driver\\Watchdog", true);
            Settings.SetValue("watchdog", "0", RegistryValueKind.DWord);
            Settings.Close();
            watchdogEnabledToolStripMenuItem.Checked = false;
            watchdogDisabledToolStripMenuItem.Checked = true;
            watchdogEnabledToolStripMenuItem.Enabled = true;
            watchdogDisabledToolStripMenuItem.Enabled = false;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
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
                "ALT+1: Load soundfont list 1\nALT+2: Load soundfont list 2\nALT+3: Load soundfont list 3\nALT+4: Load soundfont list 4\nALT+5: Load soundfont list 5\nALT+6: Load soundfont list 6\nALT+7: Load soundfont list 7\nALT+8: Load soundfont list 8\n\n" +
                "You can also reload lists that are already loaded.", "How can I change the soundfont list?", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        private void howCanIResetTheDriverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To reset the driver, press INS.\nThis will stop all the samples that are currently playing, and it'll also send a \"System Reset\" to all the MIDI channels.", "How can I reset the driver?", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        private void whatsTheBestSettingsForTheBufferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("For SoundBlaster-based audio cards, it's 10.\nFor Realtek audio cards, it's 15.\nFor VIA audio cards, it's 20.\nFor Conexant audio cards, it's 30.\nFor USB DACs, it's 30-35.\nFor all the AC'97 audio cards, it's 40.\n\nIt's possible to set it to 10 with really fast computers.", "What's the best settings for the buffer?", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        // Brand new output mode
        private void WhatIsOutput_Click(object sender, EventArgs e)
        {
            MessageBox.Show("If you check this option, the driver will create a WAV file on your desktop, called \"(programname).exe - Keppy's Driver Output File.wav\".\n\n" + 
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
                MaxCPU.Text = "Disabled";
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
                MaxCPU.Text = "75";
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
    }
}
