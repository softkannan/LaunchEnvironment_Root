using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchEnvironment.VSCode.launch
{
    public class python_config_json : launch_config_json
    {
        public python_config_json()
        {
            name = "Python";
            type = "python";
            request = "launch";
            program = "${file}";
        }
        /// <summary>
        ///  Is where the source python files are located on the server
        /// </summary>
        public string localRoot { get; set; }
        /// <summary>
        ///  Is the path to the script file on the remote machine.
        /// </summary>
        public string remoteRoot { get; set; }
        /// <summary>
        /// Is the port to connect to the remote machine on /  port when attaching to a running process
        /// </summary>
        public int? port { get; set; }
        /// <summary>
        /// Is a pass phrase used to authenticate for remote debugging
        /// </summary>
        public string secret { get; set; }
        /// <summary>
        ///  Is the ipaddress to the remove server. You can also use the value localhost
        /// </summary>
        public string host { get; set; }
        /// <summary>
        /// debug launcher options
        /// </summary>
        public List<string> debugOptions { get; set; }

    }
}
