using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveEventManger : MonoBehaviour
{
    public static WaveEventManger instance;

    public UnityEvent onWaveStart = new UnityEvent();
    public UnityEvent onWaveComplete = new UnityEvent();

    [SerializeField]
    private EnemySpawner enemySpawner;

    [SerializeField]
    private WaveData[] waveConfigs;
    private AudioSource source;
    private AudioClip clip;

    private int currentWave = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        source = GetComponent<AudioSource>();
        clip = GetComponent<AudioClip>();
        StartNextWave();
    }

    void StartNextWave()
    {
        if (currentWave < waveConfigs.Length)
        {
            source.PlayOneShot(clip);
            onWaveStart.Invoke();
            StartCoroutine(SpawnWaveWithInterval(waveConfigs[currentWave]));
        }
        else
        {
            // All waves completed
            Debug.Log("All waves completed!");
        }
    }

    IEnumerator SpawnWaveWithInterval(WaveData waveData)
    {
        List<int> enemyTypes = new List<int>();

        // Add enemy types based on wave data configuration
        for (int i = 0; i < waveData.enemy1Count; i++)
            enemyTypes.Add(0);

        for (int i = 0; i < waveData.enemy2Count; i++)
            enemyTypes.Add(1);

        for (int i = 0; i < waveData.enemy3Count; i++)
            enemyTypes.Add(2);

        for (int i = 0; i < waveData.enemy4Count; i++)
            enemyTypes.Add(3);

        // Iterate through the list in a random order
        while (enemyTypes.Count > 0)
        {
            int randomIndex = Random.Range(0, enemyTypes.Count);
            int enemyType = enemyTypes[randomIndex];
            enemyTypes.RemoveAt(randomIndex);

            enemySpawner.SpawnEnemy(enemyType);
            yield return new WaitForSeconds(waveData.SpawnInterval);
        }
        Debug.Log("doen spawning");
        // Wait for all enemies to be destroyed before moving to the next wave
        yield return new WaitForSeconds(5F);
        Debug.Log("After WaitUntil");

        onWaveComplete.Invoke();
        currentWave++;
        StartNextWave();
    }
}

