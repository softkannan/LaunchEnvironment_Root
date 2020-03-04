// RegistryHandler.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "RegistryHandler.h"
#include <string>
#include <sstream>


using namespace std;

//////////////////////////////////////////////////////////////////////////////
//
HKEY g_hKeyFile = NULL;
HKEY g_hKeyLocalMachine = NULL;
HKEY g_hKeyCurrentUser = NULL;
HKEY g_hKeyClassRoot = NULL;
BOOL g_bIsRegistryInitialized = FALSE;

inline void InitializeRegistry()
{
	if (g_bIsRegistryInitialized == FALSE)
	{
		InitializeRegistryInternal();
		g_bIsRegistryInitialized = TRUE;
	}
}

HKEY GetPrivateKey(HKEY hKey)
{
	if (hKey == HKEY_CLASSES_ROOT)
	{
		if (g_hKeyClassRoot == NULL)
		{
			REG_LOG(_T("HKEY_CLASSES_ROOT Private Registry Not Loaded"));
			return hKey;
		}
		return g_hKeyClassRoot;
	}
	else if (hKey == HKEY_CURRENT_USER)
	{
		if (g_hKeyCurrentUser == NULL)
		{
			REG_LOG(_T("HKEY_CURRENT_USER Private Registry Not Loaded"));
			return hKey;
		}
		return g_hKeyCurrentUser;
	}
	else if (hKey == HKEY_LOCAL_MACHINE)
	{
		if (g_hKeyLocalMachine == NULL)
		{
			REG_LOG(_T("HKEY_LOCAL_MACHINE Private Registry Not Loaded"));
			return hKey;
		}
		return g_hKeyLocalMachine;
	}
	return hKey;
}

//string RegTypeToString(DWORD dwType)
//{
//    string retVal = "REG_NONE";
//
//    switch (dwType)
//    {
//    case REG_BINARY:
//        retVal = "REG_BINARY";
//        break;
//    case REG_SZ:
//        retVal = "REG_SZ";
//        break;
//    case REG_EXPAND_SZ:
//        retVal = "REG_EXPAND_SZ";
//        break;
//    case REG_DWORD:
//        retVal = "REG_DWORD";
//        break;
//    case REG_DWORD_BIG_ENDIAN:
//        retVal = "REG_DWORD_BIG_ENDIAN";
//        break;
//    case REG_LINK:
//        retVal = "REG_LINK";
//        break;
//    case REG_MULTI_SZ:
//        retVal = "REG_MULTI_SZ";
//        break;
//    case REG_RESOURCE_LIST:
//        retVal = "REG_RESOURCE_LIST";
//        break;
//    case REG_FULL_RESOURCE_DESCRIPTOR:
//        retVal = "REG_FULL_RESOURCE_DESCRIPTOR";
//        break;
//    case REG_RESOURCE_REQUIREMENTS_LIST:
//        retVal = "REG_RESOURCE_REQUIREMENTS_LIST";
//        break;
//    case REG_QWORD:
//        retVal = "REG_QWORD";
//        break;
//    }
//
//    return retVal;
//}
//
//string HKEYToString(HKEY hKey)
//{
//    string retVal = "";
//
//    switch ((LONG)hKey)
//    {
//    case 0x80000000:
//        retVal = "HKEY_CLASSES_ROOT";
//        break;
//    case 0x80000001:
//        retVal = "HKEY_CURRENT_USER";
//        break;
//    case 0x80000002:
//        retVal = "HKEY_LOCAL_MACHINE";
//        break;
//    case 0x80000003:
//        retVal = "HKEY_USERS";
//        break;
//    case 0x80000004:
//        retVal = "HKEY_PERFORMANCE_DATA";
//        break;
//    case 0x80000050:
//        retVal = "HKEY_PERFORMANCE_TEXT";
//        break;
//    case 0x80000060:
//        retVal = "HKEY_PERFORMANCE_NLSTEXT";
//        break;
//    case 0x80000005:
//        retVal = "HKEY_CURRENT_CONFIG";
//        break;
//    case 0x80000006:
//        retVal = "HKEY_DYN_DATA";
//        break;
//    case 0x80000007:
//        retVal = "HKEY_CURRENT_USER_LOCAL_SETTINGS";
//        break;
//    default:
//        std::stringstream stream;
//        stream << std::hex << (LONG)hKey;
//        return stream.str();
//    }
//    return retVal;
//}
//string RegSAMToString(REGSAM samDesired)
//{
//    string retVal = "";
//
//    if (samDesired == KEY_ALL_ACCESS)
//    {
//        retVal += "KEY_ALL_ACCESS";
//    }
//    else
//    {
//        if ((samDesired & KEY_WOW64_RES) == KEY_WOW64_RES)
//        {
//            retVal += "KEY_WOW64_RES | ";
//        }
//
//        if ((samDesired & KEY_READ) == KEY_READ)
//        {
//            retVal += "KEY_READ | ";
//        }
//
//        if ((samDesired & KEY_WRITE) == KEY_WRITE)
//        {
//            retVal += "KEY_WRITE | ";
//        }
//
//        if ((samDesired & KEY_EXECUTE) == KEY_EXECUTE)
//        {
//            retVal += "KEY_EXECUTE | ";
//        }
//
//        if ((samDesired & READ_CONTROL) == READ_CONTROL)
//        {
//            retVal += "READ_CONTROL | ";
//        }
//
//        if ((samDesired & KEY_WRITE) == KEY_WRITE)
//        {
//            retVal += "KEY_WRITE | ";
//        }
//
//        if ((samDesired & KEY_WOW64_64KEY) == KEY_WOW64_64KEY)
//        {
//            retVal += "KEY_WOW64_64KEY | ";
//        }
//
//        if ((samDesired & KEY_WOW64_32KEY) == KEY_WOW64_32KEY)
//        {
//            retVal += "KEY_WOW64_32KEY | ";
//        }
//
//        if ((samDesired & KEY_SET_VALUE) == KEY_SET_VALUE)
//        {
//            retVal += "KEY_SET_VALUE | ";
//        }
//
//        if ((samDesired & KEY_READ) == KEY_READ)
//        {
//            retVal += "KEY_READ | ";
//        }
//        if ((samDesired & KEY_QUERY_VALUE) == KEY_QUERY_VALUE)
//        {
//            retVal += "KEY_QUERY_VALUE | ";
//        }
//
//        if ((samDesired & KEY_NOTIFY) == KEY_NOTIFY)
//        {
//            retVal += "KEY_NOTIFY | ";
//        }
//
//        if ((samDesired & KEY_EXECUTE) == KEY_EXECUTE)
//        {
//            retVal += "KEY_EXECUTE | ";
//        }
//
//        if ((samDesired & KEY_ENUMERATE_SUB_KEYS) == KEY_ENUMERATE_SUB_KEYS)
//        {
//            retVal += "KEY_ENUMERATE_SUB_KEYS | ";
//        }
//
//        if ((samDesired & KEY_CREATE_SUB_KEY) == KEY_CREATE_SUB_KEY)
//        {
//            retVal += "KEY_CREATE_SUB_KEY | ";
//        }
//
//        if ((samDesired & KEY_CREATE_LINK) == KEY_CREATE_LINK)
//        {
//            retVal += "KEY_CREATE_LINK";
//        }
//    }
//
//    if (retVal.length() == 0)
//    {
//        retVal = "NULL";
//    }
//
//    return retVal;
//}

