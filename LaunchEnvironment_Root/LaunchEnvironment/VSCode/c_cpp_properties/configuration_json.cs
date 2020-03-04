using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchEnvironment.VSCode.c_cpp_properties
{
    public class configuration_json
    {
        public configuration_json()
        {
            cStandard = "c11";
            cppStandard = "c++17";
            name = "win32";
            defines = new List<string> { "_DEBUG", "UNICODE", "_UNICODE" };
            intelliSenseMode = "msvc-x64";
            browse = new browse_json();
            includePath = new List<string> { "${workspaceFolder}","${include}" };

        }
        public string name { get; set; }
        public List<string> includePath { get; set; }
        public List<string> defines { get; set; }
        public string intelliSenseMode { get; set; }
        public browse_json browse { get; set; }
        public string cStandard { get; set; }
        public string cppStandard { get; set; }
    }
}
