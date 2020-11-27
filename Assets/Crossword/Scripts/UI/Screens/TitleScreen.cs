﻿using UnityEngine;
using HealingJam.GameScreens;
using HealingJam.Popups;

namespace HealingJam.Crossword.UI
{
    public class TitleScreen : FadeAndScaleTweenScreen
    {
        [SerializeField] private DailyCommonsenseLoader dailyCommonsenseLoader = null;

        public override void Enter(params object[] args)
        {
            base.Enter(args);
            
            if (dailyCommonsenseLoader.Showed == false)
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