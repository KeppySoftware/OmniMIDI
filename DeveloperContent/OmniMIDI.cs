using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace KDMAPI
{
	struct MIDIHDR
	{
		string lpdata;
		uint dwBufferLength;
		uint dwBytesRecorded;
		IntPtr dwUser;
		uint dwFlags;
		MIDIHDR lpNext;
		IntPtr reserved;
		uint dwOffset;
		IntPtr dwReserved;
	}
	
	struct Settings
	{
		int AlternativeCPU;				// Autopanic switch (DEPRECATED)
		int CapFramerate;				// Cap input framerate
		int DebugMode;					// Debug console
		int DisableNotesFadeOut;		// Disable fade-out
		int DontMissNotes;				// Slow down instead of missing notes

		int EnableSFX;					// Enable or disable FXs
		int FastHotkeys;				// Enable/Disable fast hotkeys
		int FullVelocityMode;			// Enable full velocity mode
		int IgnoreNotesBetweenVel;		// Ignore notes in between two velocity values
		int IgnoreAllEvents;			// Ignore all MIDI events
		int IgnoreSysEx;				// Ignore SysEx events
		int IgnoreSysReset;				// Ignore sysex messages
		int LimitTo88Keys;				// Limit to 88 keys
		int LiveChanges;				// Live changes
		int MT32Mode;					// Roland MT-32 mode
		int MonoRendering;				// Mono rendering (Instead of stereo by default)
		int NoBlacklistMessage;			// Disable blacklist message (DEPRECATED)
		int NoteOff1;					// Note cut INT
		int NotesCatcherWithAudio;		// For old-ass PCs
		int OverrideInstruments;		// Override channel instruments
		int PreloadSoundFonts;			// Soundfont preloading
		int SincInter;					// Sinc
		int SleepStates;				// Reduce CPU overhead
		int VolumeMonitor;				// Volume monitoring

		uint AudioBitDepth;				// Floating pouint audio
		uint AudioFrequency;			// Audio frequency
		uint AudioOutputReg;			// Audio output (All devices except AudToWAV and ASIO)
		uint BufferLength;				// Default
		uint CurrentEngine;				// Current engine
		uint DefaultSFList;				// Default soundfont list
		uint DriverPriority;			// Process priority
		int Extra8Lists;				// Enable extra 8 SoundFont lists
		uint MaxRenderingTime;			// CPU usage INT
		uint MaxVelIgnore;				// Ignore notes in between two velocity values
		uint MinVelIgnore;				// Ignore notes in between two velocity values
		uint OutputVolume;				// Volume
		uint TransposeValue;			// Pitch shift (127 = None)
		uint MaxVoices;					// Voices limit
		uint SincConv;					// Sinc
	}
	
	struct DebugInfo 
	{
		float RenderingTime;
		int ActiveVoices[] = new int[16];
		
		double ASIOInputLatency;
		double ASIOOutputLatency;
	}
	
    // KDMAPI funcs
	[DllImport("OmniMIDI.dll")]
	public static extern Boolean ReturnKDMAPIVer(out Int32 Major, out Int32 Minor, out Int32 Build, out Int32 Revision);	
			
	[DllImport("OmniMIDI.dll")]
	public static extern bool IsKDMAPIAvailable();
			
	[DllImport("OmniMIDI.dll")]
	public static extern int InitializeKDMAPIStream();
			
	[DllImport("OmniMIDI.dll")]
	public static extern int TerminateKDMAPIStream();
			
	[DllImport("OmniMIDI.dll")]
	public static extern void ResetKDMAPIStream();
			
	[DllImport("OmniMIDI.dll")]
	public static extern uint SendDirectData(uint dwMsg);
			
	[DllImport("OmniMIDI.dll")]
	public static extern uint SendDirectDataNoBuf(uint dwMsg);

	[DllImport("OmniMIDI.dll")]
	public static extern uint SendDirectLongData(ref MIDIHDR IIMidiHdr);
			
	[DllImport("OmniMIDI.dll")]
	public static extern uint SendDirectLongDataNoBuf(ref MIDIHDR IIMidiHdr);
			
	[DllImport("OmniMIDI.dll")]
	public static extern uint PrepareLongData(ref MIDIHDR IIMidiHdr);

	[DllImport("OmniMIDI.dll")]
	public static extern uint UnprepareLongData(ref MIDIHDR IIMidiHdr);
			
	[DllImport("OmniMIDI.dll")]
	public static extern void ChangeDriverSettings(ref Settings IIMidiHdr, uint StructSize);
			
	[DllImport("OmniMIDI.dll")]
	public static extern void LoadCustomSoundFontsList(ref String Directory);
			
	[DllImport("OmniMIDI.dll")]
	public static extern DebugInfo GetDriverDebugInfo();
}

namespace YourProgram 
{
	public static void Main(string[] args)
	{
		KDMAPI.InitializeKDMAPIStream();
		KDMAPI.SendDirectData(0x0);
		KDMAPI.TerminateKDMAPIStream();
		
		return;
	}
}