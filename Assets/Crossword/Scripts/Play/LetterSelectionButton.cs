using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace HealingJam.Crossword
{
    public class LetterSelectionButton : DarkModeMonoBehaviour, IPointerClickHandler
    {
        public enum ButtonState
        {
            Basic = 0, Selected = 1
        }

        [SerializeField] private Text letterText = null;
        [SerializeField] private Image image = null;
        [SerializeField] private DarkModeSprite basicSprite = null;
        [SerializeField] private DarkModeSprite selectedSprite = null;

        public Action<LetterSelectionButton> onClick = null;
        private ButtonState buttonState = ButtonState.Basic;
        public ButtonState GetButtonState => buttonState;

        private char letter;
        public char Letter
        {
            get { return letter; }
            set { letter = value; letterText.text = value.ToString(); }
        }

        public void OnPointerClick(PointerEventData eventData)
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
            }
            else
            {
                image.sprite = selectedSprite.ActiveModeSprite;
            }
        }
    }
}