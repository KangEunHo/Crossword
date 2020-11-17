using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealingJam
{
    public interface IObjectPool<T>
    {
        void Push(T item);
        T Pop();
        void ReturnAllObjectsToPool();
        void DisposeAll();
    }

    public interface IPoolable
    {
        void OnRecycled();
        void OnReturnToPool();
        void Dispose();
    }
}
