using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MagePillar : GolemPillar
{
    public GameObject magePrefab;
    public Transform[] spawnPoints;
    private GameObject SpawnedMage;


    public override void ExecuteAbility()
    {
        if (SpawnedMage == null)
        {
            foreach (var point in spawnPoints)
            {
                SpawnedMage = Instantiate(magePrefab, point.position, Quaternion.identity);
            }
        }
        else
        {
            GolemMage mage = SpawnedMage.GetComponent<GolemMage>();
            if(!mage.isEnraged)
                mage.Enraged();    
            else
                mage.ExitEnraged();
        }
    }
}
