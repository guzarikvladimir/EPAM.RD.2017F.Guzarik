using System;
using System.Linq;
using CustomServices.Concrete;

namespace Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            UserService service =
                (UserService)ServiceFactory.ServiceFactory.Create(
                    AppDomain.CurrentDomain, 
                    "AD #2", 
                    "CustomServices",
                    typeof(UserService));

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

            //// AppDomain.Unload(ad2);

            Console.ReadKey();
        }
    }
}
