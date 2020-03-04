using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchEnvironment.VSCode.arduino.boardinfo
{
    public class tool_info
    {
        public string name { get; set; }
        public string version { get; set; }
        public List<system_info> systems { get; set; }
    }
}
