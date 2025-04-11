using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDmgCol : MonoBehaviour
{
    private PlayerWeaponAgent playerWeaponAgent;
    public Rigidbody2D rb;
    private void Start()
    {
        playerWeaponAgent = GetComponentInParent<PlayerWeaponAgent>();
        rb = GetComponentInParent<Rigidbody2D>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
     
        IDamagable damagable = other.GetComponent<IDamagable>();

        if (damagable != null)
        {
            Vector2 direction = (other.transform.position - transform.position).normalized;
            Vector2 knockback = direction * playerWeaponAgent.CurrentWeapon.KnockbackForce;
            damagable.Hit(playerWeaponAgent.CurrentWeapon.Damage, knockback);
            //rb.AddForce((direction * 3) * -1, ForceMode2D.Impulse);
            CameraShake.instance.ShakeCamera(2f, 0.3f);
            playerWeaponAgent.TriggerHitStop();
            playerWeaponAgent.DisableCollider();
           

        }
    }

}
