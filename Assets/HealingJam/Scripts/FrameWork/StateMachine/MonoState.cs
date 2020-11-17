//using UnityEngine;
//using System.Collections;

//namespace HealingJam.StateMachine
//{
//    public abstract class MonoState : MonoBehaviour, IState
//    {
//        protected IStateMachine<IState> stateMachine = null;

//        public virtual void Init(object stateMachine)
//        {
//            this.stateMachine = stateMachine;
//        }

//        public abstract string Name();

//        public abstract void Enter(params object[] args);

//        public abstract void Exit(params object[] args);
//    }
//}