BOOL(WINAPI * Real_CreateProcessW)(LPCWSTR lpApplicationName,
	LPWSTR lpCommandLine,
	LPSECURITY_ATTRIBUTES lpProcessAttributes,
	LPSECURITY_ATTRIBUTES lpThreadAttributes,
	BOOL bInheritHandles,
	DWORD dwCreationFlags,
	LPVOID lpEnvironment,
	LPCWSTR lpCurrentDirectory,
	LPSTARTUPINFOW lpStartupInfo,
	LPPROCESS_INFORMATION lpProcessInformation)
	= CreateProcessW;

BOOL(WINAPI * Real_CreateProcessA)(LPCSTR lpApplicationName,
	LPSTR lpCommandLine,
	LPSECURITY_ATTRIBUTES lpProcessAttributes,
	LPSECURITY_ATTRIBUTES lpThreadAttributes,
	BOOL bInheritHandles,
	DWORD dwCreationFlags,
	LPVOID lpEnvironment,
	LPCSTR lpCurrentDirectory,
	LPSTARTUPINFOA lpStartupInfo,
	LPPROCESS_INFORMATION lpProcessInformation)
	= CreateProcessA;

LONG(WINAPI * Real_RegCreateKeyExA)(
	HKEY hKey,
	LPCSTR lpSubKey,
	DWORD Reserved,
	LPSTR lpClass,
	DWORD dwOptions,
	REGSAM samDesired,
	CONST LPSECURITY_ATTRIBUTES lpSecurityAttributes,
	PHKEY phkResult,
	LPDWORD lpdwDisposition)
    = RegCreateKeyExA;

LONG(WINAPI * Real_RegCreateKeyExW)(HKEY a0,
    LPCWSTR a1,
    DWORD a2,
    LPWSTR a3,
    DWORD a4,
    REGSAM a5,
    LPSECURITY_ATTRIBUTES a6,
    PHKEY a7,
    LPDWORD a8)
    = RegCreateKeyExW;

LONG(WINAPI * Real_RegDeleteKeyA)(
	HKEY a0,
    LPCSTR a1)
    = RegDeleteKeyA;

LONG(WINAPI * Real_RegDeleteKeyW)(
	HKEY a0,
    LPCWSTR a1)
    = RegDeleteKeyW;

LONG(WINAPI * Real_RegDeleteValueA)(
	HKEY a0,
    LPCSTR a1)
    = RegDeleteValueA;


LONG(WINAPI * Real_RegDeleteValueW)(
	HKEY a0,
    LPCWSTR a1)
    = RegDeleteValueW;

LONG(WINAPI * Real_RegEnumKeyExA)(
	HKEY a0,
    DWORD a1,
    LPSTR a2,
    LPDWORD a3,
    LPDWORD a4,
    LPSTR a5,
    LPDWORD a6,
struct _FILETIME* a7)
    = RegEnumKeyExA;

LONG(WINAPI * Real_RegEnumKeyExW)(
	HKEY a0,
    DWORD a1,
    LPWSTR a2,
    LPDWORD a3,
    LPDWORD a4,
    LPWSTR a5,
    LPDWORD a6,
struct _FILETIME* a7)
    = RegEnumKeyExW;

LONG(WINAPI * Real_RegEnumValueA)(
	HKEY a0,
    DWORD a1,
    LPSTR a2,
    LPDWORD a3,
    LPDWORD a4,
    LPDWORD a5,
    LPBYTE a6,
    LPDWORD a7)
    = RegEnumValueA;

LONG(WINAPI * Real_RegEnumValueW)(
	HKEY a0,
    DWORD a1,
    LPWSTR a2,
    LPDWORD a3,
    LPDWORD a4,
    LPDWORD a5,
    LPBYTE a6,
    LPDWORD a7)
    = RegEnumValueW;

