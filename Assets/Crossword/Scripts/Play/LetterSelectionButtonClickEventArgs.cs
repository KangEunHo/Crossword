using UnityEngine;
using System;

namespace HealingJam.Crossword
{
    public class LetterSelectionButtonClickEventArgs : EventArgs
    {
        public char letter;
        public LetterSelectionButtonClickEventArgs(char letter)
        {
            this.letter = letter;
        }
    }
}