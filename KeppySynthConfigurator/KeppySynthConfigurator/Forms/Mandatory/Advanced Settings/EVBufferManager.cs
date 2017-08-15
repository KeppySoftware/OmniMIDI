using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace KeppySynthConfigurator
{
    public partial class EVBufferManager : Form
    {
        ulong installedMemory;
        string basetextwarning = "WARNING:\nLeave at least {0} of RAM available for the operating system.";

        public EVBufferManager()
        {
            InitializeComponent();
        }

        private void EVBufferManager_Load(object sender, EventArgs e)
        {
            try
            {
                MEMORYSTATUSEX memStatus = new MEMORYSTATUSEX();
                if (!Functions.GlobalMemoryStatusEx(memStatus)) MessageBox.Show("Unknown error.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                installedMemory = memStatus.ullTotalPhys;

                if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("evbuffbyram", "0")) == 1)
                    GetRAMSize.Checked = true;
                else
                    GetRAMSize.Checked = false;

                BytesVal.Maximum = installedMemory;

                PerformCheck();
                PerformRAMCheck();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void PerformCheck()
        {
            if (GetRAMSize.Checked == true)
            {
                KeppySynthConfiguratorMain.SynthSettings.SetValue("evbuffbyram", "1", Microsoft.Win32.RegistryValueKind.DWord);
                decimal evbuffratiotemp = Convert.ToDecimal(KeppySynthConfiguratorMain.SynthSettings.GetValue("evbuffratio", "1"));
                BytesVal.Enabled = false;
                RatioVal.Enabled = true;
                BytesVal.Value = installedMemory;
                if (evbuffratiotemp == 1) RatioVal.Value = 128;
                else RatioVal.Value = evbuffratiotemp;
            }
            else
            {
                KeppySynthConfiguratorMain.SynthSettings.SetValue("evbuffbyram", "0", Microsoft.Win32.RegistryValueKind.DWord);
                ulong evbuffsizetemp = Convert.ToUInt64(KeppySynthConfiguratorMain.SynthSettings.GetValue("evbuffsize", "16384"));
                BytesVal.Enabled = true;
                RatioVal.Enabled = false;
                if (evbuffsizetemp >= installedMemory) BytesVal.Value = 16384;
                else BytesVal.Value = evbuffsizetemp;
                RatioVal.Value = 1;
            }
        }

        private void PerformRAMCheck()
        {
            ulong check = (ulong)(BytesVal.Value / RatioVal.Value);

            if (check >= (installedMemory / 8))
            {
                if ((check >= installedMemory) || ((RatioVal.Value == 1) && (GetRAMSize.Checked == true)))
                {
                    WarningPanel.Visible = true;
                    WarningSign.Image = KeppySynthConfigurator.Properties.Resources.wir;
                    WarningLabel.Text = String.Format("ERROR:\nSorry, but no.");
                    ApplySettings.Enabled = false;
                }
                else
                {
                    WarningPanel.Visible = true;
                    WarningSign.Image = KeppySynthConfigurator.Properties.Resources.wi;
                    WarningLabel.Text = String.Format("WARNING:\nLeave at least {0} of RAM available for Windows.",
                                                      Functions.ReturnSoundFontSize(null, "evbuff", (long)installedMemory / 8));
                    ApplySettings.Enabled = true;
                }
            }
            else
            {
                WarningPanel.Visible = false;
                ApplySettings.Enabled = true;
            }
        }

        private void GetRAMSize_CheckedChanged(object sender, EventArgs e)
        {
            PerformCheck();
            PerformRAMCheck();

            Properties.Settings.Default.Save();
        }

        private void BytesVal_ValueChanged(object sender, EventArgs e)
        {
            PerformRAMCheck();
        }

        private void RatioVal_ValueChanged(object sender, EventArgs e)
        {
            PerformRAMCheck();
        }

        private void ResetSettings_Click(object sender, EventArgs e)
        {
            GetRAMSize.Checked = false;
            BytesVal.Value = 16384;
            RatioVal.Value = 1;
            KeppySynthConfiguratorMain.SynthSettings.SetValue("evbuffbyram", "0", Microsoft.Win32.RegistryValueKind.DWord);
            KeppySynthConfiguratorMain.SynthSettings.SetValue("evbuffsize", BytesVal.Value, Microsoft.Win32.RegistryValueKind.QWord);
            KeppySynthConfiguratorMain.SynthSettings.SetValue("evbuffratio", RatioVal.Value, Microsoft.Win32.RegistryValueKind.DWord);
        }

        private void ApplySettings_Click(object sender, EventArgs e)
        {
            KeppySynthConfiguratorMain.SynthSettings.SetValue("evbuffsize", BytesVal.Value, Microsoft.Win32.RegistryValueKind.QWord);
            KeppySynthConfiguratorMain.SynthSettings.SetValue("evbuffratio", RatioVal.Value, Microsoft.Win32.RegistryValueKind.DWord);
            Close();
        }

        private void WarningLabel_Click(object sender, EventArgs e)
        {
            MessageBox.Show(String.Format("Leave at least 1/8 of RAM ({0}) available to Windows, to avoid unexpected data loss and system crashes.\n\nRemember, you're responsible of anything that might happen after tampering with this setting.", 
                Functions.ReturnSoundFontSize(null, "evbuff", (long)installedMemory / 8)), 
                "What does this warning mean?", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class MEMORYSTATUSEX
    {
        public uint dwLength;
        public uint dwMemoryLoad;
        public ulong ullTotalPhys;
        public ulong ullAvailPhys;
        public ulong ullTotalPageFile;
        public ulong ullAvailPageFile;
        public ulong ullTotalVirtual;
        public ulong ullAvailVirtual;
        public ulong ullAvailExtendedVirtual;
        public MEMORYSTATUSEX()
        {
            this.dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
        }
    }
}
