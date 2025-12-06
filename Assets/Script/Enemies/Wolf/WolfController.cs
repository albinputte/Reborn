using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfController : EnemyBaseController
{
    public EnemyStateMachine<WolfController> stateMachine;

    private Node HomeNode = null;
    [SerializeField] private List<Node> currentPath = new List<Node>();
    private Node currNode = null;
    private Node EndNode = null;
    public List<Node> PatrolableNodes = new List<Node>();

    public bool CanPatrol = false;
    public Action OnAnimationDone, OnAnimationActionTrigger;
    public Collider2D AttackCollider;

    public int[] PatrolBounds = new int[4];
    public float speed;

    void Start()
    {
        FindPatrollableNode();
    }

    void Update()
    {
        Patrol();
    }

   
    public void FindPatrollableNode()
    {
        HomeNode = AstarManger.instance.FindNearestNode(transform.position);

        if (HomeNode == null)
        {
            Invoke(nameof(FindPatrollableNode), 0.5f);
            return;
        }

        // Scan all nodes inside patrol bounds
        for (int y = PatrolBounds[2]; y <= PatrolBounds[3]; y++)
        {
            for (int x = PatrolBounds[0]; x <= PatrolBounds[1]; x++)
            {
                Vector3 checkPos = new Vector3(HomeNode.transform.position.x + x, HomeNode.transform.position.y + y, 0);

                Node TempNode = AstarManger.instance.FindNearestNode(checkPos);

                if (TempNode != null && Vector3.Distance(TempNode.transform.position, checkPos) < 0.1f)
                {
                    PatrolableNodes.Add(TempNode);
                }
            }
        }

        Debug.Log("Patrollable Nodes: " + PatrolableNodes.Count);

        CanPatrol = PatrolableNodes.Count > 0;
    }


    public void FindPatrol()
    {
        currNode = AstarManger.instance.FindNearestNode(transform.position);

        if (currNode == null)
        {
            Invoke(nameof(FindPatrol), 0.5f);
            return;
        }

        int loopCount = 0;
        bool foundPath = false;

        while (!foundPath)
        {
            // Pick random patrol node
            Node randomTarget = PatrolableNodes[UnityEngine.Random.Range(0, PatrolableNodes.Count)];

            EndNode = randomTarget;
            currentPath = AstarManger.instance.GeneratePath(currNode, EndNode);

            if (currentPath != null && currentPath.Count >= 3)
            {
                foundPath = true;

                // skip starting node
                currNode = currentPath[1];
                currentPath.RemoveAt(0);
            }

            loopCount++;

            if (loopCount >= 25)
            {
                // Go back home if no good patrol route found
                currentPath = AstarManger.instance.GeneratePath(currNode, HomeNode);
                foundPath = true;
            }
        }
    }


    public void Patrol()
    {
        if (!CanPatrol)
            return;

        if (currentPath.Count > 0)
        {
            Vector3 targetPos = new Vector3(
                currentPath[0].transform.position.x,
                currentPath[0].transform.position.y,
                transform.position.z
            );

            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetPos) < 0.1f)
            {
                currNode = currentPath[0];
                currentPath.RemoveAt(0);
            }
        }
        else
        {
            FindPatrol();
        }
    }
}
