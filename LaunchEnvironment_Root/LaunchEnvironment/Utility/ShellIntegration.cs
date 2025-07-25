﻿using LaunchEnvironment.Config;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LaunchEnvironment.Utility
{
    public class ShellIntegration
    {
        public static bool RegisterContextMenu()
        {
            if (!Debugger.IsAttached && UserConfig.Inst.IsElevated)
            {
                //Registry.SetValue(@"HKEY_CLASSES_ROOT\Folder\shell\LaunchEnvironment\command", "", string.Format("{0} \"%1\"", Assembly.GetExecutingAssembly().Location));

                //Registry.SetValue(@"HKEY_CLASSES_ROOT\Directory\shell\LaunchEnvironment", "", "");
                Registry.SetValue(@"HKEY_CLASSES_ROOT\Directory\shell\LaunchEnvironment\command", "", string.Format("{0} \"%V\"", Assembly.GetExecutingAssembly().Location));

                Registry.SetValue(@"HKEY_CLASSES_ROOT\Directory\Background\shell\LaunchEnvironment", "NoWorkingDirectory", "");
                Registry.SetValue(@"HKEY_CLASSES_ROOT\Directory\Background\shell\LaunchEnvironment\command", "", string.Format("{0} \"%V\"", Assembly.GetExecutingAssembly().Location));

                return true;
            }
            else if (!Debugger.IsAttached && !UserConfig.Inst.IsElevated)
            {

                Registry.SetValue(@"HKEY_CURRENT_USER\Software\Classes\Directory\shell\LaunchEnvironment\command", "", string.Format("{0} \"%V\"", Assembly.GetExecutingAssembly().Location));

                Registry.SetValue(@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\LaunchEnvironment", "NoWorkingDirectory", "");
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\LaunchEnvironment\command", "", string.Format("{0} \"%V\"", Assembly.GetExecutingAssembly().Location));
            }

            return false;
        }
    }
}
