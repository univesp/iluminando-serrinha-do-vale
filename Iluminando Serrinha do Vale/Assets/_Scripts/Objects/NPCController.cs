using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField] private GameObject _dialogueBox;
    [SerializeField] private DialogueHolder _dialogueHolder;
    [SerializeField] private CharacterIcon _characterIcon;
    private bool _isIn;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            _characterIcon.FilledIcon();
            _isIn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            _characterIcon.EmptyIcon();
            _isIn = false;
        }
    }

    private void OnMouseUpAsButton()
    {
        if(_isIn)
        {
            PlayerController.Instance.CanMove = false;
            _dialogueBox.SetActive(true);
            _dialogueHolder.StartDialogue();
            _isIn = false;
        }
    }
}