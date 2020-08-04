using System.Collections;
using UnityEngine;
using GAF.Core;
using GAF.Objects;

public class ElevatorController : MonoBehaviour
{
    [SerializeField] private Animator _currentElevator;

    [SerializeField] private GAFBakedMovieClip _playerSortingLayer;

    [SerializeField] private Transform _destination;

    [SerializeField] private AudioClip _elevatorBeebSound;
    [SerializeField] private AudioClip _elevatorDoorSound;

    private bool _isUsingElevator;
    private bool _isIn;    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            AudioPlayer.Instance.PlaySFX(_elevatorBeebSound);
            AudioPlayer.Instance.PlaySFX(_elevatorDoorSound);
            _currentElevator.Play("elevator_open");
            _isIn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !_isUsingElevator)
        {
            AudioPlayer.Instance.PlaySFX(_elevatorDoorSound);
            _currentElevator.Play("elevator_close");
            _isIn = false;
        }
    }

    private void OnMouseUpAsButton()
    {
        if(_isIn)
        {
            StartCoroutine(ElevatorEvent());
        }
    }

    private IEnumerator ElevatorEvent()
    {
        //Avisa que o jogador está usando o elevador. Assim a animação de fechar a porta não toca duas vezes
        _isUsingElevator = true;

        //Impede o jogador de movimentar até terminar de usar o elevador
        PlayerController.Instance.CanMove = false;

        //Muda a layer do jogador pra ele "entrar" no elevador
        _playerSortingLayer.settings.spriteLayerValue = 3;
        _playerSortingLayer.reload();

       
        //Chama a animação de fechar a porta
        _currentElevator.Play("elevator_close");
        AudioPlayer.Instance.PlaySFX(_elevatorDoorSound);

        //Espera o elevador fechar
        yield return new WaitForSeconds(0.9f);    

        //Avisa que terminou de usar o elevador
        _isUsingElevator = false;
        _isIn = false;

        PlayerController.Instance.transform.position = _destination.position;

        //Espera porta do outro elevador abrir
        yield return new WaitForSeconds(0.3f);

        //Volta o sorting layer normal do jogador
        _playerSortingLayer.settings.spriteLayerValue = 8;
        _playerSortingLayer.reload();

        //Libera o controle do jogador
        PlayerController.Instance.CanMove = true;
    }
}