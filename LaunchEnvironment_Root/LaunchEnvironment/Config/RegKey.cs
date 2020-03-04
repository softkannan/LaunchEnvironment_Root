using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LaunchEnvironment.Config
{
    public class RegKey
    {
        public RegKey()
        {
            RequireAdmin = false;
            Type = EnvironmentValueType.String;
            Action = EnvironmentAction.Overwrite;
            Value = "";
        }
        public string Key { get; set; }
        public string Value { get; set; }

        [XmlAttribute]
        public bool RequireAdmin { get; set; }
        [XmlAttribute]
        public EnvironmentValueType Type { get; set; }
        [XmlAttribute]
        public EnvironmentAction Action { get; set; }

        public List<RegValue> RegValues { get; set; }
    }
}
