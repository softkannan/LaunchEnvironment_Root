using LaunchEnvironment.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LaunchEnvironment.Config
{
    public class MenuBar
    {
        public string Name { get; set; }
        public List<Tool> Tools { get; set; }


        private ToolStripMenuItem _toolStripMenuItem;

       
        public void UpdateToolMenu(MenuStrip menuStrip, List<ActionVerb> actions, EventHandler toolStripClick, EventHandler toolStripActionClick)
        {
            if (Tools != null && Tools.Count > 0)
            {
                CreateTopMenu();
                GUIUtility.GenerateMenu(_toolStripMenuItem, Tools, actions, toolStripClick, toolStripActionClick);
                _toolStripMenuItem.Visible = true;
                menuStrip.Items.Insert(0,_toolStripMenuItem);
            }
        }

        private void CreateTopMenu()
        {
            _toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _toolStripMenuItem.Name = Name;
            _toolStripMenuItem.Size = new System.Drawing.Size(82, 29);
            _toolStripMenuItem.Text = Name;
            _toolStripMenuItem.Visible = false;
        }
    }
}
