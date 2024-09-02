using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerState CurrentState { get; private set; }
   

    public void InisiateState(PlayerState NewState)
    {
        CurrentState = NewState;
        CurrentState.Enter();

    }

    public void SwitchState(PlayerState NewState)
    {
        CurrentState.Exit();
        CurrentState = NewState;
        CurrentState.Enter();
    }
}
