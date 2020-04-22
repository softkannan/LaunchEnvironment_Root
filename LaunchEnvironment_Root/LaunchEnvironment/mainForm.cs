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

namespace LaunchEnvironment
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
            InitializeCustom();
        }

        #region GUI Launch Handlers

        private void mainForm_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();

            _environmentLabel.Text = string.Format("Select Environment for : {0}", RuntimeInfo.Inst.OpenFolder);

            if (RuntimeInfo.Inst.IsElevated)
            {
                this.Text = "Launch Environment (administrator)";
            }
            else
            {
                this.Text = "Launch Environment";
            }
        }

        private void openToolFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_configListBox.SelectedItem != null)
            {
                var envName = _configListBox.SelectedItem as string;
                var envItem = Configs_Root.Inst.Configs.FirstOrDefault((item) => item.Name == envName);
                if (envItem != null)
                {
                    if (Directory.Exists(ResolveValue.Inst.ResolveFullPath(envItem.ConfigPath)))
                    {
                        Process.Start(string.Format(@"{0}\explorer.exe", Environment.GetFolderPath(Environment.SpecialFolder.Windows)), string.Format("\"{0}\"", ResolveValue.Inst.ResolveFullPath(envItem.ConfigPath)));
                    }
                    else
                    {
                        ErrorLog.Inst.LogError("Unable to locate folder : {0}", ResolveValue.Inst.ResolveFullPath(envItem.ConfigPath));
                    }
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm form = new AboutForm();

            form.ShowDialog();
        }

        private void ToolButtonOrContextMenuItem_Click(object sender, EventArgs e)
        {
            var menuItem = sender as ToolStripItem;

            if (menuItem != null)
            {
                // split tag value 
                var toolNameAndVerb = (menuItem.Tag as string).Split(';');
                string toolName = "";
                string verb = "";

                if(toolNameAndVerb.Length > 0)
                {
                    toolName = toolNameAndVerb[0];
                }

                //Check click comes from RunAs context menu item 
                if(toolName == "ContextMenu")
                {
                    //if runs as context menu is clicked then user selected button infromation is stored in root node of the context menu
                    //the tag value contains the user selected tool name and update the tool name value
                    toolName = _runAsContext.Tag as string;
                }

                //if menu item contains verb then update verb part
                if(toolNameAndVerb.Length > 1)
                {
                    verb = toolNameAndVerb[1];
                }

                LaunchTool(toolName, verb);
            }
        }
        private void StripButton_MouseDown(object sender, MouseEventArgs e)
        {
            var stripItem = sender as ToolStripItem;
            if (e.Button == MouseButtons.Right)
            {
                int x = 0;
                foreach(ToolStripItem item in _mainToolBar.Items)
                {
                    x += item.Width;
                    if(item.Tag == stripItem.Tag)
                    {
                        x -= item.Width;
                        break;
                    }
                }
                _runAsContext.Tag = stripItem.Tag;
                _runAsContext.Show(_mainToolBar, new Point(x, stripItem.Height),ToolStripDropDownDirection.BelowRight);
            }
            else if(e.Button == MouseButtons.Left)
            {
                var splitButton = sender as ToolStripSplitButton;
                if (stripItem != null)
                {
                    if(splitButton != null && (!splitButton.ButtonBounds.Contains(e.Location)))
                    {
                        return;
                    }

                    LaunchTool(stripItem.Tag as string);
                }
            }
        }

        void LaunchTool(string toolName, string verb = "")
        {
            var editorObj = Editors.EditorFactory.Inst.GetEditor(toolName);

            if(editorObj == null)
            {
                ErrorLog.Inst.ShowError("Unable to find editor for given tool : {0} , verify tools.xml", toolName);
                return;
            }

            var config = BuildLaunchConfig();

            if (!string.IsNullOrWhiteSpace(verb))
            {
                config.Verb = verb;
            }

            editorObj.Launch(config);
        }

        #endregion
    }
}
