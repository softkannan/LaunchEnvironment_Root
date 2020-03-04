using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchEnvironment.VSCode.launch
{
    public class launch_config_json
    {
        public launch_config_json()
        {
           
        }
        /// <summary>
        ///  friendly name which appears in the Debug launch configuration dropdown.
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// the type of debugger to use for this launch configuration. Every installed debug extension introduces a type, for example, node for the built-in node debugger, or php and go for the PHP and Go extensions.
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// the request type of this launch configuration. Currently supported are launch and attach.
        /// </summary>
        public string request { get; set; }
        /// <summary>
        /// executable or file to run when launching the debugger
        /// </summary>
        public string program { get; set; }
        /// <summary>
        /// arguments passed to the program to debug
        /// </summary>
        public List<string> args { get; set; }
        /// <summary>
        /// break immediately when the program launches
        /// </summary>
        public bool? stopAtEntry { get; set; }
        /// <summary>
        /// current working directory for finding dependencies and other files
        /// </summary>
        public string cwd { get; set; }
        /// <summary>
        /// environment variables (the value null can be used to "undefine" a variable)
        /// </summary>
        public List<string> env { get; set; }
        /// <summary>
        /// Environment variables to add to the environment for the program. Example: [ { "name": "squid", "value": "clam" } ].
        /// </summary>
        public List<string> environment { get { return this.env; } set { this.env = value; } }
        /// <summary>
        /// what kind of console to use, for example, internalConsole, integratedTerminal, externalTerminal.
        /// </summary>
        public string console { get; set; }
        /// <summary>
        /// If set to true, launches an external console for the application. If false, no console is launched and VS Code's debugging console is used. Note this option is ignored in some cases for technical reasons.
        /// </summary>
        public bool? externalConsole { get; set; }
        /// <summary>
        /// Indicates the debugger that VS Code will connect to. Must be set to gdb or lldb. This is pre-configured on a per-operating system basis and can be changed as needed.
        /// </summary>
        public string MIMode { get; set; }
        /// <summary>
        /// The path to the debugger (such as gdb). When only the executable is specified, it will search the operating system's PATH variable for a debugger (GDB on Linux and Windows, LLDB on OS X).
        /// </summary>
        public string miDebuggerPath { get; set; }
        public debugger_json linux { get; set; }
        public debugger_json windows { get; set; }
        public debugger_json osx { get; set; }
        public List<setupCommands_json> setupCommands { get; set; }
        /// <summary>
        /// Network address of the debugger server (e.g. gdbserver) to connect to for remote debugging (example: localhost:1234).
        /// </summary>
        public string miDebuggerServerAddress { get; set; }
        /// <summary>
        /// Full path to debug server to launch.
        /// </summary>
        public string debugServerPath { get; set; }
        /// <summary>
        /// Arguments for the the debugger server.
        /// </summary>
        public string debugServerArgs { get; set; }
        /// <summary>
        /// Time in milliseconds, for the debugger to wait for the debugServer to start up. Default is 10000.
        /// </summary>
        public int? serverLaunchTimeout { get; set; }
        /// <summary>
        /// Server-started pattern to look for in the debug server output.
        /// </summary>
        public string serverStarted { get; set; }
        /// <summary>
        /// Tells the Visual Studio Windows Debugger what paths to search for symbol (.pdb) files. Separate multiple paths with a semicolon. Example "C:\\Symbols;C:\\SymbolDir2"
        /// </summary>
        public string symbolSearchPath { get; set; }
        /// <summary>
        /// Tells GDB or LLDB what paths to search for .so files. Separate multiple paths with a semicolon. Example: "/Users/user/dir1;/Users/user/dir2".
        /// </summary>
        public string additionalSOLibSearchPath { get; set; }
        /// <summary>
        /// If you want to debug a Windows dump file, set this to the path to the dump file to start debugging in the launch configuration.
        /// </summary>
        public string dumpPath { get; set; }
        /// <summary>
        /// Full path to a core dump file to debug for the specified program. Set this to the path to the core dump file to start debugging in the launch configuration. Note: core dump debugging is not supported with MinGw.
        /// </summary>
        public string coreDumpPath { get; set; }
        /// <summary>
        /// This allows mapping of the compile time paths for source to local source locations. It is an object of key/value pairs and will resolve the first string-matched path. (example: "sourceFileMap": { "/mnt/c": "c:\\" } will map any path returned by the debugger that begins with /mnt/c and convert it to c:\\. You can have multiple mappings in the object but they will be handled in the order provided.)
        /// </summary>
        public List<string> sourceFileMap { get; set; }

    }

    

    
}
