using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainer;
    private List<ActionButton> actionButtonList;
    [SerializeField] Player player;

    private void Awake()
    {
        actionButtonList = new List<ActionButton>();
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerActionSystem.Instance.OnActiveActionChanged += PlayerActionSystem_OnActiveActionChanged;
        PlayerActionSystem.Instance.OnActiveActionDeactivation += PlayerActionSystem_OnActiveActionDeactivation;
        CreatePlayerActionButtons();
        UpdateActiveActionButtonVisual();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreatePlayerActionButtons()
    {
       

        foreach(Action action in player.GetPlayerActionArray())
        {
            Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainer);
            ActionButton actionButton = actionButtonTransform.GetComponent<ActionButton>();
            actionButton.SetAction(action);
            actionButtonList.Add(actionButton);
        }
    }

    private void UpdateActiveActionButtonVisual()
    {
        foreach(ActionButton actionButton in actionButtonList)
        {
            actionButton.UpdateActiveActionButtonVisual();
        }
    }
    public void PlayerActionSystem_OnActiveActionChanged(object sender, EventArgs e)
    {
        UpdateActiveActionButtonVisual();
    }

    public void PlayerActionSystem_OnActiveActionDeactivation(object sender, EventArgs e)
    {
        UpdateActiveActionButtonVisual(); 
    }
}
