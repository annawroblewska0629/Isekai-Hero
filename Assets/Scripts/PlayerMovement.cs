using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    private float deathDealy = .5f;

    [Header("MovmentScore")]
    [SerializeField] int startMovmentScore;
    
    [Header("Tilemap")] 
    [SerializeField] Tilemap groundTilemap;
    [SerializeField] Tilemap obstacleTilemap;
    [SerializeField] int cellSize = 1;

    private void Awake()
    {
        PlayerControls playerControls = new PlayerControls();
        playerControls.Player.Enable();
        playerControls.Player.Move.performed += Move;

       
    }
    private void Update()
    {
        if (isDead())
        {
            StartCoroutine(Die());
        }
    }

    private void Move(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>() * cellSize;
        if (CanMove(direction))
        {
            transform.position += (Vector3)direction;
            startMovmentScore--;
        }
    }

    private bool CanMove(Vector2 direction)
   { 
        Vector3Int targetGridPosition = groundTilemap.WorldToCell(transform.position + (Vector3)direction);
        if(!groundTilemap.HasTile(targetGridPosition) || obstacleTilemap.HasTile(targetGridPosition) || isDead())
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool isDead()
    {
        if (startMovmentScore == 0)
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
        return startMovmentScore;
    }
}
