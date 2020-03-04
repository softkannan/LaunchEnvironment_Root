using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessEx
{
    public interface IProcessEx
    {
        Process Launch(ProcessStartInfo startInfo);
    }
}
