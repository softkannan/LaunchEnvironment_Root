using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchEnvironment.Config;
using System.IO;
using Newtonsoft.Json;

namespace LaunchEnvironment.Editors
{
    public class VisualStudioEditor : EditorDefault
    {
        public VisualStudioEditor() : base(RuntimeInfo.VSStudio)
        {

        }

        private void UpdateJsonFiles(LaunchConfig config)
        {
            var activeEnv = config.Configs.FirstOrDefault((item) => item.Type == ConfigType.gcc_linux || item.Type == ConfigType.gcc );

            if (RuntimeInfo.Inst.IsOpenFolder && activeEnv != null)
            {
                //.vs dir check
                string vsCodeDir = string.Format(@"{0}\.vs\", RuntimeInfo.Inst.ToolLaunchDir);
                if (!Directory.Exists(vsCodeDir))
                {
                    Directory.CreateDirectory(vsCodeDir);
                }

                if (activeEnv.Type == ConfigType.gcc || activeEnv.Type == ConfigType.gcc_linux)
                {
                    //c_cpp_properties.json
                    string fileName = string.Format(@"{0}\CppProperties.json", RuntimeInfo.Inst.ToolLaunchDir);
                    if (!File.Exists(fileName))
                    {
                        string goldFile = string.Format(@"{0}\OpenFolder_Resource\vs\CppProperties.json", RuntimeInfo.Inst.LaunchEnvExeDir);
                        using (TextWriter writer = new StreamWriter(fileName))
                        {
                            using (TextReader reader = new StreamReader(goldFile))
                            {
                                writer.Write(reader.ReadToEnd());
                            }
                        }
                    }

                    //tasks.json
                    fileName = string.Format(@"{0}\.vs\tasks.vs.json", RuntimeInfo.Inst.ToolLaunchDir);

                    if (!File.Exists(fileName))
                    {
                        string goldFile = string.Format(@"{0}\OpenFolder_Resource\vs\tasks.vs.json", RuntimeInfo.Inst.LaunchEnvExeDir);
                        using (TextWriter writer = new StreamWriter(fileName))
                        {
                            using (TextReader reader = new StreamReader(goldFile))
                            {
                                writer.Write(reader.ReadToEnd());
                            }
                        }
                    }

                    //launch.json
                    fileName = string.Format(@"{0}\.vs\launch.vs.json", RuntimeInfo.Inst.ToolLaunchDir);

                    if (!File.Exists(fileName))
                    {
                        string goldFile = string.Format(@"{0}\OpenFolder_Resource\vs\launch.vs.json", RuntimeInfo.Inst.LaunchEnvExeDir);
                        using (TextWriter writer = new StreamWriter(fileName))
                        {
                            using (TextReader reader = new StreamReader(goldFile))
                            {
                                writer.Write(reader.ReadToEnd());
                            }
                        }
                    }
                }
            }
        }

        public override void Launch(LaunchConfig config)
        {
            if(RuntimeInfo.Inst.IsOpenFolder && RuntimeInfo.Inst.VSVersion >= 15)
            {
                //UpdateJsonFiles(config);
                string dynamicArgs = "\"" + RuntimeInfo.Inst.ToolLaunchDir + "\"";
                var activeEnv = config.Configs.FirstOrDefault((item) => item.Type == ConfigType.gcc_linux || item.Type == ConfigType.gcc);
                if(activeEnv != null)
                {
                    dynamicArgs += " /useenv";
                }

                DynamicArgument = dynamicArgs;
            }

            base.Launch(config);
        }
    }
}
