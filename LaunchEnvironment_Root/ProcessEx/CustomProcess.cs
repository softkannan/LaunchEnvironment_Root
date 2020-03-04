using Microsoft.Win32.SafeHandles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ProcessEx
{
    public class CustomProcess
    {
        internal class OrdinalCaseInsensitiveComparer : IComparer
        {
            internal static readonly OrdinalCaseInsensitiveComparer Default = new OrdinalCaseInsensitiveComparer();

            public int Compare(Object a, Object b)
            {
                String sa = a as String;
                String sb = b as String;
                if (sa != null && sb != null)
                {
                    return String.Compare(sa, sb, StringComparison.OrdinalIgnoreCase);
                }
                return Comparer.Default.Compare(a, b);
            }
        }
        internal static class EnvironmentBlock
        {
            public static byte[] ToByteArray(StringDictionary sd, bool unicode)
            {
                // get the keys
                string[] keys = new string[sd.Count];
                byte[] envBlock = null;
                sd.Keys.CopyTo(keys, 0);

                // get the values
                string[] values = new string[sd.Count];
                sd.Values.CopyTo(values, 0);

                // sort both by the keys
                // Windows 2000 requires the environment block to be sorted by the key
                // It will first converting the case the strings and do ordinal comparison.
                Array.Sort(keys, values, OrdinalCaseInsensitiveComparer.Default);

                // create a list of null terminated "key=val" strings
                StringBuilder stringBuff = new StringBuilder();
                for (int i = 0; i < sd.Count; ++i)
                {
                    stringBuff.Append(keys[i]);
                    stringBuff.Append('=');
                    stringBuff.Append(values[i]);
                    stringBuff.Append('\0');
                }
                // an extra null at the end indicates end of list.
                stringBuff.Append('\0');

                if (unicode)
                {
                    envBlock = Encoding.Unicode.GetBytes(stringBuff.ToString());
                }
                else
                {
                    envBlock = Encoding.Default.GetBytes(stringBuff.ToString());

                    if (envBlock.Length > UInt16.MaxValue)
                        throw new InvalidOperationException("Environment Block Too Long");
                }

                return envBlock;
            }
        }

        private IntPtr _threadHandle = IntPtr.Zero;
        public IntPtr ThreadHandle { get { return _threadHandle; } }
        private IntPtr _procHandle = IntPtr.Zero;
        public IntPtr ProcHandle { get { return _procHandle; } }
        private uint _pid = 0;
        public int PID { get { return (int)_pid; } }

        private uint _tid = 0;
        public int ThreadId { get { return (int)_tid; } }

        public bool Is64BitProcess { get; private set; }
        public string BootstrapProcess { get; set; }
        private static StringBuilder BuildCommandLine(string executableFileName, string arguments)
        {
            // Construct a StringBuilder with the appropriate command line
            // to pass to CreateProcess.  If the filename isn't already 
            // in quotes, we quote it here.  This prevents some security
            // problems (it specifies exactly which part of the string
            // is the file to execute).
            StringBuilder commandLine = new StringBuilder();
            string fileName = executableFileName.Trim();
            bool fileNameIsQuoted = (fileName.StartsWith("\"", StringComparison.Ordinal) && fileName.EndsWith("\"", StringComparison.Ordinal));
            if (!fileNameIsQuoted)
            {
                commandLine.Append("\"");
            }

            commandLine.Append(fileName);

            if (!fileNameIsQuoted)
            {
                commandLine.Append("\"");
            }

            if (!String.IsNullOrWhiteSpace(arguments))
            {
                commandLine.Append(" ");
                commandLine.Append(arguments);
            }

            return commandLine;
        }

        public Process Launch(ProcessStartInfo startInfo, bool inheritHandle = false)
        {
            var retVal = LaunchProcessSuspended(startInfo, inheritHandle);
            ResumeProcess();
            return retVal;
        }

        public Process LaunchProcessSuspended(ProcessStartInfo startInfo, bool inheritHandle = false)
        {
            string commandLine;

            if (!string.IsNullOrWhiteSpace(startInfo.Verb))
            {
                commandLine = string.Format("-runverb -verb \"{0}\" -exefile \"{1}\"", 
                    startInfo.Verb.Trim(),startInfo.FileName.Trim());

                string tempArg = startInfo.Arguments.Trim();
                if(!string.IsNullOrWhiteSpace(tempArg))
                {
                    commandLine+= string.Format(" -arg \"{0}\"",tempArg);
                }

                startInfo.FileName = BootstrapProcess;
            }
            else
            {
                commandLine = startInfo.Arguments;
            }

            commandLine = BuildCommandLine(startInfo.FileName.Trim(), commandLine.Trim()).ToString();

            var startupInfo = new STARTUPINFO();
            var processInfo = new PROCESS_INFORMATION();
            bool retVal = false;
            int errorCode = 0;
            GCHandle environmentHandle = new GCHandle();
            try
            {
                // set up the creation flags paramater
                var creationFlags = ProcessCreationFlags.CREATE_SUSPENDED;

                if (startInfo.CreateNoWindow) creationFlags |= ProcessCreationFlags.CREATE_NO_WINDOW;

                // set up the environment block parameter
                IntPtr environmentPtr = (IntPtr)0;
                if (startInfo.EnvironmentVariables != null)
                {
                    creationFlags |= ProcessCreationFlags.CREATE_UNICODE_ENVIRONMENT;
                    byte[] environmentBytes = EnvironmentBlock.ToByteArray(startInfo.EnvironmentVariables, true);
                    environmentHandle = GCHandle.Alloc(environmentBytes, GCHandleType.Pinned);
                    environmentPtr = environmentHandle.AddrOfPinnedObject();
                }

                //fix working dir
                string workingDirectory = startInfo.WorkingDirectory;
                if (workingDirectory == string.Empty)
                {
                    workingDirectory = Environment.CurrentDirectory;
                }

                try { }
                finally
                {
                    retVal = NativeMethods.CreateProcess(
                            null,                           // we don't need this since all the info is in commandLine
                            commandLine,         // pointer to the command line string
                            IntPtr.Zero,                    // pointer to process security attributes, we don't need to inheriat the handle
                            IntPtr.Zero,                    // pointer to thread security attributes
                            inheritHandle,                  // handle inheritance flag
                            creationFlags,                  // creation flags
                            environmentPtr,                 // pointer to new environment block
                            workingDirectory,               // pointer to current directory name
                            ref startupInfo,                // pointer to STARTUPINFO
                            out processInfo                 // pointer to PROCESS_INFORMATION
                        );
                    if (!retVal)
                    {
                        errorCode = Marshal.GetLastWin32Error();
                    }
                    if (processInfo.hProcess != (IntPtr)0 && processInfo.hProcess != (IntPtr)NativeMethods.INVALID_HANDLE_VALUE)
                    {
                        _pid = processInfo.dwProcessId;
                        _procHandle = processInfo.hProcess;
                    }
                    if (processInfo.hThread != (IntPtr)0 && processInfo.hThread != (IntPtr)NativeMethods.INVALID_HANDLE_VALUE)
                    {
                        _threadHandle = processInfo.hThread;
                        _tid = processInfo.dwThreadId;
                    }
                }
                if (!retVal)
                {
                    if (errorCode == NativeMethods.ERROR_BAD_EXE_FORMAT || errorCode == NativeMethods.ERROR_EXE_MACHINE_TYPE_MISMATCH)
                    {
                        throw new Win32Exception(errorCode, "Invalid Application");
                    }
                    throw new Win32Exception(errorCode);
                }
                retVal = true;
            }
            finally
            {
                // free environment block
                if (environmentHandle.IsAllocated)
                {
                    environmentHandle.Free();
                }
            }

            if (_pid != 0)
            {
                return Process.GetProcessById((int)_pid);
            }
            else
            {
                return null;
            }
        }
        public Process LaunchProcessSuspended(ProcessStartInfo processInfo, bool inheritHandle,int initialResumeTime = 0)
        {
            var retVal = LaunchProcessSuspended(processInfo, inheritHandle);

            if (initialResumeTime > 0 && retVal != null)
            {
                NativeMethods.ResumeThread(_threadHandle);
                System.Threading.Thread.Sleep(initialResumeTime);
                NativeMethods.SuspendThread(_threadHandle);
            }

            return retVal;
        }

        public void ResumeProcess()
        {
            NativeMethods.ResumeThread(_threadHandle);
        }
    }
}
