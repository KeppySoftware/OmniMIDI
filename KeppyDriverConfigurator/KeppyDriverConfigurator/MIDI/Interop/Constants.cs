using System;

namespace MIDI.Interop
{
    /// <summary>
    /// Constants from MMSystem.h
    /// </summary>
    internal static class Constants
    {
        /// <summary>
        /// Max product name length (including NULL).
        /// </summary>
        internal const Int32 MAXPNAMELEN = 32;

        private const UInt32 MMSYSERR_BASE = 0;
        /// <summary>
        /// No error.
        /// </summary>
        internal const UInt32 MMSYSERR_NOERROR = MMSYSERR_BASE;
    }
}
