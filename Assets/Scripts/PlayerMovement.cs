using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    PlayerControls playerControls;
    [SerializeField] CoreGame coreGame;
    [SerializeField] GameManager gameManager;
    [SerializeField] Player player;

    [Header("MovementLimit")]
    [SerializeField] int movementLimit;
    public event EventHandler OnMovementLimitReachedZero;

    int cellSize = 2;

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Player.Enable();
        playerControls.Player.Move.performed += Move;
  
    }
    private void Start()
    {

        PlayerActionSystem.Instance.OnActiveActionChanged += PlayerActionSystem_OnActiveActionChanged;
        PlayerActionSystem.Instance.OnActiveActionDeactivation += PlayerActionSystem_OnActiveActionDeactivation;
        PauseMenu.Instance.OnGamePaused += PauseMenu_OnGamePaused;
        PauseMenu.Instance.OnGameResumed += PauseMenu_OnGameResumed;
        PauseMenu.Instance.OnGameRestart += GameManager_OnGameRestarted;
        coreGame.OnPlayerEndLevel += CoreGame_OnPlayerEndLevel;
        gameManager.OnGameRestarted += GameManager_OnGameRestarted;
        player.OnPlayerDead += GameManager_OnGameRestarted;
        // LevelGrid.Instance.SetIsNotWalkablePathNode(transform.position);
    }

    private void GameManager_OnGameRestarted(object sender, EventArgs e)
    {
        playerControls.Player.Move.performed -= Move;
    }

    private void CoreGame_OnPlayerEndLevel(object sender, EventArgs e)
    {
        DisablePlayerMovment();
    }

    private void PauseMenu_OnGameResumed(object sender, EventArgs e)
    {
        EnablePlayerMovment();
    }

    private void PauseMenu_OnGamePaused(object sender, EventArgs e)
    {
        DisablePlayerMovment();
    }

    private void Update()
    {
        MovementLimitReachedZero();
    }

    private void Move(InputAction.CallbackContext context)
    {
        // odczytanie wartosci z inputu
        Vector2 directionVector2 = context.ReadValue<Vector2>() * cellSize;
        Vector3 directionVector3 = (Vector3)directionVector2;
        Vector3 targetWorldPosition = transform.position + directionVector3;
        Vector3 behindTargetWorldPosition = transform.position + directionVector3 + directionVector3;

        /*if (TurnSystem.Instance.IsPlayerTurn() 
            && LevelGrid.Instance.IsValidGridPosition(targetWorldPosition) 
            && !LevelGrid.Instance.IsPositionBlockedByEnemy(targetWorldPosition))
        {
            UpdateMovementLimit();
            transform.Translate(directionVector3);
            TurnSystem.Instance.StartEnemyTurn();
        }
        else if (TurnSystem.Instance.IsPlayerTurn()
            && LevelGrid.Instance.IsValidGridPositionToPush(targetWorldPosition, behindTargetWorldPosition)
            && !LevelGrid.Instance.IsPositionBlockedByEnemy(behindTargetWorldPosition))
        {
            UpdateMovementLimit();
            LevelGrid.Instance.UpdatePathNodeDictionary(targetWorldPosition, behindTargetWorldPosition);
            LevelGrid.Instance.ChangeObstacleGridPosition(targetWorldPosition, behindTargetWorldPosition);
            transform.Translate(directionVector3);
            TurnSystem.Instance.StartEnemyTurn();
        }
        */
        MoveAction(directionVector3, targetWorldPosition);
        PushAction(directionVector3, targetWorldPosition, behindTargetWorldPosition);
    }

    private void MoveAction(Vector3 directionVector3, Vector3 targetWorldPosition)
    {
        if (CanMove(targetWorldPosition)) 
        {
            UpdateMovementLimit();
            transform.Translate(directionVector3);
            TurnSystem.Instance.StartEnemyTurn();
        }
 
    }

    private void PushAction(Vector3 directionVector3, Vector3 targetWorldPosition, Vector3 behindTargetWorldPosition)
    {
        if(CanPush(targetWorldPosition, behindTargetWorldPosition))
        {
            UpdateMovementLimit();
            PushObstacle(targetWorldPosition, behindTargetWorldPosition);
            transform.Translate(directionVector3);
            TurnSystem.Instance.StartEnemyTurn();
        }
    }

    private void PushObstacle(Vector3 targetWorldPosition, Vector3 behindTargetWorldPosition)
    {
        LevelGrid.Instance.UpdatePathNodeDictionary(targetWorldPosition, behindTargetWorldPosition);
        LevelGrid.Instance.ChangeObstacleGridPosition(targetWorldPosition, behindTargetWorldPosition);
    }

    private bool CanMove(Vector3 targetWorldPosition)
    {
        if (TurnSystem.Instance.IsPlayerTurn()
            && LevelGrid.Instance.IsValidGridPosition(targetWorldPosition)
            && !LevelGrid.Instance.IsPositionBlockedByEnemy(targetWorldPosition))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CanPush(Vector3 targetWorldPosition, Vector3 behindTargetWorldPosition)
    {
        if (TurnSystem.Instance.IsPlayerTurn()
            && LevelGrid.Instance.IsValidGridPositionToPush(targetWorldPosition, behindTargetWorldPosition)
            && !LevelGrid.Instance.IsPositionBlockedByEnemy(behindTargetWorldPosition))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void MovementLimitReachedZero()
    {
        // gdy limit poruszania sie osiagnie zero funkcja zwraca prawde
        if (movementLimit < 0)
        {
            OnMovementLimitReachedZero?.Invoke(this, EventArgs.Empty);
        }
    }

    public int GetMovementScore()
    {
        // funkcja zwaraca limit porszania sie
        return movementLimit;
    }

    private void UpdateMovementLimit()
    {
        if (LevelGrid.Instance.isSandAtGridPosition(transform.position))
        {
            movementLimit -= 2;
        }
        else
        {
            movementLimit--;
        }
    }

    public void DisablePlayerMovment()
    {
        playerControls.Player.Move.Disable();
    }
    public void EnablePlayerMovment()
    {
        playerControls.Player.Move.Enable();
    }

    public void PlayerActionSystem_OnActiveActionChanged(object sender, EventArgs e)
    {
        DisablePlayerMovment();
    }

    public void PlayerActionSystem_OnActiveActionDeactivation(object sender, EventArgs e)
    {
        EnablePlayerMovment();
    }
}
