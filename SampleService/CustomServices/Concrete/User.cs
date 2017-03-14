using System;
using System.Globalization;

namespace CustomServices.Concrete
{
    /// <summary>
    /// Class represents a person
    /// </summary>
    [Serializable]
    public class User : MarshalByRefObject
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
        /// Age of the user
        /// </summary>
        public int Age { get; set; }

        public override string ToString()
        {
            return $"{this.Id}" + Environment.NewLine +
                   $"{this.FirstName}" + Environment.NewLine +
                   $"{this.LastName}" + Environment.NewLine +
                   $"{this.Age.ToString(CultureInfo.CurrentCulture)}";
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
    }
}