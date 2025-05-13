using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponAnimationHandler : MonoBehaviour
{
    public event Action OnAnimationComplete;
    //add this in last frame of weapon animation
    public void AnimationFinishedTrigger()
    {
        OnAnimationComplete?.Invoke();
        if (TutorialManger.instance.TutorialIsActive && TutorialManger.instance.currentState == TutorialState.OpenInventory)
            TutorialManger.instance.OnSwordEquipped();
    }
}
