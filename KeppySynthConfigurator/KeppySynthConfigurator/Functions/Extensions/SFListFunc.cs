using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Midi;

namespace KeppySynthConfigurator
{
    class SFListFunc
    {
        public static Color SFEnabled = Color.FromArgb(0, 0, 0);
        public static Color SFDisabled = Color.FromArgb(170, 170, 170);

        public static string ReturnSoundFontSize(string preset, string ext, long length)
        {
            if (ext.ToLowerInvariant() != ".sfz")
            {
                string size = Functions.ReturnLength(length);
                return size;
            }
            else
            {
                long size = SFZInfo.GetSoundFontZSize(preset);
                if (size > 0) return Functions.ReturnLength(size);
                else return "N/A";
            }
        }

        public static string ReturnSoundFontFormat(string fileext)
        {
            if (fileext.ToLowerInvariant() == ".sf1")
                return "SF1";
            else if (fileext.ToLowerInvariant() == ".sf2")
                return "SF2";
            else if (fileext.ToLowerInvariant() == ".sfz")
                return "SFZ";
            else if (fileext.ToLowerInvariant() == ".ssx")
                return "Enc. SF";
            else if (fileext.ToLowerInvariant() == ".sfpack")
                return "SF Pack";
            else if (fileext.ToLowerInvariant() == ".sfark")
                return "SF Arch.";
            else
                return "N/A";
        }

        public static string ReturnSoundFontFormatMore(string fileext)
        {
            if (fileext.ToLowerInvariant() == ".sf1")
                return "SoundFont 1.x by E-mu Systems";
            else if (fileext.ToLowerInvariant() == ".sf2")
                return "SoundFont 2.x by Creative Labs";
            else if (fileext.ToLowerInvariant() == ".sfz")
                return "SoundFontZ by Cakewalk™";
            else if (fileext.ToLowerInvariant() == ".ssx")
                return "Princess Soft Encrypted SoundFont";
            else if (fileext.ToLowerInvariant() == ".sfpack")
                return "SoundFont compressed package";
            else if (fileext.ToLowerInvariant() == ".sfark")
                return "SoundFont Archive (SfARK)";
            else
                return "Unknown format";
        }

        private static Color ReturnColor(String result)
        {
            if (result == "@")
                return SFDisabled;
            else
                return SFEnabled;
        }

        public static void ChangeList(int SelectedList) // When you select a list from the combobox, it'll load the items from the selected list to the listbox
        {
            KeppySynthConfiguratorMain.CurrentList = KeppySynthConfiguratorMain.ListsPath[SelectedList];
            KeppySynthConfiguratorMain.whichone = SelectedList + 1;
            String WhichList = KeppySynthConfiguratorMain.CurrentList;

            try
            {
                if (!System.IO.Directory.Exists(KeppySynthConfiguratorMain.AbsolutePath))
                {
                    Directory.CreateDirectory(KeppySynthConfiguratorMain.AbsolutePath);
                    Directory.CreateDirectory(KeppySynthConfiguratorMain.PathToAllLists);
                    File.Create(WhichList).Dispose();
                    KeppySynthConfiguratorMain.Delegate.Lis.Items.Clear();
                }
                if (!System.IO.Directory.Exists(KeppySynthConfiguratorMain.PathToAllLists))
                {
                    Directory.CreateDirectory(KeppySynthConfiguratorMain.PathToAllLists);
                    File.Create(WhichList).Dispose();
                    KeppySynthConfiguratorMain.Delegate.Lis.Items.Clear();
                }
                if (!System.IO.File.Exists(WhichList))
                {
                    // Oops, list is missing
                    File.Create(WhichList).Dispose();
                    KeppySynthConfiguratorMain.Delegate.Lis.Items.Clear();
                    Functions.ShowErrorDialog(0, System.Media.SystemSounds.Question, "Information", "The soundfont list was missing, so the configurator automatically created it for you.", false, null);
                }
                try
                {
                    using (StreamReader r = new StreamReader(WhichList))
                    {
                        string line;
                        KeppySynthConfiguratorMain.Delegate.Lis.Items.Clear();
                        KeppySynthConfiguratorMain.Delegate.Lis.Refresh();
                        while ((line = r.ReadLine()) != null)
                        {
                            try
                            {
                                string result = line.Substring(0, 1);
                                string newvalue;

                                if (result == "@")
                                    line = line.Remove(0, 1);

                                FileInfo file = new FileInfo(StripSFZValues(line));

                                ListViewItem SF = new ListViewItem(new[] {
                                    line,
                                    ReturnSoundFontFormat(Path.GetExtension(StripSFZValues(line))),
                                    ReturnSoundFontSize(StripSFZValues(line), Path.GetExtension(StripSFZValues(line)), file.Length)
                                });

                                SF.ForeColor = ReturnColor(result);
                                KeppySynthConfiguratorMain.Delegate.Lis.Items.Add(SF);
                            }
                            catch
                            {
                                ListViewItem SF = new ListViewItem(new[] {
                                    line,
                                    "Missing",
                                    "N/A"
                                });

                                SF.ForeColor = Color.Red;
                                KeppySynthConfiguratorMain.Delegate.Lis.Items.Add(SF);

                                Program.DebugToConsole(false, String.Format("{0} is missing.", line), null);
                            }
                        }
                    }
                }
                catch
                {
                    // Oops, list is missing
                    File.Create(WhichList).Dispose();
                    KeppySynthConfiguratorMain.Delegate.Lis.Items.Clear();
                    Functions.ShowErrorDialog(0, System.Media.SystemSounds.Question, "Information", "The soundfont list was missing, so the configurator automatically created it for you.", false, null);
                }
                Program.DebugToConsole(false, String.Format("Switched to soundfont list {0}.", SelectedList + 1), null);
            }
            catch (Exception ex)
            {
                // Oops, something went wrong
                Program.DebugToConsole(true, null, ex);
                Functions.ShowErrorDialog(2, System.Media.SystemSounds.Hand, "Fatal error", "Fatal error during the execution of the program.\n\nPress OK to quit.", true, ex);
                Application.ExitThread();
            }
        }

