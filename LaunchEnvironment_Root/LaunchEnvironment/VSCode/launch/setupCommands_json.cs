using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchEnvironment.VSCode.launch
{
    public class setupCommands_json
    {
        public setupCommands_json()
        {
            description = "Enable pretty-printing for gdb";
            text = "-enable-pretty-printing";
            ignoreFailures = true;
        }
        public string description { get; set; }
        public string text { get; set; }
        public bool? ignoreFailures { get; set; }
    }
}
