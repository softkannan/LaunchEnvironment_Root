using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchEnvironment.VS.task
{
    /// <summary>
    /// 
    /// https://docs.microsoft.com/en-us/cpp/ide/non-msbuild-projects
    /// 
    /// ${env.<VARIABLE>}	specifies any environment variable (for example, ${env.PATH}, ${env.COMSPEC} and so on) that is set for the developer command prompt. 
    /// For more information, see Developer Command Prompt for Visual Studio.
    /// ${workspaceRoot}
    /// the full path to the workspace folder(for example, "C:\sources\hello")
    /// ${file}	the full path of the file or folder selected to run this task against (for example, "C:\sources\hello\src\hello.cpp") ${ relativeFile}
    /// the relative path to the file or folder (for example, "src\hello.cpp") ${ fileBasename}
    /// the name of the file without path or extension (for example, "hello") ${ fileDirname}
    /// the full path to the file, excluding the filename (for example, "C:\sources\hello\src") ${ fileExtname}
    /// the extension of the selected file (for example, ".cpp")
    /// </summary>
    public class vstask
    {
        public string version { get; set; }
        public List<task> tasks { get; set; }
    }
}
