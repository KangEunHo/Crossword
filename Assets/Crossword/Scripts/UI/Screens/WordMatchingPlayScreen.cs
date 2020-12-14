using UnityEngine;
using UnityEngine.UI;
using HealingJam.GameScreens;
using HealingJam.Popups;
using System.Collections.Generic;

namespace HealingJam.Crossword.UI
{
    public class WordMatchingPlayScreen : FadeAndScaleTweenScreen
    {
        private const string ABILITY_FIST_VISIT_KEY = "abilityFisrtVisit";
        private const string BADGE_TEST_FIST_VISIT_KEY = "badgeTestFisrtVisit";
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
                string message = "10가지 상식 분야 중\n 랜덤으로 20문제가 출제됩니다.\n테스트를 통해 부족한 분야를 확인해보세요.\n(제한 시간: 5분)";
                infoText.text = message;
                if (PlayerPrefsDatas.GetBoolData(ABILITY_FIST_VISIT_KEY, 1))
                {
                    PopupMgr.Instance.EnterWithAnimation(Popup.PopupID.Message, new MoveTweenPopupAnimation(MoveTweenPopupAnimation.MoveDirection.BottonToCenter, 0.25f),
                                                new PopupClosedDelegate((msg) => { gameController.SetUp(gameMode, CreateAbilityTestAnswers()); }), message);
                    PlayerPrefsDatas.SetBoolData(ABILITY_FIST_VISIT_KEY, false);
                }
                else 
                    gameController.SetUp(gameMode, CreateAbilityTestAnswers());
            }
            else
            {
                string message = string.Format("({0}) 레벨에 대한 테스트를 시작합니다.\n레벨 테스트를 완료하면\n다음 레벨의 문제를 풀 수 있습니다.", (CrosswordMapManager.Instance.ActiveLevelIndex + 1));
                infoText.text = message;
                if (PlayerPrefsDatas.GetBoolData(BADGE_TEST_FIST_VISIT_KEY, 1))
                {
                    PopupMgr.Instance.EnterWithAnimation(Popup.PopupID.Message, new MoveTweenPopupAnimation(MoveTweenPopupAnimation.MoveDirection.BottonToCenter, 0.25f),
                        new PopupClosedDelegate((msg)=> { gameController.SetUp(gameMode, CreateBadgePlayAnswers()); }), message);
                    PlayerPrefsDatas.SetBoolData(BADGE_TEST_FIST_VISIT_KEY, false);
                }
                else
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

        private void OnMessagePopupClosed(string message)
        {

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

            HashSet<int> hashSetOfmapIndex = new HashSet<int>();

            while(true)
            {
                int index = Random.Range(0, maxStage);
                if (hashSetOfmapIndex.Contains(index))
                {
                    continue;
                }
                else
                {
                    hashSetOfmapIndex.Add(index);
                }

                CrosswordMap crosswordMap = CrosswordMapManager.Instance.GetCrosswordMap(index);

                foreach (var wordData in crosswordMap.wordDatas)
                {
                    allAnswers[wordData.wordType].Add(wordData);
                }

                bool complete = true;
                foreach (var v in allAnswers.Values)
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
                    if (gameMode == GameMode.AbilityTest)
                    {
                        ScreenMgr.Instance.ChangeState(ScreenID.Title);
                    }
                    else
                    {
                        ScreenMgr.Instance.ChangeState(ScreenID.StageSelect);
                    }
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