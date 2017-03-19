using System;
using System.CodeDom;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using CustomServices.Abstract;
using CustomServices.Configuration;
using CustomServices.Exceptions;
using NLog;

namespace CustomServices.Concrete
{
    /// <summary>
    /// Class represents a master service that allows to add, remove and find users
    /// </summary>
    public sealed class UserServiceMaster : MarshalByRefObject, IService<User>
    {
        private readonly List<TcpClient> activeServices;
        private readonly List<TcpClient> removedServices;
        private readonly TcpListener listener;
        private readonly BinaryFormatter formatter;
        private List<User> collection;
        private IGenerator idGenerator;
        private string[] registeredServices;
        private Action<string> logAction = delegate { };
        private ReaderWriterLockSlim locker = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

        /// <summary>
        /// Creates an instance of master service with default generator and begins to accept slaves
        /// </summary>
        public UserServiceMaster() : this(new DefaultGenerator(1))
        {
        }

        /// <summary>
        /// Creates an instance of master service with the specified generator and begins to accept slaves
        /// </summary>
        /// <param name="generator">Class that generates id for user</param>
        public UserServiceMaster(IGenerator generator)
        {
            this.IdGenerator = generator;
            this.collection = new List<User>();
            this.formatter = new BinaryFormatter();
            this.removedServices = new List<TcpClient>();
            this.activeServices = new List<TcpClient>();

            AppConfigSection config = AppConfigSection.GetConfigSection();

            this.TuneLogger(config);
            this.GetRegisteredServices(config);

            string[] localEndPoint = config.MasterEndPoint.EndPoint.Split(':');
            this.listener = new TcpListener(new IPEndPoint(IPAddress.Parse(localEndPoint[0]), int.Parse(localEndPoint[1])));
            this.listener.Start();

            new Thread(this.Listen) { IsBackground = true }.Start();

            var t = new Timer(this.CheckConnection, null, 600000, 600000);
        }

