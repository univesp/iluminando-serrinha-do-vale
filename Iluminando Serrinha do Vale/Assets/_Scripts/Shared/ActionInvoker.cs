using UnityEngine;
using UnityEngine.Events;

public class ActionInvoker : MonoBehaviour
{
    [SerializeField] private UnityEvent _actions;
    [SerializeField] float _delayTime;

    private void OnMouseUpAsButton()
    {
        Invoke("InvokeActions", _delayTime);
    }

    public void CallActions()
    {
        Invoke("InvokeActions", _delayTime);
    }

    private void InvokeActions()
    {
        _actions.Invoke();
    }
}
