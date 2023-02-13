using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    HealthSystem healthSystem;
    PlayerMovement playerMovement;
    [SerializeField] private int actionPointsMax;
    private int currentActionPoints;
    private Action[] playerActionArray;
    public event EventHandler OnPlayerDead;
    [SerializeField] Animator playerAnimator;

    // Start is called before the first frame update
    void Awake()
    {
        playerActionArray = GetComponents<Action>();
        currentActionPoints = actionPointsMax;
        healthSystem = GetComponent<HealthSystem>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Start()
    {
        healthSystem.OnDead += HealthSystem_OnDead; 
        playerMovement.OnMovementLimitReachedZero += PlayerMovement_OnMovementLimitReachedZero;
        Debug.Log(LevelGrid.Instance.WorldPositionToGridPosition(transform.position));
    }


    private void PlayerMovement_OnMovementLimitReachedZero(object sender, EventArgs e)
    {
        Damage(healthSystem.GetHalth());
    }

    public Action[] GetPlayerActionArray()
    {
        return playerActionArray;
    }

    private void SpendActionPoints(int actionCost)
    {
        currentActionPoints -= actionCost;
    }

    private bool CanSpendActionPoints(Action action)
    {
        if(currentActionPoints >= action.GetActionPointsCost())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool TrySpendActionPoints(Action action)
    {
        if (CanSpendActionPoints(action))
        {
            SpendActionPoints(action.GetActionPointsCost());
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetActionPoints()
    {
        return currentActionPoints;
    }

    public void Damage(int damage)
    {
        healthSystem.Damage(damage);
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        StartCoroutine(Death());
    }
    
    private IEnumerator Death()
    {
        playerMovement.DisablePlayerMovment();
        // po odczekaniu danego okresu obiekt gracza zostaje zniszczony
        yield return new WaitForSeconds(0.5f);
        playerAnimator.SetTrigger("isDead");
        yield return new WaitForSeconds(1);
        OnPlayerDead?.Invoke(this, EventArgs.Empty);
    }
    public bool isPositionBloeckedByPlayer(Vector3 worldPosition)
    {
        return LevelGrid.Instance.WorldPositionToGridPosition(transform.position) == LevelGrid.Instance.WorldPositionToGridPosition(worldPosition);
    }
}
