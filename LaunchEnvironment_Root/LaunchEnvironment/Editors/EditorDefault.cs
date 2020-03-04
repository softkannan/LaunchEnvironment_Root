using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LaunchEnvironment.Config;
using ProcessEx;
using LaunchEnvironment.Utility;
using System.Collections.Specialized;
using System.Windows.Forms;

namespace LaunchEnvironment.Editors
{
    public class EditorDefault
    {
        private string toolName;

        protected string ToolName { get => toolName; }
        protected string EditorArguments { get; set; }
        protected string DynamicArgument { get; set; }
        protected string DynamicEditor { get; set; }


        public EditorDefault(string toolName)
        {
            this.toolName = toolName;
            EditorArguments = "";
            DynamicArgument = "";
            DynamicEditor = "";
        }


        protected Tool Tool
        {
            get
            {
                return RuntimeInfo.Inst.GetTool(ToolName);
            }
        }

        protected void GenerateMakeFile(LaunchConfig config)
        {
            var activeEnv = config.Configs.FirstOrDefault((item) => item.Type == ConfigType.gcc_linux || item.Type == ConfigType.gcc);
            if (RuntimeInfo.Inst.IsOpenFolder && activeEnv != null)
            {
                string fileName = string.Format(@"{0}\makefile", RuntimeInfo.Inst.ToolLaunchDir);
                if (!File.Exists(fileName))
                {
                    string srcFolder = string.Format(@"{0}\src", RuntimeInfo.Inst.ToolLaunchDir);
                    string goldMakefile = string.Format(@"{0}\OpenFolder_Resource\makefile", RuntimeInfo.Inst.LaunchEnvExeDir);
                    if (Directory.Exists(srcFolder) && File.Exists(goldMakefile))
                    {
                        using (TextWriter writer = new StreamWriter(fileName))
                        {
                            using (TextReader reader = new StreamReader(goldMakefile))
                            {
                                writer.Write(reader.ReadToEnd());
                            }
                        }
                    }
                }
            }
        }
        public void UpdateRegistry(List<Config.Config> envs)
        {
            foreach (var itemEnv in envs)
            {
                if (itemEnv.RegConfigs == null)
                {
                    continue;
                }

                var regConfig = RegConfig.RegConfigBase.GetConfig(itemEnv.Type);

                foreach (var item in itemEnv.RegConfigs)
                {
                    if (regConfig != null)
                    {
                        regConfig.SetSonfig(item);
                    }
                }
            }
        }

        protected void MergeArguments(LaunchConfig config)
        {
            //merge first config which has argument with tool arguments
            string toolArgument = Environment.ExpandEnvironmentVariables(Tool.Arguments);
            var foundConfig = config.Configs.FirstOrDefault((item) => !string.IsNullOrWhiteSpace(item.Arguments));
            if (foundConfig != null)
            {
                toolArgument = (string.IsNullOrWhiteSpace(toolArgument) ? "" : " ") + Environment.ExpandEnvironmentVariables(foundConfig.Arguments);
            }

            //merge any existing editor argument passed down, prefer to use dynamicArgument
            string editorArguments = Environment.ExpandEnvironmentVariables(EditorArguments);
            EditorArguments += (string.IsNullOrWhiteSpace(editorArguments) ? "" : " ") + toolArgument;

            //merge dynamic Argument
            editorArguments = EditorArguments;
            EditorArguments += (string.IsNullOrWhiteSpace(editorArguments) ? "" : " ") + Environment.ExpandEnvironmentVariables(DynamicArgument);
        }

        public virtual void Launch(LaunchConfig config)
        {
            MergeArguments(config);

            config.Arguments = EditorArguments;
            config.EditorPath = string.IsNullOrWhiteSpace(DynamicEditor) ? RuntimeInfo.Inst.GetToolPath(ToolName) : DynamicEditor;
            config.WorkingDir = RuntimeInfo.Inst.ToolLaunchDir;

            GenerateMakeFile(config);

            LaunchInternal(config);
        }

