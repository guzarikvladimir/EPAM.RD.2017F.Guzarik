using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomServices.Configuration
{
    public class AppConfigSection : ConfigurationSection
    {
        public AppConfigSection()
        {
        }

        [ConfigurationProperty("endPointsCollection")]
        public EndPointsCollection EndPoints
        {
            get
            {
                return this["endPointsCollection"] as EndPointsCollection;
            }
        }

        [ConfigurationProperty("filePath")]
        public FilePathElement FilePath
        {
            get
            {
                return this["filePath"] as FilePathElement;
            }
        }

        [ConfigurationProperty("logging")]
        public LoggingElement Logging
        {
            get
            {
                return this["logging"] as LoggingElement;
            }
        }

        [ConfigurationProperty("masterEndPoint")]
        public EndPointElement MasterEndPoint
        {
            get
            {
                return this["masterEndPoint"] as EndPointElement;
            }
        }

        public static AppConfigSection GetConfigSection()
        {
            return ConfigurationManager.GetSection("MasterSection") as AppConfigSection;
        }
    }
}
