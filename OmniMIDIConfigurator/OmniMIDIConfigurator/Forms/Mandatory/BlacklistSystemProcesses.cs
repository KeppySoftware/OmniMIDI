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
    public partial class BlacklistSystemProcesses : Form
    {
        public List<String> BanThesePlease = new List<String>();
        Process[] ProcessList = Process.GetProcesses();

        public BlacklistSystemProcesses()
        {
            InitializeComponent();
        }

        private void GetProcesses()
        {
            List<String> Processes = new List<String>();
            foreach (Process theprocess in ProcessList) Processes.Add(theprocess.ProcessName);

            String[] ProcessesToSort = Processes.ToArray();
            Array.Sort(ProcessesToSort, StringComparer.InvariantCulture);
            ProcessesToSort = ProcessesToSort.Distinct().ToArray();
            foreach (String Process in ProcessesToSort) RunningProcessesList.Items.Add(Process);
        }

        private void BlacklistSystemProcesses_Load(object sender, EventArgs e)
        {
            GetProcesses();
        }

        private void RefreshList_Click(object sender, EventArgs e)
        {
            RefrLab.Visible = true;
            RunningProcessesList.Items.Clear();
            ProcessList = Process.GetProcesses();
            GetProcesses();
            System.Threading.Thread.Sleep(100);
            RefrLab.Visible = false;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void OKDone_Click(object sender, EventArgs e)
        {
            foreach (var item in RunningProcessesList.SelectedItems) BanThesePlease.Add(item.ToString());
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
