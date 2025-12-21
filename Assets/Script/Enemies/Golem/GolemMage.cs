using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GolemMage : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float shootInterval = 1.5f;

    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating(nameof(Shoot), 1f, shootInterval);
    }

    void Shoot()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        proj.GetComponent<MageProjectile>().Init(dir);
    }
}