using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Animations")]
    [SerializeField] private GameObject _idleAnimation;
    [SerializeField] private GameObject _walkingAnimation;

    [Header("Destination")]
    [SerializeField] private Camera _mainCamera;
    private Vector3 _mousePos;    

    [Header("Walking")]
    [SerializeField] private float _speed;
    [SerializeField] private AudioClip _walkingSound;
    private float _walkingSoundTimer = 0;
    private bool _isWalking;

    [Header("Global variables")]
    public bool CanMove = true;
    public static PlayerController Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        MouseMovement();        
    }

    private void MouseMovement()
    {
        if (CanMove && !OptionsButtonSignal.Instance.IsIn &&!OptionsSignal.Instance.InOptions)
        {            
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                //Pega a posição do mouse
                _mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);

                //Compara a posição x entre o mouse e o jogador
                if (transform.position.x < _mousePos.x)
                {
                    //Vira sprite para a direita
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                }
                else
                {
                    //Vira sprite para a esquerda
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }

                //Aciona a animação de correr
                _idleAnimation.SetActive(false);
                _walkingAnimation.SetActive(true);

                //Libera o update para fazer o jogador andar
                _isWalking = true;
            }
        }

        if (_isWalking)
        {
            //Faz o jogador andar até o local clicado
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(_mousePos.x, transform.position.y, transform.position.z), Time.deltaTime * _speed);
            if(transform.position.x <= -7.71f)
            {
                transform.position = new Vector3(-7.71f, transform.position.y, transform.position.z);
                StopWalking();
            }
            if(transform.position.x >= 26.95f)
            {
                transform.position = new Vector3(26.95f, transform.position.y, transform.position.z);
                StopWalking();
            }
            _walkingSoundTimer -= Time.deltaTime;

            if(_walkingSoundTimer <= 0)
            {
                _walkingSoundTimer = 1.4f;
                AudioPlayer.Instance.PlayWalking(_walkingSound);
            }

            if (Vector3.Distance(transform.position, new Vector3(_mousePos.x, transform.position.y, transform.position.z)) <= 0.01f)
            {
                StopWalking();
            }
        }
    }

    public void StopWalking()
    {
        //Aciona a animação de idle
        _idleAnimation.SetActive(true);
        _walkingAnimation.SetActive(false);

        //Faz o jogador parar de andar
        _isWalking = false;

        _walkingSoundTimer = 0;
        AudioPlayer.Instance.StopWalking();
    }
}
