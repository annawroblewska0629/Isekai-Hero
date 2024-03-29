using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeAction : Action
{
    [SerializeField] Animator playerAnimator;
    [SerializeField] string actionName = "Freeze";
    [SerializeField] int actionCost = 2;

    [Header("ActionCastRange")]
    [SerializeField] int maxFrezzeCastRangeX;
    [SerializeField] int maxFreezeCastRangeY;
    [SerializeField] ActionGridVisual.ColorType actionColorType;

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
        List<Vector3Int> frezzeCastRangeGridPositionList = new List<Vector3Int>();
        Vector3Int playerPosition = LevelGrid.Instance.WorldPositionToGridPosition(transform.position);

        for (int x = -maxFrezzeCastRangeX; x <= maxFrezzeCastRangeX; x ++)
        {
            for (int y = -maxFreezeCastRangeY; y <= maxFreezeCastRangeY; y ++)
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

                frezzeCastRangeGridPositionList.Add(testGridPosition);
            }
        }
        return frezzeCastRangeGridPositionList;
      
    }
    public override ActionGridVisual.ColorType GetActionCastRangeColorType()
    {
        return actionColorType;
    }

    public override void TakeAction(Vector3Int actionCastPosition)
    {
        foreach (Vector3Int gridPosition in ActionEffectRangePositionList(actionCastPosition))
        {
            if (LevelGrid.Instance.IsPositionBlockedByEnemy(gridPosition))
            {
                Enemy enemy = LevelGrid.Instance.GetEnemyAtPosition(gridPosition);
                enemy.SetIsFreezed();
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
        List<Vector3Int> frezzeEffectGridPositionList = new List<Vector3Int>();
        if (isValidActionCastPosition(gridPosition))
        {
            frezzeEffectGridPositionList.Add(gridPosition);
        }
        return frezzeEffectGridPositionList;
    }
}
