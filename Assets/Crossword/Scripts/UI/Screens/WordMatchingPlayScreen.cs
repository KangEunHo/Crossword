using UnityEngine;
using UnityEngine.UI;
using HealingJam.GameScreens;
using HealingJam.Popups;
using System.Collections.Generic;

namespace HealingJam.Crossword.UI
{
    public class WordMatchingPlayScreen : FadeAndScaleTweenScreen
    {
        public enum GameMode
        {
            BadgePlay, AbilityTest
        }

        [SerializeField] private WordMatchingGameController gameController = null;
        [SerializeField] private Text infoText = null;
        private GameMode gameMode;

        public override void Enter(params object[] args)
        {
            base.Enter(args);
            gameMode = (GameMode)args[0];

            if (gameMode == GameMode.AbilityTest)
            {
                infoText.text = "10가지 상식 분야 중\n 랜덤으로 20문제가 출제됩니다.\n테스트를 통해 부족한 분야를 확인해보세요.\n(제한 시간: 5분)";
                gameController.SetUp(gameMode, CreateAbilityTestAnswers());
            }
            else
            {
                infoText.text = string.Format("({0}) 레벨에 대한 테스트를 시작합니다.\n레벨 테스트를 완료하면\n다음 레벨의 문제를 풀 수 있습니다.", 
                    (CrosswordMapManager.Instance.ActiveLevelIndex +1));
                gameController.SetUp(gameMode, CreateBadgePlayAnswers());
            }

            PopupMgr.Instance.StateMachine.OnStateEntered += OnPopupEntered;
            PopupMgr.Instance.StateMachine.OnStateExited += OnPopupExited;
        }

        public override void Exit(params object[] args)
        {
            base.Exit(args);

            PopupMgr.Instance.StateMachine.OnStateEntered -= OnPopupEntered;
            PopupMgr.Instance.StateMachine.OnStateExited -= OnPopupExited;
        }
        // 뱃지문제에 사용되는 문제들을 만듭니다.
        // 해당 레벨에서 문제들이 출제됩니다.
        private List<WordDataForGame> CreateBadgePlayAnswers()
        {
            Dictionary<WordData.WordType, List<WordDataForGame>> allAnswers = new Dictionary<WordData.WordType, List<WordDataForGame>>();
            for (int i = 1; i < (int)WordData.WordType.Max; ++i)
            {
                allAnswers.Add((WordData.WordType)i, new List<WordDataForGame>());
            }
            List<WordDataForGame> answers = new List<WordDataForGame>();

            int levelIndex = CrosswordMapManager.Instance.ActiveLevelIndex;
            for (int i = 0; i < CrosswordMapManager.LEVEL_IN_PACK_COUNT; ++i)
            {
                int packIndex = levelIndex * CrosswordMapManager.LEVEL_IN_PACK_COUNT + i;
                CrosswordMap crosswordMap = CrosswordMapManager.Instance.GetCrosswordMap(packIndex);

                foreach(var wordData in crosswordMap.wordDatas)
                {
                    allAnswers[wordData.wordType].Add(wordData);
                }
            }

            for (int i = 1; i < (int)WordData.WordType.Max; ++i)
            {
                List<WordDataForGame> words = allAnswers[(WordData.WordType)i];
                
                // 타입별로 랜덤하게 2문제씩을 가져옵니다. 
                for (int j = 0; j < 2; ++j)
                {
                    int randomIndex = Random.Range(0, words.Count);
                    WordDataForGame word = words[randomIndex];
                    words.RemoveAt(randomIndex);
                    WordDataForGame wordDataForGame = new WordDataForGame()
                    {
                        direction = WordDataForGame.Direction.Horizontal,
                        info = word.info,
                        word = word.word,
                        wordType = word.wordType,
                        x = 0,
                        y = 0
                    };
                    answers.Add(wordDataForGame);
                }
            }
            answers.Shuffle();
            return answers;
        }

