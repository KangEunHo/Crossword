using UnityEngine;
using HealingJam.GameScreens;
using HealingJam.Popups;

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

        private void OnStageSelectButtonClick(int stageIndex)
        {
            if (stageIndex < CrosswordMapManager.Instance.MaxStage())
            {
                CrosswordMapManager.Instance.ActiveStageIndex = stageIndex;

                GameScreen playScreen = ResourceLoader.LoadAndInstaniate<GameScreen>("Prefabs/Play Screen", ScreenMgr.Instance.transform);
                ScreenMgr.Instance.RegisterState(playScreen);
                ScreenMgr.Instance.ChangeState(ScreenID.Play);
            }
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