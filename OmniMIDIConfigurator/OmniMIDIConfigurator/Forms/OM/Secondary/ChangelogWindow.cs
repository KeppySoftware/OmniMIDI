using System;
using System.Collections.Generic;
using System.Windows.Forms;
using HtmlAgilityPack;
using System.Net;

namespace OmniMIDIConfigurator
{
    public partial class ChangelogWindow : Form
    {
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

                HtmlAgilityPack.HtmlDocument doc = web.Load(String.Format("{0}/releases/tag/{1}", Properties.Settings.Default.ProjectLink, version));
                return doc.DocumentNode.SelectSingleNode("//div[@class='markdown-body']");
            }
            catch (WebException ex)
            {
                Program.ShowError(4, "Error", "Failed to parse the changelog, the HTML node for the changelog is invalid!\n\nYou might not be connected to the Internet, or GitHub might have rate-limited your IP address temporarily.", ex);
                return null;
            }
        }

        private void GetChangelog(String version)
        {
            ChangelogBrowser.AllowNavigation = true;

            try
            {
                Version y = null;
                Version.TryParse(version, out y);

                Text = String.Format("OmniMIDI - Changelog for {0}", version);

                VersionLabel.Text = String.Format("Changelog for version {0}", version);
                HtmlNode rateNode = ReturnSelectedNode(version);

                if (rateNode != null)
                    ChangelogBrowser.DocumentText = "<html><font face=\"Tahoma\">" + rateNode.InnerHtml + "</font></body></html>";
                else
                    ChangelogBrowser.DocumentText = "<html><font face=\"Tahoma\">No Internet connection available, or you're being rate limited by GitHub.</font></body></html>";
            }
            catch
            {
                ChangelogBrowser.AllowNavigation = false;
                Program.ShowError(4, "Error", "An error has occurred while parsing the changelog from GitHub.", null);
            }
        }

        private void ChangelogBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            ChangelogBrowser.AllowNavigation = false;
        }

        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            UpdateSystem.CheckForUpdates((ModifierKeys == Keys.Shift), false, true);
        }

        private void ReleasesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangelogBrowser.AllowNavigation = true;
            GetChangelog(ReleasesList.SelectedItem.ToString());
        }
    }
}
