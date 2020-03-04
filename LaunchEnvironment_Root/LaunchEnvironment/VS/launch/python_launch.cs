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
    public class python_launch
    {
        public string version { get; set; }
        public List<defaults> defaults { get; set; }
        public List<python_configuration> configurations { get; set; }
    }
}
