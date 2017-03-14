using CustomServices.Concrete;

namespace CustomServices.Abstract
{
    /// <summary>
    /// Defines generating methods
    /// </summary>
    public interface IGenerator
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
