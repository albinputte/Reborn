using System.Collections;
using UnityEngine;

public abstract class GolemPillar : MonoBehaviour
{
    public GolemAbilityType abilityType;
    public float chargeTime = 3f;

    protected bool isCharging;
    protected bool interrupted;

    protected GolemBossController boss;
    protected Health health;

    protected virtual void Awake()
    {
        boss = GetComponentInParent<GolemBossController>();
        health = GetComponent<Health>();

        health.OnTakeDamage.AddListener(Interrupt);
    }

    public void StartCharge()
    {
        if (isCharging) return;

        interrupted = false;
        StartCoroutine(ChargeRoutine());
    }

    IEnumerator ChargeRoutine()
    {
        isCharging = true;

        // TODO: Charge VFX
        yield return new WaitForSeconds(chargeTime);

        if (!interrupted)
        {
            boss.AddAbilityPoint(abilityType);
        }

        isCharging = false;
    }

    public void Interrupt()
    {
        if (!isCharging) return;

        interrupted = true;
        StopAllCoroutines();

        // TODO: Interrupt VFX
    }

    public abstract void ExecuteAbility();
}