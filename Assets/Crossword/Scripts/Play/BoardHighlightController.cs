using UnityEngine;
using System.Collections.Generic;
using System;

namespace HealingJam.Crossword
{
    public class BoardHighlightController : DarkModeMonoBehaviour
    {
        [SerializeField] private DarkModeSprite cellHighlightSprite = null;
        [SerializeField] private DarkModeSprite cellHighlightSelectedSprite = null;

        private BoardController boardController = null;

        public WordDataForGame SelectedWordData { get; private set; } = null;
        private int selectedLetterIndex;
        private List<BoardCell> highlightCells = null;

        public Action<WordDataForGame> onCorrectAnswer = null;
        public Action<WordDataForGame> onWrongAnswer = null;

        private bool darkModeChangeTrigger = false;


        public void Init(BoardController boardController)
        {
            highlightCells = new List<BoardCell>();
            this.boardController = boardController;
            boardController.boardClickEventHandler += OnCellBoardClick;
        }

        public void OnCellBoardClick(object sender, BoardClickEvent boardClickEvent)
        {
            BoardCell boardCell = boardClickEvent.boardCell;
            WordDataForGame.Direction direction = boardClickEvent.direction;


            if (direction == WordDataForGame.Direction.None)
            {
            }
            else
            {
                WordDataForGame wordDataForGame = direction == WordDataForGame.Direction.Horizontal ? boardCell.HorizontalWordData : boardCell.VerticalWordData;
                SetUpHighlightCells(wordDataForGame);
            }
        }

        public void SetUpHighlightCells(WordDataForGame wordDataForGame)
        {
            SelectedWordData = wordDataForGame;
            selectedLetterIndex = 0;
            ClearHighlightCellsLetter();
            OffHighlightCellsSprite();
            if (wordDataForGame != null)
            {
                AddHighlightCells();
                SetNextSelectedBoardCell();
            }
        }

        private void AddHighlightCells()
        {
            for (int i = 0; i < SelectedWordData.word.Length; ++i)
            {
                int x = SelectedWordData.x;
                int y = SelectedWordData.y;
                if (SelectedWordData.direction == WordDataForGame.Direction.Horizontal)
                    x += i;
                else y += i;

                highlightCells.Add(boardController.GetBoardCell(new Vector2Int(x, y)));
            }
        }

        public void OnLetterSelectionBoardClick(object sender, LetterSelectionButtonClickEventArgs args)
        {
            if (SelectedWordData == null)
                return;

            int x = SelectedWordData.x;
            int y = SelectedWordData.y;
            if (SelectedWordData.direction == WordDataForGame.Direction.Horizontal)
                x += selectedLetterIndex;
            else y += selectedLetterIndex;

            BoardCell boardCell = boardController.GetBoardCell(new Vector2Int(x, y));
            boardCell.SetLetter(args.letter);

            selectedLetterIndex++;
            SetNextSelectedBoardCell();
        }

        private void SetNextSelectedBoardCell()
        {
            int i = selectedLetterIndex;
            for (; i < SelectedWordData.word.Length; ++i)
            {
                int x = SelectedWordData.x;
                int y = SelectedWordData.y;
                if (SelectedWordData.direction == WordDataForGame.Direction.Horizontal)
                    x += i;
                else y += i;

                if (boardController.GetBoardCell(new Vector2Int(x, y)).State == BoardCell.CellState.Completed)
                {
                    selectedLetterIndex++;
                    continue;
                }
                else
                {
                    break;
                }
            }

            if (i >= SelectedWordData.word.Length)
            {
                OnEndMatch();
            }
            else
            {
                OnHighlightCellsSprite();
            }
        }
        
        private void OnHighlightCellsSprite()
        {
            for (int i = 0; i < highlightCells.Count; ++i)
            {
                highlightCells[i].SetSprite(i == selectedLetterIndex ? cellHighlightSelectedSprite.ActiveModeSprite : cellHighlightSprite.ActiveModeSprite);
            }
        }

        private void OffHighlightCellsSprite()
        {
            foreach (var v in highlightCells)
            {
                v.SetSpriteToCurrentState();
            }
            highlightCells.Clear();
        }

        private void OnEndMatch()
        {
            bool correctAnswer = true;

            for (int i = 0; i < highlightCells.Count; ++i)
            {
                if (highlightCells[i].Letter != SelectedWordData.word[i])
                {
                    correctAnswer = false;
                    break;
                }
            }

            if (correctAnswer)
            {
                foreach (var v in highlightCells)
                {
                    v.SetCompleteState();
                }

                onCorrectAnswer?.Invoke(SelectedWordData);
            }
            else
            {
                onWrongAnswer?.Invoke(SelectedWordData);
            }
        }

        public void ClearHighlightCellsLetter()
        {
            foreach (var v in highlightCells)
            {
                if (v.State == BoardCell.CellState.None)
                    v.SetLetter(' ');
            }
        }

        private void LateUpdate()
        {
            if (darkModeChangeTrigger)
            {
                darkModeChangeTrigger = false;
                OnHighlightCellsSprite();
            }
        }
        public override void DarkModeChanged(bool darkMode)
        {
            darkModeChangeTrigger = true;
        }
    }
}