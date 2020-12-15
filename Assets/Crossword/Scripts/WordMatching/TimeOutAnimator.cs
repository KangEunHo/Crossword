using UnityEngine;
using System;

namespace HealingJam.Crossword
{
    public class TimeOutAnimator : MonoBehaviour
    {
        public void PlayAnimation(Action animationEndAction)
        {
            this.animationEndAction = animationEndAction;
            gameObject.SetActive(true);

            SoundMgr.Instance.PlayOneShot(SoundMgr.Instance.countDown);
        }

        private Action animationEndAction = null;

        public void OnAnimationEnd()
        {
            animationEndAction?.Invoke();
            gameObject.SetActive(false);
        }
    }
}