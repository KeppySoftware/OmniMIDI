using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms.VisualStyles;
using Microsoft.Win32;
// For SF info
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Midi;
using System.Media;
using System.Net;

namespace KeppySynthConfigurator
{
    public partial class KeppySynthConfiguratorMain : Form
    {
        // Delegate for BasicFunc
        public static KeppySynthConfiguratorMain Delegate;

        public static string LastBrowserPath { get; set; }
        public static string LastImportExportPath { get; set; }

        // SHA256
        [DllImport("keppysynth.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void ReleaseID(StringBuilder SHA256Code, Int32 length);

        // Themes handler
        public static int CurrentTheme = 0;

        // Lists
        public static ListViewItem _itemDnD = null;
        public static string List1PathOld { get; set; }
        public static string List2PathOld { get; set; }
        public static string List3PathOld { get; set; }
        public static string List4PathOld { get; set; }
        public static string List5PathOld { get; set; }
        public static string List6PathOld { get; set; }
        public static string List7PathOld { get; set; }
        public static string List8PathOld { get; set; }

        // Buffer stuff
        public int CurrentIndexFreq { get; set; }

        public static string soundfontnewlocation = System.Environment.GetEnvironmentVariable("USERPROFILE");

        public static string AbsolutePath = soundfontnewlocation + "\\Keppy's Synthesizer";
        public static string ListsPath = soundfontnewlocation + "\\Keppy's Synthesizer\\lists";
        public static string List1Path = soundfontnewlocation + "\\Keppy's Synthesizer\\lists\\keppymidi.sflist";
        public static string List2Path = soundfontnewlocation + "\\Keppy's Synthesizer\\lists\\keppymidib.sflist";
        public static string List3Path = soundfontnewlocation + "\\Keppy's Synthesizer\\lists\\keppymidic.sflist";
        public static string List4Path = soundfontnewlocation + "\\Keppy's Synthesizer\\lists\\keppymidid.sflist";
        public static string List5Path = soundfontnewlocation + "\\Keppy's Synthesizer\\lists\\keppymidie.sflist";
        public static string List6Path = soundfontnewlocation + "\\Keppy's Synthesizer\\lists\\keppymidif.sflist";
        public static string List7Path = soundfontnewlocation + "\\Keppy's Synthesizer\\lists\\keppymidig.sflist";
        public static string List8Path = soundfontnewlocation + "\\Keppy's Synthesizer\\lists\\keppymidih.sflist";
        public static string List9Path = soundfontnewlocation + "\\Keppy's Synthesizer\\lists\\keppymidii.sflist";
        public static string List10Path = soundfontnewlocation + "\\Keppy's Synthesizer\\lists\\keppymidij.sflist";
        public static string List11Path = soundfontnewlocation + "\\Keppy's Synthesizer\\lists\\keppymidik.sflist";
        public static string List12Path = soundfontnewlocation + "\\Keppy's Synthesizer\\lists\\keppymidil.sflist";
        public static string List13Path = soundfontnewlocation + "\\Keppy's Synthesizer\\lists\\keppymidim.sflist";
        public static string List14Path = soundfontnewlocation + "\\Keppy's Synthesizer\\lists\\keppymidin.sflist";
        public static string List15Path = soundfontnewlocation + "\\Keppy's Synthesizer\\lists\\keppymidio.sflist";
        public static string List16Path = soundfontnewlocation + "\\Keppy's Synthesizer\\lists\\keppymidip.sflist";
        // Lists

        // Work
        public static int greenfade = 0;
        public static int openadvanced { get; set; }
        public static int whichone { get; set; }
        public static string CurrentList { get; set; }
        public static bool AvoidSave = false;

        public static RegistryKey SynthSettings = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Settings", true);
        public static RegistryKey Channels = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Channels", true);
        public static RegistryKey Watchdog = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Watchdog", true);
        public static RegistryKey SynthPaths = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Keppy's Synthesizer\\Paths", true);

        public KeppySynthConfiguratorMain(String[] args)
        {
            InitializeComponent();
            Delegate = this;
            VolTrackBar.BackColor = Color.Empty;
            this.FormClosing += new FormClosingEventHandler(CloseConfigurator);
            try
            {
                foreach (String s in args)
                {
                    switch (s.Substring(0, 4).ToUpper())
                    {
                        case "/AST":
                            openadvanced = 1;
                            break;
                        case "/MIX":
                            System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\keppysynth\\KeppySynthMixerWindow.exe");
                            return;
                        default:
                            // do other stuff...
                            break;
                    }
                }
            }
            catch
            {

            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == (int)Program.BringToFrontMessage)
            {
                WinAPI.ShowWindow(Handle, WinAPI.SW_RESTORE);
                WinAPI.SetForegroundWindow(Handle);
            }
        }

        private void KeppySynthConfiguratorMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Bass.BASS_Free();
            Bass.FreeMe();
            SynthSettings.Close();
            Watchdog.Close();
        }

        // Just stuff to reduce code's length
        private void SFZCompliant()
        {
            MessageBox.Show("This driver is \"SFZ format 2.0\" compliant.", "SFZ format support", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AddSoundfontDragNDrop(String SelectedList, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            Functions.AddSoundfontsToSelectedList(CurrentList, s);
        }

        public void AddSoundfontDragNDropTriv(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        // Here we go!
        private void KeppySynthConfiguratorMain_Load(object sender, EventArgs e)
        {
            try
            {
                // SAS THEME HANDLER   
                Bass.LoadMe();
                Lis.Columns[0].Tag = 7;
                Lis.Columns[1].Tag = 1;
                Lis.Columns[2].Tag = 1;
                Lis_SizeChanged(Lis, new EventArgs());
                this.ThemeCheck.RunWorkerAsync();
                this.Size = new Size(665, 481);
                // MIDI out selector disabler
                Functions.CheckMIDIMapper();

                if (!Properties.Settings.Default.ButterBoy)
                {
                    EnableBB.Visible = true;
                    EnableBBS.Visible = true;
                }

                CLi.Image = Properties.Resources.ClearIcon;
                AddSF.Image = Properties.Resources.AddSFIcon;
                RmvSF.Image = Properties.Resources.RmvSFIcon;
                MvU.Image = Properties.Resources.MvUpIcon;
                MvD.Image = Properties.Resources.MvDwIcon;
                LoadToApp.Image = Properties.Resources.ReloadIcon;
                EnableSF.Image = Properties.Resources.EnableIcon;
                DisableSF.Image = Properties.Resources.DisableIcon;
                IEL.Image = Properties.Resources.ImportIcon;
                EL.Image = Properties.Resources.ExportIcon;
                WhatIsOutput.Image = Properties.Resources.what;
                WhatIsXAudio.Image = Properties.Resources.what;
                StatusBuf.Image = Properties.Resources.what;

                Functions.InitializeLastPath();
                SelectedListBox.Text = "List 1";
                KeppySynthConfiguratorMain.whichone = 1;

                Functions.LoadSettings();

                // If /AS is specified, switch to the Settings tab automatically
                if (openadvanced == 1)
                {
                    TabsForTheControls.SelectedIndex = 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void VolTrackBar_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (VolTrackBar.Value <= 49)
                    VolSimView.ForeColor = Color.Red;
                else
                    VolSimView.ForeColor = Color.Blue;

                decimal VolVal = (decimal)VolTrackBar.Value / 100;
                VolSimView.Text = String.Format("{0}%", Math.Round(VolVal, MidpointRounding.AwayFromZero).ToString("000"));
                VolIntView.Text = String.Format("Real value: {0}%", VolVal.ToString("000.00"));
                SynthSettings.SetValue("volume", VolTrackBar.Value.ToString(), RegistryValueKind.DWord);
            }
            catch (Exception ex)
            {
                Functions.ShowErrorDialog(1, System.Media.SystemSounds.Asterisk, "Error", "Error during access to the registry!", true, ex);
            }
        }

        private void ExportSettings_Click(object sender, EventArgs e)
        {
            if (ExportSettingsDialog.ShowDialog() == DialogResult.OK)
            {
                Functions.ExportSettings(ExportSettingsDialog.FileName);
                MessageBox.Show("The settings have been exported to the selected registry file!", "Keppy's Synthesizer - Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ImportSettings_Click(object sender, EventArgs e)
        {
            if (ImportSettingsDialog.ShowDialog() == DialogResult.OK)
            {
                Functions.ImportSettings(ImportSettingsDialog.FileName);
                MessageBox.Show("The settings have been imported from the selected registry file!", "Keppy's Synthesizer - Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void CLi_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to clear the list?", "Keppy's Synthesizer Configurator ~ Clear list " + whichone.ToString(), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    Lis.Items.Clear();
                    File.Delete(CurrentList);
                    var TempFile = File.Create(CurrentList);
                    TempFile.Close();
                    if (Convert.ToInt32(Watchdog.GetValue("currentsflist")) == whichone)
                    {
                        Watchdog.SetValue("rel" + whichone.ToString(), "1", RegistryValueKind.DWord);
                    }
                }
                catch (Exception ex)
                {
                    Functions.ReinitializeList(ex, CurrentList);
                }
            }
        }

        private void Lis_MouseDown(object sender, MouseEventArgs e)
        {
            Lis.PointToClient(new Point(e.X, e.Y));
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (Lis.SelectedIndices.Count == 1)
                        _itemDnD = Lis.GetItemAt(e.X, e.Y);
                    break;

                case MouseButtons.Right:
                    RightClickMenu.Show(Lis, new Point(e.X, e.Y));
                    break;

                case MouseButtons.Middle:
                    SelectedSFInfo(e);
                    break;
            }
        }

        private void Lis_MouseMove(object sender, MouseEventArgs e)
        {
            if (_itemDnD == null || Lis.SelectedIndices.Count > 1)
                return;

            Cursor = Cursors.UpArrow;

            int lastItemBottom = Math.Min(e.Y, Lis.Items[Lis.Items.Count - 1].GetBounds(ItemBoundsPortion.Entire).Bottom - 1);

            ListViewItem itemOver = Lis.GetItemAt(0, lastItemBottom);

            if (itemOver == null)
                return;

            Rectangle rc = itemOver.GetBounds(ItemBoundsPortion.Entire);
            if (e.Y < rc.Top + (rc.Height / 2))
            {
                Lis.LineBefore = itemOver.Index;
                Lis.LineAfter = -1;
            }
            else
            {
                Lis.LineBefore = -1;
                Lis.LineAfter = itemOver.Index;
            }

            Lis.Invalidate();
        }

        private void Lis_MouseUp(object sender, MouseEventArgs e)
        {
            if (_itemDnD == null || Lis.SelectedIndices.Count > 1)
                return;

            try
            {
                int lastItemBottom = Math.Min(e.Y, Lis.Items[Lis.Items.Count - 1].GetBounds(ItemBoundsPortion.Entire).Bottom - 1);

                ListViewItem itemOver = Lis.GetItemAt(0, lastItemBottom);

                if (itemOver == null)
                    return;

                Rectangle rc = itemOver.GetBounds(ItemBoundsPortion.Entire);

                bool insertBefore;
                if (e.Y < rc.Top + (rc.Height / 2))
                {
                    insertBefore = true;
                }
                else
                {
                    insertBefore = false;
                }

                if (_itemDnD != itemOver)
                {
                    if (insertBefore)
                    {
                        Lis.Items.Remove(_itemDnD);
                        Lis.Items.Insert(itemOver.Index, _itemDnD);
                    }
                    else
                    {
                        Lis.Items.Remove(_itemDnD);
                        Lis.Items.Insert(itemOver.Index + 1, _itemDnD);
                    }
                }

                Lis.LineAfter =
                Lis.LineBefore = -1;

                Lis.Invalidate();
            }
            finally
            {
                _itemDnD = null;
                Cursor = Cursors.Default;
                Functions.SaveList(CurrentList);
            }
        }

        private void OpenSFDefaultApp_Click(object sender, EventArgs e)
        {
            OpenSoundFont();
        }

        private void OpenSFMainDirectory_Click(object sender, EventArgs e)
        {
            OpenSoundFontDirectory();
        }

        private void OpenSoundFont()
        {
            try
            {
                int howmany = Lis.SelectedItems.Count;
                if (howmany == 0)
                {
                    MessageBox.Show("Select a SoundFont first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (howmany == 1)
                {
                    String name = Lis.SelectedItems[0].Text.ToString();
                    Functions.OpenSFWithDefaultApp(name);
                    Program.DebugToConsole(false, String.Format("Opened soundfont from list: {0}", name), null);
                }
                else if (howmany > 1)
                {
                    DialogResult dialogResult = MessageBox.Show("Are you sure you want to open multiple SoundFonts at the same time?\n\nDoing so could make your computer lag, or in worst cases, hang.", "Keppy's Synthesizer - Open multiple SoundFonts", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialogResult == DialogResult.Yes)
                    {
                        for (int i = Lis.SelectedIndices.Count - 1; i >= 0; i--)
                        {
                            String name = Lis.SelectedItems[i].Text.ToString();
                            Functions.OpenSFWithDefaultApp(name);
                            Program.DebugToConsole(false, String.Format("Opened soundfont from list: {0}", name), null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Functions.ReinitializeList(ex, CurrentList);
            }
        }

        private void OpenSoundFontDirectory()
        {
            try
            {
                int howmany = Lis.SelectedItems.Count;
                if (howmany == 0)
                {
                    MessageBox.Show("Select a SoundFont first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (howmany == 1)
                {
                    String name = Lis.SelectedItems[0].Text.ToString();
                    Process.Start(Path.GetDirectoryName(name));
                    Program.DebugToConsole(false, String.Format("Opened soundfont directory from list: {0}", name), null);
                }
                else if (howmany > 1)
                {
                    MessageBox.Show("You can't open folders from multiple SoundFonts!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                Functions.ReinitializeList(ex, CurrentList);
            }
        }

        private void AddSF_Click(object sender, EventArgs e)
        {
            try
            {
                SoundfontImport.InitialDirectory = LastBrowserPath;
                SoundfontImport.FileName = "";
                Functions.OpenFileDialogAddCustomPaths(SoundfontImport);
                if (SoundfontImport.ShowDialog(this) == DialogResult.OK)
                {
                    Functions.SetLastPath(Path.GetDirectoryName(SoundfontImport.FileNames[0]));
                    Functions.AddSoundfontsToSelectedList(CurrentList, SoundfontImport.FileNames);
                }
            }
            catch (Exception ex)
            {
                Functions.ReinitializeList(ex, CurrentList);
            }
        }

        private void RmvSF_Click(object sender, EventArgs e)
        {
            try
            {
                if (Lis.SelectedIndices.Count < 1)
                {
                    MessageBox.Show("Select a soundfont first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                for (int i = Lis.SelectedIndices.Count - 1; i >= 0; i--)
                {
                    String name = Lis.SelectedItems[i].Text.ToString();
                    Lis.Items.RemoveAt(Lis.SelectedIndices[i]);
                    Program.DebugToConsole(false, String.Format("Removed soundfont from list: {0}", name), null);
                    Functions.SaveList(CurrentList);
                    Functions.TriggerReload();
                }
            }
            catch (Exception ex)
            {
                Functions.ReinitializeList(ex, CurrentList);
            }
        }

        private void Lis_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (Lis.SelectedIndices.Count != -1 && Lis.SelectedIndices.Count > 0)
                {
                    for (int i = Lis.SelectedIndices.Count - 1; i >= 0; i--)
                    {
                        String name = Lis.SelectedItems[i].Text.ToString();
                        Lis.Items.RemoveAt(Lis.SelectedIndices[i]);
                        Program.DebugToConsole(false, String.Format("Removed soundfont from list: {0}", name), null);
                        Functions.SaveList(CurrentList);
                        Functions.TriggerReload();
                    }
                }
            }
            catch (Exception ex)
            {
                Functions.ReinitializeList(ex, CurrentList);
            }
        }

        private void SelectedSFInfo(MouseEventArgs e)
        {
            AvoidSave = true;
            if (Lis.GetItemAt(e.X, e.Y) != null)
            {
                try
                {
                    _itemDnD = null;
                    Lis.LineAfter =
                    Lis.LineBefore = -1;
                    Lis.Invalidate();
                    Lis.SelectedItems.Clear();
                    Lis.GetItemAt(e.X, e.Y).Selected = true;
                    Program.DebugToConsole(false, String.Format("Currently showing info for soundfont: {0}", Lis.GetItemAt(e.X, e.Y).Text.ToString()), null);
                    SoundFontInfo frm = new SoundFontInfo(Lis.GetItemAt(e.X, e.Y).Text.ToString());
                    if (!SoundFontInfo.ERROR)
                    {
                        frm.ShowDialog(this);
                    }
                    SoundFontInfo.ERROR = false;
                    SoundFontInfo.Quitting = false;
                    frm.Dispose();
                }
                catch (Exception ex)
                {
                    Functions.ShowErrorDialog(1, System.Media.SystemSounds.Hand, "Fatal error", "Fatal error during the execution of this program!\n\nPress OK to quit.", true, ex);
                    Environment.Exit(-1);
                }
            }
            AvoidSave = false;
        }

        private void MvU_Click(object sender, EventArgs e)
        {
            MoveListViewItems(Lis, MoveDirection.Up);
        }

        private void MvD_Click(object sender, EventArgs e)
        {
            MoveListViewItems(Lis, MoveDirection.Down);
        }

        private enum MoveDirection { Up = -1, Down = 1 };
        private static void MoveListViewItems(ListView sender, MoveDirection direction)
        {
            int dir = (int)direction;
            int opp = dir * -1;

            bool valid = sender.SelectedItems.Count > 0 &&
                            ((direction == MoveDirection.Down && (sender.SelectedItems[sender.SelectedItems.Count - 1].Index < sender.Items.Count - 1))
                        || (direction == MoveDirection.Up && (sender.SelectedItems[0].Index > 0)));

            if (valid)
            {
                foreach (ListViewItem item in sender.SelectedItems)
                {
                    string str = item.Text;
                    int index = item.Index + dir;

                    sender.Items.RemoveAt(item.Index);
                    sender.Items.Insert(index, item);
                }
                Functions.SaveList(CurrentList);
                Functions.TriggerReload();
            }
            else if (valid || KeppySynthConfiguratorMain.Delegate.Lis.SelectedIndices.Count < 1 || KeppySynthConfiguratorMain.Delegate.Lis.SelectedIndices.Count > 1)
            {
                MessageBox.Show("Select a soundfont first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadToApp_Click(object sender, EventArgs e)
        {
            Watchdog.SetValue("currentsflist", whichone, RegistryValueKind.DWord);
            Watchdog.SetValue("rel" + whichone.ToString(), "1", RegistryValueKind.DWord);
            Program.DebugToConsole(false, String.Format("(Re)Loaded soundfont list {0}.", whichone), null);
        }

        private void EnableSF_Click(object sender, EventArgs e)
        {
            SFEnableDisableSwitch(true);
        }

        private void DisableSF_Click(object sender, EventArgs e)
        {
            SFEnableDisableSwitch(false);
        }

        private void menuItem35_Click(object sender, EventArgs e)
        {
            SFEnableDisableSwitch(true);
        }

        private void menuItem38_Click(object sender, EventArgs e)
        {
            SFEnableDisableSwitch(false);
        }

        private void SFEnableDisableSwitch(Boolean Enable)
        {
            try
            {
                if (Lis.SelectedIndices.Count < 1)
                {
                    MessageBox.Show("Select a SoundFont first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                for (int i = Lis.SelectedIndices.Count - 1; i >= 0; i--)
                {
                    if (Enable)
                    {
                        if (Lis.SelectedItems[i].ForeColor != Functions.SFEnabled)
                        {
                            Lis.SelectedItems[i].ForeColor = Functions.SFEnabled;
                            Functions.SaveList(CurrentList);
                            Functions.TriggerReload();
                            Program.DebugToConsole(false, String.Format("Enabled soundfont: {0}", Lis.SelectedItems[i].Text), null);
                        }
                    }
                    else
                    {
                        if (Lis.SelectedItems[i].ForeColor != Functions.SFDisabled)
                        {
                            Lis.SelectedItems[i].ForeColor = Functions.SFDisabled;
                            Functions.SaveList(CurrentList);
                            Functions.TriggerReload();
                            Program.DebugToConsole(false, String.Format("Disabled soundfont: {0}", Lis.SelectedItems[i].Text), null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Functions.ReinitializeList(ex, CurrentList);
            }
        }

        private void IEL_Click(object sender, EventArgs e)
        {
            try
            {
                ExternalListImport.FileName = "";
                ExternalListImport.InitialDirectory = LastImportExportPath;
                if (ExternalListImport.ShowDialog(this) == DialogResult.OK)
                {
                    Functions.SetLastImportExportPath(Path.GetDirectoryName(ExternalListImport.FileNames[0]));
                    foreach (string file in ExternalListImport.FileNames)
                    {
                        using (StreamReader r = new StreamReader(file))
                        {
                            List<string> SFList = new List<string>();
                            string line;
                            while ((line = r.ReadLine()) != null) // Read the external list and add the items to the selected list
                            {

                                bool isabsolute = Path.IsPathRooted(line);  // Check if the path to the soundfont is absolute or relative
                                string relativepath;
                                string absolutepath;
                                if (isabsolute == false) // Not absolute, let's convert it
                                {
                                    relativepath = String.Format("{0}{1}", Path.GetDirectoryName(file), String.Format("\\{0}", line));
                                    absolutepath = new Uri(relativepath).LocalPath;
                                    SFList.Add(absolutepath);
                                }
                                else // Absolute, let's just add it straight away
                                {
                                    SFList.Add(line);
                                }                           
                            }
                            Functions.AddSoundfontsToSelectedList(CurrentList, SFList.ToArray());
                        }
                        Functions.SaveList(CurrentList);
                        Functions.TriggerReload();
                    }
                }
            }
            catch (Exception ex)
            {
                Functions.ShowErrorDialog(1, System.Media.SystemSounds.Hand, "Error", "Error during the import process of the list!", true, ex);
            }
        }

        private void EL_Click(object sender, EventArgs e)
        {
            ExternalListExport.FileName = "";
            ExternalListExport.InitialDirectory = LastImportExportPath;
            if (ExternalListExport.ShowDialog(this) == DialogResult.OK)
            {
                Functions.SetLastImportExportPath(Path.GetDirectoryName(ExternalListExport.FileNames[0]));
                System.IO.StreamWriter SaveFile = new System.IO.StreamWriter(ExternalListExport.FileName);
                Functions.SetLastPath(LastBrowserPath);
                foreach (ListViewItem item in Lis.Items)
                {
                    SaveFile.WriteLine(item.Text.ToString());
                }
                SaveFile.Close();
                Program.DebugToConsole(false, String.Format("Exported list {0} to {1}.", CurrentList, ExternalListExport.FileName), null);
                Functions.ShowErrorDialog(1, System.Media.SystemSounds.Question, "Soundfont list exported!", String.Format("Soundfont list exported succesfully to \"{0}\\\"", Path.GetDirectoryName(ExternalListExport.FileName)), false, null);               
            }
        }

        // Color buttons

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
            // I bet you're wondering,
            ColorButton(AddSF, Pens.Violet, e);
        }

        private void ButtonUpDown(object sender, PaintEventArgs e)
        {
            // How can it set the color to two buttons, if you're only parsing one?
            ColorButton(MvU, Pens.Sienna, e);
        }

        private void ButtonEnable(object sender, PaintEventArgs e)
        {
            // Well, it's quite simple,
            ColorButton(EnableSF, Pens.Green, e);
        }

        private void ButtonDisable(object sender, PaintEventArgs e)
        {
            // Well, it's quite simple,
            ColorButton(DisableSF, Pens.Red, e);
        }

        private void ButtonLoad(object sender, PaintEventArgs e)
        {
            // The button parsed is only used to calculate the area of the rectangle,
            ColorButton(LoadToApp, Pens.PowderBlue, e);
        }

        private void ImportListButton(object sender, PaintEventArgs e)
        {
            // The actual button that is going to get the color is parsed through "e",
            ColorButton(IEL, new Pen(Color.FromArgb(114, 141, 208)), e);
        }

        private void ClearListButton(object sender, PaintEventArgs e)
        {
            // Ye, it's not really that easy to explain...
            ColorButton(CLi, Pens.BlueViolet, e);
        }

        // Stuff

        private void SelectedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Functions.ChangeList(SelectedListBox.SelectedIndex + 1);
        }

        private void Lis_DragDrop(object sender, DragEventArgs e)
        {
            AddSoundfontDragNDrop(CurrentList, e);
        }

        private void Lis_DragEnter(object sender, DragEventArgs e)
        {
            AddSoundfontDragNDropTriv(e);
        }

        // End of the soundfont lists functions
        // ------------------------------------

        public static void DeleteDirectory(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(target_dir, false);
        }

        private void DeleteUserData_Click(object sender, EventArgs e)
        {
            Boolean RestartAfterDelete = false;
            DialogResult dialogResult = MessageBox.Show("Deleting the driver's user data will delete all the SoundFont lists, the DLL overrides and will also uninstall LoudMax.\nThis action is irreversible!\n\nAre you sure you want to continue?\nAfter deleting the data, the configurator will restart.", "Keppy's Synthesizer - Clear driver's user data", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                DialogResult dialogResult2 = MessageBox.Show("Would you like to restart the configurator after the process?", "Keppy's Synthesizer - Clear driver's user data", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult2 == DialogResult.Yes)
                    RestartAfterDelete = true;

                DeleteDirectory(System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\Keppy's Synthesizer\\");
                if (RestartAfterDelete)
                {
                    System.Diagnostics.Process.Start(Application.ExecutablePath);
                    this.Close();
                }
                else
                {
                    this.Close();
                }
            }
        }

        private void resetToDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Set some values...
            VolTrackBar.Value = 10000;
            PolyphonyLimit.Value = 512;
            MaxCPU.Value = 65;
            Frequency.Text = "48000";
            bufsize.Value = 30;
            SPFRate.Value = 100;
            Preload.Checked = true;
            NoteOffCheck.Checked = false;
            SincInter.Checked = false;
            EnableSFX.Checked = false;
            SysResetIgnore.Checked = false;
            OutputWAV.Checked = false;
            KeppySynthConfiguratorMain.Delegate.AudioEngBox.Text = "XAudio";
            ManualAddBuffer.Checked = false;

            // And then...
            Functions.SaveSettings();

            // Messagebox here
            Program.DebugToConsole(false, "Settings restored.", null);
            MessageBox.Show("Settings restored to the default values!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void applySettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Just save the Settings
            Functions.SaveSettings();
            EnableFade();

            // Messagebox here
            Program.DebugToConsole(false, "Applied new settings.", null);
        }

        private void blackMIDIsPresetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Set some values...
            VolTrackBar.Value = 10000;
            PolyphonyLimit.Value = 1000;
            MaxCPU.Value = 75;
            Frequency.Text = "44100";
            Preload.Checked = true;
            NoteOffCheck.Checked = false;
            SincInter.Checked = false;
            EnableSFX.Checked = false;
            SysResetIgnore.Checked = true;
            OutputWAV.Checked = false;
            KeppySynthConfiguratorMain.Delegate.AudioEngBox.Text = "WASAPI";
            bufsize.Value = 20;
            SPFRate.Value = 100;
            AudioEngBox_SelectedIndexChanged(null, null);
            ManualAddBuffer.Checked = false;

            // And then...
            Functions.SaveSettings();

            // Messagebox here
            MessageBox.Show("The Black MIDIs preset has been applied!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void lowLatencyPresetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Set some values...
            VolTrackBar.Value = 10000;
            PolyphonyLimit.Value = 500;
            MaxCPU.Value = 80;
            Frequency.Text = "44100";
            Preload.Checked = true;
            NoteOffCheck.Checked = false;
            SincInter.Checked = true;
            EnableSFX.Checked = true;
            SysResetIgnore.Checked = false;
            OutputWAV.Checked = false;
            KeppySynthConfiguratorMain.Delegate.AudioEngBox.Text = "XAudio";
            bufsize.Value = 20;
            SPFRate.Value = 100;
            AudioEngBox_SelectedIndexChanged(null, null);
            ManualAddBuffer.Checked = false;

            // And then...
            Functions.SaveSettings();

            // Messagebox here
            MessageBox.Show("The low latency preset has been applied!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void chiptunesRetrogamingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Set some values...
            VolTrackBar.Value = 10000;
            PolyphonyLimit.Value = 64;
            MaxCPU.Value = 0;
            Frequency.Text = "22050";
            KeppySynthConfiguratorMain.Delegate.AudioEngBox.Text = "DirectSound";
            AudioEngBox_SelectedIndexChanged(null, null);
            bufsize.Value = 0;
            SPFRate.Value = 100;
            Preload.Checked = true;
            NoteOffCheck.Checked = false;
            SincInter.Checked = false;
            EnableSFX.Checked = true;
            SysResetIgnore.Checked = false;
            OutputWAV.Checked = false;
            ManualAddBuffer.Checked = false;

            // And then...
            Functions.SaveSettings();

            // Messagebox here
            MessageBox.Show("The chiptunes/retrogaming preset has been applied!\n\n\"The NES soundfont\" is recommended for chiptunes.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void keppysSteinwayPianoRealismToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Set some values...
            VolTrackBar.Value = 10000;
            PolyphonyLimit.Value = 850;
            MaxCPU.Value = 85;
            Frequency.Text = "66150";
            bufsize.Value = 40;
            SPFRate.Value = 80;
            Preload.Checked = true;
            NoteOffCheck.Checked = true;
            SincInter.Checked = true;
            EnableSFX.Checked = true;
            SysResetIgnore.Checked = true;
            OutputWAV.Checked = false;
            KeppySynthConfiguratorMain.Delegate.AudioEngBox.Text = "DirectSound";
            AudioEngBox_SelectedIndexChanged(null, null);
            ManualAddBuffer.Checked = false;

            // And then...
            Functions.SaveSettings();

            // Messagebox here
            MessageBox.Show("\"High fidelity audio (For HQ SoundFonts)\" has been applied!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SBLowLatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Set some values...
            VolTrackBar.Value = 10000;
            PolyphonyLimit.Value = 1000;
            MaxCPU.Value = 75;
            Frequency.Text = "48000";
            bufsize.Value = 15;
            SPFRate.Value = 75;
            Preload.Checked = true;
            NoteOffCheck.Checked = false;
            SincInter.Checked = false;
            EnableSFX.Checked = true;
            SysResetIgnore.Checked = false;
            OutputWAV.Checked = false;
            KeppySynthConfiguratorMain.Delegate.AudioEngBox.Text = "WASAPI";
            bufsize.Value = 15;
            SPFRate.Value = 100;
            AudioEngBox_SelectedIndexChanged(null, null);
            ManualAddBuffer.Checked = false;

            // And then...
            Functions.SaveSettings();

            // Messagebox here
            MessageBox.Show("\"SoundBlaster - Low Latency\" has been applied!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Now, menustrip functions here

        private void openDebugWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\keppysynth\\KeppySynthDebugWindow.exe");
        }

        private void openTheMixerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\keppysynth\\KeppySynthMixerWindow.exe");
        }

        private void openTheBlacklistManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KeppySynthBlacklistSystem frm = new KeppySynthBlacklistSystem();
            frm.ShowDialog(this);
            frm.Dispose();
        }

        private void informationAboutTheDriverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InfoDialog frm = new InfoDialog(0);
            frm.ShowDialog(this);
            frm.Dispose();
        }

        private void SeeChangelog_Click(object sender, EventArgs e)
        {
            FileVersionInfo Driver = FileVersionInfo.GetVersionInfo(UpdateSystem.UpdateFileVersion);
            ChangelogWindow frm = new ChangelogWindow(Driver.FileVersion.ToString());
            frm.ShowDialog(this);
            frm.Dispose();
        }

        private void SeeLatestChangelog_Click(object sender, EventArgs e)
        {
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(UpdateSystem.UpdateTextFile);
            StreamReader reader = new StreamReader(stream);
            String newestversion = reader.ReadToEnd();
            Version x = null;
            Version.TryParse(newestversion.ToString(), out x);
            ChangelogWindow frm = new ChangelogWindow(x.ToString());
            frm.ShowDialog(this);
            frm.Dispose();
        }

        private void changeDefaultMIDIOutDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\keppysynth\\midioutsetter32.exe");
        }

        private void changeDefault64bitMIDIOutDeviceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\keppysynth\\midioutsetter64.exe");
        }

        private void openUpdaterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                UpdateSystem.CheckForUpdates(true, false);
            }
            else
            {
                UpdateSystem.CheckForUpdates(false, false);
            }
        }

        private void LoudMaxInstallMenu_Click(object sender, EventArgs e)
        {
            if (!floatingpointaudio.Checked)
            {
                DialogResult dialogResult = MessageBox.Show("LoudMax is useless without 32-bit float audio rendering.\nPlease enable it by going to \"Additional settings > Advanced audio settings > Audio bit depth\".\n\nDo you want to continue anyway?", "Keppy's Synthesizer - LoudMax", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    Functions.LoudMaxInstall();
                }
            }
            else
            {
                Functions.LoudMaxInstall();
            }
        }

        private void LoudMaxUninstallMenu_Click(object sender, EventArgs e)
        {
            Functions.LoudMaxUninstall();
        }

        private void DLLOverrideFolder_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(soundfontnewlocation + "\\Keppy's Synthesizer\\dlloverride"))
            {
                Directory.CreateDirectory(soundfontnewlocation + "\\Keppy's Synthesizer\\dlloverride");
                File.WriteAllText(soundfontnewlocation + "\\Keppy's Synthesizer\\dlloverride\\What's DLL override.txt", KeppySynthConfigurator.Properties.Resources.whatoverride);
            }
            if (!Directory.Exists(soundfontnewlocation + "\\Keppy's Synthesizer\\dlloverride\\32"))
            {
                Directory.CreateDirectory(soundfontnewlocation + "\\Keppy's Synthesizer\\dlloverride\\32");
                File.Create(soundfontnewlocation + "\\Keppy's Synthesizer\\dlloverride\\32\\PUT 32-BIT DLLs HERE").Dispose();
            }
            if (!Directory.Exists(soundfontnewlocation + "\\Keppy's Synthesizer\\dlloverride\\64"))
            {
                Directory.CreateDirectory(soundfontnewlocation + "\\Keppy's Synthesizer\\dlloverride\\64");
                File.Create(soundfontnewlocation + "\\Keppy's Synthesizer\\dlloverride\\64\\PUT 64-BIT DLLs HERE").Dispose();
            }

            Process.Start(new ProcessStartInfo()
            {
                FileName = soundfontnewlocation + "\\Keppy's Synthesizer\\dlloverride",
                UseShellExecute = true,
                Verb = "open"
            });
        }

        private void GiveFeedback_Click(object sender, EventArgs e)
        {
            Process.Start("https://goo.gl/forms/sLlghX6mzr3FWG5i2");
        }

        private void reportABugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to report a bug about Keppy's Synthesizer?\n\nHere are the requisites for a report:\n1) Make a video of the issue.\n2) Describe all the steps to reproduce the bug.\n3) Please give as much information as you can, to allow me (KaleidonKep99) to fix it as soon as possible.", "Report a bug...", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                Process.Start("https://github.com/KaleidonKep99/Keppy-s-MIDI-Driver/issues");
            }
            else if (dialogResult == DialogResult.No)
            {

            }
        }

        private void downloadTheSourceCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/KaleidonKep99/Keppy-s-MIDI-Driver");
        }

        private void visitKeppyStudiosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://keppystudios.com");
        }

        private void getTheMIDIMapperForWindows8xToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://drive.google.com/file/d/0B05Sp4zxPFR6UW9CQ0RRak85eDA/view?usp=sharing");
        }

        private void getTheMIDIMapperForWindows10ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://plus.google.com/+RichardForhenson/posts/bkrqUfbV3xz");
        }

        private void donateToSupportUsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = "";

            string business = "prapapappo1999@gmail.com";
            string description = "Donation";
            string country = "US";
            string currency = "USD";

            url += "https://www.paypal.com/cgi-bin/webscr" +
                "?cmd=" + "_donations" +
                "&business=" + business +
                "&lc=" + country +
                "&item_name=" + description +
                "&currency_code=" + currency +
                "&bn=" + "PP%2dDonationsBF";

            Process.Start(url);
        }

        private void CapFram_Click(object sender, EventArgs e)
        {
            if (!CapFram.Checked)
            {
                CapFram.Checked = true;
                Functions.SetFramerate(1);
            }
            else
            {
                CapFram.Checked = false;
                Functions.SetFramerate(0);
            }
        }

        private void changeTheSizeOfTheEVBufferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KeppySynthEVBuffer frm = new KeppySynthEVBuffer();
            frm.ShowDialog(this);
            frm.Dispose();
        }

        private void changeDirectoryOfTheOutputToWAVModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KeppySynthOutputWAVDir frm = new KeppySynthOutputWAVDir();
            frm.ShowDialog(this);
            frm.Dispose();
        }

        private void changeDefaultSoundfontListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KeppySynthDefaultSFList frm = new KeppySynthDefaultSFList();
            frm.ShowDialog(this);
            frm.Dispose();
        }

        private void changeDefaultSoundfontListToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            KeppySynthDefaultSFList frm = new KeppySynthDefaultSFList();
            frm.ShowDialog(this);
            frm.Dispose();
        }

        private void assignASoundfontListToASpecificAppToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KeppySynthSFListAssign frm = new KeppySynthSFListAssign();
            frm.ShowDialog(this);
            frm.Dispose();
        }

        private void manageFolderFavouritesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KeppySynthFavouritesManager frm = new KeppySynthFavouritesManager();
            frm.ShowDialog(this);
            frm.Dispose();
        }

        private void RegDriver_Click(object sender, EventArgs e)
        {
            Functions.DriverRegistry(0);
        }

        private void UnregDriver_Click(object sender, EventArgs e)
        {
            Functions.DriverRegistry(1);
        }

        private void AMIDIMapInstallMenu_Click(object sender, EventArgs e)
        {
            Functions.MIDIMapRegistry(0);
        }

        private void AMIDIMapUninstallMenu_Click(object sender, EventArgs e)
        {
            Functions.MIDIMapRegistry(1);
        }

        // Priority stuff

        private void DePrio_Click(object sender, EventArgs e)
        {
            if (!DePrio.Checked)
            {
                Functions.ButtonStatus(false);
                DePrio.Checked = true;
                RTPrio.Checked = false;
                HiPrio.Checked = false;
                HNPrio.Checked = false;
                NoPrio.Checked = false;
                LNPrio.Checked = false;
                LoPrio.Checked = false;
                Functions.SetDriverPriority(0);
            }
            else
            {
                Functions.ButtonStatus(true);
                DePrio.Checked = false;
                RTPrio.Checked = true;
                HiPrio.Checked = false;
                HNPrio.Checked = false;
                NoPrio.Checked = false;
                LNPrio.Checked = false;
                LoPrio.Checked = false;
                Functions.SetDriverPriority(1);
            }
        }

        private void RTPrio_Click(object sender, EventArgs e)
        {
            Functions.ButtonStatus(true);
            DePrio.Checked = false;
            RTPrio.Checked = true;
            HiPrio.Checked = false;
            HNPrio.Checked = false;
            NoPrio.Checked = false;
            LNPrio.Checked = false;
            LoPrio.Checked = false;
            Functions.SetDriverPriority(1);
        }

        private void HiPrio_Click(object sender, EventArgs e)
        {
            Functions.ButtonStatus(true);
            DePrio.Checked = false;
            RTPrio.Checked = false;
            HiPrio.Checked = true;
            HNPrio.Checked = false;
            NoPrio.Checked = false;
            LNPrio.Checked = false;
            LoPrio.Checked = false;
            Functions.SetDriverPriority(2);
        }

        private void HNPrio_Click(object sender, EventArgs e)
        {
            Functions.ButtonStatus(true);
            DePrio.Checked = false;
            RTPrio.Checked = false;
            HiPrio.Checked = false;
            HNPrio.Checked = true;
            NoPrio.Checked = false;
            LNPrio.Checked = false;
            LoPrio.Checked = false;
            Functions.SetDriverPriority(3);
        }

        private void NoPrio_Click(object sender, EventArgs e)
        {
            Functions.ButtonStatus(true);
            DePrio.Checked = false;
            RTPrio.Checked = false;
            HiPrio.Checked = false;
            HNPrio.Checked = false;
            NoPrio.Checked = true;
            LNPrio.Checked = false;
            LoPrio.Checked = false;
            Functions.SetDriverPriority(4);
        }

        private void LNPrio_Click(object sender, EventArgs e)
        {
            Functions.ButtonStatus(true);
            DePrio.Checked = false;
            RTPrio.Checked = false;
            HiPrio.Checked = false;
            HNPrio.Checked = false;
            NoPrio.Checked = false;
            LNPrio.Checked = true;
            LoPrio.Checked = false;
            Functions.SetDriverPriority(5);
        }

        private void LoPrio_Click(object sender, EventArgs e)
        {
            Functions.ButtonStatus(true);
            DePrio.Checked = false;
            RTPrio.Checked = false;
            HiPrio.Checked = false;
            HNPrio.Checked = false;
            NoPrio.Checked = false;
            LNPrio.Checked = false;
            LoPrio.Checked = true;
            Functions.SetDriverPriority(6);
        }

        // Priority stuff

        private void CloseConfigurator(object sender, CancelEventArgs e)
        {
            try
            {
                Bass.BASS_Free();
                Application.Exit();
            }
            catch
            {
                Application.Exit();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bass.BASS_Free();
            Application.Exit();
        }

        // Guide part
        private void isThereAnyShortcutForToOpenTheConfiguratorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To open the configurator while playing a MIDI, press ALT+9.\nYou could also press ALT+0 to directly open the \"Settings\" tab.",
                "What are the hotkeys to open the configurator?", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        private void whatAreTheHotkeysToChangeTheVolumeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To change the volume, simply press \"Add\" or \"Subtract\" buttons of the numeric keypad.\n\nYou can disable the hotkeys through \"Advanced settings > Volume hotkeys\".",
                "What are the hotkeys to change the volume?", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        private void howCanIChangeTheSoundfontListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To change the current soundfont list, press and hold ALT, then click a number from 1 to 8.\n\n" +
                "ALT+1: Load soundfont list 1\nALT+2: Load soundfont list 2\nALT+3: Load soundfont list 3\nALT+4: Load soundfont list 4\nALT+5: Load soundfont list 5\nALT+6: Load soundfont list 6\nALT+7: Load soundfont list 7\nALT+8: Load soundfont list 8\nCTRL+ALT+1: Load soundfont list 9\nCTRL+ALT+2: Load soundfont list 10\nCTRL+ALT+3: Load soundfont list 11\nCTRL+ALT+4: Load soundfont list 12\nCTRL+ALT+5: Load soundfont list 13\nCTRL+ALT+6: Load soundfont list 14\nCTRL+ALT+7: Load soundfont list 15\nCTRL+ALT+8: Load soundfont list 16\n\n" +
                "You can also reload lists that are already loaded in memory.", "How can I change the soundfont list?", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        private void howCanIResetTheDriverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To reset the driver, press INS.\nThis will stop all the samples that are currently playing, and it'll also send a \"System Reset\" to all the MIDI channels.", "How can I reset the driver?", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        private void whatsTheBestSettingsForTheBufferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("For SoundBlaster-based audio cards, it's 10.\nFor Realtek audio cards, it's 15-20.\nFor VIA audio cards, it's 20.\nFor Conexant audio cards, it's 30.\nFor USB DACs, it's 25-35.\nFor all the AC'97 audio cards, it's 35.\n\nIt's possible to set it to 10 with really fast computers.", "What's the best settings for the buffer?", MessageBoxButtons.OK, MessageBoxIcon.Question);
        }

        private void WhatsAutoPanic_Click(object sender, EventArgs e)
        {
            MessageBox.Show("\"Automatic MIDI panic\" will tell the driver to kill all the active notes, when the CPU usage is equal or higher than 100%.", "What's the automatic MIDI panic?", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void StatusBuf_Click(object sender, EventArgs e)
        {
            Int32[] valuearray = new Int32[10];
            Functions.ChangeRecommendedBuffer(CurrentIndexFreq, out valuearray);
            String message = String.Format("You should.\n\nKeep the buffer size between {0} and {1}, which are the optimal values for performance and audio quality, when outputting the audio at a frequency of {2}Hz.\n\nIncreasing the buffer too much could lead to unexpected crashes during playback, while decreasing it could make the app not output any audio to the speakers.", valuearray[4], valuearray[5], Frequency.Text);
            MessageBox.Show(message, "Do I need to be careful with the buffer size setting?", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SignatureCheck_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                Functions.GetSHA256OfDLLs(true);
            }
            else
            {
                Functions.GetSHA256OfDLLs(false);
            }
        }

        // Brand new output mode
        private void WhatIsOutput_Click(object sender, EventArgs e)
        {
            MessageBox.Show("If you check this option, the driver will create a WAV file on your desktop, called \"(programname).exe - Keppy's Synthesizer Output File.wav\".\n\n" +
                "You can change the output directory by clicking \"Settings > Change OTW directory\".\n\n" +
                "(The audio output to the speakers/headphones will be disabled, to avoid corruptions in the audio export.)", 
                "\"OTW mode\"? What is it?", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);                         
        }

        // Brand new XAudio disabler
        private void WhatIsXAudio_Click(object sender, EventArgs e)
        {
            MessageBox.Show("\"Engines\" are used by the driver to interface with your computer's sound card, and output the audio stream to your speakers/headphones.\n\nXAudio is the fastest of them all, but DirectSound can be used too, if your computer isn't able to achieve low latency, or it's too old for this new engine.\n\nIf you're planning to do heavy professional audio editing, you should pick ASIO, which achieves really low latency, at a cost of a bit more CPU usage.\n\nThere's also WASAPI now, which is able to achieve REALLY low latencies with little CPU usage, but \"Exclusive mode\" is needed to get latencies close to 1ms, which will disallow other apps from outputting audio.", 
                "What does engines do?", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Frequency_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentIndexFreq = Frequency.SelectedIndex;
            CheckBuffer();
        }

        private void bufsize_ValueChanged(object sender, EventArgs e)
        {
            CheckBuffer();
        }

        private void CheckBuffer()
        {
            if (KeppySynthConfiguratorMain.Delegate.AudioEngBox.Text == "XAudio" 
                || KeppySynthConfiguratorMain.Delegate.AudioEngBox.Text == "ASIO"
                || KeppySynthConfiguratorMain.Delegate.AudioEngBox.Text == "WASAPI")
            {
                Int32[] valuearray = new Int32[10];
                Functions.ChangeRecommendedBuffer(CurrentIndexFreq, out valuearray);

                if (bufsize.Value >= valuearray[0] && bufsize.Value <= valuearray[1])
                    StatusBuf.Image = KeppySynthConfigurator.Properties.Resources.wir;
                else if (bufsize.Value >= valuearray[2] && bufsize.Value <= valuearray[3])
                    StatusBuf.Image = KeppySynthConfigurator.Properties.Resources.wi;
                else if (bufsize.Value >= valuearray[4] && bufsize.Value <= valuearray[5])
                    StatusBuf.Image = KeppySynthConfigurator.Properties.Resources.ok;
                else if (bufsize.Value >= valuearray[6] && bufsize.Value <= valuearray[7])
                    StatusBuf.Image = KeppySynthConfigurator.Properties.Resources.wi;
                else if (bufsize.Value >= valuearray[8] && bufsize.Value <= valuearray[9])
                    StatusBuf.Image = KeppySynthConfigurator.Properties.Resources.wir;

                RecommendedBuffer.SetToolTip(
                    StatusBuf, 
                    String.Format("It is recommended to set a buffer size with {0}Hz audio is between {1} and {2}.",
                    Frequency.Text, valuearray[4], valuearray[5]));

                BufferText.Text = String.Format( "Set a buffer length for the driver, from 1 to 100 (Optimal range is between {0} and {1}):", 
                    valuearray[4], valuearray[5]);
            }
        }

        private void AudioEngBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (KeppySynthConfiguratorMain.Delegate.AudioEngBox.Text == "DirectSound")
            {
                menuItem32.Enabled = true;
                StatusBuf.Visible = false;
                OutputWAV.Enabled = false;
                OutputWAV.Checked = false;
                Label4.Enabled = false;
                SPFRate.Enabled = false;
                ManualAddBuffer.Visible = true;
                ChangeDefaultOutput.Enabled = true;
                changeDirectoryOfTheOutputToWAVModeToolStripMenuItem.Enabled = false;
                SleepStateRCO.Enabled = true;
                if (VolTrackBar.Value > 10000)
                {
                    VolTrackBar.Value = 10000;
                }
                VolTrackBar.Maximum = 10000;
                BufferText.Text = "Set a additional buffer length for the driver, from 0 to 1000:";
                bufsize.Minimum = 0;
                bufsize.Maximum = 1000;
                bufsize.Enabled = false;
                if (ManualAddBuffer.Checked == true)
                {
                    bufsize.Enabled = true;
                }
                else
                {
                    bufsize.Enabled = false;
                    bufsize.Value = 0;
                }
            }
            else
            {
                if (KeppySynthConfiguratorMain.Delegate.AudioEngBox.Text == "XAudio")
                {
                    menuItem32.Enabled = true;
                    ChangeDefaultOutput.Enabled = false;
                    Label4.Enabled = true;
                    SPFRate.Enabled = true;
                }
                else
                {
                    menuItem32.Enabled = false;
                    ChangeDefaultOutput.Enabled = true;
                    Label4.Enabled = false;
                    SPFRate.Enabled = false;
                }
                StatusBuf.Visible = true;
                OutputWAV.Enabled = true;
                ManualAddBuffer.Visible = false;
                changeDirectoryOfTheOutputToWAVModeToolStripMenuItem.Enabled = true;
                SleepStateRCO.Enabled = false;
                BufferText.Text = "Set a buffer length for the driver, from 1 to 100:";
                bufsize.Minimum = 1;
                bufsize.Maximum = 100;
                bufsize.Enabled = true;
                bufsize.Value = 15;
                CheckBuffer();
            }
        }

        private void OutputWAV_CheckedChanged(object sender, EventArgs e)
        {
            if (OutputWAV.Checked == true)
            {
                StatusBuf.Visible = false;
                KeppySynthConfiguratorMain.Delegate.AudioEngBox.Enabled = false;
                KeppySynthConfiguratorMain.Delegate.AudioEngBox.Text = "XAudio";
                Label5.Enabled = false;
                bufsize.Enabled = false;
                MaxCPU.Enabled = false;
                BufferText.Enabled = false;
                bufsize.Enabled = false;
                bufsize.Minimum = 0;
                bufsize.Value = 0;
                MaxCPU.Value = 0;
            }
            else if (OutputWAV.Checked == false)
            {
                StatusBuf.Visible = true;
                KeppySynthConfiguratorMain.Delegate.AudioEngBox.Enabled = true;
                Label5.Enabled = true;
                bufsize.Enabled = true;
                MaxCPU.Enabled = true;
                BufferText.Enabled = true;
                bufsize.Enabled = true;
                bufsize.Minimum = 1;
                bufsize.Value = 15;
                MaxCPU.Value = 75;
            }
        }

        private void VMSEmu_CheckedChanged(object sender, EventArgs e)
        {
            if (ManualAddBuffer.Checked == true)
            {
                bufsize.Enabled = true;
            }
            else
            {
                bufsize.Enabled = false;
            }
        }

        private void MIDINameNoSpace_Click(object sender, EventArgs e)
        {
            if (MIDINameNoSpace.Checked == false)
            {
                SynthSettings.SetValue("shortname", "1", RegistryValueKind.DWord);
                SynthSettings.SetValue("newdevicename", "0", RegistryValueKind.DWord);
                MaskSynthesizerAsAnother.Enabled = false;
                MIDINameNoSpace.Checked = true;
            }
            else
            {
                SynthSettings.SetValue("shortname", "0", RegistryValueKind.DWord);
                MaskSynthesizerAsAnother.Enabled = true;
                MIDINameNoSpace.Checked = false;
            }
        }

        private void useoldbuffersystem_Click(object sender, EventArgs e)
        {
            if (useoldbuffersystem.Checked == false)
            {
                SynthSettings.SetValue("oldbuffersystem", "1", RegistryValueKind.DWord);
                useoldbuffersystem.Checked = true;
            }
            else
            {
                SynthSettings.SetValue("oldbuffersystem", "0", RegistryValueKind.DWord);
                useoldbuffersystem.Checked = false;
            }
        }

        private void slowdownnoskip_Click(object sender, EventArgs e)
        {
            if (slowdownnoskip.Checked == false)
            {
                SynthSettings.SetValue("vms2emu", "1", RegistryValueKind.DWord);
                slowdownnoskip.Checked = true;
            }
            else
            {
                SynthSettings.SetValue("vms2emu", "0", RegistryValueKind.DWord);
                slowdownnoskip.Checked = false;
            }
        }

        private void autopanicmode_Click(object sender, EventArgs e)
        {
            if (autopanicmode.Checked == false)
            {
                SynthSettings.SetValue("alternativecpu", "1", RegistryValueKind.DWord);
                autopanicmode.Checked = true;
            }
            else
            {
                SynthSettings.SetValue("alternativecpu", "0", RegistryValueKind.DWord);
                autopanicmode.Checked = false;
            }
        }

        private void hotkeys_Click(object sender, EventArgs e)
        {
            if (hotkeys.Checked == false)
            {
                SynthSettings.SetValue("allhotkeys", "1", RegistryValueKind.DWord);
                hotkeys.Checked = true;
            }
            else
            {
                SynthSettings.SetValue("allhotkeys", "0", RegistryValueKind.DWord);
                hotkeys.Checked = false;
            }
        }

        private void autoupdate_Click(object sender, EventArgs e)
        {
            if (autoupdate.Checked == false)
            {
                SynthSettings.SetValue("autoupdatecheck", "1", RegistryValueKind.DWord);
                autoupdate.Checked = true;
            }
            else
            {
                SynthSettings.SetValue("autoupdatecheck", "0", RegistryValueKind.DWord);
                autoupdate.Checked = false;
            }
        }

        private void SleepStateRCO_Click(object sender, EventArgs e)
        {
            if (SleepStateRCO.Checked == true)
            {
                SynthSettings.SetValue("rco", "1", RegistryValueKind.DWord);
                SleepStateRCO.Checked = false;
            }
            else
            {
                SynthSettings.SetValue("rco", "0", RegistryValueKind.DWord);
                SleepStateRCO.Checked = true;
            }
        }

        private void FadeoutDisable_Click(object sender, EventArgs e)
        {
            if (FadeoutDisable.Checked == false)
            {
                SynthSettings.SetValue("fadeoutdisable", "1", RegistryValueKind.DWord);
                FadeoutDisable.Checked = true;
            }
            else
            {
                SynthSettings.SetValue("fadeoutdisable", "0", RegistryValueKind.DWord);
                FadeoutDisable.Checked = false;
            }
        }

        private void enableextra8sf_Click(object sender, EventArgs e)
        {
            if (enableextra8sf.Checked == false)
            {
                SynthSettings.SetValue("extra8lists", "1", RegistryValueKind.DWord);
                enableextra8sf.Checked = true;
                SelectedListBox.Items.Add("List 9");
                SelectedListBox.Items.Add("List 10");
                SelectedListBox.Items.Add("List 11");
                SelectedListBox.Items.Add("List 12");
                SelectedListBox.Items.Add("List 13");
                SelectedListBox.Items.Add("List 14");
                SelectedListBox.Items.Add("List 15");
                SelectedListBox.Items.Add("List 16");
            }
            else
            {
                SynthSettings.SetValue("extra8lists", "0", RegistryValueKind.DWord);
                enableextra8sf.Checked = false;
                SelectedListBox.Items.RemoveAt(8);
                SelectedListBox.Items.RemoveAt(8);
                SelectedListBox.Items.RemoveAt(8);
                SelectedListBox.Items.RemoveAt(8);
                SelectedListBox.Items.RemoveAt(8);
                SelectedListBox.Items.RemoveAt(8);
                SelectedListBox.Items.RemoveAt(8);
                SelectedListBox.Items.RemoveAt(8);
            }
        }

        private void floatingpointaudio_Click(object sender, EventArgs e)
        {
            if (floatingpointaudio.Checked == false)
            {
                SynthSettings.SetValue("32bit", "1", RegistryValueKind.DWord);
                floatingpointaudio.Checked = true;
                bit16audio.Checked = false;
                bit8audio.Checked = false;
            }
        }

        private void bit16audio_Click(object sender, EventArgs e)
        {
            if (bit16audio.Checked == false)
            {
                SynthSettings.SetValue("32bit", "2", RegistryValueKind.DWord);
                floatingpointaudio.Checked = false;
                bit16audio.Checked = true;
                bit8audio.Checked = false;
            }
        }

        private void bit8audio_Click(object sender, EventArgs e)
        {
            if (bit8audio.Checked == false)
            {
                SynthSettings.SetValue("32bit", "3", RegistryValueKind.DWord);
                floatingpointaudio.Checked = false;
                bit16audio.Checked = false;
                bit8audio.Checked = true;
            }
        }

        private void MonophonicFunc_Click(object sender, EventArgs e)
        {
            if (MonophonicFunc.Checked == false)
            {
                SynthSettings.SetValue("monorendering", "1", RegistryValueKind.DWord);
                MonophonicFunc.Checked = true;
            }
            else
            {
                SynthSettings.SetValue("monorendering", "0", RegistryValueKind.DWord);
                MonophonicFunc.Checked = false;
            }
        }

        private void SysExIgnore_Click(object sender, EventArgs e)
        {
            if (SysExIgnore.Checked == false)
            {
                SynthSettings.SetValue("sysexignore", "1", RegistryValueKind.DWord);
                SysExIgnore.Checked = true;
            }
            else
            {
                SynthSettings.SetValue("sysexignore", "0", RegistryValueKind.DWord);
                SysExIgnore.Checked = false;
            }
        }

        private void AllNotesIgnore_Click(object sender, EventArgs e)
        {
            if (AllNotesIgnore.Checked == false)
            {
                SynthSettings.SetValue("allnotesignore", "1", RegistryValueKind.DWord);
                AllNotesIgnore.Checked = true;
                SysExIgnore.Checked = true;
            }
            else
            {
                SynthSettings.SetValue("allnotesignore", "0", RegistryValueKind.DWord);
                AllNotesIgnore.Checked = false;
            }
        }

        private void FullVelocityMode_Click(object sender, EventArgs e)
        {
            if (FullVelocityMode.Checked == false)
            {
                SynthSettings.SetValue("fullvelocity", "1", RegistryValueKind.DWord);
                FullVelocityMode.Checked = true;
            }
            else
            {
                SynthSettings.SetValue("fullvelocity", "0", RegistryValueKind.DWord);
                FullVelocityMode.Checked = false;
            }
        }

        private void NoteOFFtoON_Click(object sender, EventArgs e)
        {
            if (NoteOFFtoON.Checked == false)
            {
                SynthSettings.SetValue("turnnoteoffintonoteon", "1", RegistryValueKind.DWord);
                NoteOFFtoON.Checked = true;
            }
            else
            {
                SynthSettings.SetValue("turnnoteoffintonoteon", "0", RegistryValueKind.DWord);
                NoteOFFtoON.Checked = false;
            }
        }

        private void RevbNChor_Click(object sender, EventArgs e)
        {
            RevbNChorForm frm = new RevbNChorForm();
            frm.ShowDialog(this);
            frm.Dispose();
        }

        private void DebugModePls_Click(object sender, EventArgs e)
        {
            if (DebugModePls.Checked == false)
            {
                SynthSettings.SetValue("debugmode", "1", RegistryValueKind.DWord);
                DebugModePls.Checked = true;
            }
            else
            {
                SynthSettings.SetValue("debugmode", "0", RegistryValueKind.DWord);
                DebugModePls.Checked = false;
            }
        }

        private void DebugModeOpenNotepad_Click(object sender, EventArgs e)
        {
            String DirectoryDebug = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Keppy's Synthesizer\\debug\\";
            try
            {
                Process.Start(DirectoryDebug);
            }
            catch
            {
                Directory.CreateDirectory(DirectoryDebug);
                Process.Start(DirectoryDebug);
            }
        }

        private void IgnoreNotes1_Click(object sender, EventArgs e)
        {
            if (IgnoreNotes1.Checked == false)
            {
                SynthSettings.SetValue("ignorenotes1", "1", RegistryValueKind.DWord);
                IgnoreNotes1.Checked = true;
                IgnoreNotesInterval.Enabled = true;
            }
            else
            {
                SynthSettings.SetValue("ignorenotes1", "0", RegistryValueKind.DWord);
                IgnoreNotes1.Checked = false;
                IgnoreNotesInterval.Enabled = false;
            }
        }

        private void ChangeDefaultOutput_Click(object sender, EventArgs e)
        {
            if (KeppySynthConfiguratorMain.Delegate.AudioEngBox.Text == "ASIO")
            {
                DefaultASIOAudioOutput frm = new DefaultASIOAudioOutput();
                frm.ShowDialog(this);
                frm.Dispose();
            }
            else if (KeppySynthConfiguratorMain.Delegate.AudioEngBox.Text == "WASAPI")
            {
                DefaultWASAPIAudioOutput frm = new DefaultWASAPIAudioOutput();
                frm.ShowDialog(this);
                frm.Dispose();
            }
            else
            {
                KeppySynthDefaultOutput frm = new KeppySynthDefaultOutput();
                frm.ShowDialog(this);
                frm.Dispose();
            }
        }

        private void IgnoreNotesInterval_Click(object sender, EventArgs e)
        {
            KeppySynthVelocityIntervals frm = new KeppySynthVelocityIntervals();
            frm.ShowDialog(this);
            frm.Dispose();
        }

        private void ChangePitchShift_Click(object sender, EventArgs e)
        {
            KeppySynthPitchShifting frm = new KeppySynthPitchShifting();
            frm.ShowDialog(this);
            frm.Dispose();
        }

        private void MaskSynthesizerAsAnother_Click(object sender, EventArgs e)
        {
            MaskSynthAsAnother frm = new MaskSynthAsAnother();
            frm.ShowDialog(this);
            frm.Dispose();
        }

        private void SettingsPresetsBtn_Click(object sender, EventArgs e)
        {
            SettingsPresets.Show(SettingsPresetsBtn, new System.Drawing.Point(1, 20));
        }

        private void SetSynthDefault_Click(object sender, EventArgs e)
        {
            if (!SetSynthDefault.Checked)
            {
                SynthSettings.SetValue("defaultmidiout", "1", RegistryValueKind.DWord);
                SetSynthDefault.Checked = true;
            }
            else
            {
                SynthSettings.SetValue("defaultmidiout", "0", RegistryValueKind.DWord);
                SetSynthDefault.Checked = false;
            }
        }

        private void AMIDIMapCpl_Click(object sender, EventArgs e)
        {
            try
            {
                var process = System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\keppysynth\\amidimap.cpl");
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Functions.ShowErrorDialog(1, System.Media.SystemSounds.Asterisk, "Error", "Can not open the Alternative MIDI Mapper applet!", true, ex);
            }
        }

        // Snap feature

        private const int SnapDist = 25;

        private bool DoSnap(int pos, int edge)
        {
            int delta = pos - edge;
            return delta > 0 && delta <= SnapDist;
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);
            Screen scn = Screen.FromPoint(this.Location);
            if (DoSnap(this.Left, scn.WorkingArea.Left)) this.Left = scn.WorkingArea.Left;
            if (DoSnap(this.Top, scn.WorkingArea.Top)) this.Top = scn.WorkingArea.Top;
            if (DoSnap(scn.WorkingArea.Right, this.Right)) this.Left = scn.WorkingArea.Right - this.Width;
            if (DoSnap(scn.WorkingArea.Bottom, this.Bottom)) this.Top = scn.WorkingArea.Bottom - this.Height;
        }

        private void ThemeCheck_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                while (true)
                {
                    if (VisualStyleInformation.IsEnabledByUser == true)
                    {
                        if (CurrentTheme == 0)
                        {
                            CurrentTheme = 1;
                            this.Invoke(new MethodInvoker(delegate { SoundFontTab.BackColor = Color.White; }));
                            this.Invoke(new MethodInvoker(delegate { Settings.BackColor = Color.White; }));
                            this.Invoke(new MethodInvoker(delegate { this.Refresh(); }));
                        }
                    }
                    else
                    {
                        if (CurrentTheme == 1)
                        {
                            CurrentTheme = 0;
                            this.Invoke(new MethodInvoker(delegate { SoundFontTab.BackColor = SystemColors.Control; }));
                            this.Invoke(new MethodInvoker(delegate { Settings.BackColor = SystemColors.Control; }));
                            this.Invoke(new MethodInvoker(delegate { this.Refresh(); }));
                        }
                    }
                    System.Threading.Thread.Sleep(100);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
            catch
            {

            }
        }

        // Troubleshooter
        private void PanicButton_Click(object sender, EventArgs e)
        {
            TabsForTheControls.SelectedIndex = 1;

            String title = "Keppy's Synthesizer - Troubleshooting";

            String Success = "Super-duper!\n\nHave fun with the driver!";
            String isitworking = "Now test the driver with a MIDI application.\n\nIs it working now?";
            String isitworking2 = "Try again now.\n\nTest the driver with a MIDI application.\n\nIs it working now?";     
            String weak = "Maybe your PC is too weak.\n\nReport your computer specifications to KaleidonKep99, on GitHub, by filling an issue.";
            String panic1 = "Don't panic.\nKeppy's Synthesizer is a pretty sensitive software, and heavy changes to the settings could make it unusable.\n\n" +
                "Before you think about uninstalling it and moving to another synth, let's try analizying the issue.\n\n" +
                "We'll first try resetting the normal settings to default. Press OK to reset the settings.";
            String panic2 = "We'll now try resetting the advanced settings.\n\n" +
                "Changing the advanced settings without knowing what they do could cause Keppy's Synthesizer to behave abnormally.\n\n" +
                "We'll now try resetting the advanced settings to default. Press OK to reset the settings.";
            String panic3 = "We'll now try reducing the workload on your computer by adjusting the settings.\n\n" +
                "The configurator will reduce the maximum voices, increase the buffer etc.\n\n" +
                "Press OK to start.";
            String panic4 = "Since none of this worked, we'll now try reinstalling the driver.\n\n" +
                "The configurator will now delete all the driver's registry keys, and run the installer.\n" +
                "If, after the reinstall, the driver continues to have issues running on your computer, please report the problem to KaleidonKep99, on GitHub.\n\n" +
                "Press OK to continue.";

            // Troubleshoot part 1
            MessageBox.Show(panic1, title, MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Set some values...
            VolTrackBar.Value = 10000;
            PolyphonyLimit.Value = 512;
            MaxCPU.Value = 65;
            Frequency.Text = "48000";
            bufsize.Value = 30;
            SPFRate.Value = 100;
            Preload.Checked = true;
            NoteOffCheck.Checked = false;
            SincInter.Checked = false;
            EnableSFX.Checked = false;
            SysResetIgnore.Checked = false;
            OutputWAV.Checked = false;
            KeppySynthConfiguratorMain.Delegate.AudioEngBox.Text = "XAudio";
            ManualAddBuffer.Checked = false;

            // And then...
            Functions.SaveSettings();

            DialogResult dialogResult = MessageBox.Show(isitworking, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                MessageBox.Show(Success, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                return;
            }

            // Troubleshoot part 2
            MessageBox.Show(panic2, title, MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Set some values...
            SynthSettings.SetValue("sndbfvalue", 100, RegistryValueKind.DWord);
            SynthSettings.SetValue("newevbuffvalue", 16384, RegistryValueKind.DWord);

            DialogResult dialogResult2 = MessageBox.Show(isitworking, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult2 == DialogResult.Yes)
            {
                MessageBox.Show(Success, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                return;
            }

            // Troubleshoot part 3
            MessageBox.Show(panic3, title, MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Set some values...
            PolyphonyLimit.Value = 350;
            MaxCPU.Value = 75;
            Frequency.Text = "44100";
            bufsize.Value = 40;

            // And then...
            Functions.SaveSettings();

            DialogResult dialogResult3 = MessageBox.Show(isitworking, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult3 == DialogResult.Yes)
            {
                MessageBox.Show(weak, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Set some values...
            PolyphonyLimit.Value = 250;
            MaxCPU.Value = 80;
            Frequency.Text = "32000";
            bufsize.Value = 50;

            // And then...
            Functions.SaveSettings();

            DialogResult dialogResult4 = MessageBox.Show(isitworking2, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult4 == DialogResult.Yes)
            {
                MessageBox.Show(weak, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                MessageBox.Show(panic4, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                var p = new System.Diagnostics.Process();
                p.StartInfo.FileName = Application.ExecutablePath;
                p.StartInfo.Arguments = "/REI";
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                Application.ExitThread();
                return;
            }
        }

        // Links

        private void SoftpediaPage_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.softpedia.com/get/Multimedia/Audio/Audio-Mixers-Synthesizers/Keppys-Synthesizer.shtml");
        }

        private void KepChannel_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.youtube.com/channel/UCJeqODojIv4TdeHcBfHJRnA");
        }

        // Tools

        private bool Resizing = false;
        private void Lis_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                if (!Resizing)
                {
                    Resizing = true;
                    ListView listView = sender as ListView;
                    if (listView != null)
                    {
                        float totalColumnWidth = 0;

                        for (int i = 0; i < listView.Columns.Count; i++)
                            totalColumnWidth += Convert.ToInt32(listView.Columns[i].Tag);

                        for (int i = 0; i < listView.Columns.Count; i++)
                        {
                            float colPercentage;
                            colPercentage = (Convert.ToInt32(listView.Columns[i].Tag) / totalColumnWidth);
                            if (i == 0)
                                listView.Columns[i].Width = ((int)(colPercentage * listView.ClientRectangle.Width)) - 10;
                            else
                                listView.Columns[i].Width = ((int)(colPercentage * listView.ClientRectangle.Width));
                        }
                    }
                }

            }
            catch { }
            finally { Resizing = false; }
        }

        private void CheckIfUserPressesEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Functions.SaveSettings();
                EnableFade();
            }
        }

        
        private void EnableFade()
        {
            SystemSounds.Question.Play();
            SavedLabel.Enabled = false;
            greenfade = 128;
            SavedLabel.Enabled = true;
        }

        private void SavedLabel_Tick(object sender, EventArgs e)
        {
            applySettingsToolStripMenuItem.Text = "Saved!";
            applySettingsToolStripMenuItem.Font = new Font(applySettingsToolStripMenuItem.Font, FontStyle.Bold);
            applySettingsToolStripMenuItem.ForeColor = Color.FromArgb(0, greenfade, 0);
            greenfade--;
            if (greenfade <= 1)
            {
                SavedLabel.Enabled = false;
                applySettingsToolStripMenuItem.Text = "Apply settings";
                applySettingsToolStripMenuItem.Font = new Font(applySettingsToolStripMenuItem.Font, FontStyle.Regular);
                applySettingsToolStripMenuItem.ForeColor = SystemColors.ControlText;
            }
            System.Threading.Thread.Sleep(1);
        }

        private void WinMMPatch32_Click(object sender, EventArgs e)
        {
            Functions.ApplyWinMMPatch(false);
        }

        private void WinMMPatch64_Click(object sender, EventArgs e)
        {
            Functions.ApplyWinMMPatch(true);
        }

        private void WinMMPatchRmv_Click(object sender, EventArgs e)
        {
            Functions.RemoveWinMMPatch();
        }

        private void ResetToDefault_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to reinstall the driver?\n\nThe configurator will download the latest installer, and remove all the old registry keys.\nYou'll lose ALL the settings.", "Keppy's Synthesizer - Reinstall the driver from scratch", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                var p = new System.Diagnostics.Process();
                p.StartInfo.FileName = Application.ExecutablePath;
                p.StartInfo.Arguments = "/REI";
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                Application.ExitThread();
            }
        }

        private void ChangeUpdateBranch_Click(object sender, EventArgs e)
        {
            SelectBranch frm = new SelectBranch();
            frm.ShowDialog(this);
            frm.Dispose();
        }

        // Credits

        private void HAPLink_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.nuget.org/packages/HtmlAgilityPack/");
        }

        private void BASSLink_Click(object sender, EventArgs e)
        {
            Process.Start("http://bass.radio42.com/");
        }

        private void BASSNetLink_Click(object sender, EventArgs e)
        {
            Process.Start("http://bass.radio42.com/");
        }

        private void FodyCredit_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/Fody");
        }

        private void menuItem46_Click(object sender, EventArgs e)
        {
            try
            {
                BASS_DEVICEINFO info = new BASS_DEVICEINFO();
                String DeviceID = "0";
                Bass.BASS_GetDeviceInfo(0, info);
                for (int n = 0; Bass.BASS_GetDeviceInfo(n, info); n++)
                {
                    if (info.IsDefault == true)
                    {
                        DeviceID = info.driver;
                        break;
                    }
                }
                Process.Start(
                    @"C:\Windows\System32\rundll32.exe",
                    String.Format(@"C:\Windows\System32\shell32.dll,Control_RunDLL C:\Windows\System32\mmsys.cpl ms-mmsys:,{0},spatial", DeviceID));
            }
            catch
            {
                MessageBox.Show("This function requires Windows 10 Creators Update or newer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EnableBB_Click(object sender, EventArgs e)
        {
            EnableBB.Visible = false;
            EnableBBS.Visible = false;
            Properties.Settings.Default.ButterBoy = true;
            Properties.Settings.Default.Save();
            MessageBox.Show("Super-duper!\n\nI'm happy that you changed your mind!", "Butter Boy", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        int paintReps = 0;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            System.Threading.Thread.Sleep(1);

            if (paintReps++ % 500 == 0)
                Application.DoEvents();
        }

    }
}