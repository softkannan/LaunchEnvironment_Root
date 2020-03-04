using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchEnvironment.VSCode.launch
{
    public class gcc_config_json : launch_config_json
    {
        public gcc_config_json()
        {
            setupCommands = new List<setupCommands_json> { new setupCommands_json() };
            miDebuggerPath = "gdb";
            MIMode = "gdb";
            externalConsole = true;
            env = new List<string>();
            cwd = "${workspaceFolder}";
            stopAtEntry = false;
            args = new List<string>();
            program = "${workspaceFolder}/";
            request = "launch";
            type = "cppdbg";
            name = "(gdb) Launch";
        }
    }
}
