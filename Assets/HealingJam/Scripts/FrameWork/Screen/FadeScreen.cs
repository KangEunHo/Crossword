using UnityEngine;
using DG.Tweening;

namespace HealingJam.GameScreens
{
    public class FadeScreen : GameScreen
    {
        [SerializeField] protected CanvasGroup canvasGroup = null;
        protected float fadeDuration = 0.25f;
        private Tween canvasGroupTween = null;

        public override void Init(object stateMachine)
        {
            base.Init(stateMachine);
            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();

            canvasGroupTween = canvasGroup.DOFade(1f, fadeDuration).SetAutoKill(false).SetLink(gameObject).Pause();
        }

        public override void Enter(params object[] args)
        {
            base.Enter(args);
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;

            canvasGroupTween.OnComplete(() =>
            {
                canvasGroup.blocksRaycasts = true;
                OnEnterFadeComplete(args);
            });

            canvasGroupTween.PlayForward();
        }

        public override void Exit(params object[] args)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = false;

            canvasGroupTween.OnRewind(() =>
            {
                gameObject.SetActive(false);
                OnExitFadeComplete(args);
            });

            canvasGroupTween.PlayBackwards();
        }

        protected virtual void OnEnterFadeComplete(params object[] args) { }
        protected virtual void OnExitFadeComplete(params object[] args) { }
    }
}