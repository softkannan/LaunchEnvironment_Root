using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LaunchEnvironment.Config
{
    public class RegValue
    {
        public RegValue()
        {
            Type = EnvironmentValueType.Path;
            Action = EnvironmentAction.Overwrite;
            Value = "";
        }

        [XmlAttribute]
        public EnvironmentValueType Type { get; set; }
        [XmlAttribute]
        public EnvironmentAction Action { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
