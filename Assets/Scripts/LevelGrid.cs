using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }
    private Dictionary<Vector3Int, Enemy> enemiesGridPositionDictionary = new Dictionary<Vector3Int, Enemy>();
    [SerializeField] private Grid grid;
    
    [Header("Tilemap")]
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap obstacleTilemap;
    [SerializeField] private Tilemap sandTilemap;
    [SerializeField] private TileBase obstacleTile;

    [Header("Key")]
    [SerializeField] Transform key;

    [Header("SandCost")]
    [SerializeField] int sandCost;

    [Header("EnemyPoints")]
    [SerializeField] List<Transform> enemyTransformPointsList;
    List<Vector3Int> enemyGridPointsList = new List<Vector3Int>();

    BoundsInt boundsInt;
    Dictionary<Vector3Int, PathNode> pathNodeDictionary = new Dictionary<Vector3Int, PathNode>();
    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one EnemiesGridPositons!" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
        boundsInt = groundTilemap.cellBounds;
        CreatePathNodeDictionary();
        CheckIsWalkablePathNode();
        SetAdditionalCostPathNode();
        CreateGridPointsList();

    }

    private void CreateGridPointsList()
    {
        foreach(Transform transform in enemyTransformPointsList)
        {
            enemyGridPointsList.Add(WorldPositionToGridPosition(transform.position));
        }
    }

    private void CreatePathNodeDictionary()
    {
        for (int x = boundsInt.min.x; x < boundsInt.max.x; x++)
        {
            for (int y = boundsInt.min.y; y < boundsInt.max.y; y++)
            {
                Vector3Int gridPosition = new Vector3Int(x, y, 0);
                if (groundTilemap.HasTile(gridPosition))
                {
                    PathNode pathNode = new PathNode(gridPosition);
                    pathNodeDictionary.Add(gridPosition, pathNode);

                }
            }
        }
    }

    private void CheckIsWalkablePathNode()
    {
        foreach (Vector3Int key in pathNodeDictionary.Keys)
        {
            if (obstacleTilemap.HasTile(key))
            {
                pathNodeDictionary[key].SetIsWalkable(false);
            }
        }
    }

    private void SetAdditionalCostPathNode()
    {
        foreach (Vector3Int key in pathNodeDictionary.Keys)
        {
            if (sandTilemap.HasTile(key))
            {
                pathNodeDictionary[key].SetAdditionalCost(sandCost);
            }
        }
    }

    public void SetIsWalkablePathNode(Vector3 worldPosition, bool value)
    {
        pathNodeDictionary[WorldPositionToGridPosition(worldPosition)].SetIsWalkable(value);
    }

    public void UpdatePathNodeDictionary(Vector3 previousWorldPosition, Vector3 currentWorldPosition)
    {
        pathNodeDictionary[WorldPositionToGridPosition(previousWorldPosition)].SetIsWalkable(true);
        pathNodeDictionary[WorldPositionToGridPosition(currentWorldPosition)].SetIsWalkable(false);
    }
    public Dictionary<Vector3Int, PathNode> GetPathNodeDictionary()
    {
        return pathNodeDictionary;
    }

    public PathNode GetPathNode(Vector3Int gridPosition)
    {
        return pathNodeDictionary[gridPosition];
    }

    public Vector3 GetCellCenterWorld(Vector3Int gridPosition)
    {
        return grid.GetCellCenterWorld(gridPosition);
    }

    public Vector3Int WorldPositionToGridPosition(Vector3 worldPosition)
    {
        return grid.WorldToCell(worldPosition);
    }

    public void AddEnemyPosition(Vector3 enemyWorldPosition, Enemy enemy)
    {
        enemiesGridPositionDictionary.Add(WorldPositionToGridPosition(enemyWorldPosition), enemy);
    }

    public void RemoveEnemyPosition(Vector3 enemyWorldPosition)
    {
        enemiesGridPositionDictionary.Remove(WorldPositionToGridPosition(enemyWorldPosition));
    }

    public void EnemyChangingPosition(Vector3 previousWorldPosition, Vector3 newWorldPosition, Enemy enemy)
    {
        RemoveEnemyPosition(previousWorldPosition);
        AddEnemyPosition(newWorldPosition, enemy);
    }

    public bool IsPositionBlockedByEnemy(Vector3 worldPosition)
    {
        return enemiesGridPositionDictionary.ContainsKey(WorldPositionToGridPosition(worldPosition));
    }
    
    public bool IsPositionBlockedByEnemy(Vector3Int gridPosition)
    {
        return enemiesGridPositionDictionary.ContainsKey(gridPosition);
    }

    public Enemy GetEnemyAtPosition(Vector3Int gridPosition)
    {
        Enemy enemy = enemiesGridPositionDictionary[gridPosition];
        return enemy;
    }


    private bool IsGroundBlockedByKey(Vector3 worldPosition)
    {
        if(key != null)
        {
            if (key.transform.position != worldPosition)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    public bool IsValidGridPosition(Vector3 worldPosition)
    {
        // fukncja konwertuje pozycje na pozycje kafelkowa
        // Gdy mapa sie konczy lub gracz napotka przeszkode w kierunku ktorym chce sie poruszyc fukncja zwaraca falsz
        if (!groundTilemap.HasTile(WorldPositionToGridPosition(worldPosition)) || obstacleTilemap.HasTile(WorldPositionToGridPosition(worldPosition)))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public bool IsValidGridPosition(Vector3Int gridPosition)
    {
        // fukncja konwertuje pozycje na pozycje kafelkowa
        // Gdy mapa sie konczy lub gracz napotka przeszkode w kierunku ktorym chce sie poruszyc fukncja zwaraca falsz
        if (!groundTilemap.HasTile(gridPosition) || obstacleTilemap.HasTile(gridPosition))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool IsGroundAtGridPosition(Vector3Int gridPosition)
    {
        return groundTilemap.HasTile(gridPosition);
    }

    public bool IsValidGridPositionToPush(Vector3 worldPosition, Vector3 behindWorldPosition)
    {
        
        // Gdy w kierunku w ktorym chce poruszyc sie gracz napotka przeszkode i nie znjaduje sie za nia inny obiekt oraz mapa nie konczy sie za przeszkoda fukncja zwraca prawde
        if (obstacleTilemap.HasTile(WorldPositionToGridPosition(worldPosition)) 
            && !obstacleTilemap.HasTile(WorldPositionToGridPosition(behindWorldPosition)) 
            && groundTilemap.HasTile(WorldPositionToGridPosition(behindWorldPosition))
            && !IsGroundBlockedByKey(behindWorldPosition)
            && !enemyGridPointsList.Contains(WorldPositionToGridPosition(behindWorldPosition)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ChangeObstacleGridPosition(Vector3 worldPosition, Vector3 behindWorldPosition)
    {

        obstacleTilemap.SetTile(WorldPositionToGridPosition(behindWorldPosition), obstacleTile);
        obstacleTilemap.SetTile(WorldPositionToGridPosition(worldPosition), null);
    }

    public bool isSandAtGridPosition(Vector3 worldPosition)
    {
        return sandTilemap.HasTile(WorldPositionToGridPosition(worldPosition));
    }
}
