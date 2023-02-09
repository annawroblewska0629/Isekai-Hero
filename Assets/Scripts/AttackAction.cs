using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : Action
{

    [Header("ActionCastRange")]
    [SerializeField] int maxAttackCastRangeX;
    [SerializeField] int maxAttackCastRangeY;
    [SerializeField] int maxAttackEffectRangeY;
    [SerializeField]ActionGridVisual.ColorType actionColorType; 
    
    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public override string GetActionName()
    {
        return "Attack";
    }

    public override List<Vector3Int> ActionCastRangePositionList()
    {
        List<Vector3Int> attackCastRangeGridPositionList = new List<Vector3Int>();
        Vector3Int playerPosition = LevelGrid.Instance.WorldPositionToGridPosition(transform.position);

        for (int x = -maxAttackCastRangeX; x <= maxAttackCastRangeX; x++)
        {
            for (int y = -maxAttackCastRangeY; y <= maxAttackCastRangeY; y++)
            {
                Vector3Int offsetPosition = new Vector3Int(x, y, 0);
                Vector3Int testGridPosition = playerPosition + offsetPosition;
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                if (playerPosition == testGridPosition)
                {
                    continue;
                }

                attackCastRangeGridPositionList.Add(testGridPosition);
            }
        }
        return attackCastRangeGridPositionList;
    }

    public override ActionGridVisual.ColorType GetActionCastRangeColorType()
    {
        return actionColorType;
    }


    public override void TakeAction(Vector3Int actionCastPosition)
    {
        foreach(Vector3Int gridPosition in ActionEffectRangePositionList(actionCastPosition))
        {
            if (LevelGrid.Instance.IsPositionBlockedByEnemy(gridPosition))
            {
                Enemy enemy = LevelGrid.Instance.GetEnemyAtPosition(gridPosition);
                enemy.Damage(1);
            }
        }
        
    }

    public override int GetActionPointsCost()
    {
        return 5;
    }

    public override List<Vector3Int> ActionEffectRangePositionList(Vector3Int gridPosition)
    {
        List<Vector3Int> atackEffectGridPositionList = new List<Vector3Int>();
        if (isValidActionCastPosition(gridPosition))
        {
            for (int y = 0; y <= maxAttackEffectRangeY; y++)
            {

                Vector3Int offsetGridPosition = new Vector3Int(0, y, 0);
                Vector3Int testGridPosition = gridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsGroundAtGridPosition(testGridPosition))
                {
                    continue;
                }
                atackEffectGridPositionList.Add(testGridPosition);

            }
        }
        return atackEffectGridPositionList;
    }
}


       


