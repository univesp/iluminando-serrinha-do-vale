using UnityEngine;
using UnityEngine.EventSystems;

public class UISounds : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    [SerializeField] private AudioClip _enterSound;
    [SerializeField] private AudioClip _clickSound;

    public void OnPointerDown(PointerEventData eventData)
    {
        AudioPlayer.Instance.PlaySFX(_clickSound);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioPlayer.Instance.PlaySFX(_enterSound);
    }
}