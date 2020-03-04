using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchEnvironment.VSCode.launch
{
    /// <summary>
    ///  "linux": {
    ///   "MIMode": "gdb",
    ///   "miDebuggerPath": "/usr/bin/gdb"
    /// },
    /// "osx": {
    ///    "MIMode": "lldb"
    /// },
    /// "windows": {
    ///  "MIMode": "gdb",
    ///  "miDebuggerPath": "C:\\MinGw\\bin\\gdb.exe"
    /// }
    /// </summary>
    public class debugger_json
    {
        public debugger_json()
        {
            miDebuggerPath = "gdb";
            MIMode = "gdb";
        }
        public string MIMode { get; set; }
        public string miDebuggerPath { get; set; }
    }
}
