using MyServiceLibrary.Abstract;
using MyServiceLibrary.Models;

namespace MyServiceLibrary.Concrete
{
    /// <summary>
    /// Class implements an IIdGenerator interface
    /// </summary>
    public class DefaultIdGenerator : IIdGenerator
    {
        private int current;

        /// <summary>
        /// Initializes the start value from which generating will be started
        /// </summary>
        /// <param name="start">Start value</param>
        public DefaultIdGenerator(int start)
        {
            this.current = start;
        }

        /// <summary>
        /// Generates an id
        /// </summary>
        /// <remarks>Increments previous id by 1</remarks>
        /// <returns>Returns an actual id</returns>
        public int GenerateId()
        {
            return this.current++;
        }

        /// <summary>
        /// Generates id on user's fields
        /// </summary>
        /// <param name="user">An instance of user</param>
        /// <returns>Returns an id</returns>
        public int GenerateId(User user)
        {
            return user.GetHashCode();
        }
    }
}
