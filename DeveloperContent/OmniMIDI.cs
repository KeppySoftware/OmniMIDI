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
	
	enum OMSettingMode {
		OM_SET						= 0x0,
		OM_GET						= 0x1
	}
	
	enum OMSetting
	{
		OM_CAPFRAMERATE				= 0x10000,
		OM_DEBUGMMODE				= 0x10001,
		OM_DISABLEFADEOUT			= 0x10002,
		OM_DONTMISSNOTES			= 0x10003,

		OM_ENABLESFX				= 0x10004,
		OM_FULLVELOCITY				= 0x10005,
		OM_IGNOREVELOCITYRANGE		= 0x10006,
		OM_IGNOREALLEVENTS			= 0x10007,
		OM_IGNORESYSEX				= 0x10008,
		OM_IGNORESYSRESET			= 0x10009,
		OM_LIMITRANGETO88			= 0x10010,
		OM_MT32MODE					= 0x10011,
		OM_MONORENDERING			= 0x10012,
		OM_NOTEOFF1					= 0x10013,
		OM_EVENTPROCWITHAUDIO		= 0x10014,
		OM_SINCINTER				= 0x10015,
		OM_SLEEPSTATES				= 0x10016,

		OM_AUDIOBITDEPTH			= 0x10017,
		OM_AUDIOFREQ				= 0x10018,
		OM_CURRENTENGINE			= 0x10019,
		OM_BUFFERLENGTH				= 0x10020,
		OM_MAXRENDERINGTIME			= 0x10021,
		OM_MINIGNOREVELRANGE		= 0x10022,
		OM_MAXIGNOREVELRANGE		= 0x10023,
		OM_OUTPUTVOLUME				= 0x10024,
		OM_TRANSPOSE				= 0x10025,
		OM_MAXVOICES				= 0x10026,
		OM_SINCINTERCONV			= 0x10027,

		OM_OVERRIDENOTELENGTH		= 0x10028,
		OM_NOTELENGTH				= 0x10029,
		OM_ENABLEDELAYNOTEOFF		= 0x10030,
		OM_DELAYNOTEOFFVAL			= 0x10031
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
	public static extern bool ReturnKDMAPIVer(out Int32 Major, out Int32 Minor, out Int32 Build, out Int32 Revision);	
			
	[DllImport("OmniMIDI.dll")]
	public static extern bool IsKDMAPIAvailable();
			
	[DllImport("OmniMIDI.dll")]
	public static extern int InitializeKDMAPIStream();
			
	[DllImport("OmniMIDI.dll")]
	public static extern int TerminateKDMAPIStream();
			
	[DllImport("OmniMIDI.dll")]
	public static extern void ResetKDMAPIStream();
			
	[DllImport("OmniMIDI.dll")]
	public static extern uint SendCustomEvent(uint eventtype, uint chan, uint param);
	
	[DllImport("OmniMIDI.dll")]
	public static extern void SendDirectData(uint dwMsg);

	[DllImport("OmniMIDI.dll")]
	public static extern uint SendDirectLongData(ref MIDIHDR IIMidiHdr);
			
	[DllImport("OmniMIDI.dll")]
	public static extern uint SendDirectLongDataNoBuf(ref MIDIHDR IIMidiHdr);
			
	[DllImport("OmniMIDI.dll")]
	public static extern uint PrepareLongData(ref MIDIHDR IIMidiHdr);

	[DllImport("OmniMIDI.dll")]
	public static extern uint UnprepareLongData(ref MIDIHDR IIMidiHdr);
			
	[DllImport("OmniMIDI.dll")]
	public static extern bool DriverSettings(OMSetting Setting, OMSettingMode Mode, IntPtr Value, Int32 cbValue);
			
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