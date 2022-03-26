using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField]
    public Transform player;
    public Vector3 space;
    float gridRadius;
    float gridDiameter;
    public int gridSizeX;
    public int gridSizeZ;
    Vector3 bottomLeftPosition;
    public Node[,] grid;
    LayerMask unwalkable;

    private void Awake()
    {
        
        gridInitilaize();
        CreateGrid();


    }
    public void Start()
    {
    }

    private void CreateGrid()
    {
        unwalkable = LayerMask.GetMask("Unwalkable");
        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeZ; j++)
            {
                Vector3 nodePosition = bottomLeftPosition + Vector3.right * (i * gridDiameter + gridRadius) + Vector3.forward * (j * gridDiameter + gridRadius);
                if (Physics.CheckSphere(nodePosition, gridRadius, unwalkable))
                {
                    grid[i, j] = new Node(nodePosition, i, j, true);
                }
                else
                {
                    grid[i, j] = new Node(nodePosition, i, j, false);
                }

            }
        }
    }

    public Node getNodeForCurrent(Vector3 nodePosition)
    {
        float percentX = Mathf.Clamp01((nodePosition.x + space.x / 2) / space.x);
        float percentZ = Mathf.Clamp01((nodePosition.z + space.z / 2) / space.z);
        int gridNumX = Mathf.RoundToInt(percentX * (gridSizeX - 1));
        int gridNumZ = Mathf.RoundToInt(percentZ * (gridSizeZ - 1));

        return grid[gridNumX, gridNumZ];
    }

    public List<Node> FindNeighbours(Node node)
    {

        List<Node> Neighbours=new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                if (x == 0 && z == 0)
                    continue;

                int checkX = node.x + x;
                int checkZ = node.z + z;
                if(checkX<gridSizeX&&checkX>=0&& checkZ < gridSizeZ && checkZ >= 0)
                {
                    Neighbours.Add(grid[checkX,checkZ]);
                }
            }
        }
        return Neighbours;
    }
    private void gridInitilaize()
    {
        gridRadius = 1;
        gridDiameter = gridRadius * 2;
        gridSizeX = Mathf.RoundToInt(space.x / gridDiameter);
        gridSizeZ = Mathf.RoundToInt(space.z / gridDiameter);
        bottomLeftPosition = -(space / 2);
        grid = new Node[gridSizeX, gridSizeZ];
    }


    public List<Node> path;
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, space);
        

        if(grid != null)
        {
            foreach (Node node in grid)
            {

                if (node.unwalkable == true)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawCube(node.position, new Vector3(0.2f, 0.1f, 0.2f));
                }
                else
                {
                    if (node.position == getNodeForCurrent(player.position).position)
                    {
                        Gizmos.color = Color.blue;
                        
                    }
                    else if (path.Contains(node) && path != null)
                    {
                        
                        Gizmos.color = Color.green;
                    }
                    else
                    {
                        Gizmos.color = Color.black;
                    }
                    
                    Gizmos.DrawCube(node.position, new Vector3(1, 0.1f, 1));
                }
                
                
            }
        }
        
    }

   
}
