using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    private List<Enemy> ListOfEnemies = new List<Enemy>();

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one EnemyManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    // Start is called before the first frame update
    private void Start()
    {
        Enemy.OnEnemySpawned += Enemy_OnEnemySpawned;
        Enemy.OnEnemyDead += Enemy_OnEnemyDead;
    }

    private void Enemy_OnEnemySpawned(object sender, EventArgs e)
    {
        Enemy enemy = sender as Enemy;

        ListOfEnemies.Add(enemy);

    }

    private void Enemy_OnEnemyDead(object sender, EventArgs e)
    {
        Enemy enemy = sender as Enemy;

        ListOfEnemies.Remove(enemy);
    }

    public List<Enemy> GetEnemyList()
    {
        return ListOfEnemies;
    }
}