LONG(WINAPI * Real_RegOpenKeyExA)(
	HKEY a0,
    LPCSTR a1,
    DWORD a2,
    REGSAM a3,
    PHKEY a4)
    = RegOpenKeyExA;

LONG(WINAPI * Real_RegOpenKeyExW)(
	HKEY a0,
    LPCWSTR a1,
    DWORD a2,
    REGSAM a3,
    PHKEY a4)
    = RegOpenKeyExW;

LONG(WINAPI * Real_RegQueryInfoKeyA)(
	HKEY a0,
    LPSTR a1,
    LPDWORD a2,
    LPDWORD a3,
    LPDWORD a4,
    LPDWORD a5,
    LPDWORD a6,
    LPDWORD a7,
    LPDWORD a8,
    LPDWORD a9,
    LPDWORD a10,
struct _FILETIME* a11)
    = RegQueryInfoKeyA;

LONG(WINAPI * Real_RegQueryInfoKeyW)(
	HKEY a0,
    LPWSTR a1,
    LPDWORD a2,
    LPDWORD a3,
    LPDWORD a4,
    LPDWORD a5,
    LPDWORD a6,
    LPDWORD a7,
    LPDWORD a8,
    LPDWORD a9,
    LPDWORD a10,
struct _FILETIME* a11)
    = RegQueryInfoKeyW;

LONG(WINAPI * Real_RegQueryValueExA)(
	HKEY a0,
    LPCSTR a1,
    LPDWORD a2,
    LPDWORD a3,
    LPBYTE a4,
    LPDWORD a5)
    = RegQueryValueExA;

LONG(WINAPI * Real_RegQueryValueExW)(
	HKEY a0,
    LPCWSTR a1,
    LPDWORD a2,
    LPDWORD a3,
    LPBYTE a4,
    LPDWORD a5)
    = RegQueryValueExW;

LONG(WINAPI * Real_RegSetValueExA)(
	HKEY a0,
    LPCSTR a1,
    DWORD a2,
    DWORD a3,
    const BYTE* a4,
    DWORD a5)
    = RegSetValueExA;

LONG(WINAPI * Real_RegSetValueExW)(
	HKEY a0,
    LPCWSTR a1,
    DWORD a2,
    DWORD a3,
    const BYTE* a4,
    DWORD a5)
    = RegSetValueExW;

/////////////////////////////////////////////////////////////
// Detours
//
BOOL WINAPI Mine_CreateProcessA(LPCSTR lpApplicationName,
	LPSTR lpCommandLine,
	LPSECURITY_ATTRIBUTES lpProcessAttributes,
	LPSECURITY_ATTRIBUTES lpThreadAttributes,
	BOOL bInheritHandles,
	DWORD dwCreationFlags,
	LPVOID lpEnvironment,
	LPCSTR lpCurrentDirectory,
	LPSTARTUPINFOA lpStartupInfo,
	LPPROCESS_INFORMATION lpProcessInformation)
{
	BOOL rv = 0;
	__try {
		rv = DetourCreateProcessWithDllExA(lpApplicationName,
			lpCommandLine,
			lpProcessAttributes,
			lpThreadAttributes,
			bInheritHandles,
			dwCreationFlags,
			lpEnvironment,
			lpCurrentDirectory,
			lpStartupInfo,
			lpProcessInformation,
			g_szDllPath,
			Real_CreateProcessA);
	}
	__finally {
	};
	return rv;
}
BOOL WINAPI Mine_CreateProcessW(LPCWSTR lpApplicationName,
	LPWSTR lpCommandLine,
	LPSECURITY_ATTRIBUTES lpProcessAttributes,
	LPSECURITY_ATTRIBUTES lpThreadAttributes,
	BOOL bInheritHandles,
	DWORD dwCreationFlags,
	LPVOID lpEnvironment,
	LPCWSTR lpCurrentDirectory,
	LPSTARTUPINFOW lpStartupInfo,
	LPPROCESS_INFORMATION lpProcessInformation)
{
	BOOL rv = 0;
	__try {
		rv = DetourCreateProcessWithDllExW(lpApplicationName,
			lpCommandLine,
			lpProcessAttributes,
			lpThreadAttributes,
			bInheritHandles,
			dwCreationFlags,
			lpEnvironment,
			lpCurrentDirectory,
			lpStartupInfo,
			lpProcessInformation,
			g_szDllPath,
			Real_CreateProcessW);
	}
	__finally {
	};
	return rv;
}
LONG WINAPI Mine_RegCreateKeyExA(HKEY hKey,
    LPCSTR lpSubKey,
    DWORD Reserved,
    LPSTR lpClass,
    DWORD dwOptions,
    REGSAM samDesired,
    LPSECURITY_ATTRIBUTES lpSecurityAttributes,
    PHKEY phkResult,
    LPDWORD lpdwDisposition)
{
    LONG rv = 0;
    __try 
	{
		InitializeRegistry();
		HKEY localKey = GetPrivateKey(hKey);
		if ((rv = Real_RegCreateKeyExA(localKey, lpSubKey, Reserved, lpClass, dwOptions, samDesired, lpSecurityAttributes, phkResult, lpdwDisposition)) != ERROR_SUCCESS)
		{
			rv = Real_RegCreateKeyExA(hKey, lpSubKey, Reserved, lpClass, dwOptions, samDesired, lpSecurityAttributes, phkResult, lpdwDisposition);
		}
		LOG_FOUND_KEY(_T("RegCreateKeyExA Found Key %s"),lpSubKey)
    }
    __finally {
    };
    return rv;
}

