using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    private float deathDealy = .5f;
    PlayerControls playerControls;
    Vector3 directionVector3;

    [Header("MovementLimit")]
    [SerializeField] int startMovementLimit;

    [Header("Tilemap")]
    [SerializeField] Tilemap groundTilemap;
    [SerializeField] Tilemap obstacleTilemap;
    [SerializeField] TileBase obstacleTile;
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
        Vector2 directionVector2 = context.ReadValue<Vector2>() * cellSize;
        directionVector3 = (Vector3)directionVector2;
        if (CanMove(directionVector3))
        {
            transform.Translate(directionVector3);
            startMovementLimit--;
        }
        if(CanPush(directionVector3))
          {
            Vector3Int targetGridPositionToMove = groundTilemap.WorldToCell(transform.position + directionVector3);
            Vector3Int targetGridPositionToPush = groundTilemap.WorldToCell(transform.position + directionVector3 + directionVector3);
            
            obstacleTilemap.SetTile(targetGridPositionToPush, obstacleTile);
            obstacleTilemap.SetTile(targetGridPositionToMove, null);
            transform.Translate(directionVector3);
            startMovementLimit--;
          }
        
    }

    private bool CanMove(Vector3 direction)
   { 
        Vector3Int targetGridPositionToMove = groundTilemap.WorldToCell(transform.position + direction);
        if(!groundTilemap.HasTile(targetGridPositionToMove) || obstacleTilemap.HasTile(targetGridPositionToMove))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    private bool CanPush(Vector3 direction)
    {
        Vector3Int targetGridPositionToMove = obstacleTilemap.WorldToCell(transform.position + direction);
        Vector3Int targetGridPositionToPush = obstacleTilemap.WorldToCell(transform.position + directionVector3 + directionVector3);
        //Vector3Int targetGridPositionToPush = targetGridPositionToMove + groundTilemap.WorldToCell(direction);
        if ( obstacleTilemap.HasTile(targetGridPositionToMove) && !obstacleTilemap.HasTile(targetGridPositionToPush) && groundTilemap.HasTile(targetGridPositionToPush) )
        {
            return true;
        }
        else
        {
            return false;
        }

    }
   
   
    private bool isDead()
    {
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
        yield return new WaitForSeconds(deathDealy);
        Destroy(gameObject);
    }

    public int GetMovmentScore()
    {
        return startMovementLimit;
    }
}
