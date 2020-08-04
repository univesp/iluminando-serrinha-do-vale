using UnityEngine;
using UnityEngine.Events;

public class DialogueHolder : MonoBehaviour
{
    [SerializeField] private ConversationScriptableObject[] _conversation;
    [SerializeField] private UnityEvent[] _actions;
    [SerializeField] private bool _repeatAction;
    private int _currentConversationIndex;

    public void StartDialogue()
    {
        DialogueReader.Instance.SetStartDialogue(this);
    }

    public bool CheckIsPlayer(int dialogueIndex)
    {
        //Se o nome do speaker for Rosângela, é jogador
        if (_conversation[_currentConversationIndex].Dialogue[dialogueIndex].Speaker.Name == "Rosângela")
        {
            return true;
        }
        return false;
    }

    public bool CheckForExistingDialogue(int dialogueIndex)
    {
        if (dialogueIndex < _conversation[_currentConversationIndex].Dialogue.Length && dialogueIndex >= 0)
        {
            return true;
        }
        return false;
    }

    public int GetDialogueLenght()
    {
        return _conversation[_currentConversationIndex].Dialogue.Length;

    }

    public bool CheckForExistingLine(int dialogueIndex, int lineIndex)
    {
        if (dialogueIndex >= _conversation[_currentConversationIndex].Dialogue.Length)
        {
            dialogueIndex = _conversation[_currentConversationIndex].Dialogue.Length - 1;
        }

        if (lineIndex < _conversation[_currentConversationIndex].Dialogue[dialogueIndex].Lines.Length && lineIndex >= 0)
        {
            return true;
        }
        return false;
    }

    public string GetLine(int dialogueIndex, int lineIndex)
    {
        return _conversation[_currentConversationIndex].Dialogue[dialogueIndex].Lines[lineIndex];
    }

    public int GetLineLenght(int dialogueIndex)
    {
        return _conversation[_currentConversationIndex].Dialogue[dialogueIndex].Lines.Length;
    }

    public string GetSpeakerName(int dialogueIndex)
    {
        return _conversation[_currentConversationIndex].Dialogue[dialogueIndex].Speaker.Name;

    }

    public Sprite GetSpeakerSprite(int dialogueIndex)
    {
        switch(_conversation[_currentConversationIndex].Dialogue[dialogueIndex].SpeakerExpression)
        {
            case DialogueLine.SpeakerExpressions.Normal:
                return _conversation[_currentConversationIndex].Dialogue[dialogueIndex].Speaker.Sprite[0];

            case DialogueLine.SpeakerExpressions.Talking:
                return _conversation[_currentConversationIndex].Dialogue[dialogueIndex].Speaker.Sprite[1];

            case DialogueLine.SpeakerExpressions.Thinking:
                return _conversation[_currentConversationIndex].Dialogue[dialogueIndex].Speaker.Sprite[2];

            default:
                return null;
        }
    }

    public Sprite GetSpeakerSprite(int dialogueIndex, DialogueLine.SpeakerExpressions expression)
    {
        switch (expression)
        {
            case DialogueLine.SpeakerExpressions.Normal:
                return _conversation[_currentConversationIndex].Dialogue[dialogueIndex].Speaker.Sprite[0];

            case DialogueLine.SpeakerExpressions.Talking:
                return _conversation[_currentConversationIndex].Dialogue[dialogueIndex].Speaker.Sprite[1];

            case DialogueLine.SpeakerExpressions.Thinking:
                return _conversation[_currentConversationIndex].Dialogue[dialogueIndex].Speaker.Sprite[2];

            default:
                return null;
        }
    }

    public DialogueLine.SpeakerActions GetSpeakerAction(int dialogueIndex)
    {
        return _conversation[_currentConversationIndex].Dialogue[dialogueIndex].SpeakerAction;
    }

    public bool CheckActions()
    {
        //Verifica se as ações existem no evento
        if (_actions.Length > 0 && _currentConversationIndex < _actions.Length)
        {
            return true;
        }
        return false;
    }

    public void InvokeActions()
    {
        _actions[_currentConversationIndex].Invoke();
    }

    public void NextConversationIndex()
    {
        if (_currentConversationIndex < _conversation.Length - 1)
        {
            _currentConversationIndex++;
        }
    }

    public void SetConversationIndex(int index)
    {
        _currentConversationIndex = index;
    }
}
