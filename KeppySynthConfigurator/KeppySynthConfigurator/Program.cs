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
            try
            {
                RegistryKey rkCurrentUser = Registry.CurrentUser;
                CopyKey(rkCurrentUser, "SOFTWARE\\Keppy's Driver", "SOFTWARE\\Keppy's Synthesizer");
                Directory.Move(System.Environment.GetEnvironmentVariable("USERPROFILE").ToString() + "\\Keppy's Driver\\", System.Environment.GetEnvironmentVariable("USERPROFILE").ToString() + "\\Keppy's Synthesizer\\");
                Directory.Delete(System.Environment.GetEnvironmentVariable("USERPROFILE").ToString() + "\\Keppy's Driver\\");
                DoAnyway(args);
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
                DoAnyway(args);
            }
        }

        public static void DebugToConsole(bool isException, String message, Exception ex)
        {
            String CurrentTime = DateTime.Now.ToString("HH:mm:ss.fff");
            try
            {
                if (isException)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(String.Format("{0}", CurrentTime));
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(String.Format(" - Exception {0}", ex));
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
                if (!Functions.IsWindowsVistaOrNewer())
                {
                    Functions.ShowErrorDialog(Properties.Resources.erroricon, System.Media.SystemSounds.Hand, "Fatal error", "Windows XP is not supported.", true, null);
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
                Application.SetCompatibleTextRenderingDefault(false);
                try
                {
                    foreach (String s in args)
                    {
                        switch (s.Substring(0, 4).ToUpper())
                        {
                            case "/ASP":
                                runmode = 1;
                                window = 0;
                                break;
                            case "/REI":
                                RegistryKey sourceKey = Registry.CurrentUser.OpenSubKey("SOFTWARE", true);
                                sourceKey.DeleteSubKeyTree("Keppy's Synthesizer", true);
                                sourceKey.Close();
                                Functions.CheckForUpdates(true, true);
                                return;
                            case "/INF":
                                runmode = 2;
                                window = 1;
                                break;
                            case "/DBG":
                                runmode = 0;
                                window = 0;
                                AllocConsole();
                                break;
                            default:
                                runmode = 0;
                                window = 0;
                                break;
                        }
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

        public static void ExecuteForm(Int32 runmode, String[] args, Mutex m, Int32 form)
        {
            if (runmode == 0)
            {
                Functions.CheckForUpdates(false, true);
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
