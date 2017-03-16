using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using CustomServices.Concrete;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            //AppDomainSetup ads = new AppDomainSetup
            //{
            //    ApplicationBase = AppDomain.CurrentDomain.BaseDirectory,
            //    PrivateBinPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AD #2"),
            //    ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile
            //};
            //var ad2 = AppDomain.CreateDomain("AD #2");

            //var assembly = Assembly.Load("CustomServices");

            //UserService service =
            //    (UserService)
            //        ad2.CreateInstanceAndUnwrap(assembly.FullName, typeof(UserService).FullName);

            UserService service = new UserService();

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
            //AppDomain.Unload(ad2);
            Console.ReadKey();
        }
    }
}
