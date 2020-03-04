using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LaunchEnvironment.Config
{
    public class ResolveValue
    {
        public ResolveValue()
        {
            string exePath = Assembly.GetExecutingAssembly().Location;

            WorkingDir = Path.GetDirectoryName(exePath);
        }
        private static ResolveValue _inst = new ResolveValue();
        public static ResolveValue Inst { get { return _inst; } }

        public string WorkingDir { get; set; }

        public string ResolveFullPath(in string path)
        {
            if(string.IsNullOrEmpty(path))
            {
                return "";
            }

            try
            {

                string expandedValue = Environment.ExpandEnvironmentVariables(path);

                if (expandedValue.IndexOf("..\\") == -1 &&
                    expandedValue.IndexOf(@".\\") == -1 &&
                    expandedValue.IndexOf("../") == -1 &&
                    expandedValue.IndexOf(@"./") == -1 &&
                    expandedValue.IndexOf(@".") != 0 &&
                    expandedValue.IndexOf(@"..") != 0)
                {
                    if (Directory.Exists(expandedValue))
                    {
                        return expandedValue;
                    }
                    if (File.Exists(expandedValue))
                    {
                        return expandedValue;
                    }
                }

                try
                {
                    string relativePath = Path.GetFullPath(Path.Combine(WorkingDir, expandedValue));
                    if (Directory.Exists(relativePath))
                    {
                        return relativePath;
                    }
                    if (File.Exists(relativePath))
                    {
                        return relativePath;
                    }
                }
                catch (Exception)
                { }

                return expandedValue;
            }
            catch(Exception)
            {
            }
            return path;
        }
        public string ResolveEnvironmentValue(in EnvironmentValueType type, in string newValue)
        {
            string retVal="";

            switch(type)
            {
                case EnvironmentValueType.Path:
                    {
                        if (!string.IsNullOrWhiteSpace(newValue))
                        {
                            var envValues = newValue.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                            if (envValues != null && envValues.Length > 0)
                            {
                                foreach (var item in envValues)
                                {
                                    try
                                    {
                                        string finalPath = ResolveFullPath(item);
                                        retVal += finalPath + ";";
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                            }
                        }
                        return retVal.TrimEnd(';');
                    }
                default:
                    retVal = string.IsNullOrWhiteSpace(newValue) ? "" : newValue;
                    break;
            }
            return retVal;
        }

        public object ResolveRegistryValue(in string regValue, in EnvironmentValueType type, out RegistryValueKind kind)
        {
            object retVal = null;

            switch (type)
            {
                case EnvironmentValueType.Path:
                    {
                        if (!string.IsNullOrWhiteSpace(regValue))
                        {
                            string pathValue = "";
                            var regValues = regValue.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                            if (regValues != null && regValues.Length > 0)
                            {
                                foreach (var item in regValues)
                                {
                                    try
                                    {
                                        string finalPath = ResolveFullPath(item);
                                        pathValue += finalPath + ";";
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                            }
                            retVal = pathValue.TrimEnd(';');
                        }
                        else
                        {
                            retVal = "";
                        }
                        kind = RegistryValueKind.String;
                    }
                    break;
                case EnvironmentValueType.String:
                    retVal = regValue;
                    kind = RegistryValueKind.String;
                    break;
                case EnvironmentValueType.Binary:
                    throw new InvalidOperationException("ValueType.Binary not supported");
                    //break;
                case EnvironmentValueType.DWord:
                    if (string.IsNullOrWhiteSpace(regValue))
                    {
                        retVal = Int32.Parse(regValue);
                    }
                    else
                    {
                        retVal = 0;
                    }
                    kind = RegistryValueKind.String;
                    break;
                case EnvironmentValueType.ExpandString:
                    retVal = regValue;
                    kind = RegistryValueKind.ExpandString;
                    break;
                case EnvironmentValueType.MultiString:
                    retVal = regValue;
                    kind = RegistryValueKind.MultiString;
                    break;
                case EnvironmentValueType.None:
                    throw new InvalidOperationException("ValueType.None not supported");
                    //break;
                case EnvironmentValueType.QWord:
                    if (string.IsNullOrWhiteSpace(regValue))
                    {
                        retVal = Int64.Parse(regValue);
                    }
                    else
                    {
                        retVal = (Int64) 0;
                    }
                    kind = RegistryValueKind.QWord;
                    break;
                case EnvironmentValueType.Unknown:
                    throw new InvalidOperationException("ValueType.Unknown not supported");
                    //break;
                default:
                    retVal = regValue;
                    kind = RegistryValueKind.String;
                    break;
            }
            return retVal;
        }

    }
}
