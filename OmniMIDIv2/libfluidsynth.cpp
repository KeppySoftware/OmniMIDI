/*

	OmniMIDI v15+ (Rewrite) for Windows NT

	This file contains the required code to run the driver under Windows 7 SP1 and later.

*/

#include <Windows.h>
#include <fluidsynth.h>

fluid_audio_driver_t* (WINAPI* new_fluid_audio_driver)(fluid_settings_t* settings, fluid_synth_t* synth) = 0;
fluid_audio_driver_t* (WINAPI* new_fluid_audio_driver2)(fluid_settings_t* settings, fluid_audio_func_t func, void* data) = 0;
void (WINAPI* delete_fluid_audio_driver)(fluid_audio_driver_t* driver) = 0;
int (WINAPI* fluid_audio_driver_register)(const char** adrivers) = 0;

fluid_file_renderer_t* (WINAPI* new_fluid_file_renderer)(fluid_synth_t* synth) = 0;
void (WINAPI* delete_fluid_file_renderer)(fluid_file_renderer_t* dev) = 0;

int (WINAPI* fluid_file_renderer_process_block)(fluid_file_renderer_t* dev) = 0;
int (WINAPI* fluid_file_set_encoding_quality)(fluid_file_renderer_t* dev, double q) = 0;

fluid_synth_t* (WINAPI* new_fluid_synth)(fluid_settings_t* settings) = 0;
void(WINAPI* delete_fluid_synth)(fluid_synth_t* synth) = 0;

double(WINAPI* fluid_synth_get_cpu_load)(fluid_synth_t* synth) = 0;
const char* (WINAPI* fluid_synth_error)(fluid_synth_t* synth) = 0;

int (WINAPI* fluid_synth_noteon)(fluid_synth_t* synth, int chan, int key, int vel) = 0;
int (WINAPI* fluid_synth_noteoff)(fluid_synth_t* synth, int chan, int key) = 0;
int (WINAPI* fluid_synth_cc)(fluid_synth_t* synth, int chan, int ctrl, int val) = 0;
int (WINAPI* fluid_synth_get_cc)(fluid_synth_t* synth, int chan, int ctrl, int* pval) = 0;
int (WINAPI* fluid_synth_sysex)(fluid_synth_t* synth, const char* data, int len, char* response, int* response_len, int* handled, int dryrun) = 0;
int (WINAPI* fluid_synth_pitch_bend)(fluid_synth_t* synth, int chan, int val) = 0;
int (WINAPI* fluid_synth_get_pitch_bend)(fluid_synth_t* synth, int chan, int* ppitch_bend) = 0;
int (WINAPI* fluid_synth_pitch_wheel_sens)(fluid_synth_t* synth, int chan, int val) = 0;
int (WINAPI* fluid_synth_get_pitch_wheel_sens)(fluid_synth_t* synth, int chan, int* pval) = 0;
int (WINAPI* fluid_synth_program_change)(fluid_synth_t* synth, int chan, int program) = 0;
int (WINAPI* fluid_synth_channel_pressure)(fluid_synth_t* synth, int chan, int val) = 0;
int (WINAPI* fluid_synth_key_pressure)(fluid_synth_t* synth, int chan, int key, int val) = 0;
int (WINAPI* fluid_synth_bank_select)(fluid_synth_t* synth, int chan, int bank) = 0;
int (WINAPI* fluid_synth_sfont_select)(fluid_synth_t* synth, int chan, int sfont_id) = 0;
int (WINAPI* fluid_synth_program_select)(fluid_synth_t* synth, int chan, int sfont_id, int bank_num, int preset_num) = 0;
int (WINAPI* fluid_synth_program_select_by_sfont_name)(fluid_synth_t* synth, int chan, const char* sfont_name, int bank_num, int preset_num) = 0;
int (WINAPI* fluid_synth_get_program)(fluid_synth_t* synth, int chan, int* sfont_id, int* bank_num, int* preset_num) = 0;
int (WINAPI* fluid_synth_unset_program)(fluid_synth_t* synth, int chan) = 0;
int (WINAPI* fluid_synth_program_reset)(fluid_synth_t* synth) = 0;
int (WINAPI* fluid_synth_system_reset)(fluid_synth_t* synth) = 0;