LONG WINAPI Mine_RegCreateKeyExW(HKEY hKey,
    LPCWSTR a1,
    DWORD a2,
    LPWSTR a3,
    DWORD a4,
    REGSAM a5,
    LPSECURITY_ATTRIBUTES a6,
    PHKEY a7,
    LPDWORD a8)
{
    LONG rv = 0;
    __try 
	{
		InitializeRegistry();
		HKEY localKey = GetPrivateKey(hKey);
		if ((rv = Real_RegCreateKeyExW(localKey, a1, a2, a3, a4, a5, a6, a7, a8)) != ERROR_SUCCESS)
		{
			rv = Real_RegCreateKeyExW(hKey, a1, a2, a3, a4, a5, a6, a7, a8);
		}
		LOG_FOUND_KEY(_T("RegCreateKeyExW Found Key %S"), a1)
    }
    __finally {
    };
    return rv;
}

LONG WINAPI Mine_RegDeleteKeyA(HKEY hKey, LPCSTR a1)
{
    LONG rv = 0;
    __try 
	{
		InitializeRegistry();
		HKEY localKey = GetPrivateKey(hKey);
		if ((rv = Real_RegDeleteKeyA(localKey, a1)) != ERROR_SUCCESS)
		{
			rv = Real_RegDeleteKeyA(hKey, a1);
		}
		LOG_FOUND_KEY(_T("RegDeleteKeyA Found Key %s"), a1)
    }
    __finally {
    };
    return rv;
}

LONG WINAPI Mine_RegDeleteKeyW(HKEY hKey, LPCWSTR a1)
{
    LONG rv = 0;
    __try 
	{
		InitializeRegistry();
		HKEY localKey = GetPrivateKey(hKey);
		if ((rv = Real_RegDeleteKeyW(localKey, a1)) != ERROR_SUCCESS)
		{
			rv = Real_RegDeleteKeyW(hKey, a1);
		}
		LOG_FOUND_KEY(_T("RegDeleteKeyW Found Key %S"), a1)
    }
    __finally {
    };
    return rv;
}

LONG WINAPI Mine_RegDeleteValueA(HKEY hKey, LPCSTR a1)
{
    LONG rv = 0;
    __try 
	{
		InitializeRegistry();
		HKEY localKey = GetPrivateKey(hKey);
		if ((rv = Real_RegDeleteValueA(localKey, a1)) != ERROR_SUCCESS)
		{
			rv = Real_RegDeleteValueA(hKey, a1);
		}
		LOG_FOUND_KEY(_T("RegDeleteValueA Found Key %s"), a1)
    }
    __finally {
    };
    return rv;
}

LONG WINAPI Mine_RegDeleteValueW(HKEY hKey, LPCWSTR a1)
{
    LONG rv = 0;
    __try 
	{
		InitializeRegistry();
		HKEY localKey = GetPrivateKey(hKey);
		if ((rv = Real_RegDeleteValueW(localKey, a1)) != ERROR_SUCCESS)
		{
			rv = Real_RegDeleteValueW(hKey, a1);
		}
		LOG_FOUND_KEY(_T("RegDeleteValueW Found Key %S"), a1)
    }
    __finally {
    };
    return rv;
}

LONG WINAPI Mine_RegEnumKeyExA(HKEY hKey,
    DWORD a1,
    LPSTR a2,
    LPDWORD a3,
    LPDWORD a4,
    LPSTR a5,
    LPDWORD a6,
    LPFILETIME a7)
{
    LONG rv = 0;
    __try 
	{
		InitializeRegistry();
		HKEY localKey = GetPrivateKey(hKey);
		if ((rv = Real_RegEnumKeyExA(localKey, a1, a2, a3, a4, a5, a6, a7)) != ERROR_SUCCESS)
		{
			rv = Real_RegEnumKeyExA(hKey, a1, a2, a3, a4, a5, a6, a7);
		}
		LOG_FOUND_KEY(_T("RegEnumKeyExA Found Key %s"), a2)
    }
    __finally {
    };
    return rv;
}

LONG WINAPI Mine_RegEnumKeyExW(HKEY hKey,
    DWORD a1,
    LPWSTR a2,
    LPDWORD a3,
    LPDWORD a4,
    LPWSTR a5,
    LPDWORD a6,
struct _FILETIME* a7)
{
    LONG rv = 0;
    __try 
	{
		InitializeRegistry();
		HKEY localKey = GetPrivateKey(hKey);
		if ((rv = Real_RegEnumKeyExW(localKey, a1, a2, a3, a4, a5, a6, a7)) != ERROR_SUCCESS)
		{
			rv = Real_RegEnumKeyExW(hKey, a1, a2, a3, a4, a5, a6, a7);
		}
		LOG_FOUND_KEY(_T("RegEnumKeyExW Found Key %S"), a2)
    }
    __finally {
    };
    return rv;
}

LONG WINAPI Mine_RegEnumValueA(HKEY hKey,
    DWORD a1,
    LPSTR a2,
    LPDWORD a3,
    LPDWORD a4,
    LPDWORD a5,
    LPBYTE a6,
    LPDWORD a7)
{
    LONG rv = 0;
    __try 
	{
		InitializeRegistry();
		HKEY localKey = GetPrivateKey(hKey);
		if ((rv = Real_RegEnumValueA(localKey, a1, a2, a3, a4, a5, a6, a7)) != ERROR_SUCCESS)
		{
			rv = Real_RegEnumValueA(hKey, a1, a2, a3, a4, a5, a6, a7);
		}
		LOG_FOUND_KEY(_T("RegEnumValueA Found Key %s"), a2)
    }
    __finally {
    };
    return rv;
}

