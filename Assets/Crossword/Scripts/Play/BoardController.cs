using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

namespace HealingJam.Crossword
{
    public class BoardController : MonoBehaviour, IPointerClickHandler
    {
        public const int MAP_SIZE_X = 10;
        public const int MAP_SIZE_Y = 9;
        public const float CellSize = 54f;

        [SerializeField] private BoardCell[] childBoardCells = null;
        private BoardCell[,] boardCells = null;
        private RectTransform myRectTransform = null;
        public event EventHandler<BoardClickEvent> boardClickEventHandler = null;
        private WordDataForGame.Direction selectedWordDataDirection = WordDataForGame.Direction.None;

        private void Start()
        {
            myRectTransform = transform as RectTransform;

        }

        public void GenerateBoard(CrosswordMap crosswordMap)
        {
            // 현재 맵 사이즈는 10 x 9로 고정됩니다.
            EditorDebug.Assert(crosswordMap.mapSizeX == MAP_SIZE_X && crosswordMap.mapSizeY == MAP_SIZE_Y);

            // 일차원 배열의 셀들을 이차원 배열로 옮긴 후 셀들의 인덱스를 설정합니다.
            boardCells = new BoardCell[MAP_SIZE_Y, MAP_SIZE_X];
            int cellIndex = 0;
            for (int y = 0; y < MAP_SIZE_Y; ++y)
            {
                for (int x = 0; x < MAP_SIZE_X; ++x)
                {
                    boardCells[y, x] = childBoardCells[cellIndex++];
                    boardCells[y, x].Init(new Vector2Int(x, y));
                }
            }

            // 단어들에 위치에 해당하는 셀에 단어를 입력합니다.
            foreach (var wordData in crosswordMap.wordDatas)
            {
                for (int i = 0; i < wordData.word.Length; ++i)
                {
                    if (wordData.direction.HasFlag(WordDataForGame.Direction.Horizontal))
                    {
                        boardCells[wordData.y, wordData.x + i].HorizontalWordData = wordData;
                    }
                    if (wordData.direction.HasFlag(WordDataForGame.Direction.Vertical))
                    {
                        boardCells[wordData.y + i, wordData.x].VerticalWordData = wordData;
                    }
                }
            }

            //사용하지 않는 셀이라면 사용하지 않는 상태로 변경합니다.
            for (int y = 0; y < MAP_SIZE_Y; ++y)
            {
                for (int x = 0; x < MAP_SIZE_X; ++x)
                {
                    BoardCell boardCell = boardCells[y, x];
                    if (boardCell.HorizontalWordData == null && boardCell.VerticalWordData == null)
                        boardCell.SetUnuseState();
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(myRectTransform, eventData.position, null, out Vector2 localPos);
            // 맨 위의 인덱스가 0 이므로 y 값을 뒤집어줍니다.
            localPos.y = -localPos.y;
            localPos += myRectTransform.sizeDelta * 0.5f;
            Vector2Int clickIndex = Vector2Int.FloorToInt(localPos / CellSize);

            if (IsValidIndex(clickIndex) == false)
                return;

            BoardCell clickBoardCell = boardCells[clickIndex.y, clickIndex.x];
            // 
            if (clickBoardCell.State == BoardCell.CellState.None)
            {
                selectedWordDataDirection = DirectionCalculation(clickBoardCell, selectedWordDataDirection);
                boardClickEventHandler?.Invoke(this, new BoardClickEvent(clickBoardCell, selectedWordDataDirection));
            }
        }

        private WordDataForGame.Direction DirectionCalculation(BoardCell boardCell, WordDataForGame.Direction currentDirection)
        {
            if (currentDirection == WordDataForGame.Direction.None)
            {
                if (boardCell.HorizontalWordData != null)
                    return WordDataForGame.Direction.Horizontal;
                if (boardCell.VerticalWordData != null)
                    return WordDataForGame.Direction.Vertical;
            }
            else if (currentDirection == WordDataForGame.Direction.Horizontal)
            {
                if (boardCell.VerticalWordData != null)
                    return WordDataForGame.Direction.Vertical;
            }
            else if (currentDirection == WordDataForGame.Direction.Vertical)
            {
                if (boardCell.HorizontalWordData != null)
                    return WordDataForGame.Direction.Horizontal;
            }

            if (boardCell.HorizontalWordData == null && boardCell.VerticalWordData == null)
                return WordDataForGame.Direction.None;

            return currentDirection;
        }

        // 유효한 인덱스인지 검사합니다.
        public bool IsValidIndex(Vector2Int index)
        {
            return index.x >= 0 && index.x < MAP_SIZE_X && index.y >= 0 && index.y < MAP_SIZE_Y;
        }

        public BoardCell GetBoardCell(Vector2Int index)
        {
            if (IsValidIndex(index) == false)
            {
                EditorDebug.LogError("out of index");
            }
            return boardCells[index.y, index.x];
        }

        public bool IsCompletedWord(WordDataForGame wordDataForGame)
        {
            for (int i = 0; i < wordDataForGame.word.Length; ++i)
            {
                int x = wordDataForGame.x;
                int y = wordDataForGame.y;
                if (wordDataForGame.direction == WordDataForGame.Direction.Horizontal)
                    x += i;
                else if (wordDataForGame.direction == WordDataForGame.Direction.Horizontal)
                    y += i;

                if (GetBoardCell(new Vector2Int(x, y)).State == BoardCell.CellState.None)
                    return false;
            }
            return true;
        }
    }
}