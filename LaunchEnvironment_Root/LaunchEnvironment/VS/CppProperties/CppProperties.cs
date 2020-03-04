using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchEnvironment.VS.CppProperties
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/cpp/ide/non-msbuild-projects
    /// </summary>
    public class CppProperties
    {
        public List<configuration> configurations { get; set; }
    }
}
