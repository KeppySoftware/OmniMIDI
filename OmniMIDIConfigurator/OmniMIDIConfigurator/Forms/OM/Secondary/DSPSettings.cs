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
    public partial class DSPSettings : Form
    {
        public DSPSettings()
        {
            InitializeComponent();

            ROverride.Checked = Convert.ToBoolean(Convert.ToInt32(Program.SynthSettings.GetValue("ReverbOverride", 0)));
            COverride.Checked = Convert.ToBoolean(Convert.ToInt32(Program.SynthSettings.GetValue("ChorusOverride", 0)));

            // REVERB
            RIGTb.Value = LimitToRange(Convert.ToInt32(Program.SynthSettings.GetValue("ReverbInGain", 96000)), 0, 96000);
            RMTb.Value = LimitToRange(Convert.ToInt32(Program.SynthSettings.GetValue("ReverbMix", 96000)), 0, 96000);
            RTVal.Value = LimitToRange(Convert.ToInt32(Program.SynthSettings.GetValue("ReverbTime", 1000000)), 0, 3000000) / 1000.0m;
            HFRTRVal.Value = LimitToRange(Convert.ToInt32(Program.SynthSettings.GetValue("ReverbHighFreqRTRatio", 1)), 1, 999) / 1000.0m;
            // REVERB

            // CHORUS
            CWDMb.Value = LimitToRange(Convert.ToInt32(Program.SynthSettings.GetValue("ChorusWetDryMix", 50)), 0, 100);
            CDpb.Value = LimitToRange(Convert.ToInt32(Program.SynthSettings.GetValue("ChorusDepth", 10)), 0, 100);
            CFbb.Value = LimitToRange(Convert.ToInt32(Program.SynthSettings.GetValue("ChorusFeedback", 25)) - 100, -99, 99);
            CFrb.Value = LimitToRange(Convert.ToInt32(Program.SynthSettings.GetValue("ChorusFrequency", 1100)), 0, 10000) / 1000.0m;
            CDlb.Value = LimitToRange(Convert.ToInt32(Program.SynthSettings.GetValue("ChorusDelay", 16)), 0, 20);
            CSMc.Checked = Convert.ToBoolean(Convert.ToInt32(Program.SynthSettings.GetValue("ChorusSineMode", 1)));
            CPhC.SelectedIndex = LimitToRange(Convert.ToInt32(Program.SynthSettings.GetValue("ChorusPhase", 2)), 0, 4);
            // CHORUS

            // ECHO

            // ECHO

            RIGTb_Scroll(null, null);
            RMTb_Scroll(null, null);

            CWDMb_Scroll(null, null);
            CDpb_Scroll(null, null);
            CFbb_Scroll(null, null);
        }

        public int LimitToRange(int Val, int Min, int Max)
        {
            if (Val >= Min)
            {
                if (Val <= Max)
                    return Val;

                return Min;
            }

            return Max;
        }

        private void RIGTb_Scroll(object sender, EventArgs e)
        {
            float Val = (RIGTb.Value - 96000) / 1000.0f;
            RIGVal.Text = String.Format("{0}dB", Val < -95.9f ? "∞" : Val.ToString("0.0"));
        }

        private void RMTb_Scroll(object sender, EventArgs e)
        {
            float Val = (RMTb.Value - 96000) / 1000.0f;
            RMVal.Text = String.Format("{0}db", Val < -95.9f ? "∞" : Val.ToString("0.0"));
        }

        private void CWDMb_Scroll(object sender, EventArgs e)
        {
            CWDMVal.Text = String.Format("{0}%", CWDMb.Value);
        }

        private void CDpb_Scroll(object sender, EventArgs e)
        {
            CDpVal.Text = String.Format("{0}%", CDpb.Value);
        }

        private void CFbb_Scroll(object sender, EventArgs e)
        {
            CFbVal.Text = String.Format("{0}", CFbb.Value);
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ApplyBtn_Click(object sender, EventArgs e)
        {
            // REVERB
            Program.SynthSettings.SetValue("ReverbOverride", Convert.ToInt32(ROverride.Checked));
            Program.SynthSettings.SetValue("ReverbInGain", RIGTb.Value);
            Program.SynthSettings.SetValue("ReverbMix", RMTb.Value);
            Program.SynthSettings.SetValue("ReverbTime", Convert.ToInt32(RTVal.Value * 1000));
            Program.SynthSettings.SetValue("ReverbHighFreqRTRatio", Convert.ToInt32(HFRTRVal.Value * 1000));
            // REVERB

            // ECHO
            Program.SynthSettings.SetValue("ChorusOverride", Convert.ToInt32(COverride.Checked));
            Program.SynthSettings.SetValue("ChorusWetDryMix", CWDMb.Value);
            Program.SynthSettings.SetValue("ChorusDepth", CDpb.Value);
            Program.SynthSettings.SetValue("ChorusFeedback", Convert.ToInt32(CFbb.Value + 100));
            Program.SynthSettings.SetValue("ChorusFrequency", Convert.ToInt32(CFrb.Value * 1000));
            Program.SynthSettings.SetValue("ChorusDelay", Convert.ToInt32(CDlb.Value));
            Program.SynthSettings.SetValue("ChorusSineMode", Convert.ToInt32(CSMc.Checked));
            Program.SynthSettings.SetValue("ChorusPhase", CPhC.SelectedIndex);
            // ECHO
        }
    }
}
