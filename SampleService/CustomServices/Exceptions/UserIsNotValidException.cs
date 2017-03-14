using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomServices.Exceptions
{
    /// <summary>
    /// Throws when user is not valid
    /// </summary>
    public class UserIsNotValidException : Exception
    {
        public UserIsNotValidException()
        {
        }

        public UserIsNotValidException(string message)
            : base(message)
        {
        }

        public UserIsNotValidException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
