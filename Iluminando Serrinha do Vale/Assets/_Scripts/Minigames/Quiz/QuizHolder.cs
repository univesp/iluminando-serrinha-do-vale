using UnityEngine;
using UnityEngine.Events;

public class QuizHolder : MonoBehaviour
{
    public ChoicesData[] Choices;

    public void StartChoices()
    {
        QuizFiller.Instance.FillData(this);
    }
}

[System.Serializable]
public class ChoicesData
{
    public string ChoiceText;
    public UnityEvent Actions;
}