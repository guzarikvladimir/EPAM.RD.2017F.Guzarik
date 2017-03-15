using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomServices.Concrete
{
    [Serializable]
    public class Message
    {
        public string Hostname { get; set; }
        public int Port { get; set; }
    }
}
