using UnityEngine;
using DG.Tweening;

namespace HealingJam.Crossword.UI
{
    public class ScaleTweenAnimation : MonoBehaviour
    {
        private Tween scaleTween = null;

        private void CreateTween()
        {
            scaleTween = transform.DOScale(Vector3.one, 0.25f)
                .SetAutoKill(false).Pause().SetLink(gameObject);
        }

        public void Play()
        {
            if (scaleTween == null)
                CreateTween();

            transform.localScale = Vector3.zero;
            scaleTween.PlayForward();
        }

        public void Rewind()
        {
            if (scaleTween == null)
                CreateTween();

            transform.localScale = Vector3.one;
            scaleTween.PlayBackwards();
        }
    }
}