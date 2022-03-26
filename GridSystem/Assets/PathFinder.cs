using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public Transform seeker;
    public Transform target;
    Grid grid;
    int count=0;
    bool findPath=true;
    Vector3 targetPosition;
     void Awake()
    {
        grid = GetComponent<Grid>();
        
    }

    void Update()
    {
        if (findPath)
        {
            StartPathFinding(seeker.position, targetPosition);
        }
        
    }
    public void StartPathFinding(Vector3 startPos,Vector3 EndPos)
    {
        Node startNode = grid.getNodeForCurrent(startPos);
        Node endNode = grid.getNodeForCurrent(EndPos);
        

        List<Node> open = new List<Node>();
        HashSet<Node> closed = new HashSet<Node>();
        

        open.Add(startNode);
        
        while (open.Count > 0)
        {
            Node current = open[0];
            for (int i = 1;  i < open.Count ; i++)
            {
                if (open[i].fCost<current.fCost || current.fCost==open[i].fCost && current.hCost>open[i].hCost)
                {
                    current = open[i];
                }
            }
            open.Remove(current);
            closed.Add(current);
            count += 1;
            
            if (current.x == endNode.x&&current.z==endNode.z)
            {
                RetracePath(startNode,endNode);
                return;
            }
            
            foreach (Node neighbour in grid.FindNeighbours(current))
            {
                if (neighbour.unwalkable || closed.Contains(neighbour))
                {
                    continue;
                }
                   
                    int newMovementCost = current.gCost + getDistance(current, neighbour);
                    if (newMovementCost < neighbour.gCost || !open.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCost;
                        neighbour.hCost = getDistance(neighbour, endNode);
                        neighbour.parent = current;
                        
                        if (!open.Contains(neighbour))
                        {
                            open.Add(neighbour);
                            
                        }
                    }
            }

            
        }
    }

    private int getDistance(Node Seek, Node Target)
    {
        int distanceX = Mathf.Abs(Seek.x - Target.x);
        int distanceZ = Mathf.Abs(Seek.z - Target.z);
        if (distanceX > distanceZ)
        {
            return 14 * distanceZ + 10 * (distanceX - distanceZ);
        }
        else
        {
            return 14 * distanceX + 10 * (distanceZ - distanceX);
        }
    }

    private void RetracePath(Node startNode,Node EndNode)
    {
       
        List<Node> path = new List<Node>();
        Node currentNode = EndNode;
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        
        grid.path = path;
        GetComponent<move>().path=path;
        findPath=false;
    }
    public void startMovement(Vector3 pos)
    {
        targetPosition = pos;
        findPath = true;
    }
}
