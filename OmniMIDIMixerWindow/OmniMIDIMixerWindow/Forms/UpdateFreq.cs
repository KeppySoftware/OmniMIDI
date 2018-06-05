using System;
using System.Windows.Forms;

namespace OmniMIDIMixerWindow.Forms
{
    public partial class UpdateFreq : Form
    {
        String DefMs = "The peak meter will refresh every ~{0}ms.";

        public UpdateFreq()
        {
            InitializeComponent();
        }

        private void UpdateFreq_Load(object sender, EventArgs e)
        {
            UpdateFreqSet.Value = Properties.Settings.Default.VolUpdateHz;
            RefreshValMs.Text = String.Format(DefMs, Convert.ToInt32((1.0 / Properties.Settings.Default.VolUpdateHz) * 1000.0));
        }

        private void UpdateFreqSet_ValueChanged(object sender, EventArgs e)
        {
            RefreshValMs.Text = String.Format(DefMs, Convert.ToInt32((1.0 / (double)UpdateFreqSet.Value) * 1000.0));
        }

        private void SaveValue_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.VolUpdateHz = (int)UpdateFreqSet.Value;
            Properties.Settings.Default.Save();
            Close();
        }
    }
}
