using SmallHedge.SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConquestPressureplate : MonoBehaviour
{
    [Header("Visuals")]
    [ SerializeField] private SpriteRenderer ppSprite;
    [SerializeField] private  Sprite openSprite;
   [SerializeField] private Sprite closedSprite;

    [Header("Logic")]
    [SerializeField] private GameObject[] torches; // Assign the 4 torch GameObjects
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] enemySpawnPoints; // Optional: multiple spawn points
    [SerializeField] private GameObject rewardChest;
    [SerializeField] private Transform chestSpawnPoint;

    private int currentStage = 0; // how many torches are lit
    private List<GameObject> activeEnemies = new List<GameObject>();
    private bool isPlateLocked = false;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || isPlateLocked || currentStage >= 4)
            return;

        isPlateLocked = true;
        ppSprite.sprite = closedSprite;
        //PlaydingSound(currentStage);
        StartCoroutine(SpawnEnemiesWithDelay());

    }
    private IEnumerator SpawnEnemiesWithDelay()
    {
        int enemiesToSpawn = currentStage + 1;
        List<Transform> shuffledSpawnPoints = new List<Transform>(enemySpawnPoints);
        ShuffleList(shuffledSpawnPoints);

        yield return new WaitForSeconds(0.5f); //  Delay before first enemy spawns

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Vector3 spawnPos = shuffledSpawnPoints[i % shuffledSpawnPoints.Count].position;

            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            activeEnemies.Add(enemy);

            if (enemy.TryGetComponent(out ConquestEnemy conquestEnemy))
                conquestEnemy.Init(this);

            yield return new WaitForSeconds(0.25f); //  Stagger time between each enemy
        }
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }

    public void NotifyEnemyDied(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
        }

        if (activeEnemies.Count <= 0)
        {
            StartCoroutine(HandleStageCompleteWithDelay());
        }

    }
    private IEnumerator HandleStageCompleteWithDelay()
    {
        PlaydingSound(currentStage);
        yield return new WaitForSeconds(Random.Range(1f, 2f)); //  Delay before lighting torch
      
        if (currentStage < torches.Length)
        {
            torches[currentStage].SetActive(true);
            SoundManager.PlaySound(SoundType.Torch_LitUp);
        }

        currentStage++;
        isPlateLocked = false;
        ppSprite.sprite = openSprite;

        if (currentStage >= 4)
        {
            StartCoroutine(SpawnChestWithDelay(1.5f));
        }
    }

    public void PlaydingSound(int index)
    {
        switch (index)
        {
            case 0:
                SoundManager.PlaySound(SoundType.Conquest_Finish_Ding_1);
                break;
            case 1:
                SoundManager.PlaySound(SoundType.Conquest_Finish_Ding_2);
                break;
            case 2:
                SoundManager.PlaySound(SoundType.Conquest_Finish_Ding_3);
                break;
            case 3:
                SoundManager.PlaySound(SoundType.Conquest_Finish_Ding_4);
                break;
            default:
                return;
        }
    }
    private IEnumerator SpawnChestWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
       
        Instantiate(rewardChest, chestSpawnPoint.position, Quaternion.identity);
        SoundManager.PlaySound(SoundType.Conquest_Chest_Landing);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           
    }

}
