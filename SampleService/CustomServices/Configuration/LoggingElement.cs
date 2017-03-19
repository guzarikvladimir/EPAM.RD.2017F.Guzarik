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
                string val = this["value"] as string;
                return string.Equals(val, "true", StringComparison.InvariantCultureIgnoreCase);
            }
        }
    }
}
