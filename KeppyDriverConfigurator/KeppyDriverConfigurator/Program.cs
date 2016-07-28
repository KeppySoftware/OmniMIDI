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

namespace KeppyDriverConfigurator
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
            RegistryKey SynthSettings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Driver\\Settings", true);
            bool ok;
            Mutex m = new Mutex(true, "KeppyDriverConfigurator", out ok);
            if (!ok)
            {
                return;
            }
            try
            {
                if (Convert.ToInt32(SynthSettings.GetValue("autoupdatecheck", 1)) == 1)
                {
                    WebClient client = new WebClient();
                    Stream stream = client.OpenRead("https://raw.githubusercontent.com/KaleidonKep99/Keppy-s-Driver/master/output/keppydriverupdate.txt");
                    StreamReader reader = new StreamReader(stream);
                    String newestversion = reader.ReadToEnd();
                    FileVersionInfo Driver = FileVersionInfo.GetVersionInfo(Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\keppydrv\\keppydrv.dll");
                    Version x = null;
                    Version.TryParse(newestversion.ToString(), out x);
                    Version y = null;
                    Version.TryParse(Driver.FileVersion.ToString(), out y);
                    Thread.Sleep(50);
                    if (x > y)
                    {
                        DialogResult dialogResult = MessageBox.Show("A new update for Keppy's Driver has been found.\n\nVersion installed: " + Driver.FileVersion.ToString() + "\nVersion available online: " + newestversion.ToString() + "\n\nWould you like to update now?\nIf you choose \"Yes\", the configurator will be automatically closed.\n\n(You can disable the automatic update check through the advanced settings.)", "New version of Keppy's Driver found", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dialogResult == DialogResult.Yes)
                        {
                            Process.Start("https://github.com/KaleidonKep99/Keppy-s-Driver/releases");
                            Application.ExitThread();
                        }
                        else if (dialogResult == DialogResult.No)
                        {
                            SynthSettings.Close();
                            Application.EnableVisualStyles();
                            Application.SetCompatibleTextRenderingDefault(false);
                            Application.Run(new KeppyDriverConfiguratorMain(args));
                            GC.KeepAlive(m);
                        }
                    }
                    else
                    {
                        SynthSettings.Close();
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new KeppyDriverConfiguratorMain(args));
                        GC.KeepAlive(m);
                    }
                }
                else
                {
                    SynthSettings.Close();
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new KeppyDriverConfiguratorMain(args));
                    GC.KeepAlive(m);
                }
            }
            catch
            {
                SynthSettings.Close();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new KeppyDriverConfiguratorMain(args));
                GC.KeepAlive(m);
            }
        }
    }
}
