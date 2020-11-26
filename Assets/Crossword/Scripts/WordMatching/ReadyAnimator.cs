using UnityEngine;
using System;

namespace HealingJam.Crossword
{
    public class ReadyAnimator : MonoBehaviour
    {
        public void PlayAnimation(Action animationEndAction)
        {
            this.animationEndAction = animationEndAction;
            gameObject.SetActive(true);
        }

        private Action animationEndAction = null;

        public void OnAnimationEnd()
        {
            animationEndAction?.Invoke();
            gameObject.SetActive(false);
        }
    }
}