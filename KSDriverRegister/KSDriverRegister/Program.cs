using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace KSDriverRegister
{
    class Program
    {
        public static RegistryKey driver32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
        public static RegistryKey driver64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);

        static void Main(string[] args)
        {
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
                using (RegistryKey clsid32 = driver32.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\Drivers32", true))
                {
                    try
                    {
                        if (clsid32.GetValue("midi9").ToString() == "keppysynth\\keppysynth.dll")
                        {
                            clsid32.DeleteValue("midi9");
                        }
                    }
                    catch
                    {

                    }

                    for (int i = 1; i <= 9; i++)
                    {
                        try
                        {
                            if (clsid32.GetValue(String.Format("midi{0}", i)).ToString() == "wdmaud.drv")
                            {
                                clsid32.SetValue(String.Format("midi{0}", i), "keppysynth\\keppysynth.dll");
                                Console.Write("Succesfully registered.");
                                break;
                            }
                            else if (clsid32.GetValue(String.Format("midi{0}", i)).ToString() == "keppysynth\\keppysynth.dll")
                            {
                                Console.Write("Already registered.");
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        catch
                        {
                            Console.Write("No MIDI driver values available.");
                            break;
                        }
                    }
                }
                if (Environment.Is64BitOperatingSystem)
                {
                    using (RegistryKey clsid64 = driver64.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\Drivers32", true))
                    {
                        try
                        {
                            if (clsid64.GetValue("midi9").ToString() == "keppysynth\\keppysynth.dll")
                            {
                                clsid64.DeleteValue("midi9");
                            }
                        }
                        catch
                        {

                        }

                        for (int i = 1; i <= 9; i++)
                        {
                            try
                            {
                                if (clsid64.GetValue(String.Format("midi{0}", i)).ToString() == "wdmaud.drv")
                                {
                                    clsid64.SetValue(String.Format("midi{0}", i), "keppysynth\\keppysynth.dll");
                                    Console.Write("Succesfully registered.");
                                    break;
                                }
                                else if (clsid64.GetValue(String.Format("midi{0}", i)).ToString() == "keppysynth\\keppysynth.dll")
                                {
                                    Console.Write("Already registered.");
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            catch
                            {
                                Console.Write("No MIDI driver values available.");
                                break;
                            }
                        }
                    }
                }
            }
            else if (arguments[0] == "/unregister")
            {
                using (RegistryKey clsid32 = driver32.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\Drivers32", true))
                {
                    try
                    {
                        if (clsid32.GetValue("midi9").ToString() == "keppysynth\\keppysynth.dll")
                        {
                            clsid32.DeleteValue("midi9");
                        }
                    }
                    catch
                    {

                    }

                    for (int i = 1; i <= 9; i++)
                    {
                        try
                        {
                            if (clsid32.GetValue(String.Format("midi{0}", i)).ToString() == "keppysynth\\keppysynth.dll")
                            {
                                clsid32.SetValue(String.Format("midi{0}", i), "wdmaud.drv");
                                Console.Write("Succesfully unregistered.");
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        catch
                        {
                            Console.Write("Keppy's Synthesizer doesn't seem to be installed.");
                            break;
                        }
                    }
                }
                if (Environment.Is64BitOperatingSystem)
                {
                    using (RegistryKey clsid64 = driver64.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\Drivers32", true))
                    {
                        try
                        {
                            if (clsid64.GetValue("midi9").ToString() == "keppysynth\\keppysynth.dll")
                            {
                                clsid64.DeleteValue("midi9");
                            }
                        }
                        catch
                        {

                        }

                        for (int i = 1; i <= 9; i++)
                        {
                            try
                            {
                                if (clsid64.GetValue(String.Format("midi{0}", i)).ToString() == "keppysynth\\keppysynth.dll")
                                {
                                    clsid64.SetValue(String.Format("midi{0}", i), "wdmaud.drv");
                                    Console.Write("Succesfully unregistered.");
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            catch
                            {
                                Console.Write("Keppy's Synthesizer doesn't seem to be installed.");
                                break;
                            }
                        }
                    }
                }
            }
            else if (arguments[0] == "/help")
            {
                Console.Write("Keppy's Synthesizer Register/Unregister Tool");
                Console.Write(Environment.NewLine);
                Console.Write(Environment.NewLine);
                Console.Write("/register = Register the driver as a MIDI device");
                Console.Write(Environment.NewLine);
                Console.Write("/unregister = Unregister the driver");
                Console.Write(Environment.NewLine);
                Console.Write("/help = This list");
                Console.Read();
            }
            else
            {
                Console.Write("Invalid argument. Type \"KSDriverRegister.exe /help\" to see the available commands.");
                Console.Read();
            }
        }
    }
}
