using System.Threading;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Linq;

namespace OmniMIDIConfigurator
{
    static class WinAPI
    {
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern UInt32 RegisterWindowMessage(string lpString);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern Boolean PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern Boolean ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        public static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        public const uint HWND_BROADCAST = 0xFFFF;
        public const short SW_RESTORE = 9;
    }

    static class KDMAPI
    {
        // KSDAPI info
        [DllImport("OmniMIDI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern Boolean ReturnKDMAPIVer(out Int32 Major, out Int32 Minor, out Int32 Build, out Int32 Revision);

        [DllImport("OmniMIDI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void InitializeKDMAPIStream();

        [DllImport("OmniMIDI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void TerminateKDMAPIStream();

        [DllImport("OmniMIDI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void ResetKDMAPIStream();

        [DllImport("OmniMIDI.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool IsKDMAPIAvailable();

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

    static class SecurityProtocolNET45
    {
        public static SecurityProtocolType Tls12 = (SecurityProtocolType)3072;
    }

    static class Program
    {
        public static bool DebugMode = false;

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        /// 
        [STAThread]
        static void Main(String[] args)
        {
            foreach (String s in args)
            {
                if (s.ToLowerInvariant() == "/dbg" || s.ToLowerInvariant() == "/debugwindow")
                {
                    DebugMode = true;
                    AllocConsole();
                    break;
                }
            }
            if (!File.Exists(String.Format("{0}\\OmniMIDI\\bass.dll", Environment.GetFolderPath(Environment.SpecialFolder.SystemX86))) ||
                !File.Exists(String.Format("{0}\\OmniMIDI\\bassmidi.dll", Environment.GetFolderPath(Environment.SpecialFolder.SystemX86))))
            {
                DebugToConsole(false, "Can not find BASS libraries, trying to sideload them from the local directory...", null);
                if (!File.Exists(String.Format("bass.dll")) ||
                    !File.Exists(String.Format("bassmidi.dll")))
                {
                    MissingBASSLibs MissingBASSLib = new MissingBASSLibs("The system was unable to find the required BASS libraries");
                    MissingBASSLib.Source = "BASS libraries not found";
                    DebugToConsole(true, "Can not find BASS libraries", MissingBASSLib);
                    MessageBox.Show("Can not find the required BASS libraries for the configurator to work.\nEnsure that BASS.DLL and BASSMIDI.DLL are present in the configurator's root folder.\nIf they're not, please reinstall the synthesizer.\n\nClick OK to close the configurator.", "OmniMIDI ~ Configurator - Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
            }

            UpdateToOmniMIDI();
            DoAnyway(args);
        }

        public static string Truncate(this string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
        }

        public static void ShowLastMessage(String message, Boolean isException)
        {
            try
            {
                OmniMIDIConfiguratorMain.Delegate.Status.Text = String.Format(OmniMIDIConfiguratorMain.StatusTemplate, message).Truncate(90);
                if (isException)
                {
                    OmniMIDIConfiguratorMain.Delegate.StatusDoneOr.Text = "Exception";
                    OmniMIDIConfiguratorMain.Delegate.StatusDoneOr.ForeColor = Color.DarkRed;
                }
                else
                {
                    OmniMIDIConfiguratorMain.Delegate.StatusDoneOr.Text = "OK";
                    OmniMIDIConfiguratorMain.Delegate.StatusDoneOr.ForeColor = Color.DarkGreen;
                }
            }
            catch { }
        }

        public static void DebugToConsole(bool isException, String message, Exception ex)
        {
            ShowLastMessage(message, isException);
            if (DebugMode == true)
            {
                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("en-US");
                String CurrentTime = DateTime.Now.ToString("MMMM dd, yyyy | hh:mm:ss.fff tt", ci);
                try
                {
                    if (isException)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(String.Format("{0}", CurrentTime));
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(String.Format(" - {0}", ex));
                        Console.Write(Environment.NewLine);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(String.Format("{0}", CurrentTime));
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(String.Format(" - {0}", message));
                        Console.Write(Environment.NewLine);
                    }
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(" - Something went wrong while displaying the exception.");
                    Console.Write(Environment.NewLine);
                }
            }
        }

        public static uint BringToFrontMessage;
        static EventWaitHandle m;
        static void DoAnyway(String[] args)
        {
            try
            {
                DebugToConsole(false, "Started configurator.", null);

                // Parse KDMAPI version
                Int32 Major, Minor, Build, Revision;
                if (KDMAPI.ReturnKDMAPIVer(out Major, out Minor, out Build, out Revision))
                    KDMAPI.KDMAPIVer = String.Format("{0}.{1}.{2} (Revision {3})", Major, Minor, Build, Revision);
                else
                {
                    MessageBox.Show("Failed to initialize KDMAPI!\n\nPress OK to quit.", "OmniMIDI ~ Configurator | FATAL ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.ExitThread();
                }
                    
                Application.SetCompatibleTextRenderingDefault(false);
                if (!Functions.IsWindowsVistaOrNewer()) Application.ExitThread();

                int runmode = 0;
                int window = 0;
                bool ok;

                BringToFrontMessage = WinAPI.RegisterWindowMessage("OmniMIDIConfiguratorToFront");
                m = new EventWaitHandle(false, EventResetMode.ManualReset, "OmniMIDIConfigurator", out ok);
                if (!ok)
                {
                    WinAPI.PostMessage((IntPtr)WinAPI.HWND_BROADCAST, BringToFrontMessage, IntPtr.Zero, IntPtr.Zero);
                    return;
                }

                TriggerDate();

                try
                {
                    foreach (String s in args)
                    {
                        if (s.ToLowerInvariant() == "/rei")
                        {
                            TLS12Enable(true);

                            FileVersionInfo Driver = FileVersionInfo.GetVersionInfo(UpdateSystem.UpdateFileVersion);        

                            var current = Process.GetCurrentProcess();
                            Process.GetProcessesByName(current.ProcessName)
                                .Where(t => t.Id != current.Id)
                                .ToList()
                                .ForEach(t => t.Kill());

                            UpdateSystem.CheckForTLS12ThenUpdate(Driver.FileVersion, UpdateSystem.WIPE_SETTINGS);
                            return;
                        }
                        else if (s.ToLowerInvariant() == "/toomni")
                        {
                            UpdateToOmniMIDI();
                            return;
                        }
                        else if (s.ToLowerInvariant() == "/inf")
                        {
                            runmode = 2;
                            window = 1;
                            break;
                        }
                        else
                        {
                            runmode = 0;
                            window = 0;
                            break;
                        }
                    }

                    TLS12Enable(false);
                    if (Properties.Settings.Default.UpdateBranch == "choose")
                    {
                        SelectBranch frm = new SelectBranch();
                        frm.ShowInTaskbar = true;
                        frm.StartPosition = FormStartPosition.CenterScreen;
                        frm.ShowDialog();
                        frm.Dispose();
                    }

                    ExecuteForm(runmode, args, m, window);
                }
                catch
                {
                    Application.EnableVisualStyles();
                    Application.Run(new OmniMIDIConfiguratorMain(args));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public static void TLS12Enable(Boolean HideMessage)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolNET45.Tls12;
                Properties.Settings.Default.TLS12Missing = false;
            }
            catch
            {
                if (!Properties.Settings.Default.TLS12Missing && !HideMessage)
                    MessageBox.Show("Your .NET Framework doesn't seem to support TLS 1.2 encryption." +
                                    "\nThis might prevent the configurator from downloading the required update files." +
                                    "\n\nPlease install .NET Framework 4.5, for seamless updates.", "OmniMIDI - TLS 1.2 protocol not found",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Properties.Settings.Default.TLS12Missing = true;
            }
            finally
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            }
        }

        public static void TriggerDate()
        {
            DateTime BirthDate = DateTime.Now;
            Int32 CurrentYear = Convert.ToInt32(BirthDate.ToString("yyyy"));
            Int32 YearsOld = (CurrentYear - 2015);
            if (BirthDate.ToString("dd/MM") == "17/05")
                MessageBox.Show(String.Format("Today, OmniMIDI turned {0} years old!\n\nThank you fellow user for using it and helping me with the development, and happy anniversary, OmniMIDI!", (CurrentYear - 2015).ToString()), String.Format("{0} anniversary since the first release of OmniMIDI", Ordinal(YearsOld)), MessageBoxButtons.OK, MessageBoxIcon.Information);
            else if (BirthDate.ToString("dd/MM") == "05/12")
                MessageBox.Show(String.Format("Today is Keppy's birthday! He turned {0} years old!\n\nHappy birthday, you potato!", (CurrentYear - 1999).ToString()), "Happy birthday to Kepperino", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static string Ordinal(int number)
        {
            string suffix = String.Empty;

            int ones = number % 10;
            int tens = (int)Math.Floor(number / 10M) % 10;

            if (tens == 1)
            {
                suffix = "th";
            }
            else
            {
                switch (ones)
                {
                    case 1:
                        suffix = "st";
                        break;
                    case 2:
                        suffix = "nd";
                        break;
                    case 3:
                        suffix = "rd";
                        break;
                    default:
                        suffix = "th";
                        break;
                }
            }
            return String.Format("{0}{1}", number, suffix);
        }

        public static void ExecuteForm(Int32 runmode, String[] args, EventWaitHandle m, Int32 form)
        {
            Application.EnableVisualStyles();
            if (form == 0)
                Application.Run(new OmniMIDIConfiguratorMain(args));
            else if (form == 1)
                Application.Run(new InfoDialog(1));
            GC.KeepAlive(m);
        }

        public static void UpdateToOmniMIDI()
        {
            String UPSource = Environment.GetEnvironmentVariable("USERPROFILE") + "\\Keppy's Synthesizer\\";
            String UPDestination = Environment.GetEnvironmentVariable("USERPROFILE") + "\\OmniMIDI\\";

            String[] OldLists =
            {
                UPDestination + "lists\\keppymidi.sflist",
                UPDestination + "lists\\keppymidib.sflist",
                UPDestination + "lists\\keppymidic.sflist",
                UPDestination + "lists\\keppymidid.sflist",
                UPDestination + "lists\\keppymidie.sflist",
                UPDestination + "lists\\keppymidif.sflist",
                UPDestination + "lists\\keppymidig.sflist",
                UPDestination + "lists\\keppymidih.sflist",
                UPDestination + "lists\\keppymidii.sflist",
                UPDestination + "lists\\keppymidij.sflist",
                UPDestination + "lists\\keppymidik.sflist",
                UPDestination + "lists\\keppymidil.sflist",
                UPDestination + "lists\\keppymidim.sflist",
                UPDestination + "lists\\keppymidin.sflist",
                UPDestination + "lists\\keppymidio.sflist",
                UPDestination + "lists\\keppymidip.sflist",
            };

            RegistryKey Source = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Keppy's Synthesizer");
            if (Source != null)
            {
                RegistryKey Destination = Registry.CurrentUser.CreateSubKey("SOFTWARE\\OmniMIDI");
                Source.CopyTo(Destination);

                Source.Close();
                Destination.Close();

                try { Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Keppy's Synthesizer", true); } catch { }
            }

            if (Directory.Exists(UPSource))
            {
                try
                {
                    Directory.Delete(UPDestination, true);
                    Directory.Move(UPSource, UPDestination);
                    Directory.Delete(UPSource, true);
                }
                catch { }
            }

            // Some files
            try { File.Move(UPDestination + "\\keppymididrv.favlist", UPDestination + "\\OmniMIDI.favlist"); } catch { }
            try { File.Move(UPDestination + "\\blacklist\\keppymididrv.blacklist", UPDestination + "\\blacklist\\OmniMIDI.blacklist"); } catch { }

            // SF lists
            for (int i = 0; i < OldLists.Length; i++)
            {
                try { File.Move(OldLists[i], OmniMIDIConfiguratorMain.ListsPath[i]); } catch { }
                try { File.Delete(String.Format("{0}{1}", UPDestination, Path.GetFileName(OldLists[i]))); } catch { }
            }
        }

        public static void CopyTo(this RegistryKey src, RegistryKey dst)
        {
            // copy the values
            foreach (var name in src.GetValueNames())
            {
                dst.SetValue(name, src.GetValue(name), src.GetValueKind(name));
            }

            // copy the subkeys
            foreach (var name in src.GetSubKeyNames())
            {
                using (var srcSubKey = src.OpenSubKey(name, false))
                {
                    var dstSubKey = dst.CreateSubKey(name);
                    srcSubKey.CopyTo(dstSubKey);
                }
            }
        }
    }
}

public class MissingBASSLibs : Exception
{
    public MissingBASSLibs()
    {
    }

    public MissingBASSLibs(string message)
        : base(message)
    {
    }

    public MissingBASSLibs(string message, Exception inner)
        : base(message, inner)
    {
    }
}