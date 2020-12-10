using UnityEngine;
using UnityEngine.UI;
using HealingJam.GameScreens;
using HealingJam.Popups;
using HealingJam.Crossword.Save;

namespace HealingJam.Crossword.UI
{
    public class TitleScreen : FadeAndScaleTweenScreen
    {
        [SerializeField] private DailyCommonsenseLoader dailyCommonsenseLoader = null;
        [SerializeField] private GameObject title = null;
        [SerializeField] private Image badgeImage = null;
        [SerializeField] private Text badgeLevelText = null;
        [SerializeField] private GameObject commonSenseTestADImage = null;

        private bool showedDailyCommonsense = false;

        public override void Enter(params object[] args)
        {
            base.Enter(args);

            if (showedDailyCommonsense == false)
                title.AddComponent(typeof(ScaleTweenAnimation));

            OnEnterContent();

            commonSenseTestADImage.SetActive(SaveMgr.Instance.GetPlayedCommonSenseTest());
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

        public override void Escape()
        {
            PopupMgr.Instance.EnterWithAnimation(Popup.PopupID.MainBack, new MoveTweenPopupAnimation(MoveTweenPopupAnimation.MoveDirection.BottonToCenter, 0.25f));
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

        public void OnWordMatchingGameButtonClick()
        {
            if (SaveMgr.Instance.GetPlayedCommonSenseTest())
            {
                GoogleMobileAdsMgr.Instance.ShowRewardedAd(ChangeWordMatchingGameScreen);
            }
            else
            {
                ChangeWordMatchingGameScreen();
                SaveMgr.Instance.SetPlayedCommonSenseTest(true);
            }
        }

        public void OnOptionButtonClick()
        {
            PopupMgr.Instance.EnterWithAnimation(Popup.PopupID.Option, new MoveTweenPopupAnimation(MoveTweenPopupAnimation.MoveDirection.BottonToCenter, 0.25f));
        }

        private void ChangeWordMatchingGameScreen()
        {
            GameScreen playScreen = ResourceLoader.LoadAndInstaniate<GameScreen>("Prefabs/Word Matching Play Canvas", ScreenMgr.Instance.transform);
            ScreenMgr.Instance.RegisterState(playScreen);
            ScreenMgr.Instance.ChangeState(ScreenID.WordMatchingPlay, WordMatchingPlayScreen.GameMode.AbilityTest);
        }
    }
}