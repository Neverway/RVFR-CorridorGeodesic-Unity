using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public StateMachineContext context;
    public Dictionary<Enum, State> stateDictionary = new Dictionary<Enum, State>();
    public State currentState;
    private bool switchingState;
    public virtual void Awake()
    {
        context.stateMachine = this;
    }
    public void Update()
    {
        if (switchingState || currentState == null)
            return;

        currentState.UpdateState();
        Debug.Log(currentState);
    }
    public void SwitchState(State state)
    {
        if (currentState == state && !state.canEnterSelf || switchingState)
        {
            return;
        }

        switchingState = true;

        currentState?.UpdateState();
        currentState?.ExitState();

        currentState = state;

        currentState.EnterState();

        switchingState = false;
    }
}