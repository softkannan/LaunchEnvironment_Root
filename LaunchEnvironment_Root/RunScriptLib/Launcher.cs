using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ProcessEx;

namespace RunScriptLib
{
    public class LaunchProcess
    {

        public Process Launch(ProcessStartInfo procInfo,bool inheritHandle,string injectDll = "")
        {
            if (procInfo.UseShellExecute == false)
            {
                var injectProcess = new ProcessEx.CustomProcess();

                var retProc = injectProcess.LaunchProcessSuspended(procInfo, inheritHandle);
                if (!string.IsNullOrWhiteSpace(injectDll))
                {
                    DllInject.InjectDll(injectDll, retProc);
                }

                injectProcess.ResumeProcess();
                return retProc;
            }
            else
            {
                return Process.Start(procInfo);
            }
        }
    }
}
