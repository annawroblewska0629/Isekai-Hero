using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyWorldUI : MonoBehaviour
{
    [SerializeField] private Enemy ennemy;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private GameObject snowflake;
   // [SerializeField] private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        ennemy.OnEnemyFreezed += Enemy_OnEnemyFeezed;
        ennemy.OnEnemyUnfreezed += Enemy_OnEnemyUnfreezed;
        UpdateHealthBar();
    }

    private void Enemy_OnEnemyUnfreezed(object sender, EventArgs e)
    {
        snowflake.SetActive(false);
    }

    private void Enemy_OnEnemyFeezed(object sender, EventArgs e)
    {
        snowflake.SetActive(true);
    }

    private void UpdateHealthBar()
    {
        healthBarImage.fillAmount = healthSystem.GetHealthNormalized();

    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }
}
