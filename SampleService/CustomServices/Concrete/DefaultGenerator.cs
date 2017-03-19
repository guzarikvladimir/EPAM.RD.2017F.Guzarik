using System;
using CustomServices.Abstract;

namespace CustomServices.Concrete
{
    /// <summary>
    /// Class implements an IGenerator interface
    /// </summary>
    [Serializable]
    public class DefaultGenerator : IGenerator
    {
        private int current;

        /// <summary>
        /// Initializes the start value from which generating will be started
        /// </summary>
        /// <param name="start">Start value</param>
        public DefaultGenerator(int start)
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
