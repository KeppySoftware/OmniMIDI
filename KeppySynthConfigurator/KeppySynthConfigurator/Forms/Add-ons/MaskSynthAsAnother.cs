using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Data;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace KeppySynthConfigurator
{
    public partial class MaskSynthAsAnother : Form
    {
        String PreviousName = "";
        Int32 PreviousType = 4;

        public MaskSynthAsAnother()
        {
            InitializeComponent();
        }

        private void MaskSynthAsAnother_Load(object sender, EventArgs e)
        {
            try
            {
                PreviousName = KeppySynthConfiguratorMain.SynthSettings.GetValue("synthname", "Keppy's Synthesizer").ToString();
                PreviousType = Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("synthtype", 4));
                Names.Text = PreviousName;
                SynthType.SelectedIndex = PreviousType;
                Names.TextChanged += new System.EventHandler(Names_TextChanged);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            KeppySynthConfiguratorMain.SynthSettings.SetValue("synthname", PreviousName, RegistryValueKind.String);
            Close();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            try
            {
                if (Names.Text.Length >= 1)
                {
                    KeppySynthConfiguratorMain.SynthSettings.SetValue("synthname", Names.Text, RegistryValueKind.String);
                    Close();
                }
                else
                {
                    MessageBox.Show("The mask name can not be blank!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddNewNamePl0x_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Sure thing, just create an issue on GitHub where you tell me which synthesizer/driver I need to add, and I'll add it asap.\n\nClicking \"Yes\" will redirect you to the GitHub page, are you sure you want to continue?", "Can you add another name to the list?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                Process.Start("https://github.com/KaleidonKep99/Keppy-s-MIDI-Driver/issues");
            }
        }

        private void SynthType_SelectedIndexChanged(object sender, EventArgs e)
        {
            KeppySynthConfiguratorMain.SynthSettings.SetValue("synthtype", SynthType.SelectedIndex, RegistryValueKind.DWord);
        }

        private void Names_TextChanged(object sender, EventArgs e)
        {
            String TempString = Names.Text;
            if (TempString.Length >= 32)
            {
                TempString = TempString.Remove(TempString.Length - 1);
                MessageBox.Show("The maximum length for the mask name is 31 characters!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Names.Text = TempString;
            }
        }

        private void DefName_Click(object sender, EventArgs e)
        {
            Names.Text = "Keppy's Synthesizer";
            SynthType.SelectedIndex = 4;
        }
    }
}
