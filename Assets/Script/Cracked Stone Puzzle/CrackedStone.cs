using SmallHedge.SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackedStone : MonoBehaviour
{
    private Animator animator;
    private bool hasBeenHit = false;
    [SerializeField] private GameObject rewardChest;
    private void Awake()
    {
        // Get the Animator component attached to this GameObject
        animator = GetComponent<Animator>();
    
    }
  public void EnableChest()
    {
        // Enable the reward chest object
        if (rewardChest != null)
        {
            rewardChest.SetActive(true);
        }
    }
    public void ChangeChestSpriteOrder()
    {
        // Change the sorting order of the reward chest sprite
        if (rewardChest != null)
        {
            SpriteRenderer chestSpriteRenderer = rewardChest.GetComponent<SpriteRenderer>();
            if (chestSpriteRenderer != null)
            {
                chestSpriteRenderer.sortingOrder = 0; // Set to a higher sorting order
            }
        }
    }
    public void DestroyCrackedStone()
    {
        SoundManager.PlaySound(SoundType.Stone_Puzzle_Destroy);
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasBeenHit) return;
        // Check if the object that entered the trigger is tagged as "MagicSphere"
        if (collision.CompareTag("MagicSphere"))
        {
            
            animator.SetTrigger("Hit");
            hasBeenHit = true;
        }
    }
    public void HitBySphere()
    {
        if (hasBeenHit) return;
        animator.SetTrigger("Hit");
        hasBeenHit = true;
    }
}
