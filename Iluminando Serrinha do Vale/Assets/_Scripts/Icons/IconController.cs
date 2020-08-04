using UnityEngine;

public class IconController : MonoBehaviour
{
    [SerializeField] private CharacterIcon[] _characterIcons;

    private void Start()
    {
        SetImportantCharacter(0);
    }

    public void SetImportantCharacter(int characterIndex) //0 - secretary | 1 - Procurador | 2 - Engi and Gestora
    {
        switch(characterIndex)
        {
            case 0:
                _characterIcons[0].SetCurrentIcon(1);
                _characterIcons[1].SetCurrentIcon(0);
                _characterIcons[2].SetCurrentIcon(0);
                _characterIcons[3].SetCurrentIcon(0);
                break;

            case 1:
                _characterIcons[0].SetCurrentIcon(0);
                _characterIcons[1].SetCurrentIcon(1);
                _characterIcons[2].SetCurrentIcon(0);
                _characterIcons[3].SetCurrentIcon(0);
                break;

            case 2:
                _characterIcons[0].SetCurrentIcon(0);
                _characterIcons[1].SetCurrentIcon(0);
                _characterIcons[2].SetCurrentIcon(1);
                _characterIcons[3].SetCurrentIcon(1);
                break;
        }
    }
}