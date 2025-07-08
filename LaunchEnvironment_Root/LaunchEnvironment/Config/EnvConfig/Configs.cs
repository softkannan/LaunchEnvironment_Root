using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LaunchEnvironment.Config.EnvConfig
{
    public class EnvConfigs
    {
        public List<Config> Configs { get; set; }

        public static void LoadConfig()
        {
            LoadConfigJson();
        }

        private static void LoadConfigJson()
        {
            string fileLocation = Assembly.GetExecutingAssembly().Location;
            string envFile = string.Format("{0}\\Config\\Environments.json", Path.GetDirectoryName(fileLocation));
            string filePath = envFile;
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "LaunchEnvironment.Resource.Config.Environments.json";

            // A FileStream is needed to read the XML document.
            using (var fs = File.Exists(filePath) ? new StreamReader(filePath) : new StreamReader(assembly.GetManifestResourceStream(resourceName)))
            {
                try
                {
                    var json = fs.ReadToEnd();
                    var settings = new JsonSerializerSettings
                    {
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver
                        {
                            NamingStrategy = new Newtonsoft.Json.Serialization.DefaultNamingStrategy()
                        }
                    };
                    EnvConfigs obj = JsonConvert.DeserializeObject<EnvConfigs>(json, settings);
                    Inst = obj;
                }
                catch (Exception ex)
                {
                    ErrorLog.Inst.LogError($"Unable to load Config file : {filePath} : {ex.Message}");
                }
            }
#if !DEBUG
            if (!File.Exists(filePath))
            {
                ErrorLog.Inst.ShowInfo($"Unable to find `{Path.GetFileName(envFile)}` Config file : {filePath}");
            }
#endif
        }

        private static void LoadConfigXml()
        {
            EnvConfigs retVal = null;

            string fileLocation = Assembly.GetExecutingAssembly().Location;
            string envFile = string.Format("{0}\\Config\\Environments.xml", Path.GetDirectoryName(fileLocation));

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "LaunchEnvironment.Resource.Config.Environments.xml";

            XmlSerializer serializer = new XmlSerializer(typeof(EnvConfigs));
            // A FileStream is needed to read the XML document.
            using (var fs = File.Exists(envFile) ? new StreamReader(envFile) : new StreamReader(assembly.GetManifestResourceStream(resourceName)))
            {
                try
                {
                    retVal = (EnvConfigs)serializer.Deserialize(fs);
                }
                catch (Exception ex)
                {
                    ErrorLog.Inst.LogError($"Unable to load Config file : {envFile} : {ex.Message}");
                }
            }
#if !DEBUG
            if (!File.Exists(envFile))
            {
                ErrorLog.Inst.ShowInfo($"Unable to find `{Path.GetFileName(envFile)}` Config file : {envFile}");
            }
#endif
            Inst = retVal;
        }

        public static EnvConfigs Inst { get; private set; } = null;
    }
}
