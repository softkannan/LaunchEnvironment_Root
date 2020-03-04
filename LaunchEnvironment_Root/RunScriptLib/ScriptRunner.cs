using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RunScriptLib
{
    public class ScriptRunner
    {
        private string _ScriptFile="";
        private string _ExeFile="";
        private string _FileArg = "";
        private bool _useShellExecute = false;
        private bool _isRunFromConsole = false;
        private RunType _Type = RunType.None;
        private string _verb = "";

        public ScriptRunner(List<string> commandlineArgs,bool isConsoleApp)
        {
            _isRunFromConsole = isConsoleApp;
            ParseCommandline(commandlineArgs);
            _ExeFile = ResolveScriptExeFilePath(_ScriptFile,_ExeFile);
        }

        private bool IsRunFromConsole
        {
            get
            {
                return _isRunFromConsole;
                //if (_isRunFromConsole == null)
                //{
                //    //try
                //    //{
                //    //    _isRunFromConsole = true;
                //    //    _isRunFromConsole = Console.WindowHeight > 0;
                //    //}
                //    //catch { _isRunFromConsole = false; }
                //    _isRunFromConsole = false;
                //    try
                //    {
                //        var foundParent = Process.GetCurrentProcess().Parent();
                //        if (foundParent != null && (foundParent.ProcessName == "cmd" || foundParent.ProcessName == "powershell"))
                //        {
                //            if (!ProcessEx.NativeMethods.AttachConsole(-1))
                //            {
                //                Debug.Print("Creates New Console");
                //                ProcessEx.NativeMethods.AllocConsole();
                //            }

                //            _isRunFromConsole = true;
                //        }
                //    }
                //    catch
                //    {
                //        _isRunFromConsole = false;
                //    }
                //}
                //return _isRunFromConsole.Value;
            }
        }

        public ScriptRunner(string scriptFileName)
        {
            _ScriptFile = ResolveValue.Inst.ResolveFullPath(scriptFileName);
            _ExeFile = ResolveScriptExeFilePath(_ScriptFile, _ExeFile);
        }

        private void ParseCommandline(IEnumerable<string> commandlineArgs)
        {
            var enumurator = commandlineArgs.GetEnumerator();
            enumurator.Reset();

            while (enumurator.MoveNext())
            {
                string lowerCaseToken = enumurator.Current.ToLower();
                switch(lowerCaseToken)
                {
                    case CommandLineArgs.RunScript:
                        _Type = RunType.RunScript;
                        break;
                    case CommandLineArgs.RunVerb:
                        _Type = RunType.RunVerb;
                        break;
                    case CommandLineArgs.RunVerbName:
                        {
                            if (enumurator.MoveNext())
                            {
                                _verb = enumurator.Current;
                            }
                        }
                        break;
                    case CommandLineArgs.UseShellExecute:
                        {
                            _useShellExecute = true;
                        }
                        break;
                    case CommandLineArgs.ScriptFile:
                        {
                            if(enumurator.MoveNext())
                            {
                                _ScriptFile = ResolveValue.Inst.ResolveFullPath(enumurator.Current);
                            }
                        }
                        break;
                    case CommandLineArgs.ExeFileArg:
                        {
                            string scriptArg = "";
                            while(enumurator.MoveNext())
                            {
                                scriptArg += " " + Environment.ExpandEnvironmentVariables(enumurator.Current);
                            }
                            _FileArg = scriptArg.Trim();
                        }
                        break;
                    case CommandLineArgs.ExeFilePath:
                        {
                            if(enumurator.MoveNext())
                            {
                                _ExeFile = enumurator.Current;
                            }
                        }
                        break;
                }
            }

            if(string.IsNullOrWhiteSpace(_ScriptFile) && _Type != RunType.RunVerb)
            {
                enumurator.Reset();
                enumurator.MoveNext();
                _ScriptFile = enumurator.Current;
                string scriptArg = "";
                while (enumurator.MoveNext())
                {
                    scriptArg += " " + Environment.ExpandEnvironmentVariables(enumurator.Current);
                }
                _FileArg = scriptArg.Trim();
            }
        }

        private string ResolveScriptExeFilePath(string scriptFileName,string scriptExeFileName)
        {
            string retVal = "";

            //if user provides the script file name then use it
            if (!string.IsNullOrWhiteSpace(scriptExeFileName))
            {
                string tempVal = ResolveValue.Inst.ResolveFullPath(scriptExeFileName);
                //if able to locate the script engine then skip rest of the steps
                if(File.Exists(tempVal))
                {
                    return tempVal;
                }
            }

            string fileExt = Path.GetExtension(scriptFileName).ToLower();
            string autoScriptExeFileName = "";
            string[] listOfEnvVarsToLook = null;
            switch (fileExt)
            {
                case ".pys":
                case ".pyw":
                case ".pyz":
                case ".py":
                    {
                        listOfEnvVarsToLook = new string[] { "PATH","PYTHONPATH" };
                        if(IsRunFromConsole)
                        {
                            autoScriptExeFileName = "python.exe";
                        }
                        else
                        {
                            autoScriptExeFileName = "pythonw.exe";
                        }
                    }
                    break;
                case ".rb":
                    {
                        listOfEnvVarsToLook = new string[] { "PATH"};
                        autoScriptExeFileName = "ruby.exe";
                    }
                    break;
                case ".sh":
                    {
                        listOfEnvVarsToLook = new string[] { "PATH" };
                        autoScriptExeFileName = "bash.exe";
                    }
                    break;
                case ".pls":
                case ".pl":
                    {
                        listOfEnvVarsToLook = new string[] { "PATH" };
                        autoScriptExeFileName = "perl.exe";
                    }
                    break;
                case ".tcls":
                case ".tcl":
                    {
                        listOfEnvVarsToLook = new string[] { "PATH" };
                        autoScriptExeFileName = "tclsh.exe";
                    }
                    break;
                case ".js":
                case ".jse":
                case ".vbe":
                case ".vbs":
                case ".wsf":
                case ".wsh":
                    {
                        listOfEnvVarsToLook = new string[] { "PATH" };
                        if (IsRunFromConsole)
                        {
                            autoScriptExeFileName = "cscript.exe";
                        }
                        else
                        {
                            autoScriptExeFileName = "Wscript.exe";
                        }
                    }
                    break;
                case ".ps1":
                case ".psc1":
                    {
                        listOfEnvVarsToLook = new string[] { "PATH" };
                        autoScriptExeFileName = "powershell.exe";
                    }
                    break;
                case ".lua":
                    {
                        listOfEnvVarsToLook = new string[] { "PATH" };
                        autoScriptExeFileName = "lua.exe";
                    }
                    break;
            }

            //try resolve user provided name
            if (!string.IsNullOrEmpty(scriptExeFileName))
            {
                string scriptExeFile = ResolveValue.Inst.ResolveFullFilePath(scriptExeFileName, listOfEnvVarsToLook);
                if (File.Exists(scriptExeFile))
                {
                    return scriptExeFile;
                }
            }

            //try resolve auto generated name
            {
                string scriptExeFile = ResolveValue.Inst.ResolveFullFilePath(autoScriptExeFileName, listOfEnvVarsToLook);
                if (File.Exists(scriptExeFile))
                {
                    return scriptExeFile;
                }
            }

            return retVal;
        }

        public void Run()
        {
            Debug.Print("_ExeFile : {0}, _ScriptFile : {1}, _ScriptArguments : {2}, _useShellExecute : {3}, WorkingDir : {4}, HasConsole : {5}",
                _ExeFile, _ScriptFile, _FileArg, _useShellExecute, Environment.CurrentDirectory, IsRunFromConsole);

            if (File.Exists(_ExeFile))
            {
                var launcher = new LaunchProcess();
                var procInfo = new ProcessStartInfo();
                Process process = null;

                switch (_Type)
                {
                    case RunType.RunVerb:
                        {
                            procInfo.Verb = _verb;
                            string arguments = "";
                            if (!string.IsNullOrWhiteSpace(_FileArg))
                            {
                                arguments += " " + _FileArg;
                            }
                            procInfo.WorkingDirectory = Environment.CurrentDirectory;
                            procInfo.Arguments = arguments;
                            procInfo.FileName = _ExeFile;
                            procInfo.UseShellExecute = true;
                        }
                        break;
                    case RunType.RunScript:
                    case RunType.None:
                    default:
                        {
                            string arguments = _ScriptFile;
                            if (!string.IsNullOrWhiteSpace(_FileArg))
                            {
                                arguments += " " + _FileArg;
                            }
                            procInfo.WorkingDirectory = Environment.CurrentDirectory;
                            procInfo.Arguments = arguments;
                            procInfo.FileName = _ExeFile;
                            procInfo.UseShellExecute = _useShellExecute;
                        }
                        break;
                }

                process = launcher.Launch(procInfo, IsRunFromConsole);
                if(process != null && IsRunFromConsole)
                {
                    process.WaitForExit();
                }
            }
            else
            {
                string usageInfo = Utility.GetUsageInfo();
                ErrorLog.Inst.ShowInfo(usageInfo);
            }
        }

        public static void ScriptRunMain(List<string> cmdArgs,bool isConsoleApp)
        {
          
            if (cmdArgs.Count > 1)
            {
                //remove the first element
                cmdArgs.RemoveAt(0);
                if (cmdArgs.Contains(CommandLineArgs.RunDiag))
                {
                    Application.Run(new mainForm());
                }
                //else if (cmdArgs.Contains(CommandLineArgs.RunScript))
                //{
                //    var excludedList = cmdArgs.Except(new string[] { CommandLineArgs.RunScript }).ToList();
                //    var scriptRunner = new ScriptRunner(excludedList, isConsoleApp);
                //    scriptRunner.Run();
                //}
                else
                {
                    var scriptRunner = new ScriptRunner(cmdArgs.ToList(), isConsoleApp);
                    scriptRunner.Run();
                }
            }
            else
            {
                string exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string exeNameWithoutExt = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
                string pythonfileName = string.Format(@"{0}\{1}.py", exePath, exeNameWithoutExt);
                string powerShellfileName = string.Format(@"{0}\{1}.ps1", exePath, exeNameWithoutExt);
                string perlfileName = string.Format(@"{0}\{1}.pl", exePath, exeNameWithoutExt);
                string vbsfileName = string.Format(@"{0}\{1}.vbs", exePath, exeNameWithoutExt);
                string wbsfileName = string.Format(@"{0}\{1}.wbs", exePath, exeNameWithoutExt);
                List<string> scriptFileNames = new List<string> { pythonfileName, powerShellfileName, perlfileName, vbsfileName, wbsfileName };
                bool foundScript = false;
                foreach (var item in scriptFileNames)
                {
                    if (File.Exists(item))
                    {
                        var scriptRunner = new ScriptRunner(item);
                        scriptRunner.Run();
                        foundScript = true;
                        break;
                    }
                }

                if (!foundScript)
                {
                    string usageInfo = Utility.GetUsageInfo();
                    ErrorLog.Inst.ShowInfo(usageInfo);
                }
            }
        }
    }
}
