using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeteorPillar : GolemPillar
{
    [Header("Prefabs")]
    public GameObject shadowPrefab;
    public GameObject meteorPrefab;

    public GameObject BigCrystalShadow;
    public GameObject BigCrystalPrefab;

    [SerializeField]private List<GameObject> CrystalSpawnList = new List<GameObject>();
    private GolemBossController bossController;
    private CreaturePillar HealPilar;

    [Header("Meteor Settings")]
    public float spawnHeight = 10f;
    public float warningTime = 1.2f;

    [Header("Grid Settings")]
    public float gridCellSize = 2f;
    public int gridRadius = 1; // 1 = 3x3 grid

    [Header("Wave Settings")]
    public int waves = 3;
    public int meteorsPerWave = 4;
    public int BigCrystalPerWave;
    public float waveDelay = 2f;

    [Header("Random Spawn Delay")]
    public float minSpawnDelay = 0f;
    public float maxSpawnDelay = 0.4f;
    public void Start()
    {
        bossController = GetComponentInParent<GolemBossController>();
        HealPilar = GetCreaturePillar();
    }
    public override void ExecuteAbility()
    {
        StartCoroutine(MeteorGridWavesRoutine());
    }

    IEnumerator MeteorGridWavesRoutine()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;

        for (int w = 0; w < waves; w++)
        {
            Vector3 playerGridPos = SnapToGrid(player.position);

            // 1️⃣ Always spawn one on player
            SpawnMeteor(playerGridPos);

            // 2️⃣ Collect available grid positions
            List<Vector3> availablePositions = GetGridPositionsAround(playerGridPos);

            // Remove player cell so no duplicates
            availablePositions.Remove(playerGridPos);
            ShuffleCrystals();
            // Shuffle positions
            Shuffle(availablePositions);

            int count = Mathf.Min(meteorsPerWave+ BigCrystalPerWave, availablePositions.Count);

            for (int i = 0; i < count; i++)
            {
                float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
                StartCoroutine(DelayedMeteorSpawn(availablePositions[i], delay, CrystalSpawnList[i]));
                yield return new WaitForSeconds(delay);
            }

            yield return new WaitForSeconds(waveDelay);
        }
    }

    #region Grid Helpers

    Vector3 SnapToGrid(Vector3 position)
    {
        float x = Mathf.Round(position.x / gridCellSize) * gridCellSize;
        float y = Mathf.Round(position.y / gridCellSize) * gridCellSize;
        return new Vector3(x, y, 0);
    }

    List<Vector3> GetGridPositionsAround(Vector3 center)
    {
        List<Vector3> positions = new();

        for (int x = -gridRadius; x <= gridRadius; x++)
        {
            for (int y = -gridRadius; y <= gridRadius; y++)
            {
                Vector3 pos = center + new Vector3(
                    x * gridCellSize,
                    y * gridCellSize,
                    0
                );
                positions.Add(pos);
            }
        }

        return positions;
    }

    public void ShuffleCrystals()
    {
        CrystalSpawnList.Clear();

        // Add meteors
        for (int i = 0; i < meteorsPerWave; i++)
        {
            CrystalSpawnList.Add(meteorPrefab);
        }

        // Add big crystals
        for (int i = 0; i < BigCrystalPerWave; i++)
        {
            CrystalSpawnList.Add(BigCrystalPrefab);

        }

        // Shuffle list (Fisher–Yates)
        for (int i = 0; i < CrystalSpawnList.Count; i++)
        {
            int rand = Random.Range(i, CrystalSpawnList.Count);
            (CrystalSpawnList[i], CrystalSpawnList[rand]) =
                (CrystalSpawnList[rand], CrystalSpawnList[i]);
        }
    }

    void Shuffle(List<Vector3> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rnd = Random.Range(i, list.Count);
            (list[i], list[rnd]) = (list[rnd], list[i]);
        }
    }

    #endregion

    #region Meteor Spawn

    void SpawnMeteor(Vector3 groundPos)
    {
        StartCoroutine(MeteorRoutine(groundPos, meteorPrefab));
    }

    IEnumerator DelayedMeteorSpawn(Vector3 groundPos, float delay, GameObject FallingPrefab)
    {
        yield return new WaitForSeconds(delay);
        yield return MeteorRoutine(groundPos, FallingPrefab);
    }

    IEnumerator MeteorRoutine(Vector3 groundPos, GameObject FallingPrefab)
    {
        GameObject shadow = Instantiate(shadowPrefab, groundPos, Quaternion.identity);

        yield return new WaitForSeconds(warningTime);

        Vector3 meteorPos = groundPos + Vector3.up * spawnHeight;
        GameObject meteor = Instantiate(FallingPrefab, meteorPos, Quaternion.identity);

        if (meteor.TryGetComponent<FallingMeteor>(out FallingMeteor crystal))
        {
            crystal.Init(groundPos, shadow);
        }
        else if(meteor.TryGetComponent<BigCrystalFalling>(out BigCrystalFalling BigCrystal))
        {
            BigCrystal.Init(groundPos, shadow, HealPilar);
        }


     
             
    }
    private CreaturePillar GetCreaturePillar()
    {
        foreach (var pillar in bossController.pillars)
        {
            if (pillar is CreaturePillar creaturePillar)
                return creaturePillar;
        }

        Debug.LogWarning("No CreaturePillar found!");
        return null;
    }

    #endregion
}
