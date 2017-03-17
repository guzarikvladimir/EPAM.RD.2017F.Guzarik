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

        /// <summary>
        /// Compares users
        /// </summary>
        /// <param name="obj">User to compare</param>
        /// <returns>True if all fields are the same else false</returns>
        /// <exception cref="ArgumentNullException">Throws when obj is null or is not a user</exception>
        public override bool Equals(object obj)
        {
            var user = obj as User;

            if (ReferenceEquals(user, null))
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return this.Equals(user);
        }

        /// <summary>
        /// Compares users
        /// </summary>
        /// <param name="user">User to compare</param>
        /// <returns>True if all fields are the same else false</returns>
        public bool Equals(User user)
        {
            return this.Id == user.Id &&
                   string.Equals(this.FirstName, user.FirstName, StringComparison.InvariantCultureIgnoreCase) &&
                   string.Equals(this.LastName, user.LastName, StringComparison.InvariantCultureIgnoreCase) &&
                   this.Age == user.Age;
        }

        /// <summary>
        /// Returns a string representation of user
        /// </summary>
        public override string ToString()
        {
            return $"{this.Id}" + Environment.NewLine +
                   $"{this.FirstName}" + Environment.NewLine +
                   $"{this.LastName}" + Environment.NewLine +
                   $"{this.Age.ToString(CultureInfo.CurrentCulture)}";
        }

        /// <summary>
        /// Returns hash of the object on its string representation
        /// </summary>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
    }
}