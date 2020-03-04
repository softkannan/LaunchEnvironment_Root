using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LaunchEnvironment
{
    public class ErrorLog
    {
        private static ErrorLog _inst = new ErrorLog();
        public static ErrorLog Inst
        {
            get
            {
                return _inst;
            }
        }
        public void LogError(string format,params object[] args)
        {
            string message = string.Format(format, args);
            MessageBox.Show(message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
        }

        public void ShowError(string format, params object[] args)
        {
            string message = string.Format(format, args);
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void ShowInfo(string format, params object[] args)
        {
            string message = string.Format(format, args);
            MessageBox.Show(message, "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
