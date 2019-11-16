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
        public int NewVolume { get; private set; }

        public PreciseControlVol(int CV, int MV)
        {
            InitializeComponent();

            VolValN.Maximum = MV;
            VolTrackBar.Maximum = MV;

            VolValN.Value = CV;
            VolTrackBar.Value = CV;
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
