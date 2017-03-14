using System;
using System.Collections.Generic;

namespace CustomServices.Abstract
{
    public interface IService<T>
    {
        void Add(T item);
        void Remove(Func<T, bool> predicate);
        IEnumerable<T> Find(Func<T, bool> predicate);
    }
}
