using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OmniMIDIConfigurator
{
    public partial class WinMMTest : Form
    {
        IntPtr hMidiOut = IntPtr.Zero;
        IntPtr hMidiIn = IntPtr.Zero;

        public WinMMTest()
        {
            InitializeComponent();
        }

        private void WinMMTest_Load(object sender, EventArgs e)
        {
            Refreshdev_Click(sender, e);
        }

        private void Refreshdev_Click(object sender, EventArgs e)
        {
            int OutCount = WinMM.midiOutGetNumDevs();
            int IntCount = WinMM.midiInGetNumDevs();

            OutCombo.Items.Clear();
            for (uint i = 0; i < OutCount; i++)
            {
                MIDIOUTCAPS MidiDev;
                if (WinMM.midiOutGetDevCaps(i, out MidiDev, (uint)Marshal.SizeOf(typeof(MIDIOUTCAPS))) == 0)
                {
                    OutCombo.Items.Add(MidiDev.szPname);
                    OutCombo.Enabled = true;
                }
            }

            InCombo.Items.Clear();
            for (uint i = 0; i < IntCount; i++)
            {
                MIDIINCAPS MidiDev;
                if (WinMM.midiInGetDevCaps(i, out MidiDev, (uint)Marshal.SizeOf(typeof(MIDIINCAPS))) == 0)
                {
                    InCombo.Items.Add(MidiDev.szPname);
                    InCombo.Enabled = true;
                }
            }
        }

        private void InitMIDIOut_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(OutCombo.Text))
            {
                MessageBox.Show("No device selected", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (hMidiOut != IntPtr.Zero)
            {
                MessageBox.Show("There's already a device allocated now.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int res = WinMM.midiOutOpen(out hMidiOut, OutCombo.SelectedIndex, null, IntPtr.Zero, 0);
            if (res != 0)
                MessageBox.Show(String.Format("Failed to open MIDI out device.\nError: {0}", res), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void InitMIDIIn_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(InCombo.Text))
            {
                MessageBox.Show("No device selected", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (hMidiIn != IntPtr.Zero)
            {
                MessageBox.Show("There's already a device allocated now.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int res = WinMM.midiInOpen(out hMidiIn, InCombo.SelectedIndex, null, IntPtr.Zero, 0);
            if (res != 0)
                MessageBox.Show(String.Format("Failed to open MIDI in device.\nError: {0}", res), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void StopMIDIOut_Click(object sender, EventArgs e)
        {
            int res = WinMM.midiOutClose(hMidiOut);

            if (res != 0)
            {
                MessageBox.Show(String.Format("Failed to close MIDI out device.\nError: {0}", res), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else hMidiOut = IntPtr.Zero;
        }

        private void StopMIDIIn_Click(object sender, EventArgs e)
        {
            int res = WinMM.midiInClose(hMidiIn);

            if (res != 0)
            {
                MessageBox.Show(String.Format("Failed to close MIDI in device.\nError: {0}", res), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else hMidiIn = IntPtr.Zero;
        }

        private void MidiConnectA_Click(object sender, EventArgs e)
        {
            if (hMidiOut == IntPtr.Zero)
            {
                MessageBox.Show("No MIDI out device has been initialized.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (hMidiIn == IntPtr.Zero)
            {
                MessageBox.Show("No MIDI in device has been initialized.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int res = WinMM.midiConnect(hMidiIn, hMidiOut, IntPtr.Zero);

            if (res != 0)
            {
                MessageBox.Show(String.Format("Failed to execute midiConnect.\nError: {0}", res), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void MidiDisconnectA_Click(object sender, EventArgs e)
        {
            int res = WinMM.midiDisconnect(hMidiIn, hMidiOut, IntPtr.Zero);

            if (res != 0)
            {
                MessageBox.Show(String.Format("Failed to execute midiDisconnect.\nError: {0}", res), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
