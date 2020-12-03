﻿using UnityEngine;
using UnityEngine.UI;
using System;
using HealingJam.GameScreens;
using System.Collections.Generic;
using HealingJam.Crossword.UI;
using DG.Tweening;
using HealingJam.Crossword.Save;

namespace HealingJam.Crossword
{
    // 맞춘 갯수랑 문제갯수 데이터.
    public class RightAnswerCountData
    {
        public int rightAnswerCount;
        public int answerCount;
    }

    public class WordMatchingGameController : MonoBehaviour
    {
        private const float ANSWER_RATE_GAUGE_WIDTH = 380f;
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
        [SerializeField] private ReadyAnimator readyAnimator = null;
        [SerializeField] private Text progressText = null;
        [SerializeField] private Text timeText = null;
        [SerializeField] private GameObject timeObject = null;
        [SerializeField] private RectTransform answerRateGauge = null;
        [SerializeField] private Text percentText = null;

        private AnswerChecker answerChecker = null;
        public GameState State { get; set; }

        private WordMatchingPlayScreen.GameMode gameMode;
        public float ElapsedTime { get; private set; }
        private int maxAnswerCount;
        private int curAnswerIndex;
        private int answerCount;
        private Tween answerRateGaugeTween = null;

        public List<RightAnswerCountData> rightAnswerCountDatas = new List<RightAnswerCountData>();
        private List<AnswerItem.AnswerItemData> answerItemDatas = null;

        public void SetUp(WordMatchingPlayScreen.GameMode gameMode, List<WordDataForGame> answers)
        {
            //초기화.

            letterSelectionButtonController.Init();

            boardHighlightController.Init(boardController);
            hintController.Init(boardHighlightController, letterSelectionButtonController);

            letterSelectionButtonController.letterSelectionButtonClickHandler += boardHighlightController.OnLetterSelectionBoardClick;

            boardHighlightController.onCorrectAnswer += OnCorrectAnswer;
            boardHighlightController.onWrongAnswer += OnWrongAnswer;
            //

            State = GameState.Pause;

            readyAnimator.PlayAnimation(() =>
            {
                answerChecker = new AnswerChecker(answers, null);
                SetHighlightUnMatchedWord();
                State = GameState.Play;
            });
            // 첫번째 맞출 단어 설정.


            ElapsedTime = 0f;

            maxAnswerCount = answers.Count;
            curAnswerIndex = 0;

            for (int i = 0; i < (int)WordData.WordType.Max; ++i)
            {
                rightAnswerCountDatas.Add(new RightAnswerCountData());
            }

            answerItemDatas = new List<AnswerItem.AnswerItemData>();
            foreach (var answer in answers)
            {
                answerItemDatas.Add(new AnswerItem.AnswerItemData()
                {
                    correctAnswer = false,
                    wordData = answer
                });
                rightAnswerCountDatas[(int)answer.wordType].answerCount++;
            }

            ProgressTextUpdate();

            this.gameMode = gameMode;
            if (gameMode == WordMatchingPlayScreen.GameMode.AbilityTest)
            {
                timeObject.SetActive(true);
            }
            else
            {
                timeObject.SetActive(false);
            }
        }

        private void Update()
        {
            if (State == GameState.Play && gameMode == WordMatchingPlayScreen.GameMode.AbilityTest)
            {
                ElapsedTime += Time.deltaTime;
                TimeSpan t = TimeSpan.FromSeconds(ElapsedTime);
                string str = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);

                timeText.text = str;
            }
        }

        public void OnCorrectAnswer(WordDataForGame wordDataForGame)
        {
            rightAnswerCountDatas[(int)wordDataForGame.wordType].rightAnswerCount++;
            answerItemDatas[curAnswerIndex].correctAnswer = true;

            answerCount++;
            curAnswerIndex++;
            ProgressTextUpdate();
            AnswerRateUpdate();

            if (answerChecker.AddMatchedWord(wordDataForGame.word))
            {
                State = GameState.Clear;

                answerOXResult.ShowOResult(OnClear);
            }
            else
            {
                answerOXResult.ShowOResult(SetHighlightUnMatchedWord);
            }
        }

        public void OnWrongAnswer(WordDataForGame wordDataForGame)
        {
            answerItemDatas[curAnswerIndex].correctAnswer = false;
            curAnswerIndex++;
            ProgressTextUpdate();
            AnswerRateUpdate();

            if (answerChecker.AddMatchedWord(wordDataForGame.word))
            {
                State = GameState.Clear;

                answerOXResult.ShowXResult(OnClear);
            }
            else
            {
                answerOXResult.ShowXResult(SetHighlightUnMatchedWord);
            }
            //answerOXResult.ShowXResult(SetHighlightUnMatchedWord);// OnWrongAnimationEnd);

        }

        private void OnClear()
        {
            ScreenMgr.Instance.ChangeState(GameScreen.ScreenID.CommonSenseResult, answerItemDatas, rightAnswerCountDatas);
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

        private void AnswerRateUpdate()
        {
            float answerRate = 0;
            if (curAnswerIndex > 0)
            {
                answerRate = (answerCount / (float)curAnswerIndex);
            }

            percentText.text = (answerRate * 100).ToString("F0") + "%";

            if (answerRateGaugeTween != null)
                answerRateGaugeTween.Kill(true);

            float x = ANSWER_RATE_GAUGE_WIDTH * answerRate;
            float y = answerRateGauge.sizeDelta.y;
            answerRateGaugeTween = answerRateGauge.DOSizeDelta(new Vector2(x, y), 0.5f);
        }
    }
}