using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseController : MonoBehaviour
{
    public EnemyStateMachine<EnemyBaseController> stateMachine;

    public virtual void Move(Vector2 direction) { }
    public virtual bool CanSeePlayer() => false;
}
