using Microsoft.Win32;
using System;
using System.Windows.Forms;

namespace OmniMIDIConfigurator
{
    public partial class PreciseControlVol : Form
    {
        public int NewVolume { get; private set; }

        public PreciseControlVol(int CV, int MV)
        {
            InitializeComponent();

            VolValN.Maximum = MV;
            VolTrackBar.Maximum = MV;

            VolValN.Value = CV;
            VolTrackBar.Value = CV;

            LogarithmVol.Checked = Convert.ToBoolean((int)Program.SynthSettings.GetValue("LogarithmVol", 0));
        }

        private void LogarithmVol_CheckedChanged(object sender, EventArgs e)
        {
            Program.SynthSettings.SetValue("LogarithmVol", Convert.ToInt32(LogarithmVol.Checked), RegistryValueKind.DWord);
        }

        private void ValueChanged(object sender, EventArgs e)
        {
            VolValN.Scroll -= ValueChanged;
            VolTrackBar.Scroll -= ValueChanged;

            if (sender == VolTrackBar)
                VolValN.Value = VolTrackBar.Value;
            else
                VolTrackBar.Value = (int)VolValN.Value;

            VolValN.Scroll += ValueChanged;
            VolTrackBar.Scroll += ValueChanged;
        }

        private void ReturnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            NewVolume = VolTrackBar.Value;
            Close();
        }
    }
}
