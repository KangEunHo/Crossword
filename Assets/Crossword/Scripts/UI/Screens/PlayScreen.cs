using UnityEngine;
using HealingJam.GameScreens;
using HealingJam.Popups;

namespace HealingJam.Crossword.UI
{
    public class PlayScreen : FadeAndScaleTweenScreen
    {
        [SerializeField] private GameController gameController = null;

        public override void Enter(params object[] args)
        {
            base.Enter(args);

            PopupMgr.Instance.StateMachine.OnStateEntered += OnPopupEntered;
            PopupMgr.Instance.StateMachine.OnStateExited += OnPopupExited;
        }

        public override void Exit(params object[] args)
        {
            base.Exit(args);

            PopupMgr.Instance.StateMachine.OnStateEntered -= OnPopupEntered;
            PopupMgr.Instance.StateMachine.OnStateExited -= OnPopupExited;
        }

        protected override void OnExitFadeComplete(params object[] args)
        {
            ScreenMgr.Instance.UnRegisterState(this);
        }

        public override void Escape()
        {
            OnBackButtonClick();
        }

        public void OnPopupEntered(object sender, StateMachine.StateEventArgs<Popup> stateEventArgs)
        {
            if (gameController.State == GameController.GameState.Play)
            {
                gameController.State = GameController.GameState.Pause;
            }
        }

        public void OnPopupExited(object sender, StateMachine.StateEventArgs<Popup> stateEventArgs)
        {
            if (PopupMgr.Instance.StateMachine.CurrentState() == null)
            {
                if (gameController.State == GameController.GameState.Pause)
                {
                    gameController.State = GameController.GameState.Play;
                }
            }
        }

        public void OnBackButtonClick()
        {
            PopupMgr.Instance.EnterWithAnimation(Popup.PopupID.PlayExit, new MoveTweenPopupAnimation(MoveTweenPopupAnimation.MoveDirection.BottonToCenter, 0.25f),
                new PopupClosedDelegate(OnPlayExitPopupClosed), "나가시겠습니까?\n진행한 내용은 저장됩니다.");

            SoundMgr.Instance.PlayOneShotButtonSound();
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
                    gameController.SaveProgressData();
                    ScreenMgr.Instance.ChangeState(ScreenID.StageSelect);
                }
            }
        }

        public void OnModifyReviewButtonClick()
        {
            PopupMgr.Instance.EnterWithAnimation(Popup.PopupID.ModifyReview, new MoveTweenPopupAnimation(MoveTweenPopupAnimation.MoveDirection.BottonToCenter, 0.25f),
                null, (CrosswordMapManager.Instance.ActivePackIndex));

            SoundMgr.Instance.PlayOneShotButtonSound();
        }

    }
}