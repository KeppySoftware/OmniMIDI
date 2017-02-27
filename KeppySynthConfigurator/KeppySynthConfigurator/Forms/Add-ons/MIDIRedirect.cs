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
using Un4seen.Bass.AddOn.Midi;

namespace KeppySynthConfigurator
{
    public partial class MIDIRedirect : Form
    {
        public MIDIRedirect()
        {
            InitializeComponent();
        }

        private void KeppySynthDefaultOutput_Load(object sender, EventArgs e)
        {
            try
            {
                BASS_MIDI_DEVICEINFO info = new BASS_MIDI_DEVICEINFO();
                for (int n = 0; BassMidi.BASS_MIDI_InGetDeviceInfo(n, info); n++)
                {
                    DevicesList.Items.Add(info.ToString());
                }
                DevicesList.SelectedIndex = (int)KeppySynthConfiguratorMain.SynthSettings.GetValue("defaultmidiindev", 0);
                if ((int)KeppySynthConfiguratorMain.SynthSettings.GetValue("midiinenabled", 0) == 0)
                {
                    label1.Enabled = false;
                    DevicesList.Enabled = false;
                }
                else
                {
                    label1.Enabled = true;
                    DevicesList.Enabled = true;
                }
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
            Functions.SetDefaultMIDIInDevice(DevicesList.SelectedIndex);
        }

        private void EnOrDis_CheckedChanged(object sender, EventArgs e)
        {
            if (EnOrDis.Checked != true)
            {
                label1.Enabled = false;
                DevicesList.Enabled = false;
                KeppySynthConfiguratorMain.SynthSettings.SetValue("midiinenabled", 0);
            }
            else
            {
                KeppySynthConfiguratorMain.SynthSettings.GetValue("midiinenabled", 1);
                label1.Enabled = true;
                DevicesList.Enabled = true;
            }
        }

        private void Quit_Click(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }
    }
}
