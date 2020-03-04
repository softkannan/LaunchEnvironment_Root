using LaunchEnvironment.Config;
using LaunchEnvironment.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchEnvironment.Editors
{
    public class KnownCommandEditor : EditorDefault
    {
        private string _cmdName;
        public KnownCommandEditor(string cmdName) : base(RuntimeInfo.Command)
        {
            _cmdName = cmdName;
        }
        public override void Launch(LaunchConfig config)
        {
            switch(_cmdName.Trim())
            {
                case "RegisterExplorerContextMenu":
                    {
                        if (ShellIntegration.RegisterContextMenu())
                        {
                            ErrorLog.Inst.ShowInfo("Explorer Context Menu Registration Completed.");
                        }
                    }
                    break;
                case "WriteConfigRegistryValues":
                    {
                        var launchTool = Editors.EditorFactory.Inst.GetEditor(RuntimeInfo.Generic);

                        launchTool.UpdateRegistry(Configs_Root.Inst.Configs);

                        ErrorLog.Inst.ShowInfo("Environment Registry Integration completed");
                    }
                    break;
                case "UpdatePythonScriptFolder":
                    {
                        var pythonEnv = new PythonEnvironment();
                        pythonEnv.UpdateScripts(Configs_Root.Inst);

                        ErrorLog.Inst.ShowInfo("Updating Python Environment Scripts folder is completed");
                    }
                    break;
            }
        }
    }
}
