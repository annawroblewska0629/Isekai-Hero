using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDead;
    public event EventHandler OnDamaged;
    [SerializeField] private int healthMax;
    private int health;
    // Start is called before the first frame update
    void Awake()
    {
        health = healthMax;
    }
    public void Damage(int damage)
    {
        health -= damage;
        if(health < 0)
        {
            health = 0;
        }
        OnDamaged?.Invoke(this, EventArgs.Empty);
        if (health == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }
    public int GetHalth()
    {
        return health;
    }

    public float GetHealthNormalized()
    {
        return (float)health / healthMax;
    }
}
