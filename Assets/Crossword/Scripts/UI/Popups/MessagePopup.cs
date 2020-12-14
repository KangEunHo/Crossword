using UnityEngine;
using UnityEngine.UI;
using HealingJam.Popups;

namespace HealingJam.Crossword
{
    public class MessagePopup : CallbackPopup
    {
        [SerializeField] private Text mentText = null;

        public override void Enter(params object[] args)
        {
            base.Enter(args);

            if (args != null && args.Length > 1)
            {
                string ment = args[1] as string;
                mentText.text = ment;
            }
        }

        public void OnContinueButtonClick()
        {
            PopupMgr.Instance.ExitWithAnimation(popupID);
            SoundMgr.Instance.PlayOneShotButtonSound();
        }
    }
}