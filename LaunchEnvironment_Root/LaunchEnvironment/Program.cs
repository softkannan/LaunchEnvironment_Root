using LaunchEnvironment.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LaunchEnvironment
{
    static class Program
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            RuntimeInfo.LoadConfig();

            if (RuntimeInfo.Inst.RunasAdmin == false || RuntimeInfo.Inst.IsElevated)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new mainForm());
            }
            else
            {
                ProcessStartInfo procInfo = new ProcessStartInfo();

                procInfo.FileName = Assembly.GetEntryAssembly().Location;
                procInfo.Verb = "runas";
                if (Environment.GetCommandLineArgs().Length > 1)
                {
                    procInfo.Arguments = Environment.GetCommandLineArgs()[1];
                }

                Process.Start(procInfo);
            }

            //string curArg=null;
            //var args = Environment.GetCommandLineArgs();
            //if(args.Length > 1)
            //{
            //    curArg = args[1];
            //}

            //if (Properties.Settings.Default.RunasAdmin == false && (curArg == null || curArg != "runas"))
            //{
            //    Application.EnableVisualStyles();
            //    Application.SetCompatibleTextRenderingDefault(false);
            //    Application.Run(new mainForm());
            //}
            //else
            //{
            //    ProcessStartInfo procInfo = new ProcessStartInfo();

            //    procInfo.FileName = Assembly.GetEntryAssembly().Location;
            //    procInfo.Verb = "runas";
            //    procInfo.Arguments = curArg;

            //    Process.Start(procInfo);
            //}

            //bool createdNew = true;
            //using (Mutex mutex = new Mutex(true, "LaunchEnvironment_JillKannan", out createdNew))
            //{
            //    if (createdNew)
            //    {
            //        Application.EnableVisualStyles();
            //        Application.SetCompatibleTextRenderingDefault(false);
            //        Application.Run(new mainForm());
            //    }
            //    else
            //    {
            //        Process current = Process.GetCurrentProcess();
            //        foreach (Process process in Process.GetProcessesByName(current.ProcessName))
            //        {
            //            if (process.Id != current.Id)
            //            {
            //                SetForegroundWindow(process.MainWindowHandle);
            //                break;
            //            }
            //        }
            //    }
            //}
        }
    }
}
