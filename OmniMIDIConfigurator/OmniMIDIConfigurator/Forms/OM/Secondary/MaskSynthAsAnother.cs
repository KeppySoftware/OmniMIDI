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

namespace OmniMIDIConfigurator
{
    public partial class MaskSynthAsAnother : Form
    {
        String PreviousName = "";
        Int32 PreviousType = 0;
        Int32 PreviousVID = 0;
        Int32 PreviousPID = 0;

        public MaskSynthAsAnother()
        {
            InitializeComponent();
        }

        private void MaskSynthAsAnother_Load(object sender, EventArgs e)
        {
            try
            {
                PreviousName = Program.SynthSettings.GetValue("SynthName", "OmniMIDI").ToString();
                PreviousType = Convert.ToInt32(Program.SynthSettings.GetValue("SynthType", 4));
                PreviousVID = Convert.ToInt32(Program.SynthSettings.GetValue("VID", 0xFFFF));
                PreviousPID = Convert.ToInt32(Program.SynthSettings.GetValue("Pid", 0x000A));
                Names.Text = PreviousName;
                SynthType.SelectedIndex = PreviousType;
                VIDValue.Value = PreviousVID;
                PIDValue.Value = PreviousPID;
                Names.TextChanged += new System.EventHandler(Names_TextChanged);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Program.SynthSettings.SetValue("SynthName", PreviousName, RegistryValueKind.String);
            Program.SynthSettings.SetValue("SynthType", PreviousType, RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("VID", PreviousVID, RegistryValueKind.DWord);
            Program.SynthSettings.SetValue("PID", PreviousPID, RegistryValueKind.DWord);
            Close();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            try
            {
                if (Names.Text.Length >= 1)
                {
                    if (Names.Text.Equals("OmniMapper", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("You can not set the mask name to OmniMapper!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    Program.SynthSettings.SetValue("SynthName", Names.Text, RegistryValueKind.String);
                    Close();
                }
                else MessageBox.Show("The mask name can not be blank!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VIDPIDList_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.pcilookup.com/?ven=&dev=&action=submit");
        }

        private void SynthType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.SynthSettings.SetValue("SynthType", SynthType.SelectedIndex, RegistryValueKind.DWord);
        }

        private void VIDValue_ValueChanged(object sender, EventArgs e)
        {
            Program.SynthSettings.SetValue("VID", VIDValue.Value, RegistryValueKind.DWord);
        }

        private void PIDValue_ValueChanged(object sender, EventArgs e)
        {
            Program.SynthSettings.SetValue("PID", PIDValue.Value, RegistryValueKind.DWord);
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
            Names.Text = "OmniMIDI";
            SynthType.SelectedIndex = 2;
            VIDValue.Value = 0xFFFF;
            PIDValue.Value = 0x000A;
            VIDValue_ValueChanged(sender, e);
            PIDValue_ValueChanged(sender, e);
        }
    }
}
