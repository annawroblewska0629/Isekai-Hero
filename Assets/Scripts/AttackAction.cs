using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : Action
{
    [SerializeField] Animator playerAnimator;
    [SerializeField] string actionName = "Attack";
    [SerializeField] int actionCost = 5;
    [SerializeField] int damage = 1;

    [Header("ActionCastRange")]
    [SerializeField] int maxAttackCastRangeX;
    [SerializeField] int maxAttackCastRangeY;
    [SerializeField] int maxAttackEffectRangeX;
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
        return actionName;
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
                enemy.Damage(damage);
                
            }
        }
        playerAnimator.SetTrigger("isCastingSpell");
    }

    public override int GetActionPointsCost()
    {
        return actionCost;
    }

    public override List<Vector3Int> ActionEffectRangePositionList(Vector3Int gridPosition)
    {
        List<Vector3Int> atackEffectGridPositionList = new List<Vector3Int>();
        if (isValidActionCastPosition(gridPosition))
        {
            for (int x = 0; x <= maxAttackEffectRangeX; x++)
            {

                Vector3Int offsetGridPosition = new Vector3Int(-x, 0, 0);
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


       


