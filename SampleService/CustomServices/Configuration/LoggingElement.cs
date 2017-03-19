using System;
using System.Configuration;

namespace CustomServices.Configuration
{
    public class LoggingElement : ConfigurationElement
    {
        [ConfigurationProperty("value")]
        public string Value
        {
            get
            {
                return this["value"] as string;
            }
        }
    }
}
