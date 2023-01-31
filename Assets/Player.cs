using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Action[] playerActionArray;

    // Start is called before the first frame update
    void Awake()
    {
        playerActionArray = GetComponents<Action>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Action[] GetPlayerActionArray()
    {
        return playerActionArray;
    }
}
