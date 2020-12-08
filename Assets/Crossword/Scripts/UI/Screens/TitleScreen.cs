using UnityEngine;
using UnityEngine.UI;
using HealingJam.GameScreens;
using HealingJam.Popups;
using HealingJam.Crossword.Save;
using System.Collections;
using System;

namespace HealingJam.Crossword.UI
{
    public class TitleScreen : FadeAndScaleTweenScreen
    {
        [SerializeField] private DailyCommonsenseLoader dailyCommonsenseLoader = null;
        [SerializeField] private Image badgeImage = null;
        [SerializeField] private Text badgeLevelText = null;

        [SerializeField] private GameObject loadingContent = null;
        [SerializeField] private ScaleTweenAnimation buttonContent = null;

        public bool LoadedServerContents { get; private set; } = false;
        private bool showedDailyCommonsense = false;

        public override void Enter(params object[] args)
        {
            base.Enter(args);

            if (LoadedServerContents)
            {
                OnEnterContent();
            }
            else
            {
                StartCoroutine(LoadServerContents(OnEnterContent));
            }
        }

        private IEnumerator LoadServerContents(Action loadAction)
        {
            loadingContent.SetActive(true);
            buttonContent.gameObject.SetActive(false);

            yield return StartCoroutine(CrosswordMapManager.Instance.LoadCrosswordMapAtAssetBundle());

            CrosswordMapManager.Instance.SetUpDatabase();

            DailyCommonsensePopup dailyCommonsensePopup = PopupMgr.Instance.GetPopupById(Popup.PopupID.DailyCommonSense) as DailyCommonsensePopup;

            yield return StartCoroutine(dailyCommonsensePopup.LoadCommonSenseAsync());
            loadingContent.SetActive(false);

            LoadedServerContents = true;
            buttonContent.gameObject.SetActive(true);
            buttonContent.Play();

            loadAction?.Invoke();
        }

        private void OnEnterContent()
        {
            if (showedDailyCommonsense == false)
            {
                if (DailyCommonsensePopup.READY_TO_DAY_COMMONSENSE_AND_NOT_GET_TODAY_REWARD)
                {
                    CoroutineHelper.RunAfterDelay(0.5f, ShowDailyCommonsense);
                }
                showedDailyCommonsense = true;
            }

            int unlockLevel = SaveMgr.Instance.GetUnlockLevel();
            badgeImage.gameObject.SetActive(unlockLevel > 0);
            badgeImage.sprite = unlockLevel == 0 ? null : CrosswordMapManager.Instance.GetBadgeSpriteToLevelIndex(unlockLevel - 1);
            badgeLevelText.text = unlockLevel.ToString();
        }

        private void ShowDailyCommonsense()
        {
            dailyCommonsenseLoader.Show();
        }

        public void OnGameStartButtonClick()
        {
            ScreenMgr.Instance.ChangeState(ScreenID.StageSelect);
        }

        public void OnStatisticsButtonClick()
        {
            ScreenMgr.Instance.ChangeState(ScreenID.Statistics);
        }

        public void OnDailyCommonsenseButtonClick()
        {
            ShowDailyCommonsense();
        }

        public void OnWordMatchingGameStart()
        {
            GameScreen playScreen = ResourceLoader.LoadAndInstaniate<GameScreen>("Prefabs/Word Matching Play Canvas", ScreenMgr.Instance.transform);
            ScreenMgr.Instance.RegisterState(playScreen);
            ScreenMgr.Instance.ChangeState(ScreenID.WordMatchingPlay, WordMatchingPlayScreen.GameMode.AbilityTest);
        }

        public void OnOptionButtonClick()
        {
            if (LoadedServerContents)
            PopupMgr.Instance.EnterWithAnimation(Popup.PopupID.Option, new MoveTweenPopupAnimation(MoveTweenPopupAnimation.MoveDirection.BottonToCenter, 0.25f));
        }
    }
}