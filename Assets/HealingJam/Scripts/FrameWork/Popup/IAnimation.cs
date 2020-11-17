using System;

namespace HealingJam
{
    public interface IAnimation
    {
        event Action OnAnimationStart;
        event Action OnAnimationEnd;
        void PlayForward();
        void PlayBackward();
    }
}