/*

	OmniMIDI v15+ (Rewrite) for Windows NT

	This file contains the required code to run the driver under Windows 7 SP1 and later.
	This file is useful only if you want to compile the driver under Windows, it's not needed for Linux/macOS porting.

*/

#include <bass\bass.h>

BOOL(WINAPI* BASS_SetConfig)(DWORD option, DWORD value) = 0;
DWORD(WINAPI* BASS_GetConfig)(DWORD option) = 0;
BOOL(WINAPI* BASS_SetConfigPtr)(DWORD option, const void* value) = 0;
const void* (WINAPI* BASS_GetConfigPtr)(DWORD option) = 0;
DWORD(WINAPI* BASS_GetVersion)(void) = 0;
int (WINAPI* BASS_ErrorGetCode)(void) = 0;

BOOL(WINAPI* BASS_GetDeviceInfo)(DWORD device, BASS_DEVICEINFO* info) = 0;
#if defined(_WIN32) && !defined(_WIN32_WCE) && !(defined(WINAPI_FAMILY) && WINAPI_FAMILY != WINAPI_FAMILY_DESKTOP_APP)
BOOL(WINAPI* BASS_Init)(int device, DWORD freq, DWORD flags, HWND win, const void* dsguid) = 0;
#else
BOOL(WINAPI* BASS_Init)(int device, DWORD freq, DWORD flags, void* win, const void* dsguid) = 0;
#endif
BOOL(WINAPI* BASS_Free)(void) = 0;
BOOL(WINAPI* BASS_SetDevice)(DWORD device) = 0;
DWORD(WINAPI* BASS_GetDevice)(void) = 0;
BOOL(WINAPI* BASS_GetInfo)(BASS_INFO* info) = 0;
BOOL(WINAPI* BASS_Start)(void) = 0;
BOOL(WINAPI* BASS_Stop)(void) = 0;
BOOL(WINAPI* BASS_Pause)(void) = 0;
DWORD(WINAPI* BASS_IsStarted)(void) = 0;
BOOL(WINAPI* BASS_Update)(DWORD length) = 0;
float (WINAPI* BASS_GetCPU)(void) = 0;
BOOL(WINAPI* BASS_SetVolume)(float volume) = 0;
float (WINAPI* BASS_GetVolume)(void) = 0;
#if defined(_WIN32) && !defined(_WIN32_WCE) && !(defined(WINAPI_FAMILY) && WINAPI_FAMILY != WINAPI_FAMILY_DESKTOP_APP)
void* (WINAPI* BASS_GetDSoundObject)(DWORD object) = 0;
#endif

BOOL(WINAPI* BASS_Set3DFactors)(float distf, float rollf, float doppf) = 0;
BOOL(WINAPI* BASS_Get3DFactors)(float* distf, float* rollf, float* doppf) = 0;
BOOL(WINAPI* BASS_Set3DPosition)(const BASS_3DVECTOR* pos, const BASS_3DVECTOR* vel, const BASS_3DVECTOR* front, const BASS_3DVECTOR* top) = 0;
BOOL(WINAPI* BASS_Get3DPosition)(BASS_3DVECTOR* pos, BASS_3DVECTOR* vel, BASS_3DVECTOR* front, BASS_3DVECTOR* top) = 0;
void (WINAPI* BASS_Apply3D)(void) = 0;

HPLUGIN(WINAPI* BASS_PluginLoad)(const char* file, DWORD flags) = 0;
BOOL(WINAPI* BASS_PluginFree)(HPLUGIN handle) = 0;
BOOL(WINAPI* BASS_PluginEnable)(HPLUGIN handle, BOOL enable) = 0;
const BASS_PLUGININFO* (WINAPI* BASS_PluginGetInfo)(HPLUGIN handle) = 0;

