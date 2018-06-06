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

namespace OmniMIDIConfigurator
{
    public partial class DefaultOutput : Form
    {
        bool IsIt = false;

        public DefaultOutput(bool IsItWasapi)
        {
            InitializeComponent();
            IsIt = IsItWasapi;
        }

        private void DefaultOutput_Load(object sender, EventArgs e)
        {
            try
            {
                if (IsIt) Text = String.Format(Text, "WASAPI");
                else Text = String.Format(Text, "DirectSound");

                int selecteddeviceprev = (int)OmniMIDIConfiguratorMain.SynthSettings.GetValue("AudioOutput", 0);
                BASS_DEVICEINFO info = new BASS_DEVICEINFO();
                DevicesList.Items.Add("Default Windows audio output");
                Bass.BASS_GetDeviceInfo(selecteddeviceprev - 1, info);
                Bass.BASS_GetDeviceInfo(-1, info);
                if (selecteddeviceprev < 1)
                {
                    DefOut.Text = String.Format("Def. Windows audio output: Default Windows audio output", info.ToString());
                }
                else
                {
                    if (info.ToString() == "")
                    {
                        DefOut.Text = String.Format("Def. Windows audio output: No devices have been found", info.ToString());
                    }
                    else
                    {
                        DefOut.Text = String.Format("Def. Windows audio output: {0}", info.ToString());
                    }
                }
                for (int n = 0; Bass.BASS_GetDeviceInfo(n, info); n++)
                {
                    DevicesList.Items.Add(info.ToString());
                }

                try { DevicesList.SelectedIndex = selecteddeviceprev; }
                catch { DevicesList.SelectedIndex = 0; }

                DevicesList.SelectedIndexChanged += new System.EventHandler(this.DevicesList_SelectedIndexChanged);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load the dialog.\nBASS is probably unable to start, or it's missing.\n\nError:\n" + ex.Message.ToString(), "Oh no! OmniMIDI encountered an error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                Dispose();
            }
        }

        private void DevicesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Functions.SetDefaultDevice(AudioEngine.DSOUND_OR_WASAPI, DevicesList.SelectedIndex, null);
        }

        private void Quit_Click(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }
    }
}
