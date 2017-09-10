using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace KeppySynthConfigurator
{
    class TelemetryStatus
    {
        public const bool Debug = false;

        public const int USER_OK = 0;
        public const int USER_BANNED = 1;
        public const int DATA_ERROR = 2;

        public static String[] Reasons = new String[] {
            "Your user profile has been banned.",
            "Your computer has been banned.",
            "Your MAC address has been banned.",
            "Your Hardware ID has been banned."
        };
        public static int TypeOfBan = 0;
        public const int USER_BAN = 0;
        public const int PC_BAN = 1;
        public const int MAC_BAN = 2;
        public const int HWID_BAN = 3;
    }

    class TelemetryExt
    {
        static Guid GuidKey;
        static String[] Data = Properties.Resources.TelemetryLoginData.Split(new String[] { Environment.NewLine }, StringSplitOptions.None);
        static CultureInfo cultureTelemetry = new CultureInfo("en-US");
        static Random RandomID = new Random();
        static String MACAddress = (from nic in NetworkInterface.GetAllNetworkInterfaces()
                             where nic.OperationalStatus == OperationalStatus.Up
                             select nic.GetPhysicalAddress().ToString()
               ).FirstOrDefault();

        public static Int32 IsUserBanned()
        {
            try
            {
                Int32 BanStatus = TelemetryStatus.USER_OK;

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

                Byte[] hwidRAW = client.DownloadData(String.Format("ftp://{0}/{1}", Data[0], "hwid.banlist"));
                String hwid = System.Text.Encoding.UTF8.GetString(hwidRAW);

                if (BanStatus == TelemetryStatus.USER_OK)
                {
                    BanStatus = CheckPresence(userban, Environment.UserName, true); // Check userban
                    if (BanStatus == TelemetryStatus.USER_BANNED) TelemetryStatus.TypeOfBan = TelemetryStatus.USER_BAN;
                }
                if (BanStatus == TelemetryStatus.USER_OK)
                {
                    BanStatus = CheckPresence(pcban, Environment.MachineName, true); // Check PC ban
                    if (BanStatus == TelemetryStatus.USER_BANNED) TelemetryStatus.TypeOfBan = TelemetryStatus.PC_BAN;
                }
                if (BanStatus == TelemetryStatus.USER_OK)
                {
                    BanStatus = CheckPresence(macban, MACAddress, true); // Check MAC address ban
                    if (BanStatus == TelemetryStatus.USER_BANNED) TelemetryStatus.TypeOfBan = TelemetryStatus.MAC_BAN;
                }
                if (BanStatus == TelemetryStatus.USER_OK)
                {
                    BanStatus = CheckPresence(hwid, TelemetryExt.ParseHWID(), true); // Check HWID ban
                    if (BanStatus == TelemetryStatus.USER_BANNED) TelemetryStatus.TypeOfBan = TelemetryStatus.HWID_BAN;
                }

                return BanStatus;
            }
            catch { return TelemetryStatus.DATA_ERROR; }
        }

        public static Int32 CheckPresence(string datadownload, string datacomparison, bool tolower)
        {
            try
            {
                using (StringReader reader = new StringReader(datadownload))
                {
                    string line = string.Empty;
                    do
                    {
                        line = reader.ReadLine();
                        if (line != null)
                        {
                            if (tolower) if (line.ToLowerInvariant().Equals(datacomparison.ToLowerInvariant())) return TelemetryStatus.USER_BANNED;
                            else if (line.Equals(datacomparison)) return TelemetryStatus.USER_BANNED;
                        }

                    } while (line != null);
                }
                return TelemetryStatus.USER_OK;
            }
            catch { return TelemetryStatus.DATA_ERROR; }
        }

        public static bool SendInfoForTelemetry(Byte[] data, String dataraw, Boolean githubissue)
        {
            try
            {
                if (githubissue) { /* Todo */ }
                else
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
                }

                return true;
            }
            catch (Exception ex)
            {
                Program.DebugToConsole(true, null, ex);
                Functions.ShowErrorDialog(2, System.Media.SystemSounds.Exclamation, "Error", "Oh snap!\nThe configurator encountered an error while sending the telemetry info!", true, ex);
                return false;
            }
        }

        // Starting code by DeadLine: http://forum.codecall.net/topic/78149-c-tutorial-generating-a-unique-hardware-id/

        //Return a hardware identifier
        private static string Identifier(string wmiClass, string wmiProperty, string wmiMustBeTrue)
        {
            string result = "";
            System.Management.ManagementClass mc = new System.Management.ManagementClass(wmiClass);
            System.Management.ManagementObjectCollection moc = mc.GetInstances();
            foreach (System.Management.ManagementBaseObject mo in moc)
            {
                if (mo[wmiMustBeTrue].ToString() != "True") continue;
                //Only get the first one
                if (result != "") continue;
                try
                {
                    result = mo[wmiProperty].ToString();
                    break;
                }
                catch
                {
                }
            }
            return result;
        }

        private static string Identifier(string wmiClass, string wmiProperty)
        {
            string result = "";
            System.Management.ManagementClass mc = new System.Management.ManagementClass(wmiClass);
            System.Management.ManagementObjectCollection moc = mc.GetInstances();
            foreach (System.Management.ManagementBaseObject mo in moc)
            {
                //Only get the first one
                if (result != "") continue;
                try
                {
                    result = mo[wmiProperty].ToString();
                    break;
                }
                catch
                {
                }
            }
            return result;
        }

        public static string _fingerPrint = string.Empty;
        public static string ParseHWID()
        {
            //You don't need to generate the HWID again if it has already been generated. This is better for performance
            //Also, your HWID generally doesn't change when your computer is turned on but it can happen.
            //It's up to you if you want to keep generating a HWID or not if the function is called.
            if (string.IsNullOrEmpty(_fingerPrint))
            {
                _fingerPrint = GetHash(
                    "CPU >> " + CPUID() + 
                    "\nBIOS >> " + BIOSID() + 
                    "\nBASE >> " + BaseID() + 
                    "\nDISK >> " + DiskID() + 
                    "\nVIDEO >> " + VideoID() + 
                    "\nMAC >> " + MACID());
            }
            return _fingerPrint;
        }

        private static string GetHash(string s)
        {
            MD5 sec = new MD5CryptoServiceProvider();
            byte[] bt = Encoding.ASCII.GetBytes(s);
            return GetHexString(sec.ComputeHash(bt));
        }

        private static string GetHexString(IList<byte> bt)
        {
            string s = string.Empty;
            for (int i = 0; i < bt.Count; i++)
            {
                byte b = bt[i];
                int n = b;
                int n1 = n & 15;
                int n2 = (n >> 4) & 15;
                if (n2 > 9)
                    s += ((char)(n2 - 10 + 'A')).ToString(CultureInfo.InvariantCulture);
                else
                    s += n2.ToString(CultureInfo.InvariantCulture);
                if (n1 > 9)
                    s += ((char)(n1 - 10 + 'A')).ToString(CultureInfo.InvariantCulture);
                else
                    s += n1.ToString(CultureInfo.InvariantCulture);
                if ((i + 1) != bt.Count && (i + 1) % 2 == 0) s += "-";
            }
            return s;
        }

        private static string CPUID()
        {
            string retVal = Identifier("Win32_Processor", "UniqueId");
            if (retVal != "") return retVal;

            retVal = Identifier("Win32_Processor", "ProcessorId");
            if (retVal != "") return retVal;

            retVal = Identifier("Win32_Processor", "Name");
            if (retVal == "") retVal = Identifier("Win32_Processor", "Manufacturer");

            retVal += Identifier("Win32_Processor", "MaxClockSpeed");
            return retVal;
        }

        private static string BIOSID()
        {
            return Identifier("Win32_BIOS", "Manufacturer") + Identifier("Win32_BIOS", "SMBIOSBIOSVersion") + Identifier("Win32_BIOS", "IdentificationCode") + Identifier("Win32_BIOS", "SerialNumber") + Identifier("Win32_BIOS", "ReleaseDate") + Identifier("Win32_BIOS", "Version");
        }

        private static string DiskID()
        {
            return Identifier("Win32_DiskDrive", "Model") + Identifier("Win32_DiskDrive", "Manufacturer") + Identifier("Win32_DiskDrive", "Signature") + Identifier("Win32_DiskDrive", "TotalHeads");
        }

        private static string BaseID()
        {
            return Identifier("Win32_BaseBoard", "Model") + Identifier("Win32_BaseBoard", "Manufacturer") + Identifier("Win32_BaseBoard", "Name") + Identifier("Win32_BaseBoard", "SerialNumber");
        }

        private static string VideoID()
        {
            return Identifier("Win32_VideoController", "DriverVersion") + Identifier("Win32_VideoController", "Name");
        }

        private static string MACID()
        {
            return Identifier("Win32_NetworkAdapterConfiguration", "MACAddress", "IPEnabled");
        }

        // Thank you so much DeadLine for making this public! Take a look at his code here: http://forum.codecall.net/topic/78149-c-tutorial-generating-a-unique-hardware-id/

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
