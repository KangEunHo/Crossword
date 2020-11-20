using UnityEngine;
using System.Collections;

namespace HealingJam.Crossword.UI
{
    public class StageSelectButtonController : MonoBehaviour
    {
        [SerializeField] private StageSelectButton[] stageSelectButtons = null;

        private int page;
        public int SavedPage { get { return PlayerPrefs.GetInt("stage_page"); }
            set
            {
                PlayerPrefs.SetInt("stage_page", value);
            }
        }

        private void Start()
        {
            page = SavedPage;

            foreach(var v in stageSelectButtons)
            {
                v.onClickAction = OnStageSelectButtonClick;
            }

            SetPage(0);
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
        }

        private bool IsValidPage(int page)
        {
            return page >= 0 && page < CrosswordMapManager.Instance.MaxStage();
        }

        private void OnStageSelectButtonClick(int stageIndex)
        {
            if (stageIndex < CrosswordMapManager.Instance.MaxStage())
            {
                CrosswordMapManager.Instance.ActiveStageIndex = stageIndex;
                CrosswordMapManager.Instance.ActiveCrosswordMap = CrosswordMapManager.Instance.GetCrosswordMap(stageIndex);

                HealingJam.GameScreens.ScreenMgr.Instance.Exit(GameScreens.GameScreen.ScreenID.StageSelect);
                UnityEngine.SceneManagement.SceneManager.LoadScene("Play");
            }
        }

        public void OnNextButtonClick()
        {
            if (IsValidPage(page +1))
            {
                SetPage(++page);
            }
        }

        public void OnPrevButtonClick()
        {
            if (IsValidPage(page - 1))
            {
                SetPage(--page);
            }
        }
    }
}