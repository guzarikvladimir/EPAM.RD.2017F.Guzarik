using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using CustomServices.Abstract;

namespace CustomServices.Concrete
{
    /// <summary>
    /// Class represents a slave service that allows to find users
    /// </summary>
    public sealed class UserService : MarshalByRefObject, IService<User>
    {
        private List<User> collection;
        private TcpClient client;
        private BinaryFormatter formatter;
        private NetworkStream stream;

        /// <summary>
        /// Creates an instance of the service and connects to the remote master
        /// </summary>
        public UserService()
        {
            this.collection = new List<User>();
            this.formatter = new BinaryFormatter();

            this.Connect();

            var thread = new Thread(this.Listen) { IsBackground = true };
            thread.Start();
        }

        /// <summary>
        /// Finds a user in service
        /// </summary>
        /// <param name="predicate">Selection conditions</param>
        /// <returns>Returns a list of objects that spetishy the specified condition</returns>
        public List<User> Find(Func<User, bool> predicate)
        {
            if (object.ReferenceEquals(predicate, null))
            {
                throw new ArgumentNullException();
            }

            return this.collection.Where(predicate).ToList();
        }

        #region private methods

        void IService<User>.Add(User item)
        {
            throw new NotImplementedException();
        }

        void IService<User>.Remove(Func<User, bool> predicate)
        {
            throw new NotImplementedException();
        }

        private void Connect()
        {
            string[] localEndPoint = ConfigurationManager.AppSettings["LocalEndPoint"].Split(':');
            string[] remoteEndPoint = ConfigurationManager.AppSettings["MasterEndPoint"].Split(':');
            this.client = new TcpClient(new IPEndPoint(IPAddress.Parse(localEndPoint[0]), int.Parse(localEndPoint[1])));
            this.client.Connect(remoteEndPoint[0], int.Parse(remoteEndPoint[1]));

            this.stream = this.client.GetStream();

            var info = (MasterNodeChanges)this.formatter.Deserialize(this.stream);

            this.collection = info.Users.ToList();
        }

        private void Listen()
        {
            try
            {
                while (true)
                {
                    if (this.stream.DataAvailable)
                    {
                        var info = (MasterNodeChanges)this.formatter.Deserialize(this.stream);

                        switch (info.State)
                        {
                            case State.Added:
                                foreach (User user in info.Users)
                                {
                                    this.collection.Add(user);
                                }

                                break;
                            case State.Removed:
                                foreach (User user in info.Users)
                                {
                                    this.collection.Remove(user);
                                }

                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
            finally
            {
                this.client?.Close();
            }
        }

        #endregion
    }
}
