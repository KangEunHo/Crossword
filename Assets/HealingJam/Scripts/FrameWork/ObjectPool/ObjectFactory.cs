using System;
using UnityEngine;
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
        private readonly Transform parent;

        public InstantiateObjectFactory(T originObject, Transform parent)
        {
            this.originObject = originObject;
            this.parent = parent;
        }

        public T Create()
        {
            return Object.Instantiate(originObject, parent);
        }
    }
}
