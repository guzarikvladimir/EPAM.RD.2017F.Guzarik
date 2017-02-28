using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using MyServiceLibrary.Abstract;
using MyServiceLibrary.Exceptions;
using MyServiceLibrary.Models;

namespace MyServiceLibrary.Concrete
{
    /// <summary>
    /// Class to work with xml storage
    /// </summary>
    public class UserStorageXml : IUserStorage
    {
        private readonly string path;

        /// <summary>
        /// Creates a new xml storage
        /// </summary>>
        public UserStorageXml()
        {
            this.path = Properties.Settings.Default.FileName + ".xml";
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
                    new XElement("DateOfBirth", user.DateOfBirth));
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
            xdoc.Load(path);
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

                    if (childNode.Name == "DateOfBirth")
                        user.DateOfBirth = DateTime.Parse(childNode.InnerText);
                }

                collection.Add(user);
            }

            return collection;
        }
    }
}
