using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAttackCollider : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            IDamagable damagable = collision.GetComponentInChildren<IDamagable>();

            if (collision.gameObject.CompareTag("Player"))
            {
                Vector2 dir = (collision.transform.position - gameObject.transform.position).normalized;
                Debug.Log(damagable);
                if (damagable != null)
                {


                    damagable.Hit(10, dir * 10);
                    //health.TakeKnockBack(rb, (dir * 10) * -1);
                    //health.SpawnParticlesFromTarget(HitPrefab, (dir * 10));
                    CameraShake.instance.ShakeCamera(2f, 0.3f);

                }

            }
        }

    }
}
