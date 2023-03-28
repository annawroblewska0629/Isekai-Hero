using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private bool enemyAllActionsCompleted = false;
    //private int currentEnemyIndex = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            if (!enemyAllActionsCompleted)
            {
                EnemyTakeTurn();
            }
            else
            {
                TurnSystem.Instance.StartPlayerTurn();
                enemyAllActionsCompleted = false;
            }
        }
    }

    private void EnemyTakeTurn()
    {
        foreach (Enemy enemy in EnemyManager.Instance.GetEnemyList())
        {
            enemy.EnemyTakeAction();
            //currentEnemyIndex++;
        }
        enemyAllActionsCompleted = true;
    }

}
