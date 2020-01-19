using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace OmniMIDIConfigurator
{
    public partial class PitchAndTranspose : Form
    {
        String[] NoteNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
        String[] CNames = new String[] { "ch1pshift", "ch2pshift", "ch3pshift", "ch4pshift", "ch5pshift",
                "ch6pshift", "ch7pshift", "ch8pshift", "ch9pshift", "ch10pshift",
                "ch11pshift", "ch12pshift", "ch13pshift", "ch14pshift", "ch15pshift", "ch16pshift",};

        public PitchAndTranspose()
        {
            InitializeComponent();
        }

        private void PitchShifting_Load(object sender, EventArgs e)
        {
            LoadChannels();

            NewTranspose.Value = (Convert.ToInt32(Program.SynthSettings.GetValue("TransposeValue", "127")) - 127);
            NewCPitch.Value = (Convert.ToInt32(Program.SynthSettings.GetValue("CPitchValue", "8192")) - 8192);

            NewTranspose_ValueChanged(null, null);
            NewCPitch_ValueChanged(null, null);
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ApplyBtn_Click(object sender, EventArgs e)
        {
            TimerLive.Enabled = false;

            bool[] booleans = new bool[] { CH1.Checked, CH2.Checked, CH3.Checked, CH4.Checked, CH5.Checked,
                CH6.Checked, CH7.Checked, CH8.Checked, CH9.Checked, CH10.Checked,
                CH11.Checked, CH12.Checked, CH13.Checked, CH14.Checked, CH15.Checked, CH16.Checked };

            for (int i = 0; i <= 15; i++)
                Program.Channels.SetValue(CNames[i], Convert.ToInt32(booleans[i]), RegistryValueKind.DWord);

            Program.SynthSettings.SetValue("TransposeValue", (NewTranspose.Value + 127), RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("CPitchValue", (NewCPitch.Value + 8192), RegistryValueKind.DWord);

            Dispose();
        }

        private void LoadChannels()
        {
            if (Convert.ToInt32(Program.Channels.GetValue(CNames[0], "0")) == 0)
            {
                CH1.Checked = false;
            }
            else
            {
                CH1.Checked = true;
            }

            if (Convert.ToInt32(Program.Channels.GetValue(CNames[1], "0")) == 0)
            {
                CH2.Checked = false;
            }
            else
            {
                CH2.Checked = true;
            }

            if (Convert.ToInt32(Program.Channels.GetValue(CNames[2], "0")) == 0)
            {
                CH3.Checked = false;
            }
            else
            {
                CH3.Checked = true;
            }

            if (Convert.ToInt32(Program.Channels.GetValue(CNames[3], "0")) == 0)
            {
                CH4.Checked = false;
            }
            else
            {
                CH4.Checked = true;
            }

            if (Convert.ToInt32(Program.Channels.GetValue(CNames[4], "0")) == 0)
            {
                CH5.Checked = false;
            }
            else
            {
                CH5.Checked = true;
            }

            if (Convert.ToInt32(Program.Channels.GetValue(CNames[5], "0")) == 0)
            {
                CH6.Checked = false;
            }
            else
            {
                CH6.Checked = true;
            }

            if (Convert.ToInt32(Program.Channels.GetValue(CNames[6], "0")) == 0)
            {
                CH7.Checked = false;
            }
            else
            {
                CH7.Checked = true;
            }

            if (Convert.ToInt32(Program.Channels.GetValue(CNames[7], "0")) == 0)
            {
                CH8.Checked = false;
            }
            else
            {
                CH8.Checked = true;
            }

            if (Convert.ToInt32(Program.Channels.GetValue(CNames[8], "0")) == 0)
            {
                CH9.Checked = false;
            }
            else
            {
                CH9.Checked = true;
            }

            if (Convert.ToInt32(Program.Channels.GetValue(CNames[9], "0")) == 0)
            {
                CH10.Checked = false;
            }
            else
            {
                CH10.Checked = true;
            }

            if (Convert.ToInt32(Program.Channels.GetValue(CNames[10], "0")) == 0)
            {
                CH11.Checked = false;
            }
            else
            {
                CH11.Checked = true;
            }

            if (Convert.ToInt32(Program.Channels.GetValue(CNames[11], "0")) == 0)
            {
                CH12.Checked = false;
            }
            else
            {
                CH12.Checked = true;
            }

            if (Convert.ToInt32(Program.Channels.GetValue(CNames[12], "0")) == 0)
            {
                CH13.Checked = false;
            }
            else
            {
                CH13.Checked = true;
            }

            if (Convert.ToInt32(Program.Channels.GetValue(CNames[13], "0")) == 0)
            {
                CH14.Checked = false;
            }
            else
            {
                CH14.Checked = true;
            }

            if (Convert.ToInt32(Program.Channels.GetValue(CNames[14], "0")) == 0)
            {
                CH15.Checked = false;
            }
            else
            {
                CH15.Checked = true;
            }

            if (Convert.ToInt32(Program.Channels.GetValue(CNames[15], "0")) == 0)
            {
                CH16.Checked = false;
            }
            else
            {
                CH16.Checked = true;
            }
        }


        private void CheckA_Click(object sender, EventArgs e)
        {
            CH1.Checked = true;
            CH2.Checked = true;
            CH3.Checked = true;
            CH4.Checked = true;
            CH5.Checked = true;
            CH6.Checked = true;
            CH7.Checked = true;
            CH8.Checked = true;
            CH9.Checked = true;
            CH10.Checked = true;
            CH11.Checked = true;
            CH12.Checked = true;
            CH13.Checked = true;
            CH14.Checked = true;
            CH15.Checked = true;
            CH16.Checked = true;
        }

        private void UncheckA_Click(object sender, EventArgs e)
        {
            CH1.Checked = false;
            CH2.Checked = false;
            CH3.Checked = false;
            CH4.Checked = false;
            CH5.Checked = false;
            CH6.Checked = false;
            CH7.Checked = false;
            CH8.Checked = false;
            CH9.Checked = false;
            CH10.Checked = false;
            CH11.Checked = false;
            CH12.Checked = false;
            CH13.Checked = false;
            CH14.Checked = false;
            CH15.Checked = false;
            CH16.Checked = false;
        }

        private void LiveBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (LiveBtn.Checked)
            {
                TimerLive.Enabled = true;
                LiveBtn.ForeColor = Color.DarkGreen;
            }
            else
            {
                TimerLive.Enabled = false;
                LiveBtn.ForeColor = Color.DarkRed;
            }
        }

        private void TimerLive_Tick(object sender, EventArgs e)
        {
            try
            {
                bool[] booleans = new bool[] { CH1.Checked, CH2.Checked, CH3.Checked, CH4.Checked, CH5.Checked,
                CH6.Checked, CH7.Checked, CH8.Checked, CH9.Checked, CH10.Checked,
                CH11.Checked, CH12.Checked, CH13.Checked, CH14.Checked, CH15.Checked, CH16.Checked };

                for (int i = 0; i <= 15; i++)
                    Program.Channels.SetValue(CNames[i], Convert.ToInt32(booleans[i]), RegistryValueKind.DWord);

                Program.SynthSettings.SetValue("TransposeValue", (NewTranspose.Value + 127), RegistryValueKind.DWord);
                Program.SynthSettings.SetValue("CPitchValue", (NewCPitch.Value + 8192), RegistryValueKind.DWord);
            }
            catch { }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void NewTranspose_ValueChanged(object sender, EventArgs e)
        {
            Int32 Octave = Convert.ToInt32(NewTranspose.Value + 48) / 12;
            Int32 NoteInOctave = Convert.ToInt32(NewTranspose.Value) % 12;

            if (NoteInOctave < 0) NoteInOctave = NoteInOctave * (-1);

            label4.Text = String.Format("Transposed by {0}, expected root key is {1}{2}", NewTranspose.Value, NoteNames[NoteInOctave], Octave);
        }

        private void NewCPitch_ValueChanged(object sender, EventArgs e)
        {
            Double NewFreq = 440.0 + (200.0 / 16384.0 * Convert.ToDouble(NewCPitch.Value));
            Double Difference = Math.Abs(440.0 - NewFreq);

            label5.Text = String.Format("New pitch is {0}Hz, a difference of {1}Hz", NewFreq.ToString("N2"), Difference.ToString("N2"));
        }
    }
}