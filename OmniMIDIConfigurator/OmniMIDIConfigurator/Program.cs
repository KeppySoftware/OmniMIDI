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
        public static RegistryKey Channels = null;
        public static RegistryKey ChanOverride = null;
        public static RegistryKey SynthSettings = null;
        public static RegistryKey Mapper = null;
        public static RegistryKey Watchdog = null;

        public const string HKCU = "HKEY_CURRENT_USER\\";
        public const string MIPath = "SOFTWARE\\OmniMIDI";
        public const string CHPath = "SOFTWARE\\OmniMIDI\\Channels";
        public const string COPath = "SOFTWARE\\OmniMIDI\\ChanOverride";
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

            if (!Directory.Exists(Path.GetDirectoryName(Program.ListsPath[0])))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Program.ListsPath[0]));
            }

            foreach (String Arg in Args)
            {
                switch (Arg.ToLowerInvariant())
                {
                    case "/troubleshoot":
                        Functions.ResetFaultTolerantHeap();
                        Functions.ResetAppCompat();
                        SynthSettings = Registry.CurrentUser.OpenSubKey(SSPath, true);
                        if (SynthSettings == null)
                        {
                            OpenRequiredKey(ref Mixer, MIPath, true);
                            OpenRequiredKey(ref Channels, CHPath, true);
                            OpenRequiredKey(ref ChanOverride, COPath, true);
                            OpenRequiredKey(ref SynthSettings, SSPath, true);
                            OpenRequiredKey(ref Mapper, MPPath, true);
                            OpenRequiredKey(ref Watchdog, WPath, true);
                            Functions.ResetDriverSettings();
                        }
                        var c1 = Process.GetCurrentProcess();
                        Process.GetProcessesByName(c1.ProcessName)
                            .Where(t => t.Id != c1.Id)
                            .ToList()
                            .ForEach(t => t.Kill());

                        Properties.Settings.Default.Reset();
                        UpdateSystem.CheckForTLS12ThenUpdate(Driver.FileVersion, UpdateSystem.WIPE_SETTINGS);
                        return;
                    case "/prepare":
                        SynthSettings = Registry.CurrentUser.OpenSubKey(SSPath, true);
                        if (SynthSettings == null)
                        {
                            OpenRequiredKey(ref Mixer, MIPath, true);
                            OpenRequiredKey(ref Channels, CHPath, true);
                            OpenRequiredKey(ref ChanOverride, COPath, true);
                            OpenRequiredKey(ref SynthSettings, SSPath, true);
                            OpenRequiredKey(ref Mapper, MPPath, true);
                            OpenRequiredKey(ref Watchdog, WPath, true);
                            Functions.ResetDriverSettings();
                        }
                        return;
                    case "/rei":
                        var c2 = Process.GetCurrentProcess();
                        Process.GetProcessesByName(c2.ProcessName)
                            .Where(t => t.Id != c2.Id)
                            .ToList()
                            .ForEach(t => t.Kill());

                        Properties.Settings.Default.Reset();
                        UpdateSystem.CheckForTLS12ThenUpdate(Driver.FileVersion, UpdateSystem.WIPE_SETTINGS);
                        return;
                    case "/showchangelog":
                        if (Properties.Settings.Default.ShowChangelogStartUp)
                        {
                            try
                            {
                                ChangelogWindow F = new ChangelogWindow(Driver.FileVersion.ToString(), true);
                                F.FormBorderStyle = FormBorderStyle.FixedDialog;
                                F.ShowIcon = true;
                                F.ShowInTaskbar = true;
                                F.ShowDialog();
                                F.Dispose();
                            }
                            catch { }
                        }
                        break;
                    default:
                        SoundFontsToAdd.Add(Arg);
                        break;
                }
            }

            new Drv32Troubleshooter(true).ShowDialog();

            if (Properties.Settings.Default.UpdateBranch == "choose" || 
                (Properties.Settings.Default.UpdateBranch != Properties.Settings.Default.PreReleaseBranch[1] &&
                Properties.Settings.Default.UpdateBranch != Properties.Settings.Default.StableBranch[1] &&
                Properties.Settings.Default.UpdateBranch != Properties.Settings.Default.SlowBranch[1]))
            {
                SelectBranch frm = new SelectBranch();
                frm.ShowInTaskbar = true;
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.ShowDialog();
                frm.Dispose();
            }

            OpenRequiredKey(ref Mixer, MIPath, false);
            OpenRequiredKey(ref Channels, CHPath, false);
            OpenRequiredKey(ref ChanOverride, COPath, false);
            OpenRequiredKey(ref SynthSettings, SSPath, false);
            OpenRequiredKey(ref Mapper, MPPath, false);
            OpenRequiredKey(ref Watchdog, WPath, false);

            CheckDumpFiles();

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

            // Donation dialog
            DateTime CD = DateTime.Now;
            Double D = (CD.Date - Properties.Settings.Default.DonationShownWhen).TotalDays;
            if (D > 30 && !Properties.Settings.Default.DoNotShowDonation)
                new Donate().ShowDialog();

            Application.Run(new MainWindow(SoundFontsToAdd.ToArray()));
        }

        private static void CheckDumpFiles()
        {
            String DirectoryDebug = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\OmniMIDI\\dumpfiles\\";

            if (Directory.GetFiles(DirectoryDebug, "*.mdmp").Length != 0)
            {
                DialogResult W = MessageBox.Show("OmniMIDI has found some minidumps that might have been created after some driver crashes.\n\n" +
                    "Press Yes to open the minidumps folder, No to delete them, or Cancel to ignore.",
                    "OmniMIDI - Minidumps found", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                switch (W)
                {
                    case DialogResult.Yes:
                        Process.Start("explorer.exe", DirectoryDebug);
                        break;

                    case DialogResult.No:
                        DirectoryInfo DDI = new DirectoryInfo(DirectoryDebug);

                        foreach (FileInfo DumpFile in DDI.GetFiles())
                            DumpFile.Delete();

                        break;

                    default:
                        break;
                }
            }
        }

        private static bool OpenRequiredKey(ref RegistryKey Key, String KeyPath, Boolean Silent)
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
                    if (!Silent)
                    {
                        Program.ShowError(
                            4,
                            "FATAL ERROR",
                            "A fatal error has occured during the startup process of the configurator.\n\nThe configurator was unable to open the required registry keys.",
                            ex);
                    }
                    return false;
                }
            }

            return true;
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
        public const int BASS_OUTPUT = 1;
        public const int ASIO_ENGINE = 2;
        public const int WASAPI_ENGINE = 3;
        public const int XA_ENGINE = 4;
    }

}
