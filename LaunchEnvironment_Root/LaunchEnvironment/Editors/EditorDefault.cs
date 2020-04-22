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
using System.Text.RegularExpressions;

namespace LaunchEnvironment.Editors
{
    public class EditorDefault
    {
        private string _toolName;
        private Tool _tool;

        protected static Regex _envVarPattern = new Regex("%(?<EnvVar>[a-zA-Z_]+[a-zA-Z0-9_]*)%", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant);

        protected string ToolName { get => _toolName; }
        protected string LaunchArguments { get; set; }
        protected string DynamicArgument { get; set; }

        public EditorDefault()
        {
            this._tool = null;
            LaunchArguments = "";
            DynamicArgument = "";
        }

        public void Initialize(Tool tool)
        {
            this._tool = tool;
        }


        protected Tool Tool
        {
            get
            {
                if (this._tool == null)
                {
                    _tool = RuntimeInfo.Inst.GetTool(ToolName);
                }
                return _tool;
            }
        }

        protected string ResolveSourceFilePath(string file, string configID, string toolId)
        {
            string retFile = file.MatchReplace("%ConfigId%", configID, out bool foundMatch);
            retFile = retFile.MatchReplace("%ToolId%", toolId, out bool foundMatch2);
            return ResolveValue.Inst.ResolveFullPath(retFile);
        }

        protected virtual void UpdateFiles(LaunchConfig config)
        {
            foreach (var tempConfig in config.Configs)
            {
                foreach (var cpFile in tempConfig.CopyFiles)
                {
                    bool replaceFile = true;
                    if (cpFile.DestPath.IndexOf("%OpenFolder%") != -1)
                    {
                        replaceFile = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("OpenFolder"));
                    }
                    if (replaceFile)
                    {
                        string absSrcFile = ResolveSourceFilePath(cpFile.SrcPath, tempConfig.Id, Tool.Name);
                        if (File.Exists(absSrcFile))
                        {
                            string absDestFile = ResolveValue.Inst.ResolveFullPath(cpFile.DestPath);
                            try
                            {
                                if (File.Exists(absDestFile))
                                {
                                    File.Delete(absDestFile);
                                }
                            }
                            catch { }

                            File.Copy(absSrcFile, absDestFile, true);
                        }
                    }
                }
            }
        }

        protected string ExpandEnvironmentValue(string inputStr, ProcessStartInfo procStartInfo)
        {
            string inputStrExpanded = Environment.ExpandEnvironmentVariables(inputStr);
            string retVal = inputStrExpanded;
            foreach (Match match in _envVarPattern.Matches(inputStrExpanded))
            {
                string envVarKey = match.Groups["EnvVar"].Value;
                var foundKey = procStartInfo.Environment.IsExists((enVar) => string.Compare(envVarKey, enVar.Key, true) == 0);
                if (foundKey)
                {
                    var foundKeyValue = procStartInfo.Environment.First((enVar) => string.Compare(envVarKey, enVar.Key, true) == 0);
                    retVal = retVal.Replace(match.Value, Environment.ExpandEnvironmentVariables(foundKeyValue.Value));
                }
            }
            return retVal;
        }

        protected void MergeArguments(StringBuilder toolarguments,List<string> arguments, ProcessStartInfo procStartInfo)
        {
            if (arguments != null && arguments.Count > 0)
            {
                foreach (var item in arguments)
                {
                    if (Tool.UseShellExecute)
                    {
                        toolarguments.Append(Environment.ExpandEnvironmentVariables(item));
                        toolarguments.Append(" ");
                    }
                    else
                    {
                        bool foundEnvVar = true;
                        foreach (Match match in _envVarPattern.Matches(item))
                        {
                            var foundKey = procStartInfo.Environment.IsExists((enVar) => string.Compare(match.Groups["EnvVar"].Value, enVar.Key, true) == 0);
                            if (!foundKey)
                            {
                                foundEnvVar = false;
                                break;
                            }
                        }
                        if (foundEnvVar)
                        {
                            toolarguments.Append(ExpandEnvironmentValue(item, procStartInfo));
                            toolarguments.Append(" ");
                        }
                    }
                }
            }
        }

        protected string ResolveArguments(LaunchConfig config, ProcessStartInfo procStartInfo)
        {
            //merge first config which has argument with tool arguments
            var toolArgument = new StringBuilder();

            //Merge Tool arguments
            MergeArguments(toolArgument, Tool.Arguments, procStartInfo);
           
            //Merge selected config arguments
            var foundConfig = config.Configs.FirstOrDefault((item) => item.Arguments != null && item.Arguments.Count > 0);
            if (foundConfig != null && foundConfig.Arguments != null && foundConfig.Arguments.Count > 0)
            {
                MergeArguments(toolArgument, foundConfig.Arguments, procStartInfo);
            }

            var retVal = toolArgument.ToString().Trim();
            //merge dynamic Argument
            retVal += (string.IsNullOrWhiteSpace(retVal) ? "" : " ") + Environment.ExpandEnvironmentVariables(DynamicArgument);

            return retVal;
        }

        protected virtual bool LaunchCustom(LaunchConfig config)
        {
            return true;
        }

