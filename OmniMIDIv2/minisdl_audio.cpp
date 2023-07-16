#include <tsf/minisdl_audio.h>

int (SDLCALL* SDL_AudioInit)(const char* driver_name) = 0;
void (SDLCALL* SDL_AudioQuit)(void) = 0;
SDL_AudioDeviceID(SDLCALL* SDL_OpenAudioDevice)(const char* device, int iscapture, const SDL_AudioSpec* desired, SDL_AudioSpec* obtained, int allowed_changes) = 0;
void (SDLCALL* SDL_PauseAudioDevice)(SDL_AudioDeviceID dev, int pause_on) = 0;
void (SDLCALL* SDL_CloseAudioDevice)(SDL_AudioDeviceID dev) = 0;
SDL_mutex* (WINAPI* SDL_CreateMutex)(void) = 0;
void (WINAPI* SDL_DestroyMutex)(SDL_mutex*) = 0;
int (SDLCALL* SDL_LockMutex)(SDL_mutex* mutex) = 0;
int (SDLCALL* SDL_UnlockMutex)(SDL_mutex* mutex) = 0;