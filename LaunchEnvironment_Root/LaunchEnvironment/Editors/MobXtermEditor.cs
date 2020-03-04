using LaunchEnvironment.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchEnvironment.Editors
{
    public class MobXtermEditor : EditorDefault
    {
        public MobXtermEditor() : base(RuntimeInfo.MobXTerm)
        {

        }
        public override void Launch(LaunchConfig config)
        {
            if (RuntimeInfo.Inst.IsOpenFolder)
            {
                DynamicArgument = "/openfolder \"" + RuntimeInfo.Inst.ToolLaunchDir + "\"";
            }

            base.Launch(config);
        }
    }
}
