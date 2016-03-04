using System;
using System.Collections.Generic;
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
                Voices.Text = Debug.GetValue("currentvoices0") + "/" + Settings.GetValue("polyphony");
                CPU.Text = Debug.GetValue("currentcpuusage0") + "%";
                DecodedInt.Text = "Decoded data size: " + Debug.GetValue("int") +" frames (Int32 value)";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString() + "\n\nPress OK to stop the debug mode.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
        }
    }
}
