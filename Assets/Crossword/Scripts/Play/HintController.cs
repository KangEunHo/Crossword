using UnityEngine;
using System.Collections;

namespace HealingJam.Crossword
{
    public class HintController : MonoBehaviour
    {
        private const int HINT_PRICE = 10;
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
                    if (Save.SaveMgr.Instance.GetCoin() >= HINT_PRICE)
                    {
                        Save.SaveMgr.Instance.AddCoin(-10);
                        char letter = boardHighlightController.SelectedWordData.word[boardHighlightController.SelectedLetterIndex];
                        letterSelectionButtonController.LetterSelectionButtonChangeChangeAlreadySelectedStateSameLetter(letter);
                        boardHighlightController.SetCompleteHighlightCell();
                    }
                    else
                    {
                        ToastPlugin.ToastHelper.ShowToast("코인이 부족합니다");
                    }

                }
            }
        }
    }
}