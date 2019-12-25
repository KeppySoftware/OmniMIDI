using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Un4seen.Bass;

namespace OmniMIDIConfigurator
{
    class PA
    {
        [DllImport("kernel32.dll")]
        public static extern void GetNativeSystemInfo(ref SYSTEM_INFO lpSystemInfo);

        public const short PROCESSOR_ARCHITECTURE_UNKNOWN = 0xFF;
        public const short PROCESSOR_ARCHITECTURE_ARM64 = 12;
        public const short PROCESSOR_ARCHITECTURE_AMD64 = 9;
        public const short PROCESSOR_ARCHITECTURE_IA64 = 6;
        public const short PROCESSOR_ARCHITECTURE_INTEL = 0;

        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEM_INFO
        {
            public short wProcessorArchitecture;
            public short wReserved;
            public int dwPageSize;
            public IntPtr lpMinimumApplicationAddress;
            public IntPtr lpMaximumApplicationAddress;
            public IntPtr dwActiveProcessorMask;
            public int dwNumberOfProcessors;
            public int dwProcessorType;
            public int dwAllocationGranularity;
            public short wProcessorLevel;
            public short wProcessorRevision;
        }
    }

    class Functions
    {   
        // Disable WoW64 directory redirection
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);

        // Enable WoW64 directory redirection
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool Wow64RevertWow64FsRedirection(IntPtr ptr);

        // Notify for file association updates
        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

