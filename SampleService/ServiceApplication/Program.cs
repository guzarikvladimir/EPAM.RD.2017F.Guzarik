using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using CustomServices.Concrete;
using ServiceManager.Concrete;

namespace ServiceApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var service = new UserServiceMaster();

            // 1. Add a new user to the storage.
            service.Add(new User()
            {
                FirstName = "Vladimir",
                LastName = "Guzarik",
                Age = 20
            });
            service.Add(new User()
            {
                FirstName = "Vladimir",
                LastName = "SomeFam"
            });

            // 2. Remove an user from the storage.
            //service.Remove(user => user.LastName == "Guzarik");

            // 3. Search for an user by the first name.
            var userByFirstName = service.Find(user => user.FirstName == "Vladimir");

            // 4. Search for an user by the last name.
            var userByLastName = service.Find(user => user.LastName == "Guzarik");
            
            // Serialization
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream("Users.bin", FileMode.Create, FileAccess.Write))
            {
                foreach (User user in userByFirstName)
                {
                    formatter.Serialize(stream, user);
                }
            }

            // Deserialization
            var collection = new List<User>();
            try
            {
                using (Stream stream = new FileStream("Users.bin", FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    do
                    {
                        var user = (User)formatter.Deserialize(stream);
                        collection.Add(user);
                    }
                    while (true);
                }
            }
            catch
            {
                // ignored
            }

            Console.WriteLine("Collection:");
            foreach (var user in collection)
            {
                Console.WriteLine(user.ToString());
            }

            // Save to xml
            service.Save(new UserStorageXml());

            // Load from xml
            service.Load(new UserStorageXml());

            Console.WriteLine("Collection after loading:");
            foreach (var user in service.Find(user => user.FirstName == "Vladimir").ToList())
            {
                Console.WriteLine(user.ToString());
            }

            //Console.WriteLine();
            //Console.WriteLine("---Object Pool---");
            //Console.WriteLine();

            var service1 = new UserServiceCreator().Create("NewDomain");
            //var users1 = service1.Find(user => user.FirstName == "Vladimir").ToList();
            //foreach (var user in users1)
            //{
            //    Console.WriteLine(user.ToString());
            //}

            //service.Add(new User()
            //{
            //    FirstName = "FirstName",
            //    LastName = "LastName"
            //});

            //Console.WriteLine();
            //Console.WriteLine("---After adding---");
            //Console.WriteLine();

            //var users2 = service1.Find(user => user.FirstName.Any()).ToList();
            //foreach (var user in users2)
            //{
            //    Console.WriteLine(user.ToString());
            //}

            var master = new MasterServiceCreator().Create("MyDomain");

            //string[] ips = ConfigurationManager.AppSettings["SlaveIp"].Split(',');
            //foreach (var ip in ips)
            //{
            //    Console.WriteLine(ip);
            //}

            Console.ReadKey();
        }
    }
}
