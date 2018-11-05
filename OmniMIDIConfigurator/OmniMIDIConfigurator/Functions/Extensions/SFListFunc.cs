using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Midi;

namespace OmniMIDIConfigurator
{
    class SFListFunc
    {
        public static Color SFEnabled = Color.FromArgb(0, 0, 0);
        public static Color SFDisabled = Color.FromArgb(170, 170, 170);

        public static string ReturnSoundFontSize(string preset, string ext, long length)
        {
            if (ext.ToLowerInvariant() != ".sfz")
            {
                string size = Functions.ReturnLength(length, false);
                return size;
            }
            else
            {
                long size = SFZInfo.GetSoundFontZSize(preset);
                if (size > 0) return Functions.ReturnLength(size, true);
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

        public static Color ReturnColor(String result)
        {
            if (result == "@")
                return SFDisabled;
            else
                return SFEnabled;
        }

        public static string GetName(string value)
        {
            int A = value.IndexOf(" = ");
            if (A == -1) return "";
            return value.Substring(0, A);
        }

        public static string GetValue(string value)
        {
            int A = value.LastIndexOf(" = ");
            if (A == -1) return "";
            int A2 = A + (" = ").Length;
            if (A2 >= value.Length) return "";
            return value.Substring(A2);
        }

        public static void ChangeList(int SelectedList) // When you select a list from the combobox, it'll load the items from the selected list to the listbox
        {
            OmniMIDIConfiguratorMain.CurrentList = OmniMIDIConfiguratorMain.ListsPath[SelectedList];
            OmniMIDIConfiguratorMain.whichone = SelectedList + 1;
            String WhichList = OmniMIDIConfiguratorMain.CurrentList;

            try
            {
                if (!System.IO.Directory.Exists(OmniMIDIConfiguratorMain.AbsolutePath))
                {
                    Directory.CreateDirectory(OmniMIDIConfiguratorMain.AbsolutePath);
                    Directory.CreateDirectory(OmniMIDIConfiguratorMain.PathToAllLists);
                    File.Create(WhichList).Dispose();
                    OmniMIDIConfiguratorMain.Delegate.Lis.Items.Clear();
                }
                if (!System.IO.Directory.Exists(OmniMIDIConfiguratorMain.PathToAllLists))
                {
                    Directory.CreateDirectory(OmniMIDIConfiguratorMain.PathToAllLists);
                    File.Create(WhichList).Dispose();
                    OmniMIDIConfiguratorMain.Delegate.Lis.Items.Clear();
                }
                if (!System.IO.File.Exists(WhichList))
                {
                    // Oops, list is missing
                    File.Create(WhichList).Dispose();
                    OmniMIDIConfiguratorMain.Delegate.Lis.Items.Clear();
                    Functions.ShowErrorDialog(ErrorType.Information, System.Media.SystemSounds.Question, "Information", "The soundfont list was missing, so the configurator automatically created it for you.", false, null);
                }
                try
                {
                    using (StreamReader r = new StreamReader(WhichList))
                    {
                        Boolean AlreadyInitialized = false;

                        Boolean EnableState = false;
                        String SFPath = null;
                        Int32 SourcePreset = -1;
                        Int32 SourceBank = -1;
                        Int32 DestinationPreset = -1;
                        Int32 DestinationBank = 0;
                        Boolean XGDrumsetMode = false;

                        ListViewItem SF = new ListViewItem(new[] {
                                    "Unrecognizable SoundFont",
                                    "0", "0", "0", "0", "No",
                                    "Missing",
                                    "N/A"
                                    });
                        OmniMIDIConfiguratorMain.Delegate.Lis.Items.Clear();
                        OmniMIDIConfiguratorMain.Delegate.Lis.Refresh();

                        string line;
                        while ((line = r.ReadLine()) != null)
                        {
                            try
                            {
                                if (line.Equals("sf.start"))
                                {
                                    if (AlreadyInitialized) continue;

                                    // It begins, again...
                                    AlreadyInitialized = true;
                                }
                                else if (line.Equals("sf.end"))
                                {
                                    if (!AlreadyInitialized) continue;

                                    // Add it to the list.
                                    FileInfo file = new FileInfo(SFPath);
                                    SF = new ListViewItem(new[] {
                                        SFPath,
                                        SourcePreset.ToString(), SourceBank.ToString(), DestinationPreset.ToString(), DestinationBank.ToString(),
                                        (XGDrumsetMode ? "Yes" : "No"),
                                        ReturnSoundFontFormat(Path.GetExtension(SFPath)),
                                        ReturnSoundFontSize(SFPath, Path.GetExtension(SFPath), file.Length)
                                    });
                                    SF.ForeColor = EnableState ? SFEnabled : SFDisabled;
                                    OmniMIDIConfiguratorMain.Delegate.Lis.Items.Add(SF);

                                    // Reset values
                                    EnableState = false;
                                    SFPath = null;
                                    SourcePreset = -1;
                                    SourceBank = -1;
                                    DestinationPreset = -1;
                                    DestinationBank = 0;
                                    XGDrumsetMode = false;

                                    AlreadyInitialized = false;
                                }
                                else if (GetName(line).Equals("sf.path") && SFPath == null)
                                {
                                    if (!AlreadyInitialized) continue;

                                    // We've found the path! Parse it.
                                    SFPath = GetValue(line);
                                }
                                else if (GetName(line).Equals("sf.enabled"))
                                {
                                    if (!AlreadyInitialized) continue;

                                    // We've found the enable state! Crush it!
                                    EnableState = Convert.ToBoolean(Convert.ToInt32(GetValue(line)));
                                }
                                else if (GetName(line).Equals("sf.srcp"))
                                {
                                    if (!AlreadyInitialized) continue;

                                    // We've found the source preset! Take it!
                                    SourcePreset = Convert.ToInt32(GetValue(line));
                                }
                                else if (GetName(line).Equals("sf.srcb"))
                                {
                                    if (!AlreadyInitialized) continue;

                                    // We've found the source bank! Read it!
                                    SourceBank = Convert.ToInt32(GetValue(line));
                                }
                                else if (GetName(line).Equals("sf.desp"))
                                {
                                    if (!AlreadyInitialized) continue;

                                    // We've found the destination preset! Munch it!
                                    DestinationPreset = Convert.ToInt32(GetValue(line));
                                }
                                else if (GetName(line).Equals("sf.desb"))
                                {
                                    if (!AlreadyInitialized) continue;

                                    // We've found the destination preset! Munch it!
                                    DestinationBank = Convert.ToInt32(GetValue(line));
                                }
                                else if (GetName(line).Equals("sf.xgdrums"))
                                {
                                    if (!AlreadyInitialized) continue;

                                    // We've found the enable state! Crush it!
                                    XGDrumsetMode = Convert.ToBoolean(Convert.ToInt32(GetValue(line)));
                                }
                                else continue;
                            }
                            catch
                            {
                                SF = new ListViewItem(new[] {
                                    "Unrecognizable SoundFont",
                                    "0", "0", "0", "0", "No",
                                    "Missing",
                                    "N/A"
                                });

                                SF.ForeColor = Color.Red;
                                OmniMIDIConfiguratorMain.Delegate.Lis.Items.Add(SF);

                                Program.DebugToConsole(false, String.Format("{0} is missing.", line), null);
                            }
                        }
                    }
                }
                catch
                {
                    // Oops, list is missing
                    File.Create(WhichList).Dispose();
                    OmniMIDIConfiguratorMain.Delegate.Lis.Items.Clear();
                    Functions.ShowErrorDialog(ErrorType.Information, System.Media.SystemSounds.Question, "Information", "The soundfont list was missing, so the configurator automatically created it for you.", false, null);
                }
                Program.DebugToConsole(false, String.Format("Switched to soundfont list {0}.", SelectedList + 1), null);
            }
            catch (Exception ex)
            {
                // Oops, something went wrong
                Program.DebugToConsole(true, null, ex);
                Functions.ShowErrorDialog(ErrorType.Error, System.Media.SystemSounds.Hand, "Fatal error", "Fatal error during the execution of the program.\n\nPress OK to quit.", true, ex);
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
                        Functions.ShowErrorDialog(ErrorType.Error, System.Media.SystemSounds.Hand, "Error while adding soundfont", String.Format("{0} is not a valid soundfont file!", Path.GetFileName(Soundfonts[i])), false, null);
                    }
                    else
                    {
                        if (OmniMIDIConfiguratorMain.Delegate.BankPresetOverride.Checked == true)
                        {
                            using (var form = new BankNPresetSel(Path.GetFileName(Soundfonts[i]), false, true, null))
                            {
                                var result = form.ShowDialog();
                                if (result == DialogResult.OK)
                                {
                                    FileInfo file = new FileInfo(Soundfonts[i]);
                                    ListViewItem SF = new ListViewItem(new[] {
                                        Soundfonts[i],
                                        form.BankValueReturn, form.PresetValueReturn, form.DesBankValueReturn, form.DesPresetValueReturn, form.XGModeC ? "Yes" : "No",
                                        ReturnSoundFontFormat(Path.GetExtension(Soundfonts[i])),
                                        ReturnSoundFontSize(Soundfonts[i], Path.GetExtension(Soundfonts[i]), file.Length)
                                    });
                                    SF.ForeColor = SFEnabled;
                                    OmniMIDIConfiguratorMain.Delegate.Lis.Items.Add(SF);
                                }
                            }
                        }
                        else
                        {
                            FileInfo file = new FileInfo(Soundfonts[i]);
                            ListViewItem SF = new ListViewItem(new[] {
                                        Soundfonts[i],
                                        "-1", "-1", "-1", "0", "No",
                                        ReturnSoundFontFormat(Path.GetExtension(Soundfonts[i])),
                                        ReturnSoundFontSize(Soundfonts[i], Path.GetExtension(Soundfonts[i]), file.Length)
                                    });
                            SF.ForeColor = SFEnabled;
                            OmniMIDIConfiguratorMain.Delegate.Lis.Items.Add(SF);
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
                        Functions.ShowErrorDialog(ErrorType.Error, System.Media.SystemSounds.Hand, "Error while adding soundfont", String.Format("{0} is not a valid soundfont file!", Path.GetFileName(Soundfonts[i])), false, null);
                    }
                    else
                    {
                        using (var form = new BankNPresetSel(Path.GetFileName(Soundfonts[i]), false, false, null))
                        {
                            var result = form.ShowDialog();
                            if (result == DialogResult.OK)
                            {
                                FileInfo file = new FileInfo(Soundfonts[i]);
                                ListViewItem SF = new ListViewItem(new[] {
                                        Soundfonts[i],
                                        form.BankValueReturn, form.PresetValueReturn, form.DesBankValueReturn, form.DesPresetValueReturn, form.XGModeC ? "Yes" : "No",
                                        ReturnSoundFontFormat(Path.GetExtension(Soundfonts[i])),
                                        ReturnSoundFontSize(Soundfonts[i], Path.GetExtension(Soundfonts[i]), file.Length)
                                    });
                                SF.ForeColor = SFEnabled;
                                OmniMIDIConfiguratorMain.Delegate.Lis.Items.Add(SF);
                            }
                        }
                        SaveList(CurrentList);
                        TriggerReload(false);
                    }
                    Program.DebugToConsole(false, String.Format("Added soundfont to list: {0}", Soundfonts[i]), null);
                }
                else if (Path.GetExtension(Soundfonts[i]).ToLowerInvariant() == ".dls")
                {
                    Functions.ShowErrorDialog(ErrorType.Error, System.Media.SystemSounds.Exclamation, "Error", "BASSMIDI does NOT support the downloadable sounds (DLS) format!", false, null);
                }
                else if (Path.GetExtension(Soundfonts[i]).ToLowerInvariant() == ".exe" | Path.GetExtension(Soundfonts[i]).ToLowerInvariant() == ".dll")
                {
                    Functions.ShowErrorDialog(ErrorType.Error, System.Media.SystemSounds.Exclamation, "Error", "Are you really trying to add executables to the soundfonts list?", false, null);
                }
                else
                {
                    Functions.ShowErrorDialog(ErrorType.Error, System.Media.SystemSounds.Exclamation, "Error", String.Format("Invalid soundfont!\n\nFile: {0}\n\nPlease select a valid soundfont and try again!", Soundfonts[i]), false, null);
                }
            }
        }

        public static void ReinitializeList(Exception ex, String selectedlistpath) // The app encountered an error, so it'll restore the original soundfont list back
        {
            try
            {
                Program.DebugToConsole(true, null, ex);
                Functions.ShowErrorDialog(ErrorType.Error, System.Media.SystemSounds.Exclamation, "Error", "Oh snap!\nThe configurator encountered an error while editing the following list:\n" + selectedlistpath, true, ex);
                OmniMIDIConfiguratorMain.Delegate.Lis.Items.Clear();
                using (StreamReader r = new StreamReader(selectedlistpath))
                {
                    Boolean AlreadyInitialized = false;

                    Boolean EnableState = false;
                    String SFPath = null;
                    Int32 SourcePreset = -1;
                    Int32 SourceBank = -1;
                    Int32 DestinationPreset = -1;
                    Int32 DestinationBank = 0;
                    Boolean XGDrumsetMode = false;

                    ListViewItem SF = new ListViewItem(new[] {
                                    "Unrecognizable SoundFont",
                                    "0", "0", "0", "0", "No",
                                    "Missing",
                                    "N/A"
                                    });
                    OmniMIDIConfiguratorMain.Delegate.Lis.Items.Clear();
                    OmniMIDIConfiguratorMain.Delegate.Lis.Refresh();

                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        try
                        {
                            if (line.Equals("sf.start"))
                            {
                                if (AlreadyInitialized) continue;

                                // It begins, again...
                                AlreadyInitialized = true;
                            }
                            else if (line.Equals("sf.end"))
                            {
                                if (!AlreadyInitialized) continue;

                                // Add it to the list.
                                FileInfo file = new FileInfo(SFPath);
                                SF = new ListViewItem(new[] {
                                        SFPath,
                                        SourcePreset.ToString(), SourceBank.ToString(), DestinationPreset.ToString(), DestinationBank.ToString(),
                                        (XGDrumsetMode ? "Yes" : "No"),
                                        ReturnSoundFontFormat(Path.GetExtension(SFPath)),
                                        ReturnSoundFontSize(SFPath, Path.GetExtension(SFPath), file.Length)
                                    });
                                SF.ForeColor = EnableState ? SFEnabled : SFDisabled;
                                OmniMIDIConfiguratorMain.Delegate.Lis.Items.Add(SF);

                                // Reset values
                                EnableState = false;
                                SFPath = null;
                                SourcePreset = -1;
                                SourceBank = -1;
                                DestinationPreset = -1;
                                DestinationBank = 0;
                                XGDrumsetMode = false;

                                AlreadyInitialized = false;
                            }
                            else if (GetName(line).Equals("sf.path") && SFPath == null)
                            {
                                if (!AlreadyInitialized) continue;

                                // We've found the path! Parse it.
                                SFPath = GetValue(line);
                            }
                            else if (GetName(line).Equals("sf.enabled"))
                            {
                                if (!AlreadyInitialized) continue;

                                // We've found the enable state! Crush it!
                                EnableState = Convert.ToBoolean(Convert.ToInt32(GetValue(line)));
                            }
                            else if (GetName(line).Equals("sf.srcp"))
                            {
                                if (!AlreadyInitialized) continue;

                                // We've found the source preset! Take it!
                                SourcePreset = Convert.ToInt32(GetValue(line));
                            }
                            else if (GetName(line).Equals("sf.srcb"))
                            {
                                if (!AlreadyInitialized) continue;

                                // We've found the source bank! Read it!
                                SourceBank = Convert.ToInt32(GetValue(line));
                            }
                            else if (GetName(line).Equals("sf.desp"))
                            {
                                if (!AlreadyInitialized) continue;

                                // We've found the destination preset! Munch it!
                                DestinationPreset = Convert.ToInt32(GetValue(line));
                            }
                            else if (GetName(line).Equals("sf.desb"))
                            {
                                if (!AlreadyInitialized) continue;

                                // We've found the destination preset! Munch it!
                                DestinationBank = Convert.ToInt32(GetValue(line));
                            }
                            else if (GetName(line).Equals("sf.xgdrums"))
                            {
                                if (!AlreadyInitialized) continue;

                                // We've found the enable state! Crush it!
                                XGDrumsetMode = Convert.ToBoolean(Convert.ToInt32(GetValue(line)));
                            }
                            else continue;
                        }
                        catch
                        {
                            SF = new ListViewItem(new[] {
                                    "Unrecognizable SoundFont",
                                    "0", "0", "0", "0", "No",
                                    "Missing",
                                    "N/A"
                                });

                            SF.ForeColor = Color.Red;
                            OmniMIDIConfiguratorMain.Delegate.Lis.Items.Add(SF);

                            Program.DebugToConsole(false, String.Format("{0} is missing.", line), null);
                        }
                    }
                }
            }
            catch (Exception ex2)
            {
                Program.DebugToConsole(true, null, ex2);
                Functions.ShowErrorDialog(ErrorType.Error, System.Media.SystemSounds.Asterisk, "Fatal error", "Fatal error during the execution of this program!\n\nPress OK to quit.", true, ex2);
                Environment.Exit(-1);
            }
        }

