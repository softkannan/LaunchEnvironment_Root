using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchEnvironment.VS.launch
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/cpp/ide/non-msbuild-projects
    /// </summary>
    public class configuration
    {
        public string type { get; set; }
        public string project { get; set; }
        public string name { get; set; }
        public List<string> args { get; set; }
    }
}
