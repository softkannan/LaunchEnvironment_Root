using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LaunchEnvironment.Config
{
    public class EnviromentVariable
    {
        public EnviromentVariable()
        {
            Action = EnvironmentAction.Prefix;
            Type = EnvironmentValueType.Path;
            Value = "";
        }

        [XmlAttribute]
        public EnvironmentAction Action { get; set; }
        [XmlAttribute]
        public EnvironmentValueType Type { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
