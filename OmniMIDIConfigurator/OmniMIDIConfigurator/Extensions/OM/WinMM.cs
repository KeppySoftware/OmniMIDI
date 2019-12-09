using System;
using System.Runtime.InteropServices;

namespace OmniMIDIConfigurator
{
    internal static class WinMM
    {
        internal const int MMSYSERR_NOERROR = 0;
        internal const int CALLBACK_FUNCTION = 0x00030000;

        internal delegate void MidiInProc(
            IntPtr hMidiIn,
            uint wMsg,
            UIntPtr dwInstance,
            UIntPtr dwParam1,
            UIntPtr dwParam2);

        [DllImport("winmm")]
        internal static extern int midiInGetNumDevs();

        [DllImport("winmm")]
        internal static extern int midiInGetDevCaps(
            uint uDeviceID,
            out MIDIINCAPS caps,
            uint cbMidiInCaps);

        [DllImport("winmm")]
        internal static extern int midiInClose(
            IntPtr hMidiIn);

        [DllImport("winmm")]
        internal static extern int midiInOpen(
            out IntPtr lphMidiIn,
            int uDeviceID,
            MidiInProc dwCallback,
            IntPtr dwCallbackInstance,
            int dwFlags);

        [DllImport("winmm")]
        internal static extern int midiInStart(
            IntPtr hMidiIn);

        [DllImport("winmm")]
        internal static extern int midiInStop(
            IntPtr hMidiIn);

        [DllImport("winmm.dll")]
        internal static extern int midiConnect(
            IntPtr hMidi,
            IntPtr hMidiOut,
            IntPtr pReserved);

        [DllImport("winmm.dll")]
        internal static extern int midiDisconnect(
            IntPtr hMidi,
            IntPtr hMidiOut,
            IntPtr pReserved);
    }

    internal static class MIDIInEvent
    {
        // Internal
        public const int MIM_OPEN = 0x3C1;
        public const int MIM_CLOSE = 0x3C2;
        public const int MIM_DATA = 0x3C3;
        public const int MIM_LONGDATA = 0x3C4;
        public const int MIM_ERROR = 0x3C5;
        public const int MIM_LONGERROR = 0x3C6;
        public const int MIM_MOREDATA = 0x3CC;
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
