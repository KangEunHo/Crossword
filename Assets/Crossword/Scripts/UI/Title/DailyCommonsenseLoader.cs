using UnityEngine;
using System.Collections.Generic;

using HealingJam.Popups;

namespace HealingJam.Crossword
{
    public class DailyCommonsenseLoader : MonoBehaviour
    {
        public bool Showed { get; private set; } = false;

        public void Show()
        {
            Showed = true;

            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                PopupMgr.Instance.EnterWithAnimation(Popup.PopupID.InternetConnectionWarning,
                new MoveTweenPopupAnimation(MoveTweenPopupAnimation.MoveDirection.BottonToCenter, 0.5f));
            }
            else
            {
                PopupMgr.Instance.EnterWithAnimation(Popup.PopupID.DailyCommonSense,
                new MoveTweenPopupAnimation(MoveTweenPopupAnimation.MoveDirection.BottonToCenter, 0.5f));
            }
        }

        public void OnDailyCommonsenseButtonClick()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                PopupMgr.Instance.EnterWithAnimation(Popup.PopupID.InternetConnectionWarning,
                new MoveTweenPopupAnimation(MoveTweenPopupAnimation.MoveDirection.BottonToCenter, 0.5f));
            }
            else
            {
                PopupMgr.Instance.EnterWithAnimation(Popup.PopupID.DailyCommonSense,
                new MoveTweenPopupAnimation(MoveTweenPopupAnimation.MoveDirection.BottonToCenter, 0.5f));
            }
        }
    }
}