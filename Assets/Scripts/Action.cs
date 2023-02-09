using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Action : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
      
    }

    public abstract string GetActionName();

    public abstract int GetActionPointsCost();

    public abstract ActionGridVisual.ColorType GetActionCastRangeColorType();

    public abstract List<Vector3Int> ActionCastRangePositionList();

    public abstract List<Vector3Int> ActionEffectRangePositionList(Vector3Int gridPosition);

    public virtual bool isValidActionCastPosition(Vector3Int actionCastPosition)
    {
        List<Vector3Int> actionCastRangePositionList = ActionCastRangePositionList();
        return actionCastRangePositionList.Contains(actionCastPosition);

    }

    public abstract void TakeAction(Vector3Int actionCastPosition);

}
