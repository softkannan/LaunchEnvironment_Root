using LaunchEnvironment.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LaunchEnvironment.Utility
{
    public class GUIUtility
    {
        public static void GenerateMenu(ToolStripMenuItem rootMenu, List<Tool> tools, List<ActionVerb> actions, EventHandler toolStripClick, EventHandler toolStripActionClick)
        {
            foreach (var item in tools)
            {
                if (File.Exists(item.Path) || !string.IsNullOrWhiteSpace(item.Editor) || item.IsStoreApp)
                {
                    var toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                    toolStripMenuItem.Name = string.Format("{0}_{1}", rootMenu.Name, item.Name);
                    toolStripMenuItem.Text = item.Name;
                    toolStripMenuItem.Tag = item.Editor == null ? item.Name : item.Editor;
                    //toolStripMenuItem.Click += EditorsToolStripMenuItem_Click;
                    toolStripMenuItem.Click += toolStripClick;
                    if (RuntimeInfo.Inst.ShowRunAsForAll)
                    {
                        var retMenu = GenerateActionMenus(toolStripMenuItem.Name, toolStripMenuItem.Tag as string, actions, toolStripActionClick);
                        if (retMenu.Length > 0)
                        {
                            toolStripMenuItem.DropDownItems.AddRange(retMenu);
                        }
                    }
                    rootMenu.DropDownItems.Add(toolStripMenuItem);
                }
            }
        }

        public static ToolStripItem[] GenerateActionMenus(string name, string tag, List<ActionVerb> actions, EventHandler toolStripActionClick)
        {
            List<ToolStripItem> retVal = new List<ToolStripItem>();
            foreach (var item in actions)
            {
                var toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                toolStripMenuItem.Name = string.Format("{0}_{1}", name, item.Name);
                toolStripMenuItem.Text = item.Name;
                toolStripMenuItem.Tag = string.Format("{0};{1}", tag, item.Verb);
                //toolStripMenuItem.Click += EditorsToolStripActionMenuItem_Click;
                toolStripMenuItem.Click += toolStripActionClick;
                retVal.Add(toolStripMenuItem);
            }
            return retVal.ToArray();
        }
    }
}
