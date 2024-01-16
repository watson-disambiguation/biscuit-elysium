using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool walkable;
    public Vector3 worldPosition;
    public int x;
    public int y;
    public int weight;
    public Node parent;

    public int gCost, hCost;

    int heapIndex;

    public Node(bool _walkable, Vector3 _worldPosition, int _gridX, int _gridY, int _weight)
    {
        walkable = _walkable;
        worldPosition = _worldPosition;
        x = _gridX;
        y = _gridY;
        weight = _weight;
        
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public int HeapIndex
    {
        get 
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node toCompare)
    {
        int compare = fCost.CompareTo(toCompare.fCost);
        if(compare == 0)
        {
            compare = hCost.CompareTo(toCompare.hCost);
        }
        return -compare;
    }
}
