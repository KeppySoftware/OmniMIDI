// DropFileTarget.h: interface for the CDropFileTarget class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_DROPFILETARGET_H__5FDD306B_7CC8_4B2A_89DB_8048E7EE545A__INCLUDED_)
#define AFX_DROPFILETARGET_H__5FDD306B_7CC8_4B2A_89DB_8048E7EE545A__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

//-----------------------------------------------------------------------------
// Class to make a window a dropfile target
//-----------------------------------------------------------------------------
template <class T> class CDropFileTarget  
{
public:

	BEGIN_MSG_MAP(CDropFileTarget<T>)
		MESSAGE_HANDLER(WM_DROPFILES, OnDropFiles) 
	END_MSG_MAP()

	//-----------------------------------------------------------------------------
	// Function name	: RegisterDropTarget
	// Description	    : Registers whether a window accepts dropped files
	// Return type		: None
	// Parameter        : boolean indicator
	//					TRUE  - window accepts dropped files
	//					FALSE - window does not accept dropped files
	//-----------------------------------------------------------------------------
	void RegisterDropTarget(BOOL bAccept = TRUE)
	{
        T* pT = static_cast<T*>(this);
		ATLASSERT(::IsWindow(pT->m_hWnd));

		// Turn the WS_EX_ACCEPTFILES style on or off based on the value of the
		// bAccept parameter
		::DragAcceptFiles(pT->m_hWnd, bAccept);
	}

private:

	//-----------------------------------------------------------------------------
	// Function name	: OnDropFiles
	// Description	    : Handler for WM_DROPFILES message
	// Return type		: LRESULT
	// Parameter        : 
	//-----------------------------------------------------------------------------
	LRESULT OnDropFiles(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
	{
		TCHAR szFilename[MAX_PATH];

        T* pT = static_cast<T*>(this);
		ATLASSERT(::IsWindow(pT->m_hWnd));

		// Get the handle to an internal structure describing the dropped files
		HDROP hDrop = (HDROP)wParam; 

		// Get the count of the files dropped
		int nNumFiles = DragQueryFile(hDrop, 0xFFFFFFFF, NULL, 0);

		while (nNumFiles--)
		{
			// Get the path of a single file that has been dropped
			DragQueryFile(hDrop, nNumFiles, szFilename, MAX_PATH);

			// Call the function to process the file name
			pT->ProcessFile(szFilename);
		}

		// Release the memory that the system allocated for use in transferring file 
		// names to the application
		DragFinish(hDrop);

		return 0;
	}
};

#endif // !defined(AFX_DROPFILETARGET_H__5FDD306B_7CC8_4B2A_89DB_8048E7EE545A__INCLUDED_)
