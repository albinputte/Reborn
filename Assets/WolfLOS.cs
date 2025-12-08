using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfLOS : MonoBehaviour
{

    public WolfController wolf;


    private Collider2D losCollider;
    public ContactFilter2D filter;
    private Collider2D[] results = new Collider2D[5];

    void Awake()
    {
        losCollider = GetComponent<Collider2D>();

      
    }

    void Update()
    {
        int hitCount = losCollider.OverlapCollider(filter, results);

        bool playerInside = false;

        for (int i = 0; i < hitCount; i++)
        {
            if (results[i] != null && results[i].CompareTag("Player"))
            {
                playerInside = true;
                break;
            }
        }

        wolf.iSePlayer = playerInside;
    }
}

