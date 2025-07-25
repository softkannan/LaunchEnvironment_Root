﻿using LaunchEnvironment.Config;
using LaunchEnvironment.Config.EnvConfig;
using LaunchEnvironment.Utility;
using Microsoft.Win32;
using Newtonsoft.Json;
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
    public class UserConfig
    {

        public UserConfig()
        {
            OpenFolder = null;
            string exePath = Assembly.GetExecutingAssembly().Location;
            LaunchEnvExeDir = Path.GetDirectoryName(exePath);
            RunScriptPath = string.Format(@"{0}\runscript.exe", LaunchEnvExeDir);
            
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


        #region Config File Public Entires

        public List<EnviromentVariable> Envs { get; set; }
        public bool RunasAdmin { get; set; }
        public bool ShowRunAsForAll { get; set; }
        public List<Tool> Tools { get; set; }
        public List<MenuBar> MenuBar { get; set; }
        public List<ToolBarItem> ToolBar { get; set; }
        public string DefaultWorkspace { get; set; }

        [XmlArray("ContextMenu")]
        [XmlArrayItem(ElementName = "Menu")]
        public List<string> ContextMenu { get; set; }

        #endregion

        private string _launchEnvExeDir = null;

        [XmlIgnore]
        public string LaunchEnvExeDir
        {
            get { return _launchEnvExeDir; }
            private set
            {
                _launchEnvExeDir = ResolveValue.Inst.ResolveFullPath(value);
                Environment.SetEnvironmentVariable("LaunchEnvExeDir", _launchEnvExeDir, EnvironmentVariableTarget.Process);
                Environment.CurrentDirectory = _launchEnvExeDir;
            }
        }

        [XmlIgnore]
        [JsonIgnore]
        public string OpenFolder { get; set; }

        [XmlIgnore]
        public List<Tool> AvailableTools { get; private set; }

        [XmlIgnore]
        public bool IsElevated
        {
            get
            {
                return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        [XmlIgnore]
        public bool Is64Bit
        {
            get
            {
                return IntPtr.Size == 8;
            }
        }

        [XmlIgnore]
        public string RunScriptPath { get; private set; }

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
                string retVal = string.Format("{0}\\Config\\CurrentConfig_{1}.xml", Path.GetDirectoryName(fileLocation), UserConfig.Inst.CurrentTimeStamp);
                return retVal;
            }
        }

        [XmlIgnore]
        private static string ConfigFile
        {
            get
            {
                string fileLocation = Assembly.GetExecutingAssembly().Location;
                string retVal = string.Format("{0}\\Config\\UserConfig.json", Path.GetDirectoryName(fileLocation));
                return retVal;
            }
        }

        public bool IsToolAvailable(string toolName)
        {
            var foundTool = AvailableTools.FirstOrDefault((item) => string.Compare(item.Name, toolName,true) == 0);
            return foundTool != null;
        }

        public Tool GetTool(string toolName)
        {
            Tool retVal = null;
            var foundTool = AvailableTools.FirstOrDefault((item) => item.Name == toolName);
            if (foundTool != null)
            {
                retVal = foundTool;
            }
            return retVal;
        }

        public string GetToolPath(string toolName)
        {
            string retVal = "";
            var foundTool = AvailableTools.FirstOrDefault((item) => item.Name == toolName);
            if(foundTool != null)
            {
                retVal = foundTool.Path;
            }
            return retVal;
        }

        public void UpdateMenuBar(MenuStrip menuStrip, List<ActionVerb> actions, EventHandler toolStripClick)
        {
            if (this.MenuBar != null && this.MenuBar.Count > 0)
            {
                foreach (var item in this.MenuBar)
                {
                    item.UpdateToolMenu(menuStrip, actions, toolStripClick);
                }
            }
        }

        public void Process()
        {
            //AutoDetectToolPath();
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

            if(AvailableTools == null)
            {
                AvailableTools = new List<Tool>();
            }

            foreach(var item in Tools)
            {
                string newValue = ResolveValue.Inst.ResolveFullPath(item.Path);
                item.Path = newValue;

                newValue = ResolveValue.Inst.ResolveFullPath(item.ToolDir);
                item.ToolDir = newValue;

                Tool addTool = null;

                if(item.Type == ToolType.RegularApp)
                {
                    if(File.Exists(item.Path))
                    {
                        addTool = item;
                    }
                }
                else
                {
                    addTool = item;
                }

                if(addTool != null)
                {
                    //if editor is not explicitly defined then assume tool name and editor name is same
                    if(string.IsNullOrWhiteSpace(addTool.Editor))
                    {
                        addTool.Editor = addTool.Name;
                    }
                    AvailableTools.Add(addTool);
                }
            }
        }

        public static void LoadConfig()
        {
            LoadConfigJson();
        }

        private static void LoadConfigJson()
        {
            string filePath = ConfigFile;
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "LaunchEnvironment.Resource.Config.UserConfig.json";

            // A FileStream is needed to read the XML document.
            using (var fs = File.Exists(filePath) ? new StreamReader(filePath) : new StreamReader(assembly.GetManifestResourceStream(resourceName)))
            {
                try
                {
                    var json = fs.ReadToEnd();
                    var settings = new JsonSerializerSettings
                    {
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver
                        {
                            NamingStrategy = new Newtonsoft.Json.Serialization.DefaultNamingStrategy()
                        }
                    };
                    UserConfig obj = JsonConvert.DeserializeObject<UserConfig>(json, settings);
                    Inst = obj;
                }
                catch (Exception ex)
                {
                    ErrorLog.Inst.LogError($"Unable to load Config file : {filePath} : {ex.Message}");
                }
            }
#if !DEBUG
            if (!File.Exists(filePath))
            {
                ErrorLog.Inst.ShowInfo($"Unable to find `{Path.GetFileName(filePath)}` Config file : {filePath}");
            }
#endif
        }

        private static void LoadConfigXml()
        {
            string filePath = ConfigFile;

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "LaunchEnvironment.Resource.Config.UserConfig.xml";

            XmlSerializer serializer = new XmlSerializer(typeof(UserConfig));
            // A FileStream is needed to read the XML document.
            using (var fs = File.Exists(filePath) ? new StreamReader(filePath) : new StreamReader(assembly.GetManifestResourceStream(resourceName)))
            {
                try
                {
                    Inst = (UserConfig)serializer.Deserialize(fs);
                }
                catch (Exception ex)
                {
                    ErrorLog.Inst.LogError($"Unable to load Config file : {filePath} : {ex.Message}");
                }
            }
#if !DEBUG
            if (!File.Exists(filePath))
            {
                ErrorLog.Inst.ShowInfo($"Unable to find `{Path.GetFileName(filePath)}` Config file : {filePath}");
            }
#endif
        }

        public static void SaveConfig()
        {
            string filePath = ConfigFile;
            if (File.Exists(filePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(UserConfig));
                // A FileStream is needed to read the XML document.
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    serializer.Serialize(fs, Inst);
                }
            }
        }
        public static UserConfig Inst { get; private set; } = null;
    }
}
