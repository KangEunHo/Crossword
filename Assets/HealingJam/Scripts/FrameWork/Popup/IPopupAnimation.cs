using UnityEngine;
using System;

namespace HealingJam.Popups
{
    public interface IPopupAnimation : IAnimation, IDisposable
    {
        void Init(Popup popup);
    }
}