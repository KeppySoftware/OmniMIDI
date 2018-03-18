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

namespace KeppySynthConfigurator
{
    public partial class ChangelogWindow : Form
    {
        public ChangelogWindow(String version, Boolean IsItFromTheUpdateWindow)
        {
            InitializeComponent();
            UpdateBtn.Visible = !IsItFromTheUpdateWindow;
            GetChangelog(version);
        }

        private void ChangelogWindow_Load(object sender, EventArgs e)
        {
            // Null
        }

        public bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private HtmlAgilityPack.HtmlNode ReturnSelectedNode(String version)
        {
            ChangelogBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(DocumentCompleted);
            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            web.PreRequest += request =>
            {
                request.CookieContainer = new System.Net.CookieContainer();
                return true;
            };
            HtmlAgilityPack.HtmlDocument doc = web.Load(String.Format("https://github.com/KaleidonKep99/Keppy-s-Synthesizer/releases/tag/{0}", version));
            return doc.DocumentNode.SelectSingleNode("//div[@class='markdown-body']");
        }

        private void GetChangelog(String version)
        {
            try
            {
                WebClient client = new WebClient();
                Stream stream = client.OpenRead(UpdateSystem.UpdateTextFile);
                StreamReader reader = new StreamReader(stream);
                String newestversion = reader.ReadToEnd();
                Version x = null;
                Version.TryParse(newestversion.ToString(), out x);
                Version y = null;
                Version.TryParse(version, out y);

                Text = String.Format("Keppy's Synthesizer - Changelog for {0}", version);
                String htmltext = "";

                if (x < y)
                {
                    HtmlAgilityPack.HtmlNode rateNode = ReturnSelectedNode(x.ToString());
                    VersionLabel.Text = String.Format("Changelog for version {0} (Preview)", version);

                    htmltext = String.Format(
                        "<html><font face=\"Microsoft Sans Serif\">You're using an unreleased version of the driver. Here's the changelog from version {0}:<br/><hr/><br/>",
                        x.ToString());

                    htmltext += rateNode.InnerHtml;

                    htmltext += "</font></html>";
                }
                else
                {
                    VersionLabel.Text = String.Format("Changelog for version {0}", version);
                    HtmlAgilityPack.HtmlNode rateNode = ReturnSelectedNode(version);

                    htmltext = "<html><font face=\"Microsoft Sans Serif\">" + rateNode.InnerHtml;

                    if (x > y)
                        htmltext += String.Format("<br/><br/>Latest version available: <a href=\"https://github.com/KaleidonKep99/Keppy-s-Synthesizer/releases/tag/{0}\">{0}</a href>", x.ToString());

                    htmltext += "</font></html>";

                }

                ChangelogBrowser.DocumentText = htmltext;
            }
            catch (WebException ex)
            {
                Functions.ShowErrorDialog(1, System.Media.SystemSounds.Hand, "Error", "An error has occurred while parsing the changelog from GitHub.", true, ex);
                Close();
            }
        }

        private void DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var linkElements = ChangelogBrowser.Document.GetElementsByTagName("a");
            foreach (HtmlElement link in linkElements)
            {
                link.Click += (s, args) =>
                {
                    Process.Start(link.GetAttribute("href"));
                };
            }
        }

        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                UpdateSystem.CheckForUpdates(true, false, true);
            }
            else
            {
                UpdateSystem.CheckForUpdates(false, false, true);
            }
        }
    }
}
