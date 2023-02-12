using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI actionPoints;
    [SerializeField] TextMeshProUGUI movementLimits;
    [SerializeField] Player player;
    [SerializeField] PlayerMovement playerMovement;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateActionPoints();
        UpdateMovementLimit();
    }

    private void UpdateActionPoints()
    {
        actionPoints.text = player.GetActionPoints().ToString();
    }

    private void UpdateMovementLimit()
    {
        movementLimits.text = playerMovement.GetMovementScore().ToString();
    }
}
