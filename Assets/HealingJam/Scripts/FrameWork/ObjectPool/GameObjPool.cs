using UnityEngine;

namespace HealingJam
{
    [System.Serializable]
    public class ObjPoolData : PoolData
    {
        public Transform itemParent = null;

        public ObjPoolData(Transform parent, int initCount) : base(initCount)
        {
            itemParent = parent;
        }
    }

    public class GameObjPool : ObjectPool<GameObject>
    {
        private readonly Transform itemParent = null;

        public GameObjPool(IObjectFactory<GameObject> objectFactory, ObjPoolData poolData) : base(objectFactory, poolData)
        {
            itemParent = poolData.itemParent;

            for (int i = 0; i < poolData.initCount; i++)
            {
                Push(objectFactory.Create());
            }
        }

        public override GameObject Pop()
        {
            GameObject item = base.Pop();

            item.SetActive(true);

            return item;
        }

        public C Pop<C>() where C : Component
        {
            return Pop().GetComponent<C>();
        }

        public override void Push(GameObject item)
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
                    GameObject item = waitingItems.Pop();
                    Object.Destroy(item.gameObject);
                }
                waitingItems.Clear();
            }

            if (usingItems != null)
            {
                while (usingItems.Last != null)
                {
                    GameObject item = usingItems.Last.Value;
                    usingItems.RemoveLast();
                    Object.Destroy(item.gameObject);
                }
                usingItems.Clear();
            }
        }
    }
}