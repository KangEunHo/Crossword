//using UnityEngine;
//using System.Collections.Generic;
//using System;

//namespace HealingJam
//{
//    public class ObjectPoolMgr : MonoSingleton<ObjectPoolMgr>
//    {
//        [SerializeField] PoolData[] poolDatas = null;
//        private Dictionary<Type, IObjectPool<Type>> objectPools = null;

//        protected override void Awake()
//        {
//            base.Awake();

//        }

//        private void Init()
//        {
//            objectPools = new Dictionary<Type, IObjectPool<Type>>();

//            foreach (var poolData in poolDatas)
//            {
//                if (poolData.cashComponent)
//                {

//                }
//                else
//                {

//                }
//            }
//        }
//    }
//}