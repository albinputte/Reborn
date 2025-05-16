using SmallHedge.SoundManager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutGrass : MonoBehaviour
{
    [SerializeField] private GameObject grassEffectPrefab;
    private Collider2D grassCollider;
    private Animator grassAnimator;
    public event Action onTakeDamage;
    private bool isCut = false;
    private void Awake()
    {
        grassCollider = GetComponent<Collider2D>();
        grassAnimator = GetComponent<Animator>();
    }
    public void CutGrassEffect()
    {
        if (isCut) return; // Prevent multiple cuts
        // Instantiate the grass effect prefab at the position of the grass
        Vector3 spawnPosition = transform.position + new Vector3(0f, -2f, 0f);
        GameObject grassEffect = Instantiate(grassEffectPrefab, spawnPosition, Quaternion.identity);

       // SoundManager.PlaySound(SoundType.CutGrass);
        grassAnimator.SetTrigger("cut");
        // Disable the collider to prevent further interactions
        if (grassCollider != null)
        {
            grassCollider.enabled = false;
        }

        // Optionally destroy the effect after a delay
        Destroy(grassEffect, 1f);
        isCut = true; // Mark as cut
    }
    public void ontakeDamage()
    {
        onTakeDamage?.Invoke();
    }
}
