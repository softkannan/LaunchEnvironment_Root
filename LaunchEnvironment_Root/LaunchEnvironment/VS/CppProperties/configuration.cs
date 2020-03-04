using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchEnvironment.VS.CppProperties
{
    public class configuration
    {
        /// <summary>
        /// Environment variables
        ///     CppProperties.json supports system environment variable expansion for include paths and other property values.The syntax is ${ env.FOODIR}
        ///     to expand an environment variable %FOODIR%. The following system-defined variables are also supported:
        /// Variable Name   Description
        /// vsdev   The default Visual Studio environment
        ///     msvc_x86    Compile for x86 using x86 tools
        ///     msvc_arm    Compile for ARM using x86 tools
        ///     msvc_arm64  Compile for ARM64 using x86 tools
        ///     msvc_x86_x64    Compile for AMD64 using x86 tools
        ///  msvc_x64_x64    Compile for AMD64 using 64-bit tools
        ///     msvc_arm_x64 Compile for ARM using 64-bit tools
        ///     msvc_arm64_x64 Compile for ARM64 using 64-bit tools
        /// When the Linux workload is installed, the following environments are available for remotely targeting Linux and WSL:
        /// Variable Name   Description
        ///     linux_x86   Target x86 Linux remotely
        ///     linux_x64 Target x64 Linux remotely
        ///     linux_arm   Target ARM Linux remotely
        /// You can define custom environment variables in CppProperties.json either globally or per-configuration.
        /// The following example shows how default and custom environment variables can be declared and used.
        /// The global environments property declares a variable named INCLUDE that can be used by any configuration:
        /// You can also define an environments property inside a configuration, so that it applies only to that configuration, 
        /// and overrides any global variables of the same name. In the following example, the x64 configuration defines a local INCLUDE variable that overrides the global value:
        /// All custom and default environment variables are also available in tasks.vs.json and launch.vs.json.
        /// </summary>
        public List<string> inheritEnvironments { get; set; }
        /// <summary>
        /// the configuration name that appears in the C++ configuration dropdown
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// the list of folders that should be specified in the include path (maps to /I for most compilers)
        /// </summary>
        public List<string> includePath { get; set; }
        /// <summary>
        /// the list of macros that should be defined (maps to /D for most compilers)
        /// </summary>
        public List<string> defines { get; set; }

        /// <summary>
        /// one or more additional switches that can influence IntelliSense behavior
        /// </summary>
        public string compilerSwitches { get; set; }

        /// <summary>
        /// the IntelliSense engine to be used. You can specify the architecture specific variants for MSVC, gcc or Clang:
        /// msvc-x86 (default)
        /// msvc-x64
        /// msvc-arm
        /// windows-clang-x86
        /// windows-clang-x64
        /// windows-clang-arm
        /// Linux-x64
        /// Linux-x86
        /// Linux-arm
        /// gccarm
        /// </summary>
        public string intelliSenseMode { get; set; }
        /// <summary>
        /// header to be automatically included in every compilation unit (maps to /FI for MSVC or -include for clang)
        /// </summary>
        public string[] forcedInclude { get; set; }
        /// <summary>
        /// the list of macros to be undefined (maps to /U for MSVC)
        /// </summary>
        public List<string> undefines { get; set; }
    }
}
