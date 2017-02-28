using System;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyServiceLibrary.Concrete;
using MyServiceLibrary.Exceptions;
using MyServiceLibrary.Models;

namespace MyServiceLibrary.Tests
{
    [TestClass]
    public class MyServiceTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Add_NullUser_ExceptionThrown()
        {
            var service = UserServiceMaster.Instance;

            service.Add(null);
        }

        [TestMethod]
        [ExpectedException(typeof(UserIsNotValidException))]
        public void Add_DefaultUser_ExceptionThrown()
        {
            var service = UserServiceMaster.Instance;
            var user = new User();

            service.Add(user);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Find_PredicateIsNull_ExceptionThrown()
        {
            var service = UserServiceMaster.Instance;

            service.Find(null);
        }

        [TestMethod]
        public void Find_UserExist_CollectionIsNotEmpty()
        {
            var service = UserServiceMaster.Instance;
            service.Add(new User()
            {
                FirstName = "First",
                LastName = "Last"
            });

            var users = service.Find(u => u.LastName == "Last");

            Assert.AreNotEqual(0, users.Count());
        }

        [TestMethod]
        public void Find_UserIsNotExist_CollectionIsEmpty()
        {
            var service = UserServiceMaster.Instance;

            var users = service.Find(u => u.LastName == "Last");

            Assert.AreEqual(0, users.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Remove_NullUser_ExceptionThrown()
        {
            var service = UserServiceMaster.Instance;

            service.Remove(null);
        }

        [TestMethod]
        public void Remove_UserExist_UserDeleted()
        {
            var service = UserServiceMaster.Instance;
            service.Add(new User()
            {
                FirstName = "First",
                LastName = "Last"
            });
            service.Add(new User()
            {
                FirstName = "First",
                LastName = "Last"
            });

            service.Remove(u => u.LastName == "Last");

            var users = service.Find(u => u.LastName == "Last");
            Assert.AreEqual(0, users.Count());
        }

        [TestMethod]
        public void Add_CustomIdGenerator_IdDifferentsBy100()
        {
            var service = UserServiceMaster.Instance;
            service.IdGenerator = new CustomIdGenerator(1);
            service.Add(new User()
            {
                FirstName = "First",
                LastName = "Last"
            });
            service.Add(new User()
            {
                FirstName = "First",
                LastName = "Last"
            });

            var users = service.Find(u => u.LastName == "Last").ToArray();
            var difference = users[1].Id - users[0].Id;

            Assert.AreEqual(100, difference);
        }
    }
}
