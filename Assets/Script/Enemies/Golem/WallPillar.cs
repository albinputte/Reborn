using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPillar : GolemPillar
{
    public GameObject wallPrefab;
    public Transform leftSpawn;
    public Transform rightSpawn;

    public override void ExecuteAbility()
    {
        Instantiate(wallPrefab, leftSpawn.position, Quaternion.identity);
        Instantiate(wallPrefab, rightSpawn.position, Quaternion.identity);
    }
}