HSAMPLE(WINAPI* BASS_SampleLoad)(BOOL mem, const void* file, QWORD offset, DWORD length, DWORD max, DWORD flags) = 0;
HSAMPLE(WINAPI* BASS_SampleCreate)(DWORD length, DWORD freq, DWORD chans, DWORD max, DWORD flags) = 0;
BOOL(WINAPI* BASS_SampleFree)(HSAMPLE handle) = 0;
BOOL(WINAPI* BASS_SampleSetData)(HSAMPLE handle, const void* buffer) = 0;
BOOL(WINAPI* BASS_SampleGetData)(HSAMPLE handle, void* buffer) = 0;
BOOL(WINAPI* BASS_SampleGetInfo)(HSAMPLE handle, BASS_SAMPLE* info) = 0;
BOOL(WINAPI* BASS_SampleSetInfo)(HSAMPLE handle, const BASS_SAMPLE* info) = 0;
DWORD(WINAPI* BASS_SampleGetChannel)(HSAMPLE handle, DWORD flags) = 0;
DWORD(WINAPI* BASS_SampleGetChannels)(HSAMPLE handle, HCHANNEL* channels) = 0;
BOOL(WINAPI* BASS_SampleStop)(HSAMPLE handle) = 0;

HSTREAM(WINAPI* BASS_StreamCreate)(DWORD freq, DWORD chans, DWORD flags, STREAMPROC* proc, void* user) = 0;
HSTREAM(WINAPI* BASS_StreamCreateFile)(BOOL mem, const void* file, QWORD offset, QWORD length, DWORD flags) = 0;
HSTREAM(WINAPI* BASS_StreamCreateURL)(const char* url, DWORD offset, DWORD flags, DOWNLOADPROC* proc, void* user) = 0;
HSTREAM(WINAPI* BASS_StreamCreateFileUser)(DWORD system, DWORD flags, const BASS_FILEPROCS* proc, void* user) = 0;
BOOL(WINAPI* BASS_StreamFree)(HSTREAM handle) = 0;
QWORD(WINAPI* BASS_StreamGetFilePosition)(HSTREAM handle, DWORD mode) = 0;
DWORD(WINAPI* BASS_StreamPutData)(HSTREAM handle, const void* buffer, DWORD length) = 0;
DWORD(WINAPI* BASS_StreamPutFileData)(HSTREAM handle, const void* buffer, DWORD length) = 0;

HMUSIC(WINAPI* BASS_MusicLoad)(BOOL mem, const void* file, QWORD offset, DWORD length, DWORD flags, DWORD freq) = 0;
BOOL(WINAPI* BASS_MusicFree)(HMUSIC handle) = 0;

BOOL(WINAPI* BASS_RecordGetDeviceInfo)(DWORD device, BASS_DEVICEINFO* info) = 0;
BOOL(WINAPI* BASS_RecordInit)(int device) = 0;
BOOL(WINAPI* BASS_RecordFree)(void) = 0;
BOOL(WINAPI* BASS_RecordSetDevice)(DWORD device) = 0;
DWORD(WINAPI* BASS_RecordGetDevice)(void) = 0;
BOOL(WINAPI* BASS_RecordGetInfo)(BASS_RECORDINFO* info) = 0;
const char* (WINAPI* BASS_RecordGetInputName)(int input) = 0;
BOOL(WINAPI* BASS_RecordSetInput)(int input, DWORD flags, float volume) = 0;
DWORD(WINAPI* BASS_RecordGetInput)(int input, float* volume) = 0;
HRECORD(WINAPI* BASS_RecordStart)(DWORD freq, DWORD chans, DWORD flags, RECORDPROC* proc, void* user) = 0;

