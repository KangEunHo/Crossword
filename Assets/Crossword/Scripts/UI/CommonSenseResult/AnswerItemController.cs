using UnityEngine;
using System.Collections.Generic;
using HealingJam.Popups;

namespace HealingJam.Crossword.UI
{
    public class AnswerItemController : MonoBehaviour
    {
        [SerializeField] private AnswerItem answerItemPrefab = null;
        [SerializeField] private RectTransform itemParent = null;
        [SerializeField] private RectTransform content = null;

        private MonoObjPool<AnswerItem> answerItemPool = null;

        List<AnswerItem.AnswerItemData> answerItemDatas = null;

        public void Init()
        {
            answerItemPool = new MonoObjPool<AnswerItem>(new InstantiateObjectFactory<AnswerItem>(answerItemPrefab, itemParent), new ObjPoolData(itemParent, 5));
        }

        public void SetUp(List<AnswerItem.AnswerItemData> answerItemDatas)
        {
            this.answerItemDatas = answerItemDatas;
            content.anchoredPosition = Vector2.zero;
            for (int i = 0; i < answerItemDatas.Count; ++i)
            {
                var answerItem = answerItemPool.Pop();
                answerItem.transform.SetParent(content);
                answerItem.SetUp(i, answerItemDatas[i]);
                answerItem.PlusButtonClickAction = OnAnswerPlusButtonClick;
            }
        }

        private void OnDisable()
        {
            answerItemPool.ReturnAllObjectsToPool();
        }

        public void OnAnswerPlusButtonClick(int index)
        {
            PopupMgr.Instance.EnterWithAnimation(Popup.PopupID.WordData, new MoveTweenPopupAnimation(MoveTweenPopupAnimation.MoveDirection.BottonToCenter, 0.25f),
                index, answerItemDatas);

            SoundMgr.Instance.PlayOneShotButtonSound();
        }
    }
}