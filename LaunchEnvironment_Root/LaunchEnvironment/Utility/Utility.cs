using LaunchEnvironment.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LaunchEnvironment.Utility
{
    public static class Utility
    {
        public static string GetWarningMsg(this Tool pThis, string warningType)
        {
            if (pThis == null || pThis.Warnings == null || pThis.Warnings.Count == 0)
            {
                return "";
            }
            StringBuilder retVal = new StringBuilder();
            foreach (var warning in pThis.Warnings)
            {
                if (warning.StartsWith(warningType, StringComparison.OrdinalIgnoreCase))
                {
                    string[] parts = warning.Split(new char[] { ':' }, 2);
                    return parts.Length > 1 ? parts[1].Trim() : warning;
                }
            }
            return "";
        }
        public static string MatchReplace(this string pThis,string matchStr, string replaceStr,out bool foundMatch)
        {
            foundMatch = false;
            StringBuilder retVal = null;
            if (string.IsNullOrWhiteSpace(pThis) || string.IsNullOrWhiteSpace(matchStr) || string.IsNullOrWhiteSpace(replaceStr))
            {
                return "";
            }

            if(matchStr.Length > pThis.Length)
            {
                return pThis;
            }

            int previousIndex = 0;
            int index = pThis.IndexOf(matchStr, StringComparison.OrdinalIgnoreCase);
            if (index != -1)
            {
                foundMatch = true;
                retVal = new StringBuilder();
            }

            while (index != -1)
            {
                retVal.Append(pThis.Substring(previousIndex, index - previousIndex));
                retVal.Append(replaceStr);
                index += matchStr.Length;

                previousIndex = index;
                index = pThis.IndexOf(matchStr, index, StringComparison.OrdinalIgnoreCase);
            }

            if (retVal != null)
            {
                retVal.Append(pThis.Substring(previousIndex));
                return retVal.ToString();
            }
            return pThis;
        }
        public static bool IsExists<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            bool retVal = false;
            foreach(var item in source)
            {
                retVal = predicate(item);
                if(retVal)
                {
                    break;
                }
            }
            return retVal;
        }
        /// <summary>
        /// Do not use this for large txt files
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="pattern">regex pattern</param>
        /// <param name="replacement">replacement patten</param>
        public static bool ReplaceTest(string filename, string pattern,string replacement,bool isRegEx = false)
        {
            bool retVal = false;
            try
            {
                string text = File.ReadAllText(filename);

                if(isRegEx)
                {
                    Regex regEx = new Regex(pattern, RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Multiline);
                    text = regEx.Replace(text, replacement);
                }
                else
                {
                    text = text.Replace(pattern, replacement);
                }
                File.WriteAllText(filename, text);
                retVal = true;
            }
            catch(Exception)
            {

            }
            return retVal;
        }
    }
}
