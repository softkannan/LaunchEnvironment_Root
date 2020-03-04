using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchEnvironment.VSCode.settings
{
    /// <summary>
    /// You are provided with a list of Default Settings. Copy any setting that you want to change to the appropriate settings.
    /// json file. The tabs under the Search box let you switch quickly between the user and workspace settings files.
    /// .VSCode/settings.json
    /// </summary>
    public class settings_json
    {
        /// <summary>
        /// The following Visual Studio Code settings are available for the Arduino extension. 
        /// These can be set in global user preferences Ctrl + , or workspace settings (.vscode/settings.json). The later overrides the former.
        /// </summary>
        public arduino_settings_json arduino { get; set; }
        /// <summary>
        /// AutoComplete settings
        /// </summary>
        public python_settings_json python { get; set; }
    }
}
