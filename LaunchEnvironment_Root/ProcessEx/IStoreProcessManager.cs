using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessEx
{
    public interface IStoreProcessManager
    {
        bool IsAppExists(string appName);
    }
}
