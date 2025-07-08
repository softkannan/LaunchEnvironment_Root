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
using LaunchEnvironment.Config.EnvConfig;

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

            Config.EnvConfig.EnvConfigs.LoadConfig();

            UserConfig.Inst.Process();

            string workingDir = "";
            if (Environment.GetCommandLineArgs().Length > 1)
            {
                workingDir = Environment.GetCommandLineArgs()[1];
            }

            UserConfig.Inst.OpenFolder = null;
            if (Directory.Exists(workingDir))
            {
                UserConfig.Inst.OpenFolder = ResolveValue.Inst.ResolveEnvironmentValue(EnvironmentValueType.Path, workingDir.Trim('\"')); 
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
            if (EnvConfigs.Inst == null)
            {
                return;
            }

            int multiplier = _configListBox.Height;
            int formMaxHeight = this.MaximumSize.Height;
            int currentHeight = this.Height;

            //envList.Items.Add("System Default", false);

            foreach (var itemEnv in EnvConfigs.Inst.Configs)
            {
                _configListBox.Items.Add(itemEnv.Name, false);
            }

            if (_configListBox.Items.Count > 0)
            {
                _configListBox.SelectedIndex = 0;
            }

            var newHeight = currentHeight + (EnvConfigs.Inst.Configs.Count * multiplier);
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
            if (!UserConfig.Inst.IsElevated)
            {
                actions.Add(new ActionVerb() { Name = "RunAs Admin", Verb = "runas" });
            }
            actions.Add(new ActionVerb() { Name = "UpdateFiles", Verb = "updatefiles" });

            var runAsContextMenuItems = GUIUtility.GenerateContextActionMenus(_runAsContext.Name, "ContextMenu", actions, ToolButtonOrContextMenuItem_Click);
            _runAsContext.Items.AddRange(runAsContextMenuItems);

            //Build main tool bar
            if (UserConfig.Inst.ToolBar != null && UserConfig.Inst.ToolBar.Count > 0)
            {
                GenerateToolbar(_mainToolBar, UserConfig.Inst.ToolBar, actions);
            }

            //Build main menu bar Tools, Editors, Integration
            UserConfig.Inst.UpdateMenuBar(_mainMenu, actions, ToolButtonOrContextMenuItem_Click);

            //Build config context menu
            if (UserConfig.Inst.ContextMenu != null && UserConfig.Inst.ContextMenu.Count > 0)
            {
                GenerateContextMenu(_configListContextMenu, UserConfig.Inst.ContextMenu);
            }
        }

        private void GenerateContextMenu(ContextMenuStrip rootMenu, List<string> menuItems)
        {
            foreach(var item in menuItems)
            {
                if (UserConfig.Inst.IsToolAvailable(item))
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
                    var imgFile = string.Format(@"{0}\Resource\img\{1}.png", UserConfig.Inst.LaunchEnvExeDir, item.Name);
                    if (!File.Exists(imgFile))
                    {
                        imgFile = string.Format(@"{0}\Resource\img\App.png", UserConfig.Inst.LaunchEnvExeDir);
                    }
                    splitButton.Image = Image.FromFile(imgFile);
                    splitButton.Name = string.Format("{0}_toolStripBttn{1}",rootToolStrip.Name,item.Name);
                    splitButton.Text = item.Name;
                    splitButton.Tag = item.Name;
                    splitButton.MouseDown += StripButton_MouseDown;

                    foreach(var splitItem in item.Tools)
                    {
                        if (UserConfig.Inst.IsToolAvailable(splitItem))
                        {
                            var tool = UserConfig.Inst.GetTool(splitItem);
                            toolAvaialble = true;
                            var toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                            imgFile = string.Format(@"{0}\Resource\img\{1}.png", UserConfig.Inst.LaunchEnvExeDir, tool.Name);
                            if(!File.Exists(imgFile))
                            {
                                imgFile = string.Format(@"{0}\Resource\img\App.png", UserConfig.Inst.LaunchEnvExeDir);
                            }
                            toolStripMenuItem.Image = Image.FromFile(imgFile);
                            toolStripMenuItem.Name = string.Format("{0}_toolStripBttn{1}_{2}", rootToolStrip.Name, item.Name, tool.Name);
                            toolStripMenuItem.Text = tool.Name;
                            toolStripMenuItem.Tag = tool.Name;
                            toolStripMenuItem.Click += ToolButtonOrContextMenuItem_Click;
                            if (UserConfig.Inst.ShowRunAsForAll)
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
                        if (UserConfig.Inst.IsToolAvailable(splitItem))
                        {
                            var tool = UserConfig.Inst.GetTool(splitItem);
                            ToolStripItem stripButton = new ToolStripButton(); 
                            stripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                            var imgFile = string.Format(@"{0}\Resource\img\{1}.png", UserConfig.Inst.LaunchEnvExeDir, tool.Name);
                            if (!File.Exists(imgFile))
                            {
                                imgFile = string.Format(@"{0}\Resource\img\App.png", UserConfig.Inst.LaunchEnvExeDir);
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
            config.WorkingDir = UserConfig.Inst.OpenFolder;
            config.Configs = new List<Config.EnvConfig.Config>();

            for (int index = 0; index < _configListBox.Items.Count; index++)
            {
                if (_configListBox.GetItemChecked(index))
                {
                    string envName = _configListBox.Items[index] as string;
                    if (envName != null)
                    {
                        var item = EnvConfigs.Inst.Configs.FirstOrDefault((a) => string.Compare(a.Name, envName, true) == 0);
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
                        var item = EnvConfigs.Inst.Configs.FirstOrDefault((a) => string.Compare(a.Name, envName, true) == 0);
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
                Config.EnvConfig.LaunchConfig localConfig = null;
                if (File.Exists(item))
                {
                    localConfig = Config.EnvConfig.LaunchConfig.LoadCurrentConfig(item);

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
                    var genericTool = Editors.EditorFactory.Inst.GetEditor(UserConfig.Generic);
                    genericTool.CleanConfig(localConfig);
                }
            }
        }
    }
}
