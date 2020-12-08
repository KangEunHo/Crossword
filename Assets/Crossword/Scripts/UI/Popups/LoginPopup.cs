﻿using UnityEngine;
using UnityEngine.UI;
using HealingJam.Popups;
using HealingJam.Crossword.Save;

namespace HealingJam.Crossword
{
    public class LoginPopup : CallbackPopup
    {
        public override void Escape()
        {

        }

        public void OnGoogleLoginButtonClick()
        {
            SaveMgr.Instance.SetLoginType(SaveData.LoginType.Google);

            PopupMgr.Instance.ExitWithAnimation(popupID);
        }

        public void OnGuestLoginButtonClick()
        {
            SaveMgr.Instance.SetLoginType(SaveData.LoginType.Guest);

            PopupMgr.Instance.ExitWithAnimation(popupID);
        }
    }
}