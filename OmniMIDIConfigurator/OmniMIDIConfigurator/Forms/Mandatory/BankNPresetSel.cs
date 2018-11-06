using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace OmniMIDIConfigurator
{
    public partial class BankNPresetSel : Form
    {
        public bool XGModeC = false;
        public string BankValueReturn = "0";
        public string PresetValueReturn = "0";
        public string DesBankValueReturn = "0";
        public string DesPresetValueReturn = "0";
        public string SelectedSF = "";

        public BankNPresetSel(String Target, Boolean IsEditingSoundfont, Boolean IsEditingSF2, Int32[] SettingsArray)
        {
            InitializeComponent();
            SelectedSF = Target;
            SelectedSFLabel.Text = "Selected soundfont:\n" + Path.GetFileNameWithoutExtension(SelectedSF);
            DesBankVal.Value = 0;
            DesPresetVal.Value = 0;

            if (IsEditingSF2)
            {
                SrcBankVal.Minimum = -1;
                SrcPresetVal.Minimum = -1;
                DesPresetVal.Minimum = -1;
            }
            else
            {
                SrcBankVal.Enabled = false;
                SrcPresetVal.Enabled = false;
                SrcBankVal.Value = 0;
                SrcPresetVal.Value = 0;
            }

            if (IsEditingSoundfont) 
            {
                SrcBankVal.Value = SettingsArray[0];
                SrcPresetVal.Value = SettingsArray[1];
                DesBankVal.Value = SettingsArray[2];
                DesPresetVal.Value = SettingsArray[3];
                XGMode.Checked = Convert.ToBoolean(SettingsArray[4]);
            }
        }

        private void ConfirmBut_Click(object sender, EventArgs e)
        {
            XGModeC = XGMode.Checked;
            BankValueReturn = SrcBankVal.Value.ToString();
            PresetValueReturn = SrcPresetVal.Value.ToString();
            DesBankValueReturn = DesBankVal.Value.ToString();
            DesPresetValueReturn = DesPresetVal.Value.ToString();
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
            File.WriteAllText(helpFile, Properties.Resources.gmlist);
            Process.Start(helpFile);
        }

        private void BankNPresetSel_Load(object sender, EventArgs e)
        {

        }
    }
}
