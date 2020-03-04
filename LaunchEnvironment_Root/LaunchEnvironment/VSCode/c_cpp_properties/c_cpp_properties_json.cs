using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchEnvironment.VSCode.c_cpp_properties
{
    public class c_cpp_properties_json
    {
        public c_cpp_properties_json()
        {
            version = 3;
            configurations = new List<configuration_json>() { new configuration_json() };
        }
        public List<configuration_json> configurations { get; set; }
        public int version { get; set; }
    }
}
