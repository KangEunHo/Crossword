using UnityEngine;
using System.Collections.Generic;

namespace HealingJam
{
    /// <summary>
    /// 오브젝트 풀 사용법 샘플 클래스입니다.
    /// </summary>
    public class ObjPoolSample : MonoBehaviour
    {
        public GameObject prefab = null;

        GameObjPool gop = null;
        Stack<GameObject> usingItems = new Stack<GameObject>();

        // Use this for initialization
        void Start()
        {
            gop = new GameObjPool(new InstantiateObjectFactory<GameObject>(prefab, transform), new ObjPoolData(transform, 5));
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                usingItems.Push(gop.Pop());
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                if (usingItems.Count > 0)
                {
                    gop.Push(usingItems.Pop());
                }
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                gop.DisposeAll();
                usingItems.Clear();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                gop.ReturnAllObjectsToPool();
                usingItems.Clear();
            }
        }

    }
}