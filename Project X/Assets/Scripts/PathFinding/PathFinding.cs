using System.Collections.Generic;
using UnityEngine;

public class PathFinder
{
    public static Path AStarPathFinding(Grid grid, int startIndex, int targetIndex)
    {
        PathNode startNode = new PathNode(startIndex, 0, HeuristicCostEstimate(startIndex, targetIndex, grid));
        // Implement A* pathfinding algorithm here
        // This is a placeholder implementation and should be replaced with actual logic
        List<PathNode> openList = new List<PathNode>() { startNode };
        List<PathNode> closeList = new List<PathNode>();

        while (openList.Count > 0)
        {
            PathNode currentNode = openList[0];
            Debug.Log($"[PathFinding] Current Node: {currentNode.Index}");
            for (int i = 1; i < openList.Count; i++)
            {
                // Find node with lowest F cost
                if (openList[i].FCost < currentNode.FCost ||
                    (openList[i].FCost == currentNode.FCost && openList[i].HCost < currentNode.HCost))
                {
                    currentNode = openList[i];
                }
            }

            if (currentNode.Index == targetIndex)
            {
                Debug.LogWarning("[PathFinding] Path found!");
                // Path found, reconstruct path
                List<int> path = new List<int>();
                PathNode pathNode = currentNode;
                while (pathNode != null)
                {
                    path.Add(pathNode.Index);
                    pathNode = pathNode.Parent;
                }
                path.Reverse();
                return new Path(path.ToArray(), grid);
            }

            // Explore neighbors
            foreach (int neighborIndex in grid.Connections[currentNode.Index])
            {
                // if neighbor is in closed list, skip it
                if (closeList.Exists(n => n.Index == neighborIndex))
                {
                    Debug.Log($"[PathFinding] Neighbor {neighborIndex} in closed list, skipping.");
                    continue;
                }

                // Node Base cost
                float tentativeGCost = currentNode.GCost + HeuristicCostEstimate(currentNode.Index, neighborIndex, grid);
                PathNode neighborNode = openList.Find(n => n.Index == neighborIndex);
                if (neighborNode == null)
                {
                    // new Neighbor
                    neighborNode = new PathNode(neighborIndex, tentativeGCost,
                        HeuristicCostEstimate(neighborIndex, targetIndex, grid), currentNode);
                    openList.Add(neighborNode);
                }
                else if (tentativeGCost < neighborNode.GCost)
                {
                    // Better path found
                    neighborNode.GCost = tentativeGCost;
                    neighborNode.Parent = currentNode;
                }

                if (!openList.Contains(neighborNode))
                {
                    openList.Add(neighborNode);
                }
            }
            
            openList.Remove(currentNode);
            closeList.Add(currentNode);
        }
        
        Debug.LogWarning("[PathFinding] No path found.");
        return null; // No path found
    }

    // Distance between two nodes (Euclidean distance)
    // Used to estimate cost from current node to target and target to current node
    static public float HeuristicCostEstimate(int aIndex, int bIndex, Grid grid)
    {
        Vector2 aCoord = grid.IndexToGridCoord(aIndex);
        Vector2 bCoord = grid.IndexToGridCoord(bIndex);
        return Vector2.Distance(aCoord, bCoord);
    }

    static public float TotalCost(int aIndex, int bIndex, Grid grid)
    {
        float gCost = HeuristicCostEstimate(aIndex, bIndex, grid); // Placeholder for actual gCost calculation
        float hCost = HeuristicCostEstimate(bIndex, aIndex, grid); // Placeholder for actual gCost calculation
        return gCost + hCost;
    }
}

public class PathNode
{
    public int Index;
    public float GCost; // Cost from start node to this node
    public float HCost; // Heuristic cost from this node to target node
    public float FCost => GCost + HCost; // Total cost
    public PathNode Parent;

    public PathNode(int index, float gCost, float hCost, PathNode parent = null)
    {
        Index = index;
        GCost = gCost;
        HCost = hCost;
        Parent = parent;
    }
}