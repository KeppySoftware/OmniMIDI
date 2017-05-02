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
                if (info.IsInput) DevicesList.Items.Add(String.Format("{0} ({1})", info.ToString(), "Input device, Disabled"));
                else if (!info.IsInput) DevicesList.Items.Add(String.Format("{0} ({1})", info.ToString(), "Output device, Disabled"));
            }
            else if (info.IsEnabled)
            {
                if (info.IsInput) DevicesList.Items.Add(String.Format("{0} ({1})", info.ToString(), "Input device, Enabled"));
                else if (!info.IsInput) DevicesList.Items.Add(String.Format("{0} ({1})", info.ToString(), "Output device, Enabled"));
            }
            else if (info.IsUnplugged) DevicesList.Items.Add(String.Format("{0} ({1})", info.ToString(), "Unplugged"));
            else DevicesList.Items.Add(String.Format("{0} ({1})", info.ToString(), "Unknown"));

            if (n == selecteddeviceprev)
            {
                DefOut.Text = String.Format("Def. WASAPI audio output: {0}", DevicesList.Items[n - 1].ToString());
            }
        }

        private void DefaultWASAPIAudioOutput_Load(object sender, EventArgs e)
        {
            try
            {
                int selecteddeviceprev = (int)KeppySynthConfiguratorMain.SynthSettings.GetValue("defaultWdev", 0) + 2;
                BASS_WASAPI_DEVICEINFO info = new BASS_WASAPI_DEVICEINFO();
                DevicesList.Items.Add("Default Windows audio output");
                for (int n = 0; BassWasapi.BASS_WASAPI_GetDeviceInfo(n, info); n++)
                {
                    AddLineToList(info, n, selecteddeviceprev);
                }
                DevicesList.SelectedIndex = selecteddeviceprev - 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load the dialog.\nBASS is probably unable to start, or it's missing.\n\nError:\n" + ex.Message.ToString(), "Oh no! Keppy's Synthesizer encountered an error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                Dispose();
            }
        }

        private void DevicesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Functions.SetDefaultDevice("WASAPI", DevicesList.SelectedIndex - 1);
        }

        private void Quit_Click(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }
    }
}
