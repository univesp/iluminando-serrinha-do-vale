using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudiencePresentation : MonoBehaviour
{
    [SerializeField] private DialogueHolder _dialogueHolder;
    [SerializeField] private GameObject _dialogueBox;

    [SerializeField] private GameObject _engiIdle;
    [SerializeField] private GameObject _engiTalk;

    private void Start()
    {
        StartCoroutine(FirstPart());
    }

    private IEnumerator FirstPart()
    {
        yield return new WaitForSeconds(2.0f);
        _dialogueBox.SetActive(true);
        _dialogueHolder.StartDialogue();
    }

    public void TalkEngi()
    {
        _engiIdle.SetActive(false);
        _engiTalk.SetActive(true);
    }

    public void IdleEngi()
    {
        _engiIdle.SetActive(true);
        _engiTalk.SetActive(false);
    }
}