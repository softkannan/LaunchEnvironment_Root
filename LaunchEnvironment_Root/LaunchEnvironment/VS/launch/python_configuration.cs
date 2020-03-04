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
    public class python_configuration
    {
        public string interpreter { get; set; }
        public string interpreterArguments { get; set; }
        public string name { get; set; }
        public bool nativeDebug { get; set; }
        public string project { get; set; }
        public string scriptArguments { get; set; }
        public string type { get; set; }
        public string webBrowserUrl { get; set; }
    }
}
