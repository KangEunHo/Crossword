﻿using UnityEngine;
using UnityEngine.UI;
using HealingJam.GameScreens;
using HealingJam.Popups;
using HealingJam.Crossword.Save;

namespace HealingJam.Crossword.UI
{
    public class TitleScreen : FadeAndScaleTweenScreen
    {
        public bool showedDailyCommonsense = false;

        [SerializeField] private DailyCommonsenseLoader dailyCommonsenseLoader = null;
        [SerializeField] private Image badgeImage = null;
        [SerializeField] private Text badgeLevelText = null;

        public override void Enter(params object[] args)
        {
            base.Enter(args);
            
            if (showedDailyCommonsense == false)
            {
                if (DailyCommonsensePopup.ReadyTodayCommonsense && DailyCommonsensePopup.GET_TODAY_REWARDED == false)
                {
                    CoroutineHelper.RunAfterDelay(0.5f, ShowDailyCommonsense);
                }
                showedDailyCommonsense = true;
            }

            int unlockLevel = SaveMgr.Instance.GetUnlockLevel();
            badgeImage.gameObject.SetActive(unlockLevel > 0);
            badgeImage.sprite = unlockLevel == 0 ? null : CrosswordMapManager.Instance.GetBadgeSpriteToLevelIndex(unlockLevel -1);
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
            PopupMgr.Instance.EnterWithAnimation(Popup.PopupID.Option, new MoveTweenPopupAnimation(MoveTweenPopupAnimation.MoveDirection.BottonToCenter, 0.25f));
        }
    }
}