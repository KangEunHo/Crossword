using System;

namespace HealingJam.StateMachine
{
    public interface IStateMachine<T> where T : IState
    {
        event EventHandler<StateChangeEventArgs<T>> OnStateChanged;
        event EventHandler<StateEventArgs<T>> OnStateEntered;
        event EventHandler<StateEventArgs<T>> OnStateExited;
        event EventHandler<StateEventArgs<T>> OnStateRegisted;
        event EventHandler<StateEventArgs<T>> OnStateUnRegisted;

        T CurrentState();
        
        void Enter(string name, params object[] args);
        void Exit(string name, params object[] args);
        void ChangeState(string name, params object[] enterArgs);
        void RegisterState(T state);
        void UnRegisterState(T state);
    }

    public interface IState
    {
        void Init(object stateMachine);
        string Name();
        void Enter(params object[] args);
        void Exit(params object[] args);
    }
}