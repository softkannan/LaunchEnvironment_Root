using LaunchEnvironment.Config;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LaunchEnvironment.RegConfig
{
    public class PythonRegConfig : RegConfigBase
    {
        //private bool IsRunning
        //{
        //    get
        //    {
        //        Process[] pname = Process.GetProcessesByName("devenv");
        //        return pname.Length != 0;
        //    }
        //}

        //static RegistryKey _visualStudio = null;

        //protected override RegistryKey LocalMachine
        //{
        //    get
        //    {
        //        return _visualStudio;
        //    }
        //}

        //protected override RegistryKey CurrentUser
        //{
        //    get
        //    {
        //        return _visualStudio;
        //    }
        //}

        //public override void SetSonfig(LaunchConfig config, RegConfig regConfig)
        //{
        //    //if (regConfig.RequireAdmin)
        //    //{
        //    //    if (!RuntimeInfo.Inst.IsElevated)
        //    //    {
        //    //        return;
        //    //    }
        //    //}

        //    //skip the private registry write if it is already running
        //    if (!IsRunning || Debugger.IsAttached)
        //    {
        //        if (_visualStudio == null)
        //        {

        //            string VS2017Regfile = null;
        //            string vsLocalAppdata = string.Format("{0}\\Microsoft\\VisualStudio\\", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
        //            foreach (var item in Directory.GetDirectories(vsLocalAppdata, "15.0*"))
        //            {
        //                VS2017Regfile = Directory.GetFiles(item, "privateregistry.bin").FirstOrDefault();
        //                if (File.Exists(VS2017Regfile))
        //                {
        //                    break;
        //                }
        //            }
        //            if (string.IsNullOrEmpty(VS2017Regfile))
        //            {
        //                VS2017Regfile = string.Format("{0}{1}\\privateregistry.bin", vsLocalAppdata, RuntimeInfo.Inst.VSVerTag);
        //            }

        //            if (Debugger.IsAttached)
        //            {
        //                VS2017Regfile = @"G:\privateregistry.bin";
        //            }

        //            if (File.Exists(VS2017Regfile))
        //            {
        //                int regKey = RegistryNativeMethods.RegLoadAppKey(VS2017Regfile);
        //                _visualStudio = RegistryKey.FromHandle(new Microsoft.Win32.SafeHandles.SafeRegistryHandle(new IntPtr(regKey), true));
        //            }
        //        }
        //        base.SetSonfig(config, regConfig);
        //    }
        //}
    }
}
