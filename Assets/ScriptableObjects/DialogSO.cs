using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialog", fileName = "NewDialog")]
public class DialogSO : ScriptableObject
{
    [TextArea(3, 7)]
    [SerializeField] string question = "Enter question text here";
    [SerializeField] string[] answers = new string[2];
    [SerializeField] string badReply = "Enter a bad reply";
    [SerializeField] string goodReply = "Enter a bad reply";
    [SerializeField] int indexOfRightAnswer;

    public string GetAnswer(int index)
    {
        return answers[index];
    }

    public int GetCorrectAnswerIndex()
    {
        return indexOfRightAnswer;
    }

    public string GetQuestion()
    {
        return question;
    }

    public string GetBadReply()
    {

        return badReply;
    }

    public string GetGoodReply()
    {
        return goodReply;
    }
}
