using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class TileManger : MonoBehaviour
{
    [SerializeField] private Tilemap GroundLayer;
    [SerializeField] private Tilemap BuildLayer;
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject seedPrefab;

    [SerializeField] private TileBase[] GrassTiles;
    [SerializeField] private TileBase FarmLand;

    private Dictionary<Vector3Int, GameObject> placedObjects = new();
    private Dictionary<Vector3Int, float> objectTimers = new();
    private List<TileRule> tileRules = new();

    private void Start()
    {
        tileRules.Add(new TileRule(
            (tile, pos) => IsGrassTile(tile),
            (pos) => PlaceTile(pos, FarmLand)
        ));

        tileRules.Add(new TileRule(
            (tile, pos) => tile == FarmLand && !IsObjectPlaced(pos),
            (pos) => PlaceTile(pos, GetRandomGrassTile())
        ));
    }

    private void Update()
    {
        Vector3Int pos = GroundLayer.WorldToCell(cam.ScreenToWorldPoint(Input.mousePosition));

        if (Input.GetMouseButtonDown(0))
        {
            ProcessTilePlacement(pos);
        }

        if (Input.GetMouseButtonDown(1))
        {
            HandleObjectPlacement(pos);
        }
    }

    private void ProcessTilePlacement(Vector3Int pos)
    {
        TileBase tile = GroundLayer.GetTile(pos);

        foreach (var rule in tileRules)
        {
            if (rule.Condition(tile, pos))
            {
                rule.Action(pos);
                return;
            }
        }
    }

    private void HandleObjectPlacement(Vector3Int cellPosition)
    {
        if (placedObjects.ContainsKey(cellPosition))
        {
            if (Time.time - objectTimers[cellPosition] >= 20f)
            {
                RemoveObject(cellPosition);
            }
            else
            {
                Debug.Log("Object is not ready to be removed yet.");
            }
        }
        else
        {
            PlaceObject(cellPosition);
        }
    }

    private void PlaceTile(Vector3Int pos, TileBase tile)
    {
        GroundLayer.SetTile(pos, tile);
    }

    private void PlaceObject(Vector3Int cellPosition)
    {
        Vector3 objPos = GroundLayer.GetCellCenterWorld(cellPosition);
        GameObject newObj = Instantiate(seedPrefab, objPos, Quaternion.identity);
        placedObjects[cellPosition] = newObj;
        objectTimers[cellPosition] = Time.time;
    }

    private void RemoveObject(Vector3Int cellPosition)
    {
        if (placedObjects.TryGetValue(cellPosition, out var obj))
        {
            Destroy(obj);
            placedObjects.Remove(cellPosition);
            objectTimers.Remove(cellPosition);
        }
    }

    private bool IsObjectPlaced(Vector3Int cellPosition) => placedObjects.ContainsKey(cellPosition);

    private bool IsGrassTile(TileBase tile) => System.Array.Exists(GrassTiles, t => t == tile);

    private TileBase GetRandomGrassTile() => GrassTiles[Random.Range(0, GrassTiles.Length)];
}

public class TileRule
{
    public System.Func<TileBase, Vector3Int, bool> Condition { get; }
    public System.Action<Vector3Int> Action { get; }

    public TileRule(System.Func<TileBase, Vector3Int, bool> condition, System.Action<Vector3Int> action)
    {
        Condition = condition;
        Action = action;
    }
}
