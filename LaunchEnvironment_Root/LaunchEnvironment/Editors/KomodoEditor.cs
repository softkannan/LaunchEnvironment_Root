using LaunchEnvironment.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchEnvironment.Editors
{
    public class KomodoEditor : EditorDefault
    {

        public KomodoEditor() : base(RuntimeInfo.Komodo)
        {

        }

        public override void Launch(LaunchConfig config)
        {
            if (RuntimeInfo.Inst.IsOpenFolder)
            {
                DynamicArgument = "\"" + RuntimeInfo.Inst.ToolLaunchDir + "\"";
            }
            
            base.Launch(config);
        }
    }
}
