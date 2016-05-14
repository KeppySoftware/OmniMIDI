using System;
using System.Collections.Generic;
using System.Threading;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace KeppyDriverConfigurator
{
    public partial class KeppyDriverDebugWindow : Form
    {
        private static KeppyDriverDebugWindow inst;
        public static KeppyDriverDebugWindow GetForm
        {
            get
            {
                if (inst == null || inst.IsDisposed)
                {
                    inst = new KeppyDriverDebugWindow();
                }
                else
                {
                    System.Media.SystemSounds.Asterisk.Play();
                    Application.OpenForms["KeppyDriverDebugWindow"].BringToFront();
                }
                return inst;
            }
        }

        public KeppyDriverDebugWindow()
        {
            InitializeComponent();
        }

        private void DebugRefresh_Tick(object sender, EventArgs e)
        {
            try
            {
                RegistryKey Debug = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Driver", false);
                RegistryKey Settings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Driver\\Settings", false);
                Voices.Text = Convert.ToInt32(Debug.GetValue("currentvoices0")).ToString("0000") + "/" + Convert.ToInt32(Settings.GetValue("polyphony")).ToString("0000");
                if (Convert.ToInt32(Settings.GetValue("encmode")) == 1)
                {
                    CPU.Text = "Unavailable";
                }
                else
                {
                    CPU.Text = (Convert.ToInt32(Debug.GetValue("currentcpuusage0"))).ToString("000") + "%";
                }
                if (Convert.ToInt32(Settings.GetValue("xaudiodisabled")) == 1)
                {
                    DecodedInt.Visible = false;
                    DecodedInt.Text = "";
                }
                else
                {
                    DecodedInt.Visible = true;
                    DecodedInt.Text = "Decoded data size: " + Debug.GetValue("int") + " frames (Int32 value)";
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "\n\nPress OK to stop the debug mode.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }
    }
}
