﻿using UnityEngine;
using HealingJam.StateMachine;

namespace HealingJam.GameScreens
{
    public class GameScreen : MonoBehaviour, IState
    {
        public enum ScreenID
        {
            None, Title, StageSelect, Statistics, Clear, Play, WordMatchingPlay, CommonSenseResult
        }

        #region Inspector Variables

        [SerializeField] private ScreenID id = ScreenID.None;

        #endregion

        #region Member Variables

        protected MultiStateMachine<GameScreen> stateMachine = null;

        public ScreenID ID => id;
        #endregion

        public virtual void Init(object stateMachine)
        {
            this.stateMachine = stateMachine as MultiStateMachine<GameScreen>;
        }

        public string Name()
        {
            return id.ToString();
        }

        public virtual void Enter(params object[] args)
        {
            gameObject.SetActive(true);
        }

        public virtual void Exit(params object[] args)
        {
            gameObject.SetActive(false);
        }

        public virtual void Escape()
        {
            
        }
    }
}
