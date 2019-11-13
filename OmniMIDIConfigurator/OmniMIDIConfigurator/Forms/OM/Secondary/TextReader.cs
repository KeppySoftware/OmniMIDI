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
    public partial class TextReader : Form
    {
        public TextReader(String WindowTitle, String Text)
        {
            InitializeComponent();

            this.Text = String.Format("OmniMIDI - {0}", WindowTitle);
            TextContainer.Text = Text;
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
