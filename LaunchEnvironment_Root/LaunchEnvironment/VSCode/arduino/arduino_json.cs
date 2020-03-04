using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchEnvironment.VSCode.arduino
{
    /// <summary>
    /// The following settings are per sketch settings of the Arduino extension. You can find them in .vscode/arduino.json under the workspace.
    /// </summary>
    public class arduino_json
    {
        /// <summary>
        /// The main sketch file name of Arduino.
        /// </summary>
        public string sketch { get; set; }
        /// <summary>
        ///  Name of the serial port connected to the device. Can be set by the Arduino: Select Serial Port command.
        /// </summary>
        public string port { get; set; }
        /// <summary>
        /// Current selected Arduino board alias. Can be set by the Arduino: Change Board Type command. Also, you can find the board list there.
        /// </summary>
        public string board { get; set; }
        /// <summary>
        /// Arduino build output path. If not set, Arduino will create a new temporary output folder each time,
        /// which means it cannot reuse the intermediate result of the previous build, leading to long verify/upload time.
        /// So it is recommended to set the field. Arduino requires that the output path should not be the workspace itself or subfolder of the workspace, 
        /// otherwise, it may not work correctly. By default, this option is not set.
        /// </summary>
        public string output { get; set; }
        /// <summary>
        /// The short name of the debugger that will be used when the board itself does not have any debugger and there are more than one debugger available. 
        /// You can find the list of debuggers here. By default, this option is not set.
        /// </summary>
        public string debugger { get; set; }
    }
}
