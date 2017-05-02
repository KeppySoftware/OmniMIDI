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
using Un4seen.BassAsio;

namespace KeppySynthConfigurator
{
    public partial class DefaultASIOAudioOutput : Form
    {
        public DefaultASIOAudioOutput()
        {
            InitializeComponent();
        }

        private void DefaultASIOAudioOutput_Load(object sender, EventArgs e)
        {
            try
            {
                int selecteddeviceprev = (int)KeppySynthConfiguratorMain.SynthSettings.GetValue("defaultAdev", 0);
                BASS_ASIO_DEVICEINFO info = new BASS_ASIO_DEVICEINFO();
                BassAsio.BASS_ASIO_GetDeviceInfo(selecteddeviceprev, info);
                DefOut.Text = String.Format("Def. ASIO output: {0}", info.ToString());
                for (int n = 0; BassAsio.BASS_ASIO_GetDeviceInfo(n, info); n++)
                {
                    DevicesList.Items.Add(info.ToString());
                }
                DevicesList.SelectedIndex = selecteddeviceprev;
                BassAsio.BASS_ASIO_Init(DevicesList.SelectedIndex, 0);
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
            Functions.SetDefaultDevice("ASIO", DevicesList.SelectedIndex);
            BassAsio.BASS_ASIO_Free();
            BassAsio.BASS_ASIO_Init(DevicesList.SelectedIndex, 0);
        }

        private void Quit_Click(object sender, EventArgs e)
        {
            BassAsio.BASS_ASIO_Free();
            Close();
            Dispose();
        }

        private void DeviceCP_Click(object sender, EventArgs e)
        {
            BassAsio.BASS_ASIO_ControlPanel();
        }
    }
}
