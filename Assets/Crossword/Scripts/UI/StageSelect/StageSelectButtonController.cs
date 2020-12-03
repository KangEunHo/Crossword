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
        [SerializeField] private GameObject badgeGaugeObject = null;
        [SerializeField] private Image badgeGaugeImage = null;
        [SerializeField] private Text pageText = null;

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
        }

        private void OnEnable()
        {
            foreach (var v in stageSelectButtons)
            {
                v.onClickAction = OnStageSelectButtonClick;
            }

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
                int index = page * CrosswordMapManager.LEVEL_IN_PACK_COUNT + i;
                stageSelectButtons[i].SetUp(index);

                if (SaveMgr.Instance.GetCompleteData(index))
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
            }
            else
            {
                badgeImage.sprite = CrosswordMapManager.Instance.GetBadgeSprite(page);
                badgeLevelText.text = page.ToString();
            }

            pageText.text = (page + 1).ToString() + "/" + MaxPage;
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
    }
}