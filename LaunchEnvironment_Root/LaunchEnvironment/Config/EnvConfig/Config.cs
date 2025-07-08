using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LaunchEnvironment.Config.EnvConfig
{
    public class Config
    {
        public Config()
        {
            Id = "generic";
            Name = "Unknown";
            Args = null;
            RegConfigs = null;
            Envs = null;
            CopyFiles = null;
            Script = null;
            Style = null;
        }
        public string Name { get; set; }
        public string Id { get; set; }
        public string ConfigPath { get; set; }
        public string DefaultWorkspace { get; set; }
        /// <summary>
        /// Before launching the tool, copy files from the source to the destination
        /// </summary>
        public List<FileCopy> CopyFiles { get; set; }
        /// <summary>
        /// If this value present then the tool will use this value, this takes precedence
        /// </summary>
        [XmlArray("Arguments")]
        [XmlArrayItem(ElementName = "Arg")]
        public List<string> Args { get; set; }
        /// <summary>
        /// Add registry keys to the system
        /// </summary>
        public List<RegKey> RegConfigs { get; set; }
        /// <summary>
        /// Sets the environment variables for the given process
        /// </summary>
        public List<EnviromentVariable> Envs { get; set; }

        /// <summary>
        /// If this value present then the tool will use this value, this takes precedence
        /// This will force the editor to create the batch file and launch tool from the batch file
        /// 
        /// Prefixing the command line $ indicates this templated string
        /// 
        /// </summary>
        [XmlArray("BatchFile")]
        [XmlArrayItem(ElementName = "BatchCmd")]
        public List<string> Script { get; set; }

        /// <summary>
        /// If the style is set then the tool will use this value, this takes precedence
        /// </summary>
        public LaunchStyle Style { get; set; }
    }
}
