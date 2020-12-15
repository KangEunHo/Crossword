using UnityEngine;
using System;

namespace HealingJam.Crossword
{
    public class UpdateSwiper : MonoBehaviour
    {
        Vector2 pressPosition;
        public float variation = 100f;
        public Action leftSwipeAction = null;
        public Action rightSwipeAction = null;

        void Update()
        {
            if (Utility.MouseDown())
            {
                pressPosition = Utility.MousePosition();
            }

            if (Utility.MouseUp())
            {
                if (Popups.PopupMgr.Instance.StateMachine.CurrentState() == null)
                {
                    Vector2 pos = Utility.MousePosition();

                    float delta = pressPosition.x - pos.x;

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
    }
}