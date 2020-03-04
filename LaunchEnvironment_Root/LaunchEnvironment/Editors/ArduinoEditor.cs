using LaunchEnvironment.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LaunchEnvironment.Editors
{
    public class ArduinoEditor : EditorDefault
    {
        public ArduinoEditor() : base(RuntimeInfo.Arduino)
        {
        }
        public override void Launch(LaunchConfig config)
        {
            if (RuntimeInfo.Inst.IsOpenFolder)
            {
                foreach (var item in Directory.GetFiles(config.WorkingDir, "*.ino"))
                {
                    DynamicArgument = "\"" + item + "\"";
                    break;
                }
            }

            base.Launch(config);
        }
    }
}
