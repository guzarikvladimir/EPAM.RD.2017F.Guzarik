using System.Collections.Generic;
using CustomServices.Concrete;

namespace CustomServices.Abstract
{
    /// <summary>
    /// Provides methods for storage management
    /// </summary>
    public interface IUserStorage
    {
        /// <summary>
        /// Saves users to storage
        /// </summary>
        void Save(IEnumerable<User> collection);

        /// <summary>
        /// Loads users from storage
        /// </summary>
        IEnumerable<User> Load();
    }
}
