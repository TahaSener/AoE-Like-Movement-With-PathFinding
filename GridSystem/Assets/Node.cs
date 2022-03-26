using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector3 position;
    public int x;
    public int z;
    public bool unwalkable;
    public int gCost;
    public int hCost;
    public Node parent;
    public Node(Vector3 _position, int _x, int _y, bool _unwalkable)
    {
        position = _position;
         x = _x;
         z = _y;
        unwalkable = _unwalkable;
    }

    public int fCost{
        get
        {
            return gCost + hCost;
        }
     }
}
