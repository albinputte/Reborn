using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour 
{
    public bool walkable;
    public Vector3 worldPos;
    public Vector3Int tilePos;
    public Node CameFrom;
    public List<Node> ConnectedNodes = new List<Node>(); // up, down, left, right; upRight, Upleft, downRight,DownLeft
    public float Gscore;
    public float Hscore;
    public float fscore()
    {
        return Gscore + Hscore;
    }


    private void OnDrawGizmos()
    {
         Gizmos.color = Color.red;

        if (ConnectedNodes.Count > 0) {
        for (int i = 0; i < ConnectedNodes.Count; i++)
            {
                Gizmos.DrawLine(transform.position, ConnectedNodes[i].transform.position);
            }
        }
    }
}

