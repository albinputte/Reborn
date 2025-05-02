using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine<T> : MonoBehaviour where T : EnemyBaseController
{
    public BaseEnemyState<T> CurrentState;

    public void SwitchState(BaseEnemyState<T> newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    public void InstantiateState(BaseEnemyState<T> newState)
    {
        CurrentState = newState;
        CurrentState.Enter();
    }
}

   
