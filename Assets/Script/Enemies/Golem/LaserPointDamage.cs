using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointDamage : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision == null )
            return;
        if (collision.gameObject.CompareTag("Player"))
        {
            IDamagable damagable = collision.GetComponentInChildren<IDamagable>();
            if ( damagable != null)
            {
                damagable.Hit(damage, Vector2.zero);
              
            }
        }

    }


}
