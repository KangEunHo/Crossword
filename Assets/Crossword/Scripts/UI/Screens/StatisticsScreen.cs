using UnityEngine;
using HealingJam.GameScreens;
using HealingJam.Popups;

namespace HealingJam.Crossword.UI
{
    public class StatisticsScreen : FadeAndScaleTweenScreen
    {
        private const string STATISTICS_FIST_VISIT_KEY = "statisticsFisrtVisit";

        [SerializeField] private StatisticsController statisticsController = null;
        [SerializeField] private StatisticsBadgeButtonController statisticsBadgeButtonController = null;
        [SerializeField] private GameObject titleText = null;
        [SerializeField] private GameObject howto = null;

        private bool isInited = false;

        public override void Enter(params object[] args)
        {
            base.Enter(args);

            if (isInited == false)
            {
                isInited = true;
                statisticsBadgeButtonController.Init();
            }
            statisticsBadgeButtonController.SetUp();

            int unlockCompleteLevel = Save.SaveMgr.Instance.GetUnlockLevel();

            if (unlockCompleteLevel == 0)
            {
                statisticsController.PlayAnimationAll();
                statisticsBadgeButtonController.SetPositionButtonEffectAtAllButton();
            }
            else
            {
                int badgeIndex = Mathf.FloorToInt((Save.SaveMgr.Instance.GetUnlockLevel() -1) / (float)CrosswordMapManager.BADGE_IN_LEVEL_COUNT);
                statisticsController.PlayAnimation(badgeIndex);
                statisticsBadgeButtonController.SetPositionButtonEffect(badgeIndex);
                statisticsBadgeButtonController.SetContentPositionToChild(badgeIndex);
            }
            GameMgr.Instance.topUIController.SetActiveCoinButton(false);
            titleText.SetActive(true);

            if (PlayerPrefsDatas.GetBoolData(STATISTICS_FIST_VISIT_KEY, 1))
            {
                howto.SetActive(true);
                PlayerPrefsDatas.SetBoolData(STATISTICS_FIST_VISIT_KEY, false);
            }
            else
            {
                howto.SetActive(false);
            }
        }

        public void HideHowTo()
        {
            howto.SetActive(false);
        }

        public override void Exit(params object[] args)
        {
            base.Exit(args);

            GameMgr.Instance.topUIController.SetActiveCoinButton(true);
            titleText.SetActive(false);
        }

        public override void Escape()
        {
            ScreenMgr.Instance.ChangeState(ScreenID.Title);
            SoundMgr.Instance.PlayOneShotButtonSound();
        }

        public void OnHowToButtonClick()
        {
            howto.SetActive(true);

            SoundMgr.Instance.PlayOneShotButtonSound();
        }
    }
}