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
    public partial class PreciseControlVol : Form
    {
        public int ReturnValue { get; private set; }

        public PreciseControlVol()
        {
            InitializeComponent();
            VolValN.Value = (decimal)OmniMIDIConfiguratorMain.Delegate.VolTrackBar.Value;
            VolValN.Maximum = VolTrackBar.Maximum = OmniMIDIConfiguratorMain.Delegate.VolTrackBar.Maximum;
            VolTrackBar.Value = OmniMIDIConfiguratorMain.Delegate.VolTrackBar.Value;
        }

        private void VolTrackBar_Scroll(object sender, EventArgs e)
        {
            VolValN.Scroll -= VolValN_ValueChanged;
            VolValN.Value = (decimal)VolTrackBar.Value;
            VolValN.Scroll += VolValN_ValueChanged;
            OmniMIDIConfiguratorMain.Delegate.VolTrackBar.Value = VolTrackBar.Value;
        }

        private void VolValN_ValueChanged(object sender, EventArgs e)
        {
            VolTrackBar.Scroll -= VolTrackBar_Scroll;
            VolTrackBar.Value = (int)VolValN.Value;
            VolTrackBar.Scroll += VolTrackBar_Scroll;
            OmniMIDIConfiguratorMain.Delegate.VolTrackBar.Value = VolTrackBar.Value;
        }

        private void ReturnOK_Click(object sender, EventArgs e)
        {
            OmniMIDIConfiguratorMain.Delegate.VolTrackBar.Value = VolTrackBar.Value;
            Close();
        }
    }
}
