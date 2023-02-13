using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreGame : MonoBehaviour
{
   // public static CoreGame Instance;
    [SerializeField] GameObject key;
    [SerializeField] GameObject exit;
    [SerializeField] Transform player;
    [SerializeField] Animator chestAnimator;
    private bool isChestOpen = false;
    public event EventHandler OnPlayerEndLevel;

    private bool hasKey = false;
    // Start is called before the first frame update
   /* void Start()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one CoreGame!" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
    }
   */
    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            if (key != null)
            {
                PlayerHasKey();
            }
            if (!isChestOpen)
            {
                PlayerEndLevel();
            }

        }

    }

    private void PlayerHasKey()
    {
        if (LevelGrid.Instance.WorldPositionToGridPosition(player.transform.position) == LevelGrid.Instance.WorldPositionToGridPosition(key.transform.position)) 
        {
            hasKey = true;
            Destroy(key);
        }
    }

    private void PlayerEndLevel()
    {
        if(LevelGrid.Instance.WorldPositionToGridPosition(player.transform.position) == LevelGrid.Instance.WorldPositionToGridPosition(exit.transform.position) && hasKey)
        {
            isChestOpen = true;
            chestAnimator.SetTrigger("isOpen");
            OnPlayerEndLevel?.Invoke(this, EventArgs.Empty);
        }
    }
}
