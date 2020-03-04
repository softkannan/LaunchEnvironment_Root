using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchEnvironment.VSCode.settings
{
    public class python_settings_autoComplete_json
    {
        /// <summary>
        /// Specifies whether VS Code automatically adds parentheses (()) when autocompleting a function name.
        /// </summary>
        public bool? addBrackets { get; set; }
        /// <summary>
        /// Specifies modules to pre-load to improve autocomplete performance.
        /// </summary>
        public List<string> preloadModules { get; set; }
        /// <summary>
        /// Specifies locations of additional packages for which to load autocomplete data.
        /// </summary>
        public List<string> extraPaths { get; set; }
    }
}
