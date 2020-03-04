using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchEnvironment.VSCode.launch
{
    /// <summary>
    /*  {
            "name": "Python",
            "type": "python",
            "request": "launch",
            "stopOnEntry": true,
            "program": "${file}",
            "debugOptions": [
                "WaitOnAbnormalExit",
                "WaitOnNormalExit",
                "RedirectOutput"
            ]
        },
        {
            "name": "Python Console App",
            "type": "python",
            "request": "launch",
            "stopOnEntry": true,
            "program": "${file}",
            "externalConsole": true,
            "debugOptions": [
                "WaitOnAbnormalExit",
                "WaitOnNormalExit"
            ]
        },
        {
            # import ptvsd
            # ptvsd.enable_attach("my_secret", address = ('0.0.0.0', 3000))
            "name": "Attach (Remote Debug)",
            "type": "python",
            "request": "attach",
            "localRoot": "${workspaceRoot}",
            "remoteRoot": "C:/temp/myscripts",
            "port": 3000,
            "secret": "my_secret",
            "host":"ipaddress or 'localhost'"
        }
    */
    /// </summary>
    public class python_launch_json
    {
        public python_launch_json()
        {
            //debugOptions = Valid values: "RedirectOutput", "DebugStdLib", "BreakOnSystemExitZero", "Django", "Sudo", "IgnoreDjangoTemplateWarnings", "Pyramid"
            configurations = new List<python_config_json> {
                new python_config_json() { debugOptions = new List<string>{ "RedirectOutput" } },
                new python_config_json() { name="Python Console App", debugOptions = null, console= "externalTerminal", externalConsole = true },
                new python_config_json() { name="Attach (Remote Debug)", debugOptions= new List<string>{ "RedirectOutput", "Sudo" },request="attach",localRoot="${workspaceRoot}",
                    remoteRoot ="${workspaceRoot} / /usr/home/myscript" ,port=3000, secret="my_secret",host="ipaddress or 'localhost'" }
            };
            version = "0.2.0";
        }
        public string version { get; set; }

        public List<python_config_json> configurations { get; set; }
    }
}
