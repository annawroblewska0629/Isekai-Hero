using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreGame : MonoBehaviour
{
    [SerializeField] GameObject key;
    [SerializeField] GameObject exit;
    [SerializeField] Transform player;

    private bool hasKey = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            if (key != null)
            {
                PlayerHasKey();
            }
            PlayerEndLevel();
        }

    }

    private void PlayerHasKey()
    {
        if(player.transform.position == key.transform.position)
        {
            hasKey = true;
            Destroy(key);
        }
    }

    private void PlayerEndLevel()
    {
        if(player.transform.position == exit.transform.position && hasKey)
        {
            Debug.Log("Wygrales");
        }
    }
}
