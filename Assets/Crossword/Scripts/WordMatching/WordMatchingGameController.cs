using UnityEngine;
using UnityEngine.UI;
using System;
using HealingJam.GameScreens;
using System.Collections.Generic;
using HealingJam.Crossword.UI;
using DG.Tweening;
using HealingJam.Crossword.Save;

namespace HealingJam.Crossword
{
    public class WordMatchingGameController : MonoBehaviour
    {
        private const float ANSWER_RATE_GAUGE_WIDTH = 380f;
        public enum GameState
        {
            Ready, Pause, Play, Fail, Clear
        }

        [SerializeField] private BoardController boardController = null;
        [SerializeField] private LetterSelectionButtonController letterSelectionButtonController = null;
        [SerializeField] private WordMeaningController wordMeaningController = null;
        [SerializeField] private WordMeaningController bigWordMeaningController = null;
        [SerializeField] private BoardHighlightController boardHighlightController = null;
        [SerializeField] private AnswerOXResult answerOXResult = null;
        [SerializeField] private HintController hintController = null;
        [SerializeField] private ReadyAnimator readyAnimator = null;
        [SerializeField] private Text progressText = null;
        [SerializeField] private Text timeText = null;
        [SerializeField] private GameObject timeObject = null;
        [SerializeField] private RectTransform answerRateGauge = null;
        [SerializeField] private Text percentText = null;
        [SerializeField] private WrongWordAnimator wrongWordAnimator = null;
        [SerializeField] private PressChecker zoomPressChecker = null;

        private AnswerChecker answerChecker = null;
        public GameState State { get; set; }

        private WordMatchingPlayScreen.GameMode gameMode;
        public float LimitTime { get; private set; }
        private int maxAnswerCount;
        private int curAnswerIndex;
        private int answerCount;
        private Tween answerRateGaugeTween = null;

        public List<RightAnswerCountData> rightAnswerCountDatas = new List<RightAnswerCountData>();
        private List<AnswerItem.AnswerItemData> answerItemDatas = null;


        public void SetUp(WordMatchingPlayScreen.GameMode gameMode, List<WordDataForGame> answers)
        {
            this.gameMode = gameMode;
            //초기화.

            letterSelectionButtonController.Init();

            boardHighlightController.Init(boardController);
            hintController.Init(boardHighlightController, letterSelectionButtonController);

            letterSelectionButtonController.letterSelectionButtonClickHandler += boardHighlightController.OnLetterSelectionBoardClick;

            boardHighlightController.onCorrectAnswer += OnCorrectAnswer;
            boardHighlightController.onWrongAnswer += OnWrongAnswer;
            //

            State = GameState.Ready;

            GameMgr.Instance.topUIController.SetActiveBackButton(false);
            GameMgr.Instance.topUIController.SetActiveCoinButton(false);
            GameMgr.Instance.topUIController.SetActiveOptionButton(false);

            readyAnimator.PlayAnimation(() =>
            {
                answerChecker = new AnswerChecker(answers, null);
                SetHighlightUnMatchedWord();
                State = GameState.Play;

                GameMgr.Instance.topUIController.SetActiveBackButton(true);
                GameMgr.Instance.topUIController.SetActiveCoinButton(true);
                GameMgr.Instance.topUIController.SetActiveOptionButton(true);
            });
            // 첫번째 맞출 단어 설정.


            LimitTime = 60 * 5f;

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

            if (gameMode == WordMatchingPlayScreen.GameMode.AbilityTest)
            {
                timeObject.SetActive(true);
            }
            else
            {
                timeObject.SetActive(false);
            }

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

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.C))
            {
                OnClear();
            }
#endif

            if (State == GameState.Play && gameMode == WordMatchingPlayScreen.GameMode.AbilityTest)
            {
                LimitTime -= Time.deltaTime;

                if (LimitTime <= 0)
                {
                    LimitTime = 0f;
                    State = GameState.Fail;
                    OnClear();
                }
                TimeSpan t = TimeSpan.FromSeconds(LimitTime);
                string str = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);

                timeText.text = str;
            }
        }

        public void OnDebugButtonClick()
        {
            OnClear();
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

            SoundMgr.Instance.PlayOneShot(SoundMgr.Instance.rightAnswer);
        }

        public void OnWrongAnswer(WordDataForGame wordDataForGame)
        {
            SaveMgr.Instance.AddCoin(-1);
            if (SettingMgr.UseVibration)
                Vibration.Vibrate();

            answerItemDatas[curAnswerIndex].correctAnswer = false;
            curAnswerIndex++;
            ProgressTextUpdate();
            AnswerRateUpdate();

            if (answerChecker.AddMatchedWord(wordDataForGame.word))
            {
                State = GameState.Clear;

                answerOXResult.ShowXResult(()=> { wrongWordAnimator.PlayAnimation(OnClear, wordDataForGame.word); });
            }
            else
            {
                answerOXResult.ShowXResult(() => { wrongWordAnimator.PlayAnimation(SetHighlightUnMatchedWord, wordDataForGame.word); });
            }

            SoundMgr.Instance.PlayOneShot(SoundMgr.Instance.wrongAnswer);
        }

        private void OnClear()
        {
            bool alreadyCompleted = false;
            if (gameMode == WordMatchingPlayScreen.GameMode.AbilityTest)
            {

            }
            else
            {
                alreadyCompleted = SaveMgr.Instance.GetLevelData(CrosswordMapManager.Instance.ActiveLevelIndex).completed;
                SaveMgr.Instance.SetLevelData(CrosswordMapManager.Instance.ActiveLevelIndex, new LevelData()
                {
                    completed = true,
                    rightAnswerCountDatas = rightAnswerCountDatas
                });
                SaveMgr.Instance.Save();
            }
            ScreenMgr.Instance.ChangeState(GameScreen.ScreenID.CommonSenseResult, gameMode, alreadyCompleted, answerItemDatas, rightAnswerCountDatas);
        }

        private void SetHighlightUnMatchedWord()
        {
            WordData unMatchedWord = answerChecker.GetUnMatchedWord();
            boardHighlightController.OffCellGradation();
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
            bigWordMeaningController.SetText(unMatchedWord);
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
            SetCompletedLetterSelectionButton(boardHighlightController.SelectedWordData);

            SoundMgr.Instance.PlayOneShotButtonSound();
        }

        private void ProgressTextUpdate()
        {
            progressText.text = Mathf.Min((curAnswerIndex +1), maxAnswerCount).ToString() + '/' + maxAnswerCount.ToString();
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