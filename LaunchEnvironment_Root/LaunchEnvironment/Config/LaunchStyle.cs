using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LaunchEnvironment.Config
{
    public class LaunchStyle
    {
        public LaunchStyle() 
        {
            UseShellExecute = false;
            ExecuteAsAdmin = false;
            CreateNoWindow = false;
            WindowStyle = ProcessWindowStyle.Normal;
        }

        [XmlAttribute]
        public bool UseShellExecute { get; set; }

        [XmlAttribute]
        public bool ExecuteAsAdmin { get; set; }

        [XmlAttribute]
        public bool CreateNoWindow { get; set; }

        [XmlAttribute]
        public ProcessWindowStyle WindowStyle { get; set; }
    }
}
