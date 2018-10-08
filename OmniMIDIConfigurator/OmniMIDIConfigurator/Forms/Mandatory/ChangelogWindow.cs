using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HtmlAgilityPack;
using System.Net;
using System.Diagnostics;
using System.IO;

namespace OmniMIDIConfigurator
{
    public partial class ChangelogWindow : Form
    {
        Boolean SwitchingChangelog = false;

        public ChangelogWindow(String version, Boolean IsItFromTheUpdateWindow)
        {
            InitializeComponent();
            UpdateBtn.Visible = !IsItFromTheUpdateWindow;

            var OnlyFirstTenReleases = new Octokit.ApiOptions
            {
                PageSize = 11,
                PageCount = 1
            };

            try
            {
                ReleasesList.SelectedIndexChanged -= ReleasesList_SelectedIndexChanged;

                IReadOnlyList<Octokit.Release> Releases = UpdateSystem.UpdateClient.Repository.Release.GetAll("KeppySoftware", "OmniMIDI", OnlyFirstTenReleases).Result;
                foreach (Octokit.Release OneRel in Releases)
                {
                    Version x = null;
                    Version.TryParse(OneRel.TagName, out x);
                    ReleasesList.Items.Add(x.ToString());
                }

                ReleasesList.SelectedIndex = 0;
                ReleasesList.SelectedIndexChanged += ReleasesList_SelectedIndexChanged;

                ReleasesList_SelectedIndexChanged(null, null);
            }
            catch
            {
                label1.Visible = false;
                ReleasesList.Visible = false;
                GetChangelog(version);
            }
        }

        private void ChangelogWindow_Load(object sender, EventArgs e)
        {
            // NULL
        }

        public bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private HtmlAgilityPack.HtmlNode ReturnSelectedNode(String version)
        {
            try
            {
                HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
                web.PreRequest += request =>
                {
                    request.CookieContainer = new System.Net.CookieContainer();
                    return true;
                };
                HtmlAgilityPack.HtmlDocument doc = web.Load(String.Format("https://github.com/KeppySoftware/OmniMIDI/releases/tag/{0}", version));
                return doc.DocumentNode.SelectSingleNode("//div[@class='markdown-body']");
            }
            catch (WebException ex)
            {
                Functions.ShowErrorDialog(ErrorType.Error, System.Media.SystemSounds.Hand, "Error", "Failed to parse the changelog!\n\nThe HTML node for the changelog is invalid.", true, ex);
                Close();
                return null;
            }
        }

        private void GetChangelog(String version)
        {
            SwitchingChangelog = true;

            try
            {
                Version y = null;
                Version.TryParse(version, out y);

                Text = String.Format("OmniMIDI - Changelog for {0}", version);

                VersionLabel.Text = String.Format("Changelog for version {0}", version);
                HtmlNode rateNode = ReturnSelectedNode(version);

                ChangelogBrowser.DocumentText = "<html><font face=\"Tahoma\">" + rateNode.InnerHtml + "</font></body></html>";
            }
            catch
            {
                Functions.ShowErrorDialog(ErrorType.Error, System.Media.SystemSounds.Hand, "Error", "An error has occurred while parsing the changelog from GitHub.", false, null);
                Close();
            }
        }

        private void ChangelogBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            SwitchingChangelog = false;
        }

        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            UpdateSystem.CheckForUpdates((ModifierKeys == Keys.Shift), false, true);
        }

        private void ReleasesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetChangelog(ReleasesList.SelectedItem.ToString());
        }
    }
}
