using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using LaunchEnvironment.Config;
using LaunchEnvironment.Utility;
using LaunchEnvironment.Properties;
using LaunchEnvironment.Editors;

namespace LaunchEnvironment
{
    partial class mainForm
    {
        private List<ToolStripItem> _toolStripItem = new List<ToolStripItem>();

        private void InitializeCustom()
        {
            this.envlistContextMenu.SuspendLayout();
            this.mainMenu.SuspendLayout();
            this.SuspendLayout();

            EditorFactory.Inst.AddBuilder("RegisterExplorerContextMenu", (item) => new KnownCommandEditor(item));
            EditorFactory.Inst.AddBuilder("WriteConfigRegistryValues", (item) => new KnownCommandEditor(item));
            EditorFactory.Inst.AddBuilder("UpdatePythonScriptFolder", (item) => new KnownCommandEditor(item));

            Config.Configs_Root.LoadEnvironments();

            RuntimeInfo.Inst.ProcessRuntimeInfo();

            if (Environment.GetCommandLineArgs().Length > 1)
            {
                string workingDir = Environment.GetCommandLineArgs()[1];
                RuntimeInfo.Inst.ToolLaunchDir = workingDir.Trim('\"');
                RuntimeInfo.Inst.IsOpenFolder = true;
            }

            BuildControls();

            this.envlistContextMenu.ResumeLayout(false);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

            BuildEnvironmentListBox();
        }
        private void BuildEnvironmentListBox()
        {
            if (Configs_Root.Inst == null)
            {
                return;
            }

            int multiplier = envList.Height;
            int formMaxHeight = this.MaximumSize.Height;
            int currentHeight = this.Height;

            //envList.Items.Add("System Default", false);

            foreach (var itemEnv in Configs_Root.Inst.Configs)
            {
                envList.Items.Add(itemEnv.Name, false);
            }

            if (envList.Items.Count > 0)
            {
                envList.SelectedIndex = 0;
            }

            var newHeight = currentHeight + (Configs_Root.Inst.Configs.Count * multiplier);
            if(newHeight > formMaxHeight)
            {
                this.Height = formMaxHeight;
            }
            else
            {
                this.Height = newHeight;
            }
        }

        private void BuildControls()
        {
            List<ActionVerb> actions = new List<ActionVerb>();

            if (!RuntimeInfo.Inst.IsElevated)
            {
                actions.Add(new ActionVerb() { Name = "Run", Verb = "" });
                actions.Add(new ActionVerb() { Name = "RunAs Admin", Verb = "runas" });
            }
            
            var menu = GUIUtility.GenerateActionMenus(runAsContext.Name, "ContextMenu", actions, EditorsToolStripActionMenuItem_Click);
            runAsContext.Items.AddRange(menu);

            if (RuntimeInfo.Inst.ToolBar != null && RuntimeInfo.Inst.ToolBar.Count > 0)
            {
                GenerateToolbar(mainToolBar, RuntimeInfo.Inst.ToolBar, actions);
            }

            RuntimeInfo.Inst.UpdateMenuBar(mainMenu, actions, EditorsToolStripMenuItem_Click, EditorsToolStripActionMenuItem_Click);

            if (RuntimeInfo.Inst.ContextMenu != null && RuntimeInfo.Inst.ContextMenu.Count > 0)
            {
                GenerateContextMenu(envlistContextMenu, RuntimeInfo.Inst.ContextMenu);
            }
        }

        private void GenerateContextMenu(ContextMenuStrip rootMenu, List<string> menuItems)
        {
            foreach(var item in menuItems)
            {
                var toolStripMenuItem = new ToolStripMenuItem();
                toolStripMenuItem.Name = string.Format("{0}_{1}", rootMenu.Name, item);
                toolStripMenuItem.Text = item;
                toolStripMenuItem.Tag = item;
                toolStripMenuItem.Click += EditorsToolStripMenuItem_Click;
                rootMenu.Items.Add(toolStripMenuItem);
            }
        }

