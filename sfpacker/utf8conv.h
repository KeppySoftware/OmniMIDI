//////////////////////////////////////////////////////////////////////////
//
// FILE: utf8conv.h
//
// Header file defining helper functions for converting strings
// between Unicode UTF-8 and UTF-16.
//
// UTF-8 is stored in std::string; UTF-16 is stored in std::wstring.
//
// This code just uses Win32 Platform SDK and C++ standard library; 
// so it can be used also with the Express editions of Visual Studio.
//
//
// February 4th, 2011
//
// by Giovanni Dicanio <gdicanio@mvps.org>
//
//////////////////////////////////////////////////////////////////////////


#pragma once


//------------------------------------------------------------------------
//                              INCLUDES
//------------------------------------------------------------------------

#include <stdarg.h>     // variable argument lists...
#include <stdio.h>      // ...and vsprintf_s

#include <exception>    // std::exception
#include <string>       // STL string classes

#include <Windows.h>    // Win32 Platform SDK main header



namespace utf8util {


//------------------------------------------------------------------------
// Exception class representing an error occurred during UTF-8 conversion.
//------------------------------------------------------------------------
class utf8_error 
    : public std::exception
{
public:
   
    // Constructs an utf8_error with a message string that can use a
    // printf-like syntax for formatting.
    explicit utf8_error(const char * format, ...);

    // Override from std::exception::what()
    const char * what() const;


    //
    // IMPLEMENTATION
    //
private:
    char m_message[512];    // buffer for error message
};


inline utf8_error::utf8_error(const char * format, ...)
{
    // Format error message in buffer
    va_list args;
    va_start(args, format);
    vsprintf_s(m_message, format, args);
    va_end(args);
}


inline const char * utf8_error::what() const
{
    return m_message;
}

//------------------------------------------------------------------------



//------------------------------------------------------------------------
// Converts a string from UTF-8 to UTF-16.
// On error, can throw an utf8_error exception.
//------------------------------------------------------------------------
inline std::wstring utf16_from_utf8(const std::string & utf8)
{
    //
    // Special case of empty input string
    //
    if (utf8.empty())
        return std::wstring();


    //
    // Get length (in wchar_t's) of resulting UTF-16 string
    //
    const int utf16_length = ::MultiByteToWideChar(
        CP_UTF8,            // convert from UTF-8
        0,                  // default flags
        utf8.data(),        // source UTF-8 string
        utf8.length(),      // length (in chars) of source UTF-8 string
        NULL,               // unused - no conversion done in this step
        0                   // request size of destination buffer, in wchar_t's
        );
    if (utf16_length == 0)
    {
        // Error
        DWORD error = ::GetLastError();
        throw utf8_error(
            "Can't get length of UTF-16 string (MultiByteToWideChar set last error to %lu).", 
            error);
    }


    //
    // Allocate destination buffer for UTF-16 string
    //
    std::wstring utf16;
    utf16.resize(utf16_length);


    //
    // Do the conversion from UTF-8 to UTF-16
    //
    if ( ! ::MultiByteToWideChar(
        CP_UTF8,            // convert from UTF-8
        0,                  // default flags
        utf8.data(),        // source UTF-8 string
        utf8.length(),      // length (in chars) of source UTF-8 string
        &utf16[0],          // destination buffer
        utf16.length()      // size of destination buffer, in wchar_t's
        ) )
    {
        // Error
        DWORD error = ::GetLastError();
        throw utf8_error(
            "Can't convert string from UTF-8 to UTF-16 (MultiByteToWideChar set last error to %lu).", 
            error);
    }

    //
    // Return resulting UTF-16 string
    //
    return utf16;
}


//------------------------------------------------------------------------
// Converts a string from UTF-16 to UTF-8.
// On error, can throw an utf8_error exception.
//------------------------------------------------------------------------
inline std::string utf8_from_utf16(const std::wstring & utf16)
{
    //
    // Special case of empty input string
    //
    if (utf16.empty())
        return std::string();


    //
    // Get length (in chars) of resulting UTF-8 string
    //
    const int utf8_length = ::WideCharToMultiByte(
        CP_UTF8,            // convert to UTF-8
        0,                  // default flags
        utf16.data(),       // source UTF-16 string
        utf16.length(),     // source string length, in wchar_t's,
        NULL,               // unused - no conversion required in this step
        0,                  // request buffer size
        NULL, NULL          // unused
        );
    if (utf8_length == 0)
    {
        // Error
        DWORD error = ::GetLastError();
        throw utf8_error(
            "Can't get length of UTF-8 string (WideCharToMultiByte set last error to %lu).", 
            error);
    }


    //
    // Allocate destination buffer for UTF-8 string
    //
    std::string utf8;
    utf8.resize(utf8_length);


    //
    // Do the conversion from UTF-16 to UTF-8
    //
    if ( ! ::WideCharToMultiByte(
        CP_UTF8,                // convert to UTF-8
        0,                      // default flags
        utf16.data(),           // source UTF-16 string
        utf16.length(),         // source string length, in wchar_t's,
        &utf8[0],               // destination buffer
        utf8.length(),          // destination buffer size, in chars
        NULL, NULL              // unused
        ) )
    {
        // Error
        DWORD error = ::GetLastError();
        throw utf8_error(
            "Can't convert string from UTF-16 to UTF-8 (WideCharToMultiByte set last error to %lu).", 
            error);
    }


    //
    // Return resulting UTF-8 string
    //
    return utf8;
}


} // namespace utf8util


//////////////////////////////////////////////////////////////////////////

