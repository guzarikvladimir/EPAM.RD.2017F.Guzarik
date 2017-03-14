using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using CustomServices.Abstract;
using CustomServices.Exceptions;
using System.Configuration;

namespace CustomServices.Concrete
{
    [Serializable]
    public sealed class UserService : MarshalByRefObject, IService<User>
    {
        private List<User> collection;
        private TcpClient client;

        public UserService()
        {
            collection = new List<User>();

            Connect();
            var thread = new Thread(Start) { IsBackground = true };
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

        private void Connect()
        {
            try
            {
                string[] masterEndPoint = ConfigurationManager.AppSettings["MasterEndPoint"].Split(':');
                string[] localEndPoint = ConfigurationManager.AppSettings["LocalEndPoint"].Split(':');
                client = new TcpClient(new IPEndPoint(IPAddress.Parse(localEndPoint[0]), int.Parse(localEndPoint[1])));
                client.Connect(masterEndPoint[0], int.Parse(masterEndPoint[1]));
                BinaryFormatter formatter = new BinaryFormatter();
                NetworkStream stream = client.GetStream();
                var col = (MasterNodeChanges) formatter.Deserialize(stream);
                this.collection = col.Users.ToList();
                stream.Close();
            }
            catch (Exception)
            {
                client?.Close();
            }
        }

        private void Start()
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();

                NetworkStream stream = client.GetStream();
                
                while (true)
                {
                    var obj = (MasterNodeChanges) formatter.Deserialize(stream);

                    switch (obj.State)
                    {
                        case State.Added:
                            foreach (var u in obj.Users)
                            {
                                collection.Add(u);
                            }
                            break;
                        case State.Removed:
                            foreach (var u in obj.Users)
                            {
                                collection.Remove(u);
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            catch (Exception e)
            {
                // ignored
            }
            finally
            {
                client.Close();
            }
        }
    }
}
