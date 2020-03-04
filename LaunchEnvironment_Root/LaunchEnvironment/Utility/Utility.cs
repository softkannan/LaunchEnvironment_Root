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
