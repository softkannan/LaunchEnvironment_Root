using LaunchEnvironment.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchEnvironment.Editors
{
    public class OpenSSHEditor : EditorDefault
    {
        public OpenSSHEditor() : base(RuntimeInfo.OpenSSH)
        {

        }
    }
}
