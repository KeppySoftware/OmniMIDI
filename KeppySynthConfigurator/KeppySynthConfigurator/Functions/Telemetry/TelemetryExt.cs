using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public static Random RandomID = new Random();

        public static void SendInfoForTelemetry(byte[] data)
        {
            String[] Data = Properties.Resources.TelemetryLoginData.Split(new String[] { Environment.NewLine }, StringSplitOptions.None);

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
                Environment.UserName, DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), RandomID.Next(0, 2147483647).ToString("0000000000"))));
            FtpWebRequest FileFTP = (FtpWebRequest)FtpWebRequest.Create(furi);
            FileFTP.Credentials = new NetworkCredential(Data[1], Data[2]);
            FileFTP.Method = WebRequestMethods.Ftp.UploadFile;
            FileFTP.KeepAlive = false;
            FileFTP.UsePassive = true;

            using (Stream stOut = FileFTP.GetRequestStream()) stOut.Write(data, 0, data.Length);

            // Close data connection
            FtpWebResponse FileResponse = (FtpWebResponse)FileFTP.GetResponse();
            FileResponse.Close();
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
