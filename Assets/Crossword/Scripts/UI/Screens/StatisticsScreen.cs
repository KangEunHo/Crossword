using UnityEngine;
using HealingJam.GameScreens;
using HealingJam.Popups;

namespace HealingJam.Crossword.UI
{
    public class StatisticsScreen : FadeAndScaleTweenScreen
    {
        [SerializeField] private StatisticsController statisticsController = null;
        [SerializeField] private StatisticsBadgeButtonController statisticsBadgeButtonController = null;

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
        }

        public override void Escape()
        {
            ScreenMgr.Instance.ChangeState(ScreenID.Title);
        }
    }
}