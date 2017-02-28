using System.IO;

namespace MyServiceLibrary.Exceptions
{
    /// <summary>
    /// Throws when name of the storage is not found
    /// </summary>
    public class NameNotFoundException : FileNotFoundException
    {
        public NameNotFoundException(string message, FileNotFoundException inner) : base(message, inner)
        {
        }
    }
}
