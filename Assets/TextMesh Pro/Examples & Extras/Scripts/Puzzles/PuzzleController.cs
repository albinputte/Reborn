using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    [SerializeField] private GameObject runeTilePrefab;
    [SerializeField] private int gridWidth = 3;
    [SerializeField] private int gridHeight = 3;
    [SerializeField] private Transform startPos;
    [SerializeField] private float tileSpacing = 1.1f;

    private RuneTile[,] grid;

    void Start()
    {
        GenerateTiles();
    }

    void GenerateTiles()
    {
        grid = new RuneTile[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 pos = startPos.position + new Vector3(x * tileSpacing, y * tileSpacing, 0);
                GameObject tileObj = Instantiate(runeTilePrefab, pos, Quaternion.identity);
                RuneTile tile = tileObj.GetComponent<RuneTile>();

                if (tile == null)
                {
                    Debug.LogError("RuneTile component missing on prefab!");
                    continue;
                }

                tile.Init(this, new Vector2Int(x, y)); // Assign controller and position
                grid[x, y] = tile;
            }
        }
    }

    public void OnTileStepped(Vector2Int pos)
    {
        ToggleTile(pos);

        // Toggle neighbors (up/down/left/right)
        Vector2Int[] neighbors = {
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0)
        };

        foreach (var offset in neighbors)
        {
            Vector2Int neighborPos = pos + offset;
            if (IsInBounds(neighborPos))
            {
                ToggleTile(neighborPos);
            }
        }
    }

    private void ToggleTile(Vector2Int pos)
    {
        if (grid == null) return;
        if (pos.x < 0 || pos.x >= gridWidth || pos.y < 0 || pos.y >= gridHeight) return;

        if (grid[pos.x, pos.y] != null)
            grid[pos.x, pos.y].Toggle();
    }

    private bool IsInBounds(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < gridWidth && pos.y >= 0 && pos.y < gridHeight;
    }

    public void CheckPuzzleSolved()
    {
        if (grid == null) return;

        foreach (var tile in grid)
        {
            if (tile == null || !tile.isLit)
                return; // Puzzle not complete yet
        }

        // Puzzle complete!
        foreach (var tile in grid)
        {
            if (tile != null)
                tile.SetFinishState();
        }

        Debug.Log("Puzzle Complete!");
        // Add your reward/chest/spawn logic here if needed
    }
}
