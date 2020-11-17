using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;


namespace HealingJam.Popups
{
    public class ScaleTweenPopupAnimation : IPopupAnimation
    {
        protected RectTransform scaleTransform = null;
        protected float animationDuration = 0.5f;

        protected Sequence animationSequence = null;

        public event Action OnAnimationStart;
        public event Action OnAnimationEnd;

        public void Init(Popup popup)
        {
            scaleTransform = popup.transform as RectTransform;

            animationSequence = DOTween.Sequence()
                .Append(scaleTransform.DOScale(1f, animationDuration))
                .SetEase(Ease.InOutQuart)
                .SetAutoKill(false)
                .Pause();
        }

        public virtual void PlayForward()
        {
            scaleTransform.localScale = Vector3.zero;
            animationSequence.PlayForward();
            animationSequence.OnComplete(StartAnimationCallback);
        }

        public virtual void PlayBackward()
        {
            animationSequence.PlayBackwards();
            animationSequence.OnRewind(EndAnimationCallback);
        }

        public virtual void StartAnimationCallback()
        {
            OnAnimationStart?.Invoke();
        }

        public virtual void EndAnimationCallback()
        {
            OnAnimationEnd?.Invoke();
        }

        public void Dispose()
        {
            if (animationSequence != null)
            {
                animationSequence.Kill(false);
                animationSequence = null;
            }

            OnAnimationStart = null;
            OnAnimationEnd = null;
        }
    }
}