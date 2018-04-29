using System;
using System.Runtime.InteropServices;

namespace KeppySynthDebugWindow
{
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    struct LARGE_INTEGER
    {
        [FieldOffset(0)] public Int64 QuadPart;
        [FieldOffset(0)] public UInt32 LowPart;
        [FieldOffset(4)] public Int32 HighPart;
    }

    class TimerFuncs
    {
        public delegate void TimerSetDelegate();
        public delegate void TimerCompleteDelegate();

        public event TimerSetDelegate OnTimerSet;
        public event TimerCompleteDelegate OnTimerCompleted;

        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateWaitableTimer(IntPtr lpTimerAttributes, bool bManualReset, string lpTimerName);

        [DllImport("kernel32.dll")]
        public static extern bool SetWaitableTimer(IntPtr hTimer, [In] ref LARGE_INTEGER ft, int lPeriod, TimerCompleteDelegate pfnCompletionRoutine, IntPtr pArgToCompletionRoutine, bool fResume);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern Int32 WaitForSingleObject(IntPtr Handle, uint Wait);

        [DllImport("kernel32.dll")]
        public static extern bool CancelWaitableTimer(IntPtr hTimer);

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);

        public static void MicroSleep(Int64 MicroSec)
        {
            IntPtr timer;
            LARGE_INTEGER ft = new LARGE_INTEGER();

            ft.QuadPart = -(10 * MicroSec);

            timer = CreateWaitableTimer(IntPtr.Zero, true, null);
            SetWaitableTimer(timer, ref ft, 0, null, IntPtr.Zero, false);
            WaitForSingleObject(timer, 0xFFFFFFFF);
            CloseHandle(timer);
        }
    }
}
