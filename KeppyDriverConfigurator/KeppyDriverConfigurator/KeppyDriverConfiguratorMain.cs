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
        public string List1Path { get; set; }
        public string List2Path { get; set; }
        public string List3Path { get; set; }
        public string List4Path { get; set; }

        public KeppyDriverConfiguratorMain()
        {
            InitializeComponent();
        }

        // Just stuff to reduce code's length
        private void ReinitializeList(Exception ex, ListBox selectedlist, String selectedlistpath)
        {
            try
            {
                MessageBox.Show("There was an error while trying to save the soundfont list!\n\n.NET error:\n" + ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                selectedlist.Items.Clear();
                using (StreamReader r = new StreamReader(List1Path))
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
                        if (Path.GetExtension(line) == ".sf2" | Path.GetExtension(line) == ".SF2" | Path.GetExtension(line) == ".sfpack" | Path.GetExtension(line) == ".SFPACK")
                        {
                            DestinationList.Items.Add(line); // Read the external list and add the items to the selected list
                        }
                        else if (Path.GetExtension(line) == ".sfz" | Path.GetExtension(line) == ".SFZ")
                        {
                            using (var form = new BankNPresetSel(Path.GetFileName(line)))
                            {
                                var result = form.ShowDialog();
                                if (result == DialogResult.OK)
                                {
                                    string bank = form.BankValueReturn;
                                    string preset = form.PresetValueReturn;
                                    DestinationList.Items.Add("p" + bank + "," + preset + "=0,0|" + line);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show(Path.GetFileName(line) + " is not a valid soundfont!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("This is not a valid soundfont list file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
                Settings.SetValue("frequency", Frequency.Text, RegistryValueKind.DWord);

                // Advanced settings
                Settings.SetValue("buflen", bufsize.Value.ToString(), RegistryValueKind.DWord);
                Settings.SetValue("tracks", TracksLimit.Value.ToString(), RegistryValueKind.DWord);

                // Let's not forget about the volume!
                int VolumeValue = 0;
                double x = VolTrackBar.Value / 100;
                VolumeValue = Convert.ToInt32(x);
                VolSimView.Text = VolumeValue.ToString("000");
                VolIntView.Text = "Volume in 32-bit integer: " + VolTrackBar.Value.ToString();
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
            // Initialize the four list paths
            List1Path = Environment.GetEnvironmentVariable("LocalAppData") + "\\Keppy's Driver\\lists\\keppymidi.sflist";
            List2Path = Environment.GetEnvironmentVariable("LocalAppData") + "\\Keppy's Driver\\lists\\keppymidib.sflist";
            List3Path = Environment.GetEnvironmentVariable("LocalAppData") + "\\Keppy's Driver\\lists\\keppymidic.sflist";
            List4Path = Environment.GetEnvironmentVariable("LocalAppData") + "\\Keppy's Driver\\lists\\keppymidid.sflist";

            // ======= Read soundfont lists
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
            }
            // If the program fails at reading the lists, it'll create them for you
            catch
            {
                File.Create(List1Path);
                File.Create(List2Path);
                File.Create(List3Path);
                File.Create(List4Path);
            }

            // ======= Load settings from the registry

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
            Frequency.Text = Settings.GetValue("frequency").ToString();
            bufsize.Value = Convert.ToInt32(Settings.GetValue("buflen"));
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

            // And finally, the volume!
            int VolumeValue = Convert.ToInt32(Settings.GetValue("volume"));
            double x = VolumeValue / 100;
            VolSimView.Text = x.ToString("000");
            VolIntView.Text = "Volume in 32-bit integer: " + VolumeValue.ToString();
            VolTrackBar.Value = VolumeValue;
        }

        private void VolTrackBar_Scroll(object sender, EventArgs e)
        {
            try
            {
                RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Driver\\Settings", true);
                int VolumeValue = 0;
                double x = VolTrackBar.Value / 100;
                VolumeValue = Convert.ToInt32(x);
                VolSimView.Text = VolumeValue.ToString("000");
                VolIntView.Text = "Volume in 32-bit integer: " + VolTrackBar.Value.ToString();
                Settings.SetValue("volume", VolTrackBar.Value.ToString(), RegistryValueKind.DWord);
            }
            catch
            {

            }
        }

        private void AddSF1_Click(object sender, EventArgs e)
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
                            Lis1.Items.Add(str);
                        }
                        else if (Path.GetExtension(str) == ".sfz" | Path.GetExtension(str) == ".SFZ")
                        {
                            using (var form = new BankNPresetSel(Path.GetFileName(str)))
                            {
                                var result = form.ShowDialog();
                                if (result == DialogResult.OK)
                                {
                                    string bank = form.BankValueReturn;
                                    string preset = form.PresetValueReturn;
                                    Lis1.Items.Add("p" + bank + "," + preset + "=0,0|" + str);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show(Path.GetFileName(str) + " is not a valid soundfont file!", "Error while adding soundfont", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    SaveList(List1Path, Lis1);
                }
            }
            catch (Exception ex)
            {
                ReinitializeList(ex, Lis1, List1Path);
            }
        }

        private void AddSF2_Click(object sender, EventArgs e)
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
                            Lis2.Items.Add(str);
                        }
                        else if (Path.GetExtension(str) == ".sfz" | Path.GetExtension(str) == ".SFZ")
                        {
                            using (var form = new BankNPresetSel(Path.GetFileName(str)))
                            {
                                var result = form.ShowDialog();
                                if (result == DialogResult.OK)
                                {
                                    string bank = form.BankValueReturn;
                                    string preset = form.PresetValueReturn;
                                    Lis2.Items.Add("p" + bank + "," + preset + "=0,0|" + str);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show(Path.GetFileName(str) + " is not a valid soundfont file!", "Error while adding soundfont", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    SaveList(List2Path, Lis2);
                }
            }
            catch (Exception ex)
            {
                ReinitializeList(ex, Lis2, List2Path);
            }
        }

        private void AddSF3_Click(object sender, EventArgs e)
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
                            Lis3.Items.Add(str);
                        }
                        else if (Path.GetExtension(str) == ".sfz" | Path.GetExtension(str) == ".SFZ")
                        {
                            using (var form = new BankNPresetSel(Path.GetFileName(str)))
                            {
                                var result = form.ShowDialog();
                                if (result == DialogResult.OK)
                                {
                                    string bank = form.BankValueReturn;
                                    string preset = form.PresetValueReturn;
                                    Lis3.Items.Add("p" + bank + "," + preset + "=0,0|" + str);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show(Path.GetFileName(str) + " is not a valid soundfont file!", "Error while adding soundfont", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    SaveList(List3Path, Lis3);
                }
            }
            catch (Exception ex)
            {
                ReinitializeList(ex, Lis3, List3Path);
            }
        }

        private void AddSF4_Click(object sender, EventArgs e)
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
                            Lis4.Items.Add(str);
                        }
                        else if (Path.GetExtension(str) == ".sfz" | Path.GetExtension(str) == ".SFZ")
                        {
                            using (var form = new BankNPresetSel(Path.GetFileName(str)))
                            {
                                var result = form.ShowDialog();
                                if (result == DialogResult.OK)
                                {
                                    string bank = form.BankValueReturn;
                                    string preset = form.PresetValueReturn;
                                    Lis4.Items.Add("p" + bank + "," + preset + "=0,0|" + str);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show(Path.GetFileName(str) + " is not a valid soundfont file!", "Error while adding soundfont", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    SaveList(List4Path, Lis4);
                }
            }
            catch (Exception ex)
            {
                ReinitializeList(ex, Lis4, List4Path);
            }
        }

        private void RmvSF1_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = Lis1.SelectedIndices.Count - 1; i >= 0; i--)
                {
                    Lis1.Items.RemoveAt(Lis1.SelectedIndices[i]);
                }
                SaveList(List1Path, Lis1);
            }
            catch (Exception ex)
            {
                ReinitializeList(ex, Lis1, List1Path);
            }
        }

        private void RmvSF2_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = Lis2.SelectedIndices.Count - 1; i >= 0; i--)
                {
                    Lis2.Items.RemoveAt(Lis2.SelectedIndices[i]);
                }
                SaveList(List2Path, Lis2);
            }
            catch (Exception ex)
            {
                ReinitializeList(ex, Lis2, List2Path);
            }
        }

        private void RmvSF3_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = Lis3.SelectedIndices.Count - 1; i >= 0; i--)
                {
                    Lis3.Items.RemoveAt(Lis3.SelectedIndices[i]);
                }
                SaveList(List3Path, Lis3);
            }
            catch (Exception ex)
            {
                ReinitializeList(ex, Lis3, List3Path);
            }
        }

        private void RmvSF4_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = Lis4.SelectedIndices.Count - 1; i >= 0; i--)
                {
                    Lis4.Items.RemoveAt(Lis4.SelectedIndices[i]);
                }
                SaveList(List4Path, Lis4);
            }
            catch (Exception ex)
            {
                ReinitializeList(ex, Lis4, List4Path);
            }
        }

        private void MvU1_Click(object sender, EventArgs e)
        {
            try
            {
                if (Lis1.SelectedIndex > 0)
                {
                    Lis1.Items.Insert(Lis1.SelectedIndex - 1, Lis1.Items[Lis1.SelectedIndex]);
                    Lis1.Items.RemoveAt(Lis1.SelectedIndex + 1);
                    Lis1.SelectedIndex = Lis1.SelectedIndex - 1;
                }
                SaveList(List1Path, Lis1);
            }
            catch (Exception ex)
            {
                ReinitializeList(ex, Lis1, List1Path);
            }
        }

        private void MvU2_Click(object sender, EventArgs e)
        {
            try
            {
                if (Lis2.SelectedIndex > 0)
                {
                    Lis1.Items.Insert(Lis2.SelectedIndex - 1, Lis1.Items[Lis2.SelectedIndex]);
                    Lis1.Items.RemoveAt(Lis2.SelectedIndex + 1);
                    Lis1.SelectedIndex = Lis2.SelectedIndex - 1;
                }
                SaveList(List2Path, Lis2);
            }
            catch (Exception ex)
            {
                ReinitializeList(ex, Lis2, List2Path);
            }
        }

        private void MvU3_Click(object sender, EventArgs e)
        {
            try
            {
                if (Lis3.SelectedIndex > 0)
                {
                    Lis3.Items.Insert(Lis3.SelectedIndex - 1, Lis1.Items[Lis3.SelectedIndex]);
                    Lis3.Items.RemoveAt(Lis3.SelectedIndex + 1);
                    Lis3.SelectedIndex = Lis3.SelectedIndex - 1;
                }
                SaveList(List3Path, Lis3);
            }
            catch (Exception ex)
            {
                ReinitializeList(ex, Lis3, List3Path);
            }
        }

        private void MvU4_Click(object sender, EventArgs e)
        {
            try
            {
                if (Lis4.SelectedIndex > 0)
                {
                    Lis4.Items.Insert(Lis4.SelectedIndex - 1, Lis4.Items[Lis4.SelectedIndex]);
                    Lis4.Items.RemoveAt(Lis4.SelectedIndex + 1);
                    Lis4.SelectedIndex = Lis4.SelectedIndex - 1;
                }
                SaveList(List4Path, Lis4);
            }
            catch (Exception ex)
            {
                ReinitializeList(ex, Lis4, List4Path);
            }
        }

        private void MvD1_Click(object sender, EventArgs e)
        {
            try
            {
                if (Lis1.SelectedIndex < Lis1.Items.Count - 1 & Lis1.SelectedIndex != -1)
                {
                    Lis1.Items.Insert(Lis1.SelectedIndex + 2, Lis1.Items[Lis1.SelectedIndex]);
                    Lis1.Items.RemoveAt(Lis1.SelectedIndex);
                    Lis1.SelectedIndex = Lis1.SelectedIndex + 1;
                }
                SaveList(List1Path, Lis1);
            }
            catch (Exception ex)
            {
                ReinitializeList(ex, Lis1, List1Path);
            }
        }

        private void MvD2_Click(object sender, EventArgs e)
        {
            try
            {
                if (Lis2.SelectedIndex < Lis2.Items.Count - 1 & Lis2.SelectedIndex != -1)
                {
                    Lis2.Items.Insert(Lis2.SelectedIndex + 2, Lis2.Items[Lis2.SelectedIndex]);
                    Lis2.Items.RemoveAt(Lis2.SelectedIndex);
                    Lis2.SelectedIndex = Lis2.SelectedIndex + 1;
                }
                SaveList(List2Path, Lis2);
            }
            catch (Exception ex)
            {
                ReinitializeList(ex, Lis2, List2Path);
            }
        }

        private void MvD3_Click(object sender, EventArgs e)
        {
            try
            {
                if (Lis3.SelectedIndex < Lis3.Items.Count - 1 & Lis3.SelectedIndex != -1)
                {
                    Lis3.Items.Insert(Lis3.SelectedIndex + 2, Lis3.Items[Lis3.SelectedIndex]);
                    Lis3.Items.RemoveAt(Lis3.SelectedIndex);
                    Lis3.SelectedIndex = Lis3.SelectedIndex + 1;
                }
                SaveList(List3Path, Lis3);
            }
            catch (Exception ex)
            {
                ReinitializeList(ex, Lis3, List3Path);
            }
        }

        private void MvD4_Click(object sender, EventArgs e)
        {
            try
            {
                if (Lis4.SelectedIndex < Lis4.Items.Count - 1 & Lis4.SelectedIndex != -1)
                {
                    Lis4.Items.Insert(Lis4.SelectedIndex + 2, Lis4.Items[Lis4.SelectedIndex]);
                    Lis4.Items.RemoveAt(Lis4.SelectedIndex);
                    Lis4.SelectedIndex = Lis4.SelectedIndex + 1;
                }
                SaveList(List4Path, Lis4);
            }
            catch (Exception ex)
            {
                ReinitializeList(ex, Lis4, List4Path);
            }
        }

        private void CLi1_Click(object sender, EventArgs e)
        {
            try
            {
                Lis1.Items.Clear();
                File.Delete(List1Path);
                var TempFile = File.Create(List1Path);
                TempFile.Close();
            }
            catch (Exception ex)
            {
                ReinitializeList(ex, Lis1, List1Path);
            }
        }

        private void CLi2_Click(object sender, EventArgs e)
        {
            try
            {
                Lis2.Items.Clear();
                File.Delete(List2Path);
                var TempFile = File.Create(List2Path);
                TempFile.Close();
            }
            catch (Exception ex)
            {
                ReinitializeList(ex, Lis2, List2Path);
            }
        }

        private void CLi3_Click(object sender, EventArgs e)
        {
            try
            {
                Lis3.Items.Clear();
                File.Delete(List3Path);
                var TempFile = File.Create(List3Path);
                TempFile.Close();
            }
            catch (Exception ex)
            {
                ReinitializeList(ex, Lis3, List3Path);
            }
        }

        private void CLi4_Click(object sender, EventArgs e)
        {
            try
            {
                Lis4.Items.Clear();
                File.Delete(List4Path);
                var TempFile = File.Create(List4Path);
                TempFile.Close();
            }
            catch (Exception ex)
            {
                ReinitializeList(ex, Lis4, List4Path);
            }
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

        private void ApplySettings_Click(object sender, EventArgs e)
        {
            // Just save the settings
            SaveSettings();

            // Messagebox here
            MessageBox.Show("Settings saved to the registry!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ResetSettings_Click(object sender, EventArgs e)
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

            // And then...
            SaveSettings();

            // Messagebox here
            MessageBox.Show("Settings restored to the default values!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BlackMIDIPres_Click(object sender, EventArgs e)
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

            // And then...
            SaveSettings();

            // Messagebox here
            MessageBox.Show("The Black MIDIs preset has been applied!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Now, menustrip functions here

        private void openDebugWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KeppyDriverDebugWindow frm = new KeppyDriverDebugWindow();
            frm.Show();
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

        private void changeDefaultMIDIOutDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KeppyDriverMIDIOutSelectorWin frm = new KeppyDriverMIDIOutSelectorWin();
            frm.ShowDialog();
        }

        private void openUpdaterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/KaleidonKep99/Keppy-s-Driver/releases");
        }

        private void reportABugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to report a bug", "Report a bug...", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Guide part
        private void howCanIChangeTheSoundfontListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To change the current soundfont list, press and hold CTRL, then click a number from 1 to 4.\n\n" +
                "CTRL+1: Load soundfont list 1\nCTRL+2: Load soundfont list 2\nCTRL+3: Load soundfont list 3\nCTRL+4: Load soundfont list 4\n\n" +
                "You can also reload lists that are already loaded.", "How can I change the soundfont list?", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        private void howCanIResetTheDriverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To reset the driver, press INS.\nThis will stop all the samples that are currently playing, and it'll also send a \"System Reset\" to all the MIDI channels.", "How can I reset the driver?", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        private void whatsTheBestSettingsForTheBufferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("For Realtek audio cards, it's 15.\nFor VIA audio cards, it's 20.\nFor USB DACs, it's 30-35.", "What's the best settings for the buffer?", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }
    }
}
