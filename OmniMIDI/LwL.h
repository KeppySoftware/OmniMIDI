/////////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) 1995-2002 Brad Wilson
//
//  This material is provided "as is", with absolutely no warranty
//  expressed or implied. Any use is at your own risk. Permission to
//  use or copy this software for any purpose is hereby granted without
//  fee, provided the above notices are retained on all copies.
//  Permission to modify the code and to distribute modified code is
//  granted, provided the above notices are retained, and a notice that
//  the code was modified is included with the above copyright notice.
//
/////////////////////////////////////////////////////////////////////////////
//
//  This lightweight lock class was adapted from samples and ideas that
//  were put across the ATL mailing list. It is a non-starving, kernel-
//  free lock that does not order writer requests. It is optimized for
//  use with resources that can take multiple simultaneous reads,
//  particularly when writing is only an occasional task.
//
//  Multiple readers may acquire the lock without any interference with
//  one another. As soon as a writer requests the lock, additional
//  readers will spin. When the pre-writer readers have all given up
//  control of the lock, the writer will obtain it. After the writer
//  has rescinded control, the additional readers will gain access
//  to the locked resource.
//
//  This class is very lightweight. It does not use any kernel objects.
//  It is designed for rapid access to resources without requiring
//  code to undergo process and ring changes. Because the "spin"
//  method for this lock is "Sleep(0)", it is a good idea to keep
//  the lock only long enough for short operations; otherwise, CPU
//  will be wasted spinning for the lock. You can change the spin
//  mechanism by #define'ing __LW_LOCK_SPIN before including this
//  header file.
//
//  VERY VERY IMPORTANT: If you have a lock open with read access and
//  attempt to get write access as well, you will deadlock! Always
//  rescind your read access before requesting write access (and,
//  of course, don't rely on any read information across this).
//
//  This lock works in a single process only. It cannot be used, as is,
//  for cross-process synchronization. To do that, you should convert
//  this lock to using a semaphore and mutex, or use shared memory to
//  avoid kernel objects.
//
//  POTENTIAL FUTURE UPGRADES:
//
//  You may consider writing a completely different "debug" version of
//  this class that sacrifices performance for safety, by catching
//  potential deadlock situations, potential "unlock from the wrong
//  thread" situations, etc. Also, of course, it's virtually mandatory
//  that you should consider testing on an SMP box.
//
///////////////////////////////////////////////////////////////////////////

#pragma once

#ifndef _INC_CRTDBG
#include 
#endif

#ifndef _WINDOWS_
#include 
#endif

#ifndef __LW_LOCK_SPIN
#define __LW_LOCK_SPIN NTSleep(0)
#endif


class LightweightLock
{
	//  Interface

public:
	//  Constructor

	LightweightLock()
	{
		m_ReaderCount = 0;
		m_WriterCount = 0;
	}

	//  Destructor

	~LightweightLock()
	{
		_ASSERTE(m_ReaderCount == 0);
		_ASSERTE(m_WriterCount == 0);
	}

	//  Reader lock acquisition and release

	void LockForReading()
	{
		while (1)
		{
			//  If there's a writer already, spin without unnecessarily
			//  interlocking the CPUs

			if (m_WriterCount != 0)
			{
				__LW_LOCK_SPIN;
				continue;
			}

			//  Add to the readers list

			InterlockedIncrement((long*)&m_ReaderCount);

			//  Check for writers again (we may have been pre-empted). If
			//  there are no writers writing or waiting, then we're done.

			if (m_WriterCount == 0)
				break;

			//  Remove from the readers list, spin, try again

			InterlockedDecrement((long*)&m_ReaderCount);
			__LW_LOCK_SPIN;
		}
	}

	void UnlockForReading()
	{
		InterlockedDecrement((long*)&m_ReaderCount);
	}

	//  Writer lock acquisition and release

	void LockForWriting()
	{
		//  See if we can become the writer (expensive, because it inter-
		//  locks the CPUs, so writing should be an infrequent process)

		while (InterlockedExchange((long*)&m_WriterCount, 1) == 1)
		{
			__LW_LOCK_SPIN;
		}

		//  Now we're the writer, but there may be outstanding readers.
		//  Spin until there aren't any more; new readers will wait now
		//  that we're the writer.

		while (m_ReaderCount != 0)
		{
			__LW_LOCK_SPIN;
		}
	}

	void UnlockForWriting()
	{
		m_WriterCount = 0;
	}

	long GetReaderCount() { return m_ReaderCount; };
	long GetWriterConut() { return m_WriterCount; };

	//  Implementation

private:
	long volatile m_ReaderCount;
	long volatile m_WriterCount;
};