using System;
using System.IO;

namespace CustomServices.Exceptions
{
    /// <summary>
    /// Throws when name of the storage is not found
    /// </summary>
    public class NameNotFoundException : FileNotFoundException
    {
        /// <summary>
        /// Creates an empty exception
        /// </summary>
        public NameNotFoundException(string message, FileNotFoundException inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Creates an exception with the specified message
        /// </summary>
        /// <param name="message">Message of exception</param>
        public NameNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates an exception with the specifed mesage ind innedr exception
        /// </summary>
        /// <param name="message">Message of exception</param>
        /// /// <param name="inner">Inner exception</param>
        public NameNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
