using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavGrid : MonoBehaviour
{
    public bool displayGizmos;
    public int blurSize = 3;
    public int unwalkableWeight = 10;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public float unwalkableWeightRadius;
    public TerrainType[] terrainTypes;
    Dictionary<int, int> terrainDict = new Dictionary<int, int>();

    LayerMask terrainMask;

    Node[,] grid;

    float nodeDiameter;
    int gridSizeX;
    int gridSizeY;

    int maxWeight = int.MinValue;
    int minWeight = int.MaxValue;

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }
    private void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        foreach (TerrainType region in terrainTypes)
        {
            terrainMask.value |= region.terrainMask;
            terrainDict.Add((int)Mathf.Log(region.terrainMask.value,2), region.terrainWeight);
        }
        CreateGrid();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 0));
        if (grid != null && displayGizmos)
        {
            foreach (Node n in grid)
            {

                Color weightColor = Color.Lerp(Color.black, Color.white, Mathf.InverseLerp(minWeight, maxWeight, n.weight));
                Gizmos.color = (n.walkable) ? weightColor : Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * nodeDiameter * 0.9f);
            }
        }

    }

    public List<Node> getNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                int checkX = node.x +x;
                int checkY = node.y +y;

                if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }

    public Node NodeFromWorldPoint(Vector2 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x,y];
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector2 worldBottomLeft = (Vector2)transform.position - Vector2.right * gridWorldSize.x/2 - Vector2.up * gridWorldSize.y/2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector2 worldPoint = worldBottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.up * (y * nodeDiameter + nodeRadius);
                bool walkable = (Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask) == null);
                int weight = 0;

                Ray ray = new Ray(new Vector3(worldPoint.x, worldPoint.y, 10), Vector3.back);
                if(walkable)
                {
                    RaycastHit2D hit = Physics2D.GetRayIntersection(ray, 10, terrainMask);
                    if (hit)
                    {
                        if (terrainDict.TryGetValue(hit.collider.gameObject.layer, out int newWeight))
                        {
                            weight = newWeight;
                        }
                    }
                }
                if(Physics2D.OverlapCircle(worldPoint, unwalkableWeightRadius, unwalkableMask) != null)
                {
                    weight = unwalkableWeight;
                }

                grid[x, y] = new Node(walkable, worldPoint, x, y, weight);
            }
        }

        BlurWeights(blurSize);
    }

    void BlurWeights(int blurSize)
    {
        int kernelSize = blurSize * 2 + 1;
        int kernelExtent = blurSize;
        int[,] horizontalPass = new int[gridSizeX, gridSizeY];
        int[,] verticalPass = new int[gridSizeX, gridSizeY];


        for(int y = 0; y < gridSizeY; y++)
        {
            for(int x = -kernelExtent; x <= kernelExtent; x++)
            {
                int sampleX = Mathf.Clamp(x, 0, kernelExtent);
                horizontalPass[0, y] += grid[sampleX, y].weight; 
            }

            for (int x = 1; x < gridSizeX; x++)
            {
                int removeIndex = Mathf.Clamp(x - kernelExtent - 1, 0, gridSizeX);
                int addIndex = Mathf.Clamp(x + kernelExtent, 0, gridSizeX-1);

                horizontalPass[x, y] = horizontalPass[x - 1, y] - grid[removeIndex, y].weight + grid[addIndex, y].weight;

            }
        }

        for(int x = 0; x < gridSizeX; x++)
        {
            for (int y = -kernelExtent; y <= kernelExtent; y++)
            {
                int sampleY = Mathf.Clamp(y, 0, kernelExtent);
                verticalPass[x, 0] += horizontalPass[x, sampleY];
            }

            grid[x,0].weight = Mathf.RoundToInt((float)verticalPass[x, 0] / (kernelSize * kernelSize)); 

            for (int y = 1; y < gridSizeY; y++)
            {
                int removeIndex = Mathf.Clamp(y - kernelExtent - 1, 0, gridSizeY);
                int addIndex = Mathf.Clamp(y + kernelExtent, 0, gridSizeY - 1);

                verticalPass[x, y] = verticalPass[x, y - 1] - horizontalPass[x, removeIndex] + horizontalPass[x, addIndex];
                int blurredPenalty = Mathf.RoundToInt(((float)verticalPass[x, y]) / (kernelSize * kernelSize));
                grid[x,y].weight = blurredPenalty;
                if(blurredPenalty > maxWeight) maxWeight = blurredPenalty;
                if(blurredPenalty < minWeight) minWeight = blurredPenalty;
            }
        }
    }

    [System.Serializable]
    public class TerrainType
    {
        public LayerMask terrainMask;
        public int terrainWeight;
    }
}
