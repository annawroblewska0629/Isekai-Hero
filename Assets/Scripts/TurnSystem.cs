using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    private bool isPlayerTurn = true;
    public event EventHandler OnEnemyTurnStarted;
    public static TurnSystem Instance { get; private set; }  
   
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more thean one TurnSystem" + transform + "-" + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void StartEnemyTurn()
    {
        isPlayerTurn = false;
        Debug.Log("EnemyTurn");
        OnEnemyTurnStarted?.Invoke(this, EventArgs.Empty);
  
    }
    public void StartPlayerTurn()
    {
        isPlayerTurn = true;
        Debug.Log("PlayerTurn");

    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }
}
