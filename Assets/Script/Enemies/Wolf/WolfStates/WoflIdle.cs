using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoflIdle : WolfState
{
    public WoflIdle(EnemyStateMachine<WolfController> stateMachine, WolfController controller, string animName) : base(stateMachine, controller, animName)
    {
    }
}
