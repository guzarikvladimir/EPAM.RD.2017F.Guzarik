using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyServiceLibrary.Models;

namespace MyServiceLibrary.Concrete
{
    public class UserAddedEventArgs : EventArgs
    {
        public User User { get; set; }
        public string Message { get; set; }
    }
}
