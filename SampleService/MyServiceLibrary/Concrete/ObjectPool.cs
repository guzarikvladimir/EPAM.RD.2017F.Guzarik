using System;
using System.Collections.Concurrent;
using MyServiceLibrary.Abstract;

namespace MyServiceLibrary.Concrete
{
    public class ObjectPool<T, F> where T : class, new()
    {
        private readonly ConcurrentBag<T> _container = new ConcurrentBag<T>();

        private readonly IPoolObjectCreator<T, F> _objectCreator;

        private readonly object obj;

        /// <summary>Total instances.</summary>
        public int Count => this._container.Count;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ObjectPool"/> class.
        /// </summary>
        /// <param name="creator">Interface of the object creator. It can't be null.</param>
        public ObjectPool(IPoolObjectCreator<T, F> creator) : 
            this(creator, null)
        {
        }

        public ObjectPool(IPoolObjectCreator<T, F> creator, object obj)
        {
            if (creator == null)
            {
                throw new ArgumentNullException();
            }
            this._objectCreator = creator;
            this.obj = obj;
            InitiaizeObjectPool(obj);
        }

        /// <summary>Gets an object from the pool.</summary>
        /// <returns>An object.</returns>
        public T GetObject()
        {
            T obj;
            if (this._container.TryTake(out obj))
            {
                return obj;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>Returns the specified object to the pool.</summary>
        /// <param name="obj">The object to return.</param>
        public void ReturnObject(ref T obj)
        {
            this._container.Add(obj);
            obj = null;
        }

        private void InitiaizeObjectPool(object obj)
        {
            int count = Properties.Settings.Default.SlaveCount;
            for (int i = 0; i < count; i++)
            {
                _container.Add(_objectCreator.Create((F)obj));
            }
        }
    }
}
