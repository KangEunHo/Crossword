using UnityEngine;
using HealingJam.GameScreens;
using HealingJam.Popups;
using System.Collections.Generic;
using HealingJam.Crossword.Save;

namespace HealingJam.Crossword.UI
{
    public class CommonSenseResultScreen : FadeAndScaleTweenScreen
    {
        [SerializeField] ResultGaugeController resultGaugeController = null;
        [SerializeField] AnswerItemController answerItemController = null;

        private List<RightAnswerCountData> rightAnswerCountDatas = null;
        private List<AnswerItem.AnswerItemData> answerItemDatas = null;

        public override void Init(object stateMachine)
        {
            base.Init(stateMachine);

            answerItemController.Init();
        }

        public override void Enter(params object[] args)
        {
            base.Enter(args);

            answerItemDatas = args[0] as List<AnswerItem.AnswerItemData>;
            rightAnswerCountDatas = args[1] as List<RightAnswerCountData>;

            resultGaugeController.PlayAnimation(rightAnswerCountDatas);
            answerItemController.SetUp(answerItemDatas);
        }

        public override void Escape()
        {
            ScreenMgr.Instance.ChangeState(ScreenID.Title);
        }
    }
}