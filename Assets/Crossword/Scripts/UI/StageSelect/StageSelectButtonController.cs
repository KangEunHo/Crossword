using UnityEngine;
using UnityEngine.UI;
using System;

namespace HealingJam.Crossword.UI
{
    public class StageSelectButtonController : MonoBehaviour
    {
        [SerializeField] private StageSelectButton[] stageSelectButtons = null;
        [SerializeField] private Text pageText = null;

        public Action<int> onClickAction = null;

        private int page;

        public int SavedPage { get { return PlayerPrefs.GetInt("stage_page", 0); }
            set
            {
                PlayerPrefs.SetInt("stage_page", value);
            }
        }

        public int MaxPage { get { return Mathf.CeilToInt(CrosswordMapManager.Instance.MaxStage() / (float)CrosswordMapManager.PACK_IN_STAGE_COUNT); } }

        private void Start()
        {
            page = SavedPage;

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
            for (int i = 0; i < CrosswordMapManager.PACK_IN_STAGE_COUNT; ++i)
            {
                stageSelectButtons[i].SetUp(page * CrosswordMapManager.PACK_IN_STAGE_COUNT + i);
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