LONG WINAPI Mine_RegEnumValueW(HKEY hKey,
    DWORD a1,
    LPWSTR a2,
    LPDWORD a3,
    LPDWORD a4,
    LPDWORD a5,
    LPBYTE a6,
    LPDWORD a7)
{
    LONG rv = 0;
    __try 
	{
		InitializeRegistry();
		HKEY localKey = GetPrivateKey(hKey);
		if ((rv = Real_RegEnumValueW(localKey, a1, a2, a3, a4, a5, a6, a7)) != ERROR_SUCCESS)
		{
			rv = Real_RegEnumValueW(hKey, a1, a2, a3, a4, a5, a6, a7);
		}
		LOG_FOUND_KEY(_T("RegEnumValueW Found Key %S"), a2)
    }
    __finally {
    };
    return rv;
}

LONG WINAPI Mine_RegOpenKeyExA(HKEY hKey,
    LPCSTR lpSubKey,
    DWORD ulOptions,
    REGSAM samDesired,
    PHKEY phkResult)
{
    LONG rv = 0;
    __try 
	{
		InitializeRegistry();
		HKEY localKey = GetPrivateKey(hKey);
		if (rv = Real_RegOpenKeyExA(localKey, lpSubKey, ulOptions, samDesired, phkResult) != ERROR_SUCCESS)
		{
			rv = Real_RegOpenKeyExA(hKey, lpSubKey, ulOptions, samDesired, phkResult);
		}
		LOG_FOUND_KEY(_T("RegOpenKeyExA Found Key %s"), lpSubKey)
    }
    __finally {
        
    };
    return rv;
}

LONG WINAPI Mine_RegOpenKeyExW(HKEY hKey,
    LPCWSTR lpSubKey,
    DWORD ulOptions,
    REGSAM samDesired,
    PHKEY phkResult)
{
    LONG rv = 0;
    __try 
	{
		InitializeRegistry();
		HKEY localKey = GetPrivateKey(hKey);
		if (rv = Real_RegOpenKeyExW(g_hKeyClassRoot, lpSubKey, ulOptions, samDesired, phkResult) != ERROR_SUCCESS)
		{
			rv = Real_RegOpenKeyExW(hKey, lpSubKey, ulOptions, samDesired, phkResult);
		}
		LOG_FOUND_KEY(_T("RegOpenKeyExW Found Key %S"), lpSubKey)
    }
    __finally {

    };
    return rv;
}

LONG WINAPI Mine_RegQueryInfoKeyA(HKEY hKey,
    LPSTR a1,
    LPDWORD a2,
    LPDWORD a3,
    LPDWORD a4,
    LPDWORD a5,
    LPDWORD a6,
    LPDWORD a7,
    LPDWORD a8,
    LPDWORD a9,
    LPDWORD a10,
    LPFILETIME a11)
{
    LONG rv = 0;
    __try 
	{
		InitializeRegistry();
		HKEY localKey = GetPrivateKey(hKey);
		if ((rv = Real_RegQueryInfoKeyA(localKey, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11)) != ERROR_SUCCESS)
		{
			rv = Real_RegQueryInfoKeyA(hKey, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11);
		}
		LOG_FOUND_KEY(_T("RegQueryInfoKeyA Found Key %s"), a1)
    }
    __finally {
    };
    return rv;
}

LONG WINAPI Mine_RegQueryInfoKeyW(HKEY hKey,
    LPWSTR a1,
    LPDWORD a2,
    LPDWORD a3,
    LPDWORD a4,
    LPDWORD a5,
    LPDWORD a6,
    LPDWORD a7,
    LPDWORD a8,
    LPDWORD a9,
    LPDWORD a10,
    LPFILETIME a11)
{
    LONG rv = 0;
    __try 
	{
		InitializeRegistry();
		HKEY localKey = GetPrivateKey(hKey);
		if ((rv = Real_RegQueryInfoKeyW(localKey, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11)) != ERROR_SUCCESS)
		{
			rv = Real_RegQueryInfoKeyW(hKey, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11);
		}
		LOG_FOUND_KEY(_T("RegQueryInfoKeyW Found Key %S"), a1)
    }
    __finally {
    };
    return rv;
}

LONG WINAPI Mine_RegQueryValueExA(HKEY hKey,
    LPCSTR lpValueName,
    LPDWORD lpReserved,
    LPDWORD lpType,
    LPBYTE lpData,
    LPDWORD lpcbData)
{
    LONG rv = 0;
    __try 
	{
		InitializeRegistry();
		HKEY localKey = GetPrivateKey(hKey);
		if ((rv = Real_RegQueryValueExA(localKey, lpValueName, lpReserved, lpType, lpData, lpcbData)) != ERROR_SUCCESS)
		{
			rv = Real_RegQueryValueExA(hKey, lpValueName, lpReserved, lpType, lpData, lpcbData);
		}
		LOG_FOUND_KEY(_T("RegQueryValueExA Found Key %s"), lpValueName)
    }
    __finally {
    };
    return rv;
}

LONG WINAPI Mine_RegQueryValueExW(HKEY hKey,
    LPCWSTR lpValueName,
    LPDWORD lpReserved,
    LPDWORD lpType,
    LPBYTE lpData,
    LPDWORD lpcbData)
{
    LONG rv = 0;
    __try 
	{
		InitializeRegistry();
		HKEY localKey = GetPrivateKey(hKey);
		if ((rv = Real_RegQueryValueExW(localKey, lpValueName, lpReserved, lpType, lpData, lpcbData)) != ERROR_SUCCESS)
		{
			rv = Real_RegQueryValueExW(hKey, lpValueName, lpReserved, lpType, lpData, lpcbData);
		}
		LOG_FOUND_KEY(_T("RegQueryValueExW Found Key %S"), lpValueName)
    }
    __finally {
    };
    return rv;
}

