using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchEnvironment.VSCode.task
{
    public class task_json
    {
        public task_json()
        {
            label = "build";
            type = "shell";
            command = "make";
            args = new List<string>();
            group = "build";
            presentation = new presentation_json();
            problemMatcher = "$msCompile";
        }

        public string label { get; set; }
        public string type { get; set; }
        public string command { get; set; }
        public List<string> args { get; set; }
        public string group { get; set; }
        public presentation_json presentation { get; set; }
        public string problemMatcher { get; set; }

    }
}
