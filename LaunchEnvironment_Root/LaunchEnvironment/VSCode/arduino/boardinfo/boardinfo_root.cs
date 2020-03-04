using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchEnvironment.VSCode.arduino.boardinfo
{
    public class boardinfo_root
    {
        public string name { get; set; }
        public string maintainer { get; set; }
        public string websiteURL { get; set; }
        public string email { get; set; }
        public help_info help { get; set; }
        public List<platform> platforms { get; set; }
        public List<tool_info> tools { get; set; }
    }
}
