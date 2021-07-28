using System;
using System.Runtime.InteropServices;

namespace OmniMIDIConfigurator
{
    class KDMAPI
    {
        [DllImport("OmniMIDI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int ReturnKDMAPIVer(ref Int32 Major, ref Int32 Minor, ref Int32 Build, ref Int32 Revision);

        [DllImport("OmniMIDI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int InitializeKDMAPIStream();

        [DllImport("OmniMIDI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int TerminateKDMAPIStream();

        [DllImport("OmniMIDI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void ResetKDMAPIStream();

        [DllImport("OmniMIDI.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "#44")]
        public static extern void DisableFeedbackMode();

        [DllImport("OmniMIDI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int IsKDMAPIAvailable();

        [DllImport("OmniMIDI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int SendDirectData(uint dwMsg);

        [DllImport("OmniMIDI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int SendDirectDataNoBuf(uint dwMsg);

        [DllImport("OmniMIDI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int SendDirectLongData(UIntPtr IIMidiHdr);

        [DllImport("OmniMIDI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern int SendDirectLongDataNoBuf(UIntPtr IIMidiHdr);

        public static String KDMAPIVer = "Null";
    }
}
