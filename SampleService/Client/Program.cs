using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CustomServices.Concrete;
using ServiceManager.Concrete;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            //var service = new UserServiceCreator().Create("NewDomain");
            var service = new UserService();

            var content = service.Find(x => x.FirstName.Any()).ToList();

            foreach (var user in content)
            {
                Console.WriteLine(user.ToString());
            }

            Console.ReadKey();

            content = service.Find(x => x.FirstName.Any()).ToList();

            Console.WriteLine("After adding:");
            foreach (var user in content)
            {
                Console.WriteLine(user.ToString());
            }

            Console.ReadKey();

            content = service.Find(x => x.FirstName.Any()).ToList();

            Console.WriteLine("After removing:");
            foreach (var user in content)
            {
                Console.WriteLine(user.ToString());
            }

            Console.ReadKey();
        }
    }
}
