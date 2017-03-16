using System;
using System.Globalization;

namespace CustomServices.Concrete
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
        /// Age of the user
        /// </summary>
        public int Age { get; set; }

        public override bool Equals(object obj)
        {
            var user = obj as User;

            if (ReferenceEquals(user, null))
            {
                throw new ArgumentException(nameof(user));
            }

            return Equals(user);
        }

        public bool Equals(User user)
        {
            return this.Id == user.Id &&
                   string.Equals(this.FirstName, user.FirstName, StringComparison.InvariantCultureIgnoreCase) &&
                   string.Equals(this.LastName, user.LastName, StringComparison.InvariantCultureIgnoreCase) &&
                   this.Age == user.Age;
        }

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