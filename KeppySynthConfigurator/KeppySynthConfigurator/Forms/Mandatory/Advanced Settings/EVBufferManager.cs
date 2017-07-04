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
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);

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
                if (!GlobalMemoryStatusEx(memStatus)) MessageBox.Show("Unknown error.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                installedMemory = memStatus.ullTotalPhys;

                if ((int)KeppySynthConfiguratorMain.SynthSettings.GetValue("evbuffbyram", "0") == 1)
                    GetRAMSize.Checked = true;
                else
                    GetRAMSize.Checked = false;

                BytesVal.Maximum = installedMemory;
                WarningLabel.Text = String.Format("WARNING:\nLeave at least {0} of RAM available for Windows.",
                    Functions.ReturnSoundFontSize(null, "evbuff", (long)installedMemory / 8));

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
                BytesVal.Enabled = false;
                RatioVal.Enabled = true;
                BytesVal.Value = installedMemory;
                RatioVal.Value = Convert.ToDecimal(KeppySynthConfiguratorMain.SynthSettings.GetValue("evbuffratio", "1"));
            }
            else
            {
                KeppySynthConfiguratorMain.SynthSettings.SetValue("evbuffbyram", "0", Microsoft.Win32.RegistryValueKind.DWord);
                decimal evbuffsizetemp = Convert.ToDecimal(KeppySynthConfiguratorMain.SynthSettings.GetValue("evbuffsize", "16384"));
                BytesVal.Enabled = true;
                RatioVal.Enabled = false;
                if (evbuffsizetemp == installedMemory) BytesVal.Value = 16384;
                else BytesVal.Value = evbuffsizetemp;
                RatioVal.Value = 1;
            }
        }

        private void PerformRAMCheck()
        {
            long check = (long)(BytesVal.Value / RatioVal.Value);

            if (check > (long)(installedMemory / 8)) WarningPanel.Visible = true;
            else WarningPanel.Visible = false;
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
