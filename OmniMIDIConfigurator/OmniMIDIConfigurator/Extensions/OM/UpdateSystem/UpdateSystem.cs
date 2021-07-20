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

        // public static string UpdateFile = Properties.Settings.Default.ProjectLink + "/releases/download/{0}/OmniMIDIUpdate.exe";
        public static string SetupFile = Properties.Settings.Default.ProjectLink + "/releases/download/{0}/OmniMIDISetup.exe";
        public static string UpdatePage = Properties.Settings.Default.ProjectLink + "/releases/tag/{0}";
        public static string UpdateFileVersion = String.Format("{0}\\OmniMIDI\\OmniMIDI.dll", Environment.GetFolderPath(Environment.SpecialFolder.System));

        public const int NORMAL = 0x0;
        public const int USERFOLDER_PATH = 0x1;
        public const int WIPE_SETTINGS = 0xF;

        public static bool IsInternetAvailable()
        {
            int Desc;
            return InternetGetConnectedState(out Desc, 0);
        }

        public static void CheckForTLS12ThenUpdate(String ReturnVal, Int32 InstallMode)
        {
            if (!ReturnVal.Equals("0.0.0.0"))
            {
                if (Program.TLS12Available())
                {
                    Forms.DLEngine frm = new Forms.DLEngine(ReturnVal, String.Format("Downloading update {0}...", ReturnVal, @"{0}"), null, null, InstallMode);
                    frm.StartPosition = FormStartPosition.CenterScreen;
                    frm.ShowDialog();
                }
                else Process.Start(String.Format(UpdatePage, ReturnVal));
            }
        }

        public static void TriggerUpdateWindow(Version CurVer, Version OLVer, String newestversion, bool forced, bool startup, bool isitfromthechangelogwindow)
        {
            String UTitle = null;
            String UText = null;
            String RVal = "0.0.0.0";

            if (forced && startup) CheckForTLS12ThenUpdate(newestversion, UpdateSystem.NORMAL);
            else
            {
                if (OLVer == CurVer && forced)
                {
                    UTitle = String.Format("Reinstall version ({0})", CurVer.ToString());
                    UText = String.Format("Would you like to reinstall OmniMIDI?\nCurrent version online is {0}, the same as yours.\n\nPress Yes to confirm, or No to close the window.", CurVer.ToString());
                    RVal = CurVer.ToString();
                }
                else if (OLVer < CurVer && forced)
                {
                    UTitle = String.Format("Downgrade to version {0}", OLVer.ToString());
                    UText = String.Format("Are you sure you want to downgrade OmniMIDI?\nCurrent version online is {0}, you have {1}.\n\nPress Yes to confirm, or No to close the window.", OLVer.ToString(), CurVer.ToString());
                    RVal = OLVer.ToString();
                }
                else
                {
                    UTitle = String.Format("Update found ({0})", OLVer.ToString());
                    UText = String.Format("A new update for OmniMIDI has been found.\nCurrent version online is {0}, you have {1}.\n\nWould you like to update now?", OLVer.ToString(), CurVer.ToString());
                    RVal = OLVer.ToString();
                }

                DialogResult RES = Program.ShowError(1, UTitle, UText, null);
                if (RES == DialogResult.Yes) CheckForTLS12ThenUpdate(RVal, UpdateSystem.NORMAL);
            }
        }

        public static void NoUpdates(bool startup, bool internetok)
        {
            if (!startup)
                Program.ShowError(0, "No updates found", "No updates for the driver have been found.", null);
        }

        public static void CheckChangelog()
        {
            if (!Functions.IsWindowsVistaOrNewer()) return;

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
                    Program.ShowError(4, "Unknown error", "An error has occurred while trying to show you the latest changelog.\nPlease try again later.\n\nPress OK to continue.", ex);
                }
            }
        }

        public static string GetCurrentBranch()
        {
            if (Properties.Settings.Default.UpdateBranch == Properties.Settings.Default.PreReleaseBranch[1])
                return Properties.Settings.Default.PreReleaseBranch[0];
            else if (Properties.Settings.Default.UpdateBranch == Properties.Settings.Default.StableBranch[1])
                return Properties.Settings.Default.StableBranch[0];
            else if (Properties.Settings.Default.UpdateBranch == Properties.Settings.Default.SlowBranch[1])
                return Properties.Settings.Default.SlowBranch[0];
            else
                return "No branch selected";
        }

        public static Color GetCurrentBranchColor()
        {
            if (Properties.Settings.Default.UpdateBranch == Properties.Settings.Default.PreReleaseBranch[1])
                return Color.FromArgb(221, 172, 5);
            else if (Properties.Settings.Default.UpdateBranch == Properties.Settings.Default.StableBranch[1])
                return Color.FromArgb(158, 14, 204);
            else if (Properties.Settings.Default.UpdateBranch == Properties.Settings.Default.SlowBranch[1])
                return Color.FromArgb(84, 110, 122);
            else
                return Color.FromArgb(182, 0, 0);
        }

        public static string GetCurrentBranchToolTip()
        {
            if (Properties.Settings.Default.UpdateBranch == Properties.Settings.Default.PreReleaseBranch[1])
                return "Receive all updates.\nYou may get broken updates that haven't been fully tested.\nDesigned for testers and early adopters.";
            else if (Properties.Settings.Default.UpdateBranch == Properties.Settings.Default.StableBranch[1])
                return "Receive occasional updates and urgent bugfixes (Eg. from version x.x.1.x to x.x.2.x).\nRecommended.";
            else if (Properties.Settings.Default.UpdateBranch == Properties.Settings.Default.SlowBranch[1])
                return "You will only get major releases (Eg. from version x.1.x.x to x.2.x.x).\nFor those who do not wish to update often.\nNot recommended.";
            else
                return "No information, since you didn't chose a branch.";
        }

        public static string CheckForUpdatesMini()
        {
            if (!Functions.IsWindowsVistaOrNewer()) return "nointernet";

            bool internetok = IsInternetAvailable();
            if (internetok == false)
                return "nointernet";
            else
            {
                try
                {
                    var Releases = UpdateSystem.UpdateClient.Repository.Release.GetAll("KeppySoftware", "OmniMIDI", new Octokit.ApiOptions { PageSize = 1, PageCount = 1 }).Result;
                    FileVersionInfo Driver = FileVersionInfo.GetVersionInfo(UpdateFileVersion);

                    Version DriverOnline = null;
                    Version.TryParse(Releases[0].TagName, out DriverOnline);
                    Version DriverCurrent = null;
                    Version.TryParse(Driver.FileVersion.ToString(), out DriverCurrent);

                    if (Properties.Settings.Default.UpdateBranch == Properties.Settings.Default.PreReleaseBranch[1])
                    {
                        if (DriverCurrent < DriverOnline)
                            return "yes";
                        else
                            return "no";
                    }
                    else if (Properties.Settings.Default.UpdateBranch == Properties.Settings.Default.StableBranch[1])
                    {
                        if (DriverCurrent.Major < DriverOnline.Major || DriverCurrent.Minor < DriverOnline.Minor)
                        {
                            if ((DriverCurrent.Build >= DriverOnline.Build || DriverCurrent.Build < DriverOnline.Build))
                                return "yes";
                            else
                                return "no";
                        }
                        else
                            return "no";
                    }
                    else if (Properties.Settings.Default.UpdateBranch == Properties.Settings.Default.SlowBranch[1])
                    {
                        if (DriverCurrent.Major < DriverOnline.Major)
                        {
                            if ((DriverCurrent.Minor >= DriverOnline.Minor || DriverCurrent.Minor < DriverOnline.Minor))
                                return "yes";
                            else
                                return "no";
                        }
                        else
                            return "no";
                    }
                    else
                        return "no";
                }
                catch
                {
                    Program.ShowError(4, "Unknown error", "An error has occurred while checking for updates.\nPlease try again later.\n\nPress OK to continue.", null);
                    return "fail";
                }
            }
        }

        public static void CheckForUpdates(bool forced, bool startup, bool isitfromthechangelogwindow)
        {
            if (!Functions.IsWindowsVistaOrNewer()) return;

            bool internetok = IsInternetAvailable();
            if (internetok == false)
            {
                NoUpdates(startup, false);
            }
            else
            {
                try
                {
                    var Releases = UpdateSystem.UpdateClient.Repository.Release.GetAll("KeppySoftware", "OmniMIDI", new Octokit.ApiOptions { PageSize = 1, PageCount = 1 }).Result;
                    FileVersionInfo Driver = FileVersionInfo.GetVersionInfo(UpdateFileVersion);

                    Version DriverOnline = null;
                    Version.TryParse(Releases[0].TagName, out DriverOnline);
                    Version DriverCurrent = null;
                    Version.TryParse(Driver.FileVersion.ToString(), out DriverCurrent);

                    if (Properties.Settings.Default.UpdateBranch == Properties.Settings.Default.PreReleaseBranch[1])
                    {
                        if (DriverCurrent < DriverOnline)
                            TriggerUpdateWindow(DriverCurrent, DriverOnline, Releases[0].TagName, forced, startup, isitfromthechangelogwindow);
                        else
                        {
                            if (forced)
                                TriggerUpdateWindow(DriverCurrent, DriverOnline, Driver.FileVersion, forced, startup, isitfromthechangelogwindow);
                            else
                                NoUpdates(startup, internetok);
                        }
                    }
                    else if (Properties.Settings.Default.UpdateBranch == Properties.Settings.Default.StableBranch[1])
                    {
                        if (DriverCurrent.Major < DriverOnline.Major || DriverCurrent.Minor < DriverOnline.Minor)
                        {
                            if ((DriverCurrent.Build >= DriverOnline.Build || DriverCurrent.Build < DriverOnline.Build))
                            {
                                if (Releases[0].Prerelease)
                                {
                                    DialogResult Msg = Program.ShowError(1, 
                                        "Warning", 
                                        "This version of OmniMIDI is a pre-release, and it's not meant for people in your current branch.\n\n" +
                                        "Are you sure you want to continue with the update?", null);

                                    if (Msg == DialogResult.No)
                                        return;
                                }

                                TriggerUpdateWindow(DriverCurrent, DriverOnline, Releases[0].TagName, forced, startup, isitfromthechangelogwindow);
                            }
                            else
                            {
                                if (forced)
                                    TriggerUpdateWindow(DriverCurrent, DriverOnline, Driver.FileVersion, forced, startup, isitfromthechangelogwindow);
                                else
                                    NoUpdates(startup, internetok);
                            }
                        }
                        else
                        {
                            if (forced)
                                TriggerUpdateWindow(DriverCurrent, DriverOnline, Driver.FileVersion, forced, startup, isitfromthechangelogwindow);
                            else
                                NoUpdates(startup, internetok);
                        }
                    }
                    else if (Properties.Settings.Default.UpdateBranch == Properties.Settings.Default.SlowBranch[1])
                    {
                        if (DriverCurrent.Major < DriverOnline.Major)
                        {
                            if ((DriverCurrent.Minor >= DriverOnline.Minor || DriverCurrent.Minor < DriverOnline.Minor))
                            {
                                if (Releases[0].Prerelease)
                                {
                                    DialogResult Msg = Program.ShowError(1,
                                        "Warning",
                                        "This version of OmniMIDI is a pre-release, and it's not meant for people in your current branch.\n\n" +
                                        "Are you sure you want to continue with the update?", null);

                                    if (Msg == DialogResult.No)
                                        return;
                                }

                                TriggerUpdateWindow(DriverCurrent, DriverOnline, Releases[0].TagName, forced, startup, isitfromthechangelogwindow);
                            }
                            else
                            {
                                if (forced)
                                    TriggerUpdateWindow(DriverCurrent, DriverOnline, Driver.FileVersion, forced, startup, isitfromthechangelogwindow);
                                else
                                    NoUpdates(startup, internetok);
                            }
                        }
                        else
                        {
                            if (forced)
                                TriggerUpdateWindow(DriverCurrent, DriverOnline, Driver.FileVersion, forced, startup, isitfromthechangelogwindow);
                            else
                                NoUpdates(startup, internetok);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Program.ShowError(4, "Unknown error", "An error has occurred while checking for updates.\nPlease try again later.\n\nPress OK to continue.", ex);
                    NoUpdates(startup, internetok);
                }
            }
        }
    }
}