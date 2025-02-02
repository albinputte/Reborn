
using UnityEngine;
using UnityEngine.Tilemaps;

public class EditTileMap : MonoBehaviour
{

    [SerializeField] Tilemap CurrentTilemap;
    [SerializeField] TileBase tileBase;
    [SerializeField] Camera cam;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3Int pos = CurrentTilemap.WorldToCell(cam.ScreenToWorldPoint(Input.mousePosition));

        if (Input.GetMouseButton(0)) { 
        
            PlaceTile(pos);
        
        }
    }

    void PlaceTile(Vector3Int pos)
    {
        CurrentTilemap.SetTile(pos, tileBase);
    }
}
