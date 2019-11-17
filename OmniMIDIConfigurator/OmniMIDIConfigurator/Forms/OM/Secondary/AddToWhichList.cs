using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OmniMIDIConfigurator{
    public partial class AddToWhichList : Form
    {
        public string AddToFollowingList { get; set; }
        public int Index { get; set; }

        public AddToWhichList(String SF)
        {
            InitializeComponent();
            InfoMessage.Text = String.Format(InfoMessage.Text, Path.GetFileNameWithoutExtension(SF));
        }

        private void AddToWhichList_Load(object sender, EventArgs e)
        {
            ListSel.SelectedIndex = 0;
        }

        private void AddToList_Click(object sender, EventArgs e)
        {
            AddToFollowingList = Program.ListsPath[ListSel.SelectedIndex];
            Index = ListSel.SelectedIndex;
            DialogResult = DialogResult.OK;
        }
    }
}
