using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDmgCol : MonoBehaviour
{
    private PlayerWeaponAgent playerWeaponAgent;
    private Health health;
    public Rigidbody2D rb;
    public bool isProjectile;
    private float effectCooldown = 0.5f; // Cooldown in seconds
    private float lastEffectTime = -Mathf.Infinity;
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

            damagable.Hit(
                playerWeaponAgent.CurrentWeapon.Damage + (int)StatSystem.instance.GetStat(StatsType.Damage),
                knockback
            );

            // Apply CameraShake and HitStop only if cooldown has passed
            if (Time.time - lastEffectTime >= effectCooldown)
            {
                CameraShake.instance.ShakeCamera(2f, 0.3f);
                playerWeaponAgent.TriggerHitStop();
                lastEffectTime = Time.time; // Reset cooldown timer
            }
            Debug.Log(StatSystem.instance.GetStat(StatsType.Lifesteal));
            if (StatSystem.instance.GetStat(StatsType.Lifesteal) > 0 && other.gameObject.CompareTag("Enemy"))
            {

                health.heal((int)StatSystem.instance.GetStat(StatsType.Lifesteal), false);
            }
            

            if (isProjectile)
                Destroy(gameObject);

            playerWeaponAgent.DisableCollider();
        }
    }

}
