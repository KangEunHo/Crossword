using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace HealingJam
{
    public class Swiper : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
    {
        public float variation = 100f;
        public Action leftSwipeAction = null;
        public Action rightSwipeAction = null;

        public void OnPointerDown(PointerEventData eventData)
        {
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            float delta = eventData.pressPosition.x - eventData.position.x;

            if (delta >= variation)
            {
                rightSwipeAction?.Invoke();
            }
            else if (delta <= -variation)
            {
                leftSwipeAction?.Invoke();
            }
        }
    }
}