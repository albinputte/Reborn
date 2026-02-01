using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class GolemMage : MonoBehaviour
{
    [Header("References")]
    public GameObject projectilePrefab;

    [Header("Timing")]
    public float minPause = 0.6f;
    public float maxPause = 1.2f;

    [Header("Shooting")]
    public int burstMin = 3;
    public int burstMax = 6;
    public int enragedBurstmin = 4;
    public int enragedBurstmax = 8;
    public bool isEnraged;
    [Header("Aim Bias")]
    [Range(0f, 1f)]
    public float playerAimBias = 0.75f;   // 1 = perfect aim, 0 = random
    public float maxAimAngleOffset = 25f; // degrees

    Transform player;
    bool isAttacking;

    Vector2 lastAimDir;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(AttackLoop());
        isEnraged = false;
        lastAimDir = Vector2.right;
    }



    // ============================
    // MAIN LOOP
    // ============================
    IEnumerator AttackLoop()
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {
            if (!isAttacking)
            {
                isAttacking = true;
                ChooseShootPattern();
            }
            yield return null;
        }
    }

    public void Enraged()
    {
        isEnraged = true;
    }

    public void ExitEnraged()
    {
        isEnraged = false;
    }

    void ChooseShootPattern()
    {
        if (!isEnraged)
            StartCoroutine(AimedBurst());
        else
            StartCoroutine(EnragedAimedBurst());
     
    }

    void EndPattern()
    {
        isAttacking = false;
    }


    Vector2 GetBiasedDirection()
    {
        Vector2 playerDir = (player.position - transform.position).normalized;

        float randomAngle = Random.Range(-maxAimAngleOffset, maxAimAngleOffset);
        Vector2 randomDir = Quaternion.Euler(0, 0, randomAngle) * playerDir;

        Vector2 biasedDir = Vector2.Lerp(randomDir, playerDir, playerAimBias);
        return biasedDir.normalized;
    }

    Vector2 GetDriftingDirection()
    {
        Vector2 targetDir = GetBiasedDirection();
        lastAimDir = Vector2.Lerp(lastAimDir, targetDir, 0.6f);
        return lastAimDir.normalized;
    }


    IEnumerator AimedBurst()
    {
        int shots = Random.Range(burstMin, burstMax);

        for (int i = 0; i < shots; i++)
        {
            Shoot(GetDriftingDirection());
            yield return new WaitForSeconds(Random.Range(0.15f, 0.25f));
        }

        yield return new WaitForSeconds(Random.Range(minPause, maxPause));
        EndPattern();
    }

    IEnumerator EnragedAimedBurst()
    {
        int shots = Random.Range(enragedBurstmin, enragedBurstmax);

        for (int i = 0; i < shots; i++)
        {
            Shoot(GetDriftingDirection());
            yield return new WaitForSeconds(Random.Range(0.15f, 0.25f));
        }

        yield return new WaitForSeconds(Random.Range(minPause, maxPause));
        EndPattern();
    }

    void Shoot(Vector2 dir)
    {
        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        MageProjectile mp = proj.GetComponent<MageProjectile>();
        mp.Init(dir);
       
    }

}