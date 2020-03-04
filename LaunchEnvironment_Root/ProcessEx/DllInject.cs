using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ProcessEx
{
    public class DllInject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dllName">name of the dll we want to inject</param>
        /// <param name="procName"></param>
        /// <returns></returns>
        public static int InjectDll(string dllName,string procName)
        {
            // the target process - I'm using a dummy process for this
            // if you don't have one, open Task Manager and choose wisely
            var allProc = Process.GetProcessesByName(procName);

            if (allProc != null && allProc.Length > 0)
            {
                Process targetProcess = allProc[0];
                InjectDll(dllName, targetProcess);
                return 0;
            }
            return 1;
        }

        public static void InjectDll(string dllName, Process targetProcess)
        {
            // geting the handle of the process - with required privileges
            IntPtr procHandle = NativeMethods.OpenProcess(NativeMethods.PROCESS_CREATE_THREAD | NativeMethods.PROCESS_QUERY_INFORMATION | NativeMethods.PROCESS_VM_OPERATION |
                NativeMethods.PROCESS_VM_WRITE | NativeMethods.PROCESS_VM_READ, false, targetProcess.Id);

            // searching for the address of LoadLibraryA and storing it in a pointer
            IntPtr loadLibraryAddr = NativeMethods.GetProcAddress(NativeMethods.GetModuleHandle("kernel32.dll"), "LoadLibraryA");

            // alocating some memory on the target process - enough to store the name of the dll
            // and storing its address in a pointer
            IntPtr allocMemAddress = NativeMethods.VirtualAllocEx(procHandle, IntPtr.Zero, (uint)((dllName.Length + 1) * Marshal.SizeOf(typeof(char))), NativeMethods.MEM_COMMIT | NativeMethods.MEM_RESERVE,
                NativeMethods.PAGE_READWRITE);

            // writing the name of the dll there
            UIntPtr bytesWritten;
            NativeMethods.WriteProcessMemory(procHandle, allocMemAddress, Encoding.Default.GetBytes(dllName), (uint)((dllName.Length + 1) * Marshal.SizeOf(typeof(char))), out bytesWritten);

            // creating a thread that will call LoadLibraryA with allocMemAddress as argument
            NativeMethods.CreateRemoteThread(procHandle, IntPtr.Zero, 0, loadLibraryAddr, allocMemAddress, 0, IntPtr.Zero);
        }
    }
}
