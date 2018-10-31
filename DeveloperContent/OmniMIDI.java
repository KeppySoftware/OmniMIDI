// I'm not good with Java, please help me fix this code (if there are errors, that is)

import com.sun.jna.Library;
import com.sun.jna.Native;

class MIDIHDR
{
    public String lpdata;
    public int dwBufferLength;
    public int dwBytesRecorded;
    public Object dwUser;
    public int dwFlags;
    public MIDIHDR lpNext;
    public Object reserved;
    public int dwOffset;
    public Object dwReserved;
};

class Settings 
{
	public int AlternativeCPU;				// Autopanic switch (DEPRECATED)
	public int CapFramerate;				// Cap input framerate
	public int DebugMode;					// Debug console
	public int DisableNotesFadeOut;			// Disable fade-out
	public int DontMissNotes;				// Slow down instead of missing notes

	public int EnableSFX;					// Enable or disable FXs
	public int FastHotkeys;					// Enable/Disable fast hotkeys
	public int FullVelocityMode;			// Enable full velocity mode
	public int IgnoreNotesBetweenVel;		// Ignore notes in between two velocity values
	public int IgnoreAllEvents;				// Ignore all MIDI events
	public int IgnoreSysEx;					// Ignore SysEx events
	public int IgnoreSysReset;				// Ignore sysex messages
	public int LimitTo88Keys;				// Limit to 88 keys
	public int LiveChanges;					// Live changes
	public int MT32Mode;					// Roland MT-32 mode
	public int MonoRendering;				// Mono rendering (Instead of stereo by default)
	public int NoBlacklistMessage;			// Disable blacklist message (DEPRECATED)
	public int NoteOff1;					// Note cut public uint
	public int NotesCatcherWithAudio;		// For old-ass PCs
	public int OverrideInstruments;			// Override channel instruments
	public int PreloadSoundFonts;			// Soundfont preloading
	public int SincInter;					// Sinc
	public int SleepStates;					// Reduce CPU overhead
	public int VolumeMonitor;				// Volume monitoring
	
	public int AudioBitDepth;				// Floating pouint audio
	public int AudioFrequency;				// Audio frequency
	public int AudioOutputReg;				// Audio output (All devices except AudToWAV and ASIO)
	public int BufferLength;				// Default
	public int CurrentEngine;				// Current engine
	public int DefaultSFList;				// Default soundfont list
	public int DriverPriority;				// Process priority
	public int Extra8Lists;					// Enable extra 8 SoundFont lists
	public int MaxRenderingTime;			// CPU usage public uint
	public int MaxVelIgnore;				// Ignore notes in between two velocity values
	public int MinVelIgnore;				// Ignore notes in between two velocity values
	public int OutputVolume;				// Volume
	public int TransposeValue;				// Pitch shift (127 = None)
	public int MaxVoices;					// Voices limit
	public int SincConv;					// Sinc
}

class DebugInfo
{
	public float RenderingTime;
	public int ActiveVoices[] = new int[16];
	
	public double ASIOInputLatency;
	public double ASIOOutputLatency;
}

class KDMAPI 
{
	public interface OmniMIDILib extends Library 
	{
		public bool ReturnKDMAPIVer(int Major, int Minor, int Build, int Revision);
		public bool IsKDMAPIAvailable();
		public int InitializeKDMAPIStream();
		public int TerminateKDMAPIStream();
		public void ResetKDMAPIStream();
		public int SendDirectData(int dwMsg);
		public int SendDirectLongData(MIDIHDR dwMsg);
		public int SendDirectDataNoBuf(int dwMsg);
		public int SendDirectLongDataNoBuf(MIDIHDR dwMsg);
		public int PrepareLongData(MIDIHDR dwMsg);
		public int UnprepareLongData(MIDIHDR dwMsg);
		public void ChangeDriverSettings(Settings Struct, int StructSize);
		public void LoadCustomSoundFontsList(char[] Directory);
		public DebugInfo GetDriverDebugInfo();
	}
	
	public static OmniMIDILib MainLibrary;
}

public class YourProgram
{
	public static void main(String[] args) 
	{
		KDMAPI.MainLibrary = (OmniMIDILib)Native.loadLibrary("OmniMIDI", OmniMIDILib.class);

		KDMAPI.MainLibrary.InitializeKDMAPIStream();
		KDMAPI.MainLibrary.SendDirectData(0);
		KDMAPI.MainLibrary.TerminateKDMAPIStream();
		
		return;
	}
}