        public void Launch(LaunchConfig config)
        {
            if (Tool == null)
            {
                return;
            }

            var firstConfig = config.Configs.FirstOrDefault();
            var fullToolPath = ResolveValue.Inst.ResolveFullPath(Tool.ToolPath);
            Environment.SetEnvironmentVariable("ConfigPath", firstConfig == null ? fullToolPath : ResolveValue.Inst.ResolveFullPath(firstConfig.ConfigPath));
            Environment.SetEnvironmentVariable("ToolPath", fullToolPath);
            string workingDir = "";

            if (Directory.Exists(RuntimeInfo.Inst.OpenFolder))
            {
                workingDir = RuntimeInfo.Inst.OpenFolder;
            }
            else if (firstConfig != null && !string.IsNullOrWhiteSpace(firstConfig.DefaultWorkspace))
            {
                string configWorkspace = ResolveValue.Inst.ResolveFullPath(firstConfig.DefaultWorkspace);
                if (Directory.Exists(configWorkspace))
                {
                    workingDir = configWorkspace;
                }
            }

            if (!Directory.Exists(workingDir))
            {
                string toolWorkspace = ResolveValue.Inst.ResolveFullPath(RuntimeInfo.Inst.DefaultWorkspace);
                if (Directory.Exists(toolWorkspace))
                {
                    workingDir = toolWorkspace;
                }
                else
                {
                    workingDir = ResolveValue.Inst.ResolveFullPath("%userprofile%\\Documents");
                }
            }

            Environment.SetEnvironmentVariable("OpenFolder", workingDir);

            config.WorkingDir = workingDir;

            switch (config.Verb)
            {
                case "updatefiles":
                    UpdateFiles(config);
                    break;
                default:
                    {
                        if (LaunchCustom(config))
                        {
                            LaunchInternal(config);
                        }
                    }
                    break;
            }
        }

        protected virtual Process LaunchInternal(LaunchConfig config)
        {
            if (Tool.Script.Count > 0)
            {
                foreach (var item in Tool.Script)
                {
                    var procStartInfo = new ProcessStartInfo();

                    config.EditorPath = Tool.Type == ToolType.StoreApp ? RuntimeInfo.Inst.GetToolPath(Tool.Name) : ResolveValue.Inst.ResolveFullPath(RuntimeInfo.Inst.GetToolPath(Tool.Name));

                    if (Tool.UseShellExecute == false)
                    {
                        UpdateEnvironmentVariables(procStartInfo, config);
                    }

                    if (!string.IsNullOrWhiteSpace(config.Verb))
                    {
                        procStartInfo.Verb = config.Verb;
                    }

                    procStartInfo.WorkingDirectory = config.WorkingDir;
                    procStartInfo.Arguments = ResolveArguments(config, procStartInfo); 
                    procStartInfo.FileName = config.EditorPath;
                    procStartInfo.UseShellExecute = Tool.UseShellExecute;

                    var newProc = LaunchProcess(procStartInfo);
                    newProc.WaitForExit();
                }

                return null;
            }
            else
            {
                var procStartInfo = new ProcessStartInfo();

                config.EditorPath = Tool.Type == ToolType.StoreApp ? RuntimeInfo.Inst.GetToolPath(Tool.Name) : ResolveValue.Inst.ResolveFullPath(RuntimeInfo.Inst.GetToolPath(Tool.Name));

                if (Tool.UseShellExecute == false)
                {
                    UpdateEnvironmentVariables(procStartInfo, config);
                }

                if (!string.IsNullOrWhiteSpace(config.Verb))
                {
                    procStartInfo.Verb = config.Verb;
                }

                procStartInfo.WorkingDirectory = config.WorkingDir;
                procStartInfo.Arguments = ResolveArguments(config, procStartInfo);
                procStartInfo.FileName = config.EditorPath;

                return LaunchProcess(procStartInfo);
            }
        }
        protected virtual Process LaunchProcess(ProcessStartInfo procInfo)
        {
            if (Tool.UseShellExecute == false)
            {
                procInfo.UseShellExecute = Tool.UseShellExecute;

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

            //var tempItem = new EnviromentVariable();

            //tempItem.Name = "CurrentWorkingRootFolderName";
            //tempItem.Action = EnvironmentAction.Overwrite;
            //tempItem.Type = EnvironmentValueType.String;
            //string tempFileName = string.Format(@"{0}\makefile", RuntimeInfo.Inst.ToolLaunchDir);
            //tempItem.Value = Path.GetFileName(Path.GetDirectoryName(tempFileName));

            //UpdateEnvironmentVariables(procInfo, tempItem);

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
            //var matches = from m in _envVarPattern.Matches(newValue).OfType<Match>() group m by m.Value into newM select newM;
            // duplicate matches may be ineffciant
            foreach (Match match in _envVarPattern.Matches(newValue))
            {
                string envVarKey = match.Groups["EnvVar"].Value;
                var foundKey = currentValues.IsExists((enVar) => string.Compare(envVarKey, enVar.Key, true) == 0);
                if (foundKey)
                {
                    var foundKeyValue = currentValues.First((enVar) => string.Compare(envVarKey, enVar.Key, true) == 0);
                    retVal = retVal.Replace(match.Value, Environment.ExpandEnvironmentVariables(foundKeyValue.Value));
                }
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
                        {
                            procInfo.Environment[item.Name] = ResolveValue.Inst.ResolveEnvironmentValue(item.Type, PreProcessValue(procInfo.Environment, item.Value));
                        }
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

                    var regConfig = RegConfig.RegConfigBase.GetConfig(itemEnv.Id);

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