int (WINAPI* fluid_synth_all_notes_off)(fluid_synth_t* synth, int chan) = 0;
int (WINAPI* fluid_synth_all_sounds_off)(fluid_synth_t* synth, int chan) = 0;

int (WINAPI* fluid_synth_set_gen)(fluid_synth_t* synth, int chan, int param, float value) = 0;
float (WINAPI* fluid_synth_get_gen)(fluid_synth_t* synth, int chan, int param) = 0;
int (WINAPI* fluid_synth_start)(fluid_synth_t* synth, unsigned int id, fluid_preset_t* preset, int audio_chan, int midi_chan, int key, int vel) = 0;
int (WINAPI* fluid_synth_stop)(fluid_synth_t* synth, unsigned int id) = 0;

fluid_voice_t* (WINAPI* fluid_synth_alloc_voice)(fluid_synth_t* synth, fluid_sample_t* sample, int channum, int key, int vel) = 0;
void(WINAPI* fluid_synth_start_voice)(fluid_synth_t* synth, fluid_voice_t* voice) = 0;
void(WINAPI* fluid_synth_get_voicelist)(fluid_synth_t* synth, fluid_voice_t* buf[], int bufsize, int ID) = 0;

int (WINAPI* fluid_synth_sfload)(fluid_synth_t* synth, const char* filename, int reset_presets) = 0;
int (WINAPI* fluid_synth_sfreload)(fluid_synth_t* synth, int id) = 0;
int (WINAPI* fluid_synth_sfunload)(fluid_synth_t* synth, int id, int reset_presets) = 0;
int (WINAPI* fluid_synth_add_sfont)(fluid_synth_t* synth, fluid_sfont_t* sfont) = 0;
int (WINAPI* fluid_synth_remove_sfont)(fluid_synth_t* synth, fluid_sfont_t* sfont) = 0;
int (WINAPI* fluid_synth_sfcount)(fluid_synth_t* synth) = 0;
fluid_sfont_t* (WINAPI* fluid_synth_get_sfont)(fluid_synth_t* synth, unsigned int num) = 0;
fluid_sfont_t* (WINAPI* fluid_synth_get_sfont_by_id)(fluid_synth_t* synth, int id) = 0;
fluid_sfont_t* (WINAPI* fluid_synth_get_sfont_by_name)(fluid_synth_t* synth, const char* name) = 0;
int (WINAPI* fluid_synth_set_bank_offset)(fluid_synth_t* synth, int sfont_id, int offset) = 0;
int (WINAPI* fluid_synth_get_bank_offset)(fluid_synth_t* synth, int sfont_id) = 0;

void(WINAPI* fluid_synth_set_reverb_on)(fluid_synth_t* synth, int on) = 0;
int (WINAPI* fluid_synth_reverb_on)(fluid_synth_t* synth, int fx_group, int on) = 0;

int (WINAPI* fluid_synth_set_reverb)(fluid_synth_t* synth, double roomsize, double damping, double width, double level) = 0;
int (WINAPI* fluid_synth_set_reverb_roomsize)(fluid_synth_t* synth, double roomsize) = 0;
int (WINAPI* fluid_synth_set_reverb_damp)(fluid_synth_t* synth, double damping) = 0;
int (WINAPI* fluid_synth_set_reverb_width)(fluid_synth_t* synth, double width) = 0;
int (WINAPI* fluid_synth_set_reverb_level)(fluid_synth_t* synth, double level) = 0;

double(WINAPI* fluid_synth_get_reverb_roomsize)(fluid_synth_t* synth) = 0;
double(WINAPI* fluid_synth_get_reverb_damp)(fluid_synth_t* synth) = 0;
double(WINAPI* fluid_synth_get_reverb_level)(fluid_synth_t* synth) = 0;
double(WINAPI* fluid_synth_get_reverb_width)(fluid_synth_t* synth) = 0;

