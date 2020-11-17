using UnityEngine;
using System.Collections;

namespace HealingJam.Crossword
{
    public class BoardController : MonoBehaviour
    {
        public const int MAP_SIZE_X = 10;
        public const int MAP_SIZE_Y = 9;

        [SerializeField] private BoardCell[] childBoardCells = null;
        private BoardCell[,] boardCells = null;

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
            foreach(var wordData in crosswordMap.wordDatas)
            {
                for (int i = 0; i < wordData.word.Length; ++i)
                {
                    if (wordData.direction == WordDataForGame.Direction.Horizontal)
                    {
                        boardCells[wordData.y, wordData.x + i].HorizontalWordData = wordData;
                    }
                    else if (wordData.direction == WordDataForGame.Direction.Vertical)
                    {
                        boardCells[wordData.y + i, wordData.x].HorizontalWordData = wordData;
                    }
                }
            }

            // 사용하지 않는 셀이라면 사용하지 않는 상태로 변경합니다.
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

    }
}