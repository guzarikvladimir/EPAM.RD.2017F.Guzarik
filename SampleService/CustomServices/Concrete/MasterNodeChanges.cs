﻿using System;
using System.Collections.Generic;

namespace CustomServices.Concrete
{
    [Serializable]
    public class MasterNodeChanges
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
