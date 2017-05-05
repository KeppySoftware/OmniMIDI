/*
Keppy's Synthesizer errors list
*/

static TCHAR * basserrc[95] =
{
	L"BASS_ERROR_UNKNOWN",																						// Error -1
	L"BASS_OK",																									// Error 0
	L"BASS_ERROR_MEM",																							// Error 1
	L"BASS_ERROR_FILEOPEN",																						// Error 2
	L"BASS_ERROR_DRIVER",																						// Error 3
	L"BASS_ERROR_BUFLOST",																						// Error 4
	L"BASS_ERROR_HANDLE",																						// Error 5
	L"BASS_ERROR_FORMAT",																						// Error 6
	L"BASS_ERROR_POSITION",																						// Error 7
	L"BASS_ERROR_INIT",																							// Error 8
	L"BASS_ERROR_START",																						// Error 9
	L"BASS_ERROR_SSL",																							// Error 10
	L"Unknown",																									// Error 11
	L"Unknown",																									// Error 12
	L"Unknown",																									// Error 13
	L"BASS_ERROR_ALREADY",																						// Error 14
	L"Unknown",																									// Error 15
	L"Unknown",																									// Error 16
	L"Unknown",																									// Error 17
	L"BASS_ERROR_NOCHAN",																						// Error 18
	L"BASS_ERROR_ILLTYPE",																						// Error 19
	L"BASS_ERROR_ILLPARAM",																						// Error 20
	L"BASS_ERROR_NO3D",																							// Error 21
	L"BASS_ERROR_NOEAX",																						// Error 22
	L"BASS_ERROR_DEVICE",																						// Error 23
	L"BASS_ERROR_NOPLAY",																						// Error 24
	L"BASS_ERROR_FREQ",																							// Error 25
	L"Unknown",																									// Error 26
	L"BASS_ERROR_NOTFILE",																						// Error 27
	L"Unknown",																									// Error 28
	L"BASS_ERROR_NOHW",																							// Error 29
	L"Unknown",																									// Error 30
	L"BASS_ERROR_EMPTY",																						// Error 31
	L"BASS_ERROR_NONET",																						// Error 32
	L"BASS_ERROR_CREATE",																						// Error 33
	L"BASS_ERROR_NOFX",																							// Error 34
	L"BASS_ERROR_PLAYING",																						// Error 35
	L"Unknown",																									// Error 36
	L"BASS_ERROR_NOTAVAIL",																						// Error 37
	L"BASS_ERROR_DECODE",																						// Error 38
	L"BASS_ERROR_DX",																							// Error 39
	L"BASS_ERROR_TIMEOUT",																						// Error 40
	L"BASS_ERROR_FILEFORM",																						// Error 41
	L"BASS_ERROR_SPEAKER",																						// Error 42
	L"BASS_ERROR_VERSION",																						// Error 43
	L"BASS_ERROR_CODEC",																						// Error 44
	L"BASS_ERROR_ENDED",																						// Error 45
	L"BASS_ERROR_BUSY",																							// Error 46
	L"Unknown error.",																							// Description of error -1
	L"Nothing's wrong.",																						// Description of error 0
	L"There is insufficient memory.",																			// Description of error 1
	L"The file could not be opened.",																			// Description of error 2
	L"There is no available device driver. The device may already be in use.",									// Description of error 3
	L"The sample buffer was lost.",																				// Description of error 4
	L"An invalid handle has been used.",																		// Description of error 5
	L"The sample format is not supported by the device/drivers.",												// Description of error 6
	L"The requested position is invalid, eg. it is beyond the end or the download has not yet reached it.",		// Description of error 7
	L"BASS_Init has not been successfully called.",																// Description of error 8
	L"BASS_Start has not been successfully called.",															// Description of error 9
	L"SSL/HTTPS support isn't available.",																		// Description of error 10
	L"No description available.",																				// Description of error 11
	L"No description available.",																				// Description of error 12
	L"No description available.",																				// Description of error 13
	L"Stream already initialized.",																				// Description of error 14
	L"No description available.",																				// Description of error 15
	L"No description available.",																				// Description of error 16
	L"No description available.",																				// Description of error 17
	L"Can't get a free channel.",																				// Description of error 18
	L"An illegal type was specified.",																			// Description of error 19
	L"An illegal parameter was specified.",																		// Description of error 20
	L"No 3D support.",																							// Description of error 21
	L"No EAX support.",																							// Description of error 22
	L"Illegal device number.",																					// Description of error 23
	L"The stream is not playing.",																				// Description of error 24
	L"Illegal sample rate.",																					// Description of error 25
	L"No description available.",																				// Description of error 26
	L"The stream is not a file stream.",																		// Description of error 27
	L"No description available.",																				// Description of error 28
	L"No hardware voices available.",																			// Description of error 29
	L"No description available.",																				// Description of error 30
	L"The MOD music has no sequence data.",																		// Description of error 31
	L"No internet connection could be opened.",																	// Description of error 32
	L"Couldn't create the file.",																				// Description of error 33
	L"Effects are not available.",																				// Description of error 34
	L"The channel is playing."																					// Description of error 35
	L"No description available.",																				// Description of error 36
	L"Requested data is not available.",																		// Description of error 37
	L"The channel is a \"decoding channel\".",																	// Description of error 38
	L"A sufficient DirectX version is not installed.",															// Description of error 39
	L"Connection timed out.",																					// Description of error 40
	L"Unsupported file format.",																				// Description of error 41
	L"Unavailable speaker.",																					// Description of error 42
	L"Invalid BASS version.",																					// Description of error 43
	L"Codec is not available/supported.",																		// Description of error 44
	L"The channel/file has ended.",																				// Description of error 45
	L"The device is busy (eg. in 'exclusive' use by another process).",											// Description of error 46
};

static TCHAR * basswasapierrc[4] =
{
	L"BASS_ERROR_WASAPI",																						// Error 5200
	L"BASS_ERROR_WASAPI_BUFFER",																				// Error 5201
	L"WASAPI is unavailable, or its DLL is missing.",															// Description of error 5200
	L"The buffer is too large. You need a really small buffer value for exclusive mode.",						// Description of error 5201
};

static TCHAR * bassxaerrc[6] =
{
	L"BASSXA_ERROR_STREAM",																						// Error 5000
	L"BASSXA_ERROR_FRAME",																						// Error 5001																						
	L"BASSXA_ERROR_DELETE",																						// Error 5002
	L"An error has occurred while opening the XAudio stream.",													// Description of error 5000
	L"An error has occurred while writing a frame to the XAudio stream.",										// Description of error 5001
	L"An error has occurred while deleting the XAudio stream.",													// Description of error 5002
};