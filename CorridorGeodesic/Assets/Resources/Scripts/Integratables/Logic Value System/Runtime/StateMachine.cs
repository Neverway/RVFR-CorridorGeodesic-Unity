using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neverway.Framework.Utility.StateMachine
{
    public abstract class StateMachine<TStateMachine> where TStateMachine : StateMachine<TStateMachine>
    {
        public State<TStateMachine> lastStartedState { get; private set; }
        public State<TStateMachine> currentState { get; private set; }
        public StateMachine(State<TStateMachine> startingState)
        {
            startingState.StateMachine = this as TStateMachine;
            currentState = startingState;
            currentState.OnEnter(null);
        }


        public virtual void DoUpdate() => currentState.OnUpdate();

        public void ChangeState(State<TStateMachine> newState)
        {
            if (newState == null)
                throw new System.NullReferenceException("Cannot change state to null state");

            lastStartedState = newState;
            State<TStateMachine> oldState = currentState;

            newState.StateMachine = this as TStateMachine;
            oldState.OnLeave(newState);

            //If ChangeState was called during OnLeave, let that call take over instead
            if (lastStartedState != newState)
                return;

            currentState = newState;
            newState.OnEnter(oldState);
        }

        public bool IsOnState<T>() => currentState is T;
    }
    public abstract class State<TStateMachine> where TStateMachine : StateMachine<TStateMachine>
    {
        public TStateMachine StateMachine { get; internal set; }

        public virtual void OnEnter(State<TStateMachine> lastState) { }
        public virtual void OnLeave(State<TStateMachine> newState) { }
        public abstract void OnUpdate();
    }

}