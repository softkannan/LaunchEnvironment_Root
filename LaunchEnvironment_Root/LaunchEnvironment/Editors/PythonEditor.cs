using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchEnvironment.Config;

namespace LaunchEnvironment.Editors
{
    public class PythonEditor : EditorDefault
    {
        public PythonEditor() : base(RuntimeInfo.Python)
        {

        }

        public override void Launch(LaunchConfig config)
        {
            var foundEnv = config.Configs.FirstOrDefault((item) => item.Type == ConfigType.python);

            if (foundEnv != null)
            {
                string pythonPath = string.Format("{0}\\pythonw.exe", ResolveValue.Inst.ResolveFullPath(foundEnv.InstallPath));
                string pythonidle = string.Format("\"{0}\\Lib\\idlelib\\idle.py\"", ResolveValue.Inst.ResolveFullPath(foundEnv.InstallPath));

                DynamicArgument = pythonidle;
                DynamicEditor = pythonPath;

                base.Launch(config);
            }
            else
            {
                ErrorLog.Inst.ShowError("Unable to find the python environment");
            }
        }
    }
}
