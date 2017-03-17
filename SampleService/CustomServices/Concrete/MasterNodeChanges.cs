using System;
using System.Collections.Generic;

namespace CustomServices.Concrete
{
    /// <summary>
    /// Enum says what to do with the list of users
    /// </summary>
    [Serializable]
    public enum State
    {
        /// <summary>
        /// Sets when users are added 
        /// </summary>
        Added,

        /// <summary>
        /// Sets when users are removed
        /// </summary>
        Removed
    }

    /// <summary>
    /// Class stores information about changes and ation, that will be sent throw network
    /// </summary>
    [Serializable]
    public class MasterNodeChanges
    {
        /// <summary>
        /// List of users added or removed
        /// </summary>
        public List<User> Users { get; set; }

        /// <summary>
        /// It says what to do with the list of users
        /// </summary>
        public State State { get; set; }
    }
}
