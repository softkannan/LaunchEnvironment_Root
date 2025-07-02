using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LaunchEnvironment.Config
{
    public class Tool
    {
        public Tool()
        {
            Args = new List<string>();
            Envs = new List<EnviromentVariable>();
            ToolDir = string.Empty;
            Path = string.Empty;
            Type = ToolType.RegularApp;
            Script = new List<string>();
            Style = new LaunchStyle();
            Warnings = new List<string>();
        }


        [XmlAttribute]
        public ToolType Type { get; set; }

        [XmlAttribute]
        public string Editor { get; set; }

        /// <summary>
        /// If the style is set then the tool will use this value, this takes precedence
        /// </summary>
        public LaunchStyle Style { get; set; }

        public string ToolDir { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }

        [XmlArray("Args")]
        [XmlArrayItem(ElementName = "Arg")]
        public List<string> Args { get; set; }

        [XmlArray("Script")]
        [XmlArrayItem(ElementName = "Cmd")]
        public List<string> Script { get; set; }

        public List<EnviromentVariable> Envs { get; set; }

        [XmlArray("Warnings")]
        [XmlArrayItem(ElementName = "Warning")]
        public List<string> Warnings { get; set; } 
    }
}
