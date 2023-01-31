using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : Action
{
    private int cellSize = 2;

    [Header("ActionCastRange")]
    [SerializeField] int maxAttackCastRangeX;
    [SerializeField] int maxAttackCastRangeY;
    [SerializeField]ActionGridVisual.ColorType actionColorType; 
    //PlayerMovement playerMovement;
    //ActionCastRangeVisual actionCastRangeVisual;
    // Start is called before the first frame update
    void Start()
    {
        //playerMovement = FindObjectOfType<PlayerMovement>();
        //actionCastRangeVisual = FindObjectOfType<ActionCastRangeVisual>();
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public override string GetActionName()
    {
        return "Attack";
    }

    public override List<Vector3> ActionCastRangePositionList(int maxAttackCastRangeX, int maxAttackCastRangeY)
    {
        List<Vector3> attackCastRangePositionList = new List<Vector3>();
        Vector3 playerPosition = transform.position;

        for (int x = -maxAttackCastRangeX; x <= maxAttackCastRangeX; x = x + cellSize)
        {
            for (int y = -maxAttackCastRangeY; y <= maxAttackCastRangeY; y = y + cellSize)
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

                attackCastRangePositionList.Add(testGridPosition);
            }
        }

        return attackCastRangePositionList;
    }

    public override ActionGridVisual.ColorType GetActionCastRangeColorType()
    {
        return actionColorType;
    }

    public override List<Vector3> GetAcionCastRangePositionList()
    {
        return ActionCastRangePositionList(maxAttackCastRangeX, maxAttackCastRangeY);
    }

    /*  public void OnAndOffAttackCastRangeVisual()
      {
          if (!isAttackCastRangeActive)
          {
              isAttackCastRangeActive = true;
              playerMovement.EnablePlayerMovment();
              //AttackCastRangeVisual.Instance.DeleteAttackCastRangeVisual();
              actionCastRangeVisual.DeleteActionCastRangeVisual();
              Debug.Log("raz");
          }
          else if (isAttackCastRangeActive)
          {
              //AttackCastRangeVisual.Instance.ShowAttackCastRange(AttackCastRangePositionList(maxAttackCastRangeX, maxAttackCastRangeY));
              actionCastRangeVisual.ShowActionCastRange(ActionCastRangePositionList(maxAttackCastRangeX, maxAttackCastRangeY),ActionCastRangeVisual.Color.Red);
              playerMovement.DisablePlayerMovment();
              isAttackCastRangeActive = false;
              Debug.Log("dwa");
          }
         */

}


       


