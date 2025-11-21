using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarManger : MonoBehaviour
{
  public static AstarManger instance;

    private void Awake()
    {
        instance = this;
    }

    public List<Node> GeneratePath(Node start, Node end)
    {
        List<Node> openSet = new List<Node>();

        foreach (Node n in FindObjectsOfType<Node>())
        {
            n.Gscore = float.MaxValue;
        }

        start.Gscore = 0;
        start.Hscore = Vector2.Distance(start.transform.position, end.transform.position);
        openSet.Add(start);

        while (openSet.Count > 0)
        {
            int lowestF = default;

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fscore() < openSet[lowestF].fscore())
                {
                    lowestF = i;
                }
            }

            Node currentNode = openSet[lowestF];
            openSet.Remove(currentNode);

            if (currentNode == end)
            {
                List<Node> path = new List<Node>();

                path.Insert(0, end);

                while (currentNode != start)
                {
                    currentNode = currentNode.CameFrom;
                    path.Add(currentNode);
                }

                path.Reverse();
                return path;
            }

            foreach (Node connectedNode in currentNode.ConnectedNodes)
            {
                float heldGScore = currentNode.Gscore + Vector2.Distance(currentNode.transform.position, connectedNode.transform.position);

                if (heldGScore < connectedNode.Gscore)
                {
                    connectedNode.CameFrom = currentNode;
                    connectedNode.Gscore = heldGScore;
                    connectedNode.Hscore = Vector2.Distance(connectedNode.transform.position, end.transform.position);

                    if (!openSet.Contains(connectedNode))
                    {
                        openSet.Add(connectedNode);
                    }
                }
            }
        }

        return null;
    }
    public Node FindNearestNode(Vector2 pos)
    {
        Node foundNode = null;
        float minDistance = float.MaxValue;

        foreach (Node node in FindObjectsOfType<Node>())
        {
            float currentDistance = Vector2.Distance(pos, node.transform.position);

            if (currentDistance < minDistance)
            {
                minDistance = currentDistance;
                foundNode = node;
            }
        }

        return foundNode;
    }

    public Node FindFurthestNode(Vector2 playerPos, Vector2 enemyPos)
    {
         Node foundNode = null;
        float maxDistance = default;
        float MaximumDistance = 20f;

        // Calculate the direction from player to enemy
        Vector2 direction = (enemyPos - playerPos).normalized;

        foreach (Node node in FindObjectsOfType<Node>())
        {
            // Calculate the vector from the player to the node
            Vector2 toNode = new Vector2(node.transform.position.x, node.transform.position.y) - playerPos;

            // Project the toNode vector onto the direction vector (this gives us the distance along the direction)
            float projectedDistance = Vector2.Dot(toNode, direction);

            // Only consider nodes within the maximum distance
            float currentDistance = Vector2.Distance(playerPos, node.transform.position);
            if (currentDistance < MaximumDistance && projectedDistance > maxDistance)
            {
                maxDistance = projectedDistance;
                foundNode = node;
            }
        }

        if (foundNode != null)
        {
            Debug.Log("Furthest Node Position: " + foundNode.transform.position);
        }

        return foundNode;
    }


    public Node[] AllNodes()
    {
        return FindObjectsOfType<Node>();
    }
}
