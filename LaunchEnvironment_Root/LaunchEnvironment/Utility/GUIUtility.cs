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
        public static void GenerateMenu(ToolStripMenuItem rootMenu, List<string> tools, List<ActionVerb> actions, EventHandler toolStripClick)
        {
            foreach (var item in tools)
            {
                //TODO: dont't check if tool is available, just add it to the menu
                if (RuntimeInfo.Inst.IsToolAvailable(item))
                {
                    var tool = RuntimeInfo.Inst.GetTool(item);
                    var toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                    toolStripMenuItem.Name = string.Format("{0}_{1}", rootMenu.Name, tool.Name);
                    toolStripMenuItem.Text = tool.Name;
                    toolStripMenuItem.Tag = tool.Name;
                    //toolStripMenuItem.Click += EditorsToolStripMenuItem_Click;
                    toolStripMenuItem.Click += toolStripClick;
                    if (RuntimeInfo.Inst.ShowRunAsForAll)
                    {
                        var retMenu = GenerateContextActionMenus(toolStripMenuItem.Name, toolStripMenuItem.Tag as string, actions, toolStripClick);
                        if (retMenu.Length > 0)
                        {
                            toolStripMenuItem.DropDownItems.AddRange(retMenu);
                        }
                    }
                    rootMenu.DropDownItems.Add(toolStripMenuItem);
                }
                else
                {
#if !DEBUG
                    // Log error if tool is not available
                    ErrorLog.Inst.LogError("Tool {0} is not available.", item);
#endif
                }
            }
        }

        public static ToolStripItem[] GenerateContextActionMenus(string name, string tag, List<ActionVerb> actions, EventHandler toolStripActionClick)
        {
            List<ToolStripItem> retVal = new List<ToolStripItem>();
            foreach (var item in actions)
            {
                var toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                toolStripMenuItem.Name = string.Format("{0}_{1}", name, item.Name);
                toolStripMenuItem.Text = item.Name;
                // Context menu tag item always contains "Editor valus as "ContextMenu" and verb
                toolStripMenuItem.Tag = string.Format("{0};{1}", tag, item.Verb);
                //toolStripMenuItem.Click += EditorsToolStripActionMenuItem_Click;
                toolStripMenuItem.Click += toolStripActionClick;
                retVal.Add(toolStripMenuItem);
            }
            return retVal.ToArray();
        }
    }
}
