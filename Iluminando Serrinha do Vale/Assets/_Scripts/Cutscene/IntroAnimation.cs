using System.Collections;
using UnityEngine;

public class IntroAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioClip _elevatorBeepSound;
    [SerializeField] private AudioClip _elevatorOpenSound;
    [SerializeField] private AudioSource _walkingSound;

    public void StartAnimation()
    {
        StartCoroutine(IntroSequence());
    }

    private IEnumerator IntroSequence()
    {
        _animator.Play("Intro");
        yield return new WaitForSeconds(0.8f);
        AudioPlayer.Instance.PlaySFX(_elevatorBeepSound);
        yield return new WaitForSeconds(0.2f);
        AudioPlayer.Instance.PlaySFX(_elevatorOpenSound);
        yield return new WaitForSeconds(0.5f);
        _walkingSound.Play();
        yield return new WaitForSeconds(2.0f);
        _walkingSound.Stop();
    }
}
