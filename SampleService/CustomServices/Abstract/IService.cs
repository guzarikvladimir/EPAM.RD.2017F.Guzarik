using System;
using System.Collections.Generic;

namespace CustomServices.Abstract
{
    /// <summary>
    /// Declares methods for service
    /// </summary>
    /// <typeparam name="T">Defines objects that will be stored in service</typeparam>
    public interface IService<T>
    {
        /// <summary>
        /// Adds an object to service
        /// </summary>
        /// <param name="item"></param>
        void Add(T item);

        /// <summary>
        /// Removes an object from service
        /// </summary>
        /// <param name="predicate">Selection conditions</param>
        void Remove(Func<T, bool> predicate);

        /// <summary>
        /// Finds an object in service
        /// </summary>
        /// <param name="predicate">Selection conditions</param>
        /// <returns>Returns a list of objects that spetishy the specified condition</returns>
        List<T> Find(Func<T, bool> predicate);
    }
}
