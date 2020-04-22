using LaunchEnvironment.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LaunchEnvironment.Editors
{
    public class CodeLiteEditor : EditorDefault
    {
        public CodeLiteEditor() : base()
        {
        }

        protected override bool LaunchCustom(LaunchConfig config)
        {
            string openfileName = "";
            if (Directory.Exists(RuntimeInfo.Inst.OpenFolder))
            {
                if (string.IsNullOrWhiteSpace(openfileName))
                {
                    foreach (var item in Directory.GetFiles(config.WorkingDir, "*.workspace"))
                    {
                        openfileName = item;
                        break;
                    }
                }

                if (string.IsNullOrWhiteSpace(openfileName))
                {
                    foreach (var item in Directory.GetFiles(config.WorkingDir, "*.c*"))
                    {
                        openfileName = "\"" + item + "\"";
                        break;
                    }
                }
            }

            string toolDir = string.Format("--basedir=\"{0}\"", Path.GetDirectoryName(Tool.Path));
            string userDataDir = string.Format(@"{0}\userdata", Tool.ToolPath);
            if (Directory.Exists(userDataDir))
            {
                string currentDynamicArg = toolDir;
                DynamicArgument = string.Format("{0}{1}--datadir=\"{2}\"", toolDir, string.IsNullOrWhiteSpace(toolDir) ? "" : " ", userDataDir);
            }
            else
            {
                DynamicArgument = toolDir;
            }

            if(!string.IsNullOrWhiteSpace(openfileName))
            {
                string currentDynamicArg = DynamicArgument;
                DynamicArgument = string.Format("{0} {1}", currentDynamicArg, openfileName);
            }

            return true;
        }
    }
}