        public static string GetSFZValues(string SFToAnalyze)
        {
            if (SFToAnalyze.ToLower().IndexOf('=') != -1)
                return SFToAnalyze.Substring(0, SFToAnalyze.IndexOf('|'));
            else
                return null;
        }

        public static string StripSFZValues(string SFToStrip)
        {
            if (SFToStrip.ToLower().IndexOf('=') != -1)
                return SFToStrip.Substring(SFToStrip.LastIndexOf('|') + 1);
            else
                return SFToStrip;
        }

        public static string StripDisabledState(string SFToStrip)
        {
            int charLocation = SFToStrip.IndexOf('|');

            if (SFToStrip.Substring(0, charLocation).Contains('@'))
                return Regex.Replace(SFToStrip, "@", "");
            else
                return SFToStrip;
        }

        public static void SaveList(String SelectedList) // Saves the selected list to the hard drive
        {
            if (!OmniMIDIConfiguratorMain.AvoidSave)
            {
                using (StreamWriter sw = new StreamWriter(SelectedList))
                {
                    UInt32 SFCount = 1;
                    sw.WriteLine("// Generated by OmniMIDI\n");
                    foreach (ListViewItem item in OmniMIDIConfiguratorMain.Delegate.Lis.Items)
                    {
                        sw.WriteLine(String.Format("// SoundFont n°{0}", SFCount));
                        sw.WriteLine("sf.start");
                        sw.WriteLine(String.Format("sf.path = {0}", item.Text));
                        sw.WriteLine(String.Format("sf.enabled = {0}", (item.ForeColor == SFEnabled) ? "1" : "0"));
                        sw.WriteLine(String.Format("sf.srcp = {0}", item.SubItems[1].Text));
                        sw.WriteLine(String.Format("sf.srcb = {0}", item.SubItems[2].Text));
                        sw.WriteLine(String.Format("sf.desp = {0}", item.SubItems[3].Text));
                        sw.WriteLine(String.Format("sf.desb = {0}", item.SubItems[4].Text));
                        sw.WriteLine(String.Format("sf.xgdrums = {0}", (item.SubItems[5].Text.Equals("Yes")) ? "1" : "0"));
                        sw.WriteLine("sf.end\n");
                        SFCount++;
                    }
                    sw.WriteLine("// Generated by OmniMIDI");
                }
                Program.DebugToConsole(false, String.Format("Soundfont list saved: {0}", SelectedList), null);
            }
        }

