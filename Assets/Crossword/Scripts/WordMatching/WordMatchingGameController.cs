using UnityEngine;
using HealingJam.Crossword.Save;
using HealingJam.GameScreens;
using System.Collections.Generic;

namespace HealingJam.Crossword
{
    public class WordMatchingGameController : MonoBehaviour
    {
        public enum GameState
        {
            Pause, Play, Fail, Clear
        }

        [SerializeField] private TextAsset mapTextAsset = null;
        [SerializeField] private BoardController boardController = null;
        [SerializeField] private LetterSelectionButtonController letterSelectionButtonController = null;
        [SerializeField] private WordMeaningController wordMeaningController = null;
        [SerializeField] private BoardHighlightController boardHighlightController = null;

        private AnswerChecker answerChecker = null;
        public GameState State { get; private set; }

        public float ElapsedTime { get; private set; }

        public void SetUp(params CrosswordMap[] crosswordMaps)
        {
            //초기화.

            letterSelectionButtonController.Init();
            letterSelectionButtonController.SetUpDatabase(crosswordMaps);

            boardHighlightController.Init(boardController);

            letterSelectionButtonController.letterSelectionButtonClickHandler += boardHighlightController.OnLetterSelectionBoardClick;

            boardHighlightController.onCorrectAnswer += OnCorrectAnswer;
            boardHighlightController.onWrongAnswer += OnWrongAnswer;
            //

            State = GameState.Play;

            // 첫번째 맞출 단어 설정.
            answerChecker = new AnswerChecker(crosswordMaps[0].wordDatas, null);
            WordData unMatchedWord = answerChecker.GetUnMatchedWord();
            boardController.GenerateBoard(unMatchedWord);
            WordDataForGame wordDataForGame = new WordDataForGame()
            {
                direction = WordDataForGame.Direction.Horizontal,
                x = 0,
                y = 0,
                info = unMatchedWord.info,
                word = unMatchedWord.word,
                wordType = unMatchedWord.wordType
            };
            boardHighlightController.SetUpHighlightCells(wordDataForGame);
            wordMeaningController.SetText(unMatchedWord);
            letterSelectionButtonController.SetButtonsLetter(unMatchedWord);
            
            ElapsedTime = 0f;
        }


        private void Update()
        {
            if (State == GameState.Play)
                ElapsedTime += Time.deltaTime;
        }

        public void OnCorrectAnswer(WordDataForGame wordDataForGame)
        {
            if (answerChecker.AddMatchedWord(wordDataForGame.word))
            {
                State = GameState.Clear;

                ScreenMgr.Instance.ChangeState(GameScreen.ScreenID.Clear);
            }
            else
            {
                WordData unMatchedWord = answerChecker.GetUnMatchedWord();
                boardController.GenerateBoard(unMatchedWord);
                WordDataForGame wordData = new WordDataForGame()
                {
                    direction = WordDataForGame.Direction.Horizontal,
                    x = 0,
                    y = 0,
                    info = unMatchedWord.info,
                    word = unMatchedWord.word,
                    wordType = unMatchedWord.wordType
                };
                boardHighlightController.SetUpHighlightCells(wordData);
                wordMeaningController.SetText(unMatchedWord);
                letterSelectionButtonController.SetButtonsLetter(unMatchedWord);
            }
        }

        public void OnWrongAnswer(WordDataForGame wordDataForGame)
        {
            letterSelectionButtonController.ChangeButtonsStateToAllBasic();
        }

        public void OnResetButtonClick()
        {
            boardHighlightController.SetUpHighlightCells(boardHighlightController.SelectedWordData);
            letterSelectionButtonController.SetButtonsLetter(boardHighlightController.SelectedWordData);
        }
    }
}