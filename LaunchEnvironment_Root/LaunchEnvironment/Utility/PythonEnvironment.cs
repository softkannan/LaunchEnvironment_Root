using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LaunchEnvironment.Config;

namespace LaunchEnvironment.Utility
{
    public class PythonEnvironment
    {
        public void UpdateScripts(Configs_Root allEnvironments)
        {
            foreach(var item in allEnvironments.Configs)
            {
                if(item.Type == ConfigType.python)
                {
                    string scriptsFolder = string.Format("{0}\\Scripts", ResolveValue.Inst.ResolveFullPath(item.InstallPath));

                    if(Directory.Exists(scriptsFolder))
                    {
                        Regex regExScripts = new Regex("([A-Za-z]:[^\\; \\<\\>\\:\\\"\\|\\?\\*]+)Scripts", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Multiline);
                        Regex regExpython =  new Regex("([A-Za-z]:[^\\; \\<\\>\\:\\\"\\|\\?\\*]+)python", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Multiline);
                        string pythonPath = string.Format("{0}\\python", ResolveValue.Inst.ResolveFullPath(item.InstallPath));

                        foreach (var filename in Directory.GetFiles(scriptsFolder,"*.bat"))
                        {
                            try
                            {
                                string text = File.ReadAllText(filename);

                                text = regExScripts.Replace(text, scriptsFolder);
                                text = regExpython.Replace(text, pythonPath);

                                File.WriteAllText(filename, text);
                            }
                            catch(Exception ex)
                            {
                                ErrorLog.Inst.LogError("Unable to Update file : {0} error : {1}", filename, ex.Message);
                                break;
                            }
                        }
                    }
                    else
                    {
                        ErrorLog.Inst.LogError("Unable to find the scripts folder : {0}", scriptsFolder);
                    }

                }
            }
        }
    }
}
