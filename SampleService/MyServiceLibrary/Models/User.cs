using System;
using System.Globalization;

namespace MyServiceLibrary.Models
{
    /// <summary>
    /// Class represents a person
    /// </summary>
    [Serializable]
    public class User
    {
        /// <summary>
        /// Id of the user
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// First name of the user
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of the user
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Date of birth of the user
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        public override string ToString()
        {
            return $"{this.Id}" + Environment.NewLine +
                   $"{this.FirstName}" + Environment.NewLine +
                   $"{this.LastName}" + Environment.NewLine +
                   $"{this.DateOfBirth.ToString(CultureInfo.CurrentCulture)}";
        }
    }
}
