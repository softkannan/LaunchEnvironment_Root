using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchEnvironment.VS.task
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/cpp/ide/non-msbuild-projects
    /// </summary>
    public class task
    {
        public string taskName { get; set; }
        /// <summary>
        /// You can create tasks for any file or folder by specifying its name in the appliesTo field, for example "appliesTo" : "hello.cpp". The following file masks can be used as values:
        /// "*"	task is available to all files and folders in the workspace
        /// "*/"	task is available to all folders in the workspace
        /// "*.cpp"	task is available to all files with the extension.cpp in the workspace
        /// "/*.cpp"	task is available to all files with the extension.cpp in the root of the workspace
        /// "src/*/"	task is available to all subfolders of the "src" folder
        /// "makefile"	task is available to all makefile files in the workspace
        /// "/makefile"	task is available only to the makefile in the root of the workspace
        /// </summary>
        public string appliesTo { get; set; }
        public string type { get; set; }
        public string command { get; set; }
        public List<string> args { get; set; }
        /// <summary>
        /// Use the output property to specify the executable that will launch when you press F5. For example:  "output": "${workspaceRoot}\\bin\\hellomake.exe" 
        /// </summary>
        public string output { get; set; }
    }
}
