using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ActionGridVisual : MonoBehaviour
{
 
    // public static AttackCastRangeVisual Instance { get; private set; }
    [SerializeField] Tilemap actionCastRangeTileMap;
    [SerializeField] Tilemap actionEffectRangeTileMap;
    [SerializeField] private List<ColorTypeTileBase> colorTypeTileBaseList;
    [SerializeField] TileBase actionEffectRangeTileBase;
    Vector3Int lastMouseGridPosition;
    private bool isActionActive = false;

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

    private void Update()
    {
        if (!isActionActive)
        {
            return;
        }
        if(MousePosition.GetMouseGridPosition() != lastMouseGridPosition)
        {

            lastMouseGridPosition = MousePosition.GetMouseGridPosition();
            actionEffectRangeTileMap.ClearAllTiles();
            Action activeAction = PlayerActionSystem.Instance.GetActiveAction();
            ShowActionEffectRangeVisual(activeAction.ActionEffectRangePositionList(lastMouseGridPosition));
     
        }
     
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

    private void ShowActionEffectRangeVisual(List<Vector3Int> actionEffectRangePositionList)
    {

        foreach (Vector3Int gridPosition in actionEffectRangePositionList)
        {

            actionEffectRangeTileMap.SetTile(gridPosition, actionEffectRangeTileBase);

        }

    }

    private void ShowActionCastRangeVisual(List<Vector3Int> actionCastRangePositionList, ColorType colorType)
    {
        
            foreach (Vector3Int gridPosition in actionCastRangePositionList)
            {

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
        ShowActionCastRangeVisual(activeAction.ActionCastRangePositionList(), activeAction.GetActionCastRangeColorType());
    }

    private void PlayerActionSystem_OnActiveActionChanged(object sender, EventArgs e)
    {
        UpdateActionCastRangeVisual();
        isActionActive = true;
    }
    private void PlayerActionSystem_OnActiveActionDeactivation(object sender, EventArgs e)
    {
        DeleteActionCastRangeVisual();
        isActionActive = false;
        actionEffectRangeTileMap.ClearAllTiles();
    }
}
