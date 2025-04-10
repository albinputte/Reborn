using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDmgCol : MonoBehaviour
{
    private PlayerWeaponAgent playerWeaponAgent;
    private void Start()
    {
        playerWeaponAgent = GetComponentInParent<PlayerWeaponAgent>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
     
        IDamagable damagable = other.GetComponent<IDamagable>();

        if (damagable != null)
        {
            Vector2 direction = (other.transform.position - transform.position).normalized;
            Vector2 knockback = direction * playerWeaponAgent.currentWeapon.KnockbackForce;
            damagable.Hit(playerWeaponAgent.currentWeapon.Damage, knockback);
            CameraShake.instance.ShakeCamera(2f, 0.3f);
            playerWeaponAgent.HitStop();
            playerWeaponAgent.DisableCollider();
           

        }
    }

}
