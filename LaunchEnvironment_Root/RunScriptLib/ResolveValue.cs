using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RunScriptLib
{
    public class ResolveValue
    {
        public ResolveValue()
        {
            WorkingDir = Environment.CurrentDirectory;
        }
        private static ResolveValue _inst = new ResolveValue();
        public static ResolveValue Inst { get { return _inst; } }

        public string WorkingDir { get; set; }

        public string ResolveFullFilePath(string path, params string[] environmentVar)
        {
            if (string.IsNullOrEmpty(path))
            {
                return "";
            }

            string expandedValue = Environment.ExpandEnvironmentVariables(path);
            if(File.Exists(expandedValue))
            {
                return expandedValue;
            }
            string finalPath = Path.GetFullPath(Path.Combine(WorkingDir, expandedValue));
            if (File.Exists(finalPath))
            {
                return finalPath;
            }

            foreach(var envVarName in environmentVar)
            {
                var envValue = Environment.GetEnvironmentVariable(envVarName);

                if (!string.IsNullOrWhiteSpace(envValue))
                {
                    var listOfPathValues = envValue.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    if (listOfPathValues != null && listOfPathValues.Length > 0)
                    {
                        foreach (string item in listOfPathValues)
                        {
                            finalPath = Path.GetFullPath(Path.Combine(item, expandedValue));
                            if (File.Exists(finalPath))
                            {
                                return finalPath;
                            }
                        }
                    }
                }
            }
            return expandedValue;
        }

        public string ResolveFullPath(string path)
        {
            if(string.IsNullOrEmpty(path))
            {
                return "";
            }

            string expandedValue = Environment.ExpandEnvironmentVariables(path);
            if (File.Exists(expandedValue) || Directory.Exists(expandedValue))
            {
                return expandedValue;
            }
            string finalPath = Path.GetFullPath(Path.Combine(WorkingDir, expandedValue));
            return finalPath;
        }
    }
}
