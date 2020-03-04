using LaunchEnvironment.VSCode;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchEnvironment.Config;

namespace LaunchEnvironment.Editors
{
    public class VSCodeEditor : EditorDefault
    {
        public VSCodeEditor() : base(RuntimeInfo.VSCode)
        {

        }
        private void UpdateJsonFiles(LaunchConfig config)
        {
            var activeEnv = config.Configs.FirstOrDefault((item) => item.Type == ConfigType.gcc_linux || item.Type == ConfigType.gcc || item.Type == ConfigType.python);
            var executablExt = (activeEnv != null && activeEnv.Type == ConfigType.gcc_linux) ? "" : ".exe";

            if (RuntimeInfo.Inst.IsOpenFolder && activeEnv != null)
            {
                //.VSCode dir check
                string vsCodeDir = string.Format(@"{0}\.VSCode\", RuntimeInfo.Inst.ToolLaunchDir);
                if (!Directory.Exists(vsCodeDir))
                {
                    Directory.CreateDirectory(vsCodeDir);
                }

                //c_cpp_properties.json
                if (activeEnv.Type == ConfigType.gcc || activeEnv.Type == ConfigType.gcc_linux)
                {
                    string fileName = string.Format(@"{0}\.VSCode\c_cpp_properties.json", RuntimeInfo.Inst.ToolLaunchDir);
                    if (!File.Exists(fileName))
                    {
                        using (TextWriter writer = new StreamWriter(fileName))
                        {
                            var cppProperties = new VSCode.c_cpp_properties.c_cpp_properties_json();
                            var serializer = new JsonSerializer
                            {
                                NullValueHandling = NullValueHandling.Ignore,
                                Formatting = Formatting.Indented
                            };

                            serializer.Serialize(writer, cppProperties);
                        }
                    }
                }

                //tasks.json
                if (activeEnv.Type == ConfigType.gcc || activeEnv.Type == ConfigType.gcc_linux)
                {
                    string fileName = string.Format(@"{0}\.VSCode\tasks.json", RuntimeInfo.Inst.ToolLaunchDir);

                    if (!File.Exists(fileName))
                    {
                        using (TextWriter writer = new StreamWriter(fileName))
                        {
                            var buildTask = new VSCode.task.build_task_json();
                            var serializer = new JsonSerializer
                            {
                                NullValueHandling = NullValueHandling.Ignore,
                                Formatting = Formatting.Indented
                            };

                            serializer.Serialize(writer, buildTask);
                        }
                    }
                }

                //launch.json
                if (activeEnv.Type == ConfigType.gcc || activeEnv.Type == ConfigType.gcc_linux)
                {
                    string fileName = string.Format(@"{0}\.VSCode\launch.json", RuntimeInfo.Inst.ToolLaunchDir);

                    if (!File.Exists(fileName))
                    {
                        using (TextWriter writer = new StreamWriter(fileName))
                        {
                            var tempLaunch = new VSCode.launch.gcc_launch_json();

                            string makefilename = string.Format(@"{0}\makefile", RuntimeInfo.Inst.ToolLaunchDir);
                            string exefileName = Path.GetFileName(Path.GetDirectoryName(makefilename));

                            tempLaunch.configurations[0].program = "${workspaceFolder}/" + string.Format("{0}{1}", exefileName, executablExt);
                            tempLaunch.configurations[0].miDebuggerPath = string.Format("gdb{0}", executablExt);

                            var serializer = new JsonSerializer
                            {
                                NullValueHandling = NullValueHandling.Ignore,
                                Formatting = Formatting.Indented
                            };

                            serializer.Serialize(writer, tempLaunch);
                        }
                    }
                }
                else if(activeEnv.Type == ConfigType.python)
                {
                    string fileName = string.Format(@"{0}\.VSCode\launch.json", RuntimeInfo.Inst.ToolLaunchDir);

                    if (!File.Exists(fileName))
                    {
                        using (TextWriter writer = new StreamWriter(fileName))
                        {
                            var tempLaunch = new VSCode.launch.python_launch_json();
                            
                            var serializer = new JsonSerializer
                            {
                                NullValueHandling = NullValueHandling.Ignore,
                                Formatting = Formatting.Indented
                            };

                            serializer.Serialize(writer, tempLaunch);
                        }
                    }
                }

                if (activeEnv.Type == ConfigType.python)
                {
                    string fileName = string.Format(@"{0}\.VSCode\settings.json", RuntimeInfo.Inst.ToolLaunchDir);

                    if (!File.Exists(fileName))
                    {
                        using (TextWriter writer = new StreamWriter(fileName))
                        {
                            var tempSettings = new VSCode.settings.settings_json() { python = new VSCode.settings.python_settings_json {
                                autoComplete = new VSCode.settings.python_settings_autoComplete_json()
                                {
                                    addBrackets = false,
                                    preloadModules = new List<string> { "numpy", "os", "sys", "io","time","math","requests","subprocess","logging", "argparse","base64","json","errno","BeautifulSoup"}
                                }
                            } };

                            var serializer = new JsonSerializer
                            {
                                NullValueHandling = NullValueHandling.Ignore,
                                Formatting = Formatting.Indented
                            };

                            serializer.Converters.Add(new VSCode.settings.FlatFileJsonConverter());

                            serializer.Serialize(writer, tempSettings);
                        }
                    }
                }
                else if(activeEnv.Type == ConfigType.arduino)
                {
                    string fileName = string.Format(@"{0}\.VSCode\settings.json", RuntimeInfo.Inst.ToolLaunchDir);

                    if (!File.Exists(fileName))
                    {
                        using (TextWriter writer = new StreamWriter(fileName))
                        {
                            var tempSettings = new VSCode.settings.settings_json() { arduino=new VSCode.settings.arduino_settings_json() };

                            var serializer = new JsonSerializer
                            {
                                NullValueHandling = NullValueHandling.Ignore,
                                Formatting = Formatting.Indented
                            };

                            serializer.Converters.Add(new VSCode.settings.FlatFileJsonConverter());

                            serializer.Serialize(writer, tempSettings);
                        }
                    }
                }

                if (activeEnv.Type == ConfigType.arduino)
                {
                    string fileName = string.Format(@"{0}\.VSCode\arduino.json", RuntimeInfo.Inst.ToolLaunchDir);

                    if (!File.Exists(fileName))
                    {
                        using (TextWriter writer = new StreamWriter(fileName))
                        {
                            var tempObj = new VSCode.arduino.arduino_json();

                            var serializer = new JsonSerializer
                            {
                                NullValueHandling = NullValueHandling.Ignore,
                                Formatting = Formatting.Indented
                            };

                            serializer.Converters.Add(new VSCode.settings.FlatFileJsonConverter());

                            serializer.Serialize(writer, tempObj);
                        }
                    }
                }


            }
        }

        public override void Launch(LaunchConfig config)
        {

            if (RuntimeInfo.Inst.IsOpenFolder)
            {
                DynamicArgument = "\"" + RuntimeInfo.Inst.ToolLaunchDir + "\"";
            }

            //UpdateJsonFiles(config);

            base.Launch(config);
        }
    }
}
