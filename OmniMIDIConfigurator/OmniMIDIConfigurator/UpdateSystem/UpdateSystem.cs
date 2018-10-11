using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace OmniMIDIConfigurator
{
    class UpdateSystem
    {
        [DllImport("wininet.dll")]
        public extern static bool InternetGetConnectedState(out int connDescription, int ReservedValue);

        public static string ProductName = "OmniMIDI";
        public static Octokit.GitHubClient UpdateClient = new Octokit.GitHubClient(new Octokit.ProductHeaderValue(ProductName));
     
        public static string UpdateFile = "https://github.com/KeppySoftware/OmniMIDI/releases/download/{0}/OmniMIDIUpdate.exe";
        public static string UpdatePage = "https://github.com/KaleidonKep99/OmniMIDI/releases/tag/{0}";
        public static string UpdateFileVersion = String.Format("{0}\\OmniMIDI\\OmniMIDI.dll", Environment.GetFolderPath(Environment.SpecialFolder.System));

        public static bool IsInternetAvailable()
        {
            int Desc;
            return InternetGetConnectedState(out Desc, 0);
        }

        public static void CheckForTLS12ThenUpdate(String ReturnVal)
        {
            if (!ReturnVal.Equals("0.0.0.0"))
            {
                if (!Properties.Settings.Default.TLS12Missing)
                {
                    Forms.DLEngine frm = new Forms.DLEngine(ReturnVal, String.Format("Downloading update {0}...", ReturnVal, @"{0}"), null, null, 0, true);
                    frm.StartPosition = FormStartPosition.CenterScreen;
                    frm.ShowDialog();
                }
                else Process.Start(String.Format(UpdatePage, ReturnVal));
            }
        }

        public static void TriggerUpdateWindow(Version y, Version x, String newestversion, bool forced, bool startup, bool isitfromthechangelogwindow)
        {
            String ReturnVal = "0.0.0.0";
            if (forced && startup) CheckForTLS12ThenUpdate(newestversion);
            else
            {
                UpdateYesNo upd = new UpdateYesNo(x, y, true, startup, isitfromthechangelogwindow);

                if (startup)
                    upd.StartPosition = FormStartPosition.CenterScreen;
                else
                    upd.StartPosition = FormStartPosition.CenterParent;

                DialogResult dialogResult = upd.ShowDialog();
                ReturnVal = upd.ReturnVal;
                upd.Dispose();

                if (dialogResult == DialogResult.Yes) CheckForTLS12ThenUpdate(ReturnVal);
            }
        }

        public static void NoUpdates(bool startup, bool internetok)
        {
            if (!startup)
            {
                UpdateYesNo upd = new UpdateYesNo(null, null, internetok, startup, false);
                upd.StartPosition = FormStartPosition.CenterParent;
                upd.ShowDialog();
                upd.Dispose();
            }
        }

        public static void CheckChangelog()
        {
            bool internetok = IsInternetAvailable();
            if (internetok == false)
            {
                MessageBox.Show("There's no Internet connection.\n\nYou can't see the changelog without one.", String.Format("{0} - No Internet connection available", ProductName), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    Octokit.Release Release = UpdateClient.Repository.Release.GetLatest("KeppySoftware", "OmniMIDI").Result;
                    Process.Start(String.Format(UpdatePage, Release.TagName));
                }
                catch (Exception ex)
                {
                    Functions.ShowErrorDialog(ErrorType.Error, System.Media.SystemSounds.Exclamation, "Unknown error", "An error has occurred while trying to show you the latest changelog.\nPlease try again later.\n\nPress OK to continue.", true, ex);
                }
            }
        }

        public static string GetCurrentBranch()
        {
            if (Properties.Settings.Default.UpdateBranch == "canary")
                return "Canary branch";
            else if (Properties.Settings.Default.UpdateBranch == "normal")
                return "Normal branch";
            else if (Properties.Settings.Default.UpdateBranch == "delay")
                return "Delayed branch";
            else if (Properties.Settings.Default.UpdateBranch == "choose")
                return "No branch selected";
            else
                return "Normal branch";
        }

        public static Color GetCurrentBranchColor()
        {
            if (Properties.Settings.Default.UpdateBranch == "canary")
                return Color.FromArgb(221, 172, 5);
            else if (Properties.Settings.Default.UpdateBranch == "normal")
                return Color.FromArgb(158, 14, 204);
            else if (Properties.Settings.Default.UpdateBranch == "delay")
                return Color.FromArgb(84, 110, 122);
            else if (Properties.Settings.Default.UpdateBranch == "choose")
                return Color.FromArgb(182, 0, 0);
            else
                return Color.FromArgb(255, 255, 255);
        }

        public static string GetCurrentBranchToolTip()
        {
            if (Properties.Settings.Default.UpdateBranch == "canary")
                return "Receive all updates.\nYou may get broken updates that haven't been fully tested.\nDesigned for testers and early adopters.";
            else if (Properties.Settings.Default.UpdateBranch == "normal")
                return "Receive occasional updates and urgent bugfixes (Eg. from version x.x.1.x to x.x.2.x).\nRecommended.";
            else if (Properties.Settings.Default.UpdateBranch == "delay")
                return "You will only get major releases (Eg. from version x.1.x.x to x.2.x.x).\nFor those who do not wish to update often.\nNot recommended.";
            else if (Properties.Settings.Default.UpdateBranch == "choose")
                return "No information, since you didn't chose a branch.";
            else
                return "Receive occasional updates and urgent bugfixes (Eg. from version x.x.1.x to x.x.2.x).\nRecommended.";
        }

        public static string CheckForUpdatesMini()
        {
            Program.DebugToConsole(false, "Checking for updates...", null);

            bool internetok = IsInternetAvailable();
            if (internetok == false)
            {
                Program.DebugToConsole(false, "No Internet available.", null);
                return "nointernet";
            }
            else
            {
                try
                {
                    Octokit.Release Release = UpdateClient.Repository.Release.GetLatest("KeppySoftware", "OmniMIDI").Result;
                    FileVersionInfo Driver = FileVersionInfo.GetVersionInfo(UpdateFileVersion);

                    Version DriverOnline = null;
                    Version.TryParse(Release.TagName, out DriverOnline);
                    Version DriverCurrent = null;
                    Version.TryParse(Driver.FileVersion.ToString(), out DriverCurrent);

                    if (Properties.Settings.Default.UpdateBranch == "canary")
                    {
                        if (DriverCurrent < DriverOnline)
                        {
                            Program.DebugToConsole(false, String.Format("New version found. ({0})", Release.TagName), null);
                            return "yes";
                        }
                        else
                        {
                            Program.DebugToConsole(false, String.Format("No updates have been found. Latest canary release is {0}.", Release.TagName), null);
                            return "no";
                        }
                    }
                    else if (Properties.Settings.Default.UpdateBranch == "normal")
                    {
                        if (DriverCurrent.Major < DriverOnline.Major || DriverCurrent.Minor < DriverOnline.Minor)
                        {
                            if ((DriverCurrent.Build >= DriverOnline.Build || DriverCurrent.Build < DriverOnline.Build))
                            {
                                Program.DebugToConsole(false, String.Format("New version found. ({0})", Release.TagName), null);
                                return "yes";
                            }
                            else
                            {
                                Program.DebugToConsole(false, String.Format("No updates have been found. Latest canary release is {0}.", Release.TagName), null);
                                return "no";
                            }
                        }
                        else
                        {
                            Program.DebugToConsole(false, String.Format("No updates have been found. Latest canary release is {0}.", Release.TagName), null);
                            return "no";
                        }
                    }
                    else if (Properties.Settings.Default.UpdateBranch == "delay")
                    {
                        if (DriverCurrent.Major < DriverOnline.Major)
                        {
                            if ((DriverCurrent.Minor >= DriverOnline.Minor || DriverCurrent.Minor < DriverOnline.Minor))
                            {
                                Program.DebugToConsole(false, String.Format("New version found. ({0})", Release.TagName), null);
                                return "yes";
                            }
                            else
                            {
                                Program.DebugToConsole(false, String.Format("No updates have been found. Latest canary release is {0}.", Release.TagName), null);
                                return "no";
                            }
                        }
                        else
                        {
                            Program.DebugToConsole(false, String.Format("No updates have been found. Latest canary release is {0}.", Release.TagName), null);
                            return "no";
                        }
                    }
                    else
                    {
                        Program.DebugToConsole(false, String.Format("No updates have been found. Latest canary release is {0}.", Release.TagName), null);
                        return "no";
                    }
                }
                catch
                {
                    Program.DebugToConsole(false, "Error while checking for updates.", null);
                    return "fail";
                }
            }
        }

        public static void CheckForUpdates(bool forced, bool startup, bool isitfromthechangelogwindow)
        {
            bool internetok = IsInternetAvailable();
            if (internetok == false)
            {
                NoUpdates(startup, false);
            }
            else
            {
                try
                {
                    Octokit.Release Release = UpdateClient.Repository.Release.GetLatest("KeppySoftware", "OmniMIDI").Result;
                    FileVersionInfo Driver = FileVersionInfo.GetVersionInfo(UpdateFileVersion);

                    Version DriverOnline = null;
                    Version.TryParse(Release.TagName, out DriverOnline);
                    Version DriverCurrent = null;
                    Version.TryParse(Driver.FileVersion.ToString(), out DriverCurrent);

                    if (forced == true)
                    {
                        if (Properties.Settings.Default.UpdateBranch == "canary")
                        {
                            if (DriverCurrent < DriverOnline)
                            {
                                Program.DebugToConsole(false, String.Format("New version found. Requesting user to download it. ({0})", Release.TagName), null);
                                TriggerUpdateWindow(DriverCurrent, DriverOnline, Release.TagName, forced, startup, isitfromthechangelogwindow);
                            }
                            else
                            {
                                Program.DebugToConsole(false, String.Format("The user forced a reinstall/downgrade of the program. ({0})", Release.TagName), null);
                                TriggerUpdateWindow(DriverCurrent, DriverOnline, Driver.FileVersion, forced, startup, isitfromthechangelogwindow);
                            }
                        }
                        else if (Properties.Settings.Default.UpdateBranch == "normal")
                        {
                            if (DriverCurrent.Major < DriverOnline.Major || DriverCurrent.Minor < DriverOnline.Minor)
                            {
                                if ((DriverCurrent.Build >= DriverOnline.Build || DriverCurrent.Build < DriverOnline.Build))
                                {
                                    Program.DebugToConsole(false, String.Format("New version found. Requesting user to download it. ({0})", Release.TagName), null);
                                    TriggerUpdateWindow(DriverCurrent, DriverOnline, Release.TagName, forced, startup, isitfromthechangelogwindow);
                                }
                                else
                                {
                                    Program.DebugToConsole(false, String.Format("The user forced a reinstall/downgrade of the program. ({0})", Release.TagName), null);
                                    TriggerUpdateWindow(DriverCurrent, DriverOnline, Driver.FileVersion, forced, startup, isitfromthechangelogwindow);
                                }
                            }
                            else
                            {
                                Program.DebugToConsole(false, String.Format("The user forced a reinstall/downgrade of the program. ({0})", Release.TagName), null);
                                TriggerUpdateWindow(DriverCurrent, DriverOnline, Driver.FileVersion, forced, startup, isitfromthechangelogwindow);
                            }
                        }
                        else if (Properties.Settings.Default.UpdateBranch == "delay")
                        {
                            if (DriverCurrent.Major < DriverOnline.Major)
                            {
                                if ((DriverCurrent.Minor >= DriverOnline.Minor || DriverCurrent.Minor < DriverOnline.Minor))
                                {
                                    Program.DebugToConsole(false, String.Format("New version found. Requesting user to download it. ({0})", Release.TagName), null);
                                    TriggerUpdateWindow(DriverCurrent, DriverOnline, Release.TagName, forced, startup, isitfromthechangelogwindow);
                                }
                                else
                                {
                                    Program.DebugToConsole(false, String.Format("The user forced a reinstall/downgrade of the program. ({0})", Release.TagName), null);
                                    TriggerUpdateWindow(DriverCurrent, DriverOnline, Driver.FileVersion, forced, startup, isitfromthechangelogwindow);
                                }
                            }
                            else
                            {
                                Program.DebugToConsole(false, String.Format("The user forced a reinstall/downgrade of the program. ({0})", Release.TagName), null);
                                TriggerUpdateWindow(DriverCurrent, DriverOnline, Driver.FileVersion, forced, startup, isitfromthechangelogwindow);
                            }
                        }
                    }
                    else
                    {
                        if (Properties.Settings.Default.UpdateBranch == "canary")
                        {
                            if (DriverCurrent < DriverOnline)
                            {
                                Program.DebugToConsole(false, String.Format("New version found. Requesting user to download it. ({0})", Release.TagName), null);
                                TriggerUpdateWindow(DriverCurrent, DriverOnline, Release.TagName, forced, startup, isitfromthechangelogwindow);
                            }
                            else
                            {
                                Program.DebugToConsole(false, String.Format("No updates have been found. Latest canary release is {0}.", Release.TagName), null);
                                NoUpdates(startup, internetok);
                            }
                        }
                        else if (Properties.Settings.Default.UpdateBranch == "normal")
                        {
                            if (DriverCurrent.Major < DriverOnline.Major || DriverCurrent.Minor < DriverOnline.Minor)
                            {
                                if ((DriverCurrent.Build >= DriverOnline.Build || DriverCurrent.Build < DriverOnline.Build))
                                {
                                    Program.DebugToConsole(false, String.Format("New version found. Requesting user to download it. ({0})", Release.TagName), null);
                                    TriggerUpdateWindow(DriverCurrent, DriverOnline, Release.TagName, forced, startup, isitfromthechangelogwindow);
                                }
                                else
                                {
                                    Program.DebugToConsole(false, String.Format("No updates have been found. Latest canary release is {0}.", Release.TagName), null);
                                    NoUpdates(startup, internetok);
                                }
                            }
                            else
                            {
                                Program.DebugToConsole(false, String.Format("No updates have been found. Latest canary release is {0}.", Release.TagName), null);
                                NoUpdates(startup, internetok);
                            }
                        }
                        else if (Properties.Settings.Default.UpdateBranch == "delay")
                        {
                            if (DriverCurrent.Major < DriverOnline.Major)
                            {
                                if ((DriverCurrent.Minor >= DriverOnline.Minor || DriverCurrent.Minor < DriverOnline.Minor))
                                {
                                    Program.DebugToConsole(false, String.Format("New version found. Requesting user to download it. ({0})", Release.TagName), null);
                                    TriggerUpdateWindow(DriverCurrent, DriverOnline, Release.TagName, forced, startup, isitfromthechangelogwindow);
                                }
                                else
                                {
                                    Program.DebugToConsole(false, String.Format("No updates have been found. Latest canary release is {0}.", Release.TagName), null);
                                    NoUpdates(startup, internetok);
                                }
                            }
                            else
                            {
                                Program.DebugToConsole(false, String.Format("No updates have been found. Latest canary release is {0}.", Release.TagName), null);
                                NoUpdates(startup, internetok);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Program.DebugToConsole(true, "An error has occurred while checking for updates.", ex);
                    NoUpdates(startup, internetok);
                }
            }
        }
    }
}
