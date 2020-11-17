using UnityEngine;
using System;
using HealingJam.StateMachine;
using ScreenID = HealingJam.GameScreens.GameScreen.ScreenID; 

namespace HealingJam.GameScreens
{
    public class ScreenMgr : MonoSingleton<ScreenMgr>
    {

        #region Inspector Variables

        [SerializeField] private GameScreen[] screens = null;
        [SerializeField] private Transform screenHolder = null;

        #endregion

        #region Properties 

        public MultiStateMachine<GameScreen> StateMachine { get; private set; } = null;

        #endregion

        #region Initialize Methods

        public override void Init()
        {
            StateMachine = new MultiStateMachine<GameScreen>();

            foreach(var screen in screens)
            {
                StateMachine.RegisterState(screen);
            }

            StateMachine.OnStateEntered += OnScreenEntered;
            StateMachine.OnStateExited += OnScreenExited;
        }

        public void ChangeState(ScreenID screenID, params object[] enterArgs)
        {
            StateMachine.ChangeState(screenID.ToString(), enterArgs);
        }

        public void ChangeState(string name, params object[] enterArgs)
        {
            StateMachine.ChangeState(name, enterArgs);
        }

        public void Enter(ScreenID screenID, params object[] args)
        {
            Enter(screenID.ToString(), args);
        }

        public void Enter(string name, params object[] args)
        {
            StateMachine.Enter(name, args);
        }

        public void CurrentStateExit(params object[] args)
        {
            GameScreen currentScreen = GetCurrentScreen();
            if (currentScreen == null)
            {
                EditorDebug.LogWarning("Current state is null");
                return;
            }
            Exit(currentScreen.ID, args);
        }

        public void Exit(ScreenID screenID, params object[] args)
        {
            Exit(screenID.ToString(), args);
        }

        public void Exit(string name, params object[] args)
        {
            StateMachine.Exit(name, args);
        }

        public void RegisterState(GameScreen screen)
        {
            screen.gameObject.SetActive(false);
            screen.transform.SetParent(screenHolder);
            screen.transform.SetAsFirstSibling();

            StateMachine.RegisterState(screen);
        }

        public void UnRegisterState(GameScreen screen)
        {
            StateMachine.UnRegisterState(screen);
            Destroy(screen.gameObject);
        }

        public GameScreen GetCurrentScreen()
        {
            return StateMachine.CurrentState();
        }

        private void OnScreenEntered(object sender, StateEventArgs<GameScreen> e)
        {
        }

        private void OnScreenExited(object sender, StateEventArgs<GameScreen> e)
        {

        }
        #endregion

    }
}