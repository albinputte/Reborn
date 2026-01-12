using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MeteorPillar : GolemPillar
{
    [Header("Prefabs")]
    public GameObject shadowPrefab;
    public GameObject meteorPrefab;
    public GameObject BigCrystalPrefab;

    [Header("References")]
    public Tilemap arenaTilemap;
    [SerializeField] private Transform Center;

    [Header("Meteor Settings")]
    public float spawnHeight = 10f;
    public float warningTime = 1.2f;

    [Header("Grid Settings")]
    public float gridCellSize = 2f;
    public float[] gridcellsizeOffset;
    public int gridRadius = 2;

    [Header("Wave Settings")]
    public int waves = 3;
    public int meteorsPerWave = 4;
    public int BigCrystalPerWave = 1;
    public float waveDelay = 2f;

    [Header("Spawn Bias")]
    [Range(0f, 1f)] public float playerBias = 0.7f;
    [Range(0f, 1f)] public float centerBias = 0.8f;

    private GolemBossController bossController;
    private CreaturePillar healPillar;
    private BoundsInt arenaBounds;

    // Tracks occupied tiles per wave (prevents overlap)
    private HashSet<Vector3Int> occupiedCells = new();

    void Start()
    {
        bossController = GetComponentInParent<GolemBossController>();
        healPillar = GetCreaturePillar();
        arenaBounds = arenaTilemap.cellBounds;
    }

    public override void ExecuteAbility()
    {
        StartCoroutine(MeteorWaveRoutine());
    }

    // ─────────────────────────────────────────────
    // MAIN ROUTINE
    // ─────────────────────────────────────────────
    IEnumerator MeteorWaveRoutine()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 arenaCenter = Center.position;

        for (int w = 0; w < waves; w++)
        {
            occupiedCells.Clear();

            // Small meteors (player pressure)
            for (int i = 0; i < meteorsPerWave; i++)
            {
                if (TryGetValidNearPlayer(player, out Vector3 spawnPos))
                {
                 
                    StartCoroutine(SpawnMeteorWithWarning(spawnPos, meteorPrefab));
                }

                yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
            }

            // Big crystals (arena control)
            for (int i = 0; i < BigCrystalPerWave; i++)
            {
                if (TryGetValidTowardCenter(arenaCenter, out Vector3 spawnPos))
                {
                    StartCoroutine(SpawnMeteorWithWarning(spawnPos, BigCrystalPrefab));
                }
            }

            yield return new WaitForSeconds(waveDelay);
        }
    }

    // ─────────────────────────────────────────────
    // POSITION SELECTION (TRY-PATTERN)
    // ─────────────────────────────────────────────

    bool TryGetValidNearPlayer(Transform player, out Vector3 result)
    {
        for (int attempt = 0; attempt < 15; attempt++)
        {
            Vector3 playerGridPos = SnapToGrid(player.position);
            Vector3 candidate = GetBiasedPositionNearPlayer(playerGridPos);
          
            if (IsValidAndFree(candidate))
            {
                RegisterCell(candidate);
                result = candidate;
               
                return true;
            }
        }

        result = Vector3.zero;
        return false;
    }

    bool TryGetValidTowardCenter(Vector3 center, out Vector3 result)
    {
        for (int attempt = 0; attempt < 15; attempt++)
        {
            Vector3 candidate = GetBiasedPositionTowardCenter(center);

            if (IsValidAndFree(candidate))
            {
          
                result = RegisterCell(candidate);

                return true;
            }
        }

        result = Vector3.zero;
        return false;
    }

    bool IsValidAndFree(Vector3 worldPos)
    {
        Vector3Int cell = arenaTilemap.WorldToCell(worldPos);

        if (!arenaBounds.Contains(cell)) return false;
        if (!arenaTilemap.HasTile(cell)) return false;
        if (occupiedCells.Contains(cell)) return false;

        return true;
    }

    Vector3 RegisterCell(Vector3 worldPos)
    {
        Vector3Int cell = arenaTilemap.WorldToCell(worldPos);
        occupiedCells.Add(cell);
        return arenaTilemap.CellToWorld(cell);
    }

    // ─────────────────────────────────────────────
    // BIAS LOGIC
    // ─────────────────────────────────────────────

    Vector3 GetBiasedPositionNearPlayer(Vector3 playerGridPos)
    {
        List<Vector3> candidates = new();

        for (int x = -gridRadius; x <= gridRadius; x++)
        {
            for (int y = -gridRadius; y <= gridRadius; y++)
            {
                if (x == 0 && y == 0) continue;

                Vector3 pos = playerGridPos + new Vector3(
                    x * (gridCellSize + Random.Range(gridcellsizeOffset[0],gridcellsizeOffset[1])),
                    y * (gridCellSize + Random.Range(gridcellsizeOffset[2], gridcellsizeOffset[3])),
                    0
                );

                candidates.Add(pos);
                
            }
        }

        candidates.Sort((a, b) =>
            Vector3.Distance(a, playerGridPos)
            .CompareTo(Vector3.Distance(b, playerGridPos)));

        int maxIndex = Mathf.RoundToInt(
            Mathf.Lerp(candidates.Count - 1, 0, playerBias)
        );

        return candidates[Random.Range(0, maxIndex + 1)];
    }

    Vector3 GetBiasedPositionTowardCenter(Vector3 center)
    {
        float radius = gridRadius * gridCellSize * 2f;
        Vector2 random = Random.insideUnitCircle * radius;
        Vector2 biased = Vector2.Lerp(random, Vector2.zero, centerBias);
        return SnapToGrid(center + new Vector3(biased.x, biased.y, 0));
    }

    // ─────────────────────────────────────────────
    // SPAWNING
    // ─────────────────────────────────────────────

    IEnumerator SpawnMeteorWithWarning(Vector3 groundPos, GameObject prefab)
    {
        // Shadow ONLY spawns if meteor WILL spawn
        GameObject shadow = Instantiate(shadowPrefab, groundPos, Quaternion.identity);

        yield return new WaitForSeconds(warningTime);

        Vector3 spawnPos = groundPos + Vector3.up * spawnHeight;
        GameObject meteor = Instantiate(prefab, spawnPos, Quaternion.identity);

        if (meteor.TryGetComponent<FallingMeteor>(out FallingMeteor crystal))
        {
            crystal.Init(groundPos, shadow);
        }
        else if (meteor.TryGetComponent<BigCrystalFalling>(out BigCrystalFalling bigCrystal))
        {
            bigCrystal.Init(groundPos, shadow, healPillar);
        }
    }

    // ─────────────────────────────────────────────
    // HELPERS
    // ─────────────────────────────────────────────

    Vector3 SnapToGrid(Vector3 position)
    {
        float x = Mathf.Round(position.x / gridCellSize) * gridCellSize;
        float y = Mathf.Round(position.y / gridCellSize) * gridCellSize;
        return new Vector3(x, y, 0);
    }

    private CreaturePillar GetCreaturePillar()
    {
        foreach (var pillar in bossController.pillars)
        {
            if (pillar is CreaturePillar creature)
                return creature;
        }

        Debug.LogWarning("No CreaturePillar found!");
        return null;
    }
}
