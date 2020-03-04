using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchEnvironment.Config
{
    public enum EnvironmentAction
    {
        //Append value to existing environment value
        Append,
        //ovrwrite the existing environment value
        Overwrite,
        //prefix value to existing environment value
        Prefix
    }
}
