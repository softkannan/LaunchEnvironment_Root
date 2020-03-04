using LaunchEnvironment.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchEnvironment.Editors
{
    class WingIDEEditor : EditorDefault
    {
        public WingIDEEditor() : base(RuntimeInfo.WingIDE)
        {
        }
        
        public override void Launch(LaunchConfig config)
        {
            if (RuntimeInfo.Inst.IsOpenFolder)
            {
                string pyFileName = "";

                foreach (var item in Directory.GetFiles(config.WorkingDir,"*.py"))
                {
                    pyFileName = item;
                    break;
                }
                DynamicArgument = "\"" + pyFileName + "\"";
            }

            base.Launch(config);
        }
    }
}
