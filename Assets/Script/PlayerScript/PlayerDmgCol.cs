using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDmgCol : MonoBehaviour
{
    private PlayerWeaponAgent playerWeaponAgent;
    private Health health;
    public Rigidbody2D rb;
    public bool isProjectile;
    private void Start()
    {
        playerWeaponAgent = FindAnyObjectByType<PlayerWeaponAgent>();
        health = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponentInChildren<Health>();
        //rb = GetComponentInParent<Rigidbody2D>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
     
        IDamagable damagable = other.GetComponent<IDamagable>();

        if (damagable != null)
        {
            Vector2 direction = (other.transform.position - transform.position).normalized;
            Vector2 knockback = direction * playerWeaponAgent.CurrentWeapon.KnockbackForce;
            damagable.Hit(playerWeaponAgent.CurrentWeapon.Damage + (int)StatSystem.instance.GetStat(StatsType.Damage), knockback);
            //rb.AddForce((direction * 3) * -1, ForceMode2D.Impulse);
            CameraShake.instance.ShakeCamera(2f, 0.3f);
            if (StatSystem.instance.GetStat(StatsType.Lifesteal) > 0)
                health.heal((int)StatSystem.instance.GetStat(StatsType.Lifesteal));
            if(isProjectile)
                Destroy(gameObject);
            
            playerWeaponAgent.TriggerHitStop();
            playerWeaponAgent.DisableCollider();
          



        }
    }

}
