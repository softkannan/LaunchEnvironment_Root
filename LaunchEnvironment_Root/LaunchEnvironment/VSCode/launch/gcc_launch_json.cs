using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchEnvironment.VSCode.launch
{
    public class gcc_launch_json
    {
        public gcc_launch_json()
        {
            configurations = new List<launch_config_json> { new launch_config_json() };
            version = "0.2.0";
        }
        public string version { get; set; }

        public List<launch_config_json> configurations { get; set; }
    }
}
