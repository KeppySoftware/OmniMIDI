using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeppySynthConfigurator
{
    public partial class WinMMPatches : Form
    {
        public WinMMPatches()
        {
            InitializeComponent();

            if (!Environment.Is64BitOperatingSystem)
            {
                KSDAPI32.Enabled = true;
            }
            else
            {
                KSDAPI32.Enabled = true;
                KSDAPI64.Enabled = true;
            }
        }

        private void KSDAPI32_Click(object sender, EventArgs e)
        {
            Functions.ApplyWinMMPatch(false);
        }

        private void KSDAPI64_Click(object sender, EventArgs e)
        {
            Functions.ApplyWinMMPatch(true);
        }

        private void WMMW32_Click(object sender, EventArgs e)
        {
            Functions.ApplyWinMMWRPPatch(false);
        }

        private void WMMW64_Click(object sender, EventArgs e)
        {
            Functions.ApplyWinMMWRPPatch(true);
        }

        private void UnpatchApp_Click(object sender, EventArgs e)
        {
            Functions.RemoveWinMMPatch();
        }

        private void DifferencesPatch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/KeppySoftware/Keppy-s-Synthesizer/wiki/What's-the-difference-between-the-WinMM-patches%3F");
        }

        private void OKClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
