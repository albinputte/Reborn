using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemyState<T> where T : EnemyBaseController
{
    protected readonly EnemyStateMachine<T> stateMachine;
    protected readonly T controller;
    protected readonly string animName;

    protected BaseEnemyState(EnemyStateMachine<T> stateMachine, T controller, string animName)
    {
        this.stateMachine = stateMachine;
        this.controller = controller;
        this.animName = animName;
    }

    public virtual void Enter() { }

    public virtual void Exit() { }

    public virtual void LogicUpdate() { }

    public virtual void PhysicsUpdate() { }
}