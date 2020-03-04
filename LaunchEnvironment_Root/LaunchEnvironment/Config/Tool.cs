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
            Arguments = "";
            Envs = null;
            BaseDir = "";
            IsStoreApp = false;
            Script = new List<string>();
        }

        public bool IsStoreApp { get; set; }
        public string BaseDir { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public string Editor { get; set; }
        public bool ByPassRegistry { get; set; }
        public bool UseShellExecute { get; set; }
        public string Arguments { get; set; }

        [XmlArray("Script")]
        [XmlArrayItem(ElementName = "Cmd")]
        public List<string> Script { get; set; }

        public List<EnviromentVariable> Envs { get; set; }
    }
}
