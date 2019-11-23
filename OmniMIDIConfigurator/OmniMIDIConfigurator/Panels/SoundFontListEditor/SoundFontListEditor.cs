using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;
using System.Media;

namespace OmniMIDIConfigurator
{
    public partial class SoundFontListEditor : UserControl
    {
        public static SoundFontListEditor Delegate;
        static Boolean SelectedIndexCSF = true;

        public SoundFontListEditor(String[] SFs)
        {
            InitializeComponent();

            Delegate = this;

            try
            {
                for (int i = 0; i < Lis.Columns.Count; i++)
                {
                    if (Properties.Settings.Default.SFColumnsSize[i] != -1)
                        Lis.Columns[i].Width = Properties.Settings.Default.SFColumnsSize[i];
                    else
                    {
                        Properties.Settings.Default.SFColumnsSize[i] = Lis.Columns[i].Width;
                        Properties.Settings.Default.Save();
                    }
                }
            }
            catch { Functions.ResetSpecificSetting("SFColumnsSize"); }

            // Attach context menu
            Lis.ContextMenu = LisCM;

            // Prepare CSFWatcher
            SoundFontListExtension.OpenCSFWatcher(false, null);

            SFlg.BackgroundImage = Properties.Resources.Question;
            CLi.BackgroundImage = Properties.Resources.ClearIcon;
            AddSF.BackgroundImage = Properties.Resources.AddSFIcon;
            RmvSF.BackgroundImage = Properties.Resources.RmvSFIcon;
            MvU.BackgroundImage = Properties.Resources.MvUpIcon;
            MvD.BackgroundImage = Properties.Resources.MvDwIcon;
            LoadToApp.BackgroundImage = Properties.Resources.ReloadIcon;
            Preload.BackgroundImage = Properties.Resources.PreloadIcon;
            IEL.BackgroundImage = Properties.Resources.ImportIcon;
            EL.BackgroundImage = Properties.Resources.ExportIcon;

            // Add the SoundFonts before activating the CSFWatcher
            if (SFs != null && SFs.Count() > 0)
            {
                foreach (String SF in SFs)
                {
                    if (SoundFontListExtension.CheckSupportedFormat(Path.GetExtension(SF)))
                    {
                        using (AddToWhichList TF = new AddToWhichList(SF))
                        {
                            if (TF.ShowDialog() == DialogResult.OK)
                            {
                                SelectedListBox.SelectedIndex = TF.Index;

                                String[] TSF = new String[] { SF };
                                ListViewItem[] iSFs = SoundFontListExtension.AddSFToList(TSF, false, true);

                                if (iSFs != null)
                                {
                                    foreach (ListViewItem iSF in iSFs)
                                        Lis.Items.Add(iSF);
                                }

                                SoundFontListExtension.SaveList(ref Lis, SelectedListBox.SelectedIndex, null);
                            }
                        }
                    }

                }
            }

            SelectedListBox.SelectedIndex = Properties.Settings.Default.LastListSelected;
        }

        public void CloseCSFWatcherExt()
        {
            SoundFontListExtension.CloseCSFWatcher();
        }

        private void CSFHandler(object source, FileSystemEventArgs e)
        {
            if (SelectedIndexCSF && !SoundFontListExtension.StopCheck)
            {
                this.Invoke((Action)delegate {
                    Lis.ItemChecked -= new ItemCheckedEventHandler(Lis_ItemChecked);
                    SelectedListBox_SelectedIndexChanged(null, null);
                    Lis.ItemChecked += new ItemCheckedEventHandler(Lis_ItemChecked);
                });
            }
        }

        private void CopySoundFonts()
        {
            String FCB;
            List<String> CD = new List<String>();

            foreach (ListViewItem item in Lis.SelectedItems)
                CD.Add(item.Text);

            FCB = String.Join("\t", CD.ToArray());

            Clipboard.SetText(FCB);
        }

        private void PasteSoundFonts()
        {
            String FCB = Clipboard.GetText();

            if (!String.IsNullOrEmpty(FCB))
            {
                String InvalidChars = new String(Path.GetInvalidPathChars());
                String[] Is = FCB.Split('\t');

                for (int i = 0; i < Is.Count(); i++)
                {
                    foreach (char C in InvalidChars)
                    {
                        Is[i] = Is[i].Replace(C.ToString(), "");
                    }
                }

                ListViewItem[] iSFs = SoundFontListExtension.AddSFToList(Is, false, true);

                if (iSFs != null)
                {
                    foreach (ListViewItem iSF in iSFs)
                        Lis.Items.Add(iSF);

                    SoundFontListExtension.SaveList(ref Lis, SelectedListBox.SelectedIndex, null);
                }
            }
        }

