using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingMeteor : MonoBehaviour
{
    public float fallSpeed = 15f;
    public int damage = 20;
    public float damageRadius = 1.5f;

    private Vector3 targetPosition;
    private GameObject shadow;

    public void Init(Vector3 targetPos, GameObject shadowObj)
    {
        targetPosition = targetPos;
        shadow = shadowObj;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            fallSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPosition) <= 0.05f)
        {
            Impact();
        }
    }

    void Impact()
    {
        // Damage in radius
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            targetPosition,
            damageRadius
        );

        foreach (var hit in hits)
        {
            IDamagable damagable = hit.GetComponent<IDamagable>();
            if (damagable != null) {
                //damagable.Hit(damage, Vector2.zero);
            }
            else
            {
                damagable = hit.GetComponentInChildren<IDamagable>();
                if (damagable != null) {
                    damagable.Hit(damage, Vector2.zero);
                }
            }

        }

        if (shadow != null)
            Destroy(shadow);

        Destroy(gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
#endif
}
