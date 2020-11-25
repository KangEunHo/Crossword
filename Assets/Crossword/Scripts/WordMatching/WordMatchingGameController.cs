using UnityEngine;
using UnityEngine.UI;
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

        [SerializeField] private BoardController boardController = null;
        [SerializeField] private LetterSelectionButtonController letterSelectionButtonController = null;
        [SerializeField] private WordMeaningController wordMeaningController = null;
        [SerializeField] private BoardHighlightController boardHighlightController = null;
        [SerializeField] private AnswerOXResult answerOXResult = null;
        [SerializeField] private Text progressText = null;

        private AnswerChecker answerChecker = null;
        public GameState State { get; private set; }

        public float ElapsedTime { get; private set; }
        private int maxAnswerCount;
        private int curAnswerIndex;

        public void SetUp(List<WordDataForGame> answers)
        {
            //초기화.

            letterSelectionButtonController.Init();

            boardHighlightController.Init(boardController);

            letterSelectionButtonController.letterSelectionButtonClickHandler += boardHighlightController.OnLetterSelectionBoardClick;

            boardHighlightController.onCorrectAnswer += OnCorrectAnswer;
            boardHighlightController.onWrongAnswer += OnWrongAnswer;
            //

            State = GameState.Play;

            // 첫번째 맞출 단어 설정.
            answerChecker = new AnswerChecker(answers, null);
            SetHighlightUnMatchedWord();

            ElapsedTime = 0f;

            maxAnswerCount = answers.Count;
            curAnswerIndex = 0;
            ProgressTextUpdate();
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

                answerOXResult.ShowOResult(OnClear);
            }
            else
            {
                answerOXResult.ShowOResult(SetHighlightUnMatchedWord);
            }

            curAnswerIndex++;
            ProgressTextUpdate();
        }

        public void OnWrongAnswer(WordDataForGame wordDataForGame)
        {
            answerOXResult.ShowXResult(OnWrongAnimationEnd);
        }

        private void OnClear()
        {
            ScreenMgr.Instance.ChangeState(GameScreen.ScreenID.Clear);
        }

        private void SetHighlightUnMatchedWord()
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

        private void OnWrongAnimationEnd()
        {
            WordDataForGame wordDataForGame = boardHighlightController.SelectedWordData;
            boardHighlightController.SetUpHighlightCells(wordDataForGame);
            letterSelectionButtonController.ChangeButtonsStateToAllBasic();
        }

        public void OnResetButtonClick()
        {
            boardHighlightController.SetUpHighlightCells(boardHighlightController.SelectedWordData);
            letterSelectionButtonController.SetButtonsLetter(boardHighlightController.SelectedWordData);
        }

        private void ProgressTextUpdate()
        {
            progressText.text = curAnswerIndex.ToString() + '/' + maxAnswerCount.ToString();
        }
    }
}