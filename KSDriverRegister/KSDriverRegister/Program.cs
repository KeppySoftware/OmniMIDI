using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.IO;
using System.Runtime.InteropServices;

namespace KSDriverRegister
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
            List<string> copyme = new List<string>();
            if (Environment.OSVersion.Version.Major < 6)
            {
                MessageBox.Show("Keppy's Synthesizer is not compatible with Windows NT 5.x operating systems.\n\nPress OK to quit.", "Keppy's Synthesizer R/U Tool ~ Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                Environment.Exit(0xA);
            }
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
            else if (arguments[0] == "/associate")
            {
                string ExecutableName = Path.GetFileName(Application.ExecutablePath);
                string OpenWith = Application.ExecutablePath;
                string[] extensions = { "sf2", "sfz", "sfpack" };
                try
                {
                    foreach (string ext in extensions)
                    {
                        using (RegistryKey User_Classes = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Classes\\", true))
                        using (RegistryKey User_Ext = User_Classes.CreateSubKey("." + ext))
                        using (RegistryKey User_AutoFile = User_Classes.CreateSubKey(ext + "_auto_file"))
                        using (RegistryKey User_AutoFile_Command = User_AutoFile.CreateSubKey("shell").CreateSubKey("open").CreateSubKey("command"))
                        using (RegistryKey ApplicationAssociationToasts = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\ApplicationAssociationToasts\\", true))
                        using (RegistryKey User_Classes_Applications = User_Classes.CreateSubKey("Applications"))
                        using (RegistryKey User_Classes_Applications_Exe = User_Classes_Applications.CreateSubKey(ExecutableName))
                        using (RegistryKey User_Application_Command = User_Classes_Applications_Exe.CreateSubKey("shell").CreateSubKey("open").CreateSubKey("command"))
                        using (RegistryKey User_Explorer = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\." + ext))
                        using (RegistryKey User_Choice = User_Explorer.OpenSubKey("UserChoice"))
                        {
                            User_Classes_Applications_Exe.SetValue("", "SoundFont file", RegistryValueKind.String);
                            User_Ext.SetValue("", ext + "_auto_file", RegistryValueKind.String);
                            User_Classes.SetValue("", ext + "_auto_file", RegistryValueKind.String);
                            User_Classes.CreateSubKey(ext + "_auto_file");
                            User_AutoFile_Command.SetValue("", "\"" + OpenWith + "\"" + " \"%1\"");
                            ApplicationAssociationToasts.SetValue(ext + "_auto_file_." + ext, 0);
                            ApplicationAssociationToasts.SetValue(@"Applications\" + ext + "_." + ext, 0);
                            User_Application_Command.SetValue("", "\"" + OpenWith + "\"" + " \"%1\"");
                            User_Explorer.CreateSubKey("OpenWithList").SetValue("a", ExecutableName);
                            User_Explorer.CreateSubKey("OpenWithProgids").SetValue(ext + "_auto_file", "0");
                            if (User_Choice != null) User_Explorer.DeleteSubKey("UserChoice");
                            User_Explorer.CreateSubKey("UserChoice").SetValue("ProgId", @"Applications\" + ExecutableName);
                        }
                    }
                    SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
                }
                catch { }
            }
            else if (arguments[0] == "/rmidimap")
            {
                RegisterMidiMapper(true, false);
            }
            else if (arguments[0] == "/umidimap")
            {
                RegisterMidiMapper(true, true);
            }
            else if (arguments[0] == "/rmidimapv")
            {
                RegisterMidiMapper(false, false);
            }
            else if (arguments[0] == "/umidimapv")
            {
                RegisterMidiMapper(false, true);
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

        public static void RegisterMidiMapper(bool IsSilent, bool Uninstall)
        {
            if (!Uninstall)
            {
                Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Multimedia\MIDIMap");
                if (Environment.Is64BitOperatingSystem)
                {
                    clsid32.SetValue("midimapper", "keppysynth\\amidimap.cpl");
                    clsid64.SetValue("midimapper", "keppysynth\\amidimap.cpl");
                }
                else
                {
                    clsid32.SetValue("midimapper", "keppysynth\\amidimap.cpl");
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
