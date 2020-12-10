using UnityEngine;
using HealingJam.GameScreens;
using HealingJam.Popups;
using HealingJam.Crossword.Save;
namespace HealingJam.Crossword.UI
{
    public class ClearScreen : FadeAndScaleTweenScreen
    {
        [SerializeField] private ClearUIController clearUIController = null;

        public override void Enter(params object[] args)
        {
            base.Enter(args);

            bool alreadyCompleted = (bool)args[0];
            int addCoinAmount = alreadyCompleted ? 0 : 30;
            clearUIController.SetUp(alreadyCompleted, addCoinAmount);

            GoogleMobileAdsMgr.Instance.ShowDelayInterstitial();
            GameMgr.Instance.topUIController.SetActiveBackButton(false);
            GameMgr.Instance.topUIController.SetActiveOptionButton(false);

            SoundMgr.Instance.PlayOneShot(SoundMgr.Instance.playSuccess);
        }

        public override void Exit(params object[] args)
        {
            base.Exit(args);

            GameMgr.Instance.topUIController.SetActiveBackButton(true);
            GameMgr.Instance.topUIController.SetActiveOptionButton(true);
        }

        public override void Escape()
        {
            OnExitButtonClick();
        }

        public void OnExitButtonClick()
        {
            ScreenMgr.Instance.ChangeState(ScreenID.StageSelect);
            SoundMgr.Instance.PlayOneShotButtonSound();
        }

        public void OnNextLevelButtonClick()
        {
            CrosswordMapManager.Instance.ActivePackIndex++;

            GameScreen playScreen = ResourceLoader.LoadAndInstaniate<GameScreen>("Prefabs/Play Screen", ScreenMgr.Instance.transform);
            ScreenMgr.Instance.RegisterState(playScreen);
            ScreenMgr.Instance.ChangeState(ScreenID.Play);
            SoundMgr.Instance.PlayOneShotButtonSound();
        }
    }
}