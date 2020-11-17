using UnityEngine;
using System;
using System.Collections.Generic;

namespace HealingJam.StateMachine
{
    public abstract class StateMachine<T> : IStateMachine<T> where T : IState
    {
        public event EventHandler<StateChangeEventArgs<T>> OnStateChanged;
        public event EventHandler<StateEventArgs<T>> OnStateEntered;
        public event EventHandler<StateEventArgs<T>> OnStateExited;
        public event EventHandler<StateEventArgs<T>> OnStateRegisted;
        public event EventHandler<StateEventArgs<T>> OnStateUnRegisted;

        protected Dictionary<string, T> registeredStates = new Dictionary<string, T>();
        protected T currentState;

        public virtual T CurrentState()
        {
            return currentState;
        }

        public abstract void Enter(string name, params object[] args);

        public abstract void Exit(string name, params object[] args);

        public abstract void ChangeState(string name, params object[] enterArgs);

        public abstract void RegisterState(T state);

        public abstract void UnRegisterState(T state);

        public abstract bool ContainsStateInActiveStates(string name);

        public T GetStateInRegisteredStates(string name)
        {
            return registeredStates[name];
        }

        protected void InvokeStateChanged(StateChangeEventArgs<T> e) { OnStateChanged?.Invoke(this, e); }
        protected void InvokeStateEntered(StateEventArgs<T> e) { OnStateEntered?.Invoke(this, e); }
        protected void InvokeStateExited(StateEventArgs<T> e) { OnStateExited?.Invoke(this, e); }
        protected void InvokeStateRegisted(StateEventArgs<T> e) { OnStateRegisted?.Invoke(this, e); }
        protected void InvokeStateUnRegisted(StateEventArgs<T> e) { OnStateUnRegisted?.Invoke(this, e); }

    }
}