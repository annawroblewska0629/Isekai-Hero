using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{
    private Action action;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI actionNameText;
    [SerializeField] private TextMeshProUGUI costActionText;
    [SerializeField] private GameObject activeActionButtonVisual;
    private bool isPressed = false;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAction(Action action)
    {
        this.action = action;
        
        actionNameText.text = action.GetActionName().ToUpper();
        costActionText.text = "COST: " + action.GetActionPointsCost().ToString().ToUpper();

        button.onClick.AddListener(() =>
        {
            Action activeAction = PlayerActionSystem.Instance.GetActiveAction();

            if (action != activeAction)
            {
                isPressed = false;
            }
            if (!isPressed)
            {
                PlayerActionSystem.Instance.SetActiveAction(action);
             
            }
            if (isPressed)
            {
                PlayerActionSystem.Instance.SetActiveAction(null);
               
            }
            OnAndOffAction();

        });
    }

    private void OnAndOffAction()
    {
        isPressed = !isPressed;
    }

    public void UpdateActiveActionButtonVisual()
    {
        Action activeAction = PlayerActionSystem.Instance.GetActiveAction();
        activeActionButtonVisual.SetActive(activeAction == action);
    }

}
