using UnityEngine;
using HealingJam.GameScreens;
using HealingJam.Popups;

namespace HealingJam.Crossword.UI
{
    public class StatisticsScreen : FadeAndScaleTweenScreen
    {
        [SerializeField] StatisticsController statisticsController = null;

        public override void Enter(params object[] args)
        {
            base.Enter(args);

            statisticsController.SetUp(0, 5);
        }

        public override void Escape()
        {
            ScreenMgr.Instance.ChangeState(ScreenID.Title);
        }
    }
}