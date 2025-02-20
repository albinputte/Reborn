using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] ElfPrefab;

    [SerializeField]
    public float ElfSpawnInterval;
    [SerializeField]
    private float[] spawnPointsX = new float[4];
    [SerializeField]
    private float[] spawnPointsY = new float[4];

    private List<GameObject> spawnedEnemies = new List<GameObject>();


    public void SpawnEnemy(int enemyTypeIndex)
    {
        int SpawnPointDecider = Random.Range(0, 6);
        GameObject newEnemy = Instantiate(ElfPrefab[enemyTypeIndex], new Vector3(spawnPointsX[SpawnPointDecider], spawnPointsY[SpawnPointDecider], 0), Quaternion.identity);
        spawnedEnemies.Add(newEnemy);
        /*
        // Subscribe to the Die event to remove the enemy from the list when it dies
        Health enemyHealth = newEnemy.GetComponent<Health>();
        if (enemyHealth != null)
        {
            enemyHealth.Die.AddListener(() => RemoveEnemy(newEnemy));
        }'
        */
    }

    public void RemoveEnemy(GameObject enemy)
    {
        spawnedEnemies.Remove(enemy);
        Debug.Log("Enemy removed. Total enemies: " + spawnedEnemies.Count);
    }


    public bool AllEnemiesDestroyed()
    {
        // Check if all enemies are destroyed
        Debug.Log(spawnedEnemies.Count);
        return spawnedEnemies.Count == 0;
    }


}

