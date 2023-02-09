using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDead;

    [SerializeField] private int healthMax;
    private int heatlh;
    // Start is called before the first frame update
    void Awake()
    {
        heatlh = healthMax;
    }
    public void Damage(int damage)
    {
        heatlh -= damage;
        if(heatlh < 0)
        {
            heatlh = 0;
        }
        if(heatlh == 0)
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
        return heatlh;
    }
}
