using System;
using CustomServices.Concrete;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var service =
                (UserServiceMaster)ServiceFactory.ServiceFactory.Create(
                    AppDomain.CurrentDomain, 
                    "AD Master",
                    "CustomServices", 
                    typeof(UserServiceMaster));

            //// service.Load(new UserStorageXml());

            Console.WriteLine("Master service content:");
            foreach (var user in service.GetAllUsers())
            {
                Console.WriteLine(user.ToString());
            }

            Console.WriteLine("Press any key to continue...");
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

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

            service.Remove(u => u.FirstName == "New User");

            Console.WriteLine("Master service content after removing:");
            foreach (var user in service.GetAllUsers())
            {
                Console.WriteLine(user.ToString());
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
