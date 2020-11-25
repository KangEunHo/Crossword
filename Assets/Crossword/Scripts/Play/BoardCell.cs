using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace HealingJam.Crossword
{
    public class BoardCell : DarkModeMonoBehaviour
    {
        public enum CellState
        {
            None, Unuse, Completed
        }

        [SerializeField] private DarkModeSprite cellCompleteSprite = null;
        [SerializeField] private DarkModeSprite cellUnCompleteSprite = null;

        [SerializeField] private Text letterText = null;
        [SerializeField] private Image cellImage = null;

        public Vector2Int Index { get; private set; }

        public WordDataForGame HorizontalWordData { get; set; } = null;
        public WordDataForGame VerticalWordData { get; set; } = null;
        public CellState State { get; private set; } = CellState.None;
        public char Letter { get; private set; }

        public void Init(Vector2Int index)
        {
            Index = index;
            HorizontalWordData = null;
            VerticalWordData = null;
        }

        public void SetUnuseState()
        {
            State = CellState.Unuse;
            gameObject.SetActive(false);
        }

        public void SetCompleteState()
        {
            State = CellState.Completed;
            cellImage.sprite = cellCompleteSprite.ActiveModeSprite;
        }

        public void SetSpriteToCurrentState()
        {
            if (State == CellState.None)
            {
                cellImage.sprite = cellUnCompleteSprite.ActiveModeSprite;
            }
            else if (State == CellState.Completed)
            {
                cellImage.sprite = cellCompleteSprite.ActiveModeSprite;
            }
        }

        public void SetSprite(Sprite sprite)
        {
            cellImage.sprite = sprite;
        }

        public void SetLetter(char letter)
        {
            Letter = letter;
            letterText.text = letter.ToString();
        }

        public override void DarkModeChanged(bool darkMode)
        {
            SetSpriteToCurrentState();
        }
    }
}