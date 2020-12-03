using UnityEngine;
using System.Collections.Generic;

namespace HealingJam
{
    public class MonoObjPool<T> : ObjectPool<T> where T : MonoBehaviour
    {
        private readonly Transform itemParent = null;

        public MonoObjPool(IObjectFactory<T> objectFactory, ObjPoolData poolData) : base(objectFactory, poolData)
        {
            itemParent = poolData.itemParent;

            for (int i = 0; i < poolData.initCount; i++)
            {
                Push(objectFactory.Create());
            }
        }

        public override T Pop()
        {
            T item = base.Pop();

            item.gameObject.SetActive(true);

            return item;
        }

        public C Pop<C>() where C : Component
        {
            return Pop().GetComponent<C>();
        }

        public override void Push(T item)
        {
            base.Push(item);

            item.gameObject.SetActive(false);
            item.transform.SetParent(itemParent, false);
        }

        public override void DisposeAll()
        {
            if (waitingItems != null)
            {
                while (waitingItems.Count > 0)
                {
                    MonoBehaviour item = waitingItems.Pop();
                    Object.Destroy(item.gameObject);
                }
                waitingItems.Clear();
            }

            if (usingItems != null)
            {
                while (usingItems.Last != null)
                {
                    MonoBehaviour item = usingItems.Last.Value;
                    usingItems.RemoveLast();
                    Object.Destroy(item.gameObject);
                }
                usingItems.Clear();
            }
        }
    }
}