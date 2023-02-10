using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    HealthSystem healthSystem;
    public static event EventHandler OnEnemySpawned;
    public static event EventHandler OnEnemyDead;
    List<Vector3> targetsWorldPositionList = new List<Vector3>();
    [SerializeField] Vector3 firstTargetWorldPosition;
    [SerializeField] Vector3 secondTargetWorldPosition;
    Vector3 targetWorldPosition;
    private int currentPositionIndex = 1;
    [SerializeField] Player player;
    private bool isFreezed = false;
    private int turnBeingFreezed = 0;
    private int turnBeingOnSand = 0;
    // Start is called before the first frame update

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
    }
    void Start()
    {
        LevelGrid.Instance.AddEnemyPosition(transform.position, this);
        LevelGrid.Instance.SetIsWalkablePathNode(transform.position, false);
        healthSystem.OnDead += HealthSystem_OnDead;
        targetWorldPosition = firstTargetWorldPosition;
        ListOfPositions(targetWorldPosition);
        OnEnemySpawned?.Invoke(this, EventArgs.Empty);
        //  Debug.Log(LevelGrid.Instance.WorldPositionToGridPosition(transform.position) + " pozycja enemy ");
        //  Debug.Log(LevelGrid.Instance.WorldPositionToGridPosition(targetWorldPosition)+ "target pozycja");
    }


    public void Damage(int damage)
    {
        healthSystem.Damage(damage);
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveEnemyPosition(transform.position);
        LevelGrid.Instance.SetIsWalkablePathNode(transform.position, true);
        Destroy(gameObject);
        OnEnemyDead?.Invoke(this, EventArgs.Empty);
    }

   private void Move()
    {
        Vector3 currentTargetWorldPosition = targetsWorldPositionList[currentPositionIndex];
     
        if (currentPositionIndex < targetsWorldPositionList.Count)
        {
                LevelGrid.Instance.UpdatePathNodeDictionary(transform.position, currentTargetWorldPosition);    
                 LevelGrid.Instance.EnemyChangingPosition(transform.position, currentTargetWorldPosition, this);
                transform.position = currentTargetWorldPosition;     
                 ++currentPositionIndex;
                if (EnemyCanAttack())
                {
                    Attack();
                }

        }

        
    }

    private void ListOfPositions(Vector3 targetWorldPosition)
    {
        List<Vector3Int> pathGridPositionList = Pathfinding.Instance.FindPath(LevelGrid.Instance.WorldPositionToGridPosition(transform.position), LevelGrid.Instance.WorldPositionToGridPosition(targetWorldPosition));
        

        foreach (Vector3Int pathGridPosition in pathGridPositionList)
        {
            targetsWorldPositionList.Add(LevelGrid.Instance.GetCellCenterWorld(pathGridPosition));
        }

    }

    private void ResetPath() 
    {
        targetsWorldPositionList.Clear();
        currentPositionIndex = 1;
        ListOfPositions(targetWorldPosition);
    }

    private void ResetTargetPosition()
    {
        if (transform.position == firstTargetWorldPosition)
        {
            targetWorldPosition = secondTargetWorldPosition;
            ResetPath();
        }
        if (transform.position == secondTargetWorldPosition)
        {
            targetWorldPosition = firstTargetWorldPosition;

            ResetPath();
        }
    }

    private bool EnemyCanAttack()
    {
        //top
        if(player.isPositionBloeckedByPlayer(new Vector3(transform.position.x, transform.position.y + 2, 0)))
        {
            return true;
        }
        //bottom
        if (player.isPositionBloeckedByPlayer(new Vector3(transform.position.x, transform.position.y - 2, 0)))
        {
            return true;
        }
        //right
        if (player.isPositionBloeckedByPlayer(new Vector3(transform.position.x + 2, transform.position.y, 0)))
        {
            return true;
        }
        //left
        if (player.isPositionBloeckedByPlayer(new Vector3(transform.position.x - 2, transform.position.y, 0)))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    private void Attack()
    {
        player.Damage(1);
    }


    public void EnemyTakeAction()
    {

        if (!isFreezed)
        {
            if (!LevelGrid.Instance.isSandAtGridPosition(transform.position) || turnBeingOnSand == 1)
            {
                turnBeingOnSand = 0;
                if (EnemyCanAttack())
                {
                    Attack();
                }
                else if (!LevelGrid.Instance.IsPositionBlockedByEnemy(targetsWorldPositionList[currentPositionIndex])
                    && LevelGrid.Instance.IsValidGridPosition(targetsWorldPositionList[currentPositionIndex])
                   && !player.isPositionBloeckedByPlayer(targetsWorldPositionList[currentPositionIndex]))
                {

                    Move();

                }
                else
                {
                    ResetPath();
                    Move();
                }
            }
            else
            {
                turnBeingOnSand++;
            }
        }
        else
        {
            EnemyBeingFreezed();
        }
     
            ResetTargetPosition();

    }

    public void SetIsFreezed()
    {
        isFreezed = true;
    }

    private void EnemyBeingFreezed()
    {
        turnBeingFreezed++;
        if (turnBeingFreezed == 2)
        {
            isFreezed = false;
            turnBeingFreezed = 0;
        }
    }

}
