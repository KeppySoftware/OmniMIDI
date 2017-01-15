using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace KSDriverRegister
{
    class Program
    {
        public static RegistryKey driver32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
        public static RegistryKey driver64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
        public static RegistryKey clsid32 = driver32.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\Drivers32", true);
        public static RegistryKey clsid64 = driver64.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\Drivers32", true);

        static void Main(string[] args)
        {
            if (Environment.OSVersion.Version.Major < 6)
            {
                try
                {
                    System.Media.SystemSounds.Hand.Play();
                    Environment.Exit(-1);
                }
                catch
                {
                    Environment.Exit(-1);
                }
            }

            List<string> copyme = new List<string>();
            if (args.Length != 0)
            {
                foreach (String s in args)
                {
                    copyme.Add(s);
                }
            }
            else
            {
                copyme.Add("/help");
            }

            string[] arguments = new string[copyme.ToArray().Length];
            copyme.ToArray().CopyTo(arguments, 0);

            if (arguments[0] == "/register")
            {
                if (Environment.Is64BitOperatingSystem)
                {
                    Register(true, "x86", clsid32);
                    Register(true, "x64", clsid64);
                }
                else
                {
                    Register(true, "x86", clsid32);
                }
            }
            else if (arguments[0] == "/unregister")
            {
                if (Environment.Is64BitOperatingSystem)
                {
                    Unregister(true, "x86", clsid32);
                    Unregister(true, "x64", clsid64);
                }
                else
                {
                    Unregister(true, "x86", clsid32);
                }
            }
            else if (arguments[0] == "/registerv")
            {
                if (Environment.Is64BitOperatingSystem)
                {
                    Register(false, "x86", clsid32);
                    Register(false, "x64", clsid64);
                }
                else
                {
                    Register(false, "x86", clsid32);
                }
            }
            else if (arguments[0] == "/unregisterv")
            {
                if (Environment.Is64BitOperatingSystem)
                {
                    Unregister(false, "x86", clsid32);
                    Unregister(false, "x64", clsid64);
                }
                else
                {
                    Unregister(false, "x86", clsid32);
                }
            }
            else if (arguments[0] == "/help")
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Keppy's Synthesizer Register/Unregister Tool\n");
                sb.AppendLine("/register = Register the driver as a MIDI device");
                sb.AppendLine("/unregister = Unregister the driver");
                sb.AppendLine("/help = This list");
                MessageBox.Show(sb.ToString(), "Keppy's Synthesizer R/U Tool ~ Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
            else
            {
                MessageBox.Show("Invalid argument.\nType \"KSDriverRegister.exe /help\" to see the available commands.", "Keppy's Synthesizer R/U Tool ~ Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                Application.Exit();
            }
        }

        public static void ShowMessage(bool IsSilent, String Text, String Title, MessageBoxIcon TypeOfError) {
            if (!IsSilent)
            {
                MessageBox.Show(Text, String.Format("Keppy's Synthesizer R/U Tool ~ {0}", Title), MessageBoxButtons.OK, TypeOfError);
            }
        }

        public static void Register(bool IsSilent, String WhichBit, RegistryKey WhichKey)
        {
            WhichKey.SetValue("midi", "wdmaud.drv");
            try
            {
                if (WhichKey.GetValue("midi9").ToString() == "keppysynth\\keppysynth.dll")
                {
                    WhichKey.DeleteValue("midi9");
                }
            }
            catch { }

            for (int i = 1; i <= 9; i++)
            {
                try
                {
                    if (WhichKey.GetValue(String.Format("midi{0}", i), null) == null)
                    {
                        WhichKey.SetValue(String.Format("midi{0}", i), "keppysynth\\keppysynth.dll");
                        ShowMessage(IsSilent, String.Format("{0} driver succesfully registered.", WhichBit), "Information", MessageBoxIcon.Information);
                        break;
                    }
                    else if (WhichKey.GetValue(String.Format("midi{0}", i)).ToString() == "wdmaud.drv")
                    {
                        WhichKey.SetValue(String.Format("midi{0}", i), "keppysynth\\keppysynth.dll");
                        ShowMessage(IsSilent, String.Format("{0} driver succesfully registered.", WhichBit), "Information", MessageBoxIcon.Information);
                        break;
                    }
                    else if (WhichKey.GetValue(String.Format("midi{0}", i)).ToString() == "keppysynth\\keppysynth.dll")
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
        }

        public static void Unregister(bool IsSilent, String WhichBit, RegistryKey WhichKey)
        {
            try
            {
                if (WhichKey.GetValue("midi9").ToString() == "keppysynth\\keppysynth.dll")
                {
                    WhichKey.DeleteValue("midi9");
                }
            }
            catch
            {

            }

            for (int i = 1; i <= 9; i++)
            {
                try
                {
                    if (WhichKey.GetValue(String.Format("midi{0}", i)).ToString() == "keppysynth\\keppysynth.dll")
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
        }
    }
}
