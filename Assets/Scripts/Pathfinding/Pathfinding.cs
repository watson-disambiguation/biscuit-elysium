using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pathfinding : MonoBehaviour
{
    [SerializeField]
    NavGrid grid;
    PathRequestManager requestManager;
    private void Awake()
    {
        if (grid == null) grid = GetComponent<NavGrid>();
        requestManager = GetComponent<PathRequestManager>();
    }

    private void Update()
    {

    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }
    IEnumerator FindPath(Vector3 start, Vector3 target)
    {
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;
        Node startNode = grid.NodeFromWorldPoint(start);
        Node targetNode = grid.NodeFromWorldPoint(target); 
        
        Heap<Node> open = new Heap<Node>(grid.MaxSize);
        HashSet<Node> closed = new HashSet<Node>();
        open.Add(startNode);

        while(open.Count>0)
        {
            Node currentNode = open.RemoveFirst();
            closed.Add(currentNode);
            if(startNode.walkable && targetNode.walkable)
            {
                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break;
                }

                foreach (Node neighbour in grid.getNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closed.Contains(neighbour)) continue;
                    int newMoveCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) + neighbour.weight;
                    if (newMoveCostToNeighbour < neighbour.gCost || !open.Contains(neighbour))
                    {
                        neighbour.gCost = newMoveCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;
                        if (!open.Contains(neighbour))
                        {
                            open.Add(neighbour);
                        }
                        else
                        {
                            open.UpdateItem(neighbour);
                        }
                    }


                }
            }
        }
        yield return null;
        if(pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);    

    }

    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = simplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
        
    }

    Vector3[] simplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 dirOld = Vector2.zero;
        if(path.Count == 0) return waypoints.ToArray();
        for (int i = 1; i < path.Count; i++)
        {
            Vector2 dirNew = new Vector2(path[i-1].x - path[i].x,path[i-1].y - path[i].y);
            if(dirNew != dirOld) { waypoints.Add(path[i].worldPosition); }
            dirOld = dirNew;
        }
        waypoints.Add(path[path.Count-1].worldPosition);
        return waypoints.ToArray();
    }

    int GetDistance(Node a, Node b)
    {
        int distX = Mathf.Abs(a.x - b.x);
        int distY = Mathf.Abs(a.y - b.y);
        int diagonals = (distX > distY) ? distY : distX;
        int horizontals = ((distX > distY) ? distX : distY) - diagonals;
        return diagonals * 14 + horizontals * 10;
    }
}
