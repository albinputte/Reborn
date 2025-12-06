using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfDeath : WolfState
{
    public WolfDeath(EnemyStateMachine<WolfController> stateMachine, WolfController controller, string animName) : base(stateMachine, controller, animName)
    {
    }

  
}
