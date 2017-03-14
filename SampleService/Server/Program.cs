using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomServices.Concrete;
using ServiceManager.Concrete;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            //var service = new MasterServiceCreator().Create("SomeDomain");
            var service = new UserServiceMaster();
            service.Load(new UserStorageXml());

            Console.WriteLine("Master service content:");
            foreach (var user in service.GetAllUsers())
            {
                Console.WriteLine(user.ToString());
            }

            Console.ReadKey();

            service.Add(new User()
            {
                FirstName = "New User",
                LastName = "New user"
            });

            Console.WriteLine("Master service content after adding:");
            foreach (var user in service.GetAllUsers())
            {
                Console.WriteLine(user.ToString());
            }

            Console.ReadKey();

            service.Remove(u => u.FirstName == "New User");

            Console.WriteLine("Master service content after removing:");
            foreach (var user in service.GetAllUsers())
            {
                Console.WriteLine(user.ToString());
            }

            Console.ReadKey();
        }
    }
}
