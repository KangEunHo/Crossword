using UnityEngine;
using HealingJam.GameScreens;
using HealingJam.Popups;
using System.Collections.Generic;

namespace HealingJam.Crossword.UI
{
    public class WordMatchingPlayScreen : FadeAndScaleTweenScreen
    {
        [SerializeField] private WordMatchingGameController gameController = null;

        public override void Enter(params object[] args)
        {
            base.Enter(args);

            List<WordDataForGame> answers = new List<WordDataForGame>();
            int maxStage = CrosswordMapManager.Instance.MaxStage();
            List<int> useMapIndex = new List<int>(maxStage);
            for (int i = 0; i < maxStage; ++i)
            {
                useMapIndex.Add(i);
            }

            useMapIndex.Shuffle();

            for (int i = 0; i < maxStage && i < 20; ++i)
            {
                WordDataForGame randomWord = CrosswordMapManager.Instance.GetCrosswordMap(useMapIndex[i]).wordDatas.RandomValue();
                WordDataForGame wordDataForGame = new WordDataForGame()
                {
                    direction = WordDataForGame.Direction.Horizontal,
                    info = randomWord.info,
                    word = randomWord.word,
                    wordType = randomWord.wordType,
                    x = 0,
                    y = 0
                };
                answers.Add(wordDataForGame);
            }
            gameController.SetUp(answers);
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

                PopupMgr.Instance.EnterWithAnimation(Popup.PopupID.PlayExit, new MoveTweenPopupAnimation(MoveTweenPopupAnimation.MoveDirection.BottonToCenter, 0.25f),
                    new PopupClosedDelegate(OnPlayExitPopupClosed), "나가시겠습니까?\n 나가게 되면 보상을 받을 수 없습니다.");
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