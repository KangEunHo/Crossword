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
                int packIndex = (int)args[1];
                reviewText.text = "지금 <color=red>팩 {0}</color>을 풀고 있습니다.\n<color=red>오타, 수정</color>해야 할 단어에\n" +
                    "대해 알려주세요.\n바로 고치겠습니다.";
            }
        }

        public void OnExitButtonClick()
        {
            Escape();
        }

        public void OnReviewButtonClick()
        {
            
        }
    }
}