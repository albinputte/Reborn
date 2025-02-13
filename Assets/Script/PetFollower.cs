using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetFollower : MonoBehaviour
{
    public Transform player;    
    public float followDistance = 2f;
    public float moveSpeed = 5f;
    public float smoothTime = 0.2f;  // Smooth movement
    public bool PlayerInRange;
    public LayerMask LayerToHit;

    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        if (!CanMove())
        { 
        Vector3 targetPosition = player.position - (player.forward * followDistance);
      
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime, moveSpeed);
        }
    }

   

    public bool CanMove()
    {
       if(Physics2D.CircleCast(transform.position, 2f, new Vector2(0f, 0f), Mathf.Infinity, LayerToHit))
        {
            return true;
        }
       else { return false; }
    }
}
