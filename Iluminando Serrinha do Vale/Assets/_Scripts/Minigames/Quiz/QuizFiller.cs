using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class QuizFiller : MonoBehaviour
{
    [SerializeField] private Transform _buttonScale;
    [SerializeField] private Transform[] _scrollRectsScale;

    [SerializeField] private GameObject[] _buttons;
    [SerializeField] private Animator[] _buttonsAnimator;
    [SerializeField] private TextMeshProUGUI[] _buttonsText;
    [SerializeField] private UnityEvent[] _actions;

    //Animators dos personagens do diálogo
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private Animator _npcAnimator;

    public static QuizFiller Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDisable()
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].SetActive(false);
        }
    }

    public void FillData(QuizHolder choices)
    {
        StartCoroutine(SetButtons(choices));
    }

    private IEnumerator SetButtons(QuizHolder choices)
    {
        //Vira as opções no lado certo
        if (DialogueReader.Instance.GetIsPlayerForQuiz())
        {
            _npcAnimator.Play("npcAvatar_exit");
            yield return new WaitForSeconds(0.5f);
            _npcAnimator.gameObject.SetActive(false);

            _buttonScale.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            for(int i = 0; i < _scrollRectsScale.Length; i++)
            {
                _scrollRectsScale[i].localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
        }
        else
        {
            _playerAnimator.Play("playerAvatar_exit");
            yield return new WaitForSeconds(0.5f);
            _playerAnimator.gameObject.SetActive(false);

            _buttonScale.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            for (int i = 0; i < _scrollRectsScale.Length; i++)
            {
                _scrollRectsScale[i].localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }
        }

        //Aguarda animação do título entrando
        yield return new WaitForSeconds(0.5f);

        _actions = new UnityEvent[choices.Choices.Length];

        for (int i = 0; i < choices.Choices.Length; i++)
        {           
            //Inicia o botão
            _buttons[i].SetActive(true);
            //Atribui a função ao botão            
            _actions[i] = choices.Choices[i].Actions;
            //Pega o texto do botão
            _buttonsText[i].text = choices.Choices[i].ChoiceText;

            yield return new WaitForSeconds(0.3f);
        }
    }

    public void InvokeButtonAction(int index)
    {
        StartCoroutine(CloseButtons(index));
    }

    private IEnumerator CloseButtons(int index)
    {
        for (int i = _buttons.Length - 1; i >= 0; i--)
        {
            if (!_buttons[i].activeSelf)
            {
                continue;
            }
            _buttonsAnimator[i].Play("quizButton_exit");
        }

        yield return new WaitForSeconds(0.5f);

        _actions[index].Invoke();
        gameObject.SetActive(false);
    }
}
