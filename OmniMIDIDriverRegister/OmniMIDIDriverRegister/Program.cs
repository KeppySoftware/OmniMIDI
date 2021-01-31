using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace OmniMIDIDriverRegister
{
    class Program
    {
        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

        public static RegistryKey driver32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
        public static RegistryKey driver64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
        public static RegistryKey clsid32 = driver32.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\Drivers32", true);
        public static RegistryKey clsid64 = driver64.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\Drivers32", true);

        static void Main(string[] args)
        {
            string arg = "/showdialog";
            if (args.Length > 0) arg = args[0];

            if (arg == "/register" || arg == "/registerv")
            {
                Register();

                /*
                Register(true, "x86", clsid32);
                if (Environment.Is64BitOperatingSystem) Register(arg == "/register" ? true : false, "x64", clsid64);
                */
            }
            else if (arg == "/unregister" || arg == "/unregisterv")
            {
                Unregister();

                /*
                Unregister(true, "x86", clsid32);
                if (Environment.Is64BitOperatingSystem) Unregister(arg == "/unregister" ? true : false, "x64", clsid64);
                */
            }
            /*
            else if (arg == "/registerv")
            {
                Register(false, "x86", clsid32);
                if (Environment.Is64BitOperatingSystem) Register(false, "x64", clsid64);
            }
            else if (arg == "/unregisterv")
            {
                Unregister(false, "x86", clsid32);
                if (Environment.Is64BitOperatingSystem) Unregister(false, "x64", clsid64);
            }
            */
            else if (arg == "/rmidimap")
            {
                RegisterMidiMapper(true, false);
            }
            else if (arg == "/umidimap")
            {
                RegisterMidiMapper(true, true);
            }
            else if (arg == "/rmidimapv")
            {
                RegisterMidiMapper(false, false);
            }
            else if (arg == "/umidimapv")
            {
                RegisterMidiMapper(false, true);
            }
            else if (arg == "/showdialog")
            {
                Application.EnableVisualStyles();
                new OmniMIDIDefaultDialog().ShowDialog();
                Application.Exit();
            }
            else if (arg == "/help")
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("OmniMIDI Register/Unregister Tool\n");
                sb.AppendLine("/showdialog = Show a dialog that allows you to register/unregister the driver manually");
                sb.AppendLine("/register = Register the driver as a MIDI device");
                sb.AppendLine("/unregister = Unregister the driver");
                sb.AppendLine("/help = This list");
                MessageBox.Show(sb.ToString(), "OmniMIDI R/U Tool ~ Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
            else
            {
                MessageBox.Show("Invalid argument.\nType \"OmniMIDIDriverRegister.exe /help\" to see the available commands.", "OmniMIDI R/U Tool ~ Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                Application.Exit();
            }
        }

        public static void ShowMessage(bool IsSilent, String Text, String Title, MessageBoxIcon TypeOfError)
        {
            if (!IsSilent)
            {
                MessageBox.Show(Text, String.Format("OmniMIDI R/U Tool ~ {0}", Title), MessageBoxButtons.OK, TypeOfError);
            }
        }

        public static void Register(/* bool IsSilent, String WhichBit, RegistryKey WhichKey */)
        {
            ProcessStartInfo SI = new ProcessStartInfo();
            SI.UseShellExecute = true;
            SI.Verb = "RunAs";

            if (Environment.Is64BitOperatingSystem)
                SI.FileName = Environment.ExpandEnvironmentVariables(@"%windir%\Sysnative\rundll32.exe");
            else
                SI.FileName = Environment.ExpandEnvironmentVariables(@"%windir%\System32\rundll32.exe");

            SI.Arguments = "OmniMIDI.dll,DriverRegistration RegisterDrv";
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

        public static void Unregister(/* bool IsSilent, String WhichBit, RegistryKey WhichKey */)
        {
            ProcessStartInfo SI = new ProcessStartInfo();
            SI.UseShellExecute = true;
            SI.Verb = "RunAs";

            if (Environment.Is64BitOperatingSystem)
                SI.FileName = Environment.ExpandEnvironmentVariables(@"%windir%\Sysnative\rundll32.exe");
            else
                SI.FileName = Environment.ExpandEnvironmentVariables(@"%windir%\System32\rundll32.exe");

            SI.Arguments = "OmniMIDI.dll,DriverRegistration UnregisterDrv";
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
        }
    }
}
