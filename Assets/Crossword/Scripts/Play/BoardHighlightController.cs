using UnityEngine;
using System.Collections.Generic;
using System;

namespace HealingJam.Crossword
{
    public class BoardHighlightController : DarkModeMonoBehaviour
    {
        [SerializeField] private DarkModeSprite cellHighlightSprite = null;
        [SerializeField] private DarkModeSprite cellHighlightSelectedSprite = null;
        [SerializeField] private GameObject boardGradation = null;

        private BoardController boardController = null;

        public WordDataForGame SelectedWordData { get; private set; } = null;
        public int SelectedLetterIndex { get; private set; } = -1;
        private List<BoardCell> highlightCells = new List<BoardCell>();

        public Action<WordDataForGame> onCorrectAnswer = null;
        public Action<WordDataForGame> onWrongAnswer = null;

        private bool darkModeChangeTrigger = false;


        public void Init(BoardController boardController)
        {
            this.boardController = boardController;
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
            SelectedLetterIndex = 0;
            ClearHighlightCellsLetter();
            OffHighlightCellsSprite();
            if (wordDataForGame != null)
            {
                AddHighlightCells();
                SetNextSelectedBoardCell();
            }
        }

        public void SetHighlightMatchedWord(WordDataForGame wordDataForGame)
        {
            SelectedWordData = wordDataForGame;
            SelectedLetterIndex = -1;
            ClearHighlightCellsLetter();
            OffHighlightCellsSprite();
            OffCellGradation();
            if (wordDataForGame != null)
            {
                AddHighlightCells();
                OnHighlightCellsSprite();
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
                x += SelectedLetterIndex;
            else y += SelectedLetterIndex;

            BoardCell boardCell = boardController.GetBoardCell(new Vector2Int(x, y));
            boardCell.SetLetter(args.letter);

            SelectedLetterIndex++;
            SetNextSelectedBoardCell();

            SoundMgr.Instance.PlayOneShotButtonSound();
        }

        private void SetNextSelectedBoardCell()
        {
            int i = SelectedLetterIndex;
            for (; i < SelectedWordData.word.Length; ++i)
            {
                int x = SelectedWordData.x;
                int y = SelectedWordData.y;
                if (SelectedWordData.direction == WordDataForGame.Direction.Horizontal)
                    x += i;
                else y += i;

                if (boardController.GetBoardCell(new Vector2Int(x, y)).State == BoardCell.CellState.Completed)
                {
                    SelectedLetterIndex++;
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
                OffCellGradation();
            }
            else
            {
                OnHighlightCellsSprite();
                OnHighlightCellGradation();
            }
        }
        
        public void SetCompleteHighlightCell()
        {
            BoardCell boardCell = GetHighlightCell();
            if (boardCell != null)
            {
                char letter = SelectedWordData.word[SelectedLetterIndex];
                boardCell.SetLetter(letter);
                boardCell.SetCompleteState();
                SelectedLetterIndex++;
                SetNextSelectedBoardCell();
            }
        }

        private void OnHighlightCellsSprite()
        {
            for (int i = 0; i < highlightCells.Count; ++i)
            {
                highlightCells[i].SetSprite(i == SelectedLetterIndex ? cellHighlightSelectedSprite.ActiveModeSprite : cellHighlightSprite.ActiveModeSprite);
            }
        }
        
        private void OnHighlightCellGradation()
        {
            boardGradation.SetActive(true);
            boardGradation.transform.SetParent(highlightCells[SelectedLetterIndex].transform, false);
        }

        public void OffCellGradation()
        {
            boardGradation.SetActive(false);
            boardGradation.transform.SetParent(transform, false);
        }

        public BoardCell GetHighlightCell()
        {
            if (SelectedLetterIndex != -1 && SelectedLetterIndex < highlightCells.Count)
            {
                return highlightCells[SelectedLetterIndex];
            }
            return null;
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
            SelectedLetterIndex = -1;
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