using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    [Header("Manual Tile Assignment")]
    public RuneTile topLeft;
    public RuneTile top;
    public RuneTile topRight;
    public RuneTile left;
    public RuneTile middle;
    public RuneTile right;
    public RuneTile bottomLeft;
    public RuneTile bottom;
    public RuneTile bottomRight;
    public Animator Stone;
    [Header("Treasure")]
    public GameObject treasurePrefab;
    public Transform treasureSpawnPoint;

    private RuneTile[,] grid = new RuneTile[3, 3];
    private bool puzzleComplete = false;

    void Start()
    {
        grid[0, 2] = topLeft;
        grid[1, 2] = top;
        grid[2, 2] = topRight;

        grid[0, 1] = left;
        grid[1, 1] = middle;
        grid[2, 1] = right;

        grid[0, 0] = bottomLeft;
        grid[1, 0] = bottom;
        grid[2, 0] = bottomRight;

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (grid[x, y] != null)
                    grid[x, y].Init(this, new Vector2Int(x, y));
            }
        }

        middle.SetLit(true);
    }

    public void OnTileStepped(Vector2Int pos)
    {
        if (puzzleComplete) return;

        // First toggle the tile you stepped on
        ToggleTile(pos);

        // Check puzzle immediately after toggling the current tile
        if (IsPuzzleNowSolved())
        {
            CompletePuzzle();
            return;
        }

        // Then toggle neighbors (only if not yet solved)
        Vector2Int[] neighbors = GetCustomNeighbors(pos);

        foreach (var offset in neighbors)
        {
            Vector2Int neighborPos = pos + offset;
            if (IsInBounds(neighborPos))
                ToggleTile(neighborPos);
        }

        // Final check after neighbors are toggled (just in case)
        if (IsPuzzleNowSolved())
        {
            CompletePuzzle();
        }
    }
    private bool IsPuzzleNowSolved()
    {
        foreach (var tile in grid)
        {
            if (tile == null || !tile.isLit)
                return false;
        }
        return true;
    }
    private void CompletePuzzle()
    {
        puzzleComplete = true;

        foreach (var tile in grid)
        {
            if (tile != null)
                tile.SetFinishState();
        }

        Debug.Log("Puzzle Complete!");

        if (treasurePrefab && treasureSpawnPoint)
            Instantiate(treasurePrefab, treasureSpawnPoint.position, Quaternion.identity);
        Stone.SetTrigger("Hit");
    }

    private Vector2Int[] GetCustomNeighbors(Vector2Int pos)
    {
        // Define all patterns by absolute grid positions
        if (pos == new Vector2Int(0, 2)) // TL
            return new[] { new Vector2Int(1, 0), new Vector2Int(0, -1) }; // T, L

        if (pos == new Vector2Int(0, 1)) // L
            return new[] {
            new Vector2Int(1, 0), // middle
            new Vector2Int(0, 1), // TL
            new Vector2Int(0, -1) // BL
        };

        if (pos == new Vector2Int(0, 0)) // BL
            return new[] {
            new Vector2Int(0, 1),  // L
            new Vector2Int(1, 0)   // Bottom
        };

        if (pos == new Vector2Int(1, 0)) // Bottom
            return new[] {
            new Vector2Int(-1, 0), // BL
            new Vector2Int(1, 0),  // BR
            new Vector2Int(0, 1)   // Middle
        };

        if (pos == new Vector2Int(2, 0)) // BR
            return new[] {
            new Vector2Int(-1, 0), // Bottom
            new Vector2Int(0, 1)   // R
        };

        if (pos == new Vector2Int(2, 1)) // R
            return new[] {
            new Vector2Int(-1, 0), // Middle
            new Vector2Int(0, 1),  // TR
            new Vector2Int(0, -1)  // BR
        };

        if (pos == new Vector2Int(2, 2)) // TR
            return new[] {
            new Vector2Int(-1, 0), // Top
            new Vector2Int(0, -1)  // Right
        };

        if (pos == new Vector2Int(1, 1)) // Middle
            return new[] {
            new Vector2Int(0, 1),  // Top
            new Vector2Int(0, -1), // Bottom
            new Vector2Int(1, 0),  // Right
            new Vector2Int(-1, 0)  // Left
        };

        if (pos == new Vector2Int(1, 2)) // Top
            return new[] {
            new Vector2Int(-1, 0), // TL
            new Vector2Int(1, 0),   // TR
            new Vector2Int(0, -1)   // Middle
        };

        return new Vector2Int[0];
    }


    private void ToggleTile(Vector2Int pos)
    {
        if (IsInBounds(pos) && grid[pos.x, pos.y] != null)
            grid[pos.x, pos.y].Toggle();
    }

    private bool IsInBounds(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < 3 && pos.y >= 0 && pos.y < 3;
    }

    public void CheckPuzzleSolved()
    {
        if (puzzleComplete) return;

        // First, check if ALL tiles are lit
        foreach (var tile in grid)
        {
            if (tile == null || !tile.isLit)
                return; // Puzzle not complete yet
        }

        // Puzzle is complete!
        puzzleComplete = true;

        foreach (var tile in grid)
        {
            if (tile != null)
                tile.SetFinishState(); // Now it’s safe
        }

        Debug.Log("Puzzle Complete!");

        if (treasurePrefab && treasureSpawnPoint)
            Instantiate(treasurePrefab, treasureSpawnPoint.position, Quaternion.identity);
    }


    public bool IsPuzzleComplete() => puzzleComplete;
}
