using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Testing
{
    public partial class TestingForm : Form
    {
        public TestingForm()
        {
            InitializeComponent();
        }

        private void TestingForm_Load(object sender, EventArgs e)
        {
            foreach(var item in Enum.GetValues(typeof(Environment.SpecialFolder)))
            {
                WriteLine("{0} = {1}", ((Environment.SpecialFolder)item).ToString(), Environment.GetFolderPath((Environment.SpecialFolder)item));
            }
        }

        void WriteLine(string format,params object[] args)
        {
            string resulStr = String.Format(format, args);
            consoleList.Items.Add(resulStr);
        }
    }
}
