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

        public static uint BringToFrontMessage;
        static void DoAnyway(String[] args)
        {
            RegistryKey SynthSettings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Settings", true);
            int runmode = 0;
            bool ok;
            BringToFrontMessage = WinAPI.RegisterWindowMessage("KeppySynthConfiguratorToFront");
            Mutex m = new Mutex(true, "KeppySynthConfigurator", out ok);
            if (!ok)
            {
                WinAPI.PostMessage((IntPtr)WinAPI.HWND_BROADCAST, BringToFrontMessage,IntPtr.Zero,IntPtr.Zero);
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
                            break;
                        default:
                            runmode = 0;
                            break;
                    }
                }

                if (runmode == 0)
                {
                    if (SynthSettings.GetValue("autoupdatecheck", "1").ToString() == "1")
                    {
                        if (Functions.IsInternetAvailable() == false)
                        {
                            MessageBox.Show("The configurator can not connect to the GitHub servers.\n\nCheck your network connection, or contact your system administrator or network service provider.\n\nPress OK to continue and open the configurator's window.", "Keppy's Synthesizer - Connection error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            try
                            {
                                WebClient client = new WebClient();
                                Stream stream = client.OpenRead("https://raw.githubusercontent.com/KaleidonKep99/Keppy-s-Driver/master/output/keppydriverupdate.txt");
                                StreamReader reader = new StreamReader(stream);
                                String newestversion = reader.ReadToEnd();
                                FileVersionInfo Driver = FileVersionInfo.GetVersionInfo(Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\keppysynth\\keppysynth.dll");
                                Version x = null;
                                Version.TryParse(newestversion.ToString(), out x);
                                Version y = null;
                                Version.TryParse(Driver.FileVersion.ToString(), out y);
                                Thread.Sleep(50);
                                if (x > y)
                                {
                                    DialogResult dialogResult = MessageBox.Show("A new update for Keppy's Synthesizer has been found.\n\nVersion installed: " + Driver.FileVersion.ToString() + "\nVersion available online: " + newestversion.ToString() + "\n\nWould you like to update now?\nIf you choose \"Yes\", the configurator will be automatically closed.\n\n(You can disable the automatic update check through the advanced settings.)", "New version of Keppy's Synthesizer found", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    if (dialogResult == DialogResult.Yes)
                                    {
                                        Forms.KeppySynthDLEngine frm = new Forms.KeppySynthDLEngine(newestversion, String.Format("Downloading update {0}, please wait... {1}%", newestversion, "{0}"), null, 0);
                                        frm.StartPosition = FormStartPosition.CenterScreen;
                                        frm.ShowDialog();
                                    }
                                }
                            }
                            catch
                            {
                                MessageBox.Show("Unknown error.\n\nPress OK to continue and open the configurator's window.", "Keppy's Synthesizer - Connection error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
                Application.EnableVisualStyles();
                SynthSettings.Close();
                Application.Run(new KeppySynthConfiguratorMain(args));
                GC.KeepAlive(m);
            }
            catch
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new KeppySynthConfiguratorMain(args));
                GC.KeepAlive(m);
            }
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

        public static void UpdateTextPosition(Form form)
        {
            Version win8version = new Version(6, 2, 9200, 0);
            Version win81version = new Version(6, 3, 9200, 0);
            Version win81u1version = new Version(6, 3, 9600, 0);

            if (Environment.OSVersion.Platform == PlatformID.Win32NT &&
                Environment.OSVersion.Version != win8version | Environment.OSVersion.Version != win81version | Environment.OSVersion.Version != win81u1version)
            {
                Graphics g = form.CreateGraphics();
                Double startingPoint = (form.Width / 2) - (g.MeasureString(form.Text.Trim(), form.Font).Width / 2);
                Double widthOfASpace = g.MeasureString(" ", form.Font).Width;
                String tmp = " ";
                Double tmpWidth = 0;

                while ((tmpWidth + widthOfASpace) < startingPoint)
                {
                    tmp += " ";
                    tmpWidth += widthOfASpace;
                }

                form.Text = tmp + form.Text.Trim();
            }
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
