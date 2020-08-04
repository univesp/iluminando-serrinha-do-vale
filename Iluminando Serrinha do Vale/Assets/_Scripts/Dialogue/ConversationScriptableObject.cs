using UnityEngine;

[CreateAssetMenu(fileName = "New Conversation", menuName = "Dialogue/Conversation")]
public class ConversationScriptableObject : ScriptableObject
{
    public DialogueLine[] Dialogue;
}

[System.Serializable]
public class DialogueLine
{
    public SpeakerScriptableObject Speaker;

    [TextArea]
    public string[] Lines;

    public enum SpeakerActions
    {
        Nothing, //Mesmo que a fala acabe, nada muda
        Change, //Muda o NPC
        Leave, //Faz o player ou o NPC saírem de cena
        End //Encerra toda a conversa, fechando a janela de diálogo
    }
    public SpeakerActions SpeakerAction;

    public enum SpeakerExpressions
    {
        Normal,
        Talking,
        Thinking
    }
    public SpeakerExpressions SpeakerExpression;
}