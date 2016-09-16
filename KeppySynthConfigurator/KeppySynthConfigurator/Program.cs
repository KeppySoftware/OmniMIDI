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

namespace KeppySynthConfigurator
{
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

        static void DoAnyway(String[] args)
        {
            RegistryKey SynthSettings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Settings", true);
            int runmode = 0;
            bool ok;
            Mutex m = new Mutex(true, "KeppySynthConfigurator", out ok);
            if (!ok)
            {
                return;
            }
            try
            {
                foreach (String s in args)
                {
                    switch (s.Substring(0, 4).ToUpper())
                    {
                        default:
                            runmode = 0;
                            break;
                    }
                }
                if (runmode == 0)
                {
                    if (SynthSettings.GetValue("autoupdatecheck", 1).ToString() == "1")
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
                                Process.Start("https://github.com/KaleidonKep99/Keppy-s-Driver/releases");
                                Application.ExitThread();
                            }
                        }
                    }
                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                SynthSettings.Close();
                Application.Run(new KeppySynthConfiguratorMain(args));
                GC.KeepAlive(m);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error as occurred while starting the configurator.\n\nMore informations down below:\n" + ex.ToString() + "\n\nThe configurator can still be used, press OK to continue", "Keppy's Synthesizer ~ Error" , MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
