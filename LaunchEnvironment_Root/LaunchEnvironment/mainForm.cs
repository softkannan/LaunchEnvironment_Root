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

            environmentLabel.Text = string.Format("Select Environment for : {0}", RuntimeInfo.Inst.ToolLaunchDir);

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
            if (envList.SelectedItem != null)
            {
                var envName = envList.SelectedItem as string;
                var envItem = Configs_Root.Inst.Configs.FirstOrDefault((item) => item.Name == envName);
                if (envItem != null)
                {
                    if (Directory.Exists(ResolveValue.Inst.ResolveFullPath(envItem.InstallPath)))
                    {
                        Process.Start(string.Format(@"{0}\explorer.exe", Environment.GetFolderPath(Environment.SpecialFolder.Windows)), string.Format("\"{0}\"", ResolveValue.Inst.ResolveFullPath(envItem.InstallPath)));
                    }
                    else
                    {
                        ErrorLog.Inst.LogError("Unable to locate folder : {0}", ResolveValue.Inst.ResolveFullPath(envItem.InstallPath));
                    }
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm form = new AboutForm();

            form.ShowDialog();
        }

        private void EditorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var menuItem = sender as ToolStripItem;

            if(menuItem != null)
            {
                var launcher = Editors.EditorFactory.Inst.GetEditor(menuItem.Tag as string);

                var config = BuildLaunchConfig();

                launcher.Launch(config);
            }
        }

        private void EditorsToolStripActionMenuItem_Click(object sender, EventArgs e)
        {
            var menuItem = sender as ToolStripItem;

            if (menuItem != null)
            {
                var toolNameAndVerb = (menuItem.Tag as string).Split(';');
                string toolName = "";
                string verb = "";

                if(toolNameAndVerb.Length > 0)
                {
                    toolName = toolNameAndVerb[0];
                }

                if(toolName == "ContextMenu")
                {
                    toolName = runAsContext.Tag as string;
                }

                if(toolNameAndVerb.Length > 1)
                {
                    verb = toolNameAndVerb[1];
                }

                var launcher = Editors.EditorFactory.Inst.GetEditor(toolName);

                var config = BuildLaunchConfig();

                config.Verb = verb;

                launcher.Launch(config);
            }
        }
        private void StripButton_MouseDown(object sender, MouseEventArgs e)
        {
            var stripItem = sender as ToolStripItem;
            if (e.Button == MouseButtons.Right)
            {
                int x = 0;
                foreach(ToolStripItem item in mainToolBar.Items)
                {
                    x += item.Width;
                    if(item.Tag == stripItem.Tag)
                    {
                        x -= item.Width;
                        break;
                    }
                }
                runAsContext.Tag = stripItem.Tag;
                runAsContext.Show(mainToolBar, new Point(x, stripItem.Height),ToolStripDropDownDirection.BelowRight);
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
                    var launcher = Editors.EditorFactory.Inst.GetEditor(stripItem.Tag as string);

                    var config = BuildLaunchConfig();

                    launcher.Launch(config);
                }
            }
        }

        #endregion
    }
}
