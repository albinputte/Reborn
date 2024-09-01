using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    private PlayerState CurrentState;
   

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
