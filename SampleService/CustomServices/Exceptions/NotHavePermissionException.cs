using System;

namespace CustomServices.Exceptions
{
    public class NotHavePermissionException : Exception
    {
        public NotHavePermissionException()
        {
        }

        public NotHavePermissionException(string message)
            : base(message)
        {
        }

        public NotHavePermissionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
