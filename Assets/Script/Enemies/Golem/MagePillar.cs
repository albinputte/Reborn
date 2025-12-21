using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MagePillar : GolemPillar
{
    public GameObject magePrefab;
    public Transform[] spawnPoints;

    public override void ExecuteAbility()
    {
        foreach (var point in spawnPoints)
        {
            Instantiate(magePrefab, point.position, Quaternion.identity);
        }
    }
}
