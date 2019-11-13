using System;
using System.IO;

namespace OmniMIDIConfigurator
{
    class SFZ
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
                        return size2;
                        break;
                    }
                }
                file.Close();

                long size = 0;
                FileInfo[] FileInfoSFZ = new DirectoryInfo(Path.GetDirectoryName(SFZFile)).GetFiles("*.*", SearchOption.AllDirectories);
                foreach (FileInfo File in FileInfoSFZ) size += File.Length;
                return size;
            }
            catch
            {
                try
                {
                    long size = 0;
                    FileInfo[] FileInfoSFZ = new DirectoryInfo(Path.GetDirectoryName(SFZFile)).GetFiles("*.*", SearchOption.AllDirectories);
                    foreach (FileInfo File in FileInfoSFZ) size += File.Length;
                    return size;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        }
    }
}
