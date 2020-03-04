using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchEnvironment.VSCode.settings
{
    public class arduino_settings_json
    {
        /// <summary>
        /// Path to Arduino, you can use a custom version of Arduino by modifying this setting to include the full path. 
        /// Example: C:\\Program Files\\Arduino for Windows, /Applications for Mac, /home/$user/Downloads/arduino-1.8.1 for Linux. (Requires a restart after change). 
        /// The default value is automatically detected from your Arduino IDE installation path.
        /// </summary>
        public string path { get; set; }
        /// <summary>
        /// Path to an executable (or script) relative to arduino.path. You can use a custom launch script to run Arduino by modifying this setting. 
        /// (Requires a restart after change) Example: run-arduino.bat for Windows, Contents/MacOS/run-arduino.sh for Mac, bin/run-arduino.sh for Linux."
        /// </summary>
        public string commandPath { get; set; }
        /// <summary>
        /// dditional URLs for 3rd party packages. You can have multiple URLs in one string with comma(,) as separator, or have a string array. The default value is empty.
        /// </summary>
        public string additionalUrls { get; set; }
        /// <summary>
        /// CLI output log level. Could be info or verbose. The default value is "info".
        /// </summary>
        public string logLevel { get; set; }
        /// <summary>
        /// Enable/disable USB detection from the VSCode Arduino extension. The default value is true.
        /// </summary>
        public bool? enableUSBDetection { get; set; }

    }
}
