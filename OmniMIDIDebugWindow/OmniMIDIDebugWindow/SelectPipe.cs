using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace OmniMIDIDebugWindow
{
    public partial class SelectPipe : Form
    {
        public int SelectedPipe { get; set; }

        public SelectPipe()
        {
            InitializeComponent();
        }

        private void PopulateList()
        {
            OMPipes.Items.Clear();

            String[] Pipes = Program.GetDebugPipesList();

            foreach (String Pipe in Pipes)
                OMPipes.Items.Add(Pipe);

            PipesFoundTxt.Text = String.Format("Found {0} {1}.", Pipes.Count(), (Pipes.Count() == 1) ? "pipe" : "pipes");
        }

        private void SelectPipe_Load(object sender, EventArgs e)
        {
            PopulateList();
        }

        private void Reload_Click(object sender, EventArgs e)
        {
            PopulateList();
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            if (OMPipes.SelectedIndex > -1)
            {
                Int32 TempPipe = int.Parse(Regex.Match(OMPipes.SelectedItem.ToString(), @"\d+").Value);
                if (!Program.DoesPipeStillExist(TempPipe))
                {
                    MessageBox.Show("The selected pipe doesn't exist anymore.\n\nTry again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    PopulateList();
                    return;
                }

                SelectedPipe = TempPipe;
                DialogResult = DialogResult.OK;
            }
            else DialogResult = DialogResult.Cancel;

            Close();
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
