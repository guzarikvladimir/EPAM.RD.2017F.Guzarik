using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using CustomServices.Abstract;
using CustomServices.Exceptions;
using NLog;

namespace CustomServices.Concrete
{
    /// <summary>
    /// Class represents a service of users
    /// </summary>
    [Serializable]
    public sealed class UserServiceMaster : MarshalByRefObject, IService<User>
    {
        private readonly IGenerator idGenerator;
        private List<User> collection;

        private readonly List<string> activeServices;
        private readonly string[] registeredServices;

        private readonly Action<string> logAction = delegate { };

        public UserServiceMaster() : this(new DefaultGenerator(1))
        {
        }

        public UserServiceMaster(IGenerator generator)
        {
            if (ReferenceEquals(generator, null))
            {
                throw new ArgumentNullException(nameof(generator));
            }

            this.idGenerator = generator;
            this.collection = new List<User>();
            this.activeServices = new List<string>();
            this.registeredServices = ConfigurationManager.AppSettings["SlaveEndPoints"].Split(',');

            bool logging = string.Equals(ConfigurationManager.AppSettings["Logging"], "true", StringComparison.OrdinalIgnoreCase);
            if (logging)
            {
                this.logAction = delegate (string str)
                {
                    Logger logger = LogManager.GetCurrentClassLogger();
                    logger.Info(str);
                };
            }

            var thread = new Thread(Listen) {IsBackground = true};
            thread.Start();
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

            user.Id = idGenerator.GenerateId();

            if (collection.Any(u => u.Id == user.Id))
            {
                throw new UserAlreadyExistsException();
            }

            collection.Add(user);

            logAction($"{user.FirstName} {user.LastName} has been added.");

            Send(new MasterNodeChanges()
            {
                Users = new List<User>() { user },
                State = State.Added
            });
        }

        /// <summary>
        /// Removes a user from the service on the specified condition
        /// </summary>
        /// <param name="predicate">Condition to delete a user</param>
        /// <exception cref="ArgumentNullException">Throws when predicate is null</exception>
        public void Remove(Func<User, bool> predicate)
        {
            if (ReferenceEquals(predicate, null))
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            var users = collection.Where(predicate).ToArray();
            foreach (var user in users)
            {
                collection.Remove(user);

                logAction($"{user.FirstName} {user.LastName} has been removed.");
            }

            Send(new MasterNodeChanges()
            {
                Users = users.ToList(),
                State = State.Removed
            });
        }

        /// <summary>
        /// Returns users that satisfy the specified condition
        /// </summary>
        /// <param name="predicate">Condition to find a user</param>
        /// <exception cref="ArgumentNullException">Throws when predicate is null</exception>
        /// <returns>Returns an enumeration of users</returns>
        public IEnumerable<User> Find(Func<User, bool> predicate)
        {
            if (ReferenceEquals(predicate, null))
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return collection.Where(predicate);
        }

        /// <summary>
        /// Saves collection to the storage
        /// </summary>
        /// <param name="storage">Class that implements interface IUserStorage</param>
        /// <exception cref="ArgumentNullException">Throws when storage is null</exception>
        public void Save(IUserStorage storage)
        {
            if (ReferenceEquals(storage, null))
            {
                throw new ArgumentNullException(nameof(storage));
            }

            storage.Save(collection);
        }

        /// <summary>
        /// Loads collection from the storage
        /// </summary>
        /// <param name="storage">Class that implements interface IUserStorage</param>
        /// <exception cref="ArgumentNullException">Throws when storage is null</exception>
        public void Load(IUserStorage storage)
        {
            if (ReferenceEquals(storage, null))
            {
                throw new ArgumentNullException(nameof(storage));
            }

            collection = storage.Load().ToList();
        }

        public IEnumerable<User> GetAllUsers()
        {
            return collection.ToArray();
        }

        private void Send(MasterNodeChanges user)
        {
            try
            {
                foreach (var activeService in activeServices)
                {
                    string[] endPoint = activeService.Split(':');
                    TcpClient client = new TcpClient(endPoint[0], int.Parse(endPoint[1]));
                    BinaryFormatter formatter = new BinaryFormatter();
                    NetworkStream stream = client.GetStream();
                    formatter.Serialize(stream, user);
                    stream.Close();
                    client.Close();
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void Listen()
        {
            TcpListener server = null;
            try
            {
                string[] endPoint = ConfigurationManager.AppSettings["MasterEndPoint"].Split(':');
                server = new TcpListener(IPAddress.Parse(endPoint[0]), int.Parse(endPoint[1]));
                server.Start();

                BinaryFormatter formatter = new BinaryFormatter();

                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();

                    string remoteEndPoint = client.Client.RemoteEndPoint.ToString();

                    if (registeredServices.Contains(remoteEndPoint))
                    {
                        activeServices.Add(remoteEndPoint);

                        NetworkStream stream = client.GetStream();

                        var obj = new MasterNodeChanges() {Users = collection.ToList(), State = State.Added};

                        formatter.Serialize(stream, obj);
                    }

                    client.Close();
                }
            }
            catch (SocketException e)
            {
                // ignored
            }
            catch (Exception e)
            {
                // ignored
            }
            finally
            {
                server?.Stop();
            }
        }
    }
}
