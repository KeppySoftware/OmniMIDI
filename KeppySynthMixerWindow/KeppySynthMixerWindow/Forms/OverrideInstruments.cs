using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeppySynthMixerWindow
{
    public partial class OverrideInstruments : Form
    {
        public static RegistryKey ChanOverride = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\ChanOverride", true);

        public OverrideInstruments()
        {
            InitializeComponent();
        }

        private void OverrideInstruments_Load(object sender, EventArgs e)
        {
            // Is override on or off
            if ((Int32)ChanOverride.GetValue("overrideinstruments", 0) == 1)
            {
                EnableOrNot.Checked = true;
                ChannelsBox.Enabled = true;
            }

            // Banks
            BC1.Value = (Int32)ChanOverride.GetValue("bc1", 0);
            BC2.Value = (Int32)ChanOverride.GetValue("bc2", 0);
            BC3.Value = (Int32)ChanOverride.GetValue("bc3", 0);
            BC4.Value = (Int32)ChanOverride.GetValue("bc4", 0);
            BC5.Value = (Int32)ChanOverride.GetValue("bc5", 0);
            BC6.Value = (Int32)ChanOverride.GetValue("bc6", 0);
            BC7.Value = (Int32)ChanOverride.GetValue("bc7", 0);
            BC8.Value = (Int32)ChanOverride.GetValue("bc8", 0);
            BC9.Value = (Int32)ChanOverride.GetValue("bc9", 0);
            BCD.Value = (Int32)ChanOverride.GetValue("bcd", 0);
            BC11.Value = (Int32)ChanOverride.GetValue("bc11", 0);
            BC12.Value = (Int32)ChanOverride.GetValue("bc12", 0);
            BC13.Value = (Int32)ChanOverride.GetValue("bc13", 0);
            BC14.Value = (Int32)ChanOverride.GetValue("bc14", 0);
            BC15.Value = (Int32)ChanOverride.GetValue("bc15", 0);
            BC16.Value = (Int32)ChanOverride.GetValue("bc16", 0);

            // Presets
            PC1.Value = (Int32)ChanOverride.GetValue("pc1", 0);
            PC2.Value = (Int32)ChanOverride.GetValue("pc2", 0);
            PC3.Value = (Int32)ChanOverride.GetValue("pc3", 0);
            PC4.Value = (Int32)ChanOverride.GetValue("pc4", 0);
            PC5.Value = (Int32)ChanOverride.GetValue("pc5", 0);
            PC6.Value = (Int32)ChanOverride.GetValue("pc6", 0);
            PC7.Value = (Int32)ChanOverride.GetValue("pc7", 0);
            PC8.Value = (Int32)ChanOverride.GetValue("pc8", 0);
            PC9.Value = (Int32)ChanOverride.GetValue("pc9", 0);
            PCD.Value = (Int32)ChanOverride.GetValue("pcd", 0);
            PC11.Value = (Int32)ChanOverride.GetValue("pc11", 0);
            PC12.Value = (Int32)ChanOverride.GetValue("pc12", 0);
            PC13.Value = (Int32)ChanOverride.GetValue("pc13", 0);
            PC14.Value = (Int32)ChanOverride.GetValue("pc14", 0);
            PC15.Value = (Int32)ChanOverride.GetValue("pc15", 0);
            PC16.Value = (Int32)ChanOverride.GetValue("pc16", 0);
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            // Set override on or off
            if (EnableOrNot.Checked == true)
                ChanOverride.SetValue("overrideinstruments", 1, RegistryValueKind.DWord);
           else
                ChanOverride.SetValue("overrideinstruments", 0, RegistryValueKind.DWord);

            // Banks
            ChanOverride.SetValue("bc1", BC1.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("bc2", BC2.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("bc3", BC3.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("bc4", BC4.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("bc5", BC5.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("bc6", BC6.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("bc7", BC7.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("bc8", BC8.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("bc9", BC9.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("bcd", BCD.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("bc11", BC11.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("bc12", BC12.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("bc13", BC13.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("bc14", BC14.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("bc15", BC15.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("bc16", BC16.Value, RegistryValueKind.DWord);

            // Presets
            ChanOverride.SetValue("pc1", PC1.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("pc2", PC2.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("pc3", PC3.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("pc4", PC4.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("pc5", PC5.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("pc6", PC6.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("pc7", PC7.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("pc8", PC8.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("pc9", PC9.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("pcd", PCD.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("pc11", PC11.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("pc12", PC12.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("pc13", PC13.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("pc14", PC14.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("pc15", PC15.Value, RegistryValueKind.DWord);
            ChanOverride.SetValue("pc16", PC16.Value, RegistryValueKind.DWord);
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            CheckMT32.Enabled = false;
            Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var helpFile = Path.Combine(Path.GetTempPath(), "help.txt");
            File.WriteAllText(helpFile, Properties.Resources.gmlist);
            Process.Start(helpFile);
        }

        private void ImportConfig_Click(object sender, EventArgs e)
        {
            String PresetTitle = "";
            try
            {
                if (ImportDialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (string x in File.ReadLines(ImportDialog.FileName, Encoding.UTF8))
                    {
                        try
                        {
                            // Title
                            if (SettingName(x) == "ConfName") PresetTitle = SettingValue(x);

                            // Banks
                            else if (SettingName(x) == "BC1") BC1.Value = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "BC2") BC2.Value = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "BC3") BC3.Value = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "BC4") BC4.Value = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "BC5") BC5.Value = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "BC6") BC6.Value = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "BC7") BC7.Value = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "BC8") BC8.Value = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "BC9") BC9.Value = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "BCD") BCD.Value = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "BC11") BC11.Value = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "BC12") BC12.Value = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "BC13") BC13.Value = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "BC14") BC14.Value = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "BC15") BC15.Value = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "BC16") BC16.Value = Convert.ToInt32(SettingValue(x));

                            // Presets
                            else if (SettingName(x) == "PC1") PC1.Value = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "PC2") PC2.Value = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "PC3") PC3.Value = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "PC4") PC4.Value = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "PC5") PC5.Value = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "PC6") PC6.Value = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "PC7") PC7.Value = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "PC8") PC8.Value = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "PC9") PC9.Value = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "PCD") PCD.Value = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "PC11") PC11.Value = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "PC12") PC12.Value = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "PC13") PC13.Value = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "PC14") PC14.Value = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "PC15") PC15.Value = Convert.ToInt32(SettingValue(x));
                            else if (SettingName(x) == "PC16") PC16.Value = Convert.ToInt32(SettingValue(x));
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(
                                "Fatal error",
                                String.Format("Invalid preset!\n\nException:\n{0}", ex.ToString()),
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                                );

                            return;
                        }
                    }

                    MessageBox.Show(
                        String.Format("The setting file \"{0}\" has been applied.", PresetTitle),
                        "Keppy's Synthesizer - Import settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // Something bad happened hehe
                MessageBox.Show(
                    "Fatal error",
                    String.Format("Fatal error during the execution of the program.\n\nPress OK to quit.\n\nException:\n{0}", ex.ToString()),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );

                Application.Exit();
            }
        }

        private void ExportConfig_Click(object sender, EventArgs e)
        {
            try
            {
                if (ExportDialog.ShowDialog() == DialogResult.OK)
                {
                    StringBuilder SettingsToText = new StringBuilder();

                    SettingsToText.AppendLine(String.Format("ConfName = {0}", Path.GetFileNameWithoutExtension(ExportDialog.FileName)));
                    SettingsToText.AppendLine();
                    SettingsToText.AppendLine("// Banks");
                    SettingsToText.AppendLine(String.Format("BC1 = {0}", BC1.Value));
                    SettingsToText.AppendLine(String.Format("BC2 = {0}", BC2.Value));
                    SettingsToText.AppendLine(String.Format("BC3 = {0}", BC3.Value));
                    SettingsToText.AppendLine(String.Format("BC4 = {0}", BC4.Value));
                    SettingsToText.AppendLine(String.Format("BC5 = {0}", BC5.Value));
                    SettingsToText.AppendLine(String.Format("BC6 = {0}", BC6.Value));
                    SettingsToText.AppendLine(String.Format("BC7 = {0}", BC7.Value));
                    SettingsToText.AppendLine(String.Format("BC8 = {0}", BC8.Value));
                    SettingsToText.AppendLine(String.Format("BC9 = {0}", BC9.Value));
                    SettingsToText.AppendLine(String.Format("BCD = {0}", BCD.Value));
                    SettingsToText.AppendLine(String.Format("BC11 = {0}", BC11.Value));
                    SettingsToText.AppendLine(String.Format("BC12 = {0}", BC12.Value));
                    SettingsToText.AppendLine(String.Format("BC13 = {0}", BC13.Value));
                    SettingsToText.AppendLine(String.Format("BC14 = {0}", BC14.Value));
                    SettingsToText.AppendLine(String.Format("BC15 = {0}", BC15.Value));
                    SettingsToText.AppendLine(String.Format("BC16 = {0}", BC16.Value));
                    SettingsToText.AppendLine();
                    SettingsToText.AppendLine("// Presets");
                    SettingsToText.AppendLine(String.Format("PC1 = {0}", PC1.Value));
                    SettingsToText.AppendLine(String.Format("PC2 = {0}", PC2.Value));
                    SettingsToText.AppendLine(String.Format("PC3 = {0}", PC3.Value));
                    SettingsToText.AppendLine(String.Format("PC4 = {0}", PC4.Value));
                    SettingsToText.AppendLine(String.Format("PC5 = {0}", PC5.Value));
                    SettingsToText.AppendLine(String.Format("PC6 = {0}", PC6.Value));
                    SettingsToText.AppendLine(String.Format("PC7 = {0}", PC7.Value));
                    SettingsToText.AppendLine(String.Format("PC8 = {0}", PC8.Value));
                    SettingsToText.AppendLine(String.Format("PC9 = {0}", PC9.Value));
                    SettingsToText.AppendLine(String.Format("PCD = {0}", PCD.Value));
                    SettingsToText.AppendLine(String.Format("PC11 = {0}", PC11.Value));
                    SettingsToText.AppendLine(String.Format("PC12 = {0}", PC12.Value));
                    SettingsToText.AppendLine(String.Format("PC13 = {0}", PC13.Value));
                    SettingsToText.AppendLine(String.Format("PC14 = {0}", PC14.Value));
                    SettingsToText.AppendLine(String.Format("PC15 = {0}", PC15.Value));
                    SettingsToText.AppendLine(String.Format("PC16 = {0}", PC16.Value));

                    File.WriteAllText(ExportDialog.FileName, SettingsToText.ToString());

                    MessageBox.Show(
                        String.Format("The setting file \"{0}\" has been saved to:\n\n{1}",
                        Path.GetFileNameWithoutExtension(ExportDialog.FileName),
                        ExportDialog.FileName),
                        "Keppy's Synthesizer - Export settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // Something bad happened hehe
                MessageBox.Show(
                    "Fatal error", 
                    String.Format("Fatal error during the execution of the program.\n\nPress OK to quit.\n\nException:\n{0}", ex.ToString()),
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                    );

                Application.Exit();
            }
        }

        private void SetBCAll_ValueChanged(object sender, EventArgs e)
        {
            BC1.Value = SetBCAll.Value;
            BC2.Value = SetBCAll.Value;
            BC3.Value = SetBCAll.Value;
            BC4.Value = SetBCAll.Value;
            BC5.Value = SetBCAll.Value;
            BC6.Value = SetBCAll.Value;
            BC7.Value = SetBCAll.Value;
            BC8.Value = SetBCAll.Value;
            BC9.Value = SetBCAll.Value;
            BCD.Value = SetBCAll.Value;
            BC11.Value = SetBCAll.Value;
            BC12.Value = SetBCAll.Value;
            BC13.Value = SetBCAll.Value;
            BC14.Value = SetBCAll.Value;
            BC15.Value = SetBCAll.Value;
            BC16.Value = SetBCAll.Value;
        }

        private void SetPCAll_ValueChanged(object sender, EventArgs e)
        {
            PC1.Value = SetPCAll.Value;
            PC2.Value = SetPCAll.Value;
            PC3.Value = SetPCAll.Value;
            PC4.Value = SetPCAll.Value;
            PC5.Value = SetPCAll.Value;
            PC6.Value = SetPCAll.Value;
            PC7.Value = SetPCAll.Value;
            PC8.Value = SetPCAll.Value;
            PC9.Value = SetPCAll.Value;
            PCD.Value = SetPCAll.Value;
            PC11.Value = SetPCAll.Value;
            PC12.Value = SetPCAll.Value;
            PC13.Value = SetPCAll.Value;
            PC14.Value = SetPCAll.Value;
            PC15.Value = SetPCAll.Value;
            PC16.Value = SetPCAll.Value;
        }

        private void CheckMT32_Tick(object sender, EventArgs e)
        {
            if ((Int32)KeppySynthMixerWindow.Settings.GetValue("MT32Mode", 0) == 1)
            {
                EnableOrNot.Enabled = false;
                ControlsBox.Enabled = false;
                ChannelsBox.Enabled = false;
                SaveBtn.Enabled = false;
                MT32IsEnabled.Visible = true;
            }
            else
            {
                EnableOrNot.Enabled = true;
                SaveBtn.Enabled = true;
                MT32IsEnabled.Visible = false;

                if (EnableOrNot.Checked == true)
                {
                    ImportConfig.Enabled = true;
                    ExportConfig.Enabled = true;
                    SetPCAll.Enabled = true;
                    SetBCAll.Enabled = true;
                    ChannelsBox.Enabled = true;
                }
                else
                {
                    ImportConfig.Enabled = false;
                    ExportConfig.Enabled = false;
                    SetPCAll.Enabled = false;
                    SetBCAll.Enabled = false;
                    ChannelsBox.Enabled = false;
                }
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
    }
}
