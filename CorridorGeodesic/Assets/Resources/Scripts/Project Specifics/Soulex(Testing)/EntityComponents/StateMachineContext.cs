using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class StateMachineContext : MonoBehaviour
{
    protected delegate void StateSwitched();
    protected event StateSwitched OnStateSwitched;
    [HideInInspector] public StateMachine stateMachine;
    public Enum previousState;
    public Enum nextState;
    public void SwitchState(Enum state)
    {
        nextState = state;
        previousState = stateMachine.stateDictionary.FirstOrDefault(k=>k.Value == stateMachine.currentState).Key;
        stateMachine.SwitchState(stateMachine.stateDictionary[state]);

        OnStateSwitched?.Invoke();
    }
}