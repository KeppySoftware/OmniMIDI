using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OmniMIDIConfigurator
{
    static class WinAPI
    {
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern UInt32 RegisterWindowMessage(string lpString);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern Boolean PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern Boolean SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern Boolean ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int GetScrollPos(System.IntPtr hWnd, int nBar);

        [DllImport("user32.dll")]
        public static extern int SetScrollPos(System.IntPtr hWnd, int nBar, int nPos, bool bRedraw);

        [DllImport("user32.dll")]
        public static extern bool LockWindowUpdate(IntPtr Handle);

        public const int LVM_FIRST = 0x1000;
        public const int LVM_SCROLL = LVM_FIRST + 20;
        public const int SBS_HORZ = 0;
        public const int SBS_VERT = 1;
        public const int WM_HSCROLL = 0x114;
        public const int WM_VSCROLL = 0x115;
        public const int WM_SETREDRAW = 0x00B;
        public const uint HWND_BROADCAST = 0xFFFF;
        public const short SW_RESTORE = 9;
    }

    static class Program
    {
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        private const int IDC_HAND = 32649;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);

        // Mandatory values
        public static RegistryKey Mixer = null;
        public static RegistryKey SynthSettings = null;
        public static RegistryKey Mapper = null;
        public static RegistryKey Watchdog = null;

        public const string HKCU = "HKEY_CURRENT_USER\\";
        public const string MIPath = "SOFTWARE\\OmniMIDI";
        public const string SSPath = "SOFTWARE\\OmniMIDI\\Configuration";
        public const string MPPath = "SOFTWARE\\OmniMIDI\\Mapper";
        public const string WPath = "SOFTWARE\\OmniMIDI\\Watchdog";

        public static string OMFixedPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\OmniMIDI";
        public static string CSFFixedPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Common SoundFonts";

        public static string OMSFPath = OMFixedPath + "\\lists";
        public static string DebugDataPath = OMFixedPath + "\\debug";

        public static Cursor SystemHandCursor = new Cursor(LoadCursor(IntPtr.Zero, IDC_HAND));

        public static uint BringToFrontMessage;
        static EventWaitHandle m;

        public static string[] ListsPath = new string[]
        {
            CSFFixedPath + "\\SoundFontList.csflist",
            OMFixedPath + "\\lists\\OmniMIDI_A.omlist",
            OMFixedPath + "\\lists\\OmniMIDI_B.omlist",
            OMFixedPath + "\\lists\\OmniMIDI_C.omlist",
            OMFixedPath + "\\lists\\OmniMIDI_D.omlist",
            OMFixedPath + "\\lists\\OmniMIDI_E.omlist",
            OMFixedPath + "\\lists\\OmniMIDI_F.omlist",
            OMFixedPath + "\\lists\\OmniMIDI_G.omlist"
        };

        [STAThread]
        static void Main(String[] Args)
        {
            FileVersionInfo Driver = FileVersionInfo.GetVersionInfo(UpdateSystem.UpdateFileVersion);
            List<String> SoundFontsToAdd = new List<String>();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Functions.CheckDriverStatusInReg("x86", Functions.CLSID32);
            if (Environment.Is64BitOperatingSystem)
                Functions.CheckDriverStatusInReg("x64", Functions.CLSID64);

            OpenRequiredKey(ref Mixer, MIPath);
            OpenRequiredKey(ref SynthSettings, SSPath);
            OpenRequiredKey(ref Mapper, MPPath);
            OpenRequiredKey(ref Watchdog, WPath);

            if (!Directory.Exists(Path.GetDirectoryName(Program.ListsPath[0])))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Program.ListsPath[0]));
            }

            if (Properties.Settings.Default.UpdateBranch == "choose")
            {
                SelectBranch frm = new SelectBranch();
                frm.ShowInTaskbar = true;
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.ShowDialog();
                frm.Dispose();
            }

            foreach (String Arg in Args)
            {
                switch (Arg.ToLowerInvariant())
                {
                    case "/rei":
                        var current = Process.GetCurrentProcess();
                        Process.GetProcessesByName(current.ProcessName)
                            .Where(t => t.Id != current.Id)
                            .ToList()
                            .ForEach(t => t.Kill());

                        UpdateSystem.CheckForTLS12ThenUpdate(Driver.FileVersion, UpdateSystem.WIPE_SETTINGS);
                        return;
                    case "/showchangelog":
                        if (Properties.Settings.Default.ShowChangelogStartUp)
                        {
                            try
                            {
                                new ChangelogWindow(Driver.FileVersion.ToString(), false).ShowDialog();
                            }
                            catch { }
                        }
                        break;
                    default:
                        SoundFontsToAdd.Add(Arg);
                        break;
                }
            }

            bool dummy;
            BringToFrontMessage = WinAPI.RegisterWindowMessage("OmniMIDIConfiguratorToFront");
            m = new EventWaitHandle(false, EventResetMode.ManualReset, "OmniMIDIConfigurator", out dummy);
            if (!dummy)
            {
                WinAPI.PostMessage((IntPtr)WinAPI.HWND_BROADCAST, BringToFrontMessage, IntPtr.Zero, IntPtr.Zero);
                return;
            }

            GC.KeepAlive(BringToFrontMessage);
            GC.KeepAlive(m);

            Application.Run(new MainWindow(SoundFontsToAdd.ToArray()));
        }

        private static void OpenRequiredKey(ref RegistryKey Key, String KeyPath)
        {
            Key = Registry.CurrentUser.OpenSubKey(KeyPath, true);
            if (Key == null)
            {
                try
                {
                    Key = Registry.CurrentUser.CreateSubKey(KeyPath, RegistryKeyPermissionCheck.ReadWriteSubTree);
                }
                catch (Exception ex)
                {
                    Program.ShowError(
                        4,
                        "FATAL ERROR",
                        "A fatal error has occured during the startup process of the configurator.\n\nThe configurator was unable to open the required registry keys.",
                        ex);
                    return;
                }
            }
        }

        public static bool TLS12Available()
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                return true;
            } catch { }
            finally
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            }

            return false;
        }

        /// <summary>
        /// Shows a messagebox.
        /// </summary>
        /// <param name="Type">Type of error. 0 = Info, 1 = Question, 2 = Warning (OK), 3 = Warning (YesNo), 4 = Error (OK), 5 = Error (YesNo)</param>
        public static DialogResult ShowError(Int32 Type, String Title, String Msg, Exception Ex)
        {
            String UMsg = Msg;
            String UTitle = String.Format("OmniMIDI - {0}", Title);
            MessageBoxButtons Btns;
            MessageBoxIcon Icn;

            if (Ex != null)
                UMsg += String.Format("\n\nException:\n{0}", Ex.ToString());

            switch (Type)
            {
                case 0:
                    Btns = MessageBoxButtons.OK;
                    Icn = MessageBoxIcon.Information;
                    break;
                case 1:
                    Btns = MessageBoxButtons.YesNo;
                    Icn = MessageBoxIcon.Question;
                    break;
                case 2:
                    Btns = MessageBoxButtons.OK;
                    Icn = MessageBoxIcon.Warning;
                    break;
                case 3:
                    Btns = MessageBoxButtons.YesNo;
                    Icn = MessageBoxIcon.Warning;
                    break;
                case 4:
                    Btns = MessageBoxButtons.OK;
                    Icn = MessageBoxIcon.Hand;
                    break;
                case 5:
                    Btns = MessageBoxButtons.YesNo;
                    Icn = MessageBoxIcon.Hand;
                    break;
                default:
                    Btns = MessageBoxButtons.OK;
                    Icn = MessageBoxIcon.None;
                    break;

            }

            return MessageBox.Show(UMsg, UTitle, Btns, Icn);
        }
    }

    static class AudioEngine
    {
        // Internal
        public const int ENCODING_MODE = 0;
        public const int DSOUND_OR_WASAPI = 1;
        public const int PRO_INTERFACE = 2;

        // Explicit names
        public const int AUDTOWAV = 0;
        public const int DSOUND_ENGINE = 1;
        public const int ASIO_ENGINE = 2;
        public const int WASAPI_ENGINE = 3;
    }

}
