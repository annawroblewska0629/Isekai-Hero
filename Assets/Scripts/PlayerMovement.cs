using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{

    PlayerControls playerControls;


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

        // do funkcji CanMove zostaje przekazana odczytana wartosc, gdy fukncja zwroci prawde obiekt gracza zostaje przemiszczony a limit porszania zmniejsza sie
        if (TurnSystem.Instance.IsPlayerTurn() 
            && LevelGrid.Instance.IsValidGridPosition(targetWorldPosition) 
            && !LevelGrid.Instance.IsPositionBlockedByEnemy(targetWorldPosition))
        {
            transform.Translate(directionVector3);
            movementLimit--;
            TurnSystem.Instance.StartEnemyTurn();
        }
        // do funkcji CanMove zostaje przekazana odczytana wartosc, gdy fukncja zwraca prawde to kafelek na danej pozycji zostaje usuniety oraz zostaje utworozny na nowej, limit poruszana zmniejsza sie
        else if (TurnSystem.Instance.IsPlayerTurn()
            && LevelGrid.Instance.IsValidGridPositionToPush(targetWorldPosition, behindTargetWorldPosition)
            && !LevelGrid.Instance.IsPositionBlockedByEnemy(behindTargetWorldPosition))
        {
            LevelGrid.Instance.UpdatePathNodeDictionary(targetWorldPosition, behindTargetWorldPosition);
            LevelGrid.Instance.ChangeObstacleGridPosition(targetWorldPosition, behindTargetWorldPosition);
            transform.Translate(directionVector3);
            movementLimit--;
            TurnSystem.Instance.StartEnemyTurn();
        }

    }

    private void MovementLimitReachedZero()
    {
        // gdy limit poruszania sie osiagnie zero funkcja zwraca prawde
        if (movementLimit == 0)
        {
            OnMovementLimitReachedZero?.Invoke(this, EventArgs.Empty);
        }
    }

    public int GetMovementScore()
    {
        // funkcja zwaraca limit porszania sie
        return movementLimit;
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
