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

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        struct WIN32_FIND_DATA
        {
            public uint dwFileAttributes;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
            public uint nFileSizeHigh;
            public uint nFileSizeLow;
            public uint dwReserved0;
            public uint dwReserved1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string cFileName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
            public string cAlternateFileName;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr FindFirstFile(string lpFileName, out WIN32_FIND_DATA lpFindFileData);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool FindNextFile(IntPtr hFindFile, out WIN32_FIND_DATA
           lpFindFileData);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool FindClose(IntPtr hFindFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool GetNamedPipeClientProcessId(IntPtr Pipe, out uint ClientProcessId);

        public SelectPipe()
        {
            InitializeComponent();
        }

        private bool DoesPipeStillExist(int requestedpipe)
        {
            try
            {
                String PipeToAdd;

                IntPtr ptr = FindFirstFile(@"\\.\pipe\*", out WIN32_FIND_DATA lpFindFileData);
                PipeToAdd = Path.GetFileName(lpFindFileData.cFileName);
                if (PipeToAdd.Contains(String.Format("OmniMIDIDbg{0}", requestedpipe))) return true;

                while (FindNextFile(ptr, out lpFindFileData))
                {
                    PipeToAdd = Path.GetFileName(lpFindFileData.cFileName);
                    if (PipeToAdd.Contains(String.Format("OmniMIDIDbg{0}", requestedpipe))) return true;
                }
                FindClose(ptr);

                return false;
            }
            catch { return false; }
        }

        private void PopulateList()
        {
            try
            {
                OMPipes.Items.Clear();

                PipesFoundTxt.Text = "Checking...";

                Int32 Found = 0;
                String PipeToAdd;
                WIN32_FIND_DATA lpFindFileData;

                IntPtr ptr = FindFirstFile(@"\\.\pipe\*", out lpFindFileData);
                PipeToAdd = Path.GetFileName(lpFindFileData.cFileName);
                if (PipeToAdd.Contains("OmniMIDIDbg"))
                {
                    OMPipes.Items.Add(String.Format("Debug pipe {0} (OmniMIDIDbg{0})", Regex.Match(PipeToAdd, @"\d+").Value));
                    Found++;
                }

                while (FindNextFile(ptr, out lpFindFileData))
                {
                    PipeToAdd = Path.GetFileName(lpFindFileData.cFileName);
                    if (PipeToAdd.Contains("OmniMIDIDbg"))
                    {
                        OMPipes.Items.Add(String.Format("Debug pipe {0} (OmniMIDIDbg{0})", Regex.Match(PipeToAdd, @"\d+").Value));
                        Found++;
                    }
                }
                FindClose(ptr);

                PipesFoundTxt.Text = String.Format("Found {0} {1}.", Found, (Found == 1) ? "pipe" : "pipes");
            }
            catch (Exception ex)
            {
                // If something goes wrong, here's an error handler
                MessageBox.Show(ex.ToString() + "\n\nPress OK to stop the debug mode.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.ExitThread();
            }
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
                if (!DoesPipeStillExist(OMPipes.SelectedIndex + 1))
                {
                    MessageBox.Show("The selected pipe doesn't exist anymore.\n\nTry again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    PopulateList();
                    return;
                }

                SelectedPipe = OMPipes.SelectedIndex;
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
