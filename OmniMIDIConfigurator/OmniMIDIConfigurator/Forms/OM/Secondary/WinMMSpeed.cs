using System;
using System.Windows.Forms;

namespace OmniMIDIConfigurator
{
    public partial class WinMMSpeed : Form
    {
        public WinMMSpeed()
        {
            InitializeComponent();

            SpeedHackVal.Value = Convert.ToDecimal(Program.SynthSettings.GetValue("WinMMSpeed", "100000000")) / 1000000;
        }

        private void DefaultBtn_Click(object sender, EventArgs e)
        {
            Program.SynthSettings.SetValue("WinMMSpeed", 100000000, Microsoft.Win32.RegistryValueKind.DWord);
            SpeedHackVal.Value = Convert.ToDecimal(Program.SynthSettings.GetValue("WinMMSpeed", "100000000")) / 1000000;
        }

        private void ReturnOK_Click(object sender, EventArgs e)
        {
            Program.SynthSettings.SetValue("WinMMSpeed", (int)(SpeedHackVal.Value * (decimal)1000000.0), Microsoft.Win32.RegistryValueKind.DWord);
            Close();
        }
    }
}
