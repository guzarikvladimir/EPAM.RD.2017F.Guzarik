using System;
using System.Collections.Generic;
using System.Linq;
using MyServiceLibrary.Models;

namespace MyServiceLibrary.Concrete
{
    /// <summary>
    /// 
    /// </summary>
    public class UserService
    {
        private readonly List<User> collection;

        public UserService()
        {
            collection = new List<User>();
        }

        public UserService(UserServiceMaster service)
        {
            service.UserAdded += ActWhenUserAdded;
            collection = service.GetAllUsers().ToList();
        }

        //public UserService(IEnumerable<User> collection)
        //{
        //    this.collection = collection.ToList();
        //}

        public IEnumerable<User> Find(Func<User, bool> predicate)
        {
            return this.collection.Where(predicate);
        }

        private void ActWhenUserAdded(object sender, UserAddedEventArgs eventArgs)
        {
            collection.Add(eventArgs.User);
        }
    }
}
