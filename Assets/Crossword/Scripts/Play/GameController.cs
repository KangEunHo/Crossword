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
        [SerializeField] private HintController hintController = null;

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
            hintController.Init(boardHighlightController, letterSelectionButtonController);

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

        private void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                SaveProgressData();
            }
        }

        public void SaveProgressData()
        {
            if (progressData != null)
            {
                SaveMgr.Instance.SetProgressData(CrosswordMapManager.Instance.ActiveStageIndex, progressData);
                SaveMgr.Instance.Save();
            }
        }

        public void OnCorrectAnswer(WordDataForGame wordDataForGame)
        {
            if (answerChecker.AddMatchedWord(wordDataForGame.word))
            {
                State = GameState.Clear;


                SaveMgr.Instance.SetCompleteData(CrosswordMapManager.Instance.ActiveStageIndex, true);
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
            //if (progressData.wrongCountTypes.ContainsKey(wordDataForGame.wordType))
            //    progressData.wrongCountTypes[wordDataForGame.wordType]++;
            //else
            //    progressData.wrongCountTypes.Add(wordDataForGame.wordType, 1);

            answerOXResult.ShowXResult(OnWrongAnimationEnd);
        }

        private void OnClear()
        {
            ScreenMgr.Instance.ChangeState(GameScreen.ScreenID.Clear);
        }

        private void SetHighlightUnMatchedWord()
        {
            WordDataForGame wordDataForGame = boardHighlightController.SelectedWordData;
            WordDataForGame unMatchedWord = null;
            if (wordDataForGame == null)
            {
                unMatchedWord = answerChecker.GetUnMatchedWord();
            }
            else
            {
                unMatchedWord = answerChecker.GetUnMatchedWordByConnecte(wordDataForGame, boardController);
                if (unMatchedWord == null)
                {
                    //Vector2Int lastIndex = new Vector2Int(wordDataForGame.x, wordDataForGame.y);
                    //if (wordDataForGame.direction == WordDataForGame.Direction.Horizontal)
                    //    lastIndex.x += wordDataForGame.word.Length;
                    //else
                    //    lastIndex.y += wordDataForGame.word.Length;

                    unMatchedWord = answerChecker.GetUnMatchedWordOrderByRange(new Vector2Int(wordDataForGame.x, wordDataForGame.y));
                }
            }

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