        private void GenerateToolbar(ToolStrip rootToolStrip,List<ToolBarItem> toolsBarItem, List<ActionVerb> actions)
        {
            rootToolStrip.SuspendLayout();

            foreach (var item in toolsBarItem)
            {
                if(item.Group.Count > 1)
                {
                    bool toolAvaialble = false;
                    var splitButton = new ToolStripSplitButton();
                    splitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                    splitButton.Image = Image.FromFile(string.Format(@"{0}\Resource\{1}.png", RuntimeInfo.Inst.LaunchEnvExeDir, item.Name));
                    splitButton.Name = string.Format("{0}_toolStripBttn{1}",rootToolStrip.Name,item.Name);
                    splitButton.Text = item.Name;
                    splitButton.Tag = item.Name;
                    splitButton.MouseDown += StripButton_MouseDown;

                    foreach(var splitItem in item.Group)
                    {
                        //if dynamic path tool then don't check the file path
                        if (string.IsNullOrWhiteSpace(splitItem.Path) || File.Exists(splitItem.Path) || splitItem.IsStoreApp )
                        {
                            toolAvaialble = true;
                            var toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                            toolStripMenuItem.Image = Image.FromFile(string.Format(@"{0}\Resource\{1}.png", RuntimeInfo.Inst.LaunchEnvExeDir, splitItem.Name));
                            toolStripMenuItem.Name = string.Format("{0}_toolStripBttn{1}_{2}", rootToolStrip.Name, item.Name, splitItem.Name);
                            toolStripMenuItem.Text = splitItem.Name;
                            toolStripMenuItem.Tag = splitItem.Name;
                            toolStripMenuItem.Click += EditorsToolStripMenuItem_Click;
                            if (RuntimeInfo.Inst.ShowRunAsForAll)
                            {
                                var retMenu = GUIUtility.GenerateActionMenus(toolStripMenuItem.Name, toolStripMenuItem.Tag as string, actions, EditorsToolStripActionMenuItem_Click);
                                if (retMenu.Length > 0)
                                {
                                    toolStripMenuItem.DropDownItems.AddRange(retMenu);
                                }
                            }
                            splitButton.DropDownItems.Add(toolStripMenuItem);
                        }
                    }

                    if(toolAvaialble)
                    {
                        rootToolStrip.Items.Add(splitButton);
                        _toolStripItem.Add(splitButton);
                    }
                }
                else
                {
                    var splitItem = item.Group.FirstOrDefault();
                    if (splitItem != null)
                    {
                        //if dynamic path tool then don't check the file path
                        if (string.IsNullOrWhiteSpace(splitItem.Path) || File.Exists(splitItem.Path) || splitItem.IsStoreApp)
                        {
                            ToolStripItem stripButton = new ToolStripButton(); 
                            stripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                            stripButton.Image = Image.FromFile(string.Format(@"{0}\Resource\{1}.png", RuntimeInfo.Inst.LaunchEnvExeDir, splitItem.Name));
                            stripButton.Name = string.Format("{0}_toolStripBttn{1}", rootToolStrip.Name, splitItem.Name);
                            stripButton.Text = splitItem.Name;
                            stripButton.Tag = splitItem.Name;
                            stripButton.MouseDown += StripButton_MouseDown;
                            rootToolStrip.Items.Add(stripButton);
                        }
                    }
                }
            }
            rootToolStrip.ResumeLayout(false);
            rootToolStrip.PerformLayout();
        }

        private LaunchConfig BuildLaunchConfig()
        {
            LaunchConfig config = new LaunchConfig();
            config.WorkingDir = string.IsNullOrWhiteSpace(RuntimeInfo.Inst.ToolLaunchDir) || (!Directory.Exists(RuntimeInfo.Inst.ToolLaunchDir)) ? Configs_Root.Inst.HomePath : RuntimeInfo.Inst.ToolLaunchDir;
            config.Configs = new List<Config.Config>();

            for (int index = 0; index < envList.Items.Count; index++)
            {
                if (envList.GetItemChecked(index))
                {
                    string envName = envList.Items[index] as string;
                    if (envName != null)
                    {
                        var item = Configs_Root.Inst.Configs.FirstOrDefault((a) => string.Compare(a.Name, envName, true) == 0);
                        if (item != null)
                        {
                            config.Configs.Add(item);
                        }
                    }
                }
            }

            if (config.Configs.Count == 0)
            {
                if (envList.SelectedItem != null)
                {
                    string envName = envList.SelectedItem as string;
                    if (envName != null)
                    {
                        var item = Configs_Root.Inst.Configs.FirstOrDefault((a) => string.Compare(a.Name, envName, true) == 0);
                        if (item != null)
                        {
                            config.Configs.Add(item);
                        }
                    }
                }
            }

            return config;
        }

        private void CleanEnvironment()
        {
            string fileLocation = Assembly.GetExecutingAssembly().Location;
            var allFiles = Directory.GetFiles(Path.GetDirectoryName(fileLocation), "CurrentConfig_*.xml").ToList();
            allFiles.Sort();

            foreach (var item in allFiles)
            {
                Config.LaunchConfig localConfig = null;
                if (File.Exists(item))
                {
                    localConfig = Config.LaunchConfig.LoadCurrentConfig(item);

                    try
                    {
                        File.Delete(item);
                    }
                    catch (Exception)
                    {

                    }
                }

                if (localConfig != null)
                {
                    var genericTool = Editors.EditorFactory.Inst.GetEditor(RuntimeInfo.Generic);
                    genericTool.CleanConfig(localConfig);
                }
            }
        }
    }
}