        public static void AddSoundfontsToSelectedList(String CurrentList, String[] Soundfonts)
        {
            for (int i = 0; i < Soundfonts.Length; i++)
            {
                if (Path.GetExtension(Soundfonts[i]).ToLowerInvariant() == ".sf1" | Path.GetExtension(Soundfonts[i]).ToLowerInvariant() == ".sf2" | Path.GetExtension(Soundfonts[i]).ToLowerInvariant() == ".sfark" | Path.GetExtension(Soundfonts[i]) == ".sfpack".ToLowerInvariant())
                {
                    int test = BassMidi.BASS_MIDI_FontInit(Soundfonts[i], BASSFlag.BASS_DEFAULT);
                    if (Bass.BASS_ErrorGetCode() != 0)
                    {
                        Functions.ShowErrorDialog(1, System.Media.SystemSounds.Hand, "Error while adding soundfont", String.Format("{0} is not a valid soundfont file!", Path.GetFileName(Soundfonts[i])), false, null);
                    }
                    else
                    {
                        if (KeppySynthConfiguratorMain.Delegate.BankPresetOverride.Checked == true)
                        {
                            using (var form = new BankNPresetSel(Path.GetFileName(Soundfonts[i]), 0, 1))
                            {
                                var result = form.ShowDialog();
                                if (result == DialogResult.OK)
                                {
                                    string sbank = form.BankValueReturn;
                                    string spreset = form.PresetValueReturn;
                                    string dbank = form.DesBankValueReturn;
                                    string dpreset = form.DesPresetValueReturn;
                                    FileInfo file = new FileInfo(Soundfonts[i]);
                                    ListViewItem SF = new ListViewItem(new[] {
                                        "p" + sbank + "," + spreset + "=" + dbank + "," + dpreset + "|" + Soundfonts[i],
                                        ReturnSoundFontFormat(Path.GetExtension(Soundfonts[i])),
                                        ReturnSoundFontSize(Soundfonts[i], Path.GetExtension(Soundfonts[i]), file.Length)
                                    });
                                    KeppySynthConfiguratorMain.Delegate.Lis.Items.Add(SF);
                                }
                            }
                        }
                        else
                        {
                            FileInfo file = new FileInfo(Soundfonts[i]);
                            ListViewItem SF = new ListViewItem(new[] {
                                        Soundfonts[i],
                                        ReturnSoundFontFormat(Path.GetExtension(Soundfonts[i])),
                                        ReturnSoundFontSize(Soundfonts[i], Path.GetExtension(Soundfonts[i]), file.Length)
                                    });
                            KeppySynthConfiguratorMain.Delegate.Lis.Items.Add(SF);
                        }
                        SaveList(CurrentList);
                        TriggerReload(false);
                    }
                    Program.DebugToConsole(false, String.Format("Added soundfont to list: {0}", Soundfonts[i]), null);
                }
                else if (Path.GetExtension(Soundfonts[i]).ToLowerInvariant() == ".sfz")
                {
                    int test = BassMidi.BASS_MIDI_FontInit(Soundfonts[i], BASSFlag.BASS_DEFAULT);
                    if (Bass.BASS_ErrorGetCode() != 0)
                    {
                        Functions.ShowErrorDialog(1, System.Media.SystemSounds.Hand, "Error while adding soundfont", String.Format("{0} is not a valid soundfont file!", Path.GetFileName(Soundfonts[i])), false, null);
                    }
                    else
                    {
                        using (var form = new BankNPresetSel(Path.GetFileName(Soundfonts[i]), 1, 0))
                        {
                            var result = form.ShowDialog();
                            if (result == DialogResult.OK)
                            {
                                string sbank = form.BankValueReturn;
                                string spreset = form.PresetValueReturn;
                                string dbank = form.DesBankValueReturn;
                                string dpreset = form.DesPresetValueReturn;
                                FileInfo file = new FileInfo(Soundfonts[i]);
                                ListViewItem SF = new ListViewItem(new[] {
                                        "p" + sbank + "," + spreset + "=" + dbank + "," + dpreset + "|" + Soundfonts[i],
                                        ReturnSoundFontFormat(Path.GetExtension(Soundfonts[i])),
                                        ReturnSoundFontSize(Soundfonts[i], Path.GetExtension(Soundfonts[i]), file.Length)
                                    });
                                KeppySynthConfiguratorMain.Delegate.Lis.Items.Add(SF);
                            }
                        }
                        SaveList(CurrentList);
                        TriggerReload(false);
                    }
                    Program.DebugToConsole(false, String.Format("Added soundfont to list: {0}", Soundfonts[i]), null);
                }
                else if (Path.GetExtension(Soundfonts[i]).ToLowerInvariant() == ".dls")
                {
                    Functions.ShowErrorDialog(1, System.Media.SystemSounds.Exclamation, "Error", "BASSMIDI does NOT support the downloadable sounds (DLS) format!", false, null);
                }
                else if (Path.GetExtension(Soundfonts[i]).ToLowerInvariant() == ".exe" | Path.GetExtension(Soundfonts[i]).ToLowerInvariant() == ".dll")
                {
                    Functions.ShowErrorDialog(1, System.Media.SystemSounds.Exclamation, "Error", "Are you really trying to add executables to the soundfonts list?", false, null);
                }
                else
                {
                    Functions.ShowErrorDialog(1, System.Media.SystemSounds.Exclamation, "Error", "Invalid soundfont!\n\nPlease select a valid soundfont and try again!", false, null);
                }
            }
        }

