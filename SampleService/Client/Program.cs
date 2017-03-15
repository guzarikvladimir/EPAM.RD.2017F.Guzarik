using System;
using System.Configuration;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using CustomServices.Concrete;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            //var service = new UserServiceCreator().Create("NewDomain");

            UserService service = null;
            TcpClient client = null;
            try
            {
                string[] remoteEndPoint = ConfigurationManager.AppSettings["MasterEndPoint"].Split(':');
                client = new TcpClient(remoteEndPoint[0], int.Parse(remoteEndPoint[1]));

                var formatter = new BinaryFormatter();
                NetworkStream stream = client.GetStream();

                service = (UserService) formatter.Deserialize(stream);
            }
            catch (Exception e)
            {
                // ignored
            }
            finally
            {
                client?.Close();
            }

            Console.WriteLine("User service content:");

            var content = service.Find(x => x.FirstName.Any()).ToList();

            foreach (var user in content)
            {
                Console.WriteLine(user.ToString());
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

            content = service.Find(x => x.FirstName.Any()).ToList();

            Console.WriteLine("After adding:");
            foreach (var user in content)
            {
                Console.WriteLine(user.ToString());
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

            content = service.Find(x => x.FirstName.Any()).ToList();

            Console.WriteLine("After removing:");
            foreach (var user in content)
            {
                Console.WriteLine(user.ToString());
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