        // 실력테스트에 사용되는 문제들을 만듭니다.
        // 모든 맵에서 문제가 출제됩니다.
        private List<WordDataForGame> CreateAbilityTestAnswers()
        {
            Dictionary<WordData.WordType, List<WordDataForGame>> allAnswers = new Dictionary<WordData.WordType, List<WordDataForGame>>();
            for (int i = 1; i < (int)WordData.WordType.Max; ++i)
            {
                allAnswers.Add((WordData.WordType)i, new List<WordDataForGame>());
            }
            List<WordDataForGame> answers = new List<WordDataForGame>();

            List<int> crosswordMapIndexes = new List<int>();
            int maxStage = CrosswordMapManager.Instance.MaxStage();
            for (int i = 0; i < maxStage; ++i)
            {
                crosswordMapIndexes.Add(i);
            }
            crosswordMapIndexes.Shuffle();

            int levelIndex = CrosswordMapManager.Instance.ActiveLevelIndex;
            for (int i = 0; i < maxStage; ++i)
            {
                CrosswordMap crosswordMap = CrosswordMapManager.Instance.GetCrosswordMap(i);

                foreach (var wordData in crosswordMap.wordDatas)
                {
                    allAnswers[wordData.wordType].Add(wordData);
                }

                if (i > 10)
                {
                    bool complete = true;
                    foreach(var v in allAnswers.Values)
                    {
                        if (v.Count < 2)
                        {
                            complete = false;
                            break;
                        }
                    }
                    if (complete)
                        break;
                }
            }

            for (int i = 1; i < (int)WordData.WordType.Max; ++i)
            {
                List<WordDataForGame> words = allAnswers[(WordData.WordType)i];

                // 타입별로 랜덤하게 2문제씩을 가져옵니다. 
                for (int j = 0; j < 2; ++j)
                {
                    int randomIndex = Random.Range(0, words.Count);
                    WordDataForGame word = words[randomIndex];
                    words.RemoveAt(randomIndex);
                    WordDataForGame wordDataForGame = new WordDataForGame()
                    {
                        direction = WordDataForGame.Direction.Horizontal,
                        info = word.info,
                        word = word.word,
                        wordType = word.wordType,
                        x = 0,
                        y = 0
                    };
                    answers.Add(wordDataForGame);
                }
            }

            answers.Shuffle();
            return answers;
        }

        protected override void OnExitFadeComplete(params object[] args)
        {
            ScreenMgr.Instance.UnRegisterState(this);
        }

        public override void Escape()
        {
            OnBackButtonClick();
        }

        public void OnBackButtonClick()
        {
            string message = "나가시겠습니까?\n 나가게 되면 보상을 받을 수 없습니다.";

            PopupMgr.Instance.EnterWithAnimation(Popup.PopupID.PlayExit, new MoveTweenPopupAnimation(MoveTweenPopupAnimation.MoveDirection.BottonToCenter, 0.25f),
                new PopupClosedDelegate(OnPlayExitPopupClosed), message);

            SoundMgr.Instance.PlayOneShotButtonSound();
        }


        public void OnPopupEntered(object sender, StateMachine.StateEventArgs<Popup> stateEventArgs)
        {
            if (gameController.State == WordMatchingGameController.GameState.Play)
            {
                gameController.State = WordMatchingGameController.GameState.Pause;
            }
        }

        public void OnPopupExited(object sender, StateMachine.StateEventArgs<Popup> stateEventArgs)
        {
            if (PopupMgr.Instance.StateMachine.CurrentState() == null)
            {
                if (gameController.State == WordMatchingGameController.GameState.Pause)
                {
                    gameController.State = WordMatchingGameController.GameState.Play;
                }
            }
        }

        private void OnPlayExitPopupClosed(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                gameController.State = WordMatchingGameController.GameState.Play;
            }
            else
            {
                if (message == "continue")
                {
                    gameController.State = WordMatchingGameController.GameState.Play;
                }
                else if (message == "exit")
                {
                    ScreenMgr.Instance.ChangeState(ScreenID.Title);
                }
            }
        }

        public void OnModifyReviewButtonClick()
        {
            PopupMgr.Instance.EnterWithAnimation(Popup.PopupID.ModifyReview, new MoveTweenPopupAnimation(MoveTweenPopupAnimation.MoveDirection.BottonToCenter, 0.25f),
                null, (CrosswordMapManager.Instance.ActivePackIndex));

            SoundMgr.Instance.PlayOneShotButtonSound();
        }
    }
}