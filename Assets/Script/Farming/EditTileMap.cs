
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

        if (Input.GetMouseButtonDown(0))
        {

            if (GroundLayer.GetTile(pos) == Grass1 || GroundLayer.GetTile(pos) == Grass2 || GroundLayer.GetTile(pos) == Grass3 || GroundLayer.GetTile(pos) == Grass4) 
            { 
            
            PlaceTile(pos, FarmLand);
            
            
            }
            else if(GroundLayer.GetTile(pos) == FarmLand  && !IsSeedPlanted(pos)) {

                PlaceTile(pos, GetRandomGrass());
            
            }
            


        }
        if (Input.GetMouseButtonDown(1) && GroundLayer.GetTile(pos) == FarmLand) 
        {
            HandleClick();
        }
    }

    void PlaceTile(Vector3Int pos, TileBase tile)
    {
        GroundLayer.SetTile(pos, tile);
    }

    TileBase GetRandomGrass()
    {
        TileBase[] grassTiles = { Grass1, Grass2, Grass3, Grass4 };
        return grassTiles[Random.Range(0, grassTiles.Length)];
    }
}