LONG WINAPI Mine_RegSetValueExA(HKEY hKey,
    LPCSTR a1,
    DWORD a2,
    DWORD a3,
    BYTE* a4,
    DWORD a5)
{
    LONG rv = 0;
    __try 
	{
		InitializeRegistry();
		HKEY localKey = GetPrivateKey(hKey);
		if ((rv = Real_RegSetValueExA(localKey, a1, a2, a3, a4, a5)) != ERROR_SUCCESS)
		{
			rv = Real_RegSetValueExA(hKey, a1, a2, a3, a4, a5);
		}
		LOG_FOUND_KEY(_T("RegSetValueExA Found Key %s"), a1)
    }
    __finally {
    };
    return rv;
}

LONG WINAPI Mine_RegSetValueExW(HKEY hKey,
    LPCWSTR a1,
    DWORD a2,
    DWORD a3,
    BYTE* a4,
    DWORD a5)
{
    LONG rv = 0;
    __try 
	{
		InitializeRegistry();
		HKEY localKey = GetPrivateKey(hKey);
		if ((rv = Real_RegSetValueExW(localKey, a1, a2, a3, a4, a5)) != ERROR_SUCCESS)
		{
			rv = Real_RegSetValueExW(hKey, a1, a2, a3, a4, a5);
		}
		LOG_FOUND_KEY(_T("RegSetValueExW Found Key %S"), a1)
    }
    __finally {
    };
    return rv;
}

/////////////////////////////////////////////////////////////
// AttachDetours
//
PCHAR DetRealName(PCHAR psz)
{
    PCHAR pszBeg = psz;
    // Move to end of name.
    while (*psz) {
        psz++;
    }
    // Move back through A-Za-z0-9 names.
    while (psz > pszBeg &&
        ((psz[-1] >= 'A' && psz[-1] <= 'Z') ||
        (psz[-1] >= 'a' && psz[-1] <= 'z') ||
        (psz[-1] >= '0' && psz[-1] <= '9'))) {
        psz--;
    }
    return psz;
}

VOID DetAttach(PVOID *ppbReal, PVOID pbMine, PCHAR psz)
{
    LONG l = DetourAttach(ppbReal, pbMine);
    if (l != 0) {
        REG_LOG(_T("Attach failed: `%s': error %d\n"), DetRealName(psz), l);
    }
}

VOID DetDetach(PVOID *ppbReal, PVOID pbMine, PCHAR psz)
{
    LONG l = DetourDetach(ppbReal, pbMine);
    if (l != 0) {        REG_LOG(_T("Detach failed: `%s': error %d\n"), DetRealName(psz), l);
    }
}

#define ATTACH(x)       DetAttach(&(PVOID&)Real_##x,Mine_##x,#x)
#define DETACH(x)       DetDetach(&(PVOID&)Real_##x,Mine_##x,#x)

LONG AttachDetours(VOID)
{
    DetourTransactionBegin();
    DetourUpdateThread(GetCurrentThread());
   
	ATTACH(CreateProcessA);
	ATTACH(CreateProcessW);
    ATTACH(RegCreateKeyExA);
    ATTACH(RegCreateKeyExW);
    ATTACH(RegDeleteKeyA);
    ATTACH(RegDeleteKeyW);
    ATTACH(RegDeleteValueA);
    ATTACH(RegDeleteValueW);
    ATTACH(RegEnumKeyExA);
    ATTACH(RegEnumKeyExW);
    ATTACH(RegEnumValueA);
    ATTACH(RegEnumValueW);
    ATTACH(RegOpenKeyExA);
    ATTACH(RegOpenKeyExW);
    ATTACH(RegQueryInfoKeyA);
    ATTACH(RegQueryInfoKeyW);
    ATTACH(RegQueryValueExA);
    ATTACH(RegQueryValueExW);
    ATTACH(RegSetValueExA);
    ATTACH(RegSetValueExW);
    
    return DetourTransactionCommit();
}

LONG DetachDetours(VOID)
{
    DetourTransactionBegin();
    DetourUpdateThread(GetCurrentThread());

	DETACH(CreateProcessA);
	DETACH(CreateProcessW);
    DETACH(RegCreateKeyExA);
    DETACH(RegCreateKeyExW);
    DETACH(RegDeleteKeyA);
    DETACH(RegDeleteKeyW);
    DETACH(RegDeleteValueA);
    DETACH(RegDeleteValueW);
    DETACH(RegEnumKeyExA);
    DETACH(RegEnumKeyExW);
    DETACH(RegEnumValueA);
    DETACH(RegEnumValueW);
    DETACH(RegOpenKeyExA);
    DETACH(RegOpenKeyExW);
    DETACH(RegQueryInfoKeyA);
    DETACH(RegQueryInfoKeyW);
    DETACH(RegQueryValueExA);
    DETACH(RegQueryValueExW);
    DETACH(RegSetValueExA);
    DETACH(RegSetValueExW);
    
    return DetourTransactionCommit();
}
//
//////////////////////////////////////////////////////////////////////////////


////////////////////////////////////////////////////////////// Logging System.
//
static BOOL g_bLog = 1;
static LONG g_nTlsIndent = -1;
static LONG g_nTlsThread = -1;
static LONG g_nThreadCnt = 0;

