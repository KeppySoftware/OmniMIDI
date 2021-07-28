using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Security.Principal;
using System.Reflection;

namespace OmniMIDIDriverRegister
{
    internal static class WinMM
    {
        [DllImport("winmm")]
        internal static extern int midiOutGetNumDevs();

        [DllImport("winmm")]
        internal static extern int midiOutGetDevCaps(
            uint uDeviceID,
            out MIDIOUTCAPS caps,
            uint cbMidiOutCaps);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MIDIOUTCAPS
    {
        public ushort wMid;
        public ushort wPid;
        public uint vDriverVersion;     //MMVERSION
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string szPname;
        public ushort wTechnology;
        public ushort wVoices;
        public ushort wNotes;
        public ushort wChannelMask;
        public uint dwSupport;
    }

    class Program
    {
        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

        public static string Argument = "/showdialog";

        static void Main(string[] args)
        {
            if (args.Length > 0) Argument = args[0];
            else
            {
                Application.EnableVisualStyles();
                new OmniMIDIDefaultDialog().ShowDialog();
                return;
            }

            switch (Argument.ToLowerInvariant())
            {
                case "/enum":
                    MIDIOUTCAPS OutCaps = new MIDIOUTCAPS();
                    int NumDevs = WinMM.midiOutGetNumDevs();

                    for (uint i = 0; i < NumDevs; i++)
                    {
                        WinMM.midiOutGetDevCaps(i, out OutCaps, (uint)Marshal.SizeOf(OutCaps));
                        Console.WriteLine(OutCaps.szPname);
                    }

                    return;
                case "/register":
                    Register(false);
                    return;
                case "/registerv":
                    Register(true);
                    return;
                case "/unregister":
                    Unregister(false);
                    return;
                case "/unregisterv":
                    Unregister(true);
                    return;
                case "/rmidimap":
                    RegisterMidiMapper(true, false);
                    return;
                case "/umidimap":
                    RegisterMidiMapper(true, true);
                    return;
                case "/rmidimapv":
                    RegisterMidiMapper(false, false);
                    return;
                case "/umidimapv":
                    RegisterMidiMapper(false, true);
                    return;
                case "/showhelp":
                case "/help":
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("OmniMIDI Register/Unregister Tool\n");
                    sb.AppendLine("/showdialog = Show a dialog that allows you to register/unregister the driver manually");
                    sb.AppendLine("/register = Register the driver as a MIDI device");
                    sb.AppendLine("/unregister = Unregister the driver");
                    sb.AppendLine("/help = This list");
                    MessageBox.Show(sb.ToString(), "OmniMIDI R/U Tool ~ Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                default:
                    MessageBox.Show("Invalid argument.\nType \"OmniMIDIDriverRegister.exe /help\" to see the available commands.", "OmniMIDI R/U Tool ~ Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
            }
        }

        public static bool RestartAsAdminIfRequired()
        {
            bool isElevated = false;
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                isElevated = principal.IsInRole(WindowsBuiltInRole.Administrator);
                identity.Dispose();
            }
            
            if (!isElevated)
            {
                ProcessStartInfo elevated = new ProcessStartInfo(Assembly.GetEntryAssembly().Location);
                elevated.Arguments = Argument;
                elevated.UseShellExecute = true;
                elevated.Verb = "runas";
                Process.Start(elevated);
                return false;
            }

            return true;
        }

        public static void ShowMessage(bool IsSilent, String Text, String Title, MessageBoxIcon TypeOfError)
        {
            if (!IsSilent)
                MessageBox.Show(Text, String.Format("OmniMIDI R/U Tool ~ {0}", Title), MessageBoxButtons.OK, TypeOfError);
        }

        public static void Register(bool IsSilent /*, String WhichBit, RegistryKey WhichKey */)
        {
            if (!RestartAsAdminIfRequired()) return;

            ProcessStartInfo SI = new ProcessStartInfo();
            SI.UseShellExecute = true;
            SI.Verb = "RunAs";

            SI.FileName = String.Format(@"{0}\{1}", Environment.GetFolderPath(Environment.SpecialFolder.System), "rundll32.exe");
            SI.Arguments = String.Format("OmniMIDI.dll,DriverRegistration RegisterDrv{0}", IsSilent ? "S" : String.Empty);
            var P = Process.Start(SI);
            P.WaitForExit();

            /*
            for (int i = 1; i <= 32; i++)
            {
                try
                {
                    if (WhichKey.GetValue(String.Format("midi{0}", i), null) == null)
                    {
                        WhichKey.SetValue(String.Format("midi{0}", i), "OmniMIDI\\OmniMIDI.dll");
                        ShowMessage(IsSilent, String.Format("{0} driver succesfully registered.", WhichBit), "Information", MessageBoxIcon.Information);
                        break;
                    }
                    else if (WhichKey.GetValue(String.Format("midi{0}", i)).ToString() == "wdmaud.drv")
                    {
                        WhichKey.SetValue(String.Format("midi{0}", i), "OmniMIDI\\OmniMIDI.dll");
                        ShowMessage(IsSilent, String.Format("{0} driver succesfully registered.", WhichBit), "Information", MessageBoxIcon.Information);
                        break;
                    }
                    else if (WhichKey.GetValue(String.Format("midi{0}", i)).ToString() == "OmniMIDI\\OmniMIDI.dll")
                    {
                        ShowMessage(IsSilent, String.Format("{0} driver already registered.", WhichBit), "Information", MessageBoxIcon.Information);
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                catch
                {
                    ShowMessage(IsSilent, "No MIDI driver values available.", "Error", MessageBoxIcon.Hand);
                    Console.Read();
                    break;
                }
            }
            */
        }

        public static void Unregister(bool IsSilent /*, String WhichBit, RegistryKey WhichKey */)
        {
            if (!RestartAsAdminIfRequired()) return;

            ProcessStartInfo SI = new ProcessStartInfo();
            SI.UseShellExecute = true;
            SI.Verb = "RunAs";

            SI.FileName = String.Format(@"{0}\{1}", Environment.GetFolderPath(Environment.SpecialFolder.System), "rundll32.exe");
            SI.Arguments = String.Format("OmniMIDI.dll,DriverRegistration UnregisterDrv{0}", IsSilent ? "S" : String.Empty);
            var P = Process.Start(SI);
            P.WaitForExit();

            /*
            for (int i = 1; i <= 32; i++)
            {
                String iS = (i < 1) ? "" : i.ToString();

                try
                {
                    if (WhichKey.GetValue(String.Format("midi{0}", i)).ToString() == "OmniMIDI\\OmniMIDI.dll")
                    {
                        WhichKey.SetValue(String.Format("midi{0}", i), "wdmaud.drv");
                        ShowMessage(IsSilent, String.Format("{0} driver succesfully unregistered.", WhichBit), "Information", MessageBoxIcon.Information);
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                catch
                {
                    ShowMessage(IsSilent, String.Format("{0} driver already unregistered.", WhichBit), "Information", MessageBoxIcon.Information);
                    break;
                }
            }
            */
        }

        public static void RegisterMidiMapper(bool IsSilent, bool Uninstall)
        {
            if (!RestartAsAdminIfRequired()) return;

            RegistryKey driver32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
            RegistryKey driver64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);

            RegistryKey clsid32 = driver32.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\Drivers32", true);
            RegistryKey clsid64 = driver64.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\Drivers32", true);

            if (!Uninstall)
            {
                Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Multimedia\MIDIMap");
                if (Environment.Is64BitOperatingSystem)
                {
                    clsid32.SetValue("midimapper", "OmniMIDI\\OmniMapper.dll");
                    clsid64.SetValue("midimapper", "OmniMIDI\\OmniMapper.dll");
                }
                else
                {
                    clsid32.SetValue("midimapper", "OmniMIDI\\OmniMapper.dll");
                }
                ShowMessage(IsSilent, "MIDI mapper successfully registered.", "Information", MessageBoxIcon.Information);
            }
            else
            {
                if (Environment.Is64BitOperatingSystem)
                {
                    clsid32.SetValue("midimapper", "midimap.dll");
                    clsid64.SetValue("midimapper", "midimap.dll");
                }
                else
                {
                    clsid32.SetValue("midimapper", "midimap.dll");
                }
                ShowMessage(IsSilent, "MIDI mapper succesfully unregistered.", "Information", MessageBoxIcon.Information);
            }

            clsid32.Close();
            driver32.Close();

            if (Environment.Is64BitOperatingSystem)
            {
                clsid64.Close();
                driver64.Close();
            }
        }
    }
}
