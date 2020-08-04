using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueReader : MonoBehaviour
{
    [Header("Dialogue Box")]
    [SerializeField] private Animator _dialogueBoxAnimator;
    //Muda a escala de acordo com a direção do balão de fala
    [SerializeField] private Transform _dialogueBoxTransform;
    [SerializeField] private Transform _textTransform;
    [SerializeField] private Transform _blurTransform;

    [Header("NPC")]
    [SerializeField] private Animator _npcAnimator;
    [SerializeField] private Image _npcImage;

    [Header("Player")]
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private Image _playerImage;

    [Header("Conversation")]
    [SerializeField] private DialogueHolder _dialogueHolder;
    private int _dialogueIndex;
    private int _lineIndex;
    [SerializeField] private TextMeshProUGUI _lineText;
    [SerializeField] private bool _isEnd;

    [Header("Letter by Letter")]
    private int _totalVisibleCharacters;

    [Header("Sound Effect")]
    [SerializeField] private AudioClip _startDialogueSound;

    public static DialogueReader Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SetStartDialogue(DialogueHolder currentDialogue)
    {
        StopAllCoroutines();
        _isEnd = false;
        _dialogueHolder = currentDialogue;
        _dialogueIndex = 0;
        _lineIndex = -1;

        //Faz a caixa de diálogo aparecer
        _dialogueBoxAnimator.gameObject.SetActive(true);

        //Define a aparência do personagem
        SetSpeakerAppearance();

        //Chama o método de imprimir o texto depois de meio segundo de delay, para dar tempo da animação tocar  
        _lineText.text = "";
        ShowLine(1);
    }

    public void ShowLine(int direction)
    {
        if (!_isEnd)
        {
            _lineIndex += direction;

            //Se a linha não existir, ele vai para o próximo diálogo
            if (!_dialogueHolder.CheckForExistingLine(_dialogueIndex, _lineIndex))
            {
                ChangeDialogue(direction);
            }
            else
            {
                //Toca o som do diálogo
                AudioPlayer.Instance.PlaySFX(_startDialogueSound);

                //Imprime a linha na tela letra por letra            
                _lineText.text = _dialogueHolder.GetLine(_dialogueIndex, _lineIndex);
                //Deixa a quantidade inicial de caracteres visíveis no zero
                _lineText.maxVisibleCharacters = 0;

                StartCoroutine(ShowLetterByLetter(_lineText));
            }
            _lineIndex = _lineIndex < 0 ? 0 : _lineIndex;
        }
    }

    private void ChangeDialogue(int direction)
    {
        _dialogueIndex += direction;
        StartCoroutine(SpeakerState(direction));

        if (_dialogueHolder.CheckForExistingDialogue(_dialogueIndex))
        {
            
            //Reseta a linha e chama para imprimir a linha de novo
            if (direction > 0)
            {
                _lineIndex = -1;
            }
            else
            {
                _lineIndex = _dialogueHolder.GetLineLenght(_dialogueIndex);
            }
            ShowLine(direction);
        }

        //Não deixa o índice ser menor que 0
        _dialogueIndex = _dialogueIndex < 0 ? 0 : _dialogueIndex;

        //Chama as ações cadastradas no diálogo e impede o diálogo de continuar após acabar
        if (_dialogueIndex == _dialogueHolder.GetDialogueLenght())
        {
            _isEnd = true;

            //Executa as ações apenas se tiver ações cadastradas
            if (_dialogueHolder.CheckActions())
            {
                _dialogueHolder.InvokeActions();
            }
        }
    }

    private void SetSpeakerAppearance()
    {        
        //Verifica se a primeira fala é do jogador ou do NPC e atribui a aparência e o nome correto
        if (_dialogueHolder.CheckIsPlayer(_dialogueIndex))
        {
            _playerImage.sprite = _dialogueHolder.GetSpeakerSprite(_dialogueIndex);
            _playerAnimator.gameObject.SetActive(true);

            //Vira a janela para o lado do jogador
            _dialogueBoxTransform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            _textTransform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            _blurTransform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);

            //Fecha a boca do NPC
            if (_dialogueIndex > 0 && !_dialogueHolder.CheckIsPlayer(_dialogueIndex - 1))
            {
                _npcImage.sprite = _dialogueHolder.GetSpeakerSprite(_dialogueIndex - 1, DialogueLine.SpeakerExpressions.Normal);
            }
        }
        else
        {
            _npcImage.sprite = _dialogueHolder.GetSpeakerSprite(_dialogueIndex);
            _npcImage.SetNativeSize();
            _npcAnimator.gameObject.SetActive(true);

            //Vira a janela para o lado do NPC
            _dialogueBoxTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            _textTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            _blurTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            //Fecha a boca do player
            //if (_playerAnimator.gameObject.activeSelf && _dialogueHolder.CheckIsPlayer(_dialogueIndex - 1))
            if(_dialogueIndex > 0 && _dialogueHolder.CheckIsPlayer(_dialogueIndex - 1))
            {
                _playerImage.sprite = _dialogueHolder.GetSpeakerSprite(_dialogueIndex - 1, DialogueLine.SpeakerExpressions.Normal);
            }
        }
    }

    private IEnumerator SpeakerState(int direction)
    {
        if (direction > 0)
        {
            switch (_dialogueHolder.GetSpeakerAction(_dialogueIndex - 1))
            {
                //Não faz nada
                case DialogueLine.SpeakerActions.Nothing:
                    if (_dialogueHolder.CheckForExistingDialogue(_dialogueIndex))
                    {
                        SetSpeakerAppearance();
                    }
                    break;

                //Troca de NPC
                case DialogueLine.SpeakerActions.Change:
                    _npcAnimator.Play("npcAvatar_exit");
                    yield return new WaitForSeconds(0.5f);
                    SetSpeakerAppearance();
                    _npcAnimator.Play("npcAvatar_enter");
                    break;

                //Apenas faz o jogador ou o NPC saírem da tela
                case DialogueLine.SpeakerActions.Leave:
                    SetSpeakerAppearance();
                    if (_dialogueHolder.CheckIsPlayer(_dialogueIndex - 1))
                    {
                        _playerAnimator.Play("playerAvatar_exit");
                    }
                    else
                    {
                        _npcAnimator.Play("npcAvatar_exit");
                    }
                    break;

                case DialogueLine.SpeakerActions.End:
                    StartCoroutine(CloseDialogueBox());
                    break;

                default:
                    //Verifica se é o último diálogo. Se for, ele não faz nada
                    if (_dialogueIndex != _dialogueHolder.GetDialogueLenght())
                    {
                        SetSpeakerAppearance();
                    }
                    break;
            }
        }
        else
        {
            SetSpeakerAppearance();
        }
    }

    private IEnumerator ShowLetterByLetter(TextMeshProUGUI currentTextMesh)
    {
        //Espera o fim do frame para pegar a quantidade certa de palavras na frase
        yield return new WaitForEndOfFrame();

        //Pega a quantidade total de caracteres do texto
        _totalVisibleCharacters = currentTextMesh.textInfo.characterCount;

        //Espera o tempo para tornar visível letra por letra
        for (int i = 0; i <= _totalVisibleCharacters; i++)
        {
            currentTextMesh.maxVisibleCharacters = i;
            yield return new WaitForSeconds(0.01f);
        }
    }

    private IEnumerator CloseDialogueBox()
    {
        _dialogueBoxAnimator.Play("dialogueBox_exit");
        _npcAnimator.Play("npcAvatar_exit");
        _playerAnimator.Play("playerAvatar_exit");

        yield return new WaitForSeconds(1.0f);

        _dialogueBoxAnimator.gameObject.SetActive(false);
        _npcAnimator.gameObject.SetActive(false);
        _playerAnimator.gameObject.SetActive(false);

        //Libera movimento do jogador
        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.CanMove = true;
        }
    }

    public bool GetIsPlayer()
    {
        return _dialogueHolder.CheckIsPlayer(_dialogueIndex);
    }

    public bool GetIsPlayerForQuiz()
    {
        return _dialogueHolder.CheckIsPlayer(_dialogueIndex - 1);
    }
}
