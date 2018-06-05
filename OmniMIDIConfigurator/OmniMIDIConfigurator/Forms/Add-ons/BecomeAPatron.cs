using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OmniMIDIConfigurator
{
    public partial class BecomeAPatron : Form
    {
        public BecomeAPatron()
        {
            InitializeComponent();
        }

        private void DontShowAnymore_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BecomeAPatronNow_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.patreon.com/KaleidonKep99");
            Close();
        }
    }
}
