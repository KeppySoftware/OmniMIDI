using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MIDI.NET.Devices;

namespace KeppyDriverConfigurator
{
    public partial class KeppyDriverMIDIOutSelectorWin : Form
    {
        private Int32 defaultID = -2; // MIDI Mapper has a device ID of -1 and all other devices are 0 or greater.
        private int newID = -2;
        private bool writeMode = false;
        private bool writeOK = false;

        public KeppyDriverMIDIOutSelectorWin()
        {
            InitializeComponent();
            Load += new EventHandler(CheckDevices);
            backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
            lvwOutputs.SelectedIndexChanged += new EventHandler(lvwOutputs_SelectedIndexChanged);
            btnSetAsDefault.Click += new EventHandler(btnSetAsDefault_Click);
        }

        private void CheckDevices(object sender, EventArgs e)
        {
            backgroundWorker.RunWorkerAsync();
        }

        void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (writeMode)
            {
                try
                {
                    Output.DefaultDeviceID = newID;
                    writeOK = true;
                }
                catch
                {
                    writeOK = false;
                }
            }
            else
            {
                Collections.Load();
                defaultID = Output.DefaultDeviceID;
            }
        }

        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (writeMode)
            {
                if (writeOK)
                {
                    lblStatus.Text = "Default MIDI out device updated successfully.";
                    lblStatus.Update();
                    System.Threading.Thread.Sleep(1000);
                    defaultID = newID;
                    lblOutputDevices.Enabled = true;
                    lvwOutputs.Enabled = true;
                    lvwOutputs.Focus();
                    lblStatus.Visible = false;
                }
                else
                {
                    MessageBox.Show(
                        this,
                        "An error occurred when attempting to write to the registry.",
                        "Registry Write Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    lblOutputDevices.Enabled = true;
                    lvwOutputs.Enabled = true;
                    lvwOutputs.Focus();
                    btnSetAsDefault.Enabled = true;
                    lblStatus.Visible = false;
                }
            }
            else
            {
                int itemCounter = 0;
                foreach (Output midiDevice in Collections.Outputs)
                {
                    ListViewItem item = new ListViewItem(midiDevice.Name);
                    item.Tag = midiDevice;
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, midiDevice.ID.ToString()));
                    lvwOutputs.Items.Add(item);
                    lvwOutputs.Items[itemCounter].Selected = midiDevice.ID == defaultID;
                    lvwOutputs.Items[itemCounter].Focused = true;
                    itemCounter++;
                }
                lblOutputDevices.Enabled = true;
                lvwOutputs.Enabled = true;
                lvwOutputs.Focus();
                lblStatus.Visible = false;
            }
        }

        void lvwOutputs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvwOutputs.SelectedItems.Count == 1)
            {
                btnSetAsDefault.Enabled = ((Output)lvwOutputs.SelectedItems[0].Tag).ID != defaultID;
            }
            else
            {
                btnSetAsDefault.Enabled = false;
            }
        }

        void btnSetAsDefault_Click(object sender, EventArgs e)
        {
            writeOK = false;
            btnSetAsDefault.Enabled = false;
            lblOutputDevices.Enabled = false;
            lvwOutputs.Enabled = false;
            lblStatus.Text = "Writing to registry... please wait!";
            lblStatus.Visible = true;
            newID = ((Output)lvwOutputs.SelectedItems[0].Tag).ID;
            writeMode = true;
            backgroundWorker.RunWorkerAsync();
        }
    }
}