VOID _PrintEnter(const TCHAR *psz, ...)
{
    DWORD dwErr = GetLastError();

    LONG nIndent = 0;
    LONG nThread = 0;
    if (g_nTlsIndent >= 0) {
        nIndent = (LONG)(LONG_PTR)TlsGetValue(g_nTlsIndent);
        TlsSetValue(g_nTlsIndent, (PVOID)(LONG_PTR)(nIndent + 1));
    }
    if (g_nTlsThread >= 0) {
        nThread = (LONG)(LONG_PTR)TlsGetValue(g_nTlsThread);
    }

    if (g_bLog && psz) {
        TCHAR szBuf[1024];
        PTCHAR pszBuf = szBuf;
        PTCHAR pszEnd = szBuf + ARRAYSIZE(szBuf) - 1;
        LONG nLen = (nIndent > 0) ? (nIndent < 35 ? nIndent * 2 : 70) : 0;
        *pszBuf++ = (CHAR)('0' + ((nThread / 100) % 10));
        *pszBuf++ = (CHAR)('0' + ((nThread / 10) % 10));
        *pszBuf++ = (CHAR)('0' + ((nThread / 1) % 10));
        *pszBuf++ = ' ';
        while (nLen-- > 0) {
            *pszBuf++ = ' ';
        }

        va_list  args;
        va_start(args, psz);

        while ((*pszBuf++ = *psz++) != 0 && pszBuf < pszEnd) {
            // Copy characters.
        }
        *pszEnd = '\0';


        TCHAR tempStr[LOG_BUF_SIZE];
		_vstprintf_s(tempStr, szBuf, args);
        OutputDebugString(tempStr);

        va_end(args);
    }
    SetLastError(dwErr);
}

VOID _PrintExit(const TCHAR *psz, ...)
{
    DWORD dwErr = GetLastError();

    LONG nIndent = 0;
    LONG nThread = 0;
    if (g_nTlsIndent >= 0) {
        nIndent = (LONG)(LONG_PTR)TlsGetValue(g_nTlsIndent) - 1;
        //ASSERT(nIndent >= 0);
        TlsSetValue(g_nTlsIndent, (PVOID)(LONG_PTR)nIndent);
    }
    if (g_nTlsThread >= 0) {
        nThread = (LONG)(LONG_PTR)TlsGetValue(g_nTlsThread);
    }

    if (g_bLog && psz) {
        TCHAR szBuf[1024];
        PTCHAR pszBuf = szBuf;
        PTCHAR pszEnd = szBuf + ARRAYSIZE(szBuf) - 1;
        LONG nLen = (nIndent > 0) ? (nIndent < 35 ? nIndent * 2 : 70) : 0;
        *pszBuf++ = (CHAR)('0' + ((nThread / 100) % 10));
        *pszBuf++ = (CHAR)('0' + ((nThread / 10) % 10));
        *pszBuf++ = (CHAR)('0' + ((nThread / 1) % 10));
        *pszBuf++ = ' ';
        while (nLen-- > 0) {
            *pszBuf++ = ' ';
        }

        va_list  args;
        va_start(args, psz);

        while ((*pszBuf++ = *psz++) != 0 && pszBuf < pszEnd) {
            // Copy characters.
        }
        *pszEnd = '\0';

        TCHAR tempStr[4096];
		_vstprintf_s(tempStr, szBuf, args);
        OutputDebugString(tempStr);

        va_end(args);
    }
    SetLastError(dwErr);
}

VOID _Print(const TCHAR *psz, ...)
{
    DWORD dwErr = GetLastError();

    LONG nIndent = 0;
    LONG nThread = 0;
    if (g_nTlsIndent >= 0) {
        nIndent = (LONG)(LONG_PTR)TlsGetValue(g_nTlsIndent);
    }
    if (g_nTlsThread >= 0) {
        nThread = (LONG)(LONG_PTR)TlsGetValue(g_nTlsThread);
    }

    if (g_bLog && psz) {
        TCHAR szBuf[1024];
        PTCHAR pszBuf = szBuf;
        PTCHAR pszEnd = szBuf + ARRAYSIZE(szBuf) - 1;
        LONG nLen = (nIndent > 0) ? (nIndent < 35 ? nIndent * 2 : 70) : 0;
        *pszBuf++ = (CHAR)('0' + ((nThread / 100) % 10));
        *pszBuf++ = (CHAR)('0' + ((nThread / 10) % 10));
        *pszBuf++ = (CHAR)('0' + ((nThread / 1) % 10));
        *pszBuf++ = ' ';
        while (nLen-- > 0) {
            *pszBuf++ = ' ';
        }

        va_list  args;
        va_start(args, psz);

        while ((*pszBuf++ = *psz++) != 0 && pszBuf < pszEnd) {
            // Copy characters.
        }
        *pszEnd = '\0';

        TCHAR tempStr[LOG_BUF_SIZE];
		_vstprintf_s(tempStr, szBuf, args);
        OutputDebugString(tempStr);

        va_end(args);
    }

    SetLastError(dwErr);
}

VOID AssertMessage(CONST PTCHAR pszMsg, CONST PTCHAR pszFile, ULONG nLine)
{
    REG_LOG(_T("ASSERT(%s) failed in %s, line %d.\n"), pszMsg, pszFile, nLine);
}

//////////////////////////////////////////////////////////////////////////////
//
PIMAGE_NT_HEADERS NtHeadersForInstance(HINSTANCE hInst)
{
    PIMAGE_DOS_HEADER pDosHeader = (PIMAGE_DOS_HEADER)hInst;
    __try {
        if (pDosHeader->e_magic != IMAGE_DOS_SIGNATURE) {
            SetLastError(ERROR_BAD_EXE_FORMAT);
            return NULL;
        }

        PIMAGE_NT_HEADERS pNtHeader = (PIMAGE_NT_HEADERS)((PBYTE)pDosHeader +
            pDosHeader->e_lfanew);
        if (pNtHeader->Signature != IMAGE_NT_SIGNATURE) {
            SetLastError(ERROR_INVALID_EXE_SIGNATURE);
            return NULL;
        }
        if (pNtHeader->FileHeader.SizeOfOptionalHeader == 0) {
            SetLastError(ERROR_EXE_MARKED_INVALID);
            return NULL;
        }
        return pNtHeader;
    }
    __except (EXCEPTION_EXECUTE_HANDLER) {
    }
    SetLastError(ERROR_EXE_MARKED_INVALID);

    return NULL;
}

