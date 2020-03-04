using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LaunchEnvironment.RegConfig
{
    internal static class RegistryNativeMethods
    {
        [Flags]
        internal enum RegSAM
        {
            AllAccess = 0xF003F,
            QueryValue = 0x0001
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/ms724884(v=vs.85).aspx
        /// </summary>
        internal enum RFlags
        {
            /// <summary>
            /// Any
            /// </summary>
            Any = 65535,

            /// <summary>
            /// No defined value type.
            /// </summary>
            RegNone = 1,

            /// <summary>
            /// ???
            /// </summary>
            Noexpand = 268435456,

            /// <summary>
            /// Bytes
            /// </summary>
            RegBinary = 8,

            /// <summary>
            /// Int32
            /// </summary>
            Dword = 24,

            /// <summary>
            /// Int32
            /// </summary>
            RegDword = 16,

            /// <summary>
            /// Int64
            /// </summary>
            Qword = 72,

            /// <summary>
            /// Int64
            /// </summary>
            RegQword = 64,

            /// <summary>
            /// A null-terminated string.
            /// This will be either a Unicode or an ANSI string,
            /// depending on whether you use the Unicode or ANSI functions.
            /// </summary>
            RegSz = 2,

            /// <summary>
            /// A sequence of null-terminated strings, terminated by an empty string (\0).
            /// The following is an example:
            /// String1\0String2\0String3\0LastString\0\0
            /// The first \0 terminates the first string, the second to the last \0 terminates the last string, 
            /// and the final \0 terminates the sequence. Note that the final terminator must be factored into the length of the string.
            /// </summary>
            RegMultiSz = 32,

            /// <summary>
            /// A null-terminated string that contains unexpanded references to environment variables (for example, "%PATH%").
            /// It will be a Unicode or ANSI string depending on whether you use the Unicode or ANSI functions. 
            /// To expand the environment variable references, use the ExpandEnvironmentStrings function.
            /// </summary>
            RegExpandSz = 4,

            RrfZeroonfailure = 536870912
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/ms724884(v=vs.85).aspx
        /// </summary>
        internal enum RType
        {
            RegNone = 0,

            RegSz = 1,
            RegExpandSz = 2,
            RegMultiSz = 7,

            RegBinary = 3,
            RegDword = 4,
            RegQword = 11,

            RegQwordLittleEndian = 11,
            RegDwordLittleEndian = 4,
            RegDwordBigEndian = 5,

            RegLink = 6,
            RegResourceList = 8,
            RegFullResourceDescriptor = 9,
            RegResourceRequirementsList = 10,
        }

        internal enum RegFileFormat
        {
            /// <summary>
            /// The key or hive is saved in standard format. The standard format is the only format supported by Windows 2000.
            /// </summary>
            REG_STANDARD_FORMAT = 1,
            /// <summary>
            /// The key or hive is saved in the latest format. The latest format is supported starting with Windows XP. 
            /// After the key or hive is saved in this format, it cannot be loaded on an earlier system.
            /// </summary>
            REG_LATEST_FORMAT = 2,
            /// <summary>
            /// The hive is saved with no compression, for faster save operations. 
            /// The hKey parameter must specify the root of a hive under HKEY_LOCAL_MACHINE or HKEY_USERS. For example, HKLM\SOFTWARE is the root of a hive.
            /// </summary>
            REG_NO_COMPRESSION = 4
        }

        private const int REG_PROCESS_APPKEY = 0x00000001;

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern int RegLoadAppKey(String hiveFile, out int hKey, RegSAM samDesired, int options, int reserved);

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern Int32 RegLoadKey(IntPtr hKey, String lpSubKey, String lpFile);

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern uint RegSaveKey(IntPtr hKey, string lpFile, IntPtr lpSecurityAttributes);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto)]
        internal static extern int RegSaveKeyEx(SafeRegistryHandle hKey, string fileName, IntPtr lpSecurityAttributes, int flags);

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern uint RegSaveKeyEx(IntPtr hKey, string lpFile, IntPtr lpSecurityAttributes, uint Flags);

        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern uint RegUnLoadKey(IntPtr hKey, string SubKey);


        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern int RegCloseKey(IntPtr hKey);

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern int RegOpenKeyEx(IntPtr hKey, string subKey, int ulOptions, int samDesired, out IntPtr hkResult);

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern uint RegSetValueEx(IntPtr hKey, [MarshalAs(UnmanagedType.LPStr)] string lpValueName, int Reserved, RegistryValueKind dwType, IntPtr lpData, int cbData);

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern uint RegQueryValueEx(IntPtr hKey, string lpValueName, int lpReserved, ref RegistryValueKind lpType, IntPtr lpData,ref int lpcbData);

        [DllImport("Advapi32.dll", EntryPoint = "RegGetValueW", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern uint RegGetValue(IntPtr hkey,string lpSubKey,string lpValue,RFlags dwFlags,out RType pdwType,IntPtr pvData,ref uint pcbData);

        internal static RegistryKey RegLoadAppKey(String hiveFile)
        {
            int hKey;
            int rc = RegLoadAppKey(hiveFile, out hKey, RegSAM.AllAccess, REG_PROCESS_APPKEY, 0);

            if (rc != 0)
            {
                if (rc == 0x20)
                {
                    ErrorLog.Inst.LogError("Please Close All the instance of the Visual Studio");
                }
                else
                {
                    ErrorLog.Inst.LogError("Unknown Error code : {0}", rc);
                }
            }

            return RegistryKey.FromHandle(new Microsoft.Win32.SafeHandles.SafeRegistryHandle(new IntPtr(hKey), true)); 
        }

    }
}
