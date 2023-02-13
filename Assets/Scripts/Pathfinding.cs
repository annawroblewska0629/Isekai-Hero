using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance { get; private set; }
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one Pathfinding! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

    }


    public List<Vector3Int> FindPath(Vector3Int startGridPosition, Vector3Int endGridPosition/* out int pathLength*/)
    {
        Dictionary<Vector3Int, PathNode> pathNodeDictionary = LevelGrid.Instance.GetPathNodeDictionary();
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        PathNode startNode = pathNodeDictionary[startGridPosition];
        PathNode endNode = pathNodeDictionary[endGridPosition];
        openList.Add(startNode);

        foreach(PathNode pathNode in pathNodeDictionary.Values)
        {
            /*pathNode.SetGCost(int.MaxValue);
            pathNode.SetHCost(0);
            pathNode.CalculateFCost();
            pathNode.ResetPreviousPathNode();
            */
            pathNode.ResetPathNode();
        }
        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostPathNode(openList);

            if (currentNode == endNode)
            {
                // Reached final node
                // pathLength = endNode.GetFCost();
                Debug.Log("chyba dziala");
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode))
                {
                    continue;
                }

                if (!neighbourNode.IsWalkable())
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost =
                    currentNode.GetGCost() + currentNode.GetExitCost();

                if (tentativeGCost < neighbourNode.GetGCost())
                {
                    neighbourNode.SetPreviousPathNode(currentNode);
                    neighbourNode.SetGCost(tentativeGCost);
                    neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endGridPosition));
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        Debug.Log("dodaje sasiada");
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        // No path found
        // pathLength = 0;
        Debug.Log("problem");
        return null;

    }

    private List<PathNode> GetNeighbourList(PathNode currentPathNode)
    {
        Dictionary<Vector3Int, PathNode> pathNodeList = LevelGrid.Instance.GetPathNodeDictionary();
        List<PathNode> pathNodeNeighbourList = new List<PathNode>();
        Vector3Int testGridPosition = currentPathNode.GetGridPosition();
        Vector3Int originalGridPosition = testGridPosition;

        //top
        testGridPosition = new Vector3Int(originalGridPosition.x,originalGridPosition.y + 1, 0);
        if (pathNodeList.ContainsKey(testGridPosition))
        {
            pathNodeNeighbourList.Add(pathNodeList[testGridPosition]);
        }
        //bottom
        testGridPosition = new Vector3Int(originalGridPosition.x, originalGridPosition.y - 1, 0);
        if (pathNodeList.ContainsKey(testGridPosition))
        {
            pathNodeNeighbourList.Add(pathNodeList[testGridPosition]);
        }
        //right
        testGridPosition = new Vector3Int(originalGridPosition.x + 1, originalGridPosition.y, 0);
        if (pathNodeList.ContainsKey(testGridPosition))
        {
            pathNodeNeighbourList.Add(pathNodeList[testGridPosition]);
        }
        //left
        testGridPosition = new Vector3Int(originalGridPosition.x - 1, originalGridPosition.y, 0);
        if (pathNodeList.ContainsKey(testGridPosition))
        {
            pathNodeNeighbourList.Add(pathNodeList[testGridPosition]);
        }

        return pathNodeNeighbourList;
    }

    public int CalculateDistance(Vector3Int startGridPosition, Vector3Int neighbourGridPosition)
    {
        return Mathf.Abs(startGridPosition.x - neighbourGridPosition.x) + Mathf.Abs(startGridPosition.y - neighbourGridPosition.y);
    }

    private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostPathNode = pathNodeList[0];
        for (int i = 0; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].GetFCost() < lowestFCostPathNode.GetFCost())
            {
                lowestFCostPathNode = pathNodeList[i];
            }
        }
        return lowestFCostPathNode;
    }

    private List<Vector3Int> CalculatePath(PathNode endNode)
    {
        List<PathNode> pathNodeList = new List<PathNode>();
        pathNodeList.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.GetPreviousPathNode() != null)
        {
            pathNodeList.Add(currentNode.GetPreviousPathNode());
            currentNode = currentNode.GetPreviousPathNode();
        }

        pathNodeList.Reverse();

        List<Vector3Int> gridPositionList = new List<Vector3Int>();
        foreach (PathNode pathNode in pathNodeList)
        { 
            gridPositionList.Add(pathNode.GetGridPosition());
        }

        return gridPositionList;
    }

    public bool HasPath(Vector3Int startGridPosition, Vector3Int endGridPosition)
    {
        return FindPath(startGridPosition, endGridPosition) != null;
    }

    /*public int GetPathLength(Vector3Int startGridPosition, Vector3Int endGridPosition)
    {
        FindPath(startGridPosition, endGridPosition, out int pathLength);
        return pathLength;
    }
    */
}
