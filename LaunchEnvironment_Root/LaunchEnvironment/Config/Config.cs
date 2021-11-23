using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LaunchEnvironment.Config
{
    public class Config
    {
        public Config()
        {
            Id = "generic";
            Name = "Unknown";
            Arguments = null;
            RegConfigs = null;
            Envs = null;
            CopyFiles = null;
            PreReqBatchFileCmds = null;
        }
        public string Name { get; set; }
        public string Id { get; set; }
        public string ConfigPath { get; set; }
        public string DefaultWorkspace { get; set; }
        public List<FileCopy> CopyFiles { get; set; }
        /// <summary>
        /// If this value present then the tool will use this value, this takes precedence
        /// </summary>
        [XmlArray("Arguments")]
        [XmlArrayItem(ElementName = "Arg")]
        public List<string> Arguments { get; set; }
        public List<RegKey> RegConfigs { get; set; }
        public List<EnviromentVariable> Envs { get; set; }

        /// <summary>
        /// If this value present then the tool will use this value, this takes precedence
        /// </summary>
        [XmlArray("PreBatchFile")]
        [XmlArrayItem(ElementName = "PreBatchFileCmd")]
        public List<string> PreReqBatchFileCmds { get; set; }
    }
}
