using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;

namespace HealingJam.Crossword
{
    public class LetterSelectionButton : DarkModeMonoBehaviour
    {
        public enum ButtonState
        {
            Basic = 0, Selected = 1, AlreadySelected =2 
        }

        [SerializeField] private Text letterText = null;
        [SerializeField] private Image image = null;
        [SerializeField] private DarkModeSprite basicSprite = null;
        [SerializeField] private DarkModeSprite selectedSprite = null;

        public Action<LetterSelectionButton> onClick = null;
        private ButtonState buttonState = ButtonState.Basic;
        public ButtonState GetButtonState => buttonState;

        private Tween animationTween = null;

        private char letter;
        public char Letter
        {
            get { return letter; }
            set { letter = value; letterText.text = value.ToString(); }
        }

        private void Start()
        {
            Button button = GetComponent<Button>();
            button.onClick.AddListener(OnPointerClick);

            animationTween = DOTween.Sequence()
            .Append(transform.DOScale(0.3f, 0.5f))
            .Join(image.DOFade(0f, 0.5f))
            .Join(letterText.DOFade(0f, 0.5f))
            .SetAutoKill(false)
            .SetLink(gameObject)
            .Pause();
        }

        public void OnPointerClick()
        {
            if (buttonState == ButtonState.Basic)
                onClick?.Invoke(this);
        }

        public override void DarkModeChanged(bool darkMode)
        {
            SetState(buttonState);
        }

        public void SetState(ButtonState state)
        {
            buttonState = state;

            if (buttonState == ButtonState.Basic)
            {
                image.sprite = basicSprite.ActiveModeSprite;
                letterText.SetAlpha(1f);
                if (animationTween != null)
                {
                    animationTween.Rewind();
                }
                Show();

            }
            else if (buttonState == ButtonState.Selected)
            {
                image.sprite = selectedSprite.ActiveModeSprite;

                if (animationTween != null)
                {
                    animationTween.Rewind();
                    animationTween.PlayForward();
                }
            }
            else
            {
                image.sprite = selectedSprite.ActiveModeSprite;
                letterText.SetAlpha(0.8f);
                if (animationTween != null)
                {
                    animationTween.Rewind();
                }
                Show();
            }
        }

        private void Show()
        {
            transform.localScale = Vector3.one;
            image.SetAlpha(1f);
        }
    }
}