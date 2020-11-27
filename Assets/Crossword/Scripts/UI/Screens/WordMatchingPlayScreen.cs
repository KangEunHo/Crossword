using UnityEngine;
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
        private GameMode gameMode;

        public override void Enter(params object[] args)
        {
            base.Enter(args);
            gameMode = (GameMode)args[0];

            if (gameMode == GameMode.AbilityTest)
            {
                gameController.SetUp(gameMode, CreateAbilityTestAnswers());
            }
            else
            {
                gameController.SetUp(gameMode, CreateBadgePlayAnswers());
            }
        }

        // 뱃지문제에 사용되는 문제들을 만듭니다.
        // 해당 레벨에서 문제들이 출제됩니다.
        private List<WordDataForGame> CreateBadgePlayAnswers()
        {
            Dictionary<WordData.WordType, List<WordDataForGame>> allAnswers = new Dictionary<WordData.WordType, List<WordDataForGame>>();
            for (int i = 0; i < (int)WordData.WordType.Max; ++i)
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

            for (int i = 0; i < (int)WordData.WordType.Max; ++i)
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

            return answers;
        }

        protected override void OnExitFadeComplete(params object[] args)
        {
            ScreenMgr.Instance.UnRegisterState(this);
        }

        public override void Escape()
        {
            ScreenMgr.Instance.ChangeState(ScreenID.StageSelect);
        }

        public void OnCoinButtonClick()
        {
            if (gameController.State == WordMatchingGameController.GameState.Play)
            {
                gameController.State = WordMatchingGameController.GameState.Pause;

                PopupMgr.Instance.EnterWithAnimation(Popup.PopupID.Shop, new MoveTweenPopupAnimation(MoveTweenPopupAnimation.MoveDirection.BottonToCenter, 0.25f),
                    new PopupClosedDelegate(OnPopupClosed));
            }
        }

        public void OnOptionButtonClick()
        {
            if (gameController.State == WordMatchingGameController.GameState.Play)
            {
                gameController.State = WordMatchingGameController.GameState.Pause;

                PopupMgr.Instance.EnterWithAnimation(Popup.PopupID.Option, new MoveTweenPopupAnimation(MoveTweenPopupAnimation.MoveDirection.BottonToCenter, 0.25f),
                    new PopupClosedDelegate(OnPopupClosed));
            }
        }

        public void OnBackButtonClick()
        {
            if (gameController.State == WordMatchingGameController.GameState.Play)
            {
                gameController.State = WordMatchingGameController.GameState.Pause;

                string message = "나가시겠습니까?\n 나가게 되면 보상을 받을 수 없습니다.";

                PopupMgr.Instance.EnterWithAnimation(Popup.PopupID.PlayExit, new MoveTweenPopupAnimation(MoveTweenPopupAnimation.MoveDirection.BottonToCenter, 0.25f),
                    new PopupClosedDelegate(OnPlayExitPopupClosed), message);
            }
        }

        private void OnPopupClosed(string message)
        {
            if (gameController.State == WordMatchingGameController.GameState.Pause)
            {
                gameController.State = WordMatchingGameController.GameState.Play;
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
    }
}