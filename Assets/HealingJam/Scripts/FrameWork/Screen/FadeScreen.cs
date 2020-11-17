using UnityEngine;
using DG.Tweening;

namespace HealingJam.GameScreens
{
    public class FadeScreen : GameScreen
    {
        protected CanvasGroup canvasGroup = null;

        public override void Init(object stateMachine)
        {
            base.Init(stateMachine);
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public override void Enter(params object[] args)
        {
            base.Enter(args);
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.DOFade(1f, 0.5f).OnComplete(() => { canvasGroup.blocksRaycasts = true;
                OnEnterFadeComplete(args);
            });
        }

        public override void Exit(params object[] args)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.DOFade(0f, 0.5f).OnComplete(() => { gameObject.SetActive(false);
                OnExitFadeComplete(args);
            });
        }

        protected virtual void OnEnterFadeComplete(params object[] args) { }
        protected virtual void OnExitFadeComplete(params object[] args) { }
    }
}