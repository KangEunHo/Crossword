using UnityEngine;
using UnityEngine.UI;
using System;
using HealingJam.Crossword.Save;

namespace HealingJam.Crossword.UI
{
    public class StageSelectButtonController : MonoBehaviour
    {
        [SerializeField] private StageSelectButton[] stageSelectButtons = null;
        [SerializeField] private Image badgeImage = null;
        [SerializeField] private Text badgeLevelText = null;
        [SerializeField] private Text guegeLevelText = null;
        [SerializeField] private GameObject badgeGaugeObject = null;
        [SerializeField] private Image badgeGaugeImage = null;
        [SerializeField] private Text pageText = null;
        [SerializeField] private Swiper pageSwiper = null;

        public Action<int> onClickAction = null;

        private int page;

        public int SavedPage { get { return PlayerPrefs.GetInt("stage_page", 0); }
            set
            {
                PlayerPrefs.SetInt("stage_page", value);
            }
        }

        public int MaxPage { get { return Mathf.CeilToInt(CrosswordMapManager.Instance.MaxStage() / (float)CrosswordMapManager.LEVEL_IN_PACK_COUNT); } }

        private void Awake()
        {
            page = SavedPage;

            foreach (var v in stageSelectButtons)
            {
                v.onClickAction = OnStageSelectButtonClick;
            }

            pageSwiper.leftSwipeAction = SetNextPage;
            pageSwiper.rightSwipeAction = SetPrevPage;
        }

        private void OnEnable()
        {
            SetPage(page);
        }

        private void OnDestroy()
        {
            SavedPage = page;
        }

        private void SetPage(int page)
        {
            int completeCount = 0;
            for (int i = 0; i < CrosswordMapManager.LEVEL_IN_PACK_COUNT; ++i)
            {
                int packIndex = page * CrosswordMapManager.LEVEL_IN_PACK_COUNT + i;
                stageSelectButtons[i].SetUp(packIndex);

                if (SaveMgr.Instance.GetCompleteData(packIndex))
                { 
                    completeCount++;
                }
            }

            bool pageCompleted = completeCount == CrosswordMapManager.LEVEL_IN_PACK_COUNT;

            badgeImage.transform.parent.gameObject.SetActive(pageCompleted);
            badgeGaugeObject.SetActive(pageCompleted == false);
            if (pageCompleted == false)
            {
                float pageProgress = completeCount / (float)CrosswordMapManager.LEVEL_IN_PACK_COUNT * 0.8f;
                badgeGaugeImage.fillAmount = 0.1f + pageProgress;
                guegeLevelText.text = (page +1).ToString();
            }
            else
            {
                badgeImage.sprite = CrosswordMapManager.Instance.GetBadgeSpriteToLevelIndex(page);
                badgeLevelText.text = (page +1).ToString();
            }

            pageText.text = (page + 1).ToString() + "/" + MaxPage;

            CrosswordMapManager.Instance.ActiveLevelIndex = page;
        }

        private bool IsValidPage(int page)
        {
            return page >= 0 && page < MaxPage;
        }

        private void OnStageSelectButtonClick(int stageIndex)
        {
            onClickAction?.Invoke(stageIndex);
        }

        public void SetNextPage()
        {
            if (IsValidPage(page +1))
            {
                SetPage(++page);
            }
        }

        public void SetPrevPage()
        {
            if (IsValidPage(page - 1))
            {
                SetPage(--page);
            }
        }

        public int GetCurrentPage()
        {
            return page;
        }
    }
}