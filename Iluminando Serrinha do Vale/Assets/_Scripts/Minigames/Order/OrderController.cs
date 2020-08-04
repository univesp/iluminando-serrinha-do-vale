using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class OrderController : MonoBehaviour
{
    [SerializeField] private Animator[] _animator;
    [SerializeField] private Transform[] _minigameTransforms;
    private OrderButton[] _mapButtons;
    private int _minigameIndex;

    [SerializeField] private Color NormalColor;
    [SerializeField] private Color RightColor;

    [SerializeField] private UnityEvent[] _rightAction;

    [SerializeField] private AudioClip _correctSound;

    private bool _correntAnswer;

    public void CheckOrder()
    {
        _correntAnswer = true;
        _mapButtons = _minigameTransforms[_minigameIndex].GetComponentsInChildren<OrderButton>();

        for (int i = 0; i < _mapButtons.Length; i++)
        {
            if (_mapButtons[i].RightIndex != i)
            {
                _correntAnswer = false;

                _mapButtons[i].ButtonImage.color = NormalColor;
            }
            else
            {
                if (_correntAnswer)
                { 
                    _mapButtons[i].ButtonImage.color = RightColor;
                }
                else
                {
                    _mapButtons[i].ButtonImage.color = NormalColor;
                }
            }
        }

        if (_correntAnswer)
        {
            StartCoroutine(CallActions());
        }
    }

    private IEnumerator CallActions()
    {
        AudioPlayer.Instance.PlaySFX(_correctSound);

        _rightAction[_minigameIndex].Invoke();

        yield return new WaitForSeconds(0.6f);

        _minigameIndex++;
    }
}