BOOL InstanceEnumerate(HINSTANCE hInst)
{
    TCHAR wzDllName[MAX_PATH];

    PIMAGE_NT_HEADERS pinh = NtHeadersForInstance(hInst);
    if (pinh && GetModuleFileName(hInst, wzDllName, ARRAYSIZE(wzDllName))) {
        REG_LOG(_T("%x %s %x\n"),reinterpret_cast<ULONG>(hInst), wzDllName, pinh->OptionalHeader.CheckSum);
        return TRUE;
    }
    return FALSE;
}

BOOL ProcessEnumerate()
{
    REG_LOG(_T("######################################################### Binaries\n"));
    for (HINSTANCE hInst = NULL; (hInst = DetourEnumerateModules(hInst)) != NULL;) {
        InstanceEnumerate(hInst);
    }
    return TRUE;
}

//////////////////////////////////////////////////////////////////////////////
//
// DLL module information
//
BOOL ThreadAttach(HMODULE hDll)
{
    (void)hDll;

    if (g_nTlsIndent >= 0) {
        TlsSetValue(g_nTlsIndent, (PVOID)0);
    }
    if (g_nTlsThread >= 0) {
        LONG nThread = InterlockedIncrement(&g_nThreadCnt);
        TlsSetValue(g_nTlsThread, (PVOID)(LONG_PTR)nThread);
    }
    return TRUE;
}

BOOL ThreadDetach(HMODULE hDll)
{
    (void)hDll;

    if (g_nTlsIndent >= 0) {
        TlsSetValue(g_nTlsIndent, (PVOID)0);
    }
    if (g_nTlsThread >= 0) {
        TlsSetValue(g_nTlsThread, (PVOID)0);
    }
    return TRUE;
}

BOOL ProcessAttach(HMODULE hDll)
{
    g_bLog = FALSE;
    g_nTlsIndent = TlsAlloc();
    g_nTlsThread = TlsAlloc();

    g_hInst = hDll;
    GetModuleFileName(g_hInst, g_szDllPath, ARRAYSIZE(g_szDllPath));

    ProcessEnumerate();

    LONG error = AttachDetours();
    if (error != NO_ERROR) {
        REG_LOG(_T("Error attaching detours: %d\n"), error);
    }

    ThreadAttach(hDll);

    g_bLog = TRUE;
    return TRUE;
}

BOOL ProcessDetach(HMODULE hDll)
{
    _PrintEnter(_T("ProcessDetach"));
    ThreadDetach(hDll);
    g_bLog = FALSE;

    LONG error = DetachDetours();
    if (error != NO_ERROR) {
        REG_LOG(_T("Error detaching detours: %d\n"), error);
    }

    if (g_nTlsIndent >= 0) {
        TlsFree(g_nTlsIndent);
    }
    if (g_nTlsThread >= 0) {
        TlsFree(g_nTlsThread);
    }
    return TRUE;
}

void InitializeRegistryInternal()
{
	CHAR tempBuff[TEMP_BUFF_SIZE];
	GetModuleFileNameA(g_hInst, tempBuff, TEMP_BUFF_SIZE);
	string exeFileLocation = tempBuff;
	string exeFileFolder = exeFileLocation.substr(0, exeFileLocation.find_last_of(_T("\\/")));

	string registryFile = exeFileFolder + _T("\\private_registry.dat");
	HKEY mainKey = NULL;

	REG_LOG(_T("Private Registry File: %s"), registryFile.c_str());

	if (RegLoadAppKey(registryFile.c_str(), &mainKey, KEY_ALL_ACCESS, 0, 0) == ERROR_SUCCESS)
	{
		if (mainKey != NULL)
		{
			REG_LOG(_T("Private Registry File mainKey: %d"), (DWORD)mainKey);
			g_hKeyFile = mainKey;

			HKEY localMachine = NULL;
			HKEY classRoot = NULL;
			HKEY currentUser = NULL;
			LONG res1 = Real_RegOpenKeyExA(g_hKeyFile, _T("HKEY_LOCAL_MACHINE"), 0, KEY_ALL_ACCESS, &localMachine);
			if (localMachine != NULL)
			{
				REG_LOG(_T("Private Registry File HKEY_LOCAL_MACHINE: %d"), (DWORD)localMachine);
				g_hKeyLocalMachine = localMachine;
			}
			LONG res2 = Real_RegOpenKeyExA(g_hKeyFile, _T("HKEY_CLASSES_ROOT"), 0, KEY_ALL_ACCESS, &classRoot);
			if (classRoot != NULL)
			{
				REG_LOG(_T("Private Registry File HKEY_CLASSES_ROOT: %d"), (DWORD)classRoot);
				g_hKeyClassRoot = classRoot;
			}
			LONG res3 = Real_RegOpenKeyExA(g_hKeyFile, _T("HKEY_CURRENT_USER"), 0, KEY_ALL_ACCESS, &currentUser);
			if (currentUser != NULL)
			{
				REG_LOG(_T("Private Registry File HKEY_CURRENT_USER: %d"), (DWORD)currentUser);
				g_hKeyCurrentUser = currentUser;
			}
		}
	}
	else
	{
		REG_LOG(_T("Private Registry File mainKey: %s RegLoadAppKey failed"), registryFile.c_str());
	}
}