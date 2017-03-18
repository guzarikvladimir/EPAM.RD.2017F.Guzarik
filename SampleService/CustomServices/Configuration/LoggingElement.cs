using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomServices.Configuration
{
    public class LoggingElement : ConfigurationElement
    {
        [ConfigurationProperty("value")]
        public bool Value
        {
            get
            {
                return string.Equals(this["value"] as string, "true", StringComparison.InvariantCultureIgnoreCase);
            }
        }
    }
}
