using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfLungeAttack : WolfState
{
    public WolfLungeAttack(EnemyStateMachine<WolfController> stateMachine, WolfController controller, string animName) : base(stateMachine, controller, animName)
    {
    }

  
}
