using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace HealingJam.Crossword
{
    public class LetterSelectionButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Text letterText = null;

        public Action<LetterSelectionButton> onClick = null;
        public char letter;

        public void SetLetter(char letter)
        {
            this.letter = letter;
            letterText.text = letter.ToString();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onClick?.Invoke(this);
        }
    }
}