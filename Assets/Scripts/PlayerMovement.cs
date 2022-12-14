using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    private float deathDealy = .5f;
    PlayerControls playerControls;
    Vector3 directionVector3;

    [Header("MovementLimit")]
    [SerializeField] int startMovementLimit;

    [Header("Tilemap")] 
    [SerializeField] int cellSize = 1;

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Player.Enable();
        playerControls.Player.Move.performed += Move;
    }

    private void Update()
    {
        if (isDead())
        {
            StartCoroutine(Die());
            playerControls.Player.Move.Disable();
        } 
    }

    private void Move(InputAction.CallbackContext context)
    {
        // odczytanie wartosci z inputu
        Vector2 directionVector2 = context.ReadValue<Vector2>() * cellSize;
        directionVector3 = (Vector3)directionVector2;
        Vector3 targetWorldPosition = transform.position + directionVector3;
        Vector3 behindTargetWorldPosition = transform.position + directionVector3 + directionVector3;
        //Vector3 targetPosition = transform.position + directionVector3;
        // do funkcji CanMove zostaje przekazana odczytana wartosc, gdy fukncja zwroci prawde obiekt gracza zostaje przemiszczony a limit porszania zmniejsza sie
        if (LevelGrid.Instance.IsValidGridPosition(targetWorldPosition) && !LevelGrid.Instance.IsPositionBlockedByEnemy(targetWorldPosition) )
        {
            transform.Translate(directionVector3);
            startMovementLimit--;
        }
        // do funkcji CanMove zostaje przekazana odczytana wartosc, gdy fukncja zwraca prawde to kafelek na danej pozycji zostaje usuniety oraz zostaje utworozny na nowej, limit poruszana zmniejsza sie
        else if (LevelGrid.Instance.IsValidGridPositionToPush(targetWorldPosition, behindTargetWorldPosition) && !LevelGrid.Instance.IsPositionBlockedByEnemy(behindTargetWorldPosition))
          {
            LevelGrid.Instance.ChangeObstacleGridPosition(targetWorldPosition, behindTargetWorldPosition);
            transform.Translate(directionVector3);
            startMovementLimit--;
          }
        
    }

    private bool isDead()
    {
        // gdy limit poruszania sie osiagnie zero funkcja zwraca prawde
        if (startMovementLimit == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator Die()
    {
        // po odczekaniu danego okresu obiekt gracza zostaje zniszczony
        yield return new WaitForSeconds(deathDealy);
        Destroy(gameObject);
    }

    public int GetMovmentScore()
    {
        // funkcja zwaraca limit porszania sie
        return startMovementLimit;
    }
}
