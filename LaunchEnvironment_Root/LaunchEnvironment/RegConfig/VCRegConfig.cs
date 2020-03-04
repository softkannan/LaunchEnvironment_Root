using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchEnvironment.Config;

namespace LaunchEnvironment.RegConfig
{
    public class VCRegConfig : RegConfigBase
    {
        private bool IsRunning
        {
            get
            {
                Process[] pname = Process.GetProcessesByName("devenv");
                return pname.Length != 0;
            }
        }

        //We follow https://visualstudioextensions.vlasovstudio.com/2017/06/29/changing-visual-studio-2017-private-registry-settings/
        //%LOCALAPPDATA%\Microsoft\VisualStudio\15.0_ [id]\privateregistry.bin
        //C:\Users\softk\AppData\Local\Microsoft\VisualStudio\15.0_b59ee41d
        private const string VSVerTag = "15.0_b59ee41d";

        public override void SetSonfig(Config.RegKey regConfig)
        {
            //skip the private registry write if it is already running
            //if (IsRunning)
            //{
            //    //If you want to quickly set a registry setting to a specific value, there is a simpler approach.
            //    //A running Visual Studio 2017 instance not only loads registry keys with the RegLoadAppKey function from the privateregistry.bin file, 
            //    //but also redirects all registry operations under the HKEY_CURRENT_USER\Software\Microsoft\VisualStudio\15.0 key to the private registry

            //    Registry.CurrentUser.GetValue(@"Software\Microsoft\VisualStudio\15.0");

            //}
            //else
            //{
            //    string VS2017Regfile = null;
            //    string vsLocalAppdata = string.Format("{0}\\Microsoft\\VisualStudio\\", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            //    foreach (var item in Directory.GetDirectories(vsLocalAppdata, "15.0*"))
            //    {
            //        VS2017Regfile = Directory.GetFiles(item, "privateregistry.bin").FirstOrDefault();
            //        if (File.Exists(VS2017Regfile))
            //        {
            //            break;
            //        }
            //    }
            //    if (string.IsNullOrEmpty(VS2017Regfile))
            //    {
            //        VS2017Regfile = string.Format("{0}{1}\\privateregistry.bin", vsLocalAppdata, VSVerTag);
            //    }
            //    if (File.Exists(VS2017Regfile))
            //    {
            //        UIntPtr regKey = RegistryNativeMethods.RegLoadAppKey(VS2017Regfile);
            //    }
            //}
        }
    }
}
