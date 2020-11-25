using UnityEngine;
using UnityEngine.UI;
using HealingJam.Crossword.Save;
using HealingJam.GameScreens;
using System;

namespace HealingJam.Crossword
{
    public class GameController : MonoBehaviour
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
        [SerializeField] private Text timeText = null; 

        private AnswerChecker answerChecker = null;
        public GameState State { get; set; }

        private ProgressData progressData = null;

        private void Start()
        {
            CrosswordMap crosswordMap = CrosswordMapManager.Instance.GetCrosswordMap(CrosswordMapManager.Instance.ActiveStageIndex);

            //초기화.
            boardController.GenerateBoard(crosswordMap);
            letterSelectionButtonController.Init();

            // 저장된 정보가 있다면.
            if (SaveMgr.Instance.TryGetProgressData(CrosswordMapManager.Instance.ActiveStageIndex, out progressData) && progressData != null)
            {
                foreach(var word in progressData.machedWords)
                {
                    boardController.SetCompleteWord(crosswordMap.GetWordData(word));
                }
            }
            else
            {
                progressData = new ProgressData();
            }

            answerChecker = new AnswerChecker(crosswordMap.wordDatas, boardController);
            boardHighlightController.Init(boardController);

            // 콜백 등록.
            boardController.boardClickEventHandler += wordMeaningController.OnCellBoardClick;
            boardController.boardClickEventHandler += letterSelectionButtonController.OnCellBoardClick;

            letterSelectionButtonController.letterSelectionButtonClickHandler += boardHighlightController.OnLetterSelectionBoardClick;

            boardHighlightController.onCorrectAnswer += OnCorrectAnswer;
            boardHighlightController.onWrongAnswer += OnWrongAnswer;
            //

            State = GameState.Play;

            // 첫번째 맞출 단어 설정.
            SetHighlightUnMatchedWord();
        }

        private void Update()
        {
            if (State == GameState.Play)
            {
                progressData.elapsedTime += Time.deltaTime;
                TimeSpan t = TimeSpan.FromSeconds(progressData.elapsedTime);
                string str = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);

                timeText.text = str;
            }
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                if (progressData != null)
                {
                    SaveMgr.Instance.SetProgressData(CrosswordMapManager.Instance.ActiveStageIndex, progressData);
                    SaveMgr.Instance.Save();
                }
            }
        }

        public void OnCorrectAnswer(WordDataForGame wordDataForGame)
        {
            if (answerChecker.AddMatchedWord(wordDataForGame.word))
            {
                State = GameState.Clear;

                // 클리어 정보 저장.
                bool perfectClear = progressData.wrongCountTypes.Values.Count == 0;
                SaveMgr.Instance.SetCompleteData(CrosswordMapManager.Instance.ActiveStageIndex, new CompleteData() { perfectClear = perfectClear });
                // 클리어시에 진행상황 삭제.
                SaveMgr.Instance.DeleteProgressData(CrosswordMapManager.Instance.ActiveStageIndex);
                SaveMgr.Instance.Save();

                answerOXResult.ShowOResult(OnClear);
            }
            else
            {
                // 진행상황에 단어 추가.
                progressData.machedWords.Add(wordDataForGame.word);

                answerOXResult.ShowOResult(SetHighlightUnMatchedWord);
            }
        }

        public void OnWrongAnswer(WordDataForGame wordDataForGame)
        {
            if (progressData.wrongCountTypes.ContainsKey(wordDataForGame.wordType))
                progressData.wrongCountTypes[wordDataForGame.wordType]++;
            else
                progressData.wrongCountTypes.Add(wordDataForGame.wordType, 1);

            answerOXResult.ShowXResult(OnWrongAnimationEnd);
        }

        private void OnClear()
        {
            ScreenMgr.Instance.ChangeState(GameScreen.ScreenID.Clear);
        }

        private void SetHighlightUnMatchedWord()
        {
            WordDataForGame unMatchedWord = answerChecker.GetUnMatchedWord();
            boardHighlightController.SetUpHighlightCells(unMatchedWord);
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
    }
}