using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimtionPlayerTrigger : MonoBehaviour
{
    private PlayerController playerController;
    void Start()
    {
      playerController = GetComponentInChildren<PlayerController>();  
    }

    public void InvokeAnimationEvent() => playerController.OnAnimationTrigger();

    public void InvokeAnimationEventTrigger() => playerController.OnAnimationEventTrigger();

}