int (WINAPI* fluid_synth_set_reverb_group_roomsize)(fluid_synth_t* synth, int fx_group, double roomsize) = 0;
int (WINAPI* fluid_synth_set_reverb_group_damp)(fluid_synth_t* synth, int fx_group, double damping) = 0;
int (WINAPI* fluid_synth_set_reverb_group_width)(fluid_synth_t* synth, int fx_group, double width) = 0;
int (WINAPI* fluid_synth_set_reverb_group_level)(fluid_synth_t* synth, int fx_group, double level) = 0;

int (WINAPI* fluid_synth_get_reverb_group_roomsize)(fluid_synth_t* synth, int fx_group, double* roomsize) = 0;
int (WINAPI* fluid_synth_get_reverb_group_damp)(fluid_synth_t* synth, int fx_group, double* damping) = 0;
int (WINAPI* fluid_synth_get_reverb_group_width)(fluid_synth_t* synth, int fx_group, double* width) = 0;
int (WINAPI* fluid_synth_get_reverb_group_level)(fluid_synth_t* synth, int fx_group, double* level) = 0;
void(WINAPI* fluid_synth_set_chorus_on)(fluid_synth_t* synth, int on) = 0;
int (WINAPI* fluid_synth_chorus_on)(fluid_synth_t* synth, int fx_group, int on) = 0;

int (WINAPI* fluid_synth_set_chorus)(fluid_synth_t* synth, int nr, double level, double speed, double depth_ms, int type) = 0;
int (WINAPI* fluid_synth_set_chorus_nr)(fluid_synth_t* synth, int nr) = 0;
int (WINAPI* fluid_synth_set_chorus_level)(fluid_synth_t* synth, double level) = 0;
int (WINAPI* fluid_synth_set_chorus_speed)(fluid_synth_t* synth, double speed) = 0;
int (WINAPI* fluid_synth_set_chorus_depth)(fluid_synth_t* synth, double depth_ms) = 0;
int (WINAPI* fluid_synth_set_chorus_type)(fluid_synth_t* synth, int type) = 0;

int (WINAPI* fluid_synth_get_chorus_nr)(fluid_synth_t* synth) = 0;
double(WINAPI* fluid_synth_get_chorus_level)(fluid_synth_t* synth) = 0;
double(WINAPI* fluid_synth_get_chorus_speed)(fluid_synth_t* synth) = 0;
double(WINAPI* fluid_synth_get_chorus_depth)(fluid_synth_t* synth) = 0;
int (WINAPI* fluid_synth_get_chorus_type)(fluid_synth_t* synth) = 0; /* see fluid_chorus_mod */

int (WINAPI* fluid_synth_set_chorus_group_nr)(fluid_synth_t* synth, int fx_group, int nr) = 0;
int (WINAPI* fluid_synth_set_chorus_group_level)(fluid_synth_t* synth, int fx_group, double level) = 0;
int (WINAPI* fluid_synth_set_chorus_group_speed)(fluid_synth_t* synth, int fx_group, double speed) = 0;
int (WINAPI* fluid_synth_set_chorus_group_depth)(fluid_synth_t* synth, int fx_group, double depth_ms) = 0;
int (WINAPI* fluid_synth_set_chorus_group_type)(fluid_synth_t* synth, int fx_group, int type) = 0;

int (WINAPI* fluid_synth_get_chorus_group_nr)(fluid_synth_t* synth, int fx_group, int* nr) = 0;
int (WINAPI* fluid_synth_get_chorus_group_level)(fluid_synth_t* synth, int fx_group, double* level) = 0;
int (WINAPI* fluid_synth_get_chorus_group_speed)(fluid_synth_t* synth, int fx_group, double* speed) = 0;
int (WINAPI* fluid_synth_get_chorus_group_depth)(fluid_synth_t* synth, int fx_group, double* depth_ms) = 0;
int (WINAPI* fluid_synth_get_chorus_group_type)(fluid_synth_t* synth, int fx_group, int* type) = 0;

