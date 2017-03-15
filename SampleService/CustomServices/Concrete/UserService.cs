using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using CustomServices.Abstract;
using CustomServices.Exceptions;

namespace CustomServices.Concrete
{
    [Serializable]
    public sealed class UserService : MarshalByRefObject, IService<User>
    {
        private readonly List<User> collection;
        private readonly TcpListener listener;

        public UserService(List<User> collection, string hostname, string port)
        {
            this.collection = collection;

            listener = new TcpListener(new IPEndPoint(IPAddress.Parse(hostname), int.Parse(port)));
            listener.Start();

            var thread = new Thread(Listen) { IsBackground = true };
            thread.Start();
        }

        public void Add(User item)
        {
            throw new NotHavePermissionException();
        }

        public void Remove(Func<User, bool> predicate)
        {
            throw new NotHavePermissionException();
        }

        public IEnumerable<User> Find(Func<User, bool> predicate)
        {
            if (ReferenceEquals(predicate, null))
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return this.collection.Where(predicate);
        }

        private void Listen()
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();

                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();

                    NetworkStream stream = client.GetStream();

                    var info = (MasterNodeChanges) formatter.Deserialize(stream);

                    switch (info.State)
                    {
                        case State.Added:
                            foreach (User user in info.Users)
                            {
                                collection.Add(user);
                            }
                            break;
                        case State.Removed:
                            foreach (User user in info.Users)
                            {
                                collection.Remove(user);
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    client.Close();
                }
            }
            catch (Exception)
            {
                // ignored
            }
            finally
            {
                listener.Stop();
            }
        }
    }
}
