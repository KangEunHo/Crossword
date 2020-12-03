using UnityEngine;
using System.Collections;

namespace HealingJam.Crossword
{
    public class HintController : MonoBehaviour
    {
        private BoardHighlightController boardHighlightController = null;
        private LetterSelectionButtonController letterSelectionButtonController = null;

        public void Init(BoardHighlightController boardHighlightController, LetterSelectionButtonController letterSelectionButtonController)
        {
            this.boardHighlightController = boardHighlightController;
            this.letterSelectionButtonController = letterSelectionButtonController;
        }

        public void OnHintButtonClick()
        {
            UseHint();
        }

        private void UseHint()
        {
            if (boardHighlightController.SelectedWordData != null)
            {
                BoardCell boardCell = boardHighlightController.GetHighlightCell();
                if (boardCell != null)
                {
                    //할것.
                    //if (coin >)
                    boardHighlightController.SetCompleteHighlightCell();
                }
            }
        }
    }
}