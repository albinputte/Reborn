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
    private SpriteRenderer sr;
    [SerializeField] private TileBase[] GrassTiles;
    [SerializeField] private TileBase FarmLand;
    [SerializeField] private LayerMask NonPlacableLayers;

    private Dictionary<Vector3Int, GameObject> placedObjects = new();
    private Dictionary<Vector3Int, float> objectTimers = new();
    private List<TileRule> tileRules = new();

    private GameObject previewObject;
    public bool BuildMode;
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

        CreatePreviewObject(seedPrefab);
        BuildMode = false;
    }

    private void Update()
    {
        if (BuildMode) { 
        Vector3Int pos = GroundLayer.WorldToCell(cam.ScreenToWorldPoint(Input.mousePosition));
        UpdatePreviewPosition(pos);

        if (Input.GetMouseButtonDown(0))
        {
            ProcessTilePlacement(pos);
        }

        if (Input.GetMouseButtonDown(1))
        {
            HandleObjectPlacement(pos);
        }
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
            if (GetObjectsInTile(cellPosition, NonPlacableLayers).Count == 0)
            {
                PlaceObject(cellPosition);
            }
        }
    }
    public void ActivateBuildMode(GameObject prefab)
    {
        CreatePreviewObject(prefab);
        seedPrefab = prefab;
        BuildMode = true;
        previewObject.SetActive(true);
    }

    public void disableBuildMode()
    {
        BuildMode = false;
        previewObject.SetActive(false);
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
        disableBuildMode();
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

    public List<GameObject> GetObjectsInTile(Vector3Int cellPosition, LayerMask layerMask)
    {
        List<GameObject> objectsInTile = new();

        Vector3 tileCenter = GroundLayer.GetCellCenterWorld(cellPosition);
        Vector3 tileSize = GroundLayer.cellSize;
        Bounds tileBounds = new Bounds(tileCenter, tileSize);

        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (!obj.activeInHierarchy) continue;

            if (((1 << obj.layer) & layerMask) != 0)
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

    private void CreatePreviewObject(GameObject Prefab)
    {
        if (Prefab == null || Prefab.GetComponent<SpriteRenderer>() == null)
        {
            Debug.LogWarning("Seed prefab or its SpriteRenderer is missing.");
            return;
        }

        previewObject = new GameObject("PreviewObject");
        sr = previewObject.AddComponent<SpriteRenderer>();

        sr.sprite = Prefab.GetComponent<SpriteRenderer>().sprite;
        sr.color = new Color(1f, 1f, 1f, 0.5f); // Semi-transparent
        sr.sortingLayerName = "Foreground";
        sr.sortingOrder = 5;

        //previewObject.SetActive(false);
    }

    private void UpdatePreviewPosition(Vector3Int cellPosition)
    {
        if (IsObjectPlaced(cellPosition) || GetObjectsInTile(cellPosition, NonPlacableLayers).Count > 0)
        {
            sr.color = new Color(1f, 0f, 0f, 0.5f);
            //previewObject.SetActive(false);
            
        }
        else
            sr.color = new Color(1f, 1f, 1f, 0.5f);

        Vector3 worldPos = GroundLayer.GetCellCenterWorld(cellPosition);
        previewObject.transform.position = worldPos;

      
          
    }
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
