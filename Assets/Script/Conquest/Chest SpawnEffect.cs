using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSpawnEffect : MonoBehaviour
{
    [SerializeField] private GameObject spawnParticles;
    [SerializeField] private GameObject shadowObj;
    public void SpawnParticles()
    {
        //instantiate the spawn particles at the position of the chest
        GameObject particles = Instantiate(spawnParticles,  shadowObj.transform.position - new Vector3(0,0.7f,0), Quaternion.identity);
        CameraShake.instance.ShakeCamera(2f, 0.3f);
    }
    public void EnableShadow()
    {
        shadowObj.SetActive(true);
    }
}
