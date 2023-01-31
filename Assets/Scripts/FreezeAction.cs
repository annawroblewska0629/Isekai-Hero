using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeAction : Action
{
    private int cellSize = 2;

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
        return "Freeze";
    }

    public override List<Vector3> ActionCastRangePositionList(int maxFreezeCastRangeX, int maxFreezeCastRangeY)
    {
        List<Vector3> freezeCastRangePositionList = new List<Vector3>();
        Vector3 playerPosition = transform.position;

        for (int x = -maxFreezeCastRangeX; x <= maxFreezeCastRangeX; x = x + cellSize)
        {
            for (int y = -maxFreezeCastRangeY; y <= maxFreezeCastRangeY; y = y + cellSize)
            {
                Vector3 offsetPosition = new Vector3(x, y, 0);
                Vector3 testGridPosition = playerPosition + offsetPosition;
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                if (playerPosition == testGridPosition)
                {
                    continue;
                }

                freezeCastRangePositionList.Add(testGridPosition);
            }
        }

        return freezeCastRangePositionList;
    }
    public override List<Vector3> GetAcionCastRangePositionList()
    {
        return ActionCastRangePositionList(maxFrezzeCastRangeX, maxFreezeCastRangeY);
    }
    public override ActionGridVisual.ColorType GetActionCastRangeColorType()
    {
        return actionColorType;
    }
}