        protected virtual Process LaunchInternal(LaunchConfig config)
        {
            if (Tool.Script.Count > 0)
            {
                foreach (var item in Tool.Script)
                {
                    var procInfo = new ProcessStartInfo();

                    if (Tool.UseShellExecute == false)
                    {
                        UpdateEnvironmentVariables(procInfo, config);
                    }

                    if (!string.IsNullOrWhiteSpace(config.Verb))
                    {
                        procInfo.Verb = config.Verb;
                    }

                    procInfo.WorkingDirectory = config.WorkingDir;
                    procInfo.Arguments = config.Arguments;
                    procInfo.FileName = config.EditorPath;
                    procInfo.UseShellExecute = Tool.UseShellExecute;

                    var newProc = LaunchProcess(procInfo);
                    newProc.WaitForExit();
                }

                return null;
            }
            else
            {
                var procInfo = new ProcessStartInfo();

                if (Tool.UseShellExecute == false)
                {
                    UpdateEnvironmentVariables(procInfo, config);
                }

                if (!string.IsNullOrWhiteSpace(config.Verb))
                {
                    procInfo.Verb = config.Verb;
                }

                procInfo.WorkingDirectory = config.WorkingDir;
                procInfo.Arguments = config.Arguments;
                procInfo.FileName = config.EditorPath;
                procInfo.UseShellExecute = Tool.UseShellExecute;

                return LaunchProcess(procInfo);
            }
        }
        protected virtual Process LaunchProcess(ProcessStartInfo procInfo)
        {
            if (Tool.UseShellExecute == false)
            {
                var injectProcess = new ProcessEx.CustomProcess();
                injectProcess.BootstrapProcess = RuntimeInfo.Inst.RunScriptPath;

                var retProc = injectProcess.LaunchProcessSuspended(procInfo, false);
                if (Tool.ByPassRegistry)
                {
                    string bitnessSuffix = "x64";
                    if (!retProc.Is64BitProcess())
                    {
                        bitnessSuffix = "win32";
                    }

                    string registryDll = string.Format("{0}\\RegistryHandler_{1}.dll", Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), bitnessSuffix);
                    if (File.Exists(registryDll))
                    {
                        DllInject.InjectDll(registryDll, retProc);
                    }
                }

                injectProcess.ResumeProcess();
                return retProc;
            }
            else
            {
                return Process.Start(procInfo);
            }
        }

        private void UpdateEnvironmentVariables(ProcessStartInfo procInfo, LaunchConfig config)
        {
            if (config.Configs == null)
            {
                return;
            }

            var tempItem = new EnviromentVariable();

            tempItem.Name = "CurrentWorkingRootFolderName";
            tempItem.Action = EnvironmentAction.Overwrite;
            tempItem.Type = EnvironmentValueType.String;
            string tempFileName = string.Format(@"{0}\makefile", RuntimeInfo.Inst.ToolLaunchDir);
            tempItem.Value = Path.GetFileName(Path.GetDirectoryName(tempFileName));

            UpdateEnvironmentVariables(procInfo, tempItem);

            if (Tool.Envs != null)
            {
                UpdateEnvironmentVariables(procInfo, Tool.Envs);
            }

            foreach (var itemEnv in config.Configs)
            {
                if (itemEnv.Envs == null)
                {
                    continue;
                }
                UpdateEnvironmentVariables(procInfo, itemEnv.Envs);
            }
        }

        private static string PreProcessValue(IDictionary<string, string> currentValues, string newValue)
        {
            string retVal = newValue;
            foreach (KeyValuePair<string, string> item in currentValues)
            {
                string patten = string.Format("%{0}%", item.Key);
                string tempVal = retVal;
                retVal = tempVal.Replace(patten, item.Value);
            }
            return retVal;
        }

        private static void UpdateEnvironmentVariables(ProcessStartInfo procInfo, EnviromentVariable item)
        {
            if (item == null)
            {
                return;
            }

            var foundKey = procInfo.Environment.IsExists((enVar) => string.Compare(item.Name, enVar.Key, true) == 0);
            if (foundKey)
            {
                switch (item.Action)
                {
                    case EnvironmentAction.Overwrite:

                        procInfo.Environment[item.Name] = ResolveValue.Inst.ResolveEnvironmentValue(item.Type, PreProcessValue(procInfo.Environment, item.Value));
                        break;
                    case EnvironmentAction.Prefix:
                        {
                            string currentValue = procInfo.EnvironmentVariables[item.Name];
                            string newValue = ResolveValue.Inst.ResolveEnvironmentValue(item.Type, PreProcessValue(procInfo.Environment, item.Value)) + ";" + currentValue;
                            procInfo.Environment[item.Name] = newValue;
                        }
                        break;
                    case EnvironmentAction.Append:
                        {
                            string currentValue = procInfo.EnvironmentVariables[item.Name];
                            string newValue = currentValue + ";" + ResolveValue.Inst.ResolveEnvironmentValue(item.Type, PreProcessValue(procInfo.Environment, item.Value));
                            procInfo.Environment[item.Name] = newValue;
                        }
                        break;
                }
            }
            else
            {
                string newValue = PreProcessValue(procInfo.Environment, ResolveValue.Inst.ResolveEnvironmentValue(item.Type, item.Value));
                procInfo.Environment[item.Name] = newValue;
            }
        }

        private static void UpdateEnvironmentVariables(ProcessStartInfo procInfo, List<EnviromentVariable> listOfEnvs)
        {
            foreach (var item in listOfEnvs)
            {
                UpdateEnvironmentVariables(procInfo, item);
            }
        }
        public void CleanConfig(LaunchConfig currentConfig)
        {
            if (currentConfig.Configs != null)
            {
                foreach (var itemEnv in currentConfig.Configs)
                {
                    if (itemEnv.RegConfigs == null)
                    {
                        continue;
                    }

                    var regConfig = RegConfig.RegConfigBase.GetConfig(itemEnv.Type);

                    foreach (var item in itemEnv.RegConfigs)
                    {
                        if (regConfig != null)
                        {
                            regConfig.CleanConfig(currentConfig, item);
                        }
                    }
                }
            }
        }
    }
}
