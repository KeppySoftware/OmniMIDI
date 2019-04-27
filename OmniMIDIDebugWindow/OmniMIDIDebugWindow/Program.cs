using System.Threading;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace OmniMIDIDebugWindow
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
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        struct WIN32_FIND_DATA
        {
            public uint dwFileAttributes;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
            public uint nFileSizeHigh;
            public uint nFileSizeLow;
            public uint dwReserved0;
            public uint dwReserved1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string cFileName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
            public string cAlternateFileName;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SYSTEM_INFO
        {
            public short wProcessorArchitecture;
            public short wReserved;
            public int dwPageSize;
            public IntPtr lpMinimumApplicationAddress;
            public IntPtr lpMaximumApplicationAddress;
            public IntPtr dwActiveProcessorMask;
            public int dwNumberOfProcessors;
            public int dwProcessorType;
            public int dwAllocationGranularity;
            public short wProcessorLevel;
            public short wProcessorRevision;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr FindFirstFile(string lpFileName, out WIN32_FIND_DATA lpFindFileData);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool FindNextFile(IntPtr hFindFile, out WIN32_FIND_DATA
           lpFindFileData);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool FindClose(IntPtr hFindFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool GetNamedPipeClientProcessId(IntPtr Pipe, out uint ClientProcessId);

        [DllImport("kernel32.dll")]
        private static extern void GetNativeSystemInfo(ref SYSTEM_INFO lpSystemInfo);

        public const short PROCESSOR_ARCHITECTURE_UNKNOWN = 0xFF;
        public const short PROCESSOR_ARCHITECTURE_ARM64 = 12;
        public const short PROCESSOR_ARCHITECTURE_AMD64 = 9;
        public const short PROCESSOR_ARCHITECTURE_IA64 = 6;
        public const short PROCESSOR_ARCHITECTURE_INTEL = 0;

        public static uint BringToFrontMessage;
        static EventWaitHandle m;
        public static Int32 SelectedDebugVal = 1;

        [STAThread]
        static void Main()
        {
            bool ok;
            BringToFrontMessage = WinAPI.RegisterWindowMessage("OmniMIDIDebugWindowToFront");
            m = new EventWaitHandle(false, EventResetMode.ManualReset, "OmniMIDIDebugWindow", out ok);
            if (!ok)
            {
                WinAPI.PostMessage((IntPtr)WinAPI.HWND_BROADCAST, BringToFrontMessage, IntPtr.Zero, IntPtr.Zero);
                return;
            }
            Process thisProc = Process.GetCurrentProcess();
            thisProc.PriorityClass = ProcessPriorityClass.Idle;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new OmniMIDIDebugWindow());
        }

        public static String[] GetDebugPipesList()
        {
            List<String> OMPipes = new List<string>();
            try
            {
                Int32 Found = 0;
                String PipeToAdd;
                WIN32_FIND_DATA lpFindFileData;

                IntPtr ptr = FindFirstFile(@"\\.\pipe\*", out lpFindFileData);
                PipeToAdd = Path.GetFileName(lpFindFileData.cFileName);
                if (PipeToAdd.Contains("OmniMIDIDbg"))
                {
                    OMPipes.Add(String.Format("Debug pipe {0} (OmniMIDIDbg{0})", Regex.Match(PipeToAdd, @"\d+").Value));
                    Found++;
                }

                while (FindNextFile(ptr, out lpFindFileData))
                {
                    PipeToAdd = Path.GetFileName(lpFindFileData.cFileName);
                    if (PipeToAdd.Contains("OmniMIDIDbg"))
                    {
                        OMPipes.Add(String.Format("Debug pipe {0} (OmniMIDIDbg{0})", Regex.Match(PipeToAdd, @"\d+").Value));
                        Found++;
                    }
                }
                FindClose(ptr);

                return OMPipes.ToArray();
            }
            catch (Exception ex)
            {
                // If something goes wrong, here's an error handler
                MessageBox.Show(ex.ToString() + "\n\nPress OK to stop the debug mode.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.ExitThread();

                return new String[] { };
            }
        }

        public static bool DoesPipeStillExist(int requestedpipe)
        {
            try
            {
                String PipeToAdd;

                IntPtr ptr = FindFirstFile(@"\\.\pipe\*", out WIN32_FIND_DATA lpFindFileData);
                PipeToAdd = Path.GetFileName(lpFindFileData.cFileName);
                if (PipeToAdd.Contains(String.Format("OmniMIDIDbg{0}", requestedpipe))) return true;

                while (FindNextFile(ptr, out lpFindFileData))
                {
                    PipeToAdd = Path.GetFileName(lpFindFileData.cFileName);
                    if (PipeToAdd.Contains(String.Format("OmniMIDIDbg{0}", requestedpipe))) return true;
                }
                FindClose(ptr);

                return false;
            }
            catch { return false; }
        }

        public static bool ConnectToFirstAvailablePipe()
        {
            String[] Pipes = GetDebugPipesList();
            if (Pipes.Length > 0)
            {
                Int32 TempPipe = int.Parse(Regex.Match(Pipes[0], @"\d+").Value);
                if (!Program.DoesPipeStillExist(TempPipe))
                    return false;

                SelectedDebugVal = TempPipe;
                return true;
            }
            else return false;
        }

        public static short GetProcessorArchitecture()
        {
            SYSTEM_INFO si = new SYSTEM_INFO();
            GetNativeSystemInfo(ref si);
            return si.wProcessorArchitecture;
        }
    }
}
