using UnityEngine;
using System.Collections.Generic;

using HealingJam.Popups;

namespace HealingJam.Crossword
{
    public class DailyCommonsenseLoader : MonoBehaviour
    {
        public void Show()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                PopupMgr.Instance.EnterWithAnimation(Popup.PopupID.InternetConnectionWarning,
                new MoveTweenPopupAnimation(MoveTweenPopupAnimation.MoveDirection.BottonToCenter, 0.5f));
            }
            else
            {
                DailyCommonsensePopup dailyCommonsensePopup = PopupMgr.Instance.GetPopupById(Popup.PopupID.DailyCommonSense) as DailyCommonsensePopup;

                if (dailyCommonsensePopup.IsLoaded)
                {
                    PopupMgr.Instance.EnterWithAnimation(Popup.PopupID.DailyCommonSense,
                    new MoveTweenPopupAnimation(MoveTweenPopupAnimation.MoveDirection.BottonToCenter, 0.5f));
                }
                else
                {
                    CoroutineHelper.StartStaticCoroutine(dailyCommonsensePopup.LoadCommonSenseAsync());
                }
            }
        }
    }
}