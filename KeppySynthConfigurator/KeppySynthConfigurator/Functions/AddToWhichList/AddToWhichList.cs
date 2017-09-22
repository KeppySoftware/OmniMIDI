using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeppySynthConfigurator
{
    public partial class AddToWhichList : Form
    {
        public string AddToFollowingList { get; set; }
        public int Index { get; set; }

        public AddToWhichList(String SF)
        {
            InitializeComponent();
            InfoMessage.Text = String.Format(InfoMessage.Text, Path.GetFileNameWithoutExtension(SF));
            if (Convert.ToInt32(KeppySynthConfiguratorMain.SynthSettings.GetValue("extra8lists", 0)) == 1)
            {
                ListSel.Items.Add("List 9");
                ListSel.Items.Add("List 10");
                ListSel.Items.Add("List 11");
                ListSel.Items.Add("List 12");
                ListSel.Items.Add("List 13");
                ListSel.Items.Add("List 14");
                ListSel.Items.Add("List 15");
                ListSel.Items.Add("List 16");
            }
        }

        private void AddToWhichList_Load(object sender, EventArgs e)
        {
            ListSel.SelectedIndex = 0;
        }

        private void AddToList_Click(object sender, EventArgs e)
        {
            AddToFollowingList = KeppySynthConfiguratorMain.ListsPath[ListSel.SelectedIndex];

            Index = ListSel.SelectedIndex;

            DialogResult = DialogResult.OK;
        }
    }
}
