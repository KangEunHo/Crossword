using UnityEngine;
using UnityEngine.UI;
using HealingJam.Popups;

namespace HealingJam.Crossword
{
    public class MainBackPopup : Popup
    {
        public override void Enter(params object[] args)
        {
            base.Enter(args);
        }


        public void OnExitButtonClick()
        {
            Application.Quit();
        }

        public void OnReviewButtonClick()
        {
            InAppReviewMgr.Instance.LunchReview();
        }
    }
}