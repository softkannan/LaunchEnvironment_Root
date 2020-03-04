using LaunchEnvironment.Config;
using LaunchEnvironment.Utility;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace LaunchEnvironment.Config
{
    public class RuntimeInfo
    {

        public RuntimeInfo()
        {
            ToolLaunchDir = null;
            string exePath = Assembly.GetExecutingAssembly().Location;
            LaunchEnvExeDir = Path.GetDirectoryName(exePath);
            RunScriptPath = string.Format(@"{0}\runscript.exe", LaunchEnvExeDir);
            IsOpenFolder = false;
        }

        public const string VSCode = "VSCode";
        public const string VSStudio = "VSStudio";
        public const string MobXTerm = "MobXTerm";
        public const string WingIDE = "WingIDE";
        public const string Komodo = "Komodo";
        public const string WinDbg = "WinDbg";
        public const string Python = "Python";
        public const string Generic = "Generic";
        public const string CodeBlocks = "CodeBlocks";
        public const string CodeLite = "CodeLite";
        public const string Arduino = "Arduino";
        public const string OpenSSH = "OpenSSH";
        public const string Command = "Command";

        [XmlIgnore]
        public string RunScriptPath { get; private set; }
        
        public List<EnviromentVariable> Envs { get; set; }
        public bool RunasAdmin { get; set; }
        public bool ShowRunAsForAll { get; set; }
        public List<Tool> ToolPath { get; set; }
        public List<MenuBar> MenuBar { get; set; }
        public List<ToolBarItem> ToolBar { get; set; }

        [XmlArray("ContextMenu")]
        [XmlArrayItem(ElementName = "Menu")]
        public List<string> ContextMenu { get; set; }

        private string _launchEnvExeDir = null;

        [XmlIgnore]
        public string LaunchEnvExeDir
        {
            get { return _launchEnvExeDir; }
            set
            {
                _launchEnvExeDir = ResolveValue.Inst.ResolveFullPath(value);
                Environment.SetEnvironmentVariable("LaunchEnvExeDir", _launchEnvExeDir, EnvironmentVariableTarget.Process);
                Environment.CurrentDirectory = _launchEnvExeDir;
            }
        }

        [XmlIgnore]
        public string ToolLaunchDir { get; set; }

        [XmlIgnore]
        public int VSVersion { get; private set; }
        

        [XmlIgnore]
        public bool IsElevated
        {
            get
            {
                return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
            }
        }
        [XmlIgnore]
        public bool IsOpenFolder { get; set; }

        [XmlIgnore]
        public bool Is64Bit
        {
            get
            {
                return IntPtr.Size == 8;
            }
        }

        [XmlIgnore]
        public string CurrentTimeStamp
        {
            get
            {
                var currTime = DateTime.Now;
                return string.Format("{0:00}_{1:00}_{2:0000}_{3:00}_{4:00}_{5:00}_{6:00}", currTime.Day, currTime.Month, currTime.Year, currTime.Hour, currTime.Minute, currTime.Second, currTime.Millisecond);
            }
        }
        [XmlIgnore]
        public string CurrentConfigFile
        {
            get
            {
                string fileLocation = Assembly.GetExecutingAssembly().Location;
                string retVal = string.Format("{0}\\CurrentConfig_{1}.xml", Path.GetDirectoryName(fileLocation), RuntimeInfo.Inst.CurrentTimeStamp);
                return retVal;
            }
        }

        [XmlIgnore]
        private static string ToolConfigFile
        {
            get
            {
                string fileLocation = Assembly.GetExecutingAssembly().Location;
                string retVal = string.Format("{0}\\Tools.xml", Path.GetDirectoryName(fileLocation));
                return retVal;
            }
        }

        

        public bool IsToolAvailable(string toolName)
        {
            var foundTool = ToolPath.FirstOrDefault((item) => item.Name == toolName);
            return foundTool != null;
        }

        public Tool GetTool(string toolName)
        {
            Tool retVal = null;
            var foundTool = ToolPath.FirstOrDefault((item) => item.Name == toolName);
            if (foundTool != null)
            {
                retVal = foundTool;
            }
            return retVal;
        }

        public string GetToolPath(string toolName)
        {
            string retVal = "";
            var foundTool = ToolPath.FirstOrDefault((item) => item.Name == toolName);
            if(foundTool != null)
            {
                retVal = foundTool.Path;
            }
            return retVal;
        }

        public void AutoDetectToolPath()
        {
            var VSSetupRegKey = new List<Tuple<string, string>> {
                new Tuple<string, string>(@"SOFTWARE\Microsoft\VisualStudio\SxS\VS7","15.0"),
                new Tuple<string, string>(@"SOFTWARE\Microsoft\VisualStudio\SxS\VS7","12.0"),
                new Tuple<string, string>(@"SOFTWARE\Microsoft\VisualStudio\SxS\VS7","10.0")
            };
            string vsStudioInsallDir = "";

            if(Environment.Is64BitProcess)
            {
                using (var localMachine32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,RegistryView.Registry32))
                {
                    foreach (var item in VSSetupRegKey)
                    {
                        using (var vsKey = localMachine32.OpenSubKey(item.Item1))
                        {
                            if (vsKey != null)
                            {
                                vsStudioInsallDir = vsKey.GetValue(item.Item2, "") as string;

                                if(Directory.Exists(vsStudioInsallDir))
                                {
                                    VSVersion =  (int)Double.Parse(item.Item2);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (var item in VSSetupRegKey)
                {
                    using (var vsKey = Registry.LocalMachine.OpenSubKey(item.Item1))
                    {
                        if (vsKey != null)
                        {
                            vsStudioInsallDir = vsKey.GetValue(item.Item2, "") as string;

                            if (Directory.Exists(vsStudioInsallDir))
                            {
                                VSVersion = (int)Double.Parse(item.Item2);
                                break;
                            }
                        }
                    }
                }
            }

            string devEnvPath = string.Format(@"{0}Common7\IDE\devenv.exe", vsStudioInsallDir);

            if(File.Exists(devEnvPath))
            {
                Environment.SetEnvironmentVariable(string.Format("{0}Dir", VSStudio), vsStudioInsallDir.TrimEnd('\\'), EnvironmentVariableTarget.Process);
            }

            List<string> vsCodeDirs = new List<string> { @"%ProgramFiles(x86)%\Microsoft VS Code", @"%ProgramFiles%\Microsoft VS Code" };
            string vsCodeDir = "";
            foreach(var item in vsCodeDirs)
            {
                var vsCodeExe = string.Format(@"{0}\Code.exe", ResolveValue.Inst.ResolveFullPath(item));
                if(File.Exists(vsCodeExe))
                {
                    vsCodeDir = item;
                    break;
                }
            }

            if(!string.IsNullOrWhiteSpace(vsCodeDir))
            {
                Environment.SetEnvironmentVariable(string.Format("{0}Dir", VSCode), ResolveValue.Inst.ResolveFullPath(vsCodeDir), EnvironmentVariableTarget.Process);
            }

        }

        public void UpdateMenuBar(MenuStrip menuStrip, List<ActionVerb> actions, EventHandler toolStripClick, EventHandler toolStripActionClick)
        {
            if (this.MenuBar != null && this.MenuBar.Count > 0)
            {
                foreach (var item in this.MenuBar)
                {
                    item.UpdateToolMenu(menuStrip, actions, toolStripClick, toolStripActionClick);
                }
            }
        }

        public void ProcessRuntimeInfo()
        {
            AutoDetectToolPath();


            if (Envs != null)
            {
                foreach (var item in Envs)
                {
                    string currentValue = Environment.GetEnvironmentVariable(item.Name);
                    if (!string.IsNullOrWhiteSpace(currentValue))
                    {
                        switch (item.Action)
                        {
                            case EnvironmentAction.Overwrite:
                                {
                                    string newValue = ResolveValue.Inst.ResolveEnvironmentValue(item.Type,item.Value);
                                    Environment.SetEnvironmentVariable(item.Name, newValue, EnvironmentVariableTarget.Process);
                                }
                                break;
                            case EnvironmentAction.Prefix:
                                {
                                    string newValue = ResolveValue.Inst.ResolveEnvironmentValue(item.Type, item.Value) + ";" + currentValue;
                                    Environment.SetEnvironmentVariable(item.Name, newValue, EnvironmentVariableTarget.Process);
                                }
                                break;
                            case EnvironmentAction.Append:
                                {
                                    string newValue = currentValue + ";" + ResolveValue.Inst.ResolveEnvironmentValue(item.Type, item.Value);
                                    Environment.SetEnvironmentVariable(item.Name, newValue, EnvironmentVariableTarget.Process);
                                }
                                break;
                        }
                    }
                    else
                    {
                        string newValue = ResolveValue.Inst.ResolveEnvironmentValue(item.Type, item.Value);
                        Environment.SetEnvironmentVariable(item.Name, newValue, EnvironmentVariableTarget.Process);
                    }
                }
            }

            if(ToolPath == null)
            {
                ToolPath = new List<Tool>();
            }

            //merge tool bar
            if(ToolBar != null)
            {
                foreach(var item in ToolBar)
                {
                    ToolPath.AddRange(item.Group);
                }
            }

            //merge editors to tool path
            if(MenuBar != null)
            {
                foreach (var item in MenuBar)
                {
                    if (item.Tools != null)
                    {
                        ToolPath.AddRange(item.Tools);
                    }
                }
            }

            foreach(var item in ToolPath)
            {
                string newValue = ResolveValue.Inst.ResolveFullPath(item.Path);
                item.Path = newValue;

                newValue = ResolveValue.Inst.ResolveFullPath(item.BaseDir);
                item.BaseDir = newValue;
            }
        }

        public static void LoadConfig()
        {
            string filePath = ToolConfigFile;
            if (File.Exists(filePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(RuntimeInfo));
                // A FileStream is needed to read the XML document.
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    _inst = (RuntimeInfo)serializer.Deserialize(fs);
                }
            }

            //if (_inst != null)
            //{
            //    _inst.ProcessRuntimeInfo();
            //}
        }

        public static void SaveConfig()
        {
            string filePath = ToolConfigFile;
            if (File.Exists(filePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(RuntimeInfo));
                // A FileStream is needed to read the XML document.
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    serializer.Serialize(fs, _inst);
                }
            }
        }
        static RuntimeInfo _inst = new RuntimeInfo();
        public static RuntimeInfo Inst
        {
            get
            {
                return _inst;
            }
        }
    }
}
