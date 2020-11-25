using UnityEngine;
using System;

namespace HealingJam.Crossword
{
    public class AnswerOXResult : MonoBehaviour
    {
        [SerializeField] private GameObject animator = null;
        [SerializeField] private GameObject oMark = null;
        [SerializeField] private GameObject xMark = null;

        private Action endAnimationAction = null;

        public void ShowOResult(Action endAnimationAction)
        {
            this.endAnimationAction = endAnimationAction;
            oMark.SetActive(true);
            xMark.SetActive(false);
            animator.SetActive(true);
        }

        public void ShowXResult(Action endAnimationAction)
        {
            this.endAnimationAction = endAnimationAction;
            xMark.SetActive(true);
            oMark.SetActive(false);
            animator.SetActive(true);
        }

        public void OnEndAnimation()
        {
            endAnimationAction?.Invoke();
            animator.SetActive(false);
        }
    }
}