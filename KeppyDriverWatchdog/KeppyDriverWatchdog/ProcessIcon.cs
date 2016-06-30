using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace KeppyDriverWatchdog
{
    class ProcessIcon : IDisposable
    {
        NotifyIcon ni;

        public ProcessIcon()
        {
            ni = new NotifyIcon();
        }

        public void Display()
        {		
            ni.MouseClick += new MouseEventHandler(ni_MouseClick);
            ni.Text = "Keppy's Driver Watchdog";
            ni.Icon = KeppyDriverWatchdog.Properties.Resources.gear;
            ni.Visible = true;

            ni.ContextMenuStrip = new ContextMenus().Create();
        }

        public void Dispose()
        {
            ni.Dispose();
        }

        void ni_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Process.Start("KeppyDriverConfigurator", null);
            }
        }
    }
}
