using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace OmniMIDIConfigurator
{
    public partial class MIDIFeedback : Form
    {
        public MIDIFeedback()
        {
            InitializeComponent();

            int NumDevs = WinMM.midiOutGetNumDevs();
            string OldDev = (string)Program.SynthSettings.GetValue("FeedbackDevice", "Microsoft GS Wavetable Synth");
            MIDIOUTCAPS Caps = new MIDIOUTCAPS();

            for (int i = 0; i < NumDevs; i++)
            {
                WinMM.midiOutGetDevCaps((uint)i, out Caps, (uint)Marshal.SizeOf(Caps));
                if (!Caps.szPname.Equals("OmniMIDI"))
                    MIDIOutDevs.Items.Add(Caps.szPname);
            }

            if (MIDIOutDevs.Items.Count < 1)
            {
                MessageBox.Show("No MIDI output devices available!\n\nPress OK to continue.", "OmniMIDI - ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            for (int c = 0; c < MIDIOutDevs.Items.Count; c++)
            {
                if (MIDIOutDevs.Items[c].Equals(OldDev))
                    MIDIOutDevs.SelectedIndex = c;
            }

            EnableFeedback.Checked = Convert.ToBoolean(Program.SynthSettings.GetValue("FeedbackEnabled", 0));
        }

        private void MIDIFeedback_Load(object sender, EventArgs e)
        {
            // Nothing
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            Program.SynthSettings.SetValue("FeedbackDevice", MIDIOutDevs.SelectedItem.ToString(), RegistryValueKind.String);
            Program.SynthSettings.SetValue("FeedbackEnabled", EnableFeedback.Checked ? 1 : 0, RegistryValueKind.DWord);

            if (Properties.Settings.Default.LiveChanges) Program.SynthSettings.SetValue("LiveChanges", "1", RegistryValueKind.DWord);

            Close();
        }
    }
}
