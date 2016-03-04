using System;
using System.Runtime.InteropServices;

namespace MIDI.Interop
{
    /// <summary>
    /// Describes the capabilities of a MIDI output device.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct MIDIOUTCAPS
    {
        /// <summary>
        /// Manufacturer identifier of the device driver for the MIDI output device.
        /// </summary>
        public UInt16 wMid;
        /// <summary>
        /// Product identifier of the MIDI output device.
        /// </summary>
        public UInt16 wPid;
        /// <summary>
        /// Version number of the device driver for the MIDI output device.
        /// The high-order byte is the major version number, and the low-order byte is the minor version number.
        /// </summary>
        public UInt32 vDriverVersion;
        /// <summary>
        /// Product name in a null-terminated string.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.MAXPNAMELEN)]
        public string szPname;
        /// <summary>
        /// Type of the MIDI output device.
        /// </summary>
        public UInt16 wTechnology;
        /// <summary>
        /// Number of voices supported by an internal synthesizer device.
        /// If the device is a port, this member is not meaningful and is set to 0.
        /// </summary>
        public UInt16 wVoices;
        /// <summary>
        /// Maximum number of simultaneous notes that can be played by an internal synthesizer device.
        /// If the device is a port, this member is not meaningful and is set to 0.
        /// </summary>
        public UInt16 wNotes;
        /// <summary>
        /// Channels that an internal synthesizer device responds to, where the least significant bit refers to channel 0 and the most significant bit to channel 15.
        /// </summary>
        public UInt16 wChannelMask;
        /// <summary>
        /// Optional functionality supported by the device.
        /// </summary>
        public UInt32 dwSupport;
    }
}
