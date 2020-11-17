using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace HealingJam.StateMachine
{
    public class MultiStateMachine<T> : StateMachine<T> where T : IState
    {
        protected LinkedList<T> activeStates = new LinkedList<T>();

        public override T CurrentState()
        {
            if (activeStates.Last == null)
                return default;

            return activeStates.Last.Value;
        }

        public override void ChangeState(string name, params object[] enterArgs)
        {
            T exitState = default;

            if (activeStates.Count > 0)
            {
                exitState = CurrentState();
                Exit(exitState.Name());
            }

            Enter(name, enterArgs);

            InvokeStateChanged(new StateChangeEventArgs<T>(exitState, CurrentState()));
        }

        public override void Enter(string name, params object[] args)
        {
            T state = GetStateInRegisteredStates(name);

            if (activeStates.Contains(state))
            {
                EditorDebug.LogWarning("already actived state  " + name);
                return;
            }

            activeStates.AddLast(state);
            state.Enter(args);

            InvokeStateEntered(new StateEventArgs<T>(state));
        }

        public override void Exit(string name, params object[] args)
        {
            T state = GetStateInRegisteredStates(name);

            if (activeStates.Contains(state) == false)
            {
                EditorDebug.LogWarning("not actived state  " + name);
                return;
            }

            activeStates.Remove(state);
            state.Exit(args);

            InvokeStateExited(new StateEventArgs<T>(state));
        }

        public override void RegisterState(T state)
        {
            state.Init(this);

            registeredStates.Add(state.Name(), state);

            InvokeStateRegisted(new StateEventArgs<T>(state));
        }

        public override void UnRegisterState(T state)
        {
            registeredStates.Remove(state.Name());

            InvokeStateUnRegisted(new StateEventArgs<T>(state));

        }

        public override bool ContainsStateInActiveStates(string name)
        {
            T state = GetStateInRegisteredStates(name);
            return activeStates.Contains(state);
        }
    }
}