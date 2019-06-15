using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Windows.Forms;

namespace OmniMIDIConfigurator
{
    public partial class AddProcesses : Form
    {
        public List<String> BanThesePlease = new List<String>();
        Process[] ProcessList = Process.GetProcesses();

        public AddProcesses()
        {
            InitializeComponent();
        }

        private void GetProcesses()
        {
            List<String> Processes = new List<String>();

            var wmiQueryString = "SELECT ProcessId, ExecutablePath, CommandLine FROM Win32_Process";
            using (var searcher = new ManagementObjectSearcher(wmiQueryString))
            using (var results = searcher.Get())
            {
                var query = from p in Process.GetProcesses()
                            join mo in results.Cast<ManagementObject>()
                            on p.Id equals (int)(uint)mo["ProcessId"]
                            select new
                            {
                                Process = p,
                                Path = (string)mo["ExecutablePath"],
                                CommandLine = (string)mo["CommandLine"],
                            };
                foreach (var item in query)
                {
                    if (!String.IsNullOrEmpty(item.Path))Processes.Add(Path.GetFileName(item.Path));
                }

                String[] ProcessesToSort = Processes.ToArray();
                Array.Sort(ProcessesToSort, StringComparer.InvariantCulture);
                ProcessesToSort = ProcessesToSort.Distinct().ToArray();
                foreach (String Process in ProcessesToSort) RunningProcessesList.Items.Add(Process);
            }
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
