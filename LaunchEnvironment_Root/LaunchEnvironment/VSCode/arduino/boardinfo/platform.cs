using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchEnvironment.VSCode.arduino.boardinfo
{
    public class platform
    {
        public string name { get; set; }
        public string architecture { get; set; }
        public string version { get; set; }
        public string category { get; set; }
        public help_info help { get; set; }
        public string url { get; set; }
        public string archiveFileName { get; set; }
        public string checksum { get; set; }
        public string size { get; set; }
        public List<board_info> boards { get; set; }
        public List<tool_dependancy> toolsDependencies { get; set; }
    }
}
