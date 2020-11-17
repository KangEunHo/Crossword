using UnityEngine;
using System.Collections.Generic;

namespace HealingJam
{
    [System.Serializable]
    public class PoolData
    {
        public int initCount = 0;

        public PoolData(int initCount)
        {
            this.initCount = initCount;
        }
    }

    public class ObjectPool<T> : IObjectPool<T>, System.IDisposable
    {
        /// <summary>
        /// 오브젝트 객체 생성을 담당합니다.
        /// </summary>
        protected IObjectFactory<T> factory;
        /// <summary>
        /// 풀의 정보를 담고있습니다.
        /// </summary>
        protected PoolData poolData;
        /// <summary>
        /// 사용하고 있지 않는 오브젝트들을 담습니다.
        /// </summary>
        protected readonly Stack<T> waitingItems = new Stack<T>();
        /// <summary>
        /// 사용중인 오브젝트들을 담습니다.
        /// </summary>
        protected readonly LinkedList<T> usingItems = new LinkedList<T>();

        public ObjectPool(IObjectFactory<T> objectFactory, PoolData poolData)
        {
            factory = objectFactory;
            this.poolData = poolData;
        }

        public virtual T Pop()
        {
            T item = waitingItems.Count == 0 ? factory.Create() : waitingItems.Pop();
            usingItems.AddLast(item);
            return item;
        }

        public virtual void Push(T item)
        {
            usingItems.Remove(item);
            waitingItems.Push(item);
        }

        public virtual void ReturnAllObjectsToPool()
        {
            while (usingItems.Last != null)
            {
                Push(usingItems.Last.Value);
            }
        }

        /// <summary>
        /// 모든 아이템들을 제거합니다.
        /// 기본적으로 Dispose시에 호출되고, 모든 아이템을 제거하고 싶을때 호출합니다. 
        /// </summary>
        public virtual void DisposeAll() { }

        public virtual void Dispose()
        {
            DisposeAll();
        }
    }
}