        public static void ReinitializeList(Exception ex, String selectedlistpath) // The app encountered an error, so it'll restore the original soundfont list back
        {
            try
            {
                Program.DebugToConsole(true, null, ex);
                Functions.ShowErrorDialog(2, System.Media.SystemSounds.Exclamation, "Error", "Oh snap!\nThe configurator encountered an error while editing the following list:\n" + selectedlistpath, true, ex);
                KeppySynthConfiguratorMain.Delegate.Lis.Items.Clear();
                using (StreamReader r = new StreamReader(selectedlistpath))
                {
                    string line;
                    KeppySynthConfiguratorMain.Delegate.Lis.Items.Clear();
                    KeppySynthConfiguratorMain.Delegate.Lis.Refresh();
                    while ((line = r.ReadLine()) != null)
                    {
                        string result = line.Substring(0, 1);
                        string newvalue;

                        if (result == "@")
                            line = line.Remove(0, 1);

                        FileInfo file = new FileInfo(StripSFZValues(line));
                        ListViewItem SF = new ListViewItem(new[] {
                                line,
                                ReturnSoundFontFormat(Path.GetExtension(StripSFZValues(line))),
                                ReturnSoundFontSize(StripSFZValues(line), Path.GetExtension(StripSFZValues(line)), file.Length)
                            });

                        SF.ForeColor = ReturnColor(result);
                        KeppySynthConfiguratorMain.Delegate.Lis.Items.Add(SF);
                    }
                }
            }
            catch (Exception ex2)
            {
                Program.DebugToConsole(true, null, ex2);
                Functions.ShowErrorDialog(1, System.Media.SystemSounds.Asterisk, "Fatal error", "Fatal error during the execution of this program!\n\nPress OK to quit.", true, ex2);
                Environment.Exit(-1);
            }
        }


