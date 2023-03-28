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


    public List<Vector3Int> FindPath(Vector3Int startGridPosition, Vector3Int endGridPosition)
    {
        Dictionary<Vector3Int, PathNode> pathNodeDictionary = LevelGrid.Instance.GetPathNodeDictionary();
        // pobieranie pocz¹tkowej kolekcji PathNode
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        PathNode startNode = pathNodeDictionary[startGridPosition];
        // przypisane wartoœci do pocz¹tkowego PathNode
        PathNode endNode = pathNodeDictionary[endGridPosition];
        // przypisane wartoœci do koñcowego PathNode
        openList.Add(startNode);

        RestartPathNodeValueInDictionary(pathNodeDictionary);
        // resetowanie wartoœæi PathNode 

        SetValueForStartNode(startNode, startGridPosition, endGridPosition);
        // Obliczanie Wartoœci startNode

        while (openList.Count > 0)
            // sprawdzanie czy openList nie jest pusta
        {
            PathNode currentNode = GetLowestFCostPathNode(openList);
            // pobieranie PathNoda o najni¿szej wartoœæi F

            if (currentNode == endNode)
                // sprawdzanie czy currentNode jest koñcowym PathNode'em
            {
                return CalculatePath(endNode);
                // zwracanie œcie¿ki
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            ChecnkingNeighbourPathNodes(currentNode, endGridPosition, closedList, openList);
            //Przeszukiwanie s¹siadów
        }
        
        return null;
        // brak œcie¿ki
    }

    void ChecnkingNeighbourPathNodes(PathNode currentNode, Vector3Int endGridPosition, List<PathNode> closedList, List<PathNode> openList)
    {
        foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
        // przeszukiwanie s¹siadów, w celu znalezienia mniejszej wartoœæi
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

            int tentativeGCost = CalculateTentativeGCost(currentNode);
            // obliczanie kosztu G dla currentNode

            if (tentativeGCost < neighbourNode.GetGCost())
            // sprawdzanie czy koszt G currentNode jest mniejszy od kosztu  s¹siada
            {
                //ustawianie wartoœæi dla neighbourNode
                SetValueForNeighbourNode(currentNode, neighbourNode, endGridPosition, tentativeGCost);

                if (!openList.Contains(neighbourNode))
                {
                    openList.Add(neighbourNode);
                }
            }
        }
    }

    private void RestartPathNodeValueInDictionary(Dictionary<Vector3Int, PathNode> pathNodeDictionary)
    {
        foreach (PathNode pathNode in pathNodeDictionary.Values)
        {
            // Resetowanie wsyztskich wartoœæi PathNode zanjduj¹cych siê w kolekcji
            pathNode.ResetPathNode();
        }
    }

    private void SetValueForStartNode( PathNode startNode, Vector3Int startGridPosition, Vector3Int endGridPosition)
    {
        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));
        startNode.CalculateFCost();
    }
    private void SetValueForNeighbourNode(PathNode currentNode, PathNode neighbourNode, Vector3Int endGridPosition, int tentativeGCost)
    {
        neighbourNode.SetPreviousPathNode(currentNode);
        neighbourNode.SetGCost(tentativeGCost);
        neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endGridPosition));
        neighbourNode.CalculateFCost();
    }

    private int CalculateTentativeGCost(PathNode currentNode)
    {
        return currentNode.GetGCost() + currentNode.GetExitCost();
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
