using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfReturnToTerritory : WolfState
{
    public WolfReturnToTerritory(EnemyStateMachine<WolfController> stateMachine, WolfController controller, string animName) : base(stateMachine, controller, animName)
    {
    }
}
