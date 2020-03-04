using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchEnvironment.Config
{
    public class Config
    {
        public Config()
        {
            Type = ConfigType.app;
            Name = "Unknown";
            Arguments = "";
            RegConfigs = null;
            Envs = null;
        }
        public string Name { get; set; }
        public ConfigType Type { get; set; }
        public string InstallPath { get; set; }
        /// <summary>
        /// If this value present then the tool will use this value, this takes precedence
        /// </summary>
        public string Arguments { get; set; }
        public List<RegKey> RegConfigs { get; set; }
        public List<EnviromentVariable> Envs { get; set; }
    }
}
