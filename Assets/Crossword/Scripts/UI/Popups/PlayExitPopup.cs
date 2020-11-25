using UnityEngine;
using System.Collections;
using HealingJam.Popups;

namespace HealingJam.Crossword
{
    public class PlayExitPopup : CallbackPopup
    {
        public void OnExitButtonClick()
        {
            PopupMgr.Instance.ExitWithAnimation(popupID, "exit");
        }

        public void OnContinueButtonClick()
        {
            PopupMgr.Instance.ExitWithAnimation(popupID, "continue");
        }
    }
}