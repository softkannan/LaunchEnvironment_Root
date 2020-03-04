using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchEnvironment.VSCode.c_cpp_properties
{
    public class browse_json
    {
        public browse_json()
        {
            limitSymbolsToIncludedHeaders = true;
            databaseFilename = "";
            path = new List<string> { "${workspaceFolder}","${include}" };
        }
        public List<string> path { get; set; }
        public bool limitSymbolsToIncludedHeaders { get; set; }
        public string databaseFilename { get; set; }
    }
}
