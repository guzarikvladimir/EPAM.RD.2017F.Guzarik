using System;
using System.Collections.Generic;

namespace CustomServices.Concrete
{
    [Serializable]
    public class MasterNodeChanges : MarshalByRefObject
    {
        public List<User> Users { get; set; }
        public State State { get; set; }
    }

    [Serializable]
    public enum State
    {
        Added,
        Removed
    }
}
