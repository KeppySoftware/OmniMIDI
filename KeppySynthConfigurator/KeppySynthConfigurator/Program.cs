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

namespace KeppySynthConfigurator
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
                    AllocConsole();
                    break;
                }
            }
            if (!File.Exists(String.Format("{0}\\keppysynth\\bass.dll", Environment.GetFolderPath(Environment.SpecialFolder.SystemX86))) ||
                !File.Exists(String.Format("{0}\\keppysynth\\bassmidi.dll", Environment.GetFolderPath(Environment.SpecialFolder.SystemX86))))
            {
                MissingBASSLibs MissingBASSLib = new MissingBASSLibs("The system was unable to find the required BASS libraries");
                MissingBASSLib.Source = "BASS libraries not found";
                DebugToConsole(true, "Can not find BASS libraries", MissingBASSLib);
                MessageBox.Show("Can not find the required BASS libraries for the configurator to work.\nEnsure that BASS.DLL and BASSMIDI.DLL are present in the configurator's root folder.\nIf they're not, please reinstall the synthesizer.\n\nClick OK to close the configurator.", "Keppy's Synthesizer ~ Configurator - Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            try
            {
                RegistryKey rkCurrentUser = Registry.CurrentUser;
                CopyKey(rkCurrentUser, "SOFTWARE\\Keppy's Driver", "SOFTWARE\\Keppy's Synthesizer");
                Directory.Move(System.Environment.GetEnvironmentVariable("USERPROFILE").ToString() + "\\Keppy's Driver\\", System.Environment.GetEnvironmentVariable("USERPROFILE").ToString() + "\\Keppy's Synthesizer\\");
                Directory.Delete(System.Environment.GetEnvironmentVariable("USERPROFILE").ToString() + "\\Keppy's Driver\\");
            }
            catch
            {
                RegistryKey sourceKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Driver", true);
                if (sourceKey != null)
                {
                    RegistryKey deleteme = Registry.CurrentUser.OpenSubKey("SOFTWARE", true);
                    deleteme.DeleteSubKeyTree("Keppy's Driver");
                    deleteme.Close();
                    sourceKey.Close();
                }
            }
            finally
            {
                DoAnyway(args);
            }
        }

        public static void DebugToConsole(bool isException, String message, Exception ex)
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

        public static uint BringToFrontMessage;
        static void DoAnyway(String[] args)
        {
            try
            {
                DebugToConsole(false, "Started configurator.", null);
                Application.SetCompatibleTextRenderingDefault(false);
                if (!Functions.IsWindowsVistaOrNewer())
                {
                    Functions.ShowErrorDialog(1, System.Media.SystemSounds.Hand, "Fatal error", "Windows XP is not supported.", true, null);
                    Application.ExitThread();
                }
                int runmode = 0;
                int window = 0;
                bool ok;
                BringToFrontMessage = WinAPI.RegisterWindowMessage("KeppySynthConfiguratorToFront");
                Mutex m = new Mutex(true, "KeppySynthConfigurator", out ok);
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
                        if (s.ToLowerInvariant() == "/asp")
                        {
                            Functions.UserProfileMigration();
                            return;
                        }
                        else if (s.ToLowerInvariant() == "/rei")
                        {
                            RegistryKey sourceKey = Registry.CurrentUser.OpenSubKey("SOFTWARE", true);
                            sourceKey.DeleteSubKeyTree("Keppy's Synthesizer", true);
                            sourceKey.Close();
                            UpdateSystem.CheckForUpdates(true, true);
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
                    if (Properties.Settings.Default.UpdateBranch == "choose")
                    {
                        MessageBox.Show("The driver's update system is divided into three branches. For future update notifications, you can pick between the following branches, which are the Canary Branch, the Normal Branch, and the Delayed Branch. These preferences can be changed at any time.\n\nCanary Branch: all updates\nNormal Branch: occasional updates (default, recommended)\nDelayed Branch: very infrequent updates (not recommended)\n\nClick OK to choose a branch.", "Keppy's Synthesizer - New update branches", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    Application.Run(new KeppySynthConfiguratorMain(args));
                    GC.KeepAlive(m);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public static void TriggerDate()
        {
            DateTime BirthDate = DateTime.Now;
            Int32 CurrentYear = Convert.ToInt32(BirthDate.ToString("yyyy"));
            Int32 YearsOld = (CurrentYear - 2015);
            if (BirthDate.ToString("dd/MM") == "17/05")
                MessageBox.Show(String.Format("Today, Keppy's Synthesizer turned {0} years old!\n\nThank you fellow user for using it and helping me with the development, and happy anniversary, Keppy's Synthesizer!", YearsOld.ToString()), String.Format("{0} anniversary since the first release of Keppy's Synthesizer", Ordinal(YearsOld)), MessageBoxButtons.OK, MessageBoxIcon.Information);
            else if (BirthDate.ToString("dd/MM") == "05/12")
                MessageBox.Show("Today is Keppy's birthday! He turned " + (YearsOld).ToString() + " years old!\n\nHappy birthday, you potato!", "Happy birthday to Kepperino", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        public static void ExecuteForm(Int32 runmode, String[] args, Mutex m, Int32 form)
        {
            if (runmode == 0)
            {
                UpdateSystem.CheckForUpdates(false, true);
            }
            Application.EnableVisualStyles();
            if (form == 0)
                Application.Run(new KeppySynthConfiguratorMain(args));
            else if (form == 1)
                Application.Run(new InfoDialog(1));
            GC.KeepAlive(m);
        }

        public static bool CopyKey(RegistryKey parentKey, string keyNameToCopy, string newKeyName)
        {
            RegistryKey destinationKey = parentKey.CreateSubKey(newKeyName);
            RegistryKey sourceKey = parentKey.OpenSubKey(keyNameToCopy, true);
            RecurseCopyKey(sourceKey, destinationKey);
            sourceKey.DeleteSubKey(keyNameToCopy);
            destinationKey.Close();
            sourceKey.Close();
            return true;
        }

        private static void RecurseCopyKey(RegistryKey sourceKey, RegistryKey destinationKey)
        {
            foreach (string valueName in sourceKey.GetValueNames())
            {
                object objValue = sourceKey.GetValue(valueName);
                RegistryValueKind valKind = sourceKey.GetValueKind(valueName);
                destinationKey.SetValue(valueName, objValue, valKind);
            }

            foreach (string sourceSubKeyName in sourceKey.GetSubKeyNames())
            {
                RegistryKey sourceSubKey = sourceKey.OpenSubKey(sourceSubKeyName);
                RegistryKey destSubKey = destinationKey.CreateSubKey(sourceSubKeyName);
                RecurseCopyKey(sourceSubKey, destSubKey);
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