double (WINAPI* BASS_ChannelBytes2Seconds)(DWORD handle, QWORD pos) = 0;
QWORD(WINAPI* BASS_ChannelSeconds2Bytes)(DWORD handle, double pos) = 0;
DWORD(WINAPI* BASS_ChannelGetDevice)(DWORD handle) = 0;
BOOL(WINAPI* BASS_ChannelSetDevice)(DWORD handle, DWORD device) = 0;
DWORD(WINAPI* BASS_ChannelIsActive)(DWORD handle) = 0;
BOOL(WINAPI* BASS_ChannelGetInfo)(DWORD handle, BASS_CHANNELINFO* info) = 0;
const char* (WINAPI* BASS_ChannelGetTags)(DWORD handle, DWORD tags) = 0;
DWORD(WINAPI* BASS_ChannelFlags)(DWORD handle, DWORD flags, DWORD mask) = 0;
BOOL(WINAPI* BASS_ChannelLock)(DWORD handle, BOOL lock) = 0;
BOOL(WINAPI* BASS_ChannelFree)(DWORD handle) = 0;
BOOL(WINAPI* BASS_ChannelPlay)(DWORD handle, BOOL restart) = 0;
BOOL(WINAPI* BASS_ChannelStop)(DWORD handle) = 0;
BOOL(WINAPI* BASS_ChannelPause)(DWORD handle) = 0;
BOOL(WINAPI* BASS_ChannelUpdate)(DWORD handle, DWORD length) = 0;
BOOL(WINAPI* BASS_ChannelSetAttribute)(DWORD handle, DWORD attrib, float value) = 0;
BOOL(WINAPI* BASS_ChannelGetAttribute)(DWORD handle, DWORD attrib, float* value) = 0;
BOOL(WINAPI* BASS_ChannelSlideAttribute)(DWORD handle, DWORD attrib, float value, DWORD time) = 0;
BOOL(WINAPI* BASS_ChannelIsSliding)(DWORD handle, DWORD attrib) = 0;
BOOL(WINAPI* BASS_ChannelSetAttributeEx)(DWORD handle, DWORD attrib, void* value, DWORD size) = 0;
DWORD(WINAPI* BASS_ChannelGetAttributeEx)(DWORD handle, DWORD attrib, void* value, DWORD size) = 0;
BOOL(WINAPI* BASS_ChannelSet3DAttributes)(DWORD handle, int mode, float min, float max, int iangle, int oangle, float outvol) = 0;
BOOL(WINAPI* BASS_ChannelGet3DAttributes)(DWORD handle, DWORD* mode, float* min, float* max, DWORD* iangle, DWORD* oangle, float* outvol) = 0;
BOOL(WINAPI* BASS_ChannelSet3DPosition)(DWORD handle, const BASS_3DVECTOR* pos, const BASS_3DVECTOR* orient, const BASS_3DVECTOR* vel) = 0;
BOOL(WINAPI* BASS_ChannelGet3DPosition)(DWORD handle, BASS_3DVECTOR* pos, BASS_3DVECTOR* orient, BASS_3DVECTOR* vel) = 0;
QWORD(WINAPI* BASS_ChannelGetLength)(DWORD handle, DWORD mode) = 0;
BOOL(WINAPI* BASS_ChannelSetPosition)(DWORD handle, QWORD pos, DWORD mode) = 0;
QWORD(WINAPI* BASS_ChannelGetPosition)(DWORD handle, DWORD mode) = 0;
DWORD(WINAPI* BASS_ChannelGetLevel)(DWORD handle) = 0;
BOOL(WINAPI* BASS_ChannelGetLevelEx)(DWORD handle, float* levels, float length, DWORD flags) = 0;
DWORD(WINAPI* BASS_ChannelGetData)(DWORD handle, void* buffer, DWORD length) = 0;
HSYNC(WINAPI* BASS_ChannelSetSync)(DWORD handle, DWORD type, QWORD param, SYNCPROC* proc, void* user) = 0;
BOOL(WINAPI* BASS_ChannelRemoveSync)(DWORD handle, HSYNC sync) = 0;
BOOL(WINAPI* BASS_ChannelSetLink)(DWORD handle, DWORD chan) = 0;
BOOL(WINAPI* BASS_ChannelRemoveLink)(DWORD handle, DWORD chan) = 0;
HDSP(WINAPI* BASS_ChannelSetDSP)(DWORD handle, DSPPROC* proc, void* user, int priority) = 0;
BOOL(WINAPI* BASS_ChannelRemoveDSP)(DWORD handle, HDSP dsp) = 0;
HFX(WINAPI* BASS_ChannelSetFX)(DWORD handle, DWORD type, int priority) = 0;
BOOL(WINAPI* BASS_ChannelRemoveFX)(DWORD handle, HFX fx) = 0;

BOOL(WINAPI* BASS_FXSetParameters)(HFX handle, const void* params) = 0;
BOOL(WINAPI* BASS_FXGetParameters)(HFX handle, void* params) = 0;
BOOL(WINAPI* BASS_FXSetPriority)(HFX handle, int priority) = 0;
BOOL(WINAPI* BASS_FXReset)(DWORD handle) = 0;