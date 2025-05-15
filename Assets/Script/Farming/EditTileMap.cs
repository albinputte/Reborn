
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EditTileMap : MonoBehaviour
{

    [SerializeField] Tilemap GroundLayer;
    [SerializeField] Tilemap BuildLayer;
    [SerializeField] TileBase Grass1;
    [SerializeField] TileBase Grass2;
    [SerializeField] TileBase Grass3;
    [SerializeField] TileBase Grass4;
    [SerializeField] TileBase FarmLand;
    [SerializeField] Camera cam;

    [SerializeField] GameObject seedPrefab;

    private bool Buildmode = false;

    private Dictionary<Vector3Int, GameObject> placedSeeds = new Dictionary<Vector3Int, GameObject>();
    private Dictionary<Vector3Int, float> seedTimers = new Dictionary<Vector3Int, float>();

    public TileBase[] wallTiles; // Assign different sprites based on bitmask
    private Dictionary<Vector3Int, TileBase> placedTiles = new Dictionary<Vector3Int, TileBase>();


    void HandleClick()
    {
        Vector3Int cellPosition = GroundLayer.WorldToCell(cam.ScreenToWorldPoint(Input.mousePosition));

        if (placedSeeds.ContainsKey(cellPosition))
        {
            float timePlaced = seedTimers[cellPosition];

            if (Time.time - timePlaced >= 20f) // 20 seconds have passed
            {
                RemoveSeed(cellPosition);
            }
            else
            {
                Debug.Log("Seed is not ready to be removed yet.");
            }
        }
        else
        {
            PlaceSeed(cellPosition);
        }
    }

    void PlaceSeed(Vector3Int cellPosition)
    {
        if (placedSeeds.ContainsKey(cellPosition))
        {
            Debug.Log("A seed is already placed here!");
            return;
        }

        Vector3 seedPos = GroundLayer.GetCellCenterWorld(cellPosition);
        GameObject newSeed = Instantiate(seedPrefab, seedPos, Quaternion.identity);

        // Store the seed and the time it was placed
        placedSeeds[cellPosition] = newSeed;
        seedTimers[cellPosition] = Time.time;
    }

    void RemoveSeed(Vector3Int cellPosition)
    {
        if (placedSeeds.ContainsKey(cellPosition))
        {
            Destroy(placedSeeds[cellPosition]); // Destroy the seed GameObject
            placedSeeds.Remove(cellPosition);
            seedTimers.Remove(cellPosition);

            Debug.Log("Seed removed!");
        }
    }

    bool IsSeedPlanted(Vector3Int cellPosition)
    {
        return placedSeeds.ContainsKey(cellPosition);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3Int pos = GroundLayer.WorldToCell(cam.ScreenToWorldPoint(Input.mousePosition));

        if (Buildmode)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PlaceWall(pos);
            }
            if (Input.GetMouseButtonDown(1))
            {
                RemoveWall(pos);
            }

        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {

                if (GroundLayer.GetTile(pos) == Grass1 || GroundLayer.GetTile(pos) == Grass2 || GroundLayer.GetTile(pos) == Grass3 || GroundLayer.GetTile(pos) == Grass4)
                {

                    PlaceTile(pos, FarmLand);


                }
                else if (GroundLayer.GetTile(pos) == FarmLand && !IsSeedPlanted(pos))
                {

                    PlaceTile(pos, GetRandomGrass());

                }
            }
            if (Input.GetMouseButtonDown(1) && GroundLayer.GetTile(pos) == FarmLand)
            {
                HandleClick();
            }
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            Buildmode = !Buildmode;
        }
    }

    void PlaceTile(Vector3Int pos, TileBase tile)
    {
        GroundLayer.SetTile(pos, tile);
    }

    private void UpdateTile(Vector3Int position)
    {
        if (!placedTiles.ContainsKey(position)) return;

        int bitmask = 0;
        if (placedTiles.ContainsKey(position + Vector3Int.up)) bitmask |= 1 << 0;
        if (placedTiles.ContainsKey(position + Vector3Int.right)) bitmask |= 1 << 1;
        if (placedTiles.ContainsKey(position + Vector3Int.down)) bitmask |= 1 << 2;
        if (placedTiles.ContainsKey(position + Vector3Int.left)) bitmask |= 1 << 3;

        BuildLayer.SetTile(position, wallTiles[bitmask]);
        placedTiles[position] = wallTiles[bitmask];
    }

    public void PlaceWall(Vector3Int position)
    {
        if (placedTiles.ContainsKey(position)) return; // Tile already exists

        BuildLayer.SetTile(position, wallTiles[15]); // Temporarily place a default wall
        placedTiles[position] = wallTiles[15];

        // Update this tile and its neighbors
        UpdateTile(position);
        UpdateTile(position + Vector3Int.up);
        UpdateTile(position + Vector3Int.down);
        UpdateTile(position + Vector3Int.left);
        UpdateTile(position + Vector3Int.right);
    }

    public void RemoveWall(Vector3Int position)
    {
        if (!placedTiles.ContainsKey(position)) return; // No wall to remove

        // Remove the tile
        BuildLayer.SetTile(position, null);
        placedTiles.Remove(position);

        // Update neighboring tiles
        UpdateTile(position + Vector3Int.up);
        UpdateTile(position + Vector3Int.down);
        UpdateTile(position + Vector3Int.left);
        UpdateTile(position + Vector3Int.right);
    }
    public List<GameObject> GetObjectsInTile(Vector3 tileCenter, Vector3 tileSize, LayerMask layerMask)
    {
        List<GameObject> objectsInTile = new List<GameObject>();
        Bounds tileBounds = new Bounds(tileCenter, tileSize);

        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == layerMask)
            {
                Vector3 objPos = obj.transform.position;
                if (tileBounds.Contains(objPos))
                {
                    objectsInTile.Add(obj);
                }
            }
        }

        return objectsInTile;
    }

    TileBase GetRandomGrass()
    {
        TileBase[] grassTiles = { Grass1, Grass2, Grass3, Grass4 };
        return grassTiles[Random.Range(0, grassTiles.Length)];
    }
}
