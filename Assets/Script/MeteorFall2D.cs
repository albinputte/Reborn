using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorFall2D : MonoBehaviour
{
    [Header("References")]
    public Transform shadow;

    [Header("Fall Settings")]
    public float fallSpeed = 10f;
    public float startHeight = 15f;

    [Header("Shadow Settings")]
    public float minShadowScale = 0.3f;
    public float maxShadowScale = 1f;

    private Vector3 targetPosition;
    private Vector3 startPosition;

    void Start()
    {
        // Store target (shadow position)
        targetPosition = shadow.position;

        // Start meteor above target
        startPosition = targetPosition + Vector3.up * startHeight;
        transform.position = startPosition;
    }

    void Update()
    {
        // Move meteor downward
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            fallSpeed * Time.deltaTime
        );

        // Calculate progress (0 = start, 1 = landed)
        float distanceRemaining = Vector3.Distance(transform.position, targetPosition);
        float totalDistance = Vector3.Distance(startPosition, targetPosition);
        float progress = 1f - (distanceRemaining / totalDistance);

        // Scale shadow as meteor approaches
        float shadowScale = Mathf.Lerp(minShadowScale, maxShadowScale, progress);
        shadow.localScale = new Vector3(shadowScale, shadowScale, 1f);

        // Impact
        if (distanceRemaining <= 0.05f)
        {
            Impact();
        }
    }

    void Impact()
    {
        // Example impact logic
        Debug.Log("Meteor Impact!");

        // Destroy meteor (optional)
        Destroy(gameObject);

        // Optional: destroy shadow
        Destroy(shadow.gameObject, 0.1f);
    }
}
