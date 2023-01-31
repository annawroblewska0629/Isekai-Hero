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

    public abstract ActionGridVisual.ColorType GetActionCastRangeColorType();

    public abstract List<Vector3> GetAcionCastRangePositionList();

    public abstract List<Vector3> ActionCastRangePositionList(int maxAttackCastRangeX, int maxAttackCastRangeY);

}
