using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ActionGridVisual : MonoBehaviour
{
 
    // public static AttackCastRangeVisual Instance { get; private set; }
    [SerializeField] Tilemap actionCastRangeTileMap;
    [SerializeField] private List<ColorTypeTileBase> colorTypeTileBaseList;
   
    
    [Serializable]
    public struct ColorTypeTileBase
    {
        public ColorType colorType;
        public TileBase tileBase;
    }

    public enum ColorType
    {
        Blue,
        Red,
    }


    void Start()
    {
        /* if (Instance != null)
         {
             Debug.LogError("There is more than one AttackCastRangeVisual!" + transform + " - " + Instance);
             Destroy(gameObject);
             return;
         }
         Instance = this;
        */
        PlayerActionSystem.Instance.OnActiveActionChanged += PlayerActionSystem_OnActiveActionChanged;
        PlayerActionSystem.Instance.OnActiveActionDeactivation += PlayerActionSystem_OnActiveActionDeactivation;
    }

    private TileBase GetColorTypeTileBase(ColorType colorType)
    {
        foreach (ColorTypeTileBase colorTypeTileBase in colorTypeTileBaseList)
        {
            if (colorTypeTileBase.colorType == colorType)
            {
                return colorTypeTileBase.tileBase;
            }
        }

        Debug.LogError("Could not find colorTypeTileBase for colorType " + colorType);
        return null;
    }

    private void ShowActionCastRangeVisual(List<Vector3> actionCastRangePositionList, ColorType colorType)
    {
        
            foreach (Vector3 worldPosition in actionCastRangePositionList)
            {
                Vector3Int gridPosition = actionCastRangeTileMap.WorldToCell(worldPosition);

                actionCastRangeTileMap.SetTile(gridPosition, GetColorTypeTileBase(colorType));

            }
       
    
    }

    private void DeleteActionCastRangeVisual()
    {
        actionCastRangeTileMap.ClearAllTiles();
      
    }

    private void UpdateActionCastRangeVisual()
    {
        DeleteActionCastRangeVisual();
        Action activeAction = PlayerActionSystem.Instance.GetActiveAction();
       
        //actionCastRangePositionList = activeAction.ActionCastRangePositionList();
        ShowActionCastRangeVisual(activeAction.GetAcionCastRangePositionList(), activeAction.GetActionCastRangeColorType());
    }

    private void PlayerActionSystem_OnActiveActionChanged(object sender, EventArgs e)
    {
        UpdateActionCastRangeVisual();
        Debug.Log("o");
    }
    private void PlayerActionSystem_OnActiveActionDeactivation(object sender, EventArgs e)
    {
        DeleteActionCastRangeVisual();
    }
}
