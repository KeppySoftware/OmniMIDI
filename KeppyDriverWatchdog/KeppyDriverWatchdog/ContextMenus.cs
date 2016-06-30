using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using System.Timers;
using Microsoft.Win32;

namespace KeppyDriverWatchdog
{
    class ContextMenus
    {
        public ContextMenuStrip Create()
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem item;
            ToolStripSeparator sep;

            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(CheckPop);
            aTimer.Interval = 1;
            aTimer.Enabled = true;

            item = new ToolStripMenuItem();
            item.Text = "Open the configurator";
            item.Click += new EventHandler(OpenConf_Click);
            menu.Items.Add(item);

            sep = new ToolStripSeparator();
            menu.Items.Add(sep);

            item = new ToolStripMenuItem();
            item.Text = "Reload list 1";
            item.Click += new EventHandler(SoundfontReload1);
            menu.Items.Add(item);

            item = new ToolStripMenuItem();
            item.Text = "Reload list 2";
            item.Click += new EventHandler(SoundfontReload2);
            menu.Items.Add(item);

            item = new ToolStripMenuItem();
            item.Text = "Reload list 3";
            item.Click += new EventHandler(SoundfontReload3);
            menu.Items.Add(item);

            item = new ToolStripMenuItem();
            item.Text = "Reload list 4";
            item.Click += new EventHandler(SoundfontReload4);
            menu.Items.Add(item);

            item = new ToolStripMenuItem();
            item.Text = "Reload list 5";
            item.Click += new EventHandler(SoundfontReload5);
            menu.Items.Add(item);

            item = new ToolStripMenuItem();
            item.Text = "Reload list 6";
            item.Click += new EventHandler(SoundfontReload6);
            menu.Items.Add(item);

            item = new ToolStripMenuItem();
            item.Text = "Reload list 7";
            item.Click += new EventHandler(SoundfontReload7);
            menu.Items.Add(item);

            item = new ToolStripMenuItem();
            item.Text = "Reload list 8";
            item.Click += new EventHandler(SoundfontReload8);
            menu.Items.Add(item);

            return menu;
        }

        void OpenConf_Click(object sender, EventArgs e)
        {
            Process.Start("KeppyDriverConfigurator", null);
        }

        void SoundfontReload1(object sender, EventArgs e)
        {
            LoadSoundfont(1);
        }

        void SoundfontReload2(object sender, EventArgs e)
        {
            LoadSoundfont(2);
        }

        void SoundfontReload3(object sender, EventArgs e)
        {
            LoadSoundfont(3);
        }

        void SoundfontReload4(object sender, EventArgs e)
        {
            LoadSoundfont(4);
        }

        void SoundfontReload5(object sender, EventArgs e)
        {
            LoadSoundfont(5);
        }

        void SoundfontReload6(object sender, EventArgs e)
        {
            LoadSoundfont(6);
        }

        void SoundfontReload7(object sender, EventArgs e)
        {
            LoadSoundfont(7);
        }

        void SoundfontReload8(object sender, EventArgs e)
        {
            LoadSoundfont(8);
        }

        private static void CheckPop(object source, EventArgs e)
        {
            RegistryKey Watchdog = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Driver\\Watchdog", true);
            if (Convert.ToInt32(Watchdog.GetValue("closewatchdog")) == 1)
            {
                Watchdog.SetValue("closewatchdog", "0", RegistryValueKind.DWord);
                Application.Exit();
            }
        }

        void LoadSoundfont(int whichone)
        {
            try
            {
                RegistryKey Watchdog = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Driver\\Watchdog", true);
                Watchdog.SetValue("rel" + whichone.ToString(), "1", RegistryValueKind.DWord);
            }
            catch
            {

            }
        }
    }
}
