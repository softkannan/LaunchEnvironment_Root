using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchEnvironment.Config;
using System.IO;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace LaunchEnvironment.Editors
{
    public class VSStudioEditor_Disabled : EditorDefault
    {
        public VSStudioEditor_Disabled() : base()
        {

        }

        private int VSVersion { get; set; }

        public void AutoDetectToolPath()
        {
            var VSSetupRegKey = new List<Tuple<string, string>> {
                new Tuple<string, string>(@"SOFTWARE\Microsoft\VisualStudio\SxS\VS7","15.0"),
                new Tuple<string, string>(@"SOFTWARE\Microsoft\VisualStudio\SxS\VS7","12.0"),
                new Tuple<string, string>(@"SOFTWARE\Microsoft\VisualStudio\SxS\VS7","10.0")
            };
            string vsStudioInsallDir = "";

            if (Environment.Is64BitProcess)
            {
                using (var localMachine32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
                {
                    foreach (var item in VSSetupRegKey)
                    {
                        using (var vsKey = localMachine32.OpenSubKey(item.Item1))
                        {
                            if (vsKey != null)
                            {
                                vsStudioInsallDir = vsKey.GetValue(item.Item2, "") as string;

                                if (Directory.Exists(vsStudioInsallDir))
                                {
                                    VSVersion = (int)Double.Parse(item.Item2);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (var item in VSSetupRegKey)
                {
                    using (var vsKey = Registry.LocalMachine.OpenSubKey(item.Item1))
                    {
                        if (vsKey != null)
                        {
                            vsStudioInsallDir = vsKey.GetValue(item.Item2, "") as string;

                            if (Directory.Exists(vsStudioInsallDir))
                            {
                                VSVersion = (int)Double.Parse(item.Item2);
                                break;
                            }
                        }
                    }
                }
            }

            string devEnvPath = string.Format(@"{0}Common7\IDE\devenv.exe", vsStudioInsallDir);

            if (File.Exists(devEnvPath))
            {
                Environment.SetEnvironmentVariable(string.Format("{0}Dir", RuntimeInfo.VSStudio), vsStudioInsallDir.TrimEnd('\\'), EnvironmentVariableTarget.Process);
            }

            List<string> vsCodeDirs = new List<string> { @"%ProgramFiles(x86)%\Microsoft VS Code", @"%ProgramFiles%\Microsoft VS Code" };
            string vsCodeDir = "";
            foreach (var item in vsCodeDirs)
            {
                var vsCodeExe = string.Format(@"{0}\Code.exe", ResolveValue.Inst.ResolveFullPath(item));
                if (File.Exists(vsCodeExe))
                {
                    vsCodeDir = item;
                    break;
                }
            }

            if (!string.IsNullOrWhiteSpace(vsCodeDir))
            {
                Environment.SetEnvironmentVariable(string.Format("{0}Dir", RuntimeInfo.VSCode), ResolveValue.Inst.ResolveFullPath(vsCodeDir), EnvironmentVariableTarget.Process);
            }

        }
    }
}
