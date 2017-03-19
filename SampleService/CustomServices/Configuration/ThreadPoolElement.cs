using System.Configuration;

namespace CustomServices.Configuration
{
    public class ThreadPoolElement : ConfigurationElement
    {
        [ConfigurationProperty("max")]
        public string Max
        {
            get
            {
                return this["max"] as string;
            }
        }

        [ConfigurationProperty("min")]
        public string Min
        {
            get
            {
                return this["min"] as string;
            }
        }
    }
}
