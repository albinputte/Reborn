using SmallHedge.SoundManager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutGrass : MonoBehaviour
{
    private Collider2D grassCollider;
    private Animator grassAnimator;
    private void Awake()
    {
        grassCollider = GetComponent<Collider2D>();
        grassAnimator = GetComponent<Animator>();
    }
    public void CutGrassEffect()
    {
       // SoundManager.PlaySound(SoundType.CutGrass);
        grassAnimator.SetTrigger("cut");
        // Disable the collider to prevent further interactions
        if (grassCollider != null)
        {
            grassCollider.enabled = false;
        }
    }
  
}
