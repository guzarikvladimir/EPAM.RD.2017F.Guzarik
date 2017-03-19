using System.Configuration;

namespace CustomServices.Configuration
{
    public class EndPointElement : ConfigurationElement
    {
        [ConfigurationProperty("hostname")]
        public string Hostname
        {
            get
            {
                return this["hostname"] as string;
            }
        }

        [ConfigurationProperty("port")]
        public string Port
        {
            get
            {
                return this["port"] as string;
            }
        }
    }
}
