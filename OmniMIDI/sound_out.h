#ifndef _sound_out_h_
#define _sound_out_h_

class sound_out
{
public:
	virtual ~sound_out() {}

	virtual const char* OpenStream(void* hwnd, unsigned sample_rate, unsigned short nch, bool floating_point, unsigned max_samples_per_frame, unsigned num_frames) = 0;
	
	virtual const char* WriteFrame(void* buffer, unsigned num_samples) = 0;
};

sound_out* CreateXAudio2Stream();

#endif