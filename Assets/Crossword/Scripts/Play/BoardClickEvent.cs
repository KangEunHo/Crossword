using UnityEngine;
using System.Collections;
using System;

namespace HealingJam.Crossword
{
    public class BoardClickEvent : EventArgs
    {
        public BoardCell boardCell;
        public WordDataForGame.Direction direction;

        public BoardClickEvent(BoardCell boardCell, WordDataForGame.Direction direction)
        {
            this.boardCell = boardCell; this.direction = direction;
        }
    }
}