/*****************************************************************************
*  BASSXA C/C++ Header
*****************************************************************************/

class sound_out
{
public:
	virtual ~sound_out() {}

	virtual const char* open(void * hwnd, unsigned sample_rate, unsigned short nch, bool floating_point, unsigned max_samples_per_frame, unsigned num_frames) = 0;

	virtual const char* write_frame(void * buffer, unsigned num_samples, bool wait) = 0;

	virtual const char* set_ratio(double ratio) = 0;

	virtual const char* pause(bool pausing) = 0;

	virtual double buffered() = 0;
};

sound_out * create_sound_out_xaudio2();

#ifndef BASSXA_H
#define BASSXA_H

#ifndef BASS_H
#include "bass.h"
#endif

#ifdef __cplusplus
extern "C" {
#endif

#ifndef BASSXADEF
#define BASSXADEF(f) WINAPI f
#endif

#ifndef BASSXASCOPE
#define BASSXASCOPE
#endif

	/* If you load the DLL using LoadLibrary() instead of using bassxa.lib,
	* you can define the functions as pointers by setting BASSXADEF(f) to
	* "(WINAPI *f)".  As you should do this only once, you can define
	* BASSXASCOPE to "extern" for subsequent includes.  Of course, all this must
	* be done before including bassxa.h!
	*/

	BASSXASCOPE sound_out * BASSXADEF(BASSXA_CreateAudioStream)();
	BASSXASCOPE BOOL BASSXADEF(BASSXA_InitializeAudioStream)(sound_out * stream, int frequency, int channels, bool floataudio, int sndbf, int frames);
	BASSXASCOPE void BASSXADEF(BASSXA_WriteFrame)(sound_out * stream, void* buffer, unsigned int data, bool wait);
	BASSXASCOPE void BASSXADEF(BASSXA_TerminateAudioStream)(sound_out * stream);
	BASSXASCOPE DWORD BASSXADEF(BASSXA_GetVersion)();

#define BASSXA_STEREO			2		/* Stereo Stream */
#define BASSXA_MONO			    1		/* Mono Stream */
#define BASSXA_FRAMEWAIT		TRUE	/* Pause, to reduce CPU usage */
#define BASSXA_FRAMEGO		    FALSE	/* Do not pause, less latency */
#define BASSXA_FLOAT			TRUE	/* Float Stream */
#define BASSXA_INT				FALSE	/* Int Stream */

	/* If any BASSXA function fails, you can use BASS_ErrorGetCode() to obtain
	* the reason for failure.  The error codes are the one from bass.h plus the
	* error codes below.  If a function succeeded, BASS_ErrorGetCode() returns
	* BASS_OK.
	*/

#define BASSXA_ERROR_STREAM			5200 /* Error while opening stream */
#define BASSXA_ERROR_FRAME		    5201 /* Error while writing frames */
#define BASSXA_ERROR_DELETE		    5202 /* Can not delete stream */

#ifdef __cplusplus
}
#endif

#endif /* BASSXA_H */