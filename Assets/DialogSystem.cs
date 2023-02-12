using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    [SerializeField] CoreGame coreGame;
    [SerializeField] DialogSO dialogSO;
    [SerializeField] GameObject dialogUI;
    [SerializeField] GameObject girl;
    [SerializeField] GameObject endButton;
    [SerializeField] GameObject playerUI;
    [SerializeField] GameObject playerCards;
     
    [Header("Question")]
    [SerializeField] TextMeshProUGUI questionText;

    [Header("Answers")]
    [SerializeField] GameObject[] answerButtons;
    int indexOfCorrectAnswer;



    // Start is called before the first frame update
    void Start()
    {
       coreGame.OnPlayerEndLevel += CoreGame_OnPlayerEndLevel;
        dialogUI.SetActive(false);
        girl.SetActive(false);
        SetButtonActive(false);
        endButton.SetActive(false);
    }

    private void Update()
    {
        
    }

    private void CoreGame_OnPlayerEndLevel(object sender, EventArgs e)
    {
        playerCards.SetActive(false);
        playerUI.SetActive(false);
        dialogUI.SetActive(true);
        girl.SetActive(true);
        SetButtonActive(true);
        DisplayDialog();
    }

    void DisplayDialog()
    {
        questionText.text = dialogSO.GetQuestion();
        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = dialogSO.GetAnswer(i);
        }
    }

    void DisplayAnswer(int index)
    {

        if (index == dialogSO.GetCorrectAnswerIndex())
        {
            questionText.text = dialogSO.GetGoodReply();

        }
        else
        {

            questionText.text = dialogSO.GetBadReply();

        }
    }

    public void onAnswerSelected(int index)
    {
        DisplayAnswer(index);
        SetButtonActive(false);
        endButton.SetActive(true);

    }
    void SetButtonActive(bool state)
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {

            answerButtons[i].SetActive(state);
        }
    }

}