int (WINAPI* fluid_synth_count_midi_channels)(fluid_synth_t* synth) = 0;
int (WINAPI* fluid_synth_count_audio_channels)(fluid_synth_t* synth) = 0;
int (WINAPI* fluid_synth_count_audio_groups)(fluid_synth_t* synth) = 0;
int (WINAPI* fluid_synth_count_effects_channels)(fluid_synth_t* synth) = 0;
int (WINAPI* fluid_synth_count_effects_groups)(fluid_synth_t* synth) = 0;

void(WINAPI* fluid_synth_set_sample_rate)(fluid_synth_t* synth, float sample_rate) = 0;
void(WINAPI* fluid_synth_set_gain)(fluid_synth_t* synth, float gain) = 0;
float (WINAPI* fluid_synth_get_gain)(fluid_synth_t* synth) = 0;
int (WINAPI* fluid_synth_set_polyphony)(fluid_synth_t* synth, int polyphony) = 0;
int (WINAPI* fluid_synth_get_polyphony)(fluid_synth_t* synth) = 0;
int (WINAPI* fluid_synth_get_active_voice_count)(fluid_synth_t* synth) = 0;
int (WINAPI* fluid_synth_get_internal_bufsize)(fluid_synth_t* synth) = 0;

int (WINAPI* fluid_synth_set_interp_method)(fluid_synth_t* synth, int chan, int interp_method) = 0;
int (WINAPI* fluid_synth_add_default_mod)(fluid_synth_t* synth, const fluid_mod_t* mod, int mode) = 0;
int (WINAPI* fluid_synth_remove_default_mod)(fluid_synth_t* synth, const fluid_mod_t* mod) = 0;
int(WINAPI* fluid_synth_activate_key_tuning)(fluid_synth_t* synth, int bank, int prog, const char* name, const double* pitch, int apply) = 0;
int(WINAPI* fluid_synth_activate_octave_tuning)(fluid_synth_t* synth, int bank, int prog, const char* name, const double* pitch, int apply) = 0;
int(WINAPI* fluid_synth_tune_notes)(fluid_synth_t* synth, int bank, int prog, int len, const int* keys, const double* pitch, int apply) = 0;
int(WINAPI* fluid_synth_activate_tuning)(fluid_synth_t* synth, int chan, int bank, int prog, int apply) = 0;
int(WINAPI* fluid_synth_deactivate_tuning)(fluid_synth_t* synth, int chan, int apply) = 0;
void(WINAPI* fluid_synth_tuning_iteration_start)(fluid_synth_t* synth) = 0;
int(WINAPI* fluid_synth_tuning_iteration_next)(fluid_synth_t* synth, int* bank, int* prog) = 0;
int (WINAPI* fluid_synth_tuning_dump)(fluid_synth_t* synth, int bank, int prog, char* name, int len, double* pitch) = 0;
int (WINAPI* fluid_synth_write_s16)(fluid_synth_t* synth, int len, void* lout, int loff, int lincr, void* rout, int roff, int rincr) = 0;
int (WINAPI* fluid_synth_write_float)(fluid_synth_t* synth, int len, void* lout, int loff, int lincr, void* rout, int roff, int rincr) = 0;
int (WINAPI* fluid_synth_nwrite_float)(fluid_synth_t* synth, int len, float** left, float** right, float** fx_left, float** fx_right) = 0;
int (WINAPI* fluid_synth_process)(fluid_synth_t* synth, int len, int nfx, float* fx[], int nout, float* out[]) = 0;
int (WINAPI* fluid_synth_set_custom_filter)(fluid_synth_t*, int type, int flags) = 0;
int (WINAPI* fluid_synth_set_channel_type)(fluid_synth_t* synth, int chan, int type) = 0;
int (WINAPI* fluid_synth_reset_basic_channel)(fluid_synth_t* synth, int chan) = 0;
int (WINAPI* fluid_synth_get_basic_channel)(fluid_synth_t* synth, int chan, int* basic_chan_out, int* mode_chan_out, int* basic_val_out) = 0;
int (WINAPI* fluid_synth_set_basic_channel)(fluid_synth_t* synth, int chan, int mode, int val) = 0;
int (WINAPI* fluid_synth_set_legato_mode)(fluid_synth_t* synth, int chan, int legatomode) = 0;
int (WINAPI* fluid_synth_get_legato_mode)(fluid_synth_t* synth, int chan, int* legatomode) = 0;
int (WINAPI* fluid_synth_set_portamento_mode)(fluid_synth_t* synth, int chan, int portamentomode) = 0;
int (WINAPI* fluid_synth_get_portamento_mode)(fluid_synth_t* synth, int chan, int* portamentomode) = 0;
int (WINAPI* fluid_synth_set_breath_mode)(fluid_synth_t* synth, int chan, int breathmode) = 0;
int (WINAPI* fluid_synth_get_breath_mode)(fluid_synth_t* synth, int chan, int* breathmode) = 0;
fluid_settings_t* (WINAPI* fluid_synth_get_settings)(fluid_synth_t* synth) = 0;
void(WINAPI* fluid_synth_add_sfloader)(fluid_synth_t* synth, fluid_sfloader_t* loader) = 0;
fluid_preset_t* (WINAPI* fluid_synth_get_channel_preset)(fluid_synth_t* synth, int chan) = 0;
int (WINAPI* fluid_synth_handle_midi_event)(void* data, fluid_midi_event_t* event) = 0;
int(WINAPI* fluid_synth_pin_preset)(fluid_synth_t* synth, int sfont_id, int bank_num, int preset_num) = 0;
int(WINAPI* fluid_synth_unpin_preset)(fluid_synth_t* synth, int sfont_id, int bank_num, int preset_num) = 0;
fluid_ladspa_fx_t* (WINAPI* fluid_synth_get_ladspa_fx)(fluid_synth_t* synth) = 0;

