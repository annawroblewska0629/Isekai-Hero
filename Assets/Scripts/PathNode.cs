using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode 
{
    private Vector3Int gridPosition;
    private int gCost;
    private int hCost;
    private int fCost;
    private int exitCost = 1;
    private bool isWalable = true;
    private PathNode previousPathNode;

    public PathNode( Vector3Int gridPosition)
    {
        this.gridPosition = gridPosition;
    }

    public void SetExitCost(int exitCost)
    {
        this.exitCost = exitCost;
    }

    public int GetExitCost()
    {
        return exitCost;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost + exitCost;
    }

    public void SetHCost(int hCost)
    {
        this.hCost = hCost;
    }

    public void SetGCost(int gCost)
    {
        this.gCost = gCost;
    }

    public int GetFCost()
    {
        return fCost;
    }
    public int GetHCost()
    {
        return hCost;
    }
    public int GetGCost()
    {
        return gCost;
    }

    public Vector3Int GetGridPosition()
    {
        return gridPosition;
    }

    public bool IsWalkable()
    {
        return isWalable;
    }

    public void SetIsWalkable(bool isWalkable)
    {
        this.isWalable = isWalkable;
    }

    public void SetPreviousPathNode(PathNode pathNode)
    {
        previousPathNode = pathNode;
    }

    public PathNode GetPreviousPathNode()
    {
        return previousPathNode;
    }

    public void ResetPreviousPathNode()
    {
        previousPathNode = null;
    }

    public void ResetPathNode()
    {
        gCost = int.MaxValue;
        hCost = 0;
        fCost = int.MaxValue;
        previousPathNode = null;
    }
}
