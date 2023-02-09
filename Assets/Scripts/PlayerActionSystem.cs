using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerActionSystem : MonoBehaviour
{
    public static PlayerActionSystem Instance { get; private set; }
    public event EventHandler OnActiveActionChanged;
    public event EventHandler OnActiveActionDeactivation;
    private Action activeAction;
    [SerializeField] private Player player;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one PlayerActionSystem!" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }
        if(activeAction != null)
        {
            HandleActiveAction();
        }
        
    }

    public void SetActiveAction(Action action)
    {
        activeAction = action;
        if(action != null)
        {
            OnActiveActionChanged?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            OnActiveActionDeactivation?.Invoke(this, EventArgs.Empty);
        }
        
    }
    public Action GetActiveAction()
    {
        return activeAction;
    }

    private void HandleActiveAction()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector3Int gridMousePosition = MousePosition.GetMouseGridPosition();

            if (!activeAction.isValidActionCastPosition(gridMousePosition))
            {
                return;
            }
            if (!player.TrySpendActionPoints(activeAction))
            {
                return;
            }
            activeAction.TakeAction(gridMousePosition);
            TurnSystem.Instance.StartEnemyTurn();
        }
    }
}
