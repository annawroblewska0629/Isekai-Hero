using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerActionSystem : MonoBehaviour
{
    public static PlayerActionSystem Instance { get; private set; }
    public event EventHandler OnActiveActionChanged;
    public event EventHandler OnActiveActionDeactivation;
    private Action activeAction;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one PlayerActionSystem!" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
