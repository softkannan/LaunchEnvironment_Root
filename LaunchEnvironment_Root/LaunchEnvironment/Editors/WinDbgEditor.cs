using LaunchEnvironment.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchEnvironment.Editors
{
    public class WinDbgEditor : EditorDefault
    {

        public WinDbgEditor(string toolName): base(toolName)
        {
        }

        public override void Launch(LaunchConfig config)
        {
            string toolPath;

            var foundConfig = config.Configs.FirstOrDefault((item) => item.Type == ConfigType.windbg);
            if (foundConfig == null)
            {
                var foundTool = RuntimeInfo.Inst.GetTool(ToolName);
                toolPath = RuntimeInfo.Inst.GetToolPath(ToolName);

                if (foundTool.IsStoreApp == false  && !File.Exists(toolPath))
                {
                    ErrorLog.Inst.ShowError("Unable to find Windbg at Path {0}", toolPath);
                    return;
                }
            }
            else
            {
                //means use local path
                if (!string.IsNullOrWhiteSpace(foundConfig.InstallPath))
                {
                    if (Tool.Name.Contains("-32Bit"))
                    {
                        toolPath = string.Format("{0}\\x86\\windbg.exe", ResolveValue.Inst.ResolveFullPath(foundConfig.InstallPath));
                    }
                    else
                    {
                        toolPath = string.Format("{0}\\x64\\windbg.exe", ResolveValue.Inst.ResolveFullPath(foundConfig.InstallPath));
                    }
                }
                else
                {
                    toolPath = "WinDbgX.exe";
                    if (Tool.Name.Contains("-32Bit"))
                    {
                        EditorArguments = "-debugArch x86";
                    }
                    else
                    {
                        EditorArguments = "-debugArch amd64";
                    }
                }

                if(foundConfig.Envs == null)
                {
                    foundConfig.Envs = new List<EnviromentVariable>();
                }

                EnviromentVariable pathVar = new EnviromentVariable() { Name = "PATH", Value = Path.GetDirectoryName(toolPath) };
                foundConfig.Envs.Add(pathVar);
                //var foundVar = foundConfig.Envs.FirstOrDefault((item) => string.Compare(item.Name, "Scripts", true) == 0);

                //if(foundVar != null)
                //{
                //    string scriptFolder = ResolveValue.Inst.ResolveFullPath(foundVar.Value);
                //    DynamicArgument = string.Format("-c\".cmdtree {0}\\cmdtree_default.txt\"", scriptFolder);
                //}
            }

            DynamicEditor = toolPath;

            base.Launch(config);
        }
    }
}
