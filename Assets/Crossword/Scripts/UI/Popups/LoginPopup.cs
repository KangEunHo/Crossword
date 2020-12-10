using UnityEngine;
using UnityEngine.UI;
using HealingJam.Popups;
using HealingJam.Crossword.Save;

namespace HealingJam.Crossword
{
    public class LoginPopup : CallbackPopup
    {
        public const string GOOGLE_CALL_BACK_KEY = "google";
        public const string GUEST_CALL_BACK_KEY = "guest";
        public override void Escape()
        {

        }

        public void OnGoogleLoginButtonClick()
        {
            SaveMgr.Instance.SetLoginType(SaveData.LoginType.Google);

            PopupMgr.Instance.ExitWithAnimation(popupID, GOOGLE_CALL_BACK_KEY);
        }

        public void OnGuestLoginButtonClick()
        {
            SaveMgr.Instance.SetLoginType(SaveData.LoginType.Guest);

            PopupMgr.Instance.ExitWithAnimation(popupID, GUEST_CALL_BACK_KEY);
        }
    }
}