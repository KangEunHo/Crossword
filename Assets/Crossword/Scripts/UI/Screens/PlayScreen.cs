using UnityEngine;
using HealingJam.GameScreens;
using HealingJam.Popups;

namespace HealingJam.Crossword.UI
{
    public class PlayScreen : FadeAndScaleTweenScreen
    {
        [SerializeField] private GameController gameController = null;

        protected override void OnExitFadeComplete(params object[] args)
        {
            ScreenMgr.Instance.UnRegisterState(this);
        }

        public override void Escape()
        {
            OnBackButtonClick();
        }

        public void OnCoinButtonClick()
        {
            if (gameController.State == GameController.GameState.Play)
            {
                gameController.State = GameController.GameState.Pause;

                PopupMgr.Instance.EnterWithAnimation(Popup.PopupID.Shop, new MoveTweenPopupAnimation(MoveTweenPopupAnimation.MoveDirection.BottonToCenter, 0.25f),
                    new PopupClosedDelegate(OnPopupClosed));
            }
        }

        public void OnOptionButtonClick()
        {
            if (gameController.State == GameController.GameState.Play)
            {
                gameController.State = GameController.GameState.Pause;

                PopupMgr.Instance.EnterWithAnimation(Popup.PopupID.Option, new MoveTweenPopupAnimation(MoveTweenPopupAnimation.MoveDirection.BottonToCenter, 0.25f),
                    new PopupClosedDelegate(OnPopupClosed));
            }
        }

        public void OnBackButtonClick()
        {
            if (gameController.State == GameController.GameState.Play)
            {
                gameController.State = GameController.GameState.Pause;

                PopupMgr.Instance.EnterWithAnimation(Popup.PopupID.PlayExit, new MoveTweenPopupAnimation(MoveTweenPopupAnimation.MoveDirection.BottonToCenter, 0.25f),
                    new PopupClosedDelegate(OnPlayExitPopupClosed));
            }
        }

        private void OnPopupClosed(string message)
        {
            if (gameController.State == GameController.GameState.Pause)
            {
                gameController.State = GameController.GameState.Play;
            }
        }

        private void OnPlayExitPopupClosed(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                gameController.State = GameController.GameState.Play;
            }
            else
            {
                if (message == "continue")
                {
                    gameController.State = GameController.GameState.Play;
                }
                else if (message == "exit")
                {
                    ScreenMgr.Instance.ChangeState(ScreenID.StageSelect);
                }
            }
        }

    }
}