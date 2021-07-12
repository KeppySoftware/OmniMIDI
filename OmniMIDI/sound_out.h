#ifndef _sound_out_h_
#define _sound_out_h_

class sound_out
{
public:
	virtual ~sound_out() {}

	virtual const char* open(void* hwnd, unsigned sample_rate, unsigned short nch, bool floating_point, unsigned max_samples_per_frame, unsigned num_frames) = 0;
	
	virtual const char* free() = 0;

	virtual const char* write_frame(void* buffer, unsigned num_samples, bool wait = true) = 0;

	virtual const char* set_ratio(double ratio) = 0;

	virtual const char* pause(bool pausing) = 0;

	virtual double buffered() = 0;
};

sound_out* create_sound_out_xaudio2();

#endif