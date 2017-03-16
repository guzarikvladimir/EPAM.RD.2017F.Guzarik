using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using CustomServices.Abstract;
using CustomServices.Exceptions;

namespace CustomServices.Concrete
{
    /// <summary>
    /// Class to work with xml storage
    /// </summary>
    public class UserStorageXml : MarshalByRefObject, IUserStorage
    {
        private readonly string path;

        /// <summary>
        /// Creates a new xml storage
        /// </summary>>
        public UserStorageXml()
        {
            this.path = ConfigurationManager.AppSettings["FileName"] + ".xml";
        }

        /// <summary>
        /// Saves the user's collection to the xml storage
        /// </summary>
        /// <remarks>If storage with the specified name does't exist, it will be created</remarks>
        public void Save(IEnumerable<User> collection)
        {
            XDocument xdoc = new XDocument();
            XElement users = new XElement("users");

            foreach (var user in collection)
            {
                XElement elem = new XElement("user",
                    new XElement("Id", user.Id),
                    new XElement("FirstName", user.FirstName),
                    new XElement("LastName", user.LastName),
                    new XElement("Age", user.Age));
                users.Add(elem);
            }
            xdoc.Add(users);
            xdoc.Save(path);
        }

        /// <summary>
        /// Loads the user's collecction from the xml storage
        /// </summary>
        /// <exception cref="NameNotFoundException">Wrong path to the storage</exception>
        public IEnumerable<User> Load()
        {
            var collection = new List<User>();

            XmlDocument xdoc = new XmlDocument();

            try
            {
                xdoc.Load(path);
            }
            catch (FileNotFoundException exc)
            {
                throw new NameNotFoundException(exc.Message, exc);
            }
            
            XmlElement xroot = xdoc.DocumentElement;
            foreach (XmlElement xnode in xroot)
            {
                User user = new User();

                foreach (XmlNode childNode in xnode.ChildNodes)
                {
                    if (childNode.Name == "Id")
                        user.Id = int.Parse(childNode.InnerText);

                    if (childNode.Name == "FirstName")
                        user.FirstName = childNode.InnerText;

                    if (childNode.Name == "LastName")
                        user.LastName = childNode.InnerText;

                    if (childNode.Name == "Age")
                        user.Age = int.Parse(childNode.InnerText);
                }

                collection.Add(user);
            }

            return collection;
        }
    }
}
