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

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        #region Embedding References
        private static readonly string[] embeddedAssemblies = new string[]
        {
            "Newtonsoft.Json",
        };

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var assemblyName = args.Name.Split(',')[0]; // Get the assembly name without version and culture info
            if (embeddedAssemblies.Length > 0 && embeddedAssemblies.Contains(assemblyName, StringComparer.OrdinalIgnoreCase))
            {
                var resourceName = $"LaunchEnvironment.Resource.Assembly.{assemblyName}.dll";
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                {
                    var assemblyData = new Byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    return Assembly.Load(assemblyData);
                }
            }
            return null; // Return null to indicate that the assembly could not be resolved
        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            if (Environment.OSVersion.Version.Major >= 6)
                SetProcessDPIAware();

            UserConfig.LoadConfig();
            
            if (UserConfig.Inst.RunasAdmin == false || UserConfig.Inst.IsElevated)
            {
                //Application.SetHighDpiMode(HighDpiMode.DpiUnawareGdiScaled);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.ThreadException += Application_ThreadException;
                Application.ApplicationExit += Application_ApplicationExit;
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

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
#if DEBUG
            ErrorLog.Inst.LogError("LaunchEnvironment Closed.");
#endif
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            ErrorLog.Inst.LogError("Unhandled Exception : {0}", e.Exception.Message);
        }
    }
}
