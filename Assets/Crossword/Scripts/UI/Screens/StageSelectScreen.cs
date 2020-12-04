﻿using UnityEngine;
using HealingJam.GameScreens;
using HealingJam.Popups;
using HealingJam.Crossword.Save;

namespace HealingJam.Crossword.UI
{
    public class StageSelectScreen : FadeAndScaleTweenScreen
    {
        [SerializeField] private StageSelectButtonController stageSelectButtonController = null;

        public override void Init(object stateMachine)
        {
            base.Init(stateMachine);

            stageSelectButtonController.onClickAction = OnStageSelectButtonClick;
        }

        public override void Enter(params object[] args)
        {
            base.Enter(args);
        }

        private void OnStageSelectButtonClick(int packIndex)
        {
            int level = packIndex / CrosswordMapManager.LEVEL_IN_PACK_COUNT;
            bool unlockLevel = level == 0;

            if (unlockLevel == false)
            {
                LevelData levelData = SaveMgr.Instance.GetLevelData(level -1);
                if (levelData.completed)
                    unlockLevel = true;
            }

            if (unlockLevel)
            {
                if (packIndex < CrosswordMapManager.Instance.MaxStage())
                {
                    CrosswordMapManager.Instance.ActivePackIndex = packIndex;

                    GameScreen playScreen = ResourceLoader.LoadAndInstaniate<GameScreen>("Prefabs/Play Screen", ScreenMgr.Instance.transform);
                    ScreenMgr.Instance.RegisterState(playScreen);
                    ScreenMgr.Instance.ChangeState(ScreenID.Play);
                }
            }
            else
            {
                Debug.Log("이전 레벨을 먼저 클리어하세요");
                // 할것. Toast 이전 레벨을 클리어하세요.
            }
        }

        public void OnBadgeButtonClick()
        {
            CrosswordMapManager.Instance.ActiveLevelIndex = stageSelectButtonController.GetCurrentPage();
            GameScreen playScreen = ResourceLoader.LoadAndInstaniate<GameScreen>("Prefabs/Word Matching Play Canvas", ScreenMgr.Instance.transform);
            ScreenMgr.Instance.RegisterState(playScreen);
            ScreenMgr.Instance.ChangeState(ScreenID.WordMatchingPlay, WordMatchingPlayScreen.GameMode.BadgePlay);
        }

        public void OnNextButtonClick()
        {
            stageSelectButtonController.SetNextPage();
        }

        public void OnPrevButtonClick()
        {
            stageSelectButtonController.SetPrevPage();
        }

        public override void Escape()
        {
            ScreenMgr.Instance.ChangeState(ScreenID.Title);
        }

        public void OnOptionButtonClick()
        {
            PopupMgr.Instance.EnterWithAnimation(Popup.PopupID.Option, new MoveTweenPopupAnimation(MoveTweenPopupAnimation.MoveDirection.BottonToCenter, 0.25f));
        }
    }
}