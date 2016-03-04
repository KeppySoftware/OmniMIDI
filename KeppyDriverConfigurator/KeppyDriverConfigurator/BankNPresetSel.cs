using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace KeppyDriverConfigurator
{
    public partial class BankNPresetSel : Form
    {
        public string BankValueReturn { get; set; }
        public string PresetValueReturn { get; set; }
        public string SelectedSF { get; set; }

        public BankNPresetSel(String Target)
        {
            InitializeComponent();
            SelectedSF = Target;
        }

        private void PresetSel_Load(object sender, EventArgs e)
        {
            SelectedSFLabel.Text = "Selected soundfont:\n" + SelectedSF;
            BankVal.Value = 0;
            PresetVal.Value = 0;
        }

        private void ConfirmBut_Click(object sender, EventArgs e)
        {
            BankValueReturn = BankVal.Value.ToString();
            PresetValueReturn = PresetVal.Value.ToString();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void WikipediaLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var helpFile = Path.Combine(Path.GetTempPath(), "help.txt");
            File.WriteAllText(helpFile, KeppyDriverConfigurator.Properties.Resources.gmlist);
            Process.Start(helpFile);
        }


    }
}
