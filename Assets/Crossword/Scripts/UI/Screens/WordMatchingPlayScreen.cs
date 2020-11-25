using UnityEngine;
using HealingJam.GameScreens;
using HealingJam.Popups;
using System.Collections.Generic;

namespace HealingJam.Crossword.UI
{
    public class WordMatchingPlayScreen : FadeAndScaleTweenScreen
    {
        [SerializeField] private WordMatchingGameController gameController = null;

        public override void Enter(params object[] args)
        {
            base.Enter(args);

            List<WordDataForGame> answers = new List<WordDataForGame>();
            int maxStage = CrosswordMapManager.Instance.MaxStage();
            List<int> useMapIndex = new List<int>(maxStage);
            for (int i = 0; i < maxStage; ++i)
            {
                useMapIndex.Add(i);
            }

            useMapIndex.Shuffle();

            for (int i = 0; i < maxStage && i < 20; ++i)
            {
                answers.Add(CrosswordMapManager.Instance.GetCrosswordMap(useMapIndex[i]).wordDatas.RandomValue());
            }
            gameController.SetUp(answers);
        }

        protected override void OnExitFadeComplete(params object[] args)
        {
            ScreenMgr.Instance.UnRegisterState(this);
        }

        public override void Escape()
        {
            ScreenMgr.Instance.ChangeState(ScreenID.StageSelect);
        }

        public void OnCoinButtonClick()
        {
            PopupMgr.Instance.EnterWithAnimation(Popup.PopupID.Shop, new MoveTweenPopupAnimation(MoveTweenPopupAnimation.MoveDirection.BottonToCenter, 0.25f));
        }

        public void OnOptionButtonClick()
        {
            PopupMgr.Instance.EnterWithAnimation(Popup.PopupID.Option, new MoveTweenPopupAnimation(MoveTweenPopupAnimation.MoveDirection.BottonToCenter, 0.25f));
        }
    }
}