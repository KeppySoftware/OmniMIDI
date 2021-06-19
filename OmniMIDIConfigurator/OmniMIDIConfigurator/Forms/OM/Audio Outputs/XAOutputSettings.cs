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
            SPFSweepRatioVal.Value = Convert.ToDecimal(Program.SynthSettings.GetValue("XASPFSweepRate", 15));

            CalculateLatency();
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            Program.SynthSettings.SetValue("XASamplesPerFrame", SPFVal.Value, Microsoft.Win32.RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("XASPFSweepRate", SPFSweepRatioVal.Value, Microsoft.Win32.RegistryValueKind.DWord);
            Functions.SetDefaultDevice(AudioEngine.XA_ENGINE, 0, null);
            Close();
            Dispose();
        }

        private void ResetBtn_Click(object sender, EventArgs e)
        {
            Program.SynthSettings.SetValue("XASamplesPerFrame", 88, Microsoft.Win32.RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("XASPFSweepRate", 15, Microsoft.Win32.RegistryValueKind.DWord);
            SPFVal.Value = 88;
        }

        private void SPFVal_ValueChanged(object sender, EventArgs e)
        {
            CalculateLatency();
        }

        private void SPFSweepRatioVal_ValueChanged(object sender, EventArgs e)
        {
            CalculateLatency();
        }

        private void CalculateLatency()
        {
            bool IsMono = Convert.ToBoolean(Convert.ToInt32(Program.SynthSettings.GetValue("MonoRendering", 0)));
            int Freq = Convert.ToInt32(Program.SynthSettings.GetValue("AudioFrequency", 48000));
            int SPF = (int)SPFVal.Value * (IsMono ? 1 : 2);
            double Latency = ((double)SPF * 1000.0d / Freq) * (double)SPFSweepRatioVal.Value;

            if (Latency > (13.5d / (IsMono ? 2 : 1)) && Latency < (20.0d / (IsMono ? 2 : 1)))
            {
                EstLat.ForeColor = Color.DarkOrange;
                EstLat.Font = new Font(EstLat.Font, FontStyle.Bold);
            }
            if (Latency <= (13.5d / (IsMono ? 2 : 1)))
            {
                EstLat.ForeColor = Color.DarkRed;
                EstLat.Font = new Font(EstLat.Font, FontStyle.Bold);
            }
            else if (Latency >= (20.0d / (IsMono ? 2 : 1)))
            {
                EstLat.ForeColor = Color.DarkGreen;
                EstLat.Font = new Font(EstLat.Font, FontStyle.Regular);
            }

            EstLat.Text = String.Format("Estimated latency: {0}ms", Latency.ToString("N2"));
        }
    }
}
