
using UnityEngine;
using UnityEngine.Tilemaps;

public class EditTileMap : MonoBehaviour
{

    [SerializeField] Tilemap CurrentTilemap;
    [SerializeField] TileBase Grass1;
    [SerializeField] TileBase Grass2;
    [SerializeField] TileBase Grass3;
    [SerializeField] TileBase Grass4;
    [SerializeField] TileBase FarmLand;
    [SerializeField] Camera cam;

    [SerializeField] GameObject seedPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3Int pos = CurrentTilemap.WorldToCell(cam.ScreenToWorldPoint(Input.mousePosition));

        if (Input.GetMouseButtonDown(0))
        {

            if (CurrentTilemap.GetTile(pos) == Grass1 || CurrentTilemap.GetTile(pos) == Grass2 || CurrentTilemap.GetTile(pos) == Grass3 || CurrentTilemap.GetTile(pos) == Grass4) 
            { 
            
            PlaceTile(pos, FarmLand);
            
            
            }
            else if(CurrentTilemap.GetTile(pos) == FarmLand) {

                PlaceTile(pos, GetRandomGrass());
            
            }
            


        }
        if (Input.GetMouseButtonDown(1) && CurrentTilemap.GetTile(pos) == FarmLand) 
        {
            Vector3 seedpos = cam.ScreenToWorldPoint(Input.mousePosition);
            seedpos.z = 0;
            GameObject newSeed = Instantiate(seedPrefab, seedpos , Quaternion.identity);


        }
    }

    void PlaceTile(Vector3Int pos, TileBase tile)
    {
        CurrentTilemap.SetTile(pos, tile);
    }

    TileBase GetRandomGrass()
    {
        TileBase[] grassTiles = { Grass1, Grass2, Grass3, Grass4 };
        return grassTiles[Random.Range(0, grassTiles.Length)];
    }
}
