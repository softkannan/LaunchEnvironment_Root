using LaunchEnvironment.Config;
using LaunchEnvironment.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LaunchEnvironment.Editors
{
    public class KnownCommandEditor : EditorDefault
    {
        [DllImport("shell32.dll", EntryPoint = "#61", CharSet = CharSet.Unicode)]
        public static extern int RunFileDlg([In] IntPtr hwndOwner, [In] IntPtr hIcon, [In] string workingDirectory, [In] string title, [In] string prompt, [In] uint flags);


        public KnownCommandEditor() :base()
        {

        }

        public void UpdateConfigRegistry(List<Config.Config> envs)
        {
            foreach (var itemEnv in envs)
            {
                if (itemEnv.RegConfigs == null)
                {
                    continue;
                }

                var regConfig = RegConfig.RegConfigBase.GetConfig(itemEnv.Id);

                foreach (var item in itemEnv.RegConfigs)
                {
                    if (regConfig != null)
                    {
                        regConfig.SetSonfig(item);
                    }
                }
            }
        }

        //public void GenerateMakeFile(LaunchConfig config)
        //{
        //    var activeEnv = config.Configs.FirstOrDefault();
        //    if (Directory.Exists(RuntimeInfo.Inst.OpenFolder) && activeEnv != null)
        //    {
        //        string folderDir = RuntimeInfo.Inst.OpenFolder;
        //        string fileName = string.Format(@"{0}\makefile", folderDir);
        //        if (!File.Exists(fileName))
        //        {
        //            string srcFolder = string.Format(@"{0}\src", folderDir);
        //            string goldMakefile = string.Format(@"{0}\OpenFolder_Resource\makefile", folderDir);
        //            if (Directory.Exists(srcFolder) && File.Exists(goldMakefile))
        //            {
        //                using (TextWriter writer = new StreamWriter(fileName))
        //                {
        //                    using (TextReader reader = new StreamReader(goldMakefile))
        //                    {
        //                        writer.Write(reader.ReadToEnd());
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        private void UpdatePythonScripts(Configs_Root allEnvironments)
        {
            foreach (var item in allEnvironments.Configs)
            {
                if (!string.IsNullOrWhiteSpace(item.Id) && item.Id.ToLower() == "python")
                {
                    string scriptsFolder = string.Format("{0}\\Scripts", ResolveValue.Inst.ResolveFullPath(item.ConfigPath));

                    if (Directory.Exists(scriptsFolder))
                    {
                        Regex regExScripts = new Regex("([A-Za-z]:[^\\; \\<\\>\\:\\\"\\|\\?\\*]+)Scripts", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Multiline);
                        Regex regExpython = new Regex("([A-Za-z]:[^\\; \\<\\>\\:\\\"\\|\\?\\*]+)python", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Multiline);
                        string pythonPath = string.Format("{0}\\python", ResolveValue.Inst.ResolveFullPath(item.ConfigPath));

                        foreach (var filename in Directory.GetFiles(scriptsFolder, "*.bat"))
                        {
                            try
                            {
                                string text = File.ReadAllText(filename);

                                text = regExScripts.Replace(text, scriptsFolder);
                                text = regExpython.Replace(text, pythonPath);

                                File.WriteAllText(filename, text);
                            }
                            catch (Exception ex)
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

        protected override bool LaunchCustom(LaunchConfig config)
        {
            switch(Tool.Path.Trim())
            {
                case "Run":
                    {
                        RunFileDlg(_mainForm.Handle, IntPtr.Zero, Environment.CurrentDirectory, null, null, 0);
                    }
                    break;
                case "RegisterExplorerContextMenu":
                    {
                        if (ShellIntegration.RegisterContextMenu())
                        {
                            ErrorLog.Inst.ShowInfo("Explorer Context Menu Registration Completed.");
                        }
                    }
                    break;
                case "WriteConfigRegistryValues":
                    {
                        UpdateConfigRegistry(Configs_Root.Inst.Configs);

                        ErrorLog.Inst.ShowInfo("Environment Registry Integration completed");
                    }
                    break;
                case "UpdatePythonScriptFolder":
                    {
                        UpdatePythonScripts(Configs_Root.Inst);

                        ErrorLog.Inst.ShowInfo("Updating Python Environment Scripts folder is completed");
                    }
                    break;
                default:
                    {
                        ErrorLog.Inst.ShowError("Unable to find known command: {0}", Tool.Path);
                    }
                    break;
            }

            return false;
        }
    }
}
