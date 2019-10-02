// OmniMIDI lock system

// Lock system
typedef struct LockSystem {
	long volatile ReaderCount;
	long volatile WriterCount;
} LockSystem;

// Critical sections but handled by OmniMIDI functions because f**k Windows
extern "C" void LockForReading(LockSystem* LockStatus)
{
	while (TRUE) {
		if (LockStatus->WriterCount != 0) continue;

		InterlockedIncrement((long*)&LockStatus->ReaderCount);

		if (LockStatus->WriterCount == 0) break;

		InterlockedDecrement((long*)&LockStatus->ReaderCount);
	}
}

extern "C" void UnlockForReading(LockSystem* LockStatus)
{
	InterlockedDecrement((long*)&LockStatus->ReaderCount);
}

extern "C" void LockForWriting(LockSystem* LockStatus) {
	while (InterlockedExchange((long*)&LockStatus->WriterCount, 1) == 1);

	while (LockStatus->ReaderCount != 0);
}

extern "C" void UnlockForWriting(LockSystem* LockStatus)
{
	LockStatus->WriterCount = 0;
}