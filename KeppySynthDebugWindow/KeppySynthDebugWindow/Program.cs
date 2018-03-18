using System.Threading;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace KeppySynthDebugWindow
{
    static class WinAPI
    {
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint RegisterWindowMessage(string lpString);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        public static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        public const uint HWND_BROADCAST = 0xFFFF;
        public const short SW_RESTORE = 9;
    }

    static class Program
    {
        public static uint BringToFrontMessage;

        [STAThread]
        static void Main()
        {
            if (Environment.OSVersion.Version.Major < 6)
            {
                try
                {
                    System.Media.SystemSounds.Hand.Play();
                    Environment.Exit(-1);
                }
                catch
                {
                    Environment.Exit(-1);
                }
            }
            bool ok;
            BringToFrontMessage = WinAPI.RegisterWindowMessage("KeppySynthDebugWindowToFront");
            EventWaitHandle m = new EventWaitHandle(false, EventResetMode.ManualReset, "KeppySynthDebugWindow", out ok);
            if (!ok)
            {
                WinAPI.PostMessage((IntPtr)WinAPI.HWND_BROADCAST, BringToFrontMessage, IntPtr.Zero, IntPtr.Zero);
                return;
            }
            Process thisProc = Process.GetCurrentProcess();
            thisProc.PriorityClass = ProcessPriorityClass.Idle;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new KeppySynthDebugWindow());
        }
    }
}
