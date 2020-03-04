using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessEx
{
    public static class ProcessEx
    {
        public static bool Is64BitProcess(this Process process)
        {
            if (Environment.Is64BitOperatingSystem)
            {
                if ((Environment.OSVersion.Version.Major > 5)
                    || ((Environment.OSVersion.Version.Major == 5) && (Environment.OSVersion.Version.Minor >= 1)))
                {
                    bool retVal;

                    return NativeMethods.IsWow64Process(process.Handle, out retVal) && !retVal;
                }
            }
            return false; // not on 64-bit Windows Emulator
        }
    }
}
