using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TurnSystem.Instance.OnEnemyTurnStarted += TurnSystem_OnEnemyTurnStarted;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator EnemyTurn()
    {
        // po odczekaniu danego okresu obiekt gracza zostaje zniszczony
        yield return new WaitForSeconds(2);
        TurnSystem.Instance.StartPlayerTurn();
    }

    public void TurnSystem_OnEnemyTurnStarted(object sender, EventArgs e)
    {
        StartCoroutine(EnemyTurn());
    }
}
