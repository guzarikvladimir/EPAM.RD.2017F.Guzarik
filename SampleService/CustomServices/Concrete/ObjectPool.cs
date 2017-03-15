using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomServices.Concrete
{
    internal class ObjectPool<T>
    {
        private readonly ConcurrentBag<T> objects;

        public ObjectPool()
        {
            objects = new ConcurrentBag<T>();
        }

        public T GetObject()
        {
            T item;
            if (objects.TryTake(out item))
            {
                return item;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void PutObject(T item)
        {
            objects.Add(item);
        }
    }
}
