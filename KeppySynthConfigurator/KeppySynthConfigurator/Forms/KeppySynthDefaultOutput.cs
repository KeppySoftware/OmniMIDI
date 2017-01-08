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

namespace KeppySynthConfigurator
{
    public partial class KeppySynthDefaultOutput : Form
    {
        public KeppySynthDefaultOutput()
        {
            InitializeComponent();
        }

        private void KeppySynthDefaultOutput_Load(object sender, EventArgs e)
        {
            try
            {
                BASS_DEVICEINFO info = new BASS_DEVICEINFO();
                DevicesList.Items.Add("Default Windows audio output");
                Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_NOSPEAKER, IntPtr.Zero);
                int curdev = Bass.BASS_GetDevice();
                Bass.BASS_Free();
                Bass.BASS_GetDeviceInfo(curdev, info);
                if (info.ToString() == "")
                {
                    DefOut.Text = String.Format("Def. Windows audio output: No devices have been found", info.ToString());
                }
                else
                {
                    DefOut.Text = String.Format("Def. Windows audio output: {0}", info.ToString());
                }
                for (int n = 0; Bass.BASS_GetDeviceInfo(n, info); n++)
                {
                    DevicesList.Items.Add(info.ToString());
                }
                DevicesList.SelectedIndex = (int)KeppySynthConfiguratorMain.SynthSettings.GetValue("defaultdev", 0);
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
            Functions.SetDefaultDevice(DevicesList.SelectedIndex);
        }

        private void Quit_Click(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }
    }
}
