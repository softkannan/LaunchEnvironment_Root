#pragma once

#define DBG_TRACE   0

#include <windows.h>
#include <stdio.h>
#include <TCHAR.H>
#include <stdarg.h>
#include <stdio.h>

#include "..\\Detours\\include\\detours.h"

#ifdef _WIN64
#pragma comment( lib, "..\\Detours\\lib.X64\\detours.lib" )
#else
#pragma comment( lib, "..\\Detours\\lib.X86\\detours.lib" )
#endif

//#ifdef _WIN64
//#pragma comment( lib, "..\\Detours\\lib.X64\\syelog.lib" )
//#else
//#pragma comment( lib, "..\\Detours\\lib.X86\\syelog.lib" )
//#endif

#define PULONG_PTR          PVOID
#define PLONG_PTR           PVOID
#define ULONG_PTR           PVOID
#define ENUMRESNAMEPROCA    PVOID
#define ENUMRESNAMEPROCW    PVOID
#define ENUMRESLANGPROCA    PVOID
#define ENUMRESLANGPROCW    PVOID
#define ENUMRESTYPEPROCA    PVOID
#define ENUMRESTYPEPROCW    PVOID
#define STGOPTIONS          PVOID


#define LOG_FOUND_KEY(...) else { TCHAR tempStr[TEMP_BUFF_SIZE]; _stprintf_s(tempStr,__VA_ARGS__); OutputDebugString(tempStr); }

//////////////////////////////////////////////////////////////////////////////
#pragma warning(disable:4127)   // Many of our asserts are constants.

#define ASSERT_ALWAYS(x)   \
    do {                                                        \
    if (!(x)) {                                                 \
            AssertMessage(#x, _T(__FILE__), _T(__LINE__));              \
            DebugBreak();                                       \
                            }                                                           \
                            } while (0)

#ifndef NDEBUG
#define ASSERT(x)           ASSERT_ALWAYS(x)
#else
#define ASSERT(x)
#endif

#define TEMP_BUFF_SIZE 4096

#define ENABLE_REG_LOG

#ifndef ENABLE_REG_LOG
#define REG_LOG(...)
#else

#define REG_LOG(...) { TCHAR tempStr[TEMP_BUFF_SIZE]; _stprintf_s(tempStr,__VA_ARGS__); OutputDebugString(tempStr); }

#endif // ENABLE_DSP_LOG

#define UNUSED(c)       (c) = (c)

//////////////////////////////////////////////////////////////////////////////
static HMODULE g_hInst = NULL;
static char g_szDllPath[MAX_PATH];
//static BOOL g_bIsRegistryInitialized;

#define LOG_BUF_SIZE 4096

BOOL ProcessEnumerate();
BOOL InstanceEnumerate(HINSTANCE hInst);

VOID _PrintEnter(const TCHAR * psz, ...);
VOID _PrintExit(const TCHAR * psz, ...);
VOID _Print(const TCHAR * psz, ...);

VOID AssertMessage(CONST PTCHAR pszMsg, CONST PTCHAR pszFile, ULONG nLine);

BOOL ThreadAttach(HMODULE hDll);
BOOL ThreadDetach(HMODULE hDll);
BOOL ProcessAttach(HMODULE hDll);
BOOL ProcessDetach(HMODULE hDll);

extern "C" {
    __declspec(dllexport) void InitializeRegistryInternal();
}

