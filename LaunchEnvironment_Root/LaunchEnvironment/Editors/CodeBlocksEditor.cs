using LaunchEnvironment.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LaunchEnvironment.Editors
{
    public class CodeBlocksEditor : EditorDefault
    {
        public CodeBlocksEditor() : base()
        {
            
        }
        /// <summary>
        /// <filename>	Specifies the project *.cbp filename or workspace *.workspace filename. 
        /// For instance <filename> may be c:\some\where\a\project.cbp. Place this argument at end of command line, just before output redirection if any.
        /// --file=<filename>[:line]	Open file in Code::Blocks and optionally jump to a specific line.
        /// </summary>
        /// <param name="config"></param>
        protected override bool LaunchCustom(LaunchConfig config)
        {
            if (Directory.Exists(RuntimeInfo.Inst.OpenFolder))
            {
                string openfile = "";

                foreach (var item in Directory.GetFiles(config.WorkingDir, "*.cbp"))
                {
                    openfile = item;
                    break;
                }

                if (string.IsNullOrWhiteSpace(openfile))
                {
                    foreach (var item in Directory.GetFiles(config.WorkingDir, "*.workspace"))
                    {
                        openfile = item;
                        break;
                    }
                }

                if (string.IsNullOrWhiteSpace(openfile))
                {
                    foreach (var item in Directory.GetFiles(config.WorkingDir, "*.c*"))
                    {
                        DynamicArgument = "--file=\"" + item + "\"";
                        break;
                    }
                }
                else
                {
                    DynamicArgument = "\"" + openfile + "\"";
                }
            }

            string userData = string.Format(@"{0}\userdata", Tool.ToolDir);

            if (Directory.Exists(userData))
            {
                string currentDynamicArg = DynamicArgument;

                DynamicArgument = string.Format("{0}{1}--user-data-dir=\"{2}\"", currentDynamicArg, string.IsNullOrWhiteSpace(currentDynamicArg) ? "" : " ", userData);
            }

            return true;
        }
    }
}