        private void SoundFontListEditor_Load(object sender, EventArgs e)
        {
            // A
            Lis.ItemChecked += new ItemCheckedEventHandler(Lis_ItemChecked);
        }

        private void ReloadListAfterError(Exception ex)
        {
            DialogResult RES = Program.ShowError(
                3,
                "Error",
                String.Format(
                    "Oh snap!\nThe configurator encountered an error while editing the following list:\n{0}\n\nDo you want to reload the list?\n\n{1}",
                    Program.ListsPath[SelectedListBox.SelectedIndex], ex.ToString()
                ),
                null
            );

            if (RES == DialogResult.Yes)
                SoundFontListExtension.ChangeList(SelectedListBox.SelectedIndex, null, false, false);
        }

        private void ColorButton(Button button, Pen pen, PaintEventArgs e)
        {
            // Wew.
            Rectangle rect = button.ClientRectangle;
            rect.Width--;
            rect.Height--;
            e.Graphics.DrawRectangle(pen, rect);
        }

        private void ButtonAddRemove(object sender, PaintEventArgs e)
        {
            ColorButton((Button)sender, Pens.Violet, e);
        }

        private void ButtonUpDown(object sender, PaintEventArgs e)
        {
            ColorButton((Button)sender, Pens.Sienna, e);
        }

        private void ButtonLoad(object sender, PaintEventArgs e)
        {
            ColorButton((Button)sender, new Pen(Color.FromArgb(102, 153, 255)), e);
        }

        private void ImportListButton(object sender, PaintEventArgs e)
        {
            ColorButton((Button)sender, new Pen(Color.FromArgb(114, 141, 208)), e);
        }

        private void ClearListButton(object sender, PaintEventArgs e)
        {
            // Ye, it's not really that easy to explain...
            ColorButton((Button)sender, Pens.BlueViolet, e);
        }

        private void SoundFontListGuideButton(object sender, PaintEventArgs e)
        {
            // Ye, it's not really that easy to explain...
            ColorButton((Button)sender, Pens.Coral, e);
        }

        private void Lis_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            SoundFontListExtension.SaveList(ref Lis, SelectedListBox.SelectedIndex, null);
        }

