using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OmniMIDIConfigurator
{
    class SFZInfo
    {
        public static long GetSoundFontZSize(String SFZFile)
        {
            try
            {
                string line;
                System.IO.StreamReader file = new System.IO.StreamReader(SFZFile);

                while ((line = file.ReadLine()) != null)
                {
                    if (line.Contains("default_path="))
                    {
                        long size2 = 0;
                        String SFZ = line.Substring(line.LastIndexOf('=') + 1);
                        String MainPath = Path.GetFullPath(String.Format("{0}\\{1}", Path.GetDirectoryName(SFZFile), SFZ));
                        FileInfo[] FileInfoSFZ2 = new DirectoryInfo(MainPath).GetFiles("*.*", SearchOption.AllDirectories);
                        foreach (FileInfo File in FileInfoSFZ2) size2 += File.Length;
                        file.Close();
                        Program.DebugToConsole(false, String.Format("Parsed size of the following SFZ SoundFont: {0}", SFZFile), null);
                        return size2;
                        break;
                    }
                }
                file.Close();

                long size = 0;
                FileInfo[] FileInfoSFZ = new DirectoryInfo(Path.GetDirectoryName(SFZFile)).GetFiles("*.*", SearchOption.AllDirectories);
                foreach (FileInfo File in FileInfoSFZ) size += File.Length;
                Program.DebugToConsole(false, String.Format("Parsed size of the following SFZ SoundFont: {0}", SFZFile), null);
                return size;
            }
            catch
            {
                try
                {
                    long size = 0;
                    FileInfo[] FileInfoSFZ = new DirectoryInfo(Path.GetDirectoryName(SFZFile)).GetFiles("*.*", SearchOption.AllDirectories);
                    foreach (FileInfo File in FileInfoSFZ) size += File.Length;
                    Program.DebugToConsole(false, String.Format("Parsed size of the following legacy SFZ SoundFont: {0}", SFZFile), null);
                    return size;
                }
                catch (Exception ex)
                {
                    Program.DebugToConsole(true, String.Format("Error while parsing the size of the following SFZ SoundFont: {0}", SFZFile), ex);
                    return 0;
                }
            }
        }
    }
}
