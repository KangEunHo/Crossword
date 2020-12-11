using UnityEngine;
using UnityEngine.UI;
using HealingJam.Popups;

namespace HealingJam.Crossword
{
    public class ModifyReviewPopup : Popup
    {
        [SerializeField] private Text reviewText = null;

        public override void Enter(params object[] args)
        {
            base.Enter(args);

            if (args != null && args.Length > 1)
            {
                int packIndex = (int)args[1] + 1;
                reviewText.text = string.Format("현재 <color=red>{0}번</color> 가로세로를 풀고 있습니다.\n<color=red>오타 및 수정사항</color>이 있으신가요?", packIndex);
            }
        }

        public void OnExitButtonClick()
        {
            Escape();
            SoundMgr.Instance.PlayOneShotButtonSound();
        }

        public void OnReviewButtonClick()
        {
            InAppReviewMgr.Instance.LunchReview();
            SoundMgr.Instance.PlayOneShotButtonSound();
        }
    }
}