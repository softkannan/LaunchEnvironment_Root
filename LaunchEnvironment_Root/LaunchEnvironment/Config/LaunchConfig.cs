using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LaunchEnvironment.Config
{
    public class LaunchConfig
    {
        public LaunchConfig()
        {
            WorkingDir = "";
            EditorPath = "";
            Arguments = "";
            Verb = "";
        }
        public List<Config> Configs { get; set; }

        public string WorkingDir { get; set; }

        public string EditorPath { get; set; }

        public string Arguments { get; set; }

        public string Verb { get; set; }
        
        public static void SaveCurrentConfig(LaunchConfig config)
        {
            string currentConfigFile = RuntimeInfo.Inst.CurrentConfigFile;
            if (File.Exists(currentConfigFile))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(LaunchConfig));
                // A FileStream is needed to read the XML document.
                using (FileStream fs = new FileStream(currentConfigFile, FileMode.Open))
                {
                    serializer.Serialize(fs, config);
                }
            }
        }

        public static LaunchConfig LoadLastConfig()
        {
            string fileLocation = Assembly.GetExecutingAssembly().Location;
            var allFiles = Directory.GetFiles(Path.GetDirectoryName(fileLocation), "CurrentConfig_*.xml").ToList();
            allFiles.Sort();
            string lastConfigfile = allFiles.FirstOrDefault();
            if (File.Exists(lastConfigfile))
            {
                return LoadCurrentConfig(lastConfigfile);
            }
            return null;
        }

        public static LaunchConfig LoadCurrentConfig(string configFile)
        {
            LaunchConfig retVal = null;
            if (File.Exists(configFile))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(LaunchConfig));
                // A FileStream is needed to read the XML document.
                using (var fs = new StreamReader(configFile))
                {
                    retVal = (LaunchConfig)serializer.Deserialize(fs);
                }
            }
            return retVal;
        }
    }
}