        // Get size of memory
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);

        public static RegistryKey D32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
        public static RegistryKey D64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
        public static RegistryKey CLSID32 = D32.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\Drivers32", false);
        public static RegistryKey CLSID64 = D64.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\Drivers32", false);
        public static class LiveChanges
        {
            public static int PreviousEngine = 0;
            public static int PreviousFrequency = 0;
            public static int PreviousBuffer = 0;
            public static int MonophonicRender = 0;
            public static int AudioBitDepth = 0;
        }

        public struct SettingStruct
        {
            public string SettingName;
            public int Value;

            public SettingStruct(string S, int V)
            {
                this.SettingName = S;
                this.Value = V;
            }
        }

        public static readonly List<SettingStruct> DefaultSettings = new List<SettingStruct>
        {
            new SettingStruct ("AudioBitDepth", 1),
            new SettingStruct ("AudioFrequency", 48000),
            new SettingStruct ("BufferLength", 0),
            new SettingStruct ("CapFramerate", 0),
            new SettingStruct ("Chorus", 0),
            new SettingStruct ("CurrentEngine", 3),
            new SettingStruct ("DebugMode", 0),
            new SettingStruct ("DefaultSFList", 0),
            new SettingStruct ("DelayNoteOffValue", 5),
            new SettingStruct ("DisableChime", 0),
            new SettingStruct ("DontMissNotes", 0),
            new SettingStruct ("EnableSFX", 1),
            new SettingStruct ("EvBufferMultRatio", 1),
            new SettingStruct ("EvBufferSize", 4096),
            new SettingStruct ("Extra8Lists", 0),
            new SettingStruct ("FastHotkeys", 1),
            new SettingStruct ("FollowDefaultAudioDevice", 0),
            new SettingStruct ("GetEvBuffSizeFromRAM", 0),
            new SettingStruct ("HyperPlayback", 0),
            new SettingStruct ("IgnoreNotesBetweenVel", 0),
            new SettingStruct ("IgnoreSysEx", 0),
            new SettingStruct ("IgnoreSysReset", 0),
            new SettingStruct ("KDMAPIEnabled", 1),
            new SettingStruct ("KeepAlive", 0),
            new SettingStruct ("LiveChanges", 0),
            new SettingStruct ("MaxRenderingTime", 75),
            new SettingStruct ("MaxVoices", 1000),
            new SettingStruct ("MonoRendering", 0),
            new SettingStruct ("NoteLengthValue", 5),
            new SettingStruct ("NoteOff1", 0),
            new SettingStruct ("NotesCatcherWithAudio", 0),
            new SettingStruct ("OutputVolume", 10000),
            new SettingStruct ("PID", 0xA),
            new SettingStruct ("PreloadSoundFonts", 1),
            new SettingStruct ("RCOverride", 0),
            new SettingStruct ("Reverb", 0),
            new SettingStruct ("SincConv", 0),
            new SettingStruct ("SincInter", 0),
            new SettingStruct ("StockWinMM", 0),
            new SettingStruct ("SynthType", 2),
            new SettingStruct ("VID", 0xFFFF),
            new SettingStruct ("VolumeBoost", 0),
            new SettingStruct ("VolumeMonitor", 0)
        };

        public static string GetProcessorArchitecture()
        {
            PA.SYSTEM_INFO si = new PA.SYSTEM_INFO();
            PA.GetNativeSystemInfo(ref si);
            switch (si.wProcessorArchitecture)
            {
                case PA.PROCESSOR_ARCHITECTURE_AMD64:
                    return "AMD64";

                case PA.PROCESSOR_ARCHITECTURE_IA64:
                    return "IA64";

                case PA.PROCESSOR_ARCHITECTURE_INTEL:
                    return "i386";

                case PA.PROCESSOR_ARCHITECTURE_ARM64:
                    return "AArch64";

                default:
                    return "NA";
            }
        }

        public static void SettingsRegEditor(bool Export)
        {
            OpenFileDialog OFD = new OpenFileDialog()
            {
                Multiselect = false,
                InitialDirectory = Properties.Settings.Default.LastImportExportPath,
                Filter = "Registry file | *.reg"
            };

            SaveFileDialog SFD = new SaveFileDialog()
            {
                OverwritePrompt = true,
                InitialDirectory = Properties.Settings.Default.LastImportExportPath,
                Filter = "Registry file | *.reg"
            };

            DialogResult RES = Export ? SFD.ShowDialog() : OFD.ShowDialog();

            if (RES == DialogResult.OK)
            {
                try
                {
                    Properties.Settings.Default.LastImportExportPath = Path.GetDirectoryName(Export ? SFD.FileName : OFD.FileName);
                    Properties.Settings.Default.Save();

                    using (Process E = new Process())
                    {
                        E.StartInfo.FileName = "reg.exe";
                        E.StartInfo.UseShellExecute = false;
                        E.StartInfo.RedirectStandardOutput = true;
                        E.StartInfo.RedirectStandardError = true;
                        E.StartInfo.CreateNoWindow = true;
                        E.StartInfo.Arguments = 
                            String.Format(
                                "{0} {1}\"{2}\" {3}", 
                                Export ? "export" : "import",
                                Export ? String.Format("\"{0}{1}\" ", Program.HKCU, Program.SSPath) : String.Empty,                              
                                Export ? SFD.FileName : OFD.FileName,
                                Export ? "/y" : String.Empty);

                        E.Start();
                        E.WaitForExit();
                    }
                }
                catch
                {
                    Program.ShowError(
                        4,
                        String.Format("Error while {0} settings", Export ? "exporting" : "importing"),
                        "An error has occurred during the execution of the task.\n\nPress OK to continue.",
                        null);
                }
            }

            OFD.Dispose();
            SFD.Dispose();
        }

        public static bool IsWindowsVistaOrNewer()
        {
            return (Environment.OSVersion.Version.Major >= 6);
        }

        public static bool IsWindows8OrLater()
        {
            return (Environment.OSVersion.Version.Major > 6 || (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor > 1));
        }

        public static int Between0And127(int integer)
        {
            if (integer < 0)
                return 0;
            else if (integer > 127)
                return 127;
            else
                return integer;
        }

        public static void DeleteDirectory(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(target_dir, false);
        }

        public static void ResetDriverSettings()
        {
            Properties.Settings.Default.AutoLoadList = false;
            Properties.Settings.Default.LiveChanges = false;
            Properties.Settings.Default.Save();

            foreach (SettingStruct Setting in DefaultSettings.ToArray())
            {
                Program.SynthSettings.SetValue(Setting.SettingName, Setting.Value, RegistryValueKind.DWord);
            }
        }

        public static bool CheckDriverStatusInReg(String WhichBit, RegistryKey WhichKey)
        {
            bool Registered = false;
            for (int i = 0; i < 32; i++)
            {
                String iS = (i == 0) ? "" : i.ToString();

                try
                {
                    if (WhichKey.GetValue(String.Format("midi{0}", iS), "wdmaud.drv").ToString() == "OmniMIDI\\OmniMIDI.dll")
                    {
                        Registered = true;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format("No MIDI driver values available.\n\nError:\n{0}", ex.ToString()), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return false;
                }
            }

            if (!Registered)
            {
                DialogResult RES = MessageBox.Show(
                    String.Format("It looks like something unregistered the {0} version of the driver from the registry.\n\nPress Yes if you want the configurator to fix the issue.", WhichBit),
                    String.Format("OmniMIDI ~ {0} driver not registered", WhichBit),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (RES == DialogResult.Yes) Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\OmniMIDI\\OmniMIDIDriverRegister.exe", "/register");
            }

            return Registered;
        }

        public static string ReturnLength(long length, bool apx)
        {
            try
            {
                if (length >= 1099511627776)
                {
                    if (length >= 1099511627776 && length < 10995116277760)
                        return ((((length / 1024f) / 1024f) / 1024f) / 1024f).ToString(apx ? "0.0 TB~" : "0.00 TB");
                    else
                        return ((((length / 1024f) / 1024f) / 1024f) / 1024f).ToString(apx ? "0 TB~" : "0.0 TB");
                }
                else if (length >= 1073741824)
                {
                    if (length >= 1073741824 && length < 10737418240)
                        return (((length / 1024f) / 1024f) / 1024f).ToString(apx ? "0.0 GB~" : "0.00 GB");
                    else
                        return (((length / 1024f) / 1024f) / 1024f).ToString(apx ? "0 GB~" : "0.0 GB");
                }
                else if (length >= 1048576)
                {
                    if (length >= 1048576 && length < 10485760)
                        return ((length / 1024f) / 1024f).ToString(apx ? "0.0 MB~" : "0.00 MB");
                    else
                        return ((length / 1024f) / 1024f).ToString(apx ? "0 MB~" : "0.0 MB");
                }
                else if (length >= 1024)
                {
                    if (length >= 1024 && length < 10240)
                        return (length / 1024f).ToString(apx ? "0.0 kB~" : "0.00 kB");
                    else
                        return (length / 1024f).ToString(apx ? "0 kB~" : "0.0 kB");
                }
                else
                {
                    if (length >= 1 && length < 1024)
                        return (length).ToString(apx ? "0.0 B~" : "0.00 B");
                    else
                        return (length / 1024f).ToString(apx ? "0 B~" : "0.0 B");
                }
            }
            catch { return "-"; }
        }

        public static void OpenAdvancedAudioSettings(String TabToOpen, String ErrorNoWork)
        {
            try
            {
                BASS_DEVICEINFO info = new BASS_DEVICEINFO();
                String DeviceID = "0";
                Bass.BASS_GetDeviceInfo(0, info);
                for (int n = 0; Bass.BASS_GetDeviceInfo(n, info); n++)
                {
                    if (info.IsDefault == true)
                    {
                        DeviceID = info.driver;
                        break;
                    }
                }
                Process.Start(
                    @"C:\Windows\System32\rundll32.exe",
                    String.Format(@"C:\Windows\System32\shell32.dll,Control_RunDLL C:\Windows\System32\mmsys.cpl ms-mmsys:,{0},{1}", DeviceID, TabToOpen));
            }
            catch
            {
                Program.ShowError(2, "Error", ErrorNoWork, null);
            }
        }

        public static void SetDefaultDevice(int E, int DEV, string ASIODEV)
        {
            if (E == AudioEngine.DSOUND_ENGINE || E == AudioEngine.WASAPI_ENGINE)
                Program.SynthSettings.SetValue("AudioOutput", DEV, RegistryValueKind.DWord);
            else if (E == AudioEngine.ASIO_ENGINE)
                Program.SynthSettings.SetValue("ASIOOutput", ASIODEV, RegistryValueKind.String);

            if (Properties.Settings.Default.LiveChanges) Program.SynthSettings.SetValue("LiveChanges", "1", RegistryValueKind.DWord);
        }

        // WinMM Patch
        public enum MachineType { Native = 0x0000, x86 = 0x014C, Itanium = 0x0200, x64 = 0x8664, ARM64 = 0xAA64 }

        public static MachineType GetAppCompiledMachineType(string fileName)
        {
            const int PE_POINTER_OFFSET = 60, MACHINE_OFFSET = 4;
            byte[] data = new byte[4096];

            using (Stream s = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                s.Read(data, 0, 4096);

            int PE_HEADER_ADDR = BitConverter.ToInt32(data, PE_POINTER_OFFSET);
            int machineUint = BitConverter.ToUInt16(data, PE_HEADER_ADDR + MACHINE_OFFSET);
            return (MachineType)machineUint;
        }

        public static bool IsProcessElevated()
        {
            bool isElevated = false;
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                isElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);
                identity.Dispose();
            }
            return isElevated;
        }

        public static void RestartAsAdmin()
        {
            Boolean isElevated = IsProcessElevated();
            String MessageString = String.Format("You don't have the rights to edit the target folder!{0}", isElevated ? "" : "\n\nDo you want to restart the configurator with admin permissions?");
            
            DialogResult Message = Program.ShowError(isElevated ? 4 : 3, "Permissions required", MessageString, null);
            if (Message == DialogResult.Yes)
            {
                try
                {
                    ProcessStartInfo elevated = new ProcessStartInfo(System.Reflection.Assembly.GetEntryAssembly().Location);
                    elevated.UseShellExecute = true;
                    elevated.Verb = "runas";
                    Process.Start(elevated);
                    Application.ExitThread();
                }
                catch { }
            }
        }

        public static void DriverRegistry(Boolean Uninstall)
        {
            try
            {
                Process Proc = Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\OmniMIDI\\OmniMIDIDriverRegister.exe", Uninstall ? "/unregisterv" : "/registerv");
                Proc.WaitForExit();
            }
            catch (Exception ex)
            {
                Program.ShowError(4, "Error", "There was an error while trying to register/unregister the driver.", ex);
            }
        }

        public static void MIDIMapRegistry(Boolean Uninstall)
        {
            try
            {
                Process Proc = Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + "\\OmniMIDI\\OmniMIDIDriverRegister.exe", Uninstall ? "/umidimapv" : "/rmidimapv");
                Proc.WaitForExit();
            }
            catch (Exception ex)
            {
                Program.ShowError(4, "Error", "There was an error while trying to register/unregister the MIDI mapper.", ex);
            }
        }

        public static bool CheckMIDIMapper() // Check if OmniMapper is installed
        {
            return (Functions.CLSID32.GetValue("midimapper", "midimap.dll").ToString() == "OmniMIDI\\OmniMapper.dll");
        }

        public static DialogResult ApplyWinMMWRPPatch(Boolean DAWMode)
        {
            OpenFileDialog WinMMDialog = new OpenFileDialog();
            WinMMDialog.Filter = "Executables (*.exe)|*.exe;";
            WinMMDialog.Title = "Select an application to patch";
            WinMMDialog.Multiselect = false;
            WinMMDialog.InitialDirectory = Properties.Settings.Default.LastPatchPath;

            if (WinMMDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    MachineType BitApp = GetAppCompiledMachineType(WinMMDialog.FileName);
                    String DirectoryPath = Path.GetDirectoryName(WinMMDialog.FileName);

                    Properties.Settings.Default.LastPatchPath = WinMMDialog.FileName;
                    Properties.Settings.Default.Save();

                    if (DirectoryPath.Contains(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32")) ||
                        DirectoryPath.Contains(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "SysWOW64")) ||
                        DirectoryPath.Contains(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "SysArm32")))
                    {
                        Program.ShowError(4, "Error", "I'm afraid I can't do that, Dave.", null);
                        WinMMDialog.Dispose();
                        return DialogResult.No;
                    }

                    RemovePatchFiles(WinMMDialog.FileName, true);
                    if (BitApp == MachineType.x86)
                        File.WriteAllBytes(String.Format("{0}\\{1}", DirectoryPath, "winmm.dll"), DAWMode ? Properties.Resources.winmm32DAW : Properties.Resources.winmm32wrp);
                    else if (BitApp == MachineType.x64)
                        File.WriteAllBytes(String.Format("{0}\\{1}", DirectoryPath, "winmm.dll"), DAWMode ? Properties.Resources.winmm64DAW : Properties.Resources.winmm64wrp);
                    else if (BitApp == MachineType.ARM64)
                        File.WriteAllBytes(String.Format("{0}\\{1}", DirectoryPath, "winmm.dll"), DAWMode ? Properties.Resources.winmmARM64DAW : Properties.Resources.winmmARM64wrp);
                    else
                    {
                        Program.ShowError(4, "Error", "Unable to patch the following executable!\nThe configurator can only patch x86, x86-64 and ARM64 executables.\n\nPress OK to continue", null);
                        WinMMDialog.Dispose();
                        return DialogResult.No;
                    }
                }
                catch
                {
                    RestartAsAdmin();
                    WinMMDialog.Dispose();
                    return DialogResult.No;
                }

                WinMMDialog.Dispose();
                return DialogResult.OK;
            }

            WinMMDialog.Dispose();
            return DialogResult.Abort;
        }

        public static DialogResult RemoveWinMMPatch()
        {
            OpenFileDialog WinMMDialog = new OpenFileDialog();

            WinMMDialog.Filter = "Executables (*.exe)|*.exe;";
            WinMMDialog.Title = "Select an application to unpatch";
            WinMMDialog.Multiselect = false;
            WinMMDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (WinMMDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    String DirectoryPath = Path.GetDirectoryName(WinMMDialog.FileName);

                    Properties.Settings.Default.LastPatchPath = WinMMDialog.FileName;
                    Properties.Settings.Default.Save();

                    if (DirectoryPath.Contains(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "System32")) ||
                        DirectoryPath.Contains(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "SysWOW64")) ||
                        DirectoryPath.Contains(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "SysArm32")))
                    {
                        Program.ShowError(4, "Error", "I'm afraid I can't do that, Dave.", null);
                        WinMMDialog.Dispose();
                        return DialogResult.No;
                    }

                    RemovePatchFiles(WinMMDialog.FileName, false);
                }
                catch 
                {
                    RestartAsAdmin();
                    WinMMDialog.Dispose();
                    return DialogResult.No;
                }

                WinMMDialog.Dispose();
                return DialogResult.OK;
            }

            WinMMDialog.Dispose();
            return DialogResult.Abort;
        }

        private static void RemovePatchFiles(String DirectoryPath, Boolean Silent)
        {
            String[] DeleteTheseFiles = { "midimap.dll", "msacm32.drv", "msacm32.dll", "msapd32.drv", "msapd32.dll", "wdmaud.drv", "wdmaud.sys", "winmm.dll", "owinmm.dll" };

            foreach (String DeleteMe in DeleteTheseFiles)
                File.Delete(String.Format("{0}\\{1}", Path.GetDirectoryName(DirectoryPath), DeleteMe));

            if (!Silent) MessageBox.Show(String.Format("\"{0}\" has been successfully unpatched!", Path.GetFileName(DirectoryPath)), "OmniMIDI - Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void LoudMaxInstall()
        {
            try
            {
                bool bit32 = false, bit64 = false;
                string userfolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\OmniMIDI\\LoudMax";

                if (!Directory.Exists(userfolder))
                    Directory.CreateDirectory(userfolder);

                // 32-bit DLL
                if (!File.Exists(userfolder + "\\LoudMax32.dll"))
                {
                    File.WriteAllBytes(String.Format("{0}\\{1}", userfolder, "LoudMax32.dll"), Properties.Resources.LoudMax32);
                    bit32 = true;
                }
                else MessageBox.Show("LoudMax 32-bit seems to be already installed.", "OmniMIDI - Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);


                // 64-bit DLL
                if (Environment.Is64BitOperatingSystem)
                {
                    if (!File.Exists(userfolder + "\\LoudMax64.dll"))
                    {
                        File.WriteAllBytes(String.Format("{0}\\{1}", userfolder, "LoudMax64.dll"), Properties.Resources.LoudMax64);
                        bit64 = true;
                    }
                    else MessageBox.Show("LoudMax 64-bit seems to be already installed.", "OmniMIDI - Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else { bit32 = true; }

                if (bit32 == true && bit64 == true)
                    MessageBox.Show("LoudMax successfully installed!", "OmniMIDI - Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                Program.ShowError(4, "LoudMax Installation", "Crap, an error!\nAre you sure you closed all the apps using the driver? They might have locked LoudMax.", ex);
            }
        }

        public static void LoudMaxUninstall()
        {
            try
            {
                bool bit32 = false, bit64 = false;
                string userfolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\OmniMIDI\\LoudMax";

                // 32-bit DLL
                if (File.Exists(userfolder + "\\LoudMax32.dll"))
                {
                    File.Delete(userfolder + "\\LoudMax32.dll");
                    bit32 = true;
                }

                // 64-bit DLL
                if (File.Exists(userfolder + "\\LoudMax64.dll"))
                {
                    File.Delete(userfolder + "\\LoudMax64.dll");
                    bit64 = true;
                }

                if (Directory.Exists(userfolder))
                    Directory.Delete(userfolder);

                if (bit32 == true || bit64 == true)
                    MessageBox.Show("LoudMax successfully uninstalled!", "OmniMIDI - Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                Program.ShowError(4,"LoudMax Installation", "Crap, an error!\nAre you sure you closed all the apps using the driver? They might have locked LoudMax.", ex);
            }
        }

        public static void LoadSoundFontList(int list)
        {
            Program.Watchdog.SetValue("currentsflist", list, RegistryValueKind.DWord);
            Program.Watchdog.SetValue(String.Format("rel{0}", list), "1", RegistryValueKind.DWord);
        }

        public static void ResetSpecificSetting(string PN)
        {
            var PV = Properties.Settings.Default.PropertyValues[PN];

            PV.PropertyValue = PV.Property.DefaultValue;
            PV.Deserialized = false;

            Properties.Settings.Default[PN] = PV.PropertyValue;
        }
    }
}
