﻿using UnityEngine;
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
        [SerializeField] private WordMeaningController bigWordMeaningController = null;
        [SerializeField] private BoardHighlightController boardHighlightController = null;
        [SerializeField] private AnswerOXResult answerOXResult = null;
        [SerializeField] private HintController hintController = null;
        [SerializeField] private PressChecker zoomPressChecker = null;

        private AnswerChecker answerChecker = null;
        public GameState State { get; set; }

        private ProgressData progressData = null;

        private void Start()
        {
            CrosswordMap crosswordMap = CrosswordMapManager.Instance.GetCrosswordMap(CrosswordMapManager.Instance.ActivePackIndex);

            //초기화.
            boardController.GenerateBoard(crosswordMap);
            letterSelectionButtonController.Init();

            // 저장된 정보가 있다면.
            if (SaveMgr.Instance.TryGetProgressData(CrosswordMapManager.Instance.ActivePackIndex, out progressData) && progressData != null)
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
            boardController.boardClickEventHandler += OnCellBoardClick;

            letterSelectionButtonController.letterSelectionButtonClickHandler += boardHighlightController.OnLetterSelectionBoardClick;

            boardHighlightController.onCorrectAnswer += OnCorrectAnswer;
            boardHighlightController.onWrongAnswer += OnWrongAnswer;
            //

            State = GameState.Play;

            // 첫번째 맞출 단어 설정.
            SetHighlightUnMatchedWord();

            zoomPressChecker.OnPointerDown = OnZoomButtonPressDown;
            zoomPressChecker.OnPointerUp = OnZoomButtonPressUp;
        }

        private void OnZoomButtonPressDown()
        {
            bigWordMeaningController.gameObject.SetActive(true);
        }

        private void OnZoomButtonPressUp()
        {
            bigWordMeaningController.gameObject.SetActive(false);
        }
#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                OnClear();
            }
        }
#endif

        public void OnCellBoardClick(object sender, BoardClickEvent boardClickEvent)
        {
            BoardCell boardCell = boardClickEvent.boardCell;
            WordDataForGame.Direction direction = boardClickEvent.direction;

            if (direction == WordDataForGame.Direction.None)
            {
                wordMeaningController.SetText(null);
                bigWordMeaningController.SetText(null);
            }
            else if (direction == WordDataForGame.Direction.Horizontal)
            {
                wordMeaningController.SetText(boardCell.HorizontalWordData);
                bigWordMeaningController.SetText(boardCell.HorizontalWordData);
                SetHighlightCellsAndLetterButtons(boardCell.HorizontalWordData);
            }
            else if (direction == WordDataForGame.Direction.Vertical)
            {
                wordMeaningController.SetText(boardCell.VerticalWordData);
                bigWordMeaningController.SetText(boardCell.VerticalWordData);
                SetHighlightCellsAndLetterButtons(boardCell.VerticalWordData);
            }
        }

        public void SetHighlightCellsAndLetterButtons(WordDataForGame wordDataForGame)
        {
            boardHighlightController.SetUpHighlightCells(wordDataForGame);
            letterSelectionButtonController.SetButtonsLetter(wordDataForGame);
            SetCompletedLetterSelectionButton(wordDataForGame);
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
                SaveMgr.Instance.SetProgressData(CrosswordMapManager.Instance.ActivePackIndex, progressData);
                SaveMgr.Instance.Save();
            }
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
                // 진행상황에 단어 추가.
                progressData.machedWords.Add(wordDataForGame.word);

                answerOXResult.ShowOResult(SetHighlightUnMatchedWord);
            }
        }

        public void OnWrongAnswer(WordDataForGame wordDataForGame)
        {
            SaveMgr.Instance.AddCoin(-1);
            if (SettingMgr.UseVibration)
                Vibration.Vibrate();
            answerOXResult.ShowXResult(OnWrongAnimationEnd);
        }

        private void OnClear()
        {
            bool alreadyCompleted = SaveMgr.Instance.GetCompleteData(CrosswordMapManager.Instance.ActivePackIndex);
            SaveMgr.Instance.SetCompleteData(CrosswordMapManager.Instance.ActivePackIndex, true);
            // 클리어시에 진행상황 삭제.
            SaveMgr.Instance.DeleteProgressData(CrosswordMapManager.Instance.ActivePackIndex);
            SaveMgr.Instance.Save();
            ScreenMgr.Instance.ChangeState(GameScreen.ScreenID.Clear, alreadyCompleted);
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
                    unMatchedWord = answerChecker.GetUnMatchedWordOrderByRange(new Vector2Int(wordDataForGame.x, wordDataForGame.y));
                }
            }

            wordMeaningController.SetText(unMatchedWord);
            bigWordMeaningController.SetText(unMatchedWord);
            SetHighlightCellsAndLetterButtons(unMatchedWord);
        }

        private void OnWrongAnimationEnd()
        {
            boardHighlightController.SetUpHighlightCells(boardHighlightController.SelectedWordData);
            letterSelectionButtonController.ChangeButtonsStateToAllBasic();
            SetCompletedLetterSelectionButton(boardHighlightController.SelectedWordData);
        }

        public void OnResetButtonClick()
        {
            SetHighlightCellsAndLetterButtons(boardHighlightController.SelectedWordData);
        }

        private void SetCompletedLetterSelectionButton(WordDataForGame wordDataForGame)
        {
            for (int i = 0; i < wordDataForGame.word.Length; ++i)
            {
                int x = wordDataForGame.x;
                int y = wordDataForGame.y;
                if (wordDataForGame.direction == WordDataForGame.Direction.Horizontal)
                    x += i;
                else y += i;

                if (boardController.GetBoardCell(new Vector2Int(x, y)).State == BoardCell.CellState.Completed)
                {
                    char completedCharacter = wordDataForGame.word[i];
                    letterSelectionButtonController.LetterSelectionButtonStateChangeSameLetter(completedCharacter);
                }
            }
        }
    }
}