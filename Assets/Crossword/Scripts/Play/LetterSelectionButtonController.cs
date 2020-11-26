using UnityEngine;
using System.Collections.Generic;
using System;

namespace HealingJam.Crossword
{
    public class LetterSelectionButtonController : MonoBehaviour
    {
        public const int BUTTON_COUNT = 12;
        [SerializeField] private LetterSelectionButton[] letterSelectionButtons = null;

        public event EventHandler<LetterSelectionButtonClickEventArgs> letterSelectionButtonClickHandler = null;

        public void Init()
        {
            EditorDebug.Assert(letterSelectionButtons.Length == BUTTON_COUNT);

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

        /// <summary>
        /// 버튼들에 글자를 설정합니다.
        /// 들어가는 단어의 글자들을 집어 넣고 나머지 글자를 채웁니다.
        /// </summary>
        /// <param name="wordDataForGame"></param>
        public void SetButtonsLetter(WordData wordData)
        {
            List<char> letters = new List<char>();

            for (int i = 0; i < wordData.word.Length; ++i)
            {
                letters.Add(wordData.word[i]);
            }

            for (int i = letters.Count; i < BUTTON_COUNT; ++i)
            {
                letters.Add(LetterDatabase.GetRandomValue());
            }

            letters.Shuffle();

            for (int i = 0; i < BUTTON_COUNT; ++i)
            {
                letterSelectionButtons[i].Letter = letters[i];
                letterSelectionButtons[i].SetState(LetterSelectionButton.ButtonState.Basic);
            }
        }

        private void OnLetterSelectionButtonClick(LetterSelectionButton letterSelectionButton)
        {
            letterSelectionButton.SetState(LetterSelectionButton.ButtonState.Selected);
            letterSelectionButtonClickHandler?.Invoke(this, new LetterSelectionButtonClickEventArgs(letterSelectionButton.Letter));
        }

        public void ChangeButtonsStateToAllBasic()
        {
            for (int i = 0; i < BUTTON_COUNT; ++i)
            {
                letterSelectionButtons[i].SetState(LetterSelectionButton.ButtonState.Basic);
            }
        }

        public void LetterSelectionButtonClickSameLetter(char letter)
        {
            for (int i = 0; i < BUTTON_COUNT; ++i)
            {
                if (letterSelectionButtons[i].GetButtonState == LetterSelectionButton.ButtonState.Basic)
                {
                    if (letterSelectionButtons[i].Letter == letter)
                    {
                        OnLetterSelectionButtonClick(letterSelectionButtons[i]);
                        return;
                    }
                }
            }
        }

        public void LetterSelectionButtonStateChangeSameLetter(char letter)
        {
            for (int i = 0; i < BUTTON_COUNT; ++i)
            {
                if (letterSelectionButtons[i].GetButtonState == LetterSelectionButton.ButtonState.Basic)
                {
                    if (letterSelectionButtons[i].Letter == letter)
                    {
                        letterSelectionButtons[i].SetState(LetterSelectionButton.ButtonState.Selected);
                        return;
                    }
                }
            }
        }
    }
}