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
            ByPassRegistry = false;
            UseShellExecute = false;
            Arguments = null;
            Envs = null;
            ToolPath = "";
            Path = "";
            Type = ToolType.RegularApp;
            Script = new List<string>();
        }


        [XmlAttribute]
        public ToolType Type { get; set; }

        [XmlAttribute]
        public string Editor { get; set; }

        [XmlAttribute]
        public bool ByPassRegistry { get; set; }

        [XmlAttribute]
        public bool UseShellExecute { get; set; }

        public string ToolPath { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }

        [XmlArray("Arguments")]
        [XmlArrayItem(ElementName = "Arg")]
        public List<string> Arguments { get; set; }

        [XmlArray("Script")]
        [XmlArrayItem(ElementName = "Cmd")]
        public List<string> Script { get; set; }

        public List<EnviromentVariable> Envs { get; set; }
    }
}
