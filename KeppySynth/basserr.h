/*
Keppy's Synthesizer errors list
*/

static TCHAR * errname[48] =
{
	L"BASS_ERROR_UNKNOWN",		// Error -1
	L"BASS_OK",					// Error 0
	L"BASS_ERROR_MEM",			// Error 1
	L"BASS_ERROR_FILEOPEN",		// Error 2
	L"BASS_ERROR_DRIVER",		// Error 3
	L"BASS_ERROR_BUFLOST",		// Error 4
	L"BASS_ERROR_HANDLE",		// Error 5
	L"BASS_ERROR_FORMAT",		// Error 6
	L"BASS_ERROR_POSITION",		// Error 7
	L"BASS_ERROR_INIT",			// Error 8
	L"BASS_ERROR_START",		// Error 9
	L"BASS_ERROR_SSL",			// Error 10
	L"Unknown",					// Error 11
	L"Unknown",					// Error 12
	L"Unknown",					// Error 13
	L"BASS_ERROR_ALREADY",		// Error 14
	L"Unknown",					// Error 15
	L"Unknown",					// Error 16
	L"Unknown",					// Error 17
	L"BASS_ERROR_NOCHAN",		// Error 18
	L"BASS_ERROR_ILLTYPE",		// Error 19
	L"BASS_ERROR_ILLPARAM",		// Error 20
	L"BASS_ERROR_NO3D",			// Error 21
	L"BASS_ERROR_NOEAX",		// Error 22
	L"BASS_ERROR_DEVICE",		// Error 23
	L"BASS_ERROR_NOPLAY",		// Error 24
	L"BASS_ERROR_FREQ",			// Error 25
	L"Unknown",					// Error 26
	L"BASS_ERROR_NOTFILE",		// Error 27
	L"Unknown",					// Error 28
	L"BASS_ERROR_NOHW",			// Error 29
	L"Unknown",					// Error 30
	L"BASS_ERROR_EMPTY",		// Error 31
	L"BASS_ERROR_NONET",		// Error 32
	L"BASS_ERROR_CREATE",		// Error 33
	L"BASS_ERROR_NOFX",			// Error 34
	L"BASS_ERROR_PLAYING",		// Error 35
	L"Unknown",					// Error 36
	L"BASS_ERROR_NOTAVAIL",		// Error 37
	L"BASS_ERROR_DECODE",		// Error 38
	L"BASS_ERROR_DX",			// Error 39
	L"BASS_ERROR_TIMEOUT",		// Error 40
	L"BASS_ERROR_FILEFORM",		// Error 41
	L"BASS_ERROR_SPEAKER",		// Error 42
	L"BASS_ERROR_VERSION",		// Error 43
	L"BASS_ERROR_CODEC",		// Error 44
	L"BASS_ERROR_ENDED",		// Error 45
	L"BASS_ERROR_BUSY",			// Error 46
};

static TCHAR * errdesc[48] =
{
	L"Unknown error.",																							// Error -1
	L"Nothing's wrong.",																						// Error 0
	L"There is insufficient memory.",																			// Error 1
	L"The file could not be opened.",																			// Error 2
	L"There is no available device driver. The device may already be in use.",									// Error 3
	L"The sample buffer was lost.",																				// Error 4
	L"An invalid handle has been used.",																		// Error 5
	L"The sample format is not supported by the device/drivers.",												// Error 6
	L"The requested position is invalid, eg. it is beyond the end or the download has not yet reached it.",		// Error 7
	L"BASS_Init has not been successfully called.",																// Error 8
	L"BASS_Start has not been successfully called.",															// Error 9
	L"SSL/HTTPS support isn't available.",																		// Error 10
	L"No description available.",																				// Error 11
	L"No description available.",																				// Error 12
	L"No description available.",																				// Error 13
	L"Stream already initialized.",																				// Error 14
	L"No description available.",																				// Error 15
	L"No description available.",																				// Error 16
	L"No description available.",																				// Error 17
	L"Can't get a free channel.",																				// Error 18
	L"An illegal type was specified.",																			// Error 19
	L"An illegal parameter was specified.",																		// Error 20
	L"No 3D support.",																							// Error 21
	L"No EAX support.",																							// Error 22
	L"Illegal device number.",																					// Error 23
	L"The stream is not playing.",																				// Error 24
	L"Illegal sample rate.",																					// Error 25
	L"No description available.",																				// Error 26
	L"The stream is not a file stream.",																		// Error 27
	L"No description available.",																				// Error 28
	L"No hardware voices available.",																			// Error 29
	L"No description available.",																				// Error 30
	L"The MOD music has no sequence data.",																		// Error 31
	L"No internet connection could be opened.",																	// Error 32
	L"Couldn't create the file.",																				// Error 33
	L"Effects are not available.",																				// Error 34
	L"The channel is playing."																					// Error 35
	L"No description available.",																				// Error 36
	L"Requested data is not available.",																		// Error 37
	L"The channel is a \"decoding channel\".",																	// Error 38
	L"A sufficient DirectX version is not installed.",															// Error 39
	L"Connection timed out.",																					// Error 40
	L"Unsupported file format.",																				// Error 41
	L"Unavailable speaker.",																					// Error 42
	L"Invalid BASS version.",																					// Error 43
	L"Codec is not available/supported.",																		// Error 44
	L"The channel/file has ended.",																				// Error 45
	L"The device is busy (eg. in 'exclusive' use by another process).",											// Error 46
};