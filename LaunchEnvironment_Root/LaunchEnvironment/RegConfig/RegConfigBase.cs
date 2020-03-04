using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchEnvironment.Config;

namespace LaunchEnvironment.RegConfig
{
    public class RegConfigBase
    {
        public static RegConfigBase GetConfig(ConfigType type)
        {
            switch(type)
            {
                case ConfigType.vcc:
                    return new VCRegConfig();
                case ConfigType.python:
                    return new PythonRegConfig();
                default:
                    return new RegConfigBase();
            }
        }

        protected virtual RegistryKey LocalMachine
        {
            get { return Registry.LocalMachine; }
        }

        protected virtual RegistryKey CurrentUser
        {
            get { return Registry.CurrentUser; }
        }

        public virtual void SetSonfig(Config.RegKey regConfig)
        {
            if (regConfig.RequireAdmin)
            {
                if (!RuntimeInfo.Inst.IsElevated)
                {
                    return;
                }
            }

            if (!string.IsNullOrWhiteSpace(regConfig.Key))
            {
                if (regConfig.Key.IndexOf(@"HKEY_LOCAL_MACHINE\") == 0)
                {
                    string regKey = regConfig.Key.Replace(@"HKEY_LOCAL_MACHINE\","");

                    RegistryKey rootKey = CurrentUser;
                    if(RuntimeInfo.Inst.IsElevated)
                    {
                        rootKey = LocalMachine;
                    }

                    WriteKey(rootKey, regKey, regConfig);

                    if (Environment.Is64BitProcess && rootKey == Registry.LocalMachine)
                    {
                        int startIndex = regKey.IndexOf(@"SOFTWARE\");
                        if (startIndex == 0)
                        {
                            using (RegistryKey wow32Key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
                            {
                                WriteKey(wow32Key, regKey, regConfig);
                            }
                        }
                    }
                }
                else if (regConfig.Key.IndexOf(@"HKEY_CURRENT_USER\") == 0)
                {
                    string regKey = regConfig.Key.Replace(@"HKEY_CURRENT_USER\", "");
                    WriteKey(CurrentUser, regKey, regConfig);
                }
                else
                {
                    string regKey = regConfig.Key;
                    WriteKey(regKey, regConfig);
                    if (Environment.Is64BitProcess)
                    {
                        int startIndex = regKey.IndexOf(@"HKEY_LOCAL_MACHINE\SOFTWARE\");
                        if (startIndex == 0)
                        {
                            string regWowKey = regKey.Replace(@"HKEY_LOCAL_MACHINE\SOFTWARE\", @"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\");
                            WriteKey(regWowKey, regConfig);
                        }
                    }
                }
            }
        }

        private static void WriteKey(RegistryKey rootKey, string subkey, Config.RegKey regKey)
        {
            if(rootKey == null)
            {
                return;
            }

            try
            {
                using (RegistryKey localKey = rootKey.CreateSubKey(subkey, RegistryKeyPermissionCheck.ReadWriteSubTree))
                {
                    if (regKey.Action == EnvironmentAction.Overwrite)
                    {
                        localKey.SetValue("", string.IsNullOrWhiteSpace(regKey.Value) ? "" : regKey.Value);
                    }
                    else
                    {
                        string currentValue = localKey.GetValue("") as string;
                        if(string.IsNullOrEmpty(currentValue))
                        {
                            localKey.SetValue("", string.IsNullOrWhiteSpace(regKey.Value) ? "" : regKey.Value);
                        }
                        else
                        {
                            string newValue = string.IsNullOrWhiteSpace(regKey.Value) ? "" : regKey.Value;
                            if (regKey.Action == EnvironmentAction.Append)
                            {
                                localKey.SetValue("", currentValue + newValue);
                            }
                            else
                            {
                                localKey.SetValue("", newValue + currentValue);
                            }
                        }
                    }

                    if (regKey.RegValues != null)
                    {
                        foreach (var item in regKey.RegValues)
                        {
                            RegistryValueKind kind;
                            var newRegValue = ResolveValue.Inst.ResolveRegistryValue(item.Value, item.Type, out kind);
                            if (regKey.Action == EnvironmentAction.Overwrite || 
                                kind != RegistryValueKind.String || 
                                kind != RegistryValueKind.MultiString || 
                                kind == RegistryValueKind.ExpandString )
                            {
                                localKey.SetValue(item.Name, newRegValue, kind);
                            }
                            else
                            {
                                string currentValue = localKey.GetValue("") as string;
                                if (string.IsNullOrEmpty(currentValue))
                                {
                                    localKey.SetValue(item.Name, newRegValue, kind);
                                }
                                else
                                {
                                    if (regKey.Action == EnvironmentAction.Append)
                                    {
                                        localKey.SetValue(item.Name, currentValue + newRegValue,kind);
                                    }
                                    else
                                    {
                                        localKey.SetValue(item.Name, newRegValue + currentValue,kind);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception)
            {

            }
        }

        private static void WriteKey(string key, Config.RegKey regConfig)
        {
            Registry.SetValue(key, "", string.IsNullOrWhiteSpace(regConfig.Value) ? "" : regConfig.Value);

            if (regConfig.RegValues != null)
            {
                foreach (var item in regConfig.RegValues)
                {
                    RegistryValueKind kind;
                    var regValue = ResolveValue.Inst.ResolveRegistryValue(item.Value, item.Type, out kind);
                    Registry.SetValue(key, item.Name, regValue, kind);
                }
            }
        }

        public virtual void CleanConfig(LaunchConfig config, Config.RegKey regConfig)
        {
            //if (!string.IsNullOrWhiteSpace(regConfig.Key))
            //{
            //    string regKey = regConfig.Key;
            //    WriteKeyEmpty(regKey, regConfig);

            //    if (Environment.Is64BitProcess)
            //    {
            //        int startIndex = regKey.IndexOf(@"HKEY_LOCAL_MACHINE\SOFTWARE\");
            //        if (startIndex == 0)
            //        {
            //            string regWowKey = regKey.Replace(@"HKEY_LOCAL_MACHINE\SOFTWARE\", @"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\");
            //            WriteKeyEmpty(regWowKey, regConfig);
            //        }
            //    }
            //}
        }
    }
}