        public static void TriggerReload(Boolean forced) // Tells OmniMIDI to load a specific list
        {
            try
            {
                if (Properties.Settings.Default.AutoLoadList || forced)
                {
                    if (Convert.ToInt32(OmniMIDIConfiguratorMain.Watchdog.GetValue("currentsflist")) == OmniMIDIConfiguratorMain.whichone)
                        OmniMIDIConfiguratorMain.Watchdog.SetValue("rel" + OmniMIDIConfiguratorMain.whichone.ToString(), "1", RegistryValueKind.DWord);

                    Program.DebugToConsole(false, String.Format("(Re)Loaded soundfont list {0}.", OmniMIDIConfiguratorMain.whichone), null);
                    OmniMIDIConfiguratorMain.Delegate.Lis.Refresh();
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
                OmniMIDIConfiguratorMain.LastBrowserPath = path;
                OmniMIDIConfiguratorMain.SynthPaths.SetValue("lastpathsfimport", path);
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
                OmniMIDIConfiguratorMain.LastImportExportPath = path;
                OmniMIDIConfiguratorMain.SynthPaths.SetValue("lastpathlistimpexp", path);
                Program.DebugToConsole(false, String.Format("Last Import/Export path is: {0}", path), null);
            }
            catch
            {
                Functions.InitializeLastPath();
            }
        }

        public static bool OpenSFWithDefaultApp(String SoundFont)
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

                return true;
            }
            catch { return false; }
        }

        public static bool OpenSFDirectory(String SoundFont)
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

                return true;
            }
            catch { return false; }
        }

    }
}
