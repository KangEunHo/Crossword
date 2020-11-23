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

            List<CrosswordMap> crosswordMaps = new List<CrosswordMap>()
            {
                CrosswordMapManager.Instance.GetCrosswordMap(0),
                CrosswordMapManager.Instance.GetCrosswordMap(1),
                CrosswordMapManager.Instance.GetCrosswordMap(2),
                CrosswordMapManager.Instance.GetCrosswordMap(3)
            };


            gameController.SetUp(crosswordMaps.ToArray());
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