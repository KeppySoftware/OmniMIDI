using System;
using System.Runtime.InteropServices;

namespace MIDI.Interop
{
    internal static class Functions
    {
        /// <summary>
        /// Retrieves the number of MIDI output devices present in the system.
        /// </summary>
        /// <returns>Returns the number of MIDI output devices.
        /// A return value of zero means that there are no devices (not that there is no error).</returns>
        [DllImport("winmm.dll")]
        internal static extern UInt32 midiOutGetNumDevs();

        /// <summary>
        /// Queries a specified MIDI output device to determine its capabilities.
        /// </summary>
        /// <param name="uDeviceID">Identifier of the MIDI output device.
        /// The device identifier specified by this parameter varies from zero to one less than the number of devices present.
        /// The MIDI_MAPPER constant is also a valid device identifier.
        /// This parameter can also be a properly cast device handle.</param>
        /// <param name="lpMidiOutCaps">Pointer to a MIDIOUTCAPS structure.
        /// This structure is filled with information about the capabilities of the device.</param>
        /// <param name="cbMidiOutCaps">Size, in bytes, of the MIDIOUTCAPS structure.</param>
        /// <returns>Result of the operation.</returns>
        [DllImport("winmm.dll")]
        internal static extern UInt32 midiOutGetDevCaps(Int32 uDeviceID, ref MIDIOUTCAPS lpMidiOutCaps, UInt32 cbMidiOutCaps);
    }
}
