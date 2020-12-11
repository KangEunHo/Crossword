using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace HealingJam.Crossword.UI
{
    public class StatisticsBadgeButtonController : MonoBehaviour
    {
        [SerializeField] private StatisticsBadgeButton statisticsBadgeButtonPrefab = null;
        [SerializeField] private RectTransform contentRT = null;
        [SerializeField] private Transform badgeEffect = null;
        [SerializeField] private StatisticsController statisticsController = null;
        [SerializeField] private ScrollRect scrollRect = null;

        private List<StatisticsBadgeButton> statisticsBadgeButtons = new List<StatisticsBadgeButton>();

        public void Init()
        {
            //int vaildBadgeIndex1 = Mathf.CeilToInt(CrosswordMapManager.Instance.MaxStage() / (float)(CrosswordMapManager.LEVEL_IN_PACK_COUNT * CrosswordMapManager.BADGE_IN_LEVEL_COUNT));
            //int vaildBadgeIndex2 = CrosswordMapManager.Instance.BadgeSpriteLength - 1;
            //int maxBadgeIndex = Mathf.Min(vaildBadgeIndex1, vaildBadgeIndex2);
            int maxBadgeIndex = CrosswordMapManager.Instance.BadgeSpriteLength;

            for (int i = 0; i < maxBadgeIndex; ++i)
            {
                StatisticsBadgeButton statisticsBadgeButton = Instantiate(statisticsBadgeButtonPrefab, contentRT);
                statisticsBadgeButton.ButtonClickAction = OnBadgeButtonClick;
                statisticsBadgeButtons.Add(statisticsBadgeButton);
            }
        }

        public void SetUp()
        {
            for (int i = 0; i < statisticsBadgeButtons.Count; ++i)
            {
                statisticsBadgeButtons[i].SetUp(i);
            }
        }

        public void OnBadgeButtonClick(int index)
        {
            statisticsController.PlayAnimation(index);
            SoundMgr.Instance.PlayOneShotButtonSound();
            SetPositionButtonEffect(index);
        }

        public void SetPositionButtonEffect(int index)
        {
            badgeEffect.SetParent(statisticsBadgeButtons[index].transform, false);
            badgeEffect.SetAsFirstSibling();
        }

        public void SetContentPositionToChild(int index)
        {
            scrollRect.SnapTo(statisticsBadgeButtons[index].transform as RectTransform);
        }
    }
}