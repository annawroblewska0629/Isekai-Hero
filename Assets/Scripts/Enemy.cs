using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    HealthSystem healthSystem;
    List<Vector3> targetsWorldPositionList = new List<Vector3>();
    Vector3 targetWorldPosition = new Vector3(5, -3, 0);
    Vector3 targetWorldPosition1 = new Vector3(-5, -3, 0);
    Vector3 targetWorldPosition2;
    private int currentPositionIndex = 1;
    private float timer = 0.25f;
    // Start is called before the first frame update

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
    }
    void Start()
    {
        LevelGrid.Instance.AddEnemyPosition(transform.position, this);
        healthSystem.OnDead += HealthSystem_OnDead;
        targetWorldPosition2 = targetWorldPosition;
      //  Debug.Log(LevelGrid.Instance.WorldPositionToGridPosition(transform.position) + " pozycja enemy ");
      //  Debug.Log(LevelGrid.Instance.WorldPositionToGridPosition(targetWorldPosition)+ "target pozycja");
    }

    // Update is called once per frame
    void Update()
    {
        if (!TurnSystem.Instance.IsPlayerTurn()  && transform.position != targetWorldPosition2)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                ListOfPositions(targetWorldPosition2);
                Move();
                targetsWorldPositionList.Clear();
            }if(transform.position == targetWorldPosition)
            {
                targetWorldPosition2 = targetWorldPosition1;
            }if(transform.position == targetWorldPosition1)
            {
                targetWorldPosition2 = targetWorldPosition;
            }
            //StartCoroutine(EnemyTurn());
        }
    }

    public void Damage(int damage)
    {
        healthSystem.Damage(damage);
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveEnemyPosition(transform.position);
        Destroy(gameObject);
    }

   private void Move()
    {
        Vector3 currentTargetWorldPosition = targetsWorldPositionList[currentPositionIndex];
        if(transform.position != currentTargetWorldPosition)
        {
            transform.position = currentTargetWorldPosition;
            //++currentPositionIndex;

        }
            TurnSystem.Instance.StartPlayerTurn();
        timer = 0.25f;
    }

    private void ListOfPositions(Vector3 targetWorldPosition)
    {
        List<Vector3Int> pathGridPositionList = Pathfinding.Instance.FindPath(LevelGrid.Instance.WorldPositionToGridPosition(transform.position), LevelGrid.Instance.WorldPositionToGridPosition(targetWorldPosition));
        //List<Vector3Int> pathGridPositionList = Pathfinding.Instance.FindPath(Vector3Int.FloorToInt(transform.position), LevelGrid.Instance.WorldPositionToGridPosition(targetWorldPosition));

        foreach (Vector3Int pathGridPosition in pathGridPositionList)
        {
            targetsWorldPositionList.Add(LevelGrid.Instance.GetCellCenterWorld(pathGridPosition));
        }

    }

    private IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(2);
        
        ListOfPositions(targetWorldPosition);
        Move();
    }

}
