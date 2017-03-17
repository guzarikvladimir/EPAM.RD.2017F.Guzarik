using System;
using System.Collections.Generic;

namespace CustomServices.Concrete
{
    [Serializable]
    public enum State
    {
        Added,

        Removed
    }

    [Serializable]
    public class MasterNodeChanges
    {
        public List<User> Users { get; set; }

        public State State { get; set; }
    }
}
