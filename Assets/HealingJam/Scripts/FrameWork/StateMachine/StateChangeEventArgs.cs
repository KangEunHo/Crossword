using UnityEngine;
using System;
using System.Collections;

namespace HealingJam.StateMachine
{
    public class StateChangeEventArgs<T> : EventArgs where T : IState
    {
        public T prevState;
        public T nextState;

        public StateChangeEventArgs(T prevState, T nextState)
        {
            this.prevState = prevState; this.nextState = nextState;
        }
    }

    public class StateEventArgs<T> : EventArgs where T : IState
    {
        public T state;

        public StateEventArgs(T state)
        {
            this.state = state;
        }
    }
}