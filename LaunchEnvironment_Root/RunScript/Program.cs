using RunScriptLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RunScript
{
    static class Program
    {
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Debug.Print(Environment.CommandLine);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ScriptRunner.ScriptRunMain(Environment.GetCommandLineArgs().ToList(),false);
        }
    }
}
