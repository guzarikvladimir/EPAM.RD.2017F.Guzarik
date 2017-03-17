using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomServices.Exceptions
{
    /// <summary>
    /// Throws when user already exists
    /// </summary>
    public class UserAlreadyExistsException : Exception
    {
        /// <summary>
        /// Creates an empty exception
        /// </summary>
        public UserAlreadyExistsException()
        {
        }

        /// <summary>
        /// Creates an exception with the specified message
        /// </summary>
        /// <param name="message">Message of exception</param>
        public UserAlreadyExistsException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates an exception with the specifed mesage ind innedr exception
        /// </summary>
        /// <param name="message">Message of exception</param>
        /// <param name="inner">Inner exception</param>
        public UserAlreadyExistsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
