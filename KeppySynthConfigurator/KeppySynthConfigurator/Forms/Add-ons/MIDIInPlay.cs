using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace KeppySynthConfigurator
{
    public partial class MIDIInPlay : Form
    {
        private static WinMM.MidiInProc midiInProc;
        private static IntPtr handle;

        private static Stopwatch FromWhenItGotReceived;
        private static Boolean CurrentReceiving = false;

        public MIDIInPlay()
        {
            InitializeComponent();

            if (KeppySynth.IsKSDAPIAvailable())
                KeppySynth.InitializeKSStream();
            else return;

            KeppySynth.ResetKSStream();
        }

        private void MidiProc(IntPtr hMidiIn, uint wMsg, IntPtr dwInstance, uint dwParam1, uint dwParam2)
        {
            FromWhenItGotReceived = Stopwatch.StartNew();
            CurrentReceiving = true;
            KeppySynth.SendDirectData(dwParam1);
        }

        private void MIDIInPlay_Load(object sender, EventArgs e)
        {
            MIDIINCAPS InCaps = new MIDIINCAPS();

            for (uint i = 0; i < WinMM.midiInGetNumDevs(); i++)
            {
                WinMM.midiInGetDevCaps(i, out InCaps, (uint)Marshal.SizeOf(InCaps));
                MIDIInList.Items.Add(InCaps.szPname);
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

            if (e.CloseReason == CloseReason.WindowsShutDown) return;
        }

        private void StatusTimer_Tick(object sender, EventArgs e)
        {
            if (handle != IntPtr.Zero && FromWhenItGotReceived != null)
            {
                if (FromWhenItGotReceived != null && FromWhenItGotReceived.ElapsedMilliseconds < 200)
                {
                    ActivityPanel.BackColor = Color.DarkGreen;
                    ActivityLabel.Text = "Activity detected.";
                }
                else
                {
                    ActivityPanel.BackColor = Color.DarkRed;
                    ActivityLabel.Text = "No activity.";
                }
            }
            else
            {
                ActivityPanel.BackColor = Color.DarkGray;
                ActivityLabel.Text = "No MIDI input selected.";
            }
        }
    }

    public static class KeppySynth
    {
        [DllImport("keppysynth.dll")]
        internal static extern void InitializeKSStream();

        [DllImport("keppysynth.dll")]
        internal static extern void TerminateKSStream();

        [DllImport("keppysynth.dll")]
        internal static extern void ResetKSStream();

        [DllImport("keppysynth.dll")]
        internal static extern bool IsKSDAPIAvailable();

        [DllImport("keppysynth.dll")]
        internal static extern int SendDirectData(uint dwMsg);

        [DllImport("keppysynth.dll")]
        internal static extern int SendDirectDataNoBuf(uint dwMsg);

        [DllImport("keppysynth.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SendDirectLongData([Out] MIDIHDR IIMidiHdr);

        [DllImport("keppysynth.dll", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int SendDirectLongDataNoBuf([Out] MIDIHDR IIMidiHdr);
    }

    internal static class WinMM
    {
        internal const int MMSYSERR_NOERROR = 0;
        internal const int CALLBACK_FUNCTION = 0x00030000;

        internal delegate void MidiInProc(
            IntPtr hMidiIn,
            uint wMsg,
            IntPtr dwInstance,
            uint dwParam1,
            uint dwParam2);

        [DllImport("winmm.dll")]
        internal static extern int midiInGetNumDevs();

        [DllImport("winmm.dll")]
        internal static extern int midiInGetDevCaps(
            uint uDeviceID,
            out MIDIINCAPS caps,
            uint cbMidiInCaps);

        [DllImport("winmm.dll")]
        internal static extern int midiInClose(
            IntPtr hMidiIn);

        [DllImport("winmm.dll")]
        internal static extern int midiInOpen(
            out IntPtr lphMidiIn,
            int uDeviceID,
            MidiInProc dwCallback,
            IntPtr dwCallbackInstance,
            int dwFlags);

        [DllImport("winmm.dll")]
        internal static extern int midiInStart(
            IntPtr hMidiIn);

        [DllImport("winmm.dll")]
        internal static extern int midiInStop(
            IntPtr hMidiIn);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MIDIHDR
    {
        public string lpdata;
        public int dwBufferLength;
        public int dwBytesRecorded;
        public IntPtr dwUser;
        public  int dwFlags;
        public  IntPtr lpNext;
        public IntPtr reserved;
        public int dwOffset;
        public IntPtr dwReserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MIDIINCAPS
    {
        public ushort wMid;
        public ushort wPid;
        public uint vDriverVersion;     // MMVERSION
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string szPname;
        public uint dwSupport;
    }
}