        public static string StripSFZValues(string SFToStrip)
        {
            if (SFToStrip.ToLower().IndexOf('=') != -1)
            {
                var matches = System.Text.RegularExpressions.Regex.Matches(SFToStrip, "[0-9]+");
                return SFToStrip.Substring(SFToStrip.LastIndexOf('|') + 1);
            }
            else
            {
                return SFToStrip;
            }
        }

        public static void SaveList(String SelectedList) // Saves the selected list to the hard drive
        {
            if (!KeppySynthConfiguratorMain.AvoidSave)
            {
                using (StreamWriter sw = new StreamWriter(SelectedList))
                {
                    foreach (ListViewItem item in KeppySynthConfiguratorMain.Delegate.Lis.Items)
                    {
                        String FirstChar;

                        if (item.ForeColor == SFEnabled)
                            FirstChar = "";
                        else if (item.ForeColor == SFDisabled)
                            FirstChar = "@";
                        else
                            FirstChar = "";

                        sw.WriteLine(String.Format("{0}{1}", FirstChar, item.Text.ToString()));
                    }
                }
                Program.DebugToConsole(false, String.Format("Soundfont list saved: {0}", SelectedList), null);
            }
        }

        public static void TriggerReload(Boolean forced) // Tells Keppy's Synthesizer to load a specific list
        {
            try
            {
                if (Properties.Settings.Default.AutoLoadList || forced)
                {
                    if (Convert.ToInt32(KeppySynthConfiguratorMain.Watchdog.GetValue("currentsflist")) == KeppySynthConfiguratorMain.whichone)
                        KeppySynthConfiguratorMain.Watchdog.SetValue("rel" + KeppySynthConfiguratorMain.whichone.ToString(), "1", RegistryValueKind.DWord);

                    Program.DebugToConsole(false, String.Format("(Re)Loaded soundfont list {0}.", KeppySynthConfiguratorMain.whichone), null);
                    KeppySynthConfiguratorMain.Delegate.Lis.Refresh();
                }
            }
            catch
            {
                Functions.InitializeLastPath();
            }
        }

        public static void SetLastPath(string path) // Saves the last path from the SoundfontImport dialog to the registry 
        {
            try
            {
                KeppySynthConfiguratorMain.LastBrowserPath = path;
                KeppySynthConfiguratorMain.SynthPaths.SetValue("lastpathsfimport", path);
                Program.DebugToConsole(false, String.Format("Last Explorer path is: {0}", path), null);
            }
            catch
            {
                Functions.InitializeLastPath();
            }
        }

        public static void SetLastImportExportPath(string path) // Saves the last path from the ExternalListImport/ExternalListExport dialog to the registry 
        {
            try
            {
                KeppySynthConfiguratorMain.LastImportExportPath = path;
                KeppySynthConfiguratorMain.SynthPaths.SetValue("lastpathlistimpexp", path);
                Program.DebugToConsole(false, String.Format("Last Import/Export path is: {0}", path), null);
            }
            catch
            {
                Functions.InitializeLastPath();
            }
        }

        public static void OpenSFWithDefaultApp(String SoundFont) // Basically changes the directory's name
        {
            try
            {
                if (SoundFont.ToLower().IndexOf('=') != -1)
                {
                    var matches = System.Text.RegularExpressions.Regex.Matches(SoundFont, "[0-9]+");
                    string sf = SoundFont.Substring(SoundFont.LastIndexOf('|') + 1);
                    Process.Start(sf);
                }
                else if (SoundFont.ToLower().IndexOf('@') != -1)
                {
                    string sf = SoundFont.Substring(SoundFont.LastIndexOf('@') + 1);
                    Process.Start(sf);
                }
                else
                {
                    Process.Start(SoundFont);
                }
            }
            catch
            {

            }
        }

        public static void OpenSFDirectory(String SoundFont) // Basically changes the directory's name
        {
            try
            {
                if (SoundFont.ToLower().IndexOf('=') != -1)
                {
                    var matches = System.Text.RegularExpressions.Regex.Matches(SoundFont, "[0-9]+");
                    string sf = SoundFont.Substring(SoundFont.LastIndexOf('|') + 1);
                    Process.Start(Path.GetDirectoryName(sf));
                }
                else if (SoundFont.ToLower().IndexOf('@') != -1)
                {
                    string sf = SoundFont.Substring(SoundFont.LastIndexOf('@') + 1);
                    Process.Start(Path.GetDirectoryName(sf));
                }
                else
                {
                    Process.Start(Path.GetDirectoryName(SoundFont));
                }
            }
            catch
            {

            }
        }

    }
}
