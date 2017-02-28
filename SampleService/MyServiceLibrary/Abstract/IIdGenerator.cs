using MyServiceLibrary.Models;

namespace MyServiceLibrary.Abstract
{
    /// <summary>
    /// Defines a method to generate an id 
    /// </summary>
    public interface IIdGenerator
    {
        /// <summary>
        /// Generate some id
        /// </summary>
        /// <returns>Returns an id</returns>
        int GenerateId();

        /// <summary>
        /// Generates id on user's fields
        /// </summary>
        /// <param name="user">An instance of user</param>
        /// <returns>Returns an id</returns>
        int GenerateId(User user);
    }
}
