using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

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

            StartCoroutine(EnemyTakeTurn());


        }
        TurnSystem.Instance.StartPlayerTurn();
    }

    private IEnumerator EnemyTakeTurn()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        foreach (Enemy enemy in EnemyManager.Instance.GetEnemyList())
        {
            enemy.EnemyTakeAction();
            //currentEnemyIndex++;
        }
    }

}
