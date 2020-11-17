using System;
using Object = UnityEngine.Object;

namespace HealingJam
{
    public interface IObjectFactory<T>
    {
        T Create();
    }

    public class DefaultObjectFactory<T> : IObjectFactory<T> where T : new()
    {
        public T Create()
        {
            return new T();
        }
    }

    public class CustomObjectFactory<T> : IObjectFactory<T>
    {
        protected Func<T> factoryMethod;

        public CustomObjectFactory(Func<T> factoryMethod)
        {
            this.factoryMethod = factoryMethod;
        }

        public T Create()
        {
            return factoryMethod();
        }
    }

    public class InstantiateObjectFactory<T> : IObjectFactory<T> where T : Object 
    {
        private readonly T originObject;

        public InstantiateObjectFactory(T originObject)
        {
            this.originObject = originObject;
        }

        public T Create()
        {
            return Object.Instantiate(originObject);
        }
    }
}
