using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomServices.Configuration
{
    public class EndPointElement : ConfigurationElement
    {
        [ConfigurationProperty("endPoint")]
        public string EndPoint
        {
            get
            {
                return this["endPoint"] as string;
            }
        }
    }
}
