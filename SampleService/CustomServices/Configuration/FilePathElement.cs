using System.Configuration;

namespace CustomServices.Configuration
{
    public class FilePathElement : ConfigurationElement
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
