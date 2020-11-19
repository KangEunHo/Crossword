using UnityEngine;
using System.Collections.Generic;
using System;

namespace HealingJam.Crossword
{
    public class LetterSelectionButtonContoller : MonoBehaviour
    {
        public const int BUTTON_COUNT = 12;
        [SerializeField] private LetterSelectionButton[] letterSelectionButtons = null;

        public event EventHandler<LetterSelectionButtonClickEventArgs> letterSelectionButtonClickHandler = null;

        private List<char> letterDatabase = null;

        public void Init(CrosswordMap crosswordMap)
        {
            EditorDebug.Assert(letterSelectionButtons.Length == BUTTON_COUNT);

            letterDatabase = new List<char>();

            foreach (var word in crosswordMap.wordDatas)
            {
                for (int i = 0; i < word.word.Length; ++i)
                {
                    letterDatabase.Add(word.word[i]);
                }
            }

            foreach(var letterSelectionButton in letterSelectionButtons)
            {
                letterSelectionButton.onClick += OnLetterSelectionButtonClick;
            }
        }

        public void OnCellBoardClick(object sender, BoardClickEvent boardClickEvent)
        {
            BoardCell boardCell = boardClickEvent.boardCell;
            WordDataForGame.Direction direction = boardClickEvent.direction;

            if (direction == WordDataForGame.Direction.None)
            {
                return;
            }
            else if (direction == WordDataForGame.Direction.Horizontal)
            {
                SetButtonsLetter(boardCell.HorizontalWordData);
            }
            else if (direction == WordDataForGame.Direction.Vertical)
            {
                SetButtonsLetter(boardCell.VerticalWordData);
            }
        }

        public void SetButtonsLetter(WordDataForGame wordDataForGame)
        {
            List<char> letters = new List<char>();

            for (int i = 0; i < wordDataForGame.word.Length; ++i)
            {
                letters.Add(wordDataForGame.word[i]);
            }

            for (int i = letters.Count; i < BUTTON_COUNT; ++i)
            {
                letters.Add(letterDatabase.RandomValue());
            }

            letters.Shuffle();

            for (int i = 0; i < BUTTON_COUNT; ++i)
            {
                letterSelectionButtons[i].SetLetter(letters[i]);
            }
        }

        private void OnLetterSelectionButtonClick(LetterSelectionButton letterSelectionButton)
        {
            letterSelectionButtonClickHandler?.Invoke(this, new LetterSelectionButtonClickEventArgs(letterSelectionButton.letter));
        }
    }
}