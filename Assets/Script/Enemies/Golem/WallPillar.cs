using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WallPillar : GolemPillar
{
    [Header("Tilemap")]
    public Tilemap WalkableTiels;

    [Header("Laser")]
    public Transform[] laserStartPostions;   // size 2
    public Transform laserMover;             // object that moves between points
    public float laserMoveSpeed = 5f;
    public bool laserMoving = false;

    [Header("Spawn Positions")]
    public Vector3 leftSpawn;
    public Vector3 rightSpawn;

    [Header("Player")]
    public Transform Player;

    [Header("Offsets")]
    public float LaserOffestVertical;
    private float HorizentalCount;
    public float LaserOffestHorizental;

    [Header("Tile Exit Spawn")]
    public GameObject[] exitTileSpawnPrefab;
    private int exitTileSpawnCount = 0;

    public GameObject Laser;
    public GameObject LaserPoint;
    // ===== Tile tracking =====
    private Vector3Int lastCell;
    private bool hasLastCell = false;

    // =========================
    // EXECUTE ABILITY
    // =========================
    public override void ExecuteAbility()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
       

        CheckSpawnHorizental(Player);
        StartLaserMovement();
    }

    // =========================
    // VERTICAL CHECK
    // =========================
    public void CheckSpawnVertical(Transform playerPos)
    {
        int baseX = Mathf.FloorToInt(playerPos.position.x);
        int baseY = Mathf.FloorToInt(playerPos.position.y);

        bool foundUp = false;
        bool foundDown = false;

        for (int i = (int)LaserOffestVertical; i >= 0; i--)
        {
            Vector3 checkPos = new Vector3(baseX, baseY + i, 0);
            if (IsValidpos(checkPos))
            {
                rightSpawn = checkPos;
                foundUp = true;
                break;
            }
        }

        for (int i = (int)LaserOffestVertical; i >= 0; i--)
        {
            Vector3 checkPos = new Vector3(baseX, baseY - i, 0);
            if (IsValidpos(checkPos))
            {
                leftSpawn = checkPos;
                foundDown = true;
                break;
            }
        }

        if (!foundUp)
            rightSpawn = new Vector3(baseX, baseY, 0);

        if (!foundDown)
            leftSpawn = new Vector3(baseX, baseY, 0);

        if (!foundUp && !foundDown)
            CheckSpawnHorizental(playerPos);
    }

    // =========================
    // HORIZONTAL CHECK
    // =========================
    public void CheckSpawnHorizental(Transform playerPos)
    {
        int baseX = Mathf.FloorToInt(playerPos.position.x);
        int baseY = Mathf.FloorToInt(playerPos.position.y);

        bool foundRight = false;
        bool foundLeft = false;

        for (int i = (int)LaserOffestHorizental; i >= 0; i--)
        {
            Vector3 checkPos = new Vector3(baseX + i, baseY, 0);
            if (IsValidpos(checkPos))
            {
                rightSpawn = checkPos;
                foundRight = true;
                break;
            }
        }

        for (int i = (int)LaserOffestHorizental; i >= 0; i--)
        {
            Vector3 checkPos = new Vector3(baseX - i, baseY, 0);
            if (IsValidpos(checkPos))
            {
                leftSpawn = checkPos;
                foundLeft = true;
                break;
            }
        }

        if (!foundRight)
            rightSpawn = new Vector3(baseX, baseY, 0);

        if (!foundLeft)
            leftSpawn = new Vector3(baseX, baseY, 0);
    }

    // =========================
    // START LASER MOVEMENT
    // =========================
    public void StartLaserMovement()
    {
        if (laserMover == null)
            return;
        exitTileSpawnCount = 0;
        HorizentalCount = 0;
        laserMoving = true;
        StopAllCoroutines();
        StartCoroutine(MoveLaserOnce(leftSpawn, rightSpawn));
    }

    // =========================
    // LASER MOVEMENT (WITH TILE EXIT)
    // =========================
    IEnumerator MoveLaserOnce(Vector3 start, Vector3 end)
    {
        LaserPoint.SetActive(true);
        Laser.SetActive(true);
        laserMover.position = start;

        lastCell = WalkableTiels.WorldToCell(start);
        hasLastCell = true;

        float distance = Vector3.Distance(start, end);
        float t = 0f;

        while (t < 1f)
        {
            t += (Time.deltaTime * laserMoveSpeed) / distance;
            laserMover.position = Vector3.Lerp(start, end, t);

            CheckTileExit();

            yield return null;
        }

        laserMover.position = end;
        laserMoving = false;
        Laser.SetActive(false);
        LaserPoint.SetActive(false);
      
    }

    // =========================
    // TILE EXIT CHECK
    // =========================
    void CheckTileExit()
    {
        Vector3Int currentCell = WalkableTiels.WorldToCell(laserMover.position);

        if (!hasLastCell)
        {
            lastCell = currentCell;
            hasLastCell = true;
            Instantiate(exitTileSpawnPrefab[1], lastCell, Quaternion.identity);
            return;
        }

        if (currentCell != lastCell)
        {
            SpawnOnExitedTile(lastCell);
            lastCell = currentCell;
        }
    }

    // =========================
    // SPAWN ON EXITED TILE
    // =========================
    void SpawnOnExitedTile(Vector3Int exitedCell)
    {
        if (!WalkableTiels.HasTile(exitedCell))
            return;
     

        Vector3 spawnWorldPos =
            WalkableTiels.GetCellCenterWorld(exitedCell);
        if (exitTileSpawnCount == 0)
        {
            Instantiate(exitTileSpawnPrefab[0], spawnWorldPos, Quaternion.identity);
            exitTileSpawnCount++;
        }
        else if (HorizentalCount == (LaserOffestHorizental *2) -1)
        {
            Instantiate(exitTileSpawnPrefab[1], spawnWorldPos, Quaternion.identity);
        }
        else
        {
            if (exitTileSpawnCount == 1)
            {
                Instantiate(exitTileSpawnPrefab[2], spawnWorldPos, Quaternion.identity);
                exitTileSpawnCount++;
            }
            else {
                Instantiate(exitTileSpawnPrefab[3], spawnWorldPos, Quaternion.identity);
                exitTileSpawnCount = 1;
            }

         
        }
        HorizentalCount++;

    }

    // =========================
    // TILE CHECK
    // =========================
    bool IsValidpos(Vector3 worldPos)
    {
        Vector3Int cell = WalkableTiels.WorldToCell(worldPos);
        return WalkableTiels.HasTile(cell);
    }
}
