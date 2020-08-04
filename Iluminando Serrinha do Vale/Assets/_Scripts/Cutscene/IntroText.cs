using System.Collections;
using UnityEngine;
using TMPro;

public class IntroText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMesh;

    [Space]

    [TextArea]
    [SerializeField] private string[] _introText;
    private int _textIndex;
    private int _totalVisibleCharacters;
    private bool _isPrintingText;

    [SerializeField] private AnimationActions _animationActions;

    private void Start()
    {
        Invoke("TextClickAction", 0.6f);
    }

    public void TextClickAction()
    {
        if(_isPrintingText)
        {
            //Para as coroutines, mostra o texto todo e avisa que parou de imprimir
            StopAllCoroutines();
            _textMesh.maxVisibleCharacters = _totalVisibleCharacters;
            _isPrintingText = false;

            //Sai dessa função
            return;
        }
        //Chama o próximo texto
        CallNextText();
    }

    private void CallNextText()
    {
        if (_textIndex < _introText.Length)
        {
            _textMesh.text = _introText[_textIndex];
            _textIndex++;
            
        StartCoroutine(LetterByLetter());
        }
        else
        {
            //Chama a próxima tela
            _animationActions.StartAnimation();
        }
    }

    private IEnumerator LetterByLetter()
    {       
        //Avisa que o texto está sendo impresso na tela
        _isPrintingText = true;

        //Espera o fim do frame para pegar a quantidade certa de palavras na frase
        yield return new WaitForEndOfFrame();

        //Pega a quantidade total de caracteres do texto
        _totalVisibleCharacters = _textMesh.textInfo.characterCount;

        //Espera o tempo para tornar visível letra por letra
        for(int i = 0; i <= _totalVisibleCharacters; i++)
        {
            _textMesh.maxVisibleCharacters = i;
            yield return new WaitForSeconds(0.01f);
        }

        //Avisa que terminou de imprimir o texto na tela
        _isPrintingText = false;
    }
}
