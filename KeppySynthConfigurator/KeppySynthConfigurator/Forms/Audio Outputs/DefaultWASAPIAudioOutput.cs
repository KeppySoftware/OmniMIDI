using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
// For device info
using Un4seen.Bass;
using Un4seen.BassWasapi;

namespace KeppySynthConfigurator
{
    public partial class DefaultWASAPIAudioOutput : Form
    {
        public DefaultWASAPIAudioOutput()
        {
            InitializeComponent();
        }

        private void AddLineToList(BASS_WASAPI_DEVICEINFO info, int n, int selecteddeviceprev)
        {
            if (info.IsDisabled)
            {
                if (info.IsInput) DevicesList.Items.Add(String.Format("{0} ({1})", info.ToString(), "Unsupported device, Disabled"));
                else if (!info.IsInput) DevicesList.Items.Add(String.Format("{0} ({1})", info.ToString(), "Output device, Disabled"));
                else if (info.IsLoopback) DevicesList.Items.Add(String.Format("{0} ({1})", info.ToString(), "Loopback device, Disabled"));
            }
            else if (info.IsEnabled)
            {
                if (info.IsInput) DevicesList.Items.Add(String.Format("{0} ({1})", info.ToString(), "Unsupported device, Enabled"));
                else if (!info.IsInput) DevicesList.Items.Add(String.Format("{0} ({1})", info.ToString(), "Output device, Enabled"));
                else if (info.IsLoopback) DevicesList.Items.Add(String.Format("{0} ({1})", info.ToString(), "Loopback device, Enabled"));
            }
            else if (info.IsUnplugged) DevicesList.Items.Add(String.Format("{0} ({1})", info.ToString(), "Unplugged device"));
            else DevicesList.Items.Add(String.Format("{0} ({1})", info.ToString(), "Unknown device"));

            if (n == selecteddeviceprev)
            {
                DefOut.Text = String.Format("Def. WASAPI audio output: {0}", DevicesList.Items[n].ToString());
            }
        }

        private void DefaultWASAPIAudioOutput_Load(object sender, EventArgs e)
        {
            try
            {
                if ((int)KeppySynthConfiguratorMain.SynthSettings.GetValue("wasapiex", 0) == 1) ExAccess.Checked = true;
                DevicesList.Items.Add("Default WASAPI output device");
                int selecteddeviceprev = (int)KeppySynthConfiguratorMain.SynthSettings.GetValue("defaultWdev", 0);
                BASS_WASAPI_DEVICEINFO info = new BASS_WASAPI_DEVICEINFO();
                for (int n = 0; BassWasapi.BASS_WASAPI_GetDeviceInfo(n, info); n++)
                {
                    AddLineToList(info, n, selecteddeviceprev);
                }

                try { DevicesList.SelectedIndex = selecteddeviceprev; }
                catch { DevicesList.SelectedIndex = 0; }

                DevicesList.SelectedIndexChanged += new System.EventHandler(this.DevicesList_SelectedIndexChanged);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load the dialog.\nBASS is probably unable to start, or it's missing.\n\nError:\n" + ex.ToString(), "Oh no! Keppy's Synthesizer encountered an error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                Dispose();
            }
        }

        private void DevicesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Functions.SetDefaultDevice(2, DevicesList.SelectedIndex);
        }

        private void Quit_Click(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }

        private void ExAccess_CheckedChanged(object sender, EventArgs e)
        {
            if (ExAccess.Checked)
            {
                KeppySynthConfiguratorMain.SynthSettings.SetValue("wasapiex", 1);
            }
            else
            {
                KeppySynthConfiguratorMain.SynthSettings.SetValue("wasapiex", 0);
            }
            KeppySynthConfiguratorMain.Delegate.AudioEngBox_SelectedIndexChanged(null, null);
        }

        private void ImConfusedHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("The only devices supported by BASSWASAPI for audio output, are the \"Output\" or \"Loopback\".\n\nSearch for the name of your default Windows audio output, and make sure it's an \"Output\" or \"Loopback\" device.\nIf it is, then select it, and you're ready to go.", "Which one of these devices is the right one?", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
