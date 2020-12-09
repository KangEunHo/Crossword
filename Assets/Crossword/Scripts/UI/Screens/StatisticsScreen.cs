using UnityEngine;
using HealingJam.GameScreens;
using HealingJam.Popups;

namespace HealingJam.Crossword.UI
{
    public class StatisticsScreen : FadeAndScaleTweenScreen
    {
        [SerializeField] private StatisticsController statisticsController = null;
        [SerializeField] private StatisticsBadgeButtonController statisticsBadgeButtonController = null;
        [SerializeField] private GameObject titleText = null;

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
            statisticsController.PlayAnimation(0);

            GameMgr.Instance.topUIController.SetActiveCoinButton(false);
            titleText.SetActive(true);
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
        }
    }
}