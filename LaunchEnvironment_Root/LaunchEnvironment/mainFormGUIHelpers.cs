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
            this._configListContextMenu.SuspendLayout();
            this._mainMenu.SuspendLayout();
            this.SuspendLayout();

            Config.Configs_Root.LoadEnvironments();

            RuntimeInfo.Inst.ProcessRuntimeInfo();

            string workingDir = "";
            if (Environment.GetCommandLineArgs().Length > 1)
            {
                workingDir = Environment.GetCommandLineArgs()[1];
            }

            RuntimeInfo.Inst.OpenFolder = null;
            if (Directory.Exists(workingDir))
            {
                RuntimeInfo.Inst.OpenFolder = ResolveValue.Inst.ResolveEnvironmentValue(EnvironmentValueType.Path, workingDir.Trim('\"')); 
            }

            BuildControls();

            this._configListContextMenu.ResumeLayout(false);
            this._mainMenu.ResumeLayout(false);
            this._mainMenu.PerformLayout();
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

            int multiplier = _configListBox.Height;
            int formMaxHeight = this.MaximumSize.Height;
            int currentHeight = this.Height;

            //envList.Items.Add("System Default", false);

            foreach (var itemEnv in Configs_Root.Inst.Configs)
            {
                _configListBox.Items.Add(itemEnv.Name, false);
            }

            if (_configListBox.Items.Count > 0)
            {
                _configListBox.SelectedIndex = 0;
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
            // Build RunAs context menu
            List<ActionVerb> actions = new List<ActionVerb>();

            actions.Add(new ActionVerb() { Name = "Run", Verb = "" });
            if (!RuntimeInfo.Inst.IsElevated)
            {
                actions.Add(new ActionVerb() { Name = "RunAs Admin", Verb = "runas" });
            }
            actions.Add(new ActionVerb() { Name = "UpdateFiles", Verb = "updatefiles" });

            var runAsContextMenuItems = GUIUtility.GenerateContextActionMenus(_runAsContext.Name, "ContextMenu", actions, ToolButtonOrContextMenuItem_Click);
            _runAsContext.Items.AddRange(runAsContextMenuItems);

            //Build main tool bar
            if (RuntimeInfo.Inst.ToolBar != null && RuntimeInfo.Inst.ToolBar.Count > 0)
            {
                GenerateToolbar(_mainToolBar, RuntimeInfo.Inst.ToolBar, actions);
            }

            //Build main menu bar Tools, Editors, Integration
            RuntimeInfo.Inst.UpdateMenuBar(_mainMenu, actions, ToolButtonOrContextMenuItem_Click);

            //Build config context menu
            if (RuntimeInfo.Inst.ContextMenu != null && RuntimeInfo.Inst.ContextMenu.Count > 0)
            {
                GenerateContextMenu(_configListContextMenu, RuntimeInfo.Inst.ContextMenu);
            }
        }

        private void GenerateContextMenu(ContextMenuStrip rootMenu, List<string> menuItems)
        {
            foreach(var item in menuItems)
            {
                if (RuntimeInfo.Inst.IsToolAvailable(item))
                {
                    var toolStripMenuItem = new ToolStripMenuItem();
                    toolStripMenuItem.Name = string.Format("{0}_{1}", rootMenu.Name, item);
                    toolStripMenuItem.Text = item;
                    toolStripMenuItem.Tag = item;
                    toolStripMenuItem.Click += ToolButtonOrContextMenuItem_Click;
                    rootMenu.Items.Add(toolStripMenuItem);
                }
            }
        }

        private void GenerateToolbar(ToolStrip rootToolStrip,List<ToolBarItem> toolsBarItem, List<ActionVerb> actions)
        {
            rootToolStrip.SuspendLayout();

            foreach (var item in toolsBarItem)
            {
                if(item.Tools != null && item.Tools.Count > 1)
                {
                    bool toolAvaialble = false;
                    var splitButton = new ToolStripSplitButton();
                    splitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                    var imgFile = string.Format(@"{0}\Resource\img\{1}.png", RuntimeInfo.Inst.LaunchEnvExeDir, item.Name);
                    if (!File.Exists(imgFile))
                    {
                        imgFile = string.Format(@"{0}\Resource\img\App.png", RuntimeInfo.Inst.LaunchEnvExeDir);
                    }
                    splitButton.Image = Image.FromFile(imgFile);
                    splitButton.Name = string.Format("{0}_toolStripBttn{1}",rootToolStrip.Name,item.Name);
                    splitButton.Text = item.Name;
                    splitButton.Tag = item.Name;
                    splitButton.MouseDown += StripButton_MouseDown;

                    foreach(var splitItem in item.Tools)
                    {
                        if (RuntimeInfo.Inst.IsToolAvailable(splitItem))
                        {
                            var tool = RuntimeInfo.Inst.GetTool(splitItem);
                            toolAvaialble = true;
                            var toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                            imgFile = string.Format(@"{0}\Resource\img\{1}.png", RuntimeInfo.Inst.LaunchEnvExeDir, tool.Name);
                            if(!File.Exists(imgFile))
                            {
                                imgFile = string.Format(@"{0}\Resource\img\App.png", RuntimeInfo.Inst.LaunchEnvExeDir);
                            }
                            toolStripMenuItem.Image = Image.FromFile(imgFile);
                            toolStripMenuItem.Name = string.Format("{0}_toolStripBttn{1}_{2}", rootToolStrip.Name, item.Name, tool.Name);
                            toolStripMenuItem.Text = tool.Name;
                            toolStripMenuItem.Tag = tool.Name;
                            toolStripMenuItem.Click += ToolButtonOrContextMenuItem_Click;
                            if (RuntimeInfo.Inst.ShowRunAsForAll)
                            {
                                var retMenu = GUIUtility.GenerateContextActionMenus(toolStripMenuItem.Name, toolStripMenuItem.Tag as string, actions, ToolButtonOrContextMenuItem_Click);
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
                    var splitItem = item.Tools != null && item.Tools.Count > 0 ? item.Tools.FirstOrDefault() : item.Name;
                    if (splitItem != null)
                    {
                        if (RuntimeInfo.Inst.IsToolAvailable(splitItem))
                        {
                            var tool = RuntimeInfo.Inst.GetTool(splitItem);
                            ToolStripItem stripButton = new ToolStripButton(); 
                            stripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                            var imgFile = string.Format(@"{0}\Resource\img\{1}.png", RuntimeInfo.Inst.LaunchEnvExeDir, tool.Name);
                            if (!File.Exists(imgFile))
                            {
                                imgFile = string.Format(@"{0}\Resource\img\App.png", RuntimeInfo.Inst.LaunchEnvExeDir);
                            }
                            stripButton.Image = Image.FromFile(imgFile);
                            stripButton.Name = string.Format("{0}_toolStripBttn{1}", rootToolStrip.Name, tool.Name);
                            stripButton.Text = tool.Name;
                            stripButton.Tag = tool.Name;
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
            config.WorkingDir = RuntimeInfo.Inst.OpenFolder;
            config.Configs = new List<Config.Config>();

            for (int index = 0; index < _configListBox.Items.Count; index++)
            {
                if (_configListBox.GetItemChecked(index))
                {
                    string envName = _configListBox.Items[index] as string;
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
                if (_configListBox.SelectedItem != null)
                {
                    string envName = _configListBox.SelectedItem as string;
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
