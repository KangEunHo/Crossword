using UnityEngine;
using UnityEngine.UI;
using System;

namespace HealingJam.Crossword
{
    public class WrongWordAnimator : MonoBehaviour
    {
        [SerializeField] private Text wordText = null;
        public void PlayAnimation(Action animationEndAction, string word)
        {
            this.animationEndAction = animationEndAction;
            wordText.text = word;
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