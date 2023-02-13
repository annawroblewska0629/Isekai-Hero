using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Animator enemyAnimator;
    HealthSystem healthSystem;
    public static event EventHandler OnEnemySpawned;
    public static event EventHandler OnEnemyDead;
    public event EventHandler OnEnemyFreezed;
    public event EventHandler OnEnemyUnfreezed;
    List<Vector3Int> pathGridPositionList = new List<Vector3Int>();
    [SerializeField] Transform firstTargetWorldPosition;
    [SerializeField] Transform secondTargetWorldPosition;
    Vector3 targetWorldPosition;
    private int currentPositionIndex = 1;
    [SerializeField] Player player;
    private bool isFreezed = false;
    private int turnBeingFreezed = 0;
    private int turnBeingOnSand = 0;
    private bool isCahsing = false;
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
        targetWorldPosition = firstTargetWorldPosition.position;
        ListOfPositions(targetWorldPosition);
        OnEnemySpawned?.Invoke(this, EventArgs.Empty);
        //  Debug.Log(LevelGrid.Instance.WorldPositionToGridPosition(transform.position) + " pozycja enemy ");
        //  Debug.Log(LevelGrid.Instance.WorldPositionToGridPosition(targetWorldPosition)+ "target pozycja");
    }


    public void Damage(int damage)
    {
        healthSystem.Damage(damage);
        enemyAnimator.SetTrigger("isHurt");
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveEnemyPosition(transform.position);
        LevelGrid.Instance.SetIsWalkablePathNode(transform.position, true);
        enemyAnimator.SetTrigger("isDead");
        OnEnemyDead?.Invoke(this, EventArgs.Empty);
    }

   private void Move()
    {
        Vector3 currentTargetWorldPosition = LevelGrid.Instance.GetCellCenterWorld(pathGridPositionList[currentPositionIndex]);
     
        if (currentPositionIndex < pathGridPositionList.Count)
        {
                LevelGrid.Instance.UpdatePathNodeDictionary(transform.position, currentTargetWorldPosition);    
                LevelGrid.Instance.EnemyChangingPosition(transform.position, currentTargetWorldPosition, this);
                transform.position = currentTargetWorldPosition;     
                 ++currentPositionIndex;
                if (EnemyCanAttack() && !LevelGrid.Instance.isSandAtGridPosition(transform.position))
                {
                    Attack();
                }

        }

        
    }

    private void ListOfPositions(Vector3 targetWorldPosition)
    {
        if (Pathfinding.Instance.HasPath(LevelGrid.Instance.WorldPositionToGridPosition(transform.position), LevelGrid.Instance.WorldPositionToGridPosition(targetWorldPosition)))
        {
            
            pathGridPositionList = Pathfinding.Instance.FindPath(LevelGrid.Instance.WorldPositionToGridPosition(transform.position), LevelGrid.Instance.WorldPositionToGridPosition(targetWorldPosition));

        }
  
    }

    private void ResetPath() 
    {
        pathGridPositionList.Clear();
        currentPositionIndex = 1;
        ListOfPositions(targetWorldPosition);
    }

    private void ResetTargetPosition()
    {
        if (LevelGrid.Instance.WorldPositionToGridPosition(transform.position) == LevelGrid.Instance.WorldPositionToGridPosition(firstTargetWorldPosition.position))
        {
            targetWorldPosition = secondTargetWorldPosition.position;
            ResetPath();
        }
        if (LevelGrid.Instance.WorldPositionToGridPosition(transform.position) == LevelGrid.Instance.WorldPositionToGridPosition(secondTargetWorldPosition.position))
        {
            targetWorldPosition = firstTargetWorldPosition.position;

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
        enemyAnimator.SetTrigger("isAttacking");
        player.Damage(1);
    }


    public void EnemyTakeAction()
    {

        if (!isFreezed)
        {
            if (EnemyCanAttack() && !LevelGrid.Instance.isSandAtGridPosition(transform.position))
            {
                Attack();
            }
            else if (isCahsing)
            {
                Chase();
            }
            else if(!Pathfinding.Instance.HasPath(LevelGrid.Instance.WorldPositionToGridPosition(transform.position), LevelGrid.Instance.WorldPositionToGridPosition(targetWorldPosition)))
            {
                StartChasing();
                
            }
            else if (!LevelGrid.Instance.isSandAtGridPosition(transform.position) || turnBeingOnSand == 1)
           
            {
                turnBeingOnSand = 0;

                if (!LevelGrid.Instance.IsPositionBlockedByEnemy(pathGridPositionList[currentPositionIndex])
                    && LevelGrid.Instance.IsValidGridPosition(pathGridPositionList[currentPositionIndex])
                   && !player.isPositionBloeckedByPlayer(pathGridPositionList[currentPositionIndex]))
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
        OnEnemyFreezed?.Invoke(this, EventArgs.Empty);
    }

    private void EnemyBeingFreezed()
    {
        turnBeingFreezed++;
        if (turnBeingFreezed == 2)
        {
            isFreezed = false;
            OnEnemyUnfreezed?.Invoke(this, EventArgs.Empty);
            turnBeingFreezed = 0;
        }
    }

    private void Chase()
    {
        targetWorldPosition = player.transform.position;
        ResetPath();
        Move();
    }

    private void StartChasing()
    {
        targetWorldPosition = player.transform.position;
        isCahsing = true;
        ResetPath();
    }

}