        private void Lis_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            try
            {
                for (int i = 0; i < Lis.Columns.Count; i++)
                {
                    Properties.Settings.Default.SFColumnsSize[i] = Lis.Columns[i].Width;
                    Properties.Settings.Default.Save();
                }
            }
            catch { Functions.ResetSpecificSetting("SFColumnsSize"); }
        }

        private void Lis_DragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            ListViewItem[] iSFs = SoundFontListExtension.AddSFToList(s, BankPresetOverride.Checked, false);

            if (iSFs != null)
            {
                foreach (ListViewItem iSF in iSFs)
                    Lis.Items.Add(iSF);

                SoundFontListExtension.SaveList(ref Lis, SelectedListBox.SelectedIndex, null);
            }
        }

        private void Lis_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void Lis_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.Control | Keys.C))
                CopySoundFonts();
            else if (e.KeyData == (Keys.Control | Keys.V))
                PasteSoundFonts();
            else if (e.KeyData == (Keys.Control | Keys.Up))
                MoveListViewItems(Lis, MoveDirection.Up);
            else if (e.KeyData == (Keys.Control | Keys.Down))
                MoveListViewItems(Lis, MoveDirection.Down);
        }

        private void Lis_MouseDown(object sender, MouseEventArgs e)
        {
            Lis.PointToClient(new Point(e.X, e.Y));

            switch (e.Button)
            {
                case MouseButtons.Middle:
                    Lis.Invalidate();
                    Lis.SelectedItems.Clear();
                    Lis.GetItemAt(e.X, e.Y).Selected = true;
                    new SoundFontInfo(Lis.GetItemAt(e.X, e.Y).Text).ShowDialog();
                    break;
            }
        }

        private enum MoveDirection { Up = -1, Down = 1 };
        private void MoveListViewItems(ListViewEx sender, MoveDirection direction)
        {
            try
            {
                int dir = (int)direction;
                int opp = dir * -1;

                bool valid = sender.SelectedItems.Count > 0 &&
                            ((direction == MoveDirection.Down && (sender.SelectedItems[sender.SelectedItems.Count - 1].Index < sender.Items.Count - 1))
                            || (direction == MoveDirection.Up && (sender.SelectedItems[0].Index > 0)));

                if (valid)
                {
                    List<int> Is = new List<int>();
                    ListViewItem[] iTBM = sender.SelectedItems.Cast<ListViewItem>().ToArray();

                    IEnumerable<ListViewItem> iTBMEnum;
                    iTBMEnum = (direction == MoveDirection.Down) ? iTBM.Reverse() : iTBM;

                    WinAPI.SendMessage(sender.Handle, WinAPI.WM_SETREDRAW, (IntPtr)0, IntPtr.Zero);
                    foreach (ListViewItem item in iTBMEnum)
                    {
                        int index = item.Index + dir;

                        sender.Items.RemoveAt(item.Index);
                        Is.Add(sender.Items.Insert(index, item).Index);
                    }

                    foreach (int I in Is)
                        sender.Items[I].Selected = true;

                    WinAPI.SendMessage(sender.Handle, WinAPI.WM_SETREDRAW, (IntPtr)1, IntPtr.Zero);
                    sender.Refresh();

                    SoundFontListExtension.SaveList(ref Lis, SelectedListBox.SelectedIndex, null);
                }
            }
            catch (Exception ex)
            {
                ReloadListAfterError(ex);
            }
        }

        private void OSF_Click(object sender, EventArgs e)
        {
            List<String> SFErr = new List<String>();

            foreach (ListViewItem Item in Lis.SelectedItems)
            {
                try { Process.Start(Item.Text); }
                catch { SFErr.Add(Item.Text); }
            }

            if (SFErr.Count > 0)
            {
                Program.ShowError(
                    2,
                    "Unable to open SoundFont(s)",
                    String.Format(
                        "The system was unable to open the following SoundFont(s). Either the SoundFont(s) do(es)n't exist, or there are no applications available for the task.\n\n{0}\n\nPress OK to continue.",
                        String.Join(Environment.NewLine, SFErr.ToArray())),
                    null);
            }
        }

        private void OSFd_Click(object sender, EventArgs e)
        {
            List<String> SFErr = new List<String>();

            foreach (ListViewItem Item in Lis.SelectedItems)
            {
                try { Process.Start("explorer.exe", String.Format("/select, \"{0}\"", Item.Text)); }
                catch { SFErr.Add(Item.Text); }
            }

            if (SFErr.Count > 0)
            {
                Program.ShowError(
                    2,
                    "Unable to open SoundFont(s)",
                    String.Format(
                        "The system was unable to open the parent directory of the following SoundFont(s). Either the SoundFont(s) do(es)n't exist, or Windows Explorer is unable to access the path(s).\n\n{0}\n\nPress OK to continue.",
                        String.Join(Environment.NewLine, SFErr.ToArray())),
                    null);
            }
        }

        private void SelectedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedIndexCSF = (SelectedListBox.SelectedIndex == 0);
            SoundFontListExtension.ChangeList(SelectedListBox.SelectedIndex, null, false, false);
            Properties.Settings.Default.LastListSelected = SelectedListBox.SelectedIndex;
            Properties.Settings.Default.Save();

            if (SelectedListBox.SelectedIndex == 0)
                SoundFontListExtension.StartCSFWatcher();

            // Activate the CSFWatcher now by assigning the events, to avoid a race condition
            SoundFontListExtension.OpenCSFWatcher(true, new FileSystemEventHandler(CSFHandler));
        }

        private void SFlg_Click(object sender, EventArgs e)
        {
            new TextReader("SoundFonts list guide", Properties.Resources.sflistguide).ShowDialog();
        }

        private void CLi_Click(object sender, EventArgs e)
        {
            DialogResult RES = Program.ShowError(1, String.Format("Clear {0}", SelectedListBox.Text), "Are you sure you want to clear this list?", null);
            if (RES == DialogResult.Yes)
            {
                try
                {
                    Lis.Items.Clear();
                    SoundFontListExtension.SaveList(ref Lis, SelectedListBox.SelectedIndex, null);
                    Program.ShowError(0, "Cleaning finished", "The list has been cleaned successfully.", null);

                }
                catch (Exception ex)
                {
                    ReloadListAfterError(ex);
                }
            }
        }

        private void AddSF_Click(object sender, EventArgs e)
        {
            String F = "SoundFont files | ";
            foreach (String Fi in Properties.Settings.Default.SupportedFormats)
                F += String.Format("*{0}; ", Fi);

            OpenFileDialog OFD = new OpenFileDialog()
            {
                Multiselect = true,
                InitialDirectory = Properties.Settings.Default.LastSoundFontPath,
                Filter = F
            };

            try
            {
                if (OFD.ShowDialog(this) == DialogResult.OK)
                {
                    Properties.Settings.Default.LastSoundFontPath = Path.GetDirectoryName(OFD.FileNames[0]);
                    Properties.Settings.Default.Save();

                    ListViewItem[] iSFs = SoundFontListExtension.AddSFToList(OFD.FileNames, BankPresetOverride.Checked, false);

                    if (iSFs != null)
                    {
                        foreach (ListViewItem iSF in iSFs)
                            Lis.Items.Add(iSF);
                    }

                    SoundFontListExtension.SaveList(ref Lis, SelectedListBox.SelectedIndex, null);
                }
            }
            catch (Exception ex)
            {
                ReloadListAfterError(ex);
            }

            OFD.Dispose();
        }

        private void RmvSF_Click(object sender, EventArgs e)
        {
            try
            {
                if (Lis.SelectedIndices.Count != -1 && Lis.SelectedIndices.Count > 0)
                {
                    foreach (int index in Lis.SelectedIndices.Cast<int>().Select(x => x).Reverse())
                        Lis.Items.RemoveAt(index);

                    SoundFontListExtension.SaveList(ref Lis, SelectedListBox.SelectedIndex, null);
                }
            }
            catch (Exception ex)
            {
                ReloadListAfterError(ex);
            }
        }

        private void LoadToApp_Click(object sender, EventArgs e)
        {
            Functions.LoadSoundFontList(SelectedListBox.SelectedIndex);
        }

        private void Preload_Click(object sender, EventArgs e)
        {
            try
            {
                if (Lis.SelectedIndices.Count != -1 && Lis.SelectedIndices.Count > 0)
                {
                    for (int i = Lis.SelectedIndices.Count - 1; i >= 0; i--)
                    {
                        switch (Lis.SelectedItems[i].SubItems[6].Text.ToLowerInvariant())
                        {
                            default:
                            case "yes":
                                Lis.SelectedItems[i].SubItems[6].Text = "No";
                                break;
                            case "no":
                                Lis.SelectedItems[i].SubItems[6].Text = "Yes";
                                break;
                        }
                    }

                    SoundFontListExtension.SaveList(ref Lis, SelectedListBox.SelectedIndex, null);
                }
            }
            catch (Exception ex)
            {
                ReloadListAfterError(ex);
            }
        }

        private void SaveList_Click(object sender, EventArgs e)
        {
            SoundFontListExtension.SaveList(ref Lis, SelectedListBox.SelectedIndex, null);
        }

        private void MvU_Click(object sender, EventArgs e)
        {
            MoveListViewItems(Lis, MoveDirection.Up);
        }

        private void MvD_Click(object sender, EventArgs e)
        {
            MoveListViewItems(Lis, MoveDirection.Down);
        }

        private void IEL_Click(object sender, EventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog()
            {
                Multiselect = true,
                InitialDirectory = Properties.Settings.Default.LastImportExportPath,
                Filter = "Soundfont lists | *.sflist; *.omlist; *.txt;"
            };

            try
            {
                if (OFD.ShowDialog(this) == DialogResult.OK)
                {
                    Properties.Settings.Default.LastImportExportPath = Path.GetDirectoryName(OFD.FileNames[0]);
                    Properties.Settings.Default.Save();

                    foreach (string ListW in OFD.FileNames)
                        SoundFontListExtension.ChangeList(-1, ListW, true, false);

                    SoundFontListExtension.SaveList(ref Lis, SelectedListBox.SelectedIndex, null);

                    Program.ShowError(0, "Import finished", "The selected lists have been imported successfully to the currently selected list in the configurator.", null);
                }
            }
            catch (Exception ex)
            {
                ReloadListAfterError(ex);
            }

            OFD.Dispose();
        }

        private void EL_Click(object sender, EventArgs e)
        {
            SaveFileDialog SFD = new SaveFileDialog()
            {
                OverwritePrompt = true,
                InitialDirectory = Properties.Settings.Default.LastImportExportPath,
                Filter = "Soundfont lists | *.sflist; *.omlist; *.txt;"
            };

            if (SFD.ShowDialog(this) == DialogResult.OK)
            {
                Properties.Settings.Default.LastImportExportPath = Path.GetDirectoryName(SFD.FileName);
                Properties.Settings.Default.Save();

                SoundFontListExtension.SaveList(ref Lis, -1, SFD.FileName);

                Program.ShowError(0, "Export finished", String.Format("The list has been exported successfully to \"{0}\".", SFD.FileName), null);
            }

            SFD.Dispose();
        }

        private void CSFs_Click(object sender, EventArgs e)
        {
            CopySoundFonts();
        }

        private void PSFs_Click(object sender, EventArgs e)
        {
            PasteSoundFonts();
        }
    }
}