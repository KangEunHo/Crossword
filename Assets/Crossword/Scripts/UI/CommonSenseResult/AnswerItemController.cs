using UnityEngine;
using System.Collections.Generic;

namespace HealingJam.Crossword.UI
{
    public class AnswerItemController : MonoBehaviour
    {
        [SerializeField] private AnswerItem answerItemPrefab = null;
        [SerializeField] private RectTransform itemParent = null;
        [SerializeField] private RectTransform content = null;

        private MonoObjPool<AnswerItem> answerItemPool = null;

        public void Init()
        {
            answerItemPool = new MonoObjPool<AnswerItem>(new InstantiateObjectFactory<AnswerItem>(answerItemPrefab, itemParent), new ObjPoolData(itemParent, 5));
        }

        public void SetUp(List<AnswerItem.AnswerItemData> answerItemDatas)
        {
            for (int i = 0; i < answerItemDatas.Count; ++i)
            {
                var answerItem = answerItemPool.Pop();
                answerItem.transform.SetParent(content);
                answerItem.SetUp(answerItemDatas[i]);
            }
        }

        private void OnDisable()
        {
            answerItemPool.ReturnAllObjectsToPool();
        }
    }
}