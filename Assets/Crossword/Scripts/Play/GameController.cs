using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace HealingJam.Crossword
{
    public class GameController : MonoBehaviour
    {
        public enum GameState
        {
            Pause, Play, Fail, Clear
        }

        [SerializeField] private TextAsset mapTextAsset = null;
        [SerializeField] private BoardController boardController = null;
        [SerializeField] private LetterSelectionButtonContoller letterSelectionButtonContoller = null;
        [SerializeField] private WordMeaningController wordMeaningController = null;
        [SerializeField] private BoardHighlightController boardHighlightController = null;

        private AnswerChecker answerChecker = null;
        public GameState State { get; private set; }

        private void Start()
        {
            CrosswordMap crosswordMap = JsonConvert.DeserializeObject<CrosswordMap>(mapTextAsset.text);
            boardController.GenerateBoard(crosswordMap);
            letterSelectionButtonContoller.Init(crosswordMap);
            answerChecker = new AnswerChecker(crosswordMap, boardController);
            boardHighlightController.Init(boardController);

            boardController.boardClickEventHandler += wordMeaningController.OnCellBoardClick;
            boardController.boardClickEventHandler += letterSelectionButtonContoller.OnCellBoardClick;

            letterSelectionButtonContoller.letterSelectionButtonClickHandler += boardHighlightController.OnLetterSelectionBoardClick;

            boardHighlightController.onCorrectAnswer += OnCorrectAnswer;

            State = GameState.Play;

            WordDataForGame unMatchedWord = answerChecker.GetUnMatchedWord();
            boardHighlightController.SetUpHighlightCells(unMatchedWord);
            wordMeaningController.SetText(unMatchedWord);
            letterSelectionButtonContoller.SetButtonsLetter(unMatchedWord);
        }

        public void OnCorrectAnswer(WordDataForGame wordDataForGame)
        {
            if (answerChecker.AddMatchedWord(wordDataForGame))
            {
                State = GameState.Clear;
                //wordMeaningController.SetText(null);
            }
            else
            {
                WordDataForGame unMatchedWord = answerChecker.GetUnMatchedWord();
                boardHighlightController.SetUpHighlightCells(unMatchedWord);
                wordMeaningController.SetText(unMatchedWord);
                letterSelectionButtonContoller.SetButtonsLetter(unMatchedWord);
            }
        }

        public void OnResetButtonClick()
        {
            boardHighlightController.SetUpHighlightCells(boardHighlightController.SelectedWordData);
            letterSelectionButtonContoller.SetButtonsLetter(boardHighlightController.SelectedWordData);
        }
    }
}