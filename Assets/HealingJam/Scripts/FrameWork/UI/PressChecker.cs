using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace HealingJam
{
    public class PressChecker : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        #region Properties

        public bool IsPressing { get; private set; }
        public Action OnPointerUp = null;
        public Action OnPointerDown = null;

        #endregion

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            IsPressing = true;
            OnPointerDown?.Invoke();
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            IsPressing = false;
            OnPointerUp?.Invoke();
        }

    }
}