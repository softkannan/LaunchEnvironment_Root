﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LaunchEnvironment.Config
{
    public class Configs_Root
    {
        public List<Config> Configs { get; set; }

        public static void LoadEnvironments()
        {
            Configs_Root retVal = null;

            string fileLocation = Assembly.GetExecutingAssembly().Location;
            string envFile = string.Format("{0}\\Config\\Environments.xml", Path.GetDirectoryName(fileLocation));
            if (File.Exists(envFile))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Configs_Root));
                // A FileStream is needed to read the XML document.
                using (var fs = new StreamReader(envFile))
                {
                    try
                    {
                        retVal = (Configs_Root)serializer.Deserialize(fs);
                    }
                    catch (Exception ex)
                    {
                        ErrorLog.Inst.LogError("Unable to load Config file : {0} : {1}", envFile, ex.Message);
                    }
                }
            }
            else
            {
                ErrorLog.Inst.LogError("Unable to file Config file : {0}", envFile);
            }

            _inst = retVal;
        }

        private static Configs_Root _inst;

        public static Configs_Root Inst
        {
            get
            {
                return _inst;
            }
        }
    }
}
