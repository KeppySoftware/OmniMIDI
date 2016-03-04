using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MIDI.Interop;

namespace MIDI.NET.Devices
{
    public static class Collections
    {
        private static IList<Output> outputs;
        
        /// <summary>
        /// Gets a list of all MIDI output devices.
        /// </summary>
        public static IList<Output> Outputs
        {
            get
            {
                if (outputs == null || outputs.Count == 0)
                {
                    Load();
                }
                return outputs;
            }
        }

        /// <summary>
        /// Loads/reloads the MIDI devices
        /// </summary>
        public static void Load()
        {
            outputs = null;
            List<Output> devices = new List<Output>();
            UInt32 numberOfDevices = Functions.midiOutGetNumDevs();
            if (numberOfDevices > 0)
            {
                for (Int32 i = 0; i < numberOfDevices; i++)
                {
                    MIDIOUTCAPS caps = new MIDIOUTCAPS();
                    if (Functions.midiOutGetDevCaps(i, ref caps, (UInt32)Marshal.SizeOf(caps)) == Constants.MMSYSERR_NOERROR)
                    {
                        devices.Add(new Output(i, caps));
                    }
                }
            }
            outputs = devices.AsReadOnly();
        }
    }
}