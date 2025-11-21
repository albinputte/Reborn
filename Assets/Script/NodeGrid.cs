using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NodeGrid : MonoBehaviour
{

    public Node NodePrefab;

    [Header("Tilemaps")]
    public Tilemap groundMap;     // Walkable layer
    public Tilemap cliffMap;      // Blocking layer

    [Header("Debug")]
    public bool showGizmos = true;

    public List<Node> nodes = new List<Node>();

    void Start()
    {
        GenerateGrid();
      
    }

    // -------------------------------------------------------
    // CHECK WALKABLE CONDITION
    // -------------------------------------------------------
    bool IsWalkable(Vector3Int pos)
    {
        // Ground tile must exist
        if (groundMap.GetTile(pos) == null)
            return false;

        // If a cliff tile exists at the same position → blocked
        if (cliffMap != null && cliffMap.GetTile(pos) != null)
            return false;

        return true;
    }

    // -------------------------------------------------------
    // GRID GENERATION
    // -------------------------------------------------------
    void GenerateGrid()
    {
        nodes.Clear();

        groundMap.CompressBounds();
        BoundsInt bounds = groundMap.cellBounds;

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            if (IsWalkable(pos))
            {
                Vector3 world = groundMap.GetCellCenterWorld(pos);
                Node node = Instantiate(NodePrefab, new Vector2(pos.x + 0.5f, pos.y + 0.5f), Quaternion.identity);

                
                nodes.Add(node);
            }
        }

        CreateConections();
    }
    public void CreateConections()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            for (int j = i + 1; j < nodes.Count; j++)
            {
                if (Vector2.Distance(nodes[i].transform.position, nodes[j].transform.position) <= 1.5f)
                {
                    ConnectNeighbors(nodes[i], nodes[j]);
                    ConnectNeighbors(nodes[j], nodes[i]);
                }
            }
        }
    }



    // -------------------------------------------------------
    // NEIGHBOR CONNECTION
    // -------------------------------------------------------
    void ConnectNeighbors(Node from, Node too)
            {
                if (from == too) return;
                from.ConnectedNodes.Add(too);

            }

            // -------------------------------------------------------
            // VISUALIZATION
            // -------------------------------------------------------
            /*
            void OnDrawGizmos()
            {
                if (!showGizmos || nodes == null || nodes.Count == 0)
                    return;

                Gizmos.color = Color.green;

                foreach (var node in nodes.Values)
                {
                    // Draw node
                    Gizmos.DrawSphere(node.worldPos, 0.1f);

                    // Draw connections to right and up (for clean visualization)
                    if (node.ConnectedNodes[3] != null)
                        Gizmos.DrawLine(node.worldPos, node.ConnectedNodes[3].worldPos);
                    if (node.ConnectedNodes[4] != null)
                        Gizmos.DrawLine(node.worldPos, node.ConnectedNodes[4].worldPos);
                    if (node.ConnectedNodes[5] != null)
                        Gizmos.DrawLine(node.worldPos, node.ConnectedNodes[5].worldPos);



                    if (node.ConnectedNodes[0] != null)
                        Gizmos.DrawLine(node.worldPos, node.ConnectedNodes[0].worldPos);
                }
            }
            */
}



