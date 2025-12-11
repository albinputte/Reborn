using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrootAttackCollider : MonoBehaviour
{
    public GrootController GrootController;

    private void Start()
    {
       GrootController = GetComponentInParent<GrootController>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            IDamagable damagable = collision.GetComponentInChildren<IDamagable>();

            if (collision.gameObject.CompareTag("Player"))
            {
                Vector2 dir = (collision.transform.position - gameObject.transform.position).normalized;

                if (damagable != null && !GrootController.BurnMode)
                {


                    damagable.Hit(5, dir * 10);
                    //health.TakeKnockBack(rb, (dir * 10) * -1);
                    //health.SpawnParticlesFromTarget(HitPrefab, (dir * 10));
                    CameraShake.instance.ShakeCamera(2f, 0.3f);

                }
                else if(damagable != null && GrootController.BurnMode) 
                {
                    damagable.Hit(10, dir * 10);
                    damagable.ApplyDamageOverTime(1, 0.5f, 10);
                    //health.TakeKnockBack(rb, (dir * 10) * -1);
                    //health.SpawnParticlesFromTarget(HitPrefab, (dir * 10));
                    CameraShake.instance.ShakeCamera(2f, 0.3f);
                }

            }
        }

    }
}