fluid_settings_t* (WINAPI* new_fluid_settings)(void) = 0;
void (WINAPI* delete_fluid_settings)(fluid_settings_t* settings) = 0;
int (WINAPI* fluid_settings_get_type)(fluid_settings_t* settings, const char* name) = 0;
int (WINAPI* fluid_settings_get_hints)(fluid_settings_t* settings, const char* name, int* val) = 0;
int (WINAPI* fluid_settings_is_realtime)(fluid_settings_t* settings, const char* name) = 0;
int (WINAPI* fluid_settings_setstr)(fluid_settings_t* settings, const char* name, const char* str) = 0;
int (WINAPI* fluid_settings_copystr)(fluid_settings_t* settings, const char* name, char* str, int len) = 0;
int (WINAPI* fluid_settings_dupstr)(fluid_settings_t* settings, const char* name, char** str) = 0;
int (WINAPI* fluid_settings_getstr_default)(fluid_settings_t* settings, const char* name, char** def) = 0;
int (WINAPI* fluid_settings_str_equal)(fluid_settings_t* settings, const char* name, const char* value) = 0;
int (WINAPI* fluid_settings_setnum)(fluid_settings_t* settings, const char* name, double val) = 0;
int (WINAPI* fluid_settings_getnum)(fluid_settings_t* settings, const char* name, double* val) = 0;
int (WINAPI* fluid_settings_getnum_default)(fluid_settings_t* settings, const char* name, double* val) = 0;
int (WINAPI* fluid_settings_getnum_range)(fluid_settings_t* settings, const char* name, double* min, double* max) = 0;
int (WINAPI* fluid_settings_setint)(fluid_settings_t* settings, const char* name, int val) = 0;
int (WINAPI* fluid_settings_getint)(fluid_settings_t* settings, const char* name, int* val) = 0;
int (WINAPI* fluid_settings_getint_default)(fluid_settings_t* settings, const char* name, int* val) = 0;
int (WINAPI* fluid_settings_getint_range)(fluid_settings_t* settings, const char* name, int* min, int* max) = 0;
void (WINAPI* fluid_settings_foreach_option)(fluid_settings_t* settings, const char* name, void* data, fluid_settings_foreach_option_t func) = 0;
int (WINAPI* fluid_settings_option_count)(fluid_settings_t* settings, const char* name) = 0;
char* (WINAPI* fluid_settings_option_concat)(fluid_settings_t* settings, const char* name, const char* separator) = 0;
void (WINAPI* fluid_settings_foreach)(fluid_settings_t* settings, void* data, fluid_settings_foreach_t func) = 0;