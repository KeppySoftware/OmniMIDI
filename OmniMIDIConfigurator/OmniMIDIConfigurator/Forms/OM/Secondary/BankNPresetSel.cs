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
        public int BankValueReturn = 0;
        public int PresetValueReturn = 0;
        public int DesBankValueReturn = 0;
        public int DesPresetValueReturn = 0;
        public int DesBankLSBValueReturn = 0;
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
                try { SrcBankVal.Value = SettingsArray[0]; } catch { SrcBankVal.Value = -1; }
                try { SrcPresetVal.Value = SettingsArray[1]; } catch { SrcPresetVal.Value = -1; }
                try { DesBankVal.Value = SettingsArray[2]; } catch { DesBankVal.Value = -1; }
                try { DesPresetVal.Value = SettingsArray[3]; } catch { DesPresetVal.Value = 0; }
                try { DesBankLSBVal.Value = SettingsArray[4]; } catch { DesBankLSBVal.Value = 0; }
                try { XGMode.Checked = Convert.ToBoolean(SettingsArray[5]); } catch { XGMode.Checked = false; }
            }
        }

        private void ConfirmBut_Click(object sender, EventArgs e)
        {
            XGModeC = XGMode.Checked;
            BankValueReturn = Convert.ToInt32(SrcBankVal.Value);
            PresetValueReturn = Convert.ToInt32(SrcPresetVal.Value);
            DesBankValueReturn = Convert.ToInt32(DesBankVal.Value);
            DesPresetValueReturn = Convert.ToInt32(DesPresetVal.Value);
            DesBankLSBValueReturn = Convert.ToInt32(DesBankLSBVal.Value);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void WikipediaLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.midi.org/specifications-old/item/gm-level-1-sound-set");
        }

        private void BankNPresetSel_Load(object sender, EventArgs e)
        {

        }
    }
}
