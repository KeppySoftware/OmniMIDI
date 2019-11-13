using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OmniMIDIConfigurator
{
    public partial class MIDIInPlay : Form
    {
        private static WinMM.MidiInProc midiInProc;
        private static IntPtr handle;

        private static Mutex mut = new Mutex();
        private static Stopwatch WhenItGotReceived;
        private static String LastEvent = "NONE";

        private static Int32 DeviceCount;

        public MIDIInPlay()
        {
            InitializeComponent();
        }

        private void MIDIInPlay_Load(object sender, EventArgs e)
        {
            LastEvent = "NONE";
            mut = new Mutex();

            RefreshInputs_Click(sender, e);
        }

        private void SetLastEvent(String EventName, Boolean Failed)
        {
            LastEvent = EventName;
            WhenItGotReceived = Stopwatch.StartNew();
        }

        private void PrintEvent(String Event, String Data, Color Col)
        {
            mut.WaitOne();
            DataLog.AppendText(String.Format("\nReceived {0}: ", Event));
            DataLog.AppendText(Data, Col);
            mut.ReleaseMutex();
        }

        private void MidiProc(IntPtr hMidiIn, uint wMsg, UIntPtr dwInstance, UIntPtr dwParam1, UIntPtr dwParam2)
        {
            String StrEvent = "0x" + dwParam1.ToUInt32().ToString("X6");
            String StrdwP2 = "0x" + dwParam2.ToUInt32().ToString("X6");

            switch (wMsg)
            {
                case MIDIInEvent.MIM_DATA:
                    KDMAPI.SendDirectData(dwParam1.ToUInt32());
                    Task.Factory.StartNew(() => PrintEvent("MIM_DATA", StrEvent, Color.Lime));
                    SetLastEvent(StrEvent, false);
                    return;
                case MIDIInEvent.MIM_MOREDATA:
                    KDMAPI.SendDirectDataNoBuf(dwParam1.ToUInt32());
                    Task.Factory.StartNew(() => PrintEvent("MIM_MOREDATA", StrEvent, Color.OrangeRed));
                    SetLastEvent(String.Format("SLOW {0}", StrEvent), false);
                    return;
                case MIDIInEvent.MIM_LONGDATA:
                    KDMAPI.SendDirectLongData(dwParam1);
                    Task.Factory.StartNew(() => PrintEvent("MIM_LONGDATA", StrEvent, Color.MediumPurple));
                    SetLastEvent(StrEvent, false);
                    return;
                case MIDIInEvent.MIM_OPEN:
                    SetLastEvent("MIM_OPEN", false);
                    Task.Factory.StartNew(() => PrintEvent("MIM_OPEN", String.Format("P1: {0} - P2: {0}", StrEvent, StrdwP2), Color.MediumPurple));
                    return;
                case MIDIInEvent.MIM_CLOSE:
                    SetLastEvent("MIM_CLOSE", false);
                    Task.Factory.StartNew(() => PrintEvent("MIM_CLOSE", String.Format("P1: {0} - P2: {0}", StrEvent, StrdwP2), Color.MediumPurple));
                    return;
                case MIDIInEvent.MIM_ERROR:
                    SetLastEvent(String.Format("MIM_ERROR {0}", StrEvent), true);
                    Task.Factory.StartNew(() => PrintEvent("MIM_ERROR", StrEvent, Color.MediumPurple));
                    return;
                case MIDIInEvent.MIM_LONGERROR:
                    SetLastEvent(String.Format("MIM_LONGERROR {0}", StrEvent), true);
                    Task.Factory.StartNew(() => PrintEvent("MIM_LONGERROR", StrEvent, Color.MediumPurple));
                    return;
                default:
                    SetLastEvent("MIM_UNK", true);
                    return;
            }
        }

        private void MIDIInList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (handle != IntPtr.Zero)
            {
                WinMM.midiInStop(handle);
                WinMM.midiInClose(handle);
            }

            midiInProc = new WinMM.MidiInProc(MidiProc);
            int retval = WinMM.midiInOpen(out handle, MIDIInList.SelectedIndex, midiInProc, IntPtr.Zero, WinMM.CALLBACK_FUNCTION);
            WinMM.midiInStart(handle);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (handle != IntPtr.Zero)
            {
                WinMM.midiInStop(handle);
                WinMM.midiInClose(handle);
            }

            mut.WaitOne();
            mut.ReleaseMutex();
            mut.Close();

            KDMAPI.TerminateKDMAPIStream();

            if (e.CloseReason == CloseReason.WindowsShutDown) return;
        }

        private void StatusTimer_Tick(object sender, EventArgs e)
        {
            if (handle != IntPtr.Zero && WhenItGotReceived != null)
            {
                ActivityPanel.BackColor = (WhenItGotReceived.ElapsedMilliseconds < 200) ? Color.DarkGreen : Color.DarkOrange;
                ActivityLabel.Text = String.Format("Received {0}.", LastEvent);
            }
            else
            {
                ActivityPanel.BackColor = Color.DarkRed;
                ActivityLabel.Text = "Inactive.";
            }
        }

        private void RefreshInputs_Click(object sender, EventArgs e)
        {
            // Check count
            DeviceCount = WinMM.midiInGetNumDevs();
            if (DeviceCount < 1)
            {
                // None available, close
                MessageBox.Show("No MIDI input devices available.", "OmniMIDI - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            // Initialize KDMAPI
            if (sender == null)
            {
                if (!Convert.ToBoolean(KDMAPI.InitializeKDMAPIStream()))
                {
                    MessageBox.Show("Unable to initialize KDMAPI.", "OmniMIDI - Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                    return;
                }

                KDMAPI.ResetKDMAPIStream();
            }

            // Initialize MIDI inputs list
            MIDIINCAPS InCaps = new MIDIINCAPS();
            MIDIInList.Items.Clear();
            for (uint i = 0; i < DeviceCount; i++)
            {
                WinMM.midiInGetDevCaps(i, out InCaps, (uint)Marshal.SizeOf(InCaps));
                MIDIInList.Items.Add(InCaps.szPname);
            }
        }

        private void DataLog_TextChanged(object sender, EventArgs e)
        {
            // set the current caret position to the end
            DataLog.SelectionStart = DataLog.Text.Length;
            // scroll it automatically
            DataLog.ScrollToCaret();
        }
    }
}

public static class RichTextBoxExtensions
{
    public static void AppendText(this RichTextBox box, string text, Color color)
    {
        box.SelectionStart = box.TextLength;
        box.SelectionLength = 0;

        box.SelectionColor = color;
        box.AppendText(text);
        box.SelectionColor = box.ForeColor;
    }
}