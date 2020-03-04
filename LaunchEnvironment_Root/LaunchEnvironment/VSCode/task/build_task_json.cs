using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchEnvironment.VSCode.task
{
    public class build_task_json
    {
        public build_task_json()
        {
            version = "2.0.0";
            tasks = new List<task_json> { new task_json() , new task_json { args = new List<string> { "debug" }, label = "builddebug" } };
        }
        public string version { get; set; }

        public List<task_json> tasks { get; set; }
    }
}
