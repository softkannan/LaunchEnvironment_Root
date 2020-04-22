using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LaunchEnvironment.Config
{
    public class ToolBarItem
    {
        public string Name { get; set; }

        [XmlArray("Tools")]
        [XmlArrayItem(ElementName = "Name")]
        public List<string> Tools { get; set; }
    }
}
