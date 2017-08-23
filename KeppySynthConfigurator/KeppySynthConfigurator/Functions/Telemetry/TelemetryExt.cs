using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;

namespace KeppySynthConfigurator
{
    class TelemetryExt
    {
        static String[] Data = Properties.Resources.TelemetryLoginData.Split(new String[] { Environment.NewLine }, StringSplitOptions.None);
        static CultureInfo cultureTelemetry = new CultureInfo("en-US");
        static Random RandomID = new Random();
        static String MACAddress = (from nic in NetworkInterface.GetAllNetworkInterfaces()
                             where nic.OperationalStatus == OperationalStatus.Up
                             select nic.GetPhysicalAddress().ToString()
               ).FirstOrDefault();

        public static bool IsUserBanned()
        {
            try
            {
                Boolean BanStatus = false;

                WebClient client = new WebClient();
                Uri duri = new Uri(String.Format("ftp://{0}", Data[0]));
                FtpWebRequest DirectoryFTP = (FtpWebRequest)FtpWebRequest.Create(duri);
                client.Credentials = new NetworkCredential(Data[1], Data[2]);

                Byte[] userbanRAW = client.DownloadData(String.Format("ftp://{0}/{1}", Data[0], "users.banlist"));
                String userban = System.Text.Encoding.UTF8.GetString(userbanRAW);

                Byte[] pcbanRAW = client.DownloadData(String.Format("ftp://{0}/{1}", Data[0], "pcs.banlist"));
                String pcban = System.Text.Encoding.UTF8.GetString(pcbanRAW);

                Byte[] macbanRAW = client.DownloadData(String.Format("ftp://{0}/{1}", Data[0], "macaddresses.banlist"));
                String macban = System.Text.Encoding.UTF8.GetString(macbanRAW);

                if (!BanStatus) BanStatus = CheckPresence(userban, Environment.UserName); // Check userban
                if (!BanStatus) BanStatus = CheckPresence(pcban, Environment.MachineName); // Check PC ban
                if (!BanStatus) BanStatus = CheckPresence(macban, MACAddress); // Check MAC address ban

                return BanStatus;
            }
            catch { return true; }
        }

        public static bool CheckPresence(string datadownload, string datacomparison)
        {
            try
            {
                using (StringReader reader = new StringReader(datadownload))
                {
                    string line = string.Empty;
                    do
                    {
                        line = reader.ReadLine();
                        if (line != null) if (line.ToLowerInvariant().Equals(datacomparison.ToLowerInvariant())) return true;

                    } while (line != null);
                }
                return false;
            }
            catch { return true; }
        }

        public static bool SendInfoForTelemetry(byte[] data)
        {
            try
            {
                try
                {
                    // Create folder
                    Uri duri = new Uri(String.Format("ftp://{0}/{1}", Data[0], Environment.UserName));
                    FtpWebRequest DirectoryFTP = (FtpWebRequest)FtpWebRequest.Create(duri);
                    DirectoryFTP.Credentials = new NetworkCredential(Data[1], Data[2]);
                    DirectoryFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                    FtpWebResponse DirectoryResponse = (FtpWebResponse)DirectoryFTP.GetResponse();
                    DirectoryResponse.Close();
                }
                catch { }

                // Upload data
                Uri furi = new Uri(String.Format("ftp://{0}/{1}/{2}", Data[0], Environment.UserName, String.Format("{0}_{1}_{2}_({3}).txt",
                    Environment.UserName, DateTime.Now.ToString("dd MMMM yyyy", cultureTelemetry), DateTime.Now.ToLongTimeString(), RandomID.Next(0, 2147483647).ToString("0000000000"))));
                FtpWebRequest FileFTP = (FtpWebRequest)FtpWebRequest.Create(furi);
                FileFTP.Credentials = new NetworkCredential(Data[1], Data[2]);
                FileFTP.Method = WebRequestMethods.Ftp.UploadFile;
                FileFTP.KeepAlive = false;
                FileFTP.UsePassive = true;

                using (Stream stOut = FileFTP.GetRequestStream()) stOut.Write(data, 0, data.Length);

                // Close data connection
                FtpWebResponse FileResponse = (FtpWebResponse)FileFTP.GetResponse();
                FileResponse.Close();

                return true;
            }
            catch (Exception ex)
            {
                Program.DebugToConsole(true, null, ex);
                Functions.ShowErrorDialog(2, System.Media.SystemSounds.Exclamation, "Error", "Oh snap!\nThe configurator encountered an error while sending the telemetry info!", true, ex);
                return false;
            }
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
