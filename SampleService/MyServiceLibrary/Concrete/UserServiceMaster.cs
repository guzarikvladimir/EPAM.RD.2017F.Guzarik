using System;
using System.Collections.Generic;
using System.Linq;
using MyServiceLibrary.Abstract;
using MyServiceLibrary.Exceptions;
using MyServiceLibrary.Models;
using NLog;

namespace MyServiceLibrary.Concrete
{
    /// <summary>
    /// Class represents a service of users
    /// </summary>
    public sealed class UserServiceMaster
    {
        private IIdGenerator idGenerator;
        private List<User> collection = new List<User>();
        private static readonly Lazy<UserServiceMaster> instance = 
            new Lazy<UserServiceMaster>(() => new UserServiceMaster());

        public static UserServiceMaster Instance
        {
            get
            {
                return instance.Value;
            }
        }

        public IIdGenerator IdGenerator
        {
            get { return IdGenerator; }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                idGenerator = value;
            }
        }

        public event EventHandler<UserAddedEventArgs> UserAdded = delegate { };

        ///// <summary>
        ///// Creates service with a custom id generator
        ///// </summary>
        ///// <param name="generator">Class with a definition of id generating function</param>
        //public UserServiceMaster(IIdGenerator generator)
        //{
        //    this.collection = new List<User>();
        //    this.generator = generator ?? new DefaultIdGenerator(1);
        //}

        ///// <summary>
        ///// Creates service with defaut id generator
        ///// </summary>
        //public UserServiceMaster()
        //{
        //    this.collection = new List<User>();
        //    this.generator = new DefaultIdGenerator(1);
        //}

        private UserServiceMaster()
        {
            
        }

        /// <summary>
        /// Adds a user to the service
        /// </summary>
        /// <remarks>Function uses generating function which generates an id for a new user</remarks>
        /// <param name="user">Instance of user</param>
        /// <exception cref="ArgumentNullException">Throws when user is null</exception>
        /// <exception cref="UserIsNotValidException">Throws when first name and last name are not defined</exception>
        /// <exception cref="UserAlreadyExistsException">Throws when a user with the same id already exists in the service</exception>
        public void Add(User user)
        {
            if (ReferenceEquals(user, null))
            {
                throw new ArgumentNullException();
            }

            if (string.IsNullOrEmpty(user.FirstName) | string.IsNullOrEmpty(user.LastName))
            {
                throw new UserIsNotValidException();
            }

            if (idGenerator == null)
            {
                idGenerator = new DefaultIdGenerator(0);
            }

            user.Id = idGenerator.GenerateId();

            if (collection.Any(u => u.Id == user.Id))
            {
                throw new UserAlreadyExistsException();
            }

            collection.Add(user);

            if (Properties.Settings.Default.Logging)
            {
                Logger logger = LogManager.GetCurrentClassLogger();
                logger.Info($"{user.FirstName} {user.LastName} has been added.");
            }
            OnUserAdded(new UserAddedEventArgs()
            {
                User = user
            });
        }

        /// <summary>
        /// Removes a user from the service on the specified condition
        /// </summary>
        /// <param name="predicate">Condition to delete a user</param>
        /// <exception cref="ArgumentNullException">Throws when user is null</exception>
        public void Remove(Func<User, bool> predicate)
        {
            if (ReferenceEquals(predicate, null))
            {
                throw new ArgumentNullException();
            }

            var users = collection.Where(predicate).ToArray();
            foreach (var user in users)
            {
                collection.Remove(user);
            }
        }

        /// <summary>
        /// Returns users that satisfy the specified condition
        /// </summary>
        /// <param name="predicate">Condition to find a user</param>
        /// <returns>Returns an enumeration of users</returns>
        public IEnumerable<User> Find(Func<User, bool> predicate)
        {
            return collection.Where(predicate);
        }

        /// <summary>
        /// Saves collection to the storage
        /// </summary>
        /// <param name="storage">Class that implements interface IUserStorage</param>
        public void Save(IUserStorage storage)
        {
            storage.Save(collection);
        }

        /// <summary>
        /// Loads collection from the storage
        /// </summary>
        /// <param name="storage">Class that implements interface IUserStorage</param>
        public void Load(IUserStorage storage)
        {
            collection = storage.Load().ToList();
        }

        public IEnumerable<User> GetAllUsers()
        {
            return collection;
        }

        private void OnUserAdded(UserAddedEventArgs e)
        {
            UserAdded(null, e);
        }
    }
}
