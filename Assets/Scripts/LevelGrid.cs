using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }
    [SerializeField] List<Transform> enemiesWorldPositions = new List<Transform>();
    List<Vector3Int> enemiesGridPositionsList = new List<Vector3Int>();
    [SerializeField] Grid grid;

    [Header("Tilemap")]
    [SerializeField] Tilemap groundTilemap;
    [SerializeField] Tilemap obstacleTilemap;
    [SerializeField] TileBase obstacleTile;

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
        foreach (Transform enemy in enemiesWorldPositions)
        {
            Vector3 enemyWroldPosition = enemy.transform.position;
            Vector3Int enemyGridPosition = grid.WorldToCell(enemyWroldPosition);
            enemiesGridPositionsList.Add(enemyGridPosition);
        }

    }

    public void AddPosition(Vector3 worldPosition)
    {
        Vector3Int gridPosition = grid.WorldToCell(worldPosition);
        enemiesGridPositionsList.Add(gridPosition);
    }

    public void RemovePositon(Vector3 worldPosition)
    {
        Vector3Int gridPosition = grid.WorldToCell(worldPosition);
        enemiesGridPositionsList.Remove(gridPosition);
    }

    public void DeleteEnemy(Transform enemy)
    {
        enemiesWorldPositions.Remove(enemy);
    }

    public bool IsPositionBlockedByEnemy(Vector3 worldPosition)
    {
        Vector3Int gridPosition = grid.WorldToCell(worldPosition);
        if (enemiesGridPositionsList.Contains(gridPosition))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsValidGridPosition(Vector3 worldPosition)
    {
        // fukncja konwertuje pozycje na pozycje kafelkowa
        Vector3Int gridPosition = groundTilemap.WorldToCell(worldPosition);
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

    public bool IsValidGridPositionToPush(Vector3 worldPosition, Vector3 behindWorldPosition)
    {
        // fukncja konwertuje pozycje na pozycje kafelkowa
        Vector3Int gridPosition = obstacleTilemap.WorldToCell(worldPosition);
        Vector3Int behindGridPosition = obstacleTilemap.WorldToCell(behindWorldPosition);
        
        // Gdy w kierunku w ktorym chce poruszyc sie gracz napotka przeszkode i nie znjaduje sie za nia inny obiekt oraz mapa nie konczy sie za przeszkoda fukncja zwraca prawde
        if (obstacleTilemap.HasTile(gridPosition) && !obstacleTilemap.HasTile(behindGridPosition) && groundTilemap.HasTile(behindGridPosition))
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
        Vector3Int gridPosition = obstacleTilemap.WorldToCell(worldPosition);
        Vector3Int behindGridPosition = obstacleTilemap.WorldToCell(behindWorldPosition);

        obstacleTilemap.SetTile(behindGridPosition, obstacleTile);
        obstacleTilemap.SetTile(gridPosition, null);
    }
}
