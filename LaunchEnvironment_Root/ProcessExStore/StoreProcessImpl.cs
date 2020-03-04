using ProcessEx;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessExStore
{
    public class StoreProcessImpl : IProcessEx
    {
        uint _pid;

        public int Pid { get => (int) _pid; }

        /// <summary>
        /// HKEY_CURRENT_USER\Software\Classes\ActivatableClasses\Package
        /// HKEY_CURRENT_USER\Software\RegisteredApplications
        /// HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\PackageRepository\Extensions\ProgIDs\AppXh09fg0r1jvyz62yqhm5yw1v5jknrdxfr
        /// IEnumerable<Windows.ApplicationModel.Package> packages = (IEnumerable<Windows.ApplicationModel.Package>) packageManager.FindPackagesForUser("");
        /// </summary>
        /// <param name="appName"></param>
        /// <returns></returns>
        private string GetAppUseModelID(string appName)
        {
            string retVal = "";

            return retVal;
        }
        public Process Launch(ProcessStartInfo startInfo)
        {
            ApplicationActivationManager appActiveManager = new ApplicationActivationManager();//Class not registered
            string appUserModelId = GetAppUseModelID(startInfo.FileName);
            appActiveManager.ActivateApplication(appUserModelId,startInfo.Arguments, ActivateOptions.None, out _pid);
            if (_pid != 0)
            {
                return Process.GetProcessById((int)_pid);
            }
            else
            {
                return null;
            }
        }
    }
}
