using UnityEngine;
using HealingJam.GameScreens;
using HealingJam.Popups;

namespace HealingJam.Crossword.UI
{
    public class ClearScreen : FadeAndScaleTweenScreen
    {
        [SerializeField] private GameObject nextStageButton = null;

        public override void Enter(params object[] args)
        {
            base.Enter(args);
            bool packLastStage = CrosswordMapManager.Instance.ActiveStageIndex > 0 && (CrosswordMapManager.Instance.ActiveStageIndex % CrosswordMapManager.LEVEL_IN_PACK_COUNT) == 0;
            nextStageButton.SetActive(packLastStage == false);
        }

        public override void Escape()
        {
            ScreenMgr.Instance.ChangeState(ScreenID.StageSelect);
        }

        public void OnCoinButtonClick()
        {
            PopupMgr.Instance.EnterWithAnimation(Popup.PopupID.Shop, new MoveTweenPopupAnimation(MoveTweenPopupAnimation.MoveDirection.BottonToCenter, 0.25f));
        }

        public void OnExitButtonClick()
        {
            ScreenMgr.Instance.ChangeState(ScreenID.StageSelect);
        }

        public void OnNextLevelButtonClick()
        {
            CrosswordMapManager.Instance.ActiveStageIndex++;

            GameScreen playScreen = ResourceLoader.LoadAndInstaniate<GameScreen>("Prefabs/Play Screen", ScreenMgr.Instance.transform);
            ScreenMgr.Instance.RegisterState(playScreen);
            ScreenMgr.Instance.ChangeState(ScreenID.Play);
        }
    }
}