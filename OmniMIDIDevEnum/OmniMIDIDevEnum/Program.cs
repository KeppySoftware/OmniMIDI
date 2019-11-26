using System;
using System.Runtime.InteropServices;

namespace OmniMIDIDevEnum
{
    internal static class WinMM
    {
        [DllImport("winmm")]
        internal static extern int midiOutGetNumDevs();

        [DllImport("winmm")]
        internal static extern int midiOutGetDevCaps(
            uint uDeviceID,
            out MIDIOUTCAPS caps,
            uint cbMidiOutCaps);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MIDIOUTCAPS
    {
        public ushort wMid;
        public ushort wPid;
        public uint vDriverVersion;     //MMVERSION
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string szPname;
        public ushort wTechnology;
        public ushort wVoices;
        public ushort wNotes;
        public ushort wChannelMask;
        public uint dwSupport;
    }

    class Program
    {
        static void Main(string[] args)
        {
            MIDIOUTCAPS OutCaps = new MIDIOUTCAPS();
            int NumDevs = WinMM.midiOutGetNumDevs();

            for (uint i = 0; i < NumDevs; i++)
            {
                WinMM.midiOutGetDevCaps(i, out OutCaps, (uint)Marshal.SizeOf(OutCaps));
                Console.WriteLine(OutCaps.szPname);
            }
        }
    }
}
