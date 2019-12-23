using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OmniMIDIConfigurator
{
    public partial class WinMMSpeed : Form
    {
        public WinMMSpeed()
        {
            InitializeComponent();
        }

        private void SDTrackBar_Scroll(object sender, EventArgs e)
        {
            SDVal.Text = String.Format("{0}%", (100.0 / SDTrackBar.Value).ToString("F4"));
        }

        private void ReturnOK_Click(object sender, EventArgs e)
        {
            Program.SynthSettings.SetValue("WinMMSpeed", SDTrackBar.Value, Microsoft.Win32.RegistryValueKind.DWord);
            Close();
        }
    }
}
