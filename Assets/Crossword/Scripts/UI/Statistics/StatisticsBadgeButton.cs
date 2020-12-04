using UnityEngine;
using UnityEngine.UI;
using System;
using HealingJam.Crossword.Save;

namespace HealingJam.Crossword.UI
{
    public class StatisticsBadgeButton : MonoBehaviour
    {
        [SerializeField] private Image badgeOriginImage = null;
        [SerializeField] private Image badgeGrayscaleImage = null;
        [SerializeField] private Text stageText = null;

        int badgeIndex;
        private bool buttonClickable = false;
        public Action<int> ButtonClickAction { get; set; }

        public void SetUp(int badgeIndex)
        {
            this.badgeIndex = badgeIndex;

            int completedLevelCount = 0;
            
            for (int i = 0; i < CrosswordMapManager.BADGE_IN_LEVEL_COUNT; ++i)
            {
                int levelIndex = badgeIndex * CrosswordMapManager.BADGE_IN_LEVEL_COUNT + i;

                LevelData levelData = SaveMgr.Instance.GetLevelData(levelIndex);

                if (levelData.completed)
                    completedLevelCount++;
                else break;
            }

            bool badgeCompleted = completedLevelCount == CrosswordMapManager.BADGE_IN_LEVEL_COUNT;
            buttonClickable = completedLevelCount > 0;

            Sprite badgeSprite = CrosswordMapManager.Instance.GetBadgeSpriteToBadgeIndex(badgeIndex);
            badgeOriginImage.sprite = badgeSprite;

            badgeGrayscaleImage.gameObject.SetActive(badgeCompleted == false);

            if (badgeCompleted == false)
            {
                badgeGrayscaleImage.sprite = badgeSprite;
                badgeGrayscaleImage.fillAmount = 1f - (completedLevelCount / (float)CrosswordMapManager.BADGE_IN_LEVEL_COUNT);
            }

            string minPackIndex = (badgeIndex * CrosswordMapManager.BADGE_IN_LEVEL_COUNT + 1).ToString();
            string maxPackIndex = (badgeIndex * CrosswordMapManager.BADGE_IN_LEVEL_COUNT + CrosswordMapManager.BADGE_IN_LEVEL_COUNT).ToString();
            stageText.text = string.Format("{0}~{1}", minPackIndex, maxPackIndex);
        }

        public void OnButtonClick()
        {
            if (buttonClickable)
            {
                ButtonClickAction?.Invoke(badgeIndex);
            }
        }

    }
}