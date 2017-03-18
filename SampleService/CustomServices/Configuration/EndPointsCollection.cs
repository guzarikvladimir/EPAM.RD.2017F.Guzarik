using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomServices.Configuration
{
    public class EndPointsCollection : ConfigurationElementCollection
    {
        public EndPointElement this[int index]
        {
            get
            {
                return base.BaseGet(index) as EndPointElement;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new EndPointElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (EndPointElement) element;
        }
    }
}
