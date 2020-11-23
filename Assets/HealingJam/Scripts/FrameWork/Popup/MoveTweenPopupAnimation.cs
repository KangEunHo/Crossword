using UnityEngine;
using DG.Tweening;
using System;


namespace HealingJam.Popups
{
    public class MoveTweenPopupAnimation : IPopupAnimation
    {
        [Flags]
        public enum MoveDirection
        {
            None = 0, LeftToCenter = 1, RightToCenter = 2, TopToCenter = 4, BottonToCenter = 8
        }

        protected RectTransform targetTransform = null;
        protected float animationDuration = 0.5f;
        protected MoveDirection moveDirection;

        protected Sequence animationSequence = null;

        public event Action OnAnimationStart;
        public event Action OnAnimationEnd;

        public MoveTweenPopupAnimation(MoveDirection moveDirection, float animationDuration)
        {
            this.moveDirection = moveDirection;
            this.animationDuration = animationDuration;
        }

        public void Init(Popup popup)
        {
            targetTransform = popup.Window;

            animationSequence = DOTween.Sequence()
                .Append(targetTransform.DOAnchorPos(Vector2.zero, animationDuration).SetEase(Ease.InOutQuart))
                .SetAutoKill(false)
                .SetLink(popup.gameObject)
                .Pause();
        }

        public virtual void PlayForward()
        {
            targetTransform.anchoredPosition = CalculateStartPosInDirection(moveDirection);
            animationSequence.PlayForward();
            animationSequence.OnComplete(StartAnimationCallback);
        }

        public virtual void PlayBackward()
        {
            targetTransform.anchoredPosition = Vector2.zero;
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

        private Vector2 CalculateStartPosInDirection(MoveDirection moveDirection)
        {
            float x = 0f;
            float y = 0f;
            float screenWidth = UIUtilities.GetScreenWidth(targetTransform);
            float screenHeight = UIUtilities.GetScreenHeight(targetTransform);

            if (moveDirection.HasFlag(MoveDirection.LeftToCenter))
                x -= screenWidth;
            else if (moveDirection.HasFlag(MoveDirection.RightToCenter))
                x += screenWidth;

            if (moveDirection.HasFlag(MoveDirection.BottonToCenter))
                y -= screenHeight;
            else if (moveDirection.HasFlag(MoveDirection.TopToCenter))
                y += screenHeight;

            return new Vector2(x, y);
        }
    }
}