        /// <summary>
        /// Gets or sets generator
        /// </summary>
        /// <exception cref="ArgumentNullException">Throws when generator is null</exception>
        public IGenerator IdGenerator
        {
            get
            {
                return this.idGenerator;
            }

            set
            {
                if (object.ReferenceEquals(value, null))
                {
                    throw new ArgumentNullException(nameof(value));
                }

                this.idGenerator = value;
            }
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
            if (object.ReferenceEquals(user, null))
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (!this.ValidateUser(user))
            {
                throw new UserIsNotValidException(nameof(user));
            }

            user.Id = this.IdGenerator.GenerateId();

            if (this.Exists(user))
            {
                throw new UserAlreadyExistsException(nameof(user));
            }

            this.locker.EnterWriteLock();
            try
            {
                this.collection.Add(user);
            }
            finally
            {
                this.locker.ExitWriteLock();
            }

            this.logAction($"{user.FirstName} {user.LastName} has been added.");

            this.Send(new MasterNodeChanges()
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
            if (object.ReferenceEquals(predicate, null))
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            User[] users;
            this.locker.EnterReadLock();
            try
            {
                users = this.collection.Where(predicate).ToArray();
            }
            finally
            {
                this.locker.ExitReadLock();
            }

            foreach (var user in users)
            {
                this.locker.EnterWriteLock();
                try
                {
                    this.collection.Remove(user);
                }
                finally
                {
                    this.locker.ExitWriteLock();
                }

                this.logAction($"{user.FirstName} {user.LastName} has been removed.");
            }

            this.Send(new MasterNodeChanges()
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
        /// <returns>Returns a list of users</returns>
        public List<User> Find(Func<User, bool> predicate)
        {
            if (object.ReferenceEquals(predicate, null))
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            this.locker.EnterReadLock();
            try
            {
                return this.collection.Where(predicate).ToList();
            }
            finally
            {
                this.locker.ExitReadLock();
            }
        }

        /// <summary>
        /// Saves collection to the storage
        /// </summary>
        /// <param name="storage">Class that implements interface IUserStorage and its save/load functions</param>
        /// <exception cref="ArgumentNullException">Throws when storage is null</exception>
        public void Save(IUserStorage storage)
        {
            if (object.ReferenceEquals(storage, null))
            {
                throw new ArgumentNullException(nameof(storage));
            }

            storage.Save(this.collection);
        }

        /// <summary>
        /// Loads collection from the storage
        /// </summary>
        /// <param name="storage">Class that implements interface IUserStorage and its save/load functions</param>
        /// <exception cref="ArgumentNullException">Throws when storage is null</exception>
        public void Load(IUserStorage storage)
        {
            if (object.ReferenceEquals(storage, null))
            {
                throw new ArgumentNullException(nameof(storage));
            }

            this.locker.EnterWriteLock();
            try
            {
                this.collection = storage.Load().ToList();
            }
            finally
            {
                this.locker.ExitWriteLock();
            }

            this.Send(new MasterNodeChanges()
            {
                Users = this.collection.ToList(),
                State = State.Added
            });
        }

        /// <summary>
        /// Returns collection of users
        /// </summary>
        public IEnumerable<User> GetAllUsers()
        {
            return this.collection.ToArray();
        }

        #region PrivateMethods

        private void TuneLogger(AppConfigSection config)
        {
            if (config.Logging.Value)
            {
                this.logAction = delegate(string str)
                {
                    Logger logger = LogManager.GetCurrentClassLogger();
                    logger.Info(str);
                };
            }
        }

        private void GetRegisteredServices(AppConfigSection config)
        {
            int count = config.EndPoints.Count;
            this.registeredServices = new string[count];
            for (int i = 0; i < count; i++)
            {
                this.registeredServices[i] = config.EndPoints[i].EndPoint;
            }
        }

        private bool ValidateUser(User user)
        {
            if (string.IsNullOrEmpty(user.FirstName) | string.IsNullOrEmpty(user.LastName))
            {
                return false;
            }

            return true;
        }

        private bool Exists(User user)
        {
            this.locker.EnterReadLock();
            try
            {
                if (this.collection.Any(u => u.Id == user.Id))
                {
                    return true;
                }
            }
            finally
            {
                this.locker.ExitReadLock();
            }

            return false;
        }

        private void Send(MasterNodeChanges user)
        {
            this.locker.EnterUpgradeableReadLock();
            try
            {
                foreach (TcpClient service in this.activeServices)
                {
                    try
                    {
                        NetworkStream stream = service.GetStream();
                        this.formatter.Serialize(stream, user);
                    }
                    catch (Exception)
                    {
                        this.locker.EnterWriteLock();
                        try
                        {
                            this.removedServices.Add(service);
                        }
                        finally
                        {
                            this.locker.ExitWriteLock();
                        }
                    }
                }
            }
            finally
            {
                this.locker.ExitUpgradeableReadLock();
            }

            this.CleanUp();
        }

        private void CleanUp()
        {
            this.locker.EnterUpgradeableReadLock();
            try
            {
                foreach (TcpClient client in this.removedServices)
                {
                    this.locker.EnterWriteLock();
                    try
                    {
                        this.activeServices.Remove(client);
                    }
                    finally
                    {
                        this.locker.ExitWriteLock();
                    }
                }
            }
            finally
            {
                this.locker.ExitUpgradeableReadLock();
            }

            this.locker.EnterWriteLock();
            try
            {
                this.removedServices.Clear();
            }
            finally
            {
                this.locker.ExitWriteLock();
            }
        }

        private void Listen()
        {
            TcpClient client = null;

            while (true)
            {
                try
                {
                    client = this.listener.AcceptTcpClient();

                    if (!this.ValidateEndPoint(client))
                    {
                        client.Close();
                    }

                    this.activeServices.Add(client);

                    NetworkStream stream = client.GetStream();

                    this.formatter.Serialize(
                        stream,
                        new MasterNodeChanges()
                        {
                            Users = this.collection.ToList()
                        });
                }
                catch
                {
                    client?.Close();
                }
            }
        }

        private bool ValidateEndPoint(TcpClient client)
        {
            string clientEndPoint = client.Client.RemoteEndPoint.ToString();

            this.locker.EnterReadLock();
            try
            {
                if (this.registeredServices.Contains(clientEndPoint))
                {
                    return true;
                }
            }
            finally
            {
                this.locker.ExitReadLock();
            }

            return false;
        }

        private void CheckConnection(object state)
        {
            this.locker.EnterUpgradeableReadLock();
            try
            {
                foreach (TcpClient client in this.activeServices)
                {
                    if (!client.Connected)
                    {
                        this.locker.EnterWriteLock();
                        try
                        {
                            this.removedServices.Add(client);
                        }
                        finally
                        {
                            this.locker.ExitWriteLock();
                        }
                    }
                }
            }
            finally
            {
                this.locker.ExitUpgradeableReadLock();
            }

            this.CleanUp();
        }

        #endregion
    }
}
