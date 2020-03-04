using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RunScriptLib
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            ProcessEx.NativeMethods.RunFileDlg(this.Handle, IntPtr.Zero, Environment.CurrentDirectory, null, null, 0);

            this.Close();
        }
    }
}
