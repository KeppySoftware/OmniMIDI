using System;
using System.Runtime.InteropServices;

namespace KeppySynthDebugWindow
{
    public class OSInfo
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct OSVERSIONINFOEX
        {
            public int dwOSVersionInfoSize;
            public int dwMajorVersion;
            public int dwMinorVersion;
            public int dwBuildNumber;
            public int dwPlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szCSDVersion;
            public short wServicePackMajor;
            public short wServicePackMinor;
            public short wSuiteMask;
            public byte wProductType;
            public byte wReserved;
        }

        public static string[] Chassis = new string[] {
             null,
             "Other",
             "Unknown",
             "Desktop",
             "Low Profile Desktop",
             "Pizza Box",
             "Mini Tower",
             "Tower",
             "Portable",
             "Laptop",
             "Notebook",
             "Hand Held",
             "Docking Station",
             "All in One",
             "Sub Notebook",
             "Space-Saving",
             "Lunch Box",
             "Main System Chassis",
             "Expansion Chassis",
             "Sub-Chassis",
             "Bus Expansion Chassis",
             "Peripheral Chassis",
             "Storage Chassis",
             "Rack Mount Chassis",
             "Sealed-Case PC",
        };

        [DllImport("kernel32.dll")]
        private static extern bool GetVersionEx(ref OSVERSIONINFOEX osVersionInfo);

        #region Private Constants
        private const int VER_NT_WORKSTATION = 1;
        private const int VER_NT_DOMAIN_CONTROLLER = 2;
        private const int VER_NT_SERVER = 3;
        private const int VER_SUITE_SMALLBUSINESS = 1;
        private const int VER_SUITE_ENTERPRISE = 2;
        private const int VER_SUITE_TERMINAL = 16;
        private const int VER_SUITE_DATACENTER = 128;
        private const int VER_SUITE_SINGLEUSERTS = 256;
        private const int VER_SUITE_PERSONAL = 512;
        private const int VER_SUITE_BLADE = 1024;
        #endregion

        #region Public Methods
        /// <summary>
        /// Returns the product type of the operating system running on this computer.
        /// </summary>
        /// <returns>A string containing the the operating system product type.</returns>
        public static string GetOSProductType()
        {
            OSVERSIONINFOEX osVersionInfo = new OSVERSIONINFOEX();
            OperatingSystem osInfo = Environment.OSVersion;

            osVersionInfo.dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX));


            if (!GetVersionEx(ref osVersionInfo))
            {
                return "";
            }
            else
            {
                if (osVersionInfo.wProductType == VER_NT_WORKSTATION)
                {
                    if ((osVersionInfo.wSuiteMask & VER_SUITE_PERSONAL) == VER_SUITE_PERSONAL)
                    {
                        if (osInfo.Version.Major == 10 || (osInfo.Version.Major == 6 && osInfo.Version.Minor >= 2))
                            return " Home";
                        else
                            return " Home Edition";
                    }
                    else
                    {
                        if (osInfo.Version.Major == 10 || (osInfo.Version.Major == 6 && osInfo.Version.Minor >= 2))
                            return " Pro";
                        else
                            return " Professional";
                    }
                }
                else if (osVersionInfo.wProductType == VER_NT_SERVER)
                {
                    if (osInfo.Version.Major == 10 || (osInfo.Version.Major == 6 && osInfo.Version.Minor >= 2))
                    {
                        if ((osVersionInfo.wSuiteMask & VER_SUITE_DATACENTER) == VER_SUITE_DATACENTER)
                            return " Datacenter";
                        else if ((osVersionInfo.wSuiteMask & VER_SUITE_ENTERPRISE) == VER_SUITE_ENTERPRISE)
                            return " Enterprise";
                        else
                            return " Standard";
                    }
                    else
                    {
                        if ((osVersionInfo.wSuiteMask & VER_SUITE_DATACENTER) == VER_SUITE_DATACENTER)
                            return " Datacenter Edition";
                        else if ((osVersionInfo.wSuiteMask & VER_SUITE_ENTERPRISE) == VER_SUITE_ENTERPRISE)
                            return " Enterprise Edition";
                        else if ((osVersionInfo.wSuiteMask & VER_SUITE_BLADE) == VER_SUITE_BLADE)
                            return " Web Edition";
                        else
                            return " Standard Edition";
                    }
                }
            }

            return "";
        }

        /// <summary>
        /// Returns the service pack information of the operating system running on this computer.
        /// </summary>
        /// <returns>A string containing the the operating system service pack information.</returns>
        public static string GetOSServicePack()
        {
            OSVERSIONINFOEX osVersionInfo = new OSVERSIONINFOEX();

            osVersionInfo.dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX));

            if (!GetVersionEx(ref osVersionInfo))
            {
                return "";
            }
            else
            {
                return " " + osVersionInfo.szCSDVersion;
            }
        }

        /// <summary>
        /// Returns the name of the operating system running on this computer.
        /// </summary>
        /// <returns>A string containing the the operating system name.</returns>
        public static string GetOSName()
        {
            OSVERSIONINFOEX osVersionInfo = new OSVERSIONINFOEX();
            OperatingSystem osInfo = Environment.OSVersion;
            string osName = "UNKNOWN";

            switch (osInfo.Platform)
            {
                case PlatformID.Win32NT:
                    {
                        switch (osInfo.Version.Major)
                        {
                            case 6:
                                if (osInfo.Version.Minor == 0)
                                {
                                    if (osVersionInfo.wProductType == VER_NT_SERVER)
                                        osName = "Windows Server 2008";
                                    else
                                        osName = "Windows Vista";
                                }
                                else if (osInfo.Version.Minor == 1)
                                {
                                    if (osVersionInfo.wProductType == VER_NT_SERVER)
                                        osName = "Windows Server 2008 R2";
                                    else
                                        osName = "Windows 7";
                                }
                                else if (osInfo.Version.Minor == 2)
                                {
                                    if (osVersionInfo.wProductType == VER_NT_SERVER)
                                        osName = "Windows Server 2012";
                                    else
                                        osName = "Windows 8";
                                }
                                else if (osInfo.Version.Minor == 3)
                                {
                                    if (osVersionInfo.wProductType == VER_NT_SERVER)
                                        osName = "Windows Server 2012 R2";
                                    else
                                        osName = "Windows 8.1";
                                }
                                break;

                            case 10:
                                if (osVersionInfo.wProductType == VER_NT_SERVER)
                                    osName = "Windows Server 2016";
                                else
                                    osName = "Windows 10";
                                break;
                        }
                        break;
                    }
            }

            return osName;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the full version of the operating system running on this computer.
        /// </summary>
        public static string OSVersion
        {
            get
            {
                return Environment.OSVersion.Version.ToString();
            }
        }

        /// <summary>
        /// Gets the major version of the operating system running on this computer.
        /// </summary>
        public static int OSMajorVersion
        {
            get
            {
                return Environment.OSVersion.Version.Major;
            }
        }

        /// <summary>
        /// Gets the minor version of the operating system running on this computer.
        /// </summary>
        public static int OSMinorVersion
        {
            get
            {
                return Environment.OSVersion.Version.Minor;
            }
        }

        /// <summary>
        /// Gets the build version of the operating system running on this computer.
        /// </summary>
        public static int OSBuildVersion
        {
            get
            {
                return Environment.OSVersion.Version.Build;
            }
        }

        /// <summary>
        /// Gets the revision version of the operating system running on this computer.
        /// </summary>
        public static int OSRevisionVersion
        {
            get
            {
                return Environment.OSVersion.Version.Revision;
            }
        }
        #endregion
    }
}
