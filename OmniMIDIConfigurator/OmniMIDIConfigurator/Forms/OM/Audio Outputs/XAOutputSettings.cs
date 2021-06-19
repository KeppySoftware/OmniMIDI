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
    public partial class XAOutputSettings : Form
    {
        public XAOutputSettings()
        {
            InitializeComponent();

            SPFVal.Value = Convert.ToDecimal(Program.SynthSettings.GetValue("XASamplesPerFrame", 88));
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            Program.SynthSettings.SetValue("XASamplesPerFrame", SPFVal.Value, Microsoft.Win32.RegistryValueKind.DWord);
            Functions.SetDefaultDevice(AudioEngine.XA_ENGINE, 0, null);
            Close();
            Dispose();
        }

        private void ResetBtn_Click(object sender, EventArgs e)
        {
            Program.SynthSettings.SetValue("XASamplesPerFrame", 88, Microsoft.Win32.RegistryValueKind.DWord);
            SPFVal.Value = 88;
        }
    }
}
