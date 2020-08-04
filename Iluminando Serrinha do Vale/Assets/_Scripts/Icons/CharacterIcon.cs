using UnityEngine;
using GAF.Core;

public class CharacterIcon : MonoBehaviour
{
    [SerializeField] private GAFBakedMovieClip[] _normalTalk;
    [SerializeField] private GAFBakedMovieClip[] _importantTalk;

    private GAFBakedMovieClip[] _currentIcon;

    public void SetCurrentIcon(int iconType) //0 - normal | 1 - important
    {
        if(iconType == 0)
        {
            _currentIcon = _normalTalk;
            _importantTalk[0].gameObject.SetActive(false);
            _importantTalk[1].gameObject.SetActive(false);
            _normalTalk[0].gameObject.SetActive(true);
            _normalTalk[1].gameObject.SetActive(false);
        }
        else
        {
            _currentIcon = _importantTalk;
            _importantTalk[0].gameObject.SetActive(true);
            _importantTalk[1].gameObject.SetActive(false);
            _normalTalk[0].gameObject.SetActive(false);
            _normalTalk[1].gameObject.SetActive(false);
        }
    }

    public void EmptyIcon()
    {
        _currentIcon[0].gotoAndPlay(_currentIcon[1].currentFrameNumber);
        _currentIcon[1].gameObject.SetActive(false);
        _currentIcon[0].gameObject.SetActive(true);
    }

    public void FilledIcon()
    {       
        _currentIcon[1].gotoAndPlay(_currentIcon[0].currentFrameNumber);
        _currentIcon[0].gameObject.SetActive(false);
        _currentIcon[1].gameObject.SetActive(true);
    }
}
