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
            // 크로스 워드맵 생성.
            CrosswordMap crosswordMap = CrosswordMapManager.Instance.ActiveCrosswordMap; //JsonConvert.DeserializeObject<CrosswordMap>(mapTextAsset.text);

            //초기화.
            boardController.GenerateBoard(crosswordMap);
            letterSelectionButtonContoller.Init(crosswordMap);
            answerChecker = new AnswerChecker(crosswordMap, boardController);
            boardHighlightController.Init(boardController);

            // 콜백 등록.
            boardController.boardClickEventHandler += wordMeaningController.OnCellBoardClick;
            boardController.boardClickEventHandler += letterSelectionButtonContoller.OnCellBoardClick;

            letterSelectionButtonContoller.letterSelectionButtonClickHandler += boardHighlightController.OnLetterSelectionBoardClick;

            boardHighlightController.onCorrectAnswer += OnCorrectAnswer;
            boardHighlightController.onWrongAnswer += OnWrongAnswer;
            //

            State = GameState.Play;

            // 첫번째 맞출 단어 설정.
            WordDataForGame unMatchedWord = answerChecker.GetUnMatchedWord();
            boardHighlightController.SetUpHighlightCells(unMatchedWord);
            wordMeaningController.SetText(unMatchedWord);
            letterSelectionButtonContoller.SetButtonsLetter(unMatchedWord);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                DarkMode.UseDarkMode = true;
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                DarkMode.UseDarkMode = false;
            }

        }

        public void OnCorrectAnswer(WordDataForGame wordDataForGame)
        {
            if (answerChecker.AddMatchedWord(wordDataForGame))
            {
                State = GameState.Clear;
            }
            else
            {
                WordDataForGame unMatchedWord = answerChecker.GetUnMatchedWord();
                boardHighlightController.SetUpHighlightCells(unMatchedWord);
                wordMeaningController.SetText(unMatchedWord);
                letterSelectionButtonContoller.SetButtonsLetter(unMatchedWord);
            }
        }

        public void OnWrongAnswer(WordDataForGame wordDataForGame)
        {
            letterSelectionButtonContoller.ChangeButtonsStateToAllBasic();
        }

        public void OnResetButtonClick()
        {
            boardHighlightController.SetUpHighlightCells(boardHighlightController.SelectedWordData);
            letterSelectionButtonContoller.SetButtonsLetter(boardHighlightController.SelectedWordData);